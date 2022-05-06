import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'

import { LoadingComponent } from '../../loading-component'

export class PreferredServiceProviderComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,

            industrySPs: [],
            locationSPs: [],
            assetTypeSPs: []
        }

        this.deleteSelectedPreferredSP = this.deleteSelectedPreferredSP.bind(this);

        PubSub.subscribe("refreshPreferredSPs", (type, date) => {
            self.refresh();
        });
    }

    componentDidMount() {
        this.refresh();
    }

    async refresh() {
        const self = this;
        epiService.getUserPreferredServiceProviders().then(function (result) {
            self.setState({
                industrySPs: [],
                locationSPs: [],
                assetTypeSPs: []
            })
            //Industry = 0,
            //Location = 1,
            //AssetType = 2
            const industrySPs = [];
            result.map(function (a, b) {
                if (a.PreferredServiceProviderLevel == 0) {
                    industrySPs.push(a); //just push here, industry type is easy
                    return true;
                }
                return false;
            });

            //group them///////////////////////////////////
            var locationSps = [];
            $.each(result, function (i, e) {
                if (e.PreferredServiceProviderLevel == 1) {
                    var found = locationSps.filter(function (a, b) {
                        if (a.PreferredUserId == e.PreferredUserId) { return true; } return false;
                    })[0];

                    if (found == null) {
                        locationSps.push(e);
                    } else {
                        //add to the found location description
                        found.LocationDescription = found.LocationDescription + " | " + e.LocationDescription;
                    }
                }
            });

            var assetSps = [];
            $.each(result, function (i, e) {
                if (e.PreferredServiceProviderLevel == 2) {
                    var found = assetSps.filter(function (a, b) {
                        if (a.PreferredUserId == e.PreferredUserId) { return true; } return false;
                    })[0];

                    if (found == null) {
                        assetSps.push(e);
                    } else {
                        //add to the found description
                        found.AssetTypeDescription = found.AssetTypeDescription + " | " + e.AssetTypeDescription;
                    }
                }
            });

            self.setState({
                industrySPs: industrySPs,
                locationSPs: locationSps,
                assetTypeSPs: assetSps,
                isLoading: false
            })

            ///////////////////////////////////////////////
        });
    }

    deleteSelectedPreferredSP(list, level) {

        let self = this;
        const preferredUserIds = list.filter(x => { return x.IsSelected }).map(x => x.PreferredUserId);

        if (preferredUserIds.length !== 0) {
            epiService.removePreferredServiceProvidersByPreferredUserIdsAndLevel(preferredUserIds, level).then(function (result) {
                notificationService.info("Removed preferred service provider(s)");
                self.refresh();
            });
        } else {
            notificationService.warn("Please select a service provider to remove");
        }
    }

    render() {

        return (
            <React.Fragment>
                {
                    this.state.isLoading ?
                        <LoadingComponent />
                        :
                        <React.Fragment>
                            <div className="preferred-sp-panel-page">
                                <h3>Preferred Service Providers</h3>

                                <div className="panel">
                                    <div className="panel-body">

                                        <div>
                                            <div className="row">
                                                <div className="col-sm-12">
                                                    <button onClick={() => { PubSub.publish("newPreferredServiceProviderModal", true) }} className="btn btn-primary"><i className="fas fa-plus"></i> Add new preferred service provider</button>
                                                    <br /><br />
                                                    {/*<!-- Nav tabs -->*/}
                                                    <div className="">
                                                        <ul className="nav nav-tabs" role="tablist">
                                                            <li role="presentation" className="active">
                                                                <a href="#" data-target="#Industry" data-toggle="tab">Industry <span className={'label ' + (this.state.industrySPs.length > 0 ? 'label-info' : 'label-default')}>{this.state.industrySPs.length}</span></a>
                                                            </li>
                                                            <li role="presentation">
                                                                <a href="#" data-target="#Location" data-toggle="tab">Location <span className={'label ' + (this.state.locationSPs.length > 0 ? 'label-info' : 'label-default')}> {this.state.locationSPs.length}</span></a>
                                                            </li>
                                                            <li role="presentation">
                                                                <a href="#" data-target="#Type" data-toggle="tab">Asset Type <span className={'label ' + (this.state.assetTypeSPs.length > 0 ? 'label-info' : 'label-default')}> {this.state.assetTypeSPs.length}</span></a>
                                                            </li>
                                                        </ul>

                                                        {/*<!-- Tab panes -->*/}
                                                        <div className="tab-content">
                                                            <div role="tabpanel" className="tab-pane fade in active" id="Industry">
                                                                <div className="row">
                                                                    <div className="col-sm-12">
                                                                        <button className="btn btn-danger" onClick={() => { this.deleteSelectedPreferredSP(this.state.industrySPs, 0) }}><i className="fas fa-times"></i> Delete selected service providers</button>
                                                                    </div>
                                                                </div>

                                                                <table className="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style={{ width: '50px' }}></th>
                                                                            <th>First Name</th>
                                                                            <th>Last Name</th>
                                                                            <th>Industry</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        {
                                                                            this.state.industrySPs.map((locSp, ii) => {
                                                                                return (
                                                                                    <tr key={ii}>
                                                                                        <td>
                                                                                            <div className="custom-checkbox">
                                                                                                <label><input type="checkbox" value="locSp.IsSelected" onChange={() => { locSp.IsSelected = !locSp.IsSelected }} /> <span className="label-text"></span></label>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td>{locSp.UserProfile.FirstName}</td>
                                                                                        <td>{locSp.UserProfile.LastName}</td>
                                                                                        <td>{locSp.UserProfile.ServiceProviderTypeDescription}</td>
                                                                                    </tr>
                                                                                )
                                                                            })
                                                                        }
                                                                    </tbody>
                                                                </table>

                                                            </div>
                                                            <div role="tabpanel" className="tab-pane fade" id="Location">
                                                                <div className="row">
                                                                    <div className="col-sm-12">
                                                                        <button className="btn btn-danger" onClick={() => { this.deleteSelectedPreferredSP(this.state.locationSPs, 1) }}><i className="fas fa-times"></i> Delete selected service providers</button>
                                                                    </div>
                                                                </div>
                                                                <table className="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style={{ width: '50px' }}></th>
                                                                            <th>First Name</th>
                                                                            <th>Last Name</th>
                                                                            <th>Locations</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        {
                                                                            this.state.locationSPs.map((lSp, ii) => {
                                                                                return (
                                                                                    <tr key={ii}>
                                                                                        <td>
                                                                                            <div className="custom-checkbox">
                                                                                                <label><input type="checkbox" value="lSp.IsSelected" onChange={() => { lSp.IsSelected = !lSp.IsSelected }} /> <span className="label-text"></span></label>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td>{lSp.UserProfile.FirstName}</td>
                                                                                        <td>{lSp.UserProfile.LastName}</td>
                                                                                        <td>{lSp.LocationDescription}</td>
                                                                                    </tr>
                                                                                )
                                                                            })
                                                                        }
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                            <div role="tabpanel" className="tab-pane fade" id="Type">
                                                                <div className="row">
                                                                    <div className="col-sm-12">
                                                                        <button className="btn btn-danger" onClick={() => { this.deleteSelectedPreferredSP(this.state.assetTypeSPs, 2) }}><i className="fas fa-times"></i> Delete selected service providers</button>
                                                                    </div>
                                                                </div>
                                                                <table className="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style={{ width: '50px' }}></th>
                                                                            <th>First Name</th>
                                                                            <th>Last Name</th>
                                                                            <th>Asset Types</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        {
                                                                            this.state.assetTypeSPs.map((atSp, ii) => {
                                                                                return (
                                                                                    <tr key={ii}>
                                                                                        <td>
                                                                                            <div className="custom-checkbox">
                                                                                                <label><input type="checkbox" value="atSp.IsSelected" onChange={() => { atSp.IsSelected = !atSp.IsSelected }} /> <span className="label-text"></span></label>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td>{atSp.UserProfile.FirstName}</td>
                                                                                        <td>{atSp.UserProfile.LastName}</td>
                                                                                        <td>{atSp.AssetTypeDescription}</td>
                                                                                    </tr>
                                                                                )
                                                                            })
                                                                        }
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    {/*<!--end tab / nav -->*/}
                                                </div>

                                            </div>
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

const element = document.getElementById('preferred-service-provider-app');
if (element != null)
    ReactDOM.render(<PreferredServiceProviderComponent addFromPreferred={false} />, element)