import React, { Component } from "react"
import ReactDOM from 'react-dom';

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'
import { helpers } from '../../helpers/helpers'
import { LoadingComponent } from '../../loading-component'

export class UnreadNotificationOverviewComponent extends Component {
    constructor(props) {
        super(props);

        const self = this;

        this.state = {
            isLoading: true,
            recentNotifications: []
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
        if (helpers.isDataTable($('.recent-notification-table')) === true) {
            self.state.notificationTable.destroy();
        }

        const msgs = await epiService.getCurrentUserMessages();
        const recentNotifications = [];
        $.each(msgs, function (i, m) {
            if (m.MessageType != 0)
                recentNotifications.push(m);
        });

        self.setState({
            recentNotifications: recentNotifications
        }, () => {
            setTimeout(() => {

                //datatables here
                const notificationTable = $('.recent-notification-table').DataTable({
                    "searching": true,
                    "order": [[2, "desc"]]
                });

                self.setState({
                    notificationTable: notificationTable,
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
                                    Recent Notifications
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-lg-12">
                                    <div>
                                        <table className="table table-bordered recent-notification-table">
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
                                                    this.state.recentNotifications.map((notification) => {
                                                        return (
                                                            <tr key={notification.MessageId}>
                                                                <td>{notification.FromUsername}</td>
                                                                <td>{notification.Subject}</td>
                                                                <td>{this.formatDate(notification.CreatedTime)}</td>
                                                                <td className="td-btn-group">
                                                                    <button title="View notification" data-toggle="tooltip" data-placement="top" onClick={() => { this.viewMessage(notification) }} className="btn btn-primary"><i className="far fa-eye"></i></button>
                                                                    <button title="Mark as read" data-toggle="tooltip" data-placement="top" onClick={() => { this.markAsRead(notification) }} className="btn btn-info"><i className="fas fa-envelope-open"></i></button>
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

