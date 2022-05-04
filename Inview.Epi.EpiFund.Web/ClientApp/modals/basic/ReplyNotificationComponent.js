import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'
import { LoadingComponent } from '../../loading-component'
import { MessageReplyMessageComponent } from './message-reply-message-component'

export class ReplyMessageNotificationComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
        this.state = {
            msg: $.extend({}, this.props.msg),
        }

        this.viewMessage = this.viewMessage.bind(this);
    }

    viewMessage() {
        const self = this;
        PubSub.publish(this.state.msg.MessageId, this.state.msg);
    }


    render() {
        return (
            <React.Fragment>
                {
                    //Have to do this to keep the components in the same "publish" context
                    <MessageReplyMessageComponent fromNotification={true} msg={this.props.msg} />
                }
                <p>New Message from: {this.state.msg.FromFullname}<b></b></p><hr />
                <button onClick={this.viewMessage} className="btn btn info view-message-btn"><i className="fas fa-envelope"></i> View Message</button>
            </React.Fragment>
        )
    }

}
