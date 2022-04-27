import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'
import { LoadingComponent } from '../../loading-component'

export class MessageNewMessageComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
        this.state = {
            isOpen: false,
            showMessageAlert: false,
            message: {
                Subject: "",
                Body: "",
            },
            contacts: [],
            sendToCorpAdmin: false,
            chooseFromContact: false,
            recipient: null
        }

        this.openModal = this.openModal.bind(this);
        this.afterOpenModal = this.afterOpenModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.send = this.send.bind(this);
        this.messageSent = this.messageSent.bind(this);

        ///////////////events/////////////////////////////////////

        PubSub.subscribe("newMessage", (type, data) => {
            const msg = {
                Subject: "",
                Body: "",
            };

            this.setState({
                isOpen: true,
                showMessageAlert: false,
                chooseFromContact: true,
                message: msg
            });
        });

        PubSub.subscribe("newMessageToUser", (type, user) => {
            const msg = {
                Subject: "",
                Body: "",

            };

            this.setState({
                isOpen: true,
                message: msg,
                showMessageAlert: false,
                recipient: user,
                chooseFromContact: false
            });
        });

        PubSub.subscribe("newMessageToCropAdmin", (type, data) => {
            const msg = data || {
                Subject: "",
                Body: "",
            };

            this.setState({
                isOpen: true,
                message: msg,
                showMessageAlert: false,
                sendToCorpAdmin: true,
                recipient: {
                    FullName: "Crop Admin",
                    Username: null
                },
                chooseFromContact: false
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
        //signalR here
    }

    afterOpenModal() {
        $('#new-message-dropdown').multiselect({
            enableFiltering: true,
            includeResetOption: true,
            includeResetDivider: true,
            numberDisplayed: 3,
            enableFullValueFiltering: false,
            enableCaseInsensitiveFiltering: true,
            buttonClass: 'btn btn-info',
            templates: {
                filter: '<li class="multiselect-item filter"><div class="input-group"><input class="form-control multiselect-search" type="text"></div></li>',
            }
        });
    }

    closeModal() {
        this.setState({ isOpen: false });
    }

    onChange() {

    }

    messageSent() {
        const self = this;
        setTimeout(function () {
            notificationService.info("Message sent");
            self.closeModal();
        }, 500);

    }

    send() {
        const self = this;

        if (self.state.message.Subject.trim() === "" || self.state.message.Body.trim() === "") {
            notificationService.warn(`Subject or body is empty`);
            return;
        }

        self.setState({ showMessageAlert: true });

        if (self.state.chooseFromContact === true) {

            const userIds = $($("#new-message-dropdown")[0]).val();
            if (userIds != null && userIds.length > 0) {

                //chi: build message recipients
                var messageRecipients = [];
                $.each(userIds, function (i, e) {
                    var foundContact = self.state.currentUserProfile.Contacts.find(function (x) { if (x.UserId == e) return true; });
                    if (foundContact != null) {
                        messageRecipients.push({
                            "recipientUserId": foundContact.UserId,
                            "recipientFullname": foundContact.FullName,
                            "recipientUsername": foundContact.Username
                        });
                    }
                });

                //now send
                $.post("/message/sendMessage",
                    JSON.stringify({
                        "message": {
                            "subject": self.state.message.Subject,
                            "body": self.state.message.Body
                        },
                        "messageRecipients": messageRecipients
                    }), function (response) {
                        self.messageSent();
                    });

            } else {
                self.setState({ showMessageAlert: false });
                notificationService.error("Please select a user.");
            }

        } else {
            //can send to crop admin
            if (self.state.sendToCorpAdmin) {
                $.post("/message/SendMessageToCorpAdmins", JSON.stringify({
                    "message": {
                        "subject": self.state.message.Subject,
                        "body": self.state.message.Body
                    },
                    "messageRecipients": [] //<-- to all crop admins for now
                }), function (response) {
                    self.messageSent();
                });
            } else {
                $.post("/message/sendMessage",
                    JSON.stringify({
                        "message": {
                            "subject": self.state.message.Subject,
                            "body": self.state.message.Body
                        },
                        "messageRecipients": [{
                            "recipientUserId": self.state.recipient.UserId,
                            "recipientFullname": self.state.recipient.FullName,
                            "recipientUsername": self.state.recipient.Username
                        }]
                    }), function (response) {
                        self.messageSent();
                    });
            }
        }
    }

    render() {
        return (
            <React.Fragment>
                {this.state.currentUserProfile != null &&
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
                                <h3>New Message</h3>
                            </div>

                            <div className="modal-body">

                                {
                                    this.state.showMessageAlert &&
                                    <div className="alert alert-success">Sending... </div>
                                }


                                {
                                    (this.state.chooseFromContact !== true && this.state.recipient != null) ?
                                        <div className="form-group">
                                            <p>To: <b>{this.state.recipient.FullName}</b> &nbsp;
                                                    {
                                                    this.state.recipient.Username != null &&
                                                    <span>&lt;{this.state.recipient.Username}&gt;</span>
                                                }
                                            </p>
                                        </div>
                                        :
                                        <div className="form-group" >
                                            <div className="row">
                                                <div className="col-sm-1"><p style={{ padding: '5px 0px 0px 5px' }}>To:</p></div>
                                                <div className="col-sm-11">
                                                    <select id="new-message-dropdown" multiple="multiple">
                                                        {
                                                            this.state.currentUserProfile.Contacts.map(c => {
                                                                return (
                                                                    <option key={c.UserId} value={c.UserId}>{c.FullName}</option>
                                                                )
                                                            })
                                                        }
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                }





                                <div className="form-group">
                                    <label>Subject</label>
                                    <input className="form-control" placeholder="Subject" value={this.state.message.Subject || ""} onChange={(e) => {
                                        this.state.message.Subject = e.target.value;
                                        this.setState({
                                            message: this.state.message
                                        })
                                    }} />
                                </div>
                                <div>
                                    <label>Body</label>
                                    <textarea className="form-control" rows="10" value={this.state.message.Body} onChange={(e) => {
                                        this.state.message.Body = e.target.value;
                                        this.setState({
                                            message: this.state.message
                                        })
                                    }} ></textarea>
                                </div>

                            </div>

                            <div className="modal-footer">
                                <button className="btn pull-right" onClick={this.closeModal}>
                                    Cancel
                                        </button>

                                <button className="btn btn-primary pull-right margin-right10px" onClick={this.send}>
                                    Send Message
                                        </button>
                            </div>
                        </div>

                    </Modal>
                }
            </React.Fragment>
        )
    }

}


