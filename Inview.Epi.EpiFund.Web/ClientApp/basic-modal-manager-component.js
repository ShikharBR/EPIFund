import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const PubSub = require('pubsub-js');


import { MessageNewMessageComponent } from './modals/basic/message-new-message-component'
import { ReplyMessageNotificationComponent } from './modals/basic/ReplyNotificationComponent'
import { MessageReplyMessageComponent } from './modals/basic/message-reply-message-component'
import { ModalQuickViewUserProfileComponent } from './modals/basic/modal-quick-view-user-profile-component'
import { NewPreferredServiceProviderModal } from './modals/preferred-service-provider/modal-new-preferred-service-provider'
import { ReferralStatusModalComponent } from './modals/user-referral/modal-referral-status'
import { UserReferralModalComponent } from './modals/user-referral/modal-user-referral'
import { ModalFileShareWithUserComponent } from './modals/file-system/modal-file-share-with-user.component'
import { IntraSiteCommunicationComponent } from "./intra-site-communication-component";

export class ModalManagerComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
    }

    render() {
        return (
            <React.Fragment>
                <IntraSiteCommunicationComponent />

                <NewPreferredServiceProviderModal />
                <ReferralStatusModalComponent />
                <UserReferralModalComponent />
                <ModalFileShareWithUserComponent />
                <ModalQuickViewUserProfileComponent />
                <MessageReplyMessageComponent fromNotification={false} />
                <MessageNewMessageComponent />
            </React.Fragment>
        );
    }
}

////5/10/2019
//Create a wrapper if it doesn't exsit
let messageComponentWrapper = document.getElementById("message-component-wrapper");
if (messageComponentWrapper == null) {
    messageComponentWrapper = document.createElement("div");
    messageComponentWrapper.id = 'message-component-wrapper';
    document.body.appendChild(messageComponentWrapper);
}
Modal.setAppElement(messageComponentWrapper);
ReactDOM.render(<ModalManagerComponent />, document.getElementById('message-component-wrapper'))
