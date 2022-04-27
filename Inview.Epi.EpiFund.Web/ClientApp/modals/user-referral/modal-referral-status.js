import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { helpers } from '../../helpers/helpers'
import { notificationService } from '../../services/notificationService'
import { LoadingComponent } from '../../loading-component'

export class ReferralStatusModalComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
        this.state = {
            isOpen: false,
            referral: {},
            isLoading: false
        }

        this.openModal = this.openModal.bind(this);
        this.afterOpenModal = this.afterOpenModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.resendEmail = this.resendEmail.bind(this);
        this.viewReferredUser = this.viewReferredUser.bind(this);

        ///////////////events/////////////////////////////////////

        PubSub.subscribe("openReferralInfoModal", (type, referral) => {
            this.setState({
                isOpen: true,
                referral: referral
            });
        });
    }

    componentDidMount() {
        this.refresh();
    }

    async refresh() {
        const profile = await epiService.getCurrentUserInfoAndProfile(true);
        this.setState({
            currentUserProfile: profile
        })
    }

    openModal() {
    }

    afterOpenModal() {

    }

    closeModal() {
        this.setState({ isOpen: false });
    }

    onChange() {

    }

    resendEmail() {
        var self = this;
        self.setState({ isLoading: true });

        notificationService.info("Resending invitation...");
        epiService.resendInvitation(self.state.referral, window.location.origin).then(function (r) {
            self.closeModal();
            PubSub.publish("refreshReferralTrackingPage");
        });
    }

    async viewReferredUser() {
        var self = this;
        const user = await epiService.getUserInfoAndProfileByUserId(self.state.referral.ReferredUserId);
        self.closeModal();
        PubSub.publish("quickViewUser", user);
    }

    formatDate(dateTime) {
        return helpers.formatDateTime(dateTime);
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
                            {
                                (this.state.isLoading && this.state.referral != null) ?
                                    <LoadingComponent /> :
                                    <React.Fragment>
                                        <div className="modal-header">
                                            <h3>
                                                Referral status:
                                                {
                                                    this.state.referral.ReferralStatus == 0 &&
                                                    <span className="status">
                                                        <label className="label label-default">Created</label>
                                                    </span>
                                                }

                                                {
                                                    this.state.referral.ReferralStatus == 1 &&
                                                    <span className="status">
                                                        <label className="label label-info">Invitation Sent</label>
                                                    </span>
                                                }

                                                {
                                                    (this.state.referral.ReferralStatus == 2 || this.state.referral.ReferralStatus == 3) &&
                                                    <span className="status">
                                                        <label className="label label-success">Registered</label>
                                                    </span>
                                                }

                                            </h3>
                                        </div>

                                        <div className="modal-body modal-body-auto-height">
                                            <div>
                                                {
                                                    (this.state.referral.ReferralStatus == 0 || this.state.referral.ReferralStatus == 1) &&
                                                    <div className="status">
                                                        <div className="form-group">
                                                            <label>Referral Email:</label>
                                                            <p>{this.state.referral.ReferralEmail}</p>
                                                        </div>
                                                        <br />
                                                        <div className="form-group">
                                                            <label>Last Updated Time:</label>
                                                            <p>{this.formatDate(this.state.referral.LastUpdateTime)}</p>
                                                        </div>
                                                    </div>
                                                }
                                                {
                                                    (this.state.referral.ReferralStatus == 2 || this.state.referral.ReferralStatus == 3) &&
                                                    <div className="status">
                                                        <p>Great! <b>{this.state.referral.ReferredUserFullName}</b> signed up using your referral link!</p>
                                                        <hr />
                                                        <div className="form-group">
                                                            <label>Referral Email:</label>
                                                            <p>{this.state.referral.ReferredUsername}</p>
                                                        </div>
                                                        <br />
                                                        <div className="form-group">
                                                            <label>Last Updated Time:</label>
                                                            <p>{this.formatDate(this.state.referral.LastUpdateTime)}</p>
                                                        </div>
                                                    </div>
                                                }
                                            </div>
                                        </div>
                                        <div className="modal-footer">
                                            {
                                                (this.state.isLoading == false && (this.state.referral.ReferralStatus == 0 || this.state.referral.ReferralStatus == 1)) &&
                                                <button onClick={this.resendEmail} className="btn btn-primary">Resend invitation</button>
                                            }

                                            {
                                                (this.state.isLoading == false && (this.state.referral.ReferralStatus == 2 || this.state.referral.ReferralStatus == 3)) &&
                                                <button onClick={this.viewReferredUser} className="btn btn-primary">View referred user's profile</button>
                                            }

                                            <button className="btn modal-default-button" onClick={this.closeModal}>Cancel</button>
                                        </div>
                                    </React.Fragment>
                            }
                        </div>

                    </Modal>
                }
            </React.Fragment>
        )
    }

}


