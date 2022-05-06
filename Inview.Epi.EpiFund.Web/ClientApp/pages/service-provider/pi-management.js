import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'

import { LoadingComponent } from '../../loading-component'
import { loadavg } from "os";

export class PIManagementComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,

            isInventoryTableLoading: true,
            isFpaActivityTableLoading: true,
            isAccountingActivityLoading: true,

            registeredPIs: [],
            inventoryTable: null,

            fpaActivityTable: null,
            accountingActivityTable: null
        }
    }

    componentDidMount() {
        this.refresh();
    }

    async refresh() {
        const self = this;
        self.setState({ isLoading: false });



        setTimeout(function () {

            //fpa activity
            if (self.state.fpaActivityTable != null)
                self.state.fpaActivityTable.destroy();

            self.setState({
                fpaActivityTable: $('.fpa-activity-table').DataTable(),
                isFpaActivityTableLoading: false
            });

            //accounting activity
            if (self.state.accountingActivityTable != null)
                self.state.accountingActivityTable.destroy();

            self.setState({
                accountingActivityTable: $('.accounting-activity-table').DataTable(),
                isAccountingActivityLoading: false
            });


            $('[data-toggle="tooltip"]').tooltip();

        }, 1000);


        //inventory
        epiService.getUserReferrals().then(function (r) {
            if (r != null) {
                var registeredIDs = r.filter(function (m) {
                    if (m.ReferralStatus == 2 || m.ReferralStatus == 3) {
                        return m;
                    }
                }).map(function (k) { return k.ReferredUserId });

                //filter it more down to PIs only
                if (registeredIDs.length > 0) {
                    epiService.getUserProfileBysUserIds(registeredIDs).then(function (r) {

                        const registeredPIs = [];
                        for (var i = 0; i < r.length; i++) {
                            var k = r[i];
                            if (k.User.UserType == 5 && k.UserTypeDescription == "Principal Investor") {
                                registeredPIs.push(k);
                            }
                        }
                        self.setState({ registeredPIs: registeredPIs });

                        if (self.state.inventoryTable != null)
                            self.state.inventoryTable.destroy();

                        setTimeout(function () {
                            self.setState({
                                inventoryTable: $('.inventory-of-marketing-table').DataTable(),
                                isInventoryTableLoading: false
                            });
                        }, 500);
                    });
                } else {
                    //no PI found. just return the table
                    if (self.state.inventoryTable != null)
                        self.state.inventoryTable.destroy();

                    setTimeout(function () {
                        self.setState({
                            inventoryTable: $('.inventory-of-marketing-table').DataTable(),
                            isInventoryTableLoading: false
                        });
                    }, 500);
                }
            }
        });
    }


    render() {

        return (
            <React.Fragment>
                {

                    <React.Fragment>
                        <div className={!this.state.isLoading ? 'hidden' : null}>
                            <LoadingComponent />
                        </div>


                        <div className={this.state.isLoading ? 'hidden' : null}>
                            <h3>My PI Management Center</h3>

                            <div className="panel">
                                <div className="panel-body">
                                    <h4>Inventory of Marketing Network Registrations</h4>
                                    <div className="col-sm-12" className={'col-sm-12 ' + (!this.state.isInventoryTableLoading ? 'hidden' : null)}>
                                        <LoadingComponent />
                                    </div>
                                    <div className={'col-sm-12 ' + (this.state.isInventoryTableLoading ? 'hidden' : null)}>
                                        <table className="table table-bordered inventory-of-marketing-table">
                                            <thead>
                                                <tr>
                                                    <th>First Name</th>
                                                    <th>Last Name</th>
                                                    <th>City</th>
                                                    <th>ST</th>
                                                    <th>Date PI Registered</th>
                                                    <th>Network PI e Mail</th>
                                                    <th>Business Phone</th>
                                                    <th><a href="#" title="Formal Purchase Agreement" data-toggle="tooltip" data-placement="top">FPA</a> in Play</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {
                                                    this.state.registeredPIs.map((pi, i) => {
                                                        return (
                                                            <tr key={i}>
                                                                <td>{pi.FirstName}</td>
                                                                <td>{pi.LastName}</td>
                                                                <td>{pi.User.City}</td>
                                                                <td>{pi.User.State}</td>
                                                                <td>{pi.User.SignupDate}</td>
                                                                <td><a href={'mailto:' + pi.User.Username}>{pi.User.Username}</a> </td>
                                                                <td>{pi.User.WorkNumber}</td>
                                                                <td></td>
                                                                <td>
                                                                    <button title="Compose message" data-toggle="tooltip" data-placement="top" onClick={() => { console.log("openMessageModal(pi)") }} className="btn btn-primary"><i className="fas fa-edit"></i></button>
                                                                    <button title="Quick view profile" data-toggle="tooltip" data-placement="top" onClick={() => { console.log("openQuickViewModal(pi)") }} className="btn btn-info"><i className="far fa-eye"></i></button>
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

                            <div className="panel">
                                <div className="panel-body">
                                    <h4><a href="#" title="Formal Purchase Agreement" data-toggle="tooltip" data-placement="top">FPA</a> Activity of Registered Network PI</h4>
                                    <div className="col-sm-12" className={'col-sm-12 ' + (!this.state.isFpaActivityTableLoading ? 'hidden' : null)}>
                                        <LoadingComponent />
                                    </div>

                                    <div className="col-sm-12" className={'col-sm-12 ' + (this.state.isFpaActivityTableLoading ? 'hidden' : null)}>
                                        <table className="table table-bordered fpa-activity-table">
                                            <thead>
                                                <tr>
                                                    <th>Asset Type</th>
                                                    <th>City</th>
                                                    <th>ST</th>
                                                    <th><i className="fas fa-graduation-cap"></i></th>
                                                    <th>Date of <a href="#" title="Formal Purchase Agreement" data-toggle="tooltip" data-placement="top">FPA</a></th>
                                                    <th>Network PI e Mail</th>
                                                    <th>Business Phone</th>
                                                    <th><a href="#" title="Formal Purchase Agreement" data-toggle="tooltip" data-placement="top">FPA</a> in Play</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                </div>
                            </div>

                            <div className="panel">
                                <div className="panel-body">
                                    <h4>Accounting Activity of Registered Network PI</h4>
                                    <div className="col-sm-12" className={'col-sm-12 ' + (!this.state.isAccountingActivityLoading ? 'hidden' : null)}>
                                        <LoadingComponent />
                                    </div>
                                    <div className="col-sm-12" className={'col-sm-12 ' + (this.state.isAccountingActivityLoading ? 'hidden' : null)}>
                                        <table className="table table-bordered accounting-activity-table">
                                            <thead>
                                                <tr>
                                                    <th>Asset Type</th>
                                                    <th>City</th>
                                                    <th>ST</th>
                                                    <th><i className="fas fa-graduation-cap"></i></th>
                                                    <th>Date of Closing</th>
                                                    <th>Closing SP</th>
                                                    <th>USCRE Fee</th>
                                                    <th>JVMP $</th>
                                                    <th>Date Wired</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </React.Fragment>
                }
            </React.Fragment>
        )
    }
}

const element = document.getElementById('pi-management-app');
if (element != null)
    ReactDOM.render(<PIManagementComponent />, element)