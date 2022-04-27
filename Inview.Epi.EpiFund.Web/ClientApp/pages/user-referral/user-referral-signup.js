import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { helpers } from '../../helpers/helpers'
import { notificationService } from '../../services/notificationService'

import { LoadingComponent } from '../../loading-component'

export class UserReferralSignupComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,
            referral: {},
            referralNotFound: false,
            url: "",
            canGoUrl: false
        };

        this.updateUrl = this.updateUrl.bind(this);
    }

    componentDidMount() {
        const self = this;

        var code = epiService.getParameterByName("referralCode");
        if (code == null) {
            self.setState({
                referralNotFound: true,
                isLoading: false
            });
        } else {
            epiService.getReferralByCode(code).then(function (r) {

                if (r != null) {
                    self.setState({
                        referralNotFound: false,
                        referral: r,
                        isLoading: false
                    });
                }

            }).fail(function () {
                self.setState({
                    referralNotFound: true,
                    isLoading: false
                });
            });;
        }
    }

    updateUrl(userType) {
        const self = this;
        var url = "";
        if (self.state.referral.FromUserFullName != "") {
            if (userType == "sp") {
                url = "/ServiceProviders/registration?referralCode=" + self.state.referral.ReferralCode
            }
            else if (userType == "pi") {
                url = "/Home/RegistrationWithReferralCode?referralCode=" + self.state.referral.ReferralCode
            }

            self.setState({ url: url, canGoUrl: true });
        }
    }

    render() {

        return (
            <React.Fragment>
                {
                    this.state.isLoading ?
                        <LoadingComponent /> :
                        <div>
                            <div>
                                <h3>Welcome to USCREonline! </h3>
                                <div className="panel">
                                    <div className="panel-body">
                                        {
                                            this.state.referralNotFound &&
                                            <div className="col-sm-12">
                                                <div className="alert alert-danger" role="alert"> <strong>Oh snap!</strong> The referral link is invalid or expired.</div>
                                            </div>
                                        }

                                        {
                                            (this.state.referralNotFound === false && this.state.referral != null) &&
                                            <div className="col-sm-12">
                                                <h3>{this.state.referral.FromUserFullName} has invited you to join USCREonline!</h3>
                                                <p>Please choose your user type:</p>


                                                <div className="funkyradio">
                                                    <div className="funkyradio-primary">
                                                        <input type="radio" name="radio" id="radio2" value="pi" onChange={() => { this.updateUrl('pi') }} />
                                                        <label htmlFor="radio2">Principal Investor</label>
                                                    </div>
                                                    <div className="funkyradio-primary">
                                                        <input type="radio" name="radio" id="radio1" value="sp" onChange={() => { this.updateUrl('sp') }} />
                                                        <label htmlFor="radio1">Service Provider</label>
                                                    </div>
                                                </div>
                                                <hr />
                                                <a disabled={!this.state.canGoUrl} href={this.state.url} className="btn btn-primary">Next <i className="fas fa-arrow-right"></i></a>
                                            </div>
                                        }
                                    </div>
                                </div>
                            </div>
                        </div>
                }
            </React.Fragment>
        )
    }
}

const element = document.getElementById("user-referral-signup-app");
if (element != null)
    ReactDOM.render(<UserReferralSignupComponent />, element);
