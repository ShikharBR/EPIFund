import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const _ = require('lodash');
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'
import { helpers } from '../../helpers/helpers'
import { LoadingComponent } from '../../loading-component'

export class UserReferralModalComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
        this.state = {
            isOpen: false,

            referrals: [],
            copiedLink: false,
            isLoading: true,
            referralCode: "",

            email: "",
            orderedEmails: [],
            validEmails: [],
            errors: [],

            selectedDownloadEmailTemplate: null,
            selectedEmailTemplate: null,
            emailTemplates: [],
            downloadTemplateUrl: ""
        }

        this.openModal = this.openModal.bind(this);
        this.afterOpenModal = this.afterOpenModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.copyLink = this.copyLink.bind(this);
        this.previewTemplate = this.previewTemplate.bind(this);
        this.addToEmail = this.addToEmail.bind(this);
        this.sendEmails = this.sendEmails.bind(this);

        ///////////////events/////////////////////////////////////

        PubSub.subscribe("openReferralModal", (type, data) => {
            this.setState({
                isOpen: true,
            });
        });
    }

    componentDidMount() {
        this.refresh();
    }

    copyLink() {
        var self = this;

        const el = document.createElement('textarea');
        el.value = self.state.referralCode;
        document.body.appendChild(el);
        el.select();
        document.execCommand('copy');
        document.body.removeChild(el);

        self.setState({ copiedLink: true });
    }

    async refresh() {
        const self = this;

        const [profile, referrals, code, templates] = await Promise.all([
            epiService.getCurrentUserInfoAndProfile(true),
            epiService.getUserReferrals(),
            epiService.getUserQuickReferralLink(),
            epiService.getEmailTemplates()
        ]);
        const templateArr = [];
        $.each(templates, function (i, e) {
            templateArr.push({
                name: e.Name.replace(".pdf", ""),
                url: e.Url
            });
        });

        this.setState({
            referrals: referrals,
            referralCode: window.location.origin + "/Referral/SignUp?referralCode=" + code.ReferralCode,
            currentUserProfile: profile,
            emailTemplates: templateArr,
            selectedEmailTemplate: templateArr[0]?.name,
            selectedDownloadEmailTemplate: templateArr[0]?.name,
            isLoading: false
        })
    }

    previewTemplate() {
        var self = this;

        if (self.state.selectedEmailTemplate != null) {
            var fe = self.state.emailTemplates.filter(function (e, i) { if (e.name == self.state.selectedEmailTemplate) return true; })[0];
            window.open(fe.url, '_blank');
        }
    }

    openModal() {

    }

    afterOpenModal() {
        this.setState({
            isLoading: false,
            validEmails: [],
            orderedEmails: [],
            email: ""
        });
    }

    closeModal() {
        this.setState({
            isOpen: false,
            copiedLink: false,
            selectedEmailTemplate: this.state.emailTemplates[0].name,
            selectedDownloadEmailTemplate: this.state.emailTemplates[0].name,
        });
    }

    onChange() {
        console.log("changed");
    }

    addToEmail() {
        var self = this;

        var input = self.state.email.trim();
        var hasError = false;
        self.setState({ errors: [] }, () => {
            if (input == "") {
                return;
            }

            //clean all input, separate ";"
            var emails = input.split(";");
            for (var i = 0; i < emails.length; i++) {
                var currentEmail = emails[i].trim();
                if (currentEmail == '')
                    continue;

                if (helpers.validateEmail(currentEmail) == false) {
                    self.state.errors.push({ msg: currentEmail + " is not valid." });
                    self.setState({
                        errors: self.state.errors
                    });
                    hasError = true;
                } else {
                    //current email is valid but we'll need to 
                    //see if this email already in user referrals
                    var found = self.state.referrals.filter(function (r) {
                        if (r.ReferralEmail == currentEmail)
                            return true;
                        return false;
                    })[0];

                    if (found != null) {
                        self.state.errors.push({ msg: currentEmail + " invitation is already sent." });
                        hasError = true;
                    } else {
                        if (self.state.validEmails.indexOf(currentEmail) == -1) {
                            self.state.validEmails.push(currentEmail);
                        }
                        else {

                            self.state.errors.push({ msg: currentEmail + " is already added." });
                            hasError = true;
                        }
                    }
                }
            }

            self.setState({
                orderedEmails: _.orderBy(self.state.validEmails)
            });

            if (hasError == false) {
                self.setState({
                    email: "",
                    errors: []
                });
            }
        });
    }

    sendEmails() {
        var self = this;
        self.setState({
            isLoading: true
        });

        notificationService.info("Sending invitation(s)...");
        var emailAddresses = self.state.validEmails.join(";");
        epiService.sendReferralEmails(emailAddresses, window.location.origin, self.state.selectedEmailTemplate).then(function (r) {
            //Tuple<List<UserReferral>, List<string>> 

            //sent invites
            if (r.Item1.length > 0) {
                notificationService.info("Invitation(s) sent.");

                PubSub.publish("refreshReferralTrackingPage");
            }

            //failed messages
            if (r.Item2.length > 0) {

                var list = "<ul style='list-style-position: outside'>";

                $.each(r.Item2, function (i, e) {
                    list += ("<li>" + e + "<hr style='margin:10px 0px;'/></li>");
                });

                list += "</ul>";

                notificationService.error(list);
            }

            self.closeModal();

        });

    }

    removeEmail(email) {
        const found = this.state.validEmails.indexOf(email);
        this.state.validEmails.splice(found, 1);

        this.setState({
            orderedEmails: _.orderBy(this.state.validEmails)
        });
    }

    render() {
        return (
            <React.Fragment>
                {
                    this.state.currentUserProfile != null &&
                    <Modal
                        isOpen={this.state.isOpen}
                        onAfterOpen={this.afterOpenModal}
                        onRequestClose={this.closeModal}
                        shouldCloseOnOverlayClick={true}
                        overlayClassName="modal-mask"
                        className="modal-wrapper"
                        closeTimeoutMS={200}>

                        <div className="modal-container">

                            <div className="modal-header">
                                <h3>New Referral</h3>
                            </div>

                            <div className="modal-body">

                                <div>
                                    {
                                        this.state.isLoading ?
                                            <LoadingComponent /> :
                                            <React.Fragment>
                                                <div>
                                                    <h4 className="text-center">Send invite through USCREonline</h4>

                                                    <div>
                                                        <div className="row">
                                                            <div className="col-sm-12">
                                                                <div className="input-group">
                                                                    <input type="text" className="form-control" placeholder="" value={this.state.email} onChange={(e) => { this.setState({ email: e.target.value }) }} onKeyPress={(e) => {
                                                                        if (e.key === "Enter") { this.addToEmail() }
                                                                    }} />
                                                                    <span className="input-group-btn">
                                                                        <button onClick={this.addToEmail} className="btn btn-primary" type="button"><i className="fas fa-plus"></i> Add to email list</button>
                                                                    </span>
                                                                </div>
                                                                <i style={{ fontWeight: 'bold' }}><small>*Enter email then hit enter or click add to email list button</small></i><br />
                                                                <i style={{ fontWeight: 'bold' }}><small>**Use ";" to separate email addresses</small></i>
                                                                {
                                                                    this.state.errors.length > 0 &&
                                                                    <div className="alert alert-warning" style={{ marginBottom: '0px' }}>
                                                                        <ul>
                                                                            {
                                                                                this.state.errors.map((err, i) => {
                                                                                    return (
                                                                                        <li key={i}>{err.msg}</li>
                                                                                    )
                                                                                })
                                                                            }
                                                                        </ul>
                                                                    </div>
                                                                }

                                                            </div>
                                                        </div>
                                                        <div className="row">
                                                            <div className="col-sm-12">
                                                                <div className="epi-chips">

                                                                    {
                                                                        this.state.orderedEmails.map((e, i) => {
                                                                            return (
                                                                                <div key={i} className="btn-group">
                                                                                    <button type="button" className="btn btn-primary">{e}</button>
                                                                                    <button type="button" className="btn" onClick={() => { this.removeEmail(e) }}>X</button>
                                                                                </div>
                                                                            )
                                                                        })
                                                                    }

                                                                </div>
                                                            </div>

                                                            <label>Current Email Template:</label>
                                                            <select value={this.state.selectedEmailTemplate} onChange={(e) => { this.setState({ selectedEmailTemplate: e.target.value }) }}>
                                                                {
                                                                    this.state.emailTemplates.map((t, i) => {
                                                                        return (
                                                                            <option key={i} value={t.name}>{t.name}</option>
                                                                        )
                                                                    })
                                                                }
                                                            </select>

                                                            <button className="btn btn-info" onClick={this.previewTemplate}>Preview</button>
                                                            <button onClick={this.sendEmails} disabled={this.state.validEmails.length === 0} className="btn btn-success pull-right" type="button"><i className="far fa-envelope"></i> Send</button>

                                                        </div>

                                                        <hr />
                                                        <div className="row">
                                                            <h4 className="text-center">More ways to invite</h4>
                                                            <div className="col-sm-12">
                                                                <div className="input-group" onClick={this.copyLink}>
                                                                    <input type="text" className="form-control" value={this.state.referralCode} disabled style={{ cursor: 'pointer' }} />
                                                                    <span className="input-group-btn">
                                                                        <button className="btn btn-primary" type="button"><i className="fas fa-link"></i> Copy Link</button>
                                                                    </span>
                                                                </div>

                                                                {
                                                                    this.state.copiedLink &&
                                                                    <div className="alert alert-success" style={{ marginBottom: '0px' }}>Copied link!</div>
                                                                }

                                                            </div>
                                                        </div>

                                                        <div className="row">
                                                            <h4 className="text-center">Email Template</h4>
                                                            <div className="col-sm-12">
                                                                <select value={this.state.selectedDownloadEmailTemplate} className="form-control" onChange={(e) => { this.setState({ selectedDownloadEmailTemplate: e.target.value }) }}>
                                                                    {
                                                                        this.state.emailTemplates.map((t, i) => {
                                                                            return (
                                                                                <option key={i} value={t.name}>{t.name}</option>
                                                                            )
                                                                        })
                                                                    }
                                                                </select>
                                                                <a href={window.location.origin + "/Referral/GetEmailTemplate?templateName=" + this.state.selectedDownloadEmailTemplate} target="_blank" className="btn btn-primary btn-block"><i className="fas fa-mail-bulk"></i> Download Outlook Email Template</a>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </React.Fragment>
                                    }

                                </div>
                            </div>

                            <div className="modal-footer">
                                <button className="btn pull-right" onClick={this.closeModal}>Cancel</button>
                            </div>
                        </div>

                    </Modal>
                }
            </React.Fragment>
        )
    }
}


