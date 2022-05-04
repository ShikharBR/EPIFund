import React, { Component } from "react"
import ReactDOM from 'react-dom';

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'

import { LoadingComponent } from '../../loading-component'

export class QuickViewComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,
            selectedUserProfile: this.props.selectedUserProfile
        };

        this.refresh();

        this.openShareModal = this.openShareModal.bind(this);
        this.sendMessage = this.sendMessage.bind(this);
        this.saveToMyContacts = this.saveToMyContacts.bind(this);
        this.removeFromMyContacts = this.removeFromMyContacts.bind(this);
        this.openNewPreferredSPModal = this.openNewPreferredSPModal.bind(this);
    }

    async refresh() {
        const self = this;
        const profile = await epiService.getCurrentUserInfoAndProfile(true);

        //check if current user is in contact
        const foundContact = profile.Contacts.find((c, i) => {
            if (c.UserId === this.state.selectedUserProfile.UserId) {
                return c;
            }
        });

        this.setState({
            currentUserProfile: profile,
            isLoading: false,
            isInContact: foundContact != null,
            ViewFromNewPreferredModal: (this.props.selectedUserProfile.ViewFromNewPreferredModal || false)
        }, () => {
        });

    }

    sendMessage() {
        if (this.props.parentModal != null) {
            this.props.parentModal.closeModal();
        }

        PubSub.publish("newMessageToUser", this.state.selectedUserProfile);
    }

    openNewPreferredSPModal() {
        const self = this;
        if (this.props.parentModal != null) {
            this.props.parentModal.closeModal();
        }

        PubSub.publish("proceedWithSelectedSP", [this.state.selectedUserProfile]);
    }

    openShareModal() {
        if (this.props.parentModal != null) {
            this.props.parentModal.closeModal();
        }

        PubSub.publish("shareFilesWithUser", this.state.selectedUserProfile);
    }

    saveToMyContacts() {
        const self = this;

        self.setState({
            isLoading: true
        });

        setTimeout(() => {
            var ownerUserId = this.state.currentUserProfile.UserId;
            var savedUserId = this.state.selectedUserProfile.UserId;

            epiService.saveContact(ownerUserId, savedUserId).then(function (response) {
                self.state.currentUserProfile.Contacts.push({ UserId: savedUserId });

                self.setState({
                    isInContact: true,
                    isLoading: false,
                }, () => {
                    notificationService.info("Added contact");
                });

            });
        }, 500);

    }

    removeFromMyContacts() {
        this.setState({
            isLoading: true
        });

        setTimeout(() => {
            epiService.removeContact(this.state.currentUserProfile.UserId, this.state.selectedUserProfile.UserId).then(() => {
                this.setState({
                    isInContact: false,
                    isLoading: false
                }, () => {
                    notificationService.info("Removed contact");
                    if (this.props.refreshList != null)
                        this.props.refreshList();
                });
            });
        }, 500);
    }

    render() {

        return (
            <React.Fragment>
                {
                    this.state.isLoading ?
                        <LoadingComponent />
                        :
                        <React.Fragment>
                            <div>
                                <div className="row">
                                    <div className="col-sm-4">
                                        <div className="profile-image">
                                            <img src={this.state.selectedUserProfile.ProfileImageUrl} alt="" className="img-thumbnail" />
                                        </div>
                                    </div>
                                    <div className="col-sm-8">
                                        <table className="table user-profile-table">
                                            <tbody>
                                                <tr>
                                                    <td colSpan="2"><h3>{this.state.selectedUserProfile.User.FullName}</h3></td>
                                                </tr>
                                                <tr>
                                                    <td>Position:</td>
                                                    <td><h4>{this.state.selectedUserProfile.JobTitle}</h4></td>
                                                </tr>
                                                <tr>
                                                    <td>Phone #:</td>
                                                    <td><h4>{this.state.selectedUserProfile.Phone}</h4></td>
                                                </tr>
                                                <tr>
                                                    <td>USCRE User Type:</td>
                                                    <td>
                                                        <h4>{this.state.selectedUserProfile.UserTypeDescription}</h4>
                                                    </td>
                                                </tr>
                                                {
                                                    this.state.selectedUserProfile.User.UserType == 14 &&
                                                    <tr>
                                                        <td>Service Provider Type / Industry:</td>
                                                        <td>
                                                            <h4>{this.state.selectedUserProfile.ServiceProviderTypeDescription}</h4>
                                                        </td>
                                                    </tr>
                                                }

                                            </tbody>
                                        </table>
                                    </div>

                                    <div className="col-sm-12">
                                        <button className="btn btn-primary margin-right10px" onClick={this.sendMessage}>
                                            <i className="fas fa-envelope"></i> Message</button>
                                        <button className="btn btn-info margin-right10px" onClick={this.openShareModal}>
                                            <i className="fas fa-folder"></i> Share folders</button>

                                        {
                                            (this.state.ViewFromNewPreferredModal === false) ?
                                                this.state.currentUserProfile.User.UserType === 5 &&
                                                <button className="btn btn-info" onClick={this.openNewPreferredSPModal}>
                                                    <i className="fas fa-star"></i> Set as preferred</button>
                                                : null
                                        }


                                        <br />
                                        <br />

                                        {
                                            !this.state.isInContact ?
                                                <button className="btn btn-success margin-right10px" onClick={this.saveToMyContacts}>
                                                    <i className="fas fa-user-plus"></i> Save to My Contacts</button>
                                                :
                                                <button className="btn btn-danger margin-right10px" onClick={this.removeFromMyContacts}>
                                                    <i className="fas fa-user-slash"></i> Remove contact</button>
                                        }
                                    </div>
                                </div>
                                <div className="row hidden">
                                    <div className="col-sm-12">

                                        <h5>Recent Messages</h5>
                                        <table className="table table-bordered">
                                            <tbody>
                                                <tr><td>Message # 1</td></tr>
                                                <tr><td>Message # 2</td></tr>
                                                <tr><td>Message # 3</td></tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>

                            </div>
                        </React.Fragment>
                }
            </React.Fragment>
        )
    }
}
