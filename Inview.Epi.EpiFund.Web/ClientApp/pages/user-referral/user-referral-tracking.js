import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { helpers } from '../../helpers/helpers'
import { notificationService } from '../../services/notificationService'

import { LoadingComponent } from '../../loading-component'

export class UserReferralTrackingComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,
            copiedLink: false,
            referrals: [],
            referralCode: "",
            referralTable: null
        };

        this.copyLink = this.copyLink.bind(this);
    }

    componentDidMount() {
        this.refresh();

        PubSub.subscribe("refreshReferralTrackingPage", (type, data) => {
            this.refresh();
        });
    }

    formatDate(dateTime) {
        return helpers.formatDateTime(dateTime);
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
        self.setState({ isLoading: true });

        const profile = await epiService.getCurrentUserInfoAndProfile(true);
        //get self referral link
        const quickReferral = await epiService.getUserQuickReferralLink();
        const code = window.location.origin + "/Referral/SignUp?referralCode=" + quickReferral.ReferralCode;

        this.setState({
            currentUserProfile: profile,
            referralCode: code
        });

        if (self.state.referralTable != null)
            self.state.referralTable.destroy();

        const referrals = await epiService.getUserReferrals();

        self.setState({ referrals: referrals });

        setTimeout(function () {
            //init the table
            self.state.referralTable = $('.referral-table').DataTable({
                ordering: false
            });

            self.setState({ isLoading: false });
        }, 1500);

        //bootstrap tooltip
        setTimeout(function () { $('[data-toggle="tooltip"]').tooltip(); }, 500);
    }

    render() {

        return (
            <React.Fragment>
                <div>

                    {
                        (this.state.isLoading && this.state.currentUserProfile == null) ?
                            <div>
                                <LoadingComponent />
                            </div> :

                            (this.state.currentUserProfile.User.UserType === 14 &&
                                this.state.currentUserProfile.IsServiceProviderSubmittedJVAgreement === false) ?
                                <div>
                                    <h3>Referral Tracking</h3>
                                    <div className="alert alert-info">Please sign up for the <a href="/Home/JointVentureMarketing">JOINT VENTURE MARKETING PROGRAM</a> before sending referrals.</div>
                                </div>
                                :
                                <div>
                                    <button onClick={() => { PubSub.publish("openReferralModal") }} className="btn btn-primary pull-right"><i className="far fa-share-square"></i> Send a new invitation</button>
                                    <h3>Referral Tracking</h3>
                                    <div className="alert alert-success">
                                        Share you referral link:
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

                                    <div className="panel">
                                        <div className="panel-body">
                                            <div className="col-sm-12">
                                                <h4 className="sp-h4">Referrals</h4><hr />

                                                {
                                                    this.state.isLoading &&
                                                    <LoadingComponent />
                                                }

                                                <div className={(this.state.isLoading ? 'hidden' : null)}>
                                                    <table className="table table-bordered referral-table">
                                                        <thead>
                                                            <tr>
                                                                <th>Type</th>
                                                                <th>Name / Email</th>
                                                                <th>Last Updated Time</th>
                                                                <th>Status</th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            {
                                                                this.state.referrals.map((referral, i) => {
                                                                    return (
                                                                        <tr key={i}>
                                                                            <td>
                                                                                {
                                                                                    referral.ReferralType === 1 &&
                                                                                    <p>Quick Link</p>
                                                                                }
                                                                                {
                                                                                    referral.ReferralType === 2 &&
                                                                                    <p>Custom Invitation</p>
                                                                                }
                                                                            </td>
                                                                            <td>
                                                                                {
                                                                                    referral.ReferredUserId == null ?
                                                                                        <p>
                                                                                            {referral.ReferralEmail}
                                                                                        </p> :
                                                                                        <p>
                                                                                            {referral.ReferredUserFullName} <br /> <b>{referral.ReferredUsername}</b>
                                                                                        </p>
                                                                                }
                                                                            </td>
                                                                            <td>{this.formatDate(referral.LastUpdateTime)}</td>
                                                                            <td>

                                                                                {
                                                                                    referral.ReferralStatus === 0 &&
                                                                                    <p className="status">
                                                                                        <label className="label label-default">Created</label>
                                                                                    </p>
                                                                                }

                                                                                {
                                                                                    referral.ReferralStatus === 1 &&
                                                                                    <p className="status">
                                                                                        <label className="label label-info">Invitation Sent</label>
                                                                                    </p>
                                                                                }

                                                                                {
                                                                                    (referral.ReferralStatus === 2 || referral.ReferralStatus == 3) &&
                                                                                    <p className="status">
                                                                                        <label className="label label-success">Registered</label>
                                                                                    </p>
                                                                                }
                                                                            </td>
                                                                            <td>
                                                                                <button className="btn btn-primary" onClick={() => { PubSub.publish('openReferralInfoModal', referral) }}><i className="fas fa-info-circle"></i></button>
                                                                            </td>
                                                                        </tr>
                                                                    )
                                                                })

                                                            }
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                    }


                </div>
            </React.Fragment>
        )
    }
}

const element = document.getElementById("user-referral-tracking-app");
if (element != null)
    ReactDOM.render(<UserReferralTrackingComponent />, element);
