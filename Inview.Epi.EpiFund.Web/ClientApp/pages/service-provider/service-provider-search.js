import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { helpers } from '../../helpers/helpers'
import { notificationService } from '../../services/notificationService'

import { LoadingComponent } from '../../loading-component'

export class SearchServiceProviderComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,

            serviceProviders: [],
            isSearching: false,
            name: "",
            company: "",
            registeredTable: null,

            addFromPreferred: this.props.addFromPreferred,

            availableServiceProviderTypes: [],
            availableStates: [],
            availableAssetTypes: []
        }

        this.onChange = this.onChange.bind(this);
        this.search = this.search.bind(this);
        this.uncheckAll = this.uncheckAll.bind(this);
        this.checkAll = this.checkAll.bind(this);
        this.updateLists = this.updateLists.bind(this);
        this.updateCheckbox = this.updateCheckbox.bind(this);
        this.refreshDataTable = this.refreshDataTable.bind(this);

        this.refresh();
    }

    async refresh() {
        const [profile, availableServiceProviderTypes, availableStates, availableAssetTypes] = await Promise.all([
            epiService.getCurrentUserInfoAndProfile(true),
            epiService.getAvailableServiceProviderTypes(),
            epiService.getAvailableStates(),
            epiService.getAvailableAssetTypes()
        ]);

        //build these lists for the checkboxes
        const pt = availableServiceProviderTypes.map((x) => {
            return {
                name: x,
                isSelected: true
            };
        });

        const st = availableStates.map((x) => {
            return {
                name: x,
                isSelected: true
            };
        });

        const at = availableAssetTypes.map((x) => {
            return {
                name: x,
                isSelected: true
            };
        });


        this.setState({
            currentUserProfile: profile,
            availableServiceProviderTypes: pt,
            availableStates: st,
            availableAssetTypes: at,
            isLoading: false
        }, () => {
            this.refreshDataTable();
        });
    }

    updateCheckbox(s) {
        s.isSelected = !s.isSelected;
        this.updateLists();
    }

    updateLists() {
        //React is dumb...
        this.setState({
            availableServiceProviderTypes: this.state.availableServiceProviderTypes,
            availableStates: this.state.availableStates,
            availableAssetTypes: this.state.availableAssetTypes
        });
    }

    uncheckAll(name) {
        const list = this.state[name];
        if (list != null) {
            list.map((l) => {
                l.isSelected = false;
            });

            this.updateLists();
        }
    }

    checkAll(name) {
        const list = this.state[name];
        if (list != null) {
            list.map((l) => {
                l.isSelected = true;
            });

            this.updateLists();
        }
    }

    onChange(e) {
        this.setState({
            [e.target.name]: e.target.value
        });
    }

    search() {
        var self = this;

        self.setState({ isSearching: true }, () => {
            //destory it
            if (helpers.isDataTable($('.registered-table')) === true) {
                self.state.registeredTable.destroy();
            }
        });

        //need to pass it whatever got selected
        var selectedSPTypes = self.state.availableServiceProviderTypes.filter(function (e, i) {
            if (e.isSelected == true) { return e; }
        }).map(function (e, i) { return e.name; });

        var selectedStates = self.state.availableStates.filter(function (e, i) {
            if (e.isSelected == true) { return e; }
        }).map(function (e, i) { return e.name; });

        var selectedAssetTypes = self.state.availableAssetTypes.filter(function (e, i) {
            if (e.isSelected == true) { return e; }
        }).map(function (e, i) { return e.name; });

        //if everything is checked... no filters should be needed
        if (selectedSPTypes.length == self.state.availableServiceProviderTypes.length)
            selectedSPTypes = null;
        if (selectedStates.length == self.state.availableStates.length)
            selectedStates = null;
        if (selectedAssetTypes.length == self.state.availableAssetTypes.length)
            selectedAssetTypes = null;

        epiService.searchServiceProviders(
            self.state.name,
            self.state.company,
            selectedSPTypes, selectedStates, selectedAssetTypes
        ).then(response => {
            self.setState({
                serviceProviders: response
            }, () => {
                self.refreshDataTable();
                self.setState({
                    isSearching: false,
                });
            });
        });
    }

    refreshDataTable() {
        var self = this;
        if (self.state.addFromPreferred == null || self.state.addFromPreferred == true) {
            self.state.registeredTable = $('.registered-table').DataTable({ "searching": false, "order": [[2, "asc"], [1, "asc"]] });
        } else {
            self.state.registeredTable = $('.registered-table').DataTable({ "searching": false, "order": [[1, "asc"], [2, "asc"]] });
        }
        $('[data-toggle="tooltip"]').tooltip();
    }

    render() {

        return (
            <React.Fragment>
                <div>
                    <h3>Search Service Providers</h3>
                    {/*<!--search-->*/}
                    <div className="panel" onKeyPress={(e) => { if (e.key === "Enter") { this.search(); } }}>
                        <div className="panel-body">
                            <div className="row">
                                <div className="col-sm-6">
                                    <div className="form-group">
                                        <label>Name</label>
                                        <input type="text" className="form-control" placeholder="" name="name" value={this.state.name || ""} onChange={this.onChange} />
                                    </div>
                                </div>
                                <div className="col-sm-6">
                                    <div className="form-group">
                                        <label>Company</label>
                                        <input type="text" className="form-control" placeholder="" name="company" value={this.state.company || ""} onChange={this.onChange} />
                                    </div>
                                </div>
                                {/*<!--filters-->*/}
                                <div className="col-sm-12">
                                    <div className="panel-group default-accordion" id="search-sp-filters-accordion" role="tablist" aria-multiselectable="true">
                                        <div className="panel panel-default">
                                            <div className="panel-heading">
                                                <h4 className="panel-title">
                                                    <a role="button" data-toggle="collapse" data-parent="#search-sp-filters-accordion" data-target="#filters">More Filters</a>
                                                </h4>
                                            </div>
                                            <div id="filters" className="panel-collapse collapse">
                                                <div className="panel-body">
                                                    <div className="panel-group default-accordion" id="search-sp-accordion" role="tablist" aria-multiselectable="true">
                                                        <div className="panel panel-default">
                                                            <div className="panel-heading">
                                                                <h4 className="panel-title">
                                                                    <a role="button" data-toggle="collapse" data-parent="#search-sp-accordion" data-target="#IndustryType">Industry Types</a>
                                                                </h4>
                                                            </div>
                                                            <div id="IndustryType" className="panel-collapse collapse in">
                                                                <div className="panel-body">
                                                                    <button className="btn btn-default" onClick={() => { this.checkAll('availableServiceProviderTypes') }}><i className="fas fa-check-square"></i> Check all</button>
                                                                    <button className="btn btn-default" onClick={() => { this.uncheckAll('availableServiceProviderTypes') }}><i className="fas fa-ban"></i> Uncheck all</button>
                                                                    <hr />
                                                                    {
                                                                        this.state.availableServiceProviderTypes.map((type, i) => {
                                                                            return (
                                                                                <div key={i} className="custom-checkbox">
                                                                                    <label><input type="checkbox" checked={type.isSelected} onChange={() => { this.updateCheckbox(type) }} /> <span className="label-text">{type.name}</span></label>
                                                                                </div>
                                                                            )
                                                                        })
                                                                    }
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div className="panel panel-default">
                                                            <div className="panel-heading">
                                                                <h4 className="panel-title">
                                                                    <a className="collapsed" role="button" data-toggle="collapse" data-parent="#search-sp-accordion" data-target="#locations">Available Business Locations</a>
                                                                </h4>
                                                            </div>
                                                            <div id="locations" className="panel-collapse collapse">
                                                                <div className="panel-body">
                                                                    <button className="btn btn-default" onClick={() => { this.checkAll('availableStates') }}><i className="fas fa-check-square"></i> Check all</button>
                                                                    <button className="btn btn-default" onClick={() => { this.uncheckAll('availableStates') }}><i className="fas fa-ban"></i> Uncheck all</button>
                                                                    <hr />
                                                                    {
                                                                        this.state.availableStates.map((type, i) => {
                                                                            return (
                                                                                <div key={i} className="custom-checkbox" style={{ float: 'left', minWidth: '180px' }}>
                                                                                    <label><input type="checkbox" checked={type.isSelected} onChange={() => { this.updateCheckbox(type) }} /> <span className="label-text">{type.name}</span></label>
                                                                                </div>
                                                                            )
                                                                        })
                                                                    }
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div className="panel panel-default">
                                                            <div className="panel-heading">
                                                                <h4 className="panel-title">
                                                                    <a className="collapsed" role="button" data-toggle="collapse" data-parent="#search-sp-accordion" data-target="#assetTypes">
                                                                        Available Business Asset Types
                                                                    </a>
                                                                </h4>
                                                            </div>
                                                            <div id="assetTypes" className="panel-collapse collapse">
                                                                <div className="panel-body">
                                                                    <button className="btn btn-default" onClick={() => { this.checkAll('availableAssetTypes') }}><i className="fas fa-check-square"></i> Check all</button>
                                                                    <button className="btn btn-default" onClick={() => { this.uncheckAll('availableAssetTypes') }}><i className="fas fa-ban"></i> Uncheck all</button>
                                                                    <hr />
                                                                    {
                                                                        this.state.availableAssetTypes.map((type, i) => {
                                                                            return (
                                                                                <div key={i} className="custom-checkbox" style={{ float: 'left', minWidth: '290px' }}>
                                                                                    <label><input type="checkbox" checked={type.isSelected} onChange={() => { this.updateCheckbox(type) }} /> <span className="label-text">{type.name}</span></label>
                                                                                </div>
                                                                            )
                                                                        })
                                                                    }
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    {/*<!--end filters-->*/}
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                {/*<!--end filters-->*/}
                                <div className="col-sm-12">
                                    <hr />
                                    <button className="btn btn-primary" onClick={this.search}><i className="fas fa-search"></i> Search</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    {/*<!--registered SP-->*/}
                    {
                        !this.state.addFromPreferred &&
                        <div className="panel">
                            <div className="panel-body">
                                <div className="row">
                                    <div className="col-sm-12">
                                        <h4 className="sp-h4">Registered Service Providers</h4>
                                        <hr />
                                        {
                                            <div>
                                                <table className={'table table-bordered registered-table ' + ((this.state.isLoading || this.state.isSearching) ? 'hidden' : null)}>
                                                    <thead>
                                                        <tr>
                                                            <th>First Name</th>
                                                            <th>Last Name</th>
                                                            <th>Industry</th>
                                                            <th>Company</th>
                                                            <th>City, State</th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>{
                                                        this.state.serviceProviders.map((sp, i) => {
                                                            return (
                                                                <tr key={i}>
                                                                    <td>{sp.FirstName}</td>
                                                                    <td>{sp.LastName}</td>
                                                                    <td>{sp.ServiceProviderTypeDescription}</td>
                                                                    <td>{sp.User.CompanyName}</td>
                                                                    <td></td>
                                                                    <td style={{ width: '170px' }}>
                                                                        <button title="Compose message" data-toggle="tooltip" data-placement="top" onClick={() => {
                                                                            PubSub.publish("newMessageToUser", sp)
                                                                        }} className="btn btn-primary margin-right10px"><i className="fas fa-edit"></i></button>
                                                                        <button title="Quick view profile" data-toggle="tooltip" data-placement="top" onClick={() => {
                                                                            PubSub.publish("quickViewUser", sp)
                                                                        }} className="btn btn-info"><i className="far fa-eye"></i></button>
                                                                    </td>
                                                                </tr>
                                                            )
                                                        })

                                                    }
                                                    </tbody>
                                                </table>
                                            </div>
                                        }
                                    </div>
                                    {
                                        (this.state.isLoading || this.state.isSearching) &&
                                        <div className="col-sm-12">
                                            <LoadingComponent />

                                        </div>
                                    }
                                </div>
                            </div>
                        </div>
                    }

                    {/*<!--preferred sp search-->*/}
                    {
                        this.state.addFromPreferred &&
                        <div className="panel">
                            <div className="panel-body">
                                <div className="row">
                                    <div className="col-sm-12">
                                        <h4 className="sp-h4">Registered Service Providers</h4>
                                        <hr />
                                        <table className={'table table-bordered registered-table ' + ((this.state.isLoading || this.state.isSearching) ? 'hidden' : null)}>
                                            <thead>
                                                <tr>
                                                    <th style={{ width: '30px' }}></th>
                                                    <th>First Name</th>
                                                    <th>Last Name</th>
                                                    <th>Industry</th>
                                                    <th style={{ width: '30px' }}></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                { /* there are differnent options when you call this component from preferred sp */

                                                    this.state.serviceProviders.map((sp, i) => {

                                                        return (
                                                            <tr key={i}>
                                                                <td>
                                                                    <div className="custom-checkbox">
                                                                        <label><input type="checkbox" value={sp.IsSelected} onChange={() => { sp.IsSelected = !sp.IsSelected }} /> <span className="label-text"></span></label>
                                                                    </div>
                                                                </td>
                                                                <td>{sp.FirstName}</td>
                                                                <td>{sp.LastName}</td>
                                                                <td>{sp.ServiceProviderTypeDescription}</td>
                                                                <td>
                                                                    <button title="Quick view profile" data-toggle="tooltip" data-placement="top" onClick={() => {
                                                                        sp.ViewFromNewPreferredModal = true;
                                                                        PubSub.publish("quickViewUser", sp)
                                                                    }} className="btn btn-info"><i className="far fa-eye"></i></button>
                                                                </td>
                                                            </tr>
                                                        )

                                                    })
                                                }
                                            </tbody>
                                        </table>
                                        <button className="btn btn-primary" onClick={() => { PubSub.publish("proceedWithSelectedSPs", this.state.serviceProviders.filter((x) => { return x.IsSelected; })) }}>Next</button>
                                    </div>
                                    {
                                        (this.state.isLoading || this.state.isSearching) &&
                                        <div className="col-sm-12">
                                            <LoadingComponent />
                                        </div>
                                    }
                                </div>
                            </div>
                        </div>
                    }


                </div >

            </React.Fragment >
        )
    }
}

const element = document.getElementById('service-provider-search-app');
if (element != null)
    ReactDOM.render(<SearchServiceProviderComponent addFromPreferred={false} />, element)