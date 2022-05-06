import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'
import { LoadingComponent } from '../../loading-component'

export class MessageReplyMessageComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
        this.state = {
            isOpen: false,
            isReply: false,
            canReply: true,
            showMessageAlert: false,
            sendFromUser: null,
            originalMessage: {},
            message: {
                Subject: "",
                Body: "",
            },
            contacts: [],
            recipient: null
        }

        this.afterOpenModal = this.afterOpenModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.send = this.send.bind(this);
        this.messageSent = this.messageSent.bind(this);
        this.viewSenderProfile = this.viewSenderProfile.bind(this);
    }

    componentDidMount() {
        this.refresh();

        ///////////////events/////////////////////////////////////
        let topic = "viewMessage";
        if (this.props.fromNotification) {
            topic = this.props.msg.MessageId;
        }

        PubSub.subscribe(topic, (type, msg) => {

            let canReply = true;
            //you can't reply notification
            if (msg.MessageType === 2) {
                canReply = false;
            }

            epiService.getUserInfoAndProfileByUserId(msg.FromUserId).then((sendFromUser) => {
                this.setState({
                    isOpen: true,
                    isReply: false,
                    canReply: canReply,
                    showMessageAlert: false,
                    originalMessage: $.extend({}, msg),
                    message: msg,
                    sendFromUser: sendFromUser
                }, () => {
                    //Chi: Add "RE:" to subject line if this is the first reply
                    if (this.state.originalMessage.Subject.indexOf("RE:") != 0)
                        this.state.message.Subject = "RE: " + this.state.originalMessage.Subject;
                    this.state.message.Body = "";
                });
            });

        });
    }

    async refresh() {
        const profile = await epiService.getCurrentUserInfoAndProfile(true);
        this.setState({
            currentUserProfile: profile
        })
    }

    viewSenderProfile() {
        this.closeModal();
        PubSub.publish("quickViewUser", this.state.sendFromUser);
    }

    afterOpenModal() {

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

            PubSub.publish("updateMessages");
        }, 500);

    }

    send() {
        var self = this;
        self.setState({
            showMessageAlert: true
        });

        $.post("/message/sendMessage",
            JSON.stringify({
                "message": self.state.message,
                "messageRecipients": [{
                    "recipientUserId": self.state.originalMessage.FromUserId,
                    "recipientFullname": self.state.originalMessage.FromFullname,
                    "recipientUsername": self.state.originalMessage.FromUsername
                }]
            }), function (response) {
                setTimeout(function () {
                    //mark the old msg as read
                    epiService.markMessageAsRead(self.state.originalMessage);
                    self.messageSent();
                }, 1000);
            });
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

                            {
                                !this.state.isReply ?

                                    <div>
                                        {
                                            this.state.originalMessage.FromFullname != null &&
                                            <div className="modal-header">
                                                <h3>From: {this.state.originalMessage.FromFullname} &lt;{this.state.originalMessage.FromUsername}&gt;</h3>
                                            </div>
                                        }

                                        <div className="modal-body">
                                            <p>Subject: <b>{this.state.originalMessage.Subject}</b></p>
                                            <hr />
                                            <p><span dangerouslySetInnerHTML={{ __html: this.state.originalMessage.Body }}></span></p>
                                        </div>
                                    </div> :


                                    //reply content
                                    <div>
                                        <div className="modal-header">
                                            <h3>Reply message</h3>
                                        </div>
                                        <div className="modal-body">

                                            {
                                                this.state.showMessageAlert &&
                                                <div className="alert alert-success">Sending... Marking message as read.</div>
                                            }

                                            <div className="form-group">
                                                <label>Subject</label>
                                                <input className={'form-control ' + (this.state.isReply ? null : 'disabled')} disabled={this.state.isReply ? null : 'disabled'} placeholder="Subject" value={this.state.message.Subject} onChange={
                                                    (e) => {
                                                        this.state.message.Subject = e.target.value;
                                                        this.setState({ message: this.state.message });
                                                    }
                                                } />
                                            </div>
                                            <div>
                                                <label>Body</label>
                                                <textarea className="form-control" rows="10" value={this.state.message.Body} onChange={
                                                    (e) => {
                                                        this.state.message.Body = e.target.value;
                                                        this.setState({ message: this.state.message });
                                                    }
                                                }></textarea>
                                            </div>

                                            <div className="form-group">
                                                <hr />
                                                <button className="btn btn-primary pull-right" onClick={this.send}>Send</button>
                                            </div>

                                        </div>
                                    </div>
                            }

                            <div className="modal-footer">
                                {
                                    this.state.sendFromUser != null &&
                                    <button style={{ float: 'left' }} className="btn btn-primary" onClick={this.viewSenderProfile}>View {this.state.sendFromUser.FullName} Profile</button>
                                }
                                <button className={'btn btn-warning margin-right10px ' + (this.state.isReply ? ' hidden ' : null) + (!this.state.canReply ? ' hidden ' : null)} onClick={() => { this.setState({ isReply: true }) }}>
                                    Reply
                                </button>
                                <button className={'btn btn-warning margin-right10px ' + (!this.state.isReply ? 'hidden' : null)} onClick={() => { this.setState({ isReply: false }) }}>
                                    Cancel
                                </button>
                                <button className="btn pull-right" onClick={this.closeModal}>Close</button>
                            </div>
                        </div>

                    </Modal>
                }
            </React.Fragment>
        )
    }

}
