import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
import NumberFormat from 'react-number-format';
const PubSub = require('pubsub-js');

import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

import { epiService } from '../../../services/epiService'
import { accountingService } from '../../../services/accountingService'
import { notificationService } from '../../../services/notificationService'

import { LoadingComponent } from '../../../loading-component'
import { helpers } from "../../../helpers/helpers";
import { setTimeout } from "timers";


export class ManageRevenueComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,
            isModalOpen: false,
            revenueTable: null,

            transactions: [],
            transaction: {}
        };

        this.closeModal = this.closeModal.bind(this);
        this.resetTransaction = this.resetTransaction.bind(this);
        this.saveTransaction = this.saveTransaction.bind(this);
        this.onChange = this.onChange.bind(this);
    }

    componentDidMount() {
        this.refresh();
    }

    async refresh() {
        const self = this;

        self.setState({
            isLoading: true,
        });

        //datatable
        const table = $(".revenue-table");
        if (helpers.isDataTable(table) === true) {
            self.state.revenueTable.destroy();
        }

        const transactions = await accountingService.getRevenueTransactions();
        self.resetTransaction();


        self.setState({
            transactions: transactions,
        }, () => {
            const revenueTable = table.DataTable({
                "searching": true,
                "order": [[0, "desc"]],
                "columnDefs": [
                    { "orderable": false, "targets": 7 }
                ]
            });

            self.setState({
                revenueTable: revenueTable,
                isLoading: false
            }, () => {
                $('[data-toggle="tooltip"]').tooltip();
            });
        });

    }

    closeModal() {
        this.setState({ isModalOpen: false });
        this.resetTransaction();
    }

    resetTransaction() {
        this.setState({
            transaction: {
                RevenueTransactionId: helpers.GuidEmpty,
                EscrowCloseDate: new Date(),
                FeePaidDate: new Date(),
                AssetNumber: 0,
                FinalSalesPrice: 0,
                USCREFee: 0,
                IsActive: 1,
                Comment: ""
            }
        });
    }

    deleteTransaction(transaction) {
        const result = confirm("Are you sure you want to remove this transaction? It cannot be undone.");
        if (result === true) {
            notificationService.info("Transaction removed");
            this.refresh();
        }
    }

    onChange(event) {
        if (event.target.name == null)
            console.warn("Event input without name");

        this.state.transaction[event.target.name] = event.target.value;

        this.setState({
            transaction: this.state.transaction
        })
    }

    async saveTransaction() {

        if (this.state.transaction.FeePaidDate == null) {
            notificationService.warn("Date fee paid input is invalid");
            return;
        }

        if (this.state.transaction.AssetNumber <= 0) {
            notificationService.warn("Asset number is invalid");
            return;
        }

        if (this.state.transaction.FinalSalesPrice === 0) {
            notificationService.warn("Final sales price is invalid");
            return;
        }


        const result = await accountingService.saveRevenueTransaction(this.state.transaction);

        if (!result.Success) {
            notificationService.error("Failed to save. " + result.Message);
        } else {
            this.closeModal();
            notificationService.info("Saved transaction");
            this.refresh();
        }
    }

    render() {

        return (
            <React.Fragment>

                <h3 className="page-header">Manage Revenue</h3>

                {
                    this.state.isLoading &&
                    <LoadingComponent />
                }

                <div className={(this.state.isLoading ? 'hidden' : null)}>
                    <div className="row">
                        <div className="col-lg-12">
                            <button className="btn btn-primary" onClick={() => { this.setState({ isModalOpen: true }) }}>Record Revenue</button>
                        </div>
                    </div>
                    <hr />
                    <div className="row">
                        <div className="col-lg-12">
                            <table className="table table-bordered revenue-table">
                                <thead>
                                    <tr>
                                        <th>Date Fee Paid</th>
                                        <th>Close of Escrow Date</th>
                                        <th>Name of PIs</th>
                                        <th>Asset #</th>
                                        <th>Asset Type</th>
                                        <th>Final Sales Price</th>
                                        <th>1% Fee to USCRE</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {
                                        this.state.transactions.map(t => {
                                            return (
                                                <tr key={t.RevenueTransactionId}>
                                                    <td>{helpers.formatDate(t.FeePaidDate)}</td>
                                                    <td>{helpers.formatDate(t.EscrowCloseDate)}</td>
                                                    <td>????</td>
                                                    <td>{t.AssetNumber}</td>
                                                    <td>????</td>
                                                    <td><NumberFormat value={t.FinalSalesPrice} displayType={'text'} thousandSeparator={true} prefix={'$'} /></td>
                                                    <td>{t.USCREFee}</td>
                                                    <td>
                                                        <button
                                                            title="Delete" data-toggle="tooltip" data-placement="top"
                                                            className="btn btn-danger" onClick={() => { this.deleteTransaction(t) }}><i className="fa fa-trash"></i></button>
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

                <Modal
                    isOpen={this.state.isModalOpen}
                    shouldCloseOnOverlayClick={true}
                    overlayClassName="modal-mask"
                    className="modal-wrapper"
                    closeTimeoutMS={200}>

                    <div className="modal-container">

                        <div className="modal-header">
                            <h3>New Renvenue Transaction</h3>
                        </div>

                        <div className="modal-body">

                            <div className="form-group">
                                <label>Date Fee Paid</label>
                                <DatePicker
                                    todayButton={"Today"}
                                    selected={this.state.transaction.FeePaidDate}
                                    onChange={(e) => { this.state.transaction.FeePaidDate = e; this.setState({ transaction: this.state.transaction }); }}
                                    placeholderText="Click to select a date"
                                />
                            </div>

                            <div className="form-group">
                                <label>Close of Escrow Date</label>
                                <DatePicker
                                    todayButton={"Today"}
                                    selected={this.state.transaction.EscrowCloseDate}
                                    onChange={(e) => { this.state.transaction.EscrowCloseDate = e; this.setState({ transaction: this.state.transaction }); }}
                                    placeholderText="Click to select a date"
                                />
                            </div>

                            <div className="form-group">
                                <label>Asset #</label>
                                <input className="form-control" type="number" placeholder="Enter asset number"
                                    min="0"
                                    name="AssetNumber"
                                    value={this.state.transaction.AssetNumber}
                                    onChange={this.onChange} />
                            </div>


                            <div className="form-group">
                                <label>Final Sales Price</label>
                                <NumberFormat className="form-control" thousandSeparator={true} prefix={'$'}
                                    min="0"
                                    name="FinalSalesPrice"
                                    value={this.state.transaction.FinalSalesPrice}
                                    placeholder="Enter final sales price"
                                    onValueChange={(values) => {
                                        const { formattedValue, value } = values;
                                        this.state.transaction.FinalSalesPrice = value;
                                    }} />
                            </div>

                            <div>
                                <label>Comment</label>
                                <textarea className="form-control" rows="3"
                                    name="Comment"
                                    value={this.state.transaction.Comment}
                                    onChange={this.onChange}></textarea>
                            </div>

                        </div>

                        <div className="modal-footer">
                            <button className="btn pull-right" onClick={this.closeModal}>Cancel</button>
                            <button className="btn btn-primary pull-right margin-right10px" onClick={this.saveTransaction}>Save</button>
                        </div>
                    </div>

                </Modal>

            </React.Fragment >
        )
    }
}

const element = document.getElementById("manage-revenue-page");
if (element != null) {
    ReactDOM.render(<ManageRevenueComponent />, element);
    Modal.setAppElement(element)
}
