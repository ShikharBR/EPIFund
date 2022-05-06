import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const PubSub = require('pubsub-js');

import { notificationService } from './services/notificationService'

export class IntraSiteCommunicationComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
        this.state = {
            epiHub: null
        };

        this.receiveMessage = this.receiveMessage.bind(this);
        this.newFilesUploaded = this.newFilesUploaded.bind(this);
    }

    componentDidMount() {
        var self = this;
        $(function () {

            if ($.connection != null) {
                self.setState({
                    epiHub: $.connection.ePIMessageHub
                }, () => {
                    //set your signalr client functions here
                    self.state.epiHub.client.receiveMessage = self.receiveMessage;
                    self.state.epiHub.client.newFilesUploaded = self.newFilesUploaded;
                    //init the hub connection
                    $.connection.hub.start().done(function () {
                        console.log("Welcome! This client is now connected.");
                    });

                    //disconnected (server is down)
                    $.connection.hub.disconnected(function () {
                        console.log('Warning! This client is no longer connected.');
                    });

                    //error from server
                    $.connection.hub.error(function (error) {
                        console.log('SignalR error: ' + error)
                    });
                });
            } else {
                console.log('SignalR error: Connection hub not found. Check scripts order?');
            }
        });
    }

    newFilesUploaded(files) {
        PubSub.publish("newFilesUploaded", files);
    }

    receiveMessage(msg) {
        console.log("receiveMessage");
        //reload dashboard
        PubSub.publish("updateMessages");

        //show notification
        //need to watch out for the msg type, it could be a notification
        if (msg.MessageType === 2) {
            notificationService.showNewNotificationAlert(msg);
        } else {
            notificationService.showNewMessageAlert(msg);
        }
    }

    render() {
        return (
            <React.Fragment>

            </React.Fragment>
        );
    }
}