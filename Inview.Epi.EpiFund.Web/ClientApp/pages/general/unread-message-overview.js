import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'
import { helpers } from '../../helpers/helpers'
import { LoadingComponent } from '../../loading-component'

export class UnreadMessageOverviewComponent extends Component {
    constructor(props) {
        super(props);

        const self = this;

        this.state = {
            isLoading: true,
            recentMessages: []
        };

        this.formatDate = this.formatDate.bind(this);
        this.viewMessage = this.viewMessage.bind(this);
        this.markAsRead = this.markAsRead.bind(this);
        this.refresh = this.refresh.bind(this);

        PubSub.subscribe("updateMessages", (type, data) => {
            this.refresh();
        });
    }

    componentDidMount() {
        this.refresh();
    }

    async refresh() {
        const self = this;

        self.setState({
            isLoading: true,
        });

        //destory it
        if (helpers.isDataTable($('.message-table')) === true) {
            self.state.messageTable.destroy();
        }

        const msgs = await epiService.getCurrentUserMessages();
        const recentMessages = [];
        $.each(msgs, function (i, m) {
            if (m.MessageType === 0)
                recentMessages.push(m);
        });

        self.setState({
            recentMessages: recentMessages
        }, () => {
            setTimeout(() => {

                //datatables here
                const messageTable = $('.message-table').DataTable({
                    "searching": true,
                    "order": [[2, "desc"]]
                });
                
                self.setState({
                    messageTable: messageTable,
                    isLoading: false
                });

                $('[data-toggle="tooltip"]').tooltip();
            }, 500);
        });
    }

    async markAsRead(msg) {
        var self = this;
        epiService.markMessageAsRead(msg).then(function () {
            if (msg.MessageType == 0)
                notificationService.info("Message marked as read");
            else
                notificationService.info("Notification marked as read");

            self.refresh();
        });
    }

    viewMessage(msg) {
        PubSub.publish("viewMessage", msg);
    }

    formatDate(dateTime) {
        return helpers.formatDateTime(dateTime);
    }

    render() {

        const self = this;

        return (
            <React.Fragment>
                <div className="panel">
                    <div className="panel-body">

                        <div className={!this.state.isLoading ? 'hidden' : null}>
                            <LoadingComponent />
                        </div>

                        <div className={this.state.isLoading ? 'hidden' : null}>
                            <div className="row">
                                <div className="col-lg-12">
                                    Unread Messages
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-lg-12">
                                    <div>
                                        <table className="table table-bordered message-table">
                                            <thead>
                                                <tr>
                                                    <th>From</th>
                                                    <th style={{ width: '250px' }}>Subject</th>
                                                    <th>Received Time</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {
                                                    this.state.recentMessages.map((msg) => {
                                                        return (
                                                            <tr key={msg.MessageId}>
                                                                <td>{msg.FromUsername}</td>
                                                                <td>{msg.Subject}</td>
                                                                <td>{this.formatDate(msg.CreatedTime)}</td>
                                                                <td className="td-btn-group">
                                                                    <button title="View message" data-toggle="tooltip" data-placement="top" onClick={() => { this.viewMessage(msg) }} className="btn btn-primary"><i className="far fa-eye"></i></button>
                                                                    <button title="Mark as read" data-toggle="tooltip" data-placement="top" onClick={() => { this.markAsRead(msg) }} className="btn btn-info"><i className="fas fa-envelope-open"></i></button>
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
                </div>

            </React.Fragment>
        )
    }
}

