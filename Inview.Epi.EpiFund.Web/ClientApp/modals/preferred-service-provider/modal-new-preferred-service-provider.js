import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'
import { LoadingComponent } from '../../loading-component'

import { SearchServiceProviderComponent } from '../../pages/service-provider/service-provider-search'

export class NewPreferredServiceProviderModal extends Component {
    constructor(props) {
        super(props);
        const self = this;
        this.state = {
            isOpen: false,
            isLoading: true,
            currentStep: 1,
            newPreferredSPModalPresetIds: [],
            isAddFromContacts: false,
            disabledChooseLevelBtn: true,

            selectedSPs: [],
            preferredServiceProviderLevel: 0,

            locations: [],
            availableStates: [],
            availableAssetTypes: [],
            availableCountries: ["USA"],
        }

        this.openModal = this.openModal.bind(this);
        this.afterOpenModal = this.afterOpenModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.canChooseLevel = this.canChooseLevel.bind(this);
        this.preview = this.preview.bind(this);
        this.save = this.save.bind(this);
        this.removeThisLocation = this.removeThisLocation.bind(this);
        this.addNewLocation = this.addNewLocation.bind(this);
        this.confirmPreview = this.confirmPreview.bind(this);
        this.onLocationChanges = this.onLocationChanges.bind(this);

        ///////////////events/////////////////////////////////////
        PubSub.subscribe("newPreferredServiceProviderModal", async (type, data) => {
            await this.refresh();

            this.setState({
                isOpen: true,
                isLoading: false,
                currentStep: 1,
                addFromPreferred: data,
            });
        });

        PubSub.subscribe("proceedWithSelectedSPs", async (type, sps) => {
            if (sps.length === 0) {
                notificationService.warn("Please select a service provider");
                return;
            }

            self.setState({
                currentStep: 3,
                selectedSPs: sps
            });
        });

        PubSub.subscribe("proceedWithSelectedSP", async (type, sps) => {
            if (sps.length === 0) {
                notificationService.warn("Please select a service provider");
                return;
            }

            await this.refresh();

            self.setState({
                isOpen: true,
                isLoading: false,
                currentStep: 3,
                selectedSPs: sps.map(ss => {
                    ss.IsSelected = true; return ss;
                }),
                newPreferredSPModalPresetIds: sps.map(sp => { sp.UserId }),
                addFromPreferred: false //<-- this is where user do a quick view sp then set preferred
            });
        });

    }

    async refresh() {
        const self = this;

        this.setState({
            isLoading: true,
            currentStep: 1,
            disabledChooseLevelBtn: true,

            selectedSPs: [],
            preferredServiceProviderLevel: 0,

            locations: [],
            availableStates: [],
            availableAssetTypes: [],
        });




        const profile = await epiService.getCurrentUserInfoAndProfile(true);

        //add a default location
        this.state.locations.push({ country: "USA", state: "Alabama" });

        //get available locations
        epiService.getAvailableStates().then(function (types) {
            const availableStates = [];
            for (var i = 0; i < types.length; i++) {
                availableStates.push({
                    "Name": types[i]
                });
            }
            self.setState({ availableStates: availableStates });
        });

        //get available asset types
        epiService.getAvailableAssetTypes().then(function (types) {
            const availableAssetTypes = [];
            for (var i = 0; i < types.length; i++) {
                availableAssetTypes.push({
                    "Name": types[i],
                    "IsSelected": false
                });
            }
            self.setState({ availableAssetTypes: availableAssetTypes });
        });

        const contacts = profile.Contacts.map(x => {
            x.IsSelected = false;
            return x;
        }).filter(x => {
            if (x.User.UserType !== 14)
                return false;
            return true;
        });

        this.setState({
            currentUserProfile: profile,
            selectedSPs: contacts,
        })
    }

    openModal() {
    }

    afterOpenModal() {

    }

    closeModal(refresh = false) {
        if (refresh)
            PubSub.publish("refreshPreferredSPs");

        this.setState({ isOpen: false });
    }

    onChange() {
    }

    save() {
        var self = this;
        self.setState({ isLoading: true });

        const preferredUserIds = self.state.selectedSPs.filter((f) => { return f.IsSelected }).map(f => f.UserId);;
        //these we don't know yet
        var state = null;
        var city = null;
        var country = null;

        if (self.state.preferredServiceProviderLevel === 0) {
            //industry
            epiService.addPreferredServiceProviders(
                preferredUserIds,
                self.state.preferredServiceProviderLevel
            ).then(function (r) {
                notificationService.info("Added preferred service provider(s)");

                //close it back
                self.closeModal(true);
            });
        } else if (self.state.preferredServiceProviderLevel == 2) {
            //asset types
            var selectedAvailableAssetTypes = self.state.availableAssetTypes.filter(x => { return x.IsSelected });

            epiService.addPreferredServiceProviders(
                preferredUserIds,
                self.state.preferredServiceProviderLevel,

                selectedAvailableAssetTypes, //<-- the selected types
                []
            ).then(function (r) {
                notificationService.info("Added preferred service provider(s)");

                //close it back
                self.closeModal(true);
            });

        }
        else if (self.state.preferredServiceProviderLevel == 1) {
            //location
            var locations = [];
            $.each(self.state.locations, function (i, e) {
                locations.push(e.country + "," + e.state);
            });

            epiService.addPreferredServiceProviders(
                preferredUserIds,
                self.state.preferredServiceProviderLevel,

                [],
                locations
            ).then(function (r) {
                notificationService.info("Added preferred service provider(s)");

                //close it back
                self.closeModal(true);
            });

        } else {
            throw new Error("Not Supported.");
        }
    }

    confirmPreview() {
        this.setState({
            currentStep: 5
        })
    }

    preview(preferredServiceProviderLevel) {
        var self = this;

        //everything is based on the "level"
        //Industry = 0,
        //Location = 1,
        //AssetType = 2

        if (preferredServiceProviderLevel != null) {
            self.setState({ preferredServiceProviderLevel: preferredServiceProviderLevel }, () => {
                //Industry / service provider type is the easiest level
                //service provider type is pulled in repo call

                //industry doesn't have to go through step 4
                if (self.state.preferredServiceProviderLevel === 0)
                    self.setState({ currentStep: 5 });
                else
                    self.setState({ currentStep: 4 });
            });
        }
    }

    onLocationChanges() {
        this.setState({
            locations: this.state.locations
        })
    }

    addNewLocation() {
        this.state.locations.push({ country: "USA", state: "Alabama" });
        this.setState({
            locations: this.state.locations
        })
    }

    removeThisLocation(location) {
        var self = this;
        if (self.state.locations > 1) {
            var i = self.state.locations.indexOf(location);
            self.state.locations.splice(i, 1);
            self.setState({ locations: self.state.locations });
        } else {
            notificationService.error("Must have at least 1 preferred location");
        }
    }

    canChooseLevel() {
        const selected = this.state.selectedSPs.filter(x => { if (x.IsSelected) return x; });
        this.setState({
            disabledChooseLevelBtn: selected.length === 0
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
                        overlayClassName="modal-mask zIndex4"
                        className="modal-wrapper"
                        closeTimeoutMS={200}>

                        {
                            this.state.isLoading ?
                                <LoadingComponent /> :
                                <div className={'modal-container ' + (this.currentStep !== 1 ? 'modal-60' : '')}>

                                    <div className="modal-header">
                                        <h3>Add New Preferred Service Provider</h3>
                                    </div>

                                    <div className={'modal-body ' + (this.state.currentStep === 1 ? 'modal-body-auto-height' : '')}>


                                        <div className="stepwizard">
                                            {
                                                this.state.newPreferredSPModalPresetIds.length === 0 ?
                                                    <div className="stepwizard-row">
                                                        <div className="stepwizard-step">
                                                            <div className={'btn btn-circle ' + (this.state.currentStep === 1 ? 'btn-primary' : 'btn-default')}>1</div>
                                                            <p>Step 1</p>
                                                        </div>
                                                        <div className="stepwizard-step">
                                                            <div className={'btn btn-circle ' + (this.state.currentStep === 2 ? 'btn-primary' : 'btn-default')}>2</div>
                                                            <p>Step 2</p>
                                                        </div>
                                                        <div className="stepwizard-step">
                                                            <div className={'btn btn-circle ' + (this.state.currentStep === 3 ? 'btn-primary' : 'btn-default')}>3</div>
                                                            <p>Step 3</p>
                                                        </div>
                                                        <div className="stepwizard-step">
                                                            <div className={'btn btn-circle ' + (this.state.currentStep === 4 ? 'btn-primary' : 'btn-default')}>4</div>
                                                            <p>Step 4</p>
                                                        </div>
                                                        <div className="stepwizard-step">
                                                            <div className={'btn btn-circle ' + (this.state.currentStep === 5 ? 'btn-primary' : 'btn-default')}>5</div>
                                                            <p>Step 5</p>
                                                        </div>
                                                    </div>//*<!--end stepwizard with no ids set-->*/
                                                    :
                                                    <div className="stepwizard-row">
                                                        <div className="stepwizard-step">
                                                            <div className={'btn btn-circle ' + (this.state.currentStep === 3 ? 'btn-primary' : 'btn-default')}>1</div>
                                                            <p>Step 1</p>
                                                        </div>
                                                        <div className="stepwizard-step">
                                                            <div className={'btn btn-circle ' + (this.state.currentStep === 4 ? 'btn-primary' : 'btn-default')}>2</div>
                                                            <p>Step 2</p>
                                                        </div>
                                                        <div className="stepwizard-step">
                                                            <div className={'btn btn-circle ' + (this.state.currentStep === 5 ? 'btn-primary' : 'btn-default')}>3</div>
                                                            <p>Step 3</p>
                                                        </div>
                                                    </div>///*<!--end stepwizard with ids set-->*/
                                            }
                                        </div>
                                        {/*<!--end stepwizard-->*/}

                                        <div className="steps col-sm-12">

                                            {
                                                this.state.currentStep === 1 &&
                                                <div id="step-1">
                                                    <div className="col-sm-5">
                                                        <div className="pull-right margin-top-50"><button className="btn btn-primary" onClick={() => { this.setState({ isAddFromContacts: false, currentStep: 2 }) }}>
                                                            Search search providers</button></div>
                                                    </div>
                                                    <div className="col-sm-2">
                                                        <div className="center-icon"><i className="fas fa-exchange-alt"></i></div>
                                                    </div>
                                                    <div className="col-sm-5">
                                                        <div className=" margin-top-50"><button className="btn btn-primary" onClick={() => { this.setState({ isAddFromContacts: true, currentStep: 2 }) }}>Add from my contacts</button></div>
                                                    </div>
                                                </div>
                                            }
                                            {/*<!--end step 1-->*/}

                                            {
                                                this.state.currentStep === 2 &&
                                                <div>
                                                    {this.state.isAddFromContacts ?
                                                        <div>
                                                            <table className="table table-bordered">
                                                                <thead>
                                                                    <tr>
                                                                        <td>Name</td>
                                                                        <td>Industry</td>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    {
                                                                        this.state.selectedSPs.map((c, i) => {
                                                                            return (
                                                                                <tr key={i}>
                                                                                    <td>
                                                                                        <div className="custom-checkbox">
                                                                                            <label>
                                                                                                <input type="checkbox" value="c.IsSelected" onChange={() => { c.IsSelected = !c.IsSelected; this.canChooseLevel(); }} /> <span className="label-text">{c.FirstName} {c.LastName}</span>
                                                                                            </label>
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>{c.ServiceProviderTypeDescription}</td>
                                                                                </tr>
                                                                            )
                                                                        })
                                                                    }
                                                                </tbody>
                                                            </table>
                                                            <button className="btn btn-primary" disabled={this.state.disabledChooseLevelBtn} onClick={() => { this.setState({ isAddFromContacts: true, currentStep: 3 }) }}>Choose preferred type</button>
                                                        </div>

                                                        :
                                                        <div className="panel panel-default">
                                                            <div className="default-template-page">
                                                                <SearchServiceProviderComponent addFromPreferred={true} />
                                                            </div>
                                                        </div>
                                                    }

                                                </div>
                                            }
                                            {/*<!--end step 2-->*/}

                                            {
                                                <div>

                                                    <div className="row row-step-3">
                                                        <div className="col-sm-4">
                                                            <button className={this.state.preferredServiceProviderLevel === 0 && this.state.currentStep == 5 ? "btn btn-primary" : "btn btn-secondary"} 
                                                            onClick={() => { this.preview(0) }}>
                                                                <i className="fas fa-industry"></i>
                                                                <hr />
                                                                Industry
                                                            </button>
                                                        </div>
                                                        <div className="col-sm-4">
                                                            <button className={this.state.preferredServiceProviderLevel === 1 && this.state.currentStep == 4 ? "btn btn-primary" : "btn btn-secondary"} 
                                                            onClick={() => { this.preview(1) }}>
                                                                <i className="fas fa-map-marked"></i>
                                                                <hr />
                                                                Location
                                                            </button>
                                                        </div>
                                                        <div className="col-sm-4">
                                                            <button className={this.state.preferredServiceProviderLevel === 2 && this.state.currentStep == 4 ? "btn btn-primary" : "btn btn-secondary"} 
                                                            onClick={() => { this.preview(2) }}>
                                                                <i className="fas fa-home"></i>
                                                                <hr />
                                                                Asset Type
                                                            </button>
                                                        </div>
                                                    </div>

                                                </div>
                                            }
                                            {/*<!--end step 3-->*/}

                                            {
                                                (this.state.currentStep === 4 && this.state.preferredServiceProviderLevel === 1) &&
                                                <div>
                                                    <h5>Please choose preferred location(s):</h5>
                                                    <table className="table table-bordered">
                                                        <tbody>
                                                            {
                                                                this.state.locations.map((location, ee) => {
                                                                    return (
                                                                        <tr key={ee}>
                                                                            <td>Country:
                                                                                <select className="form-control" value={location.country} onChange={(e) => { location.country = e.target.value; this.onLocationChanges() }}>
                                                                                    {
                                                                                        this.state.availableCountries.map((ac, ii) => {
                                                                                            return (
                                                                                                <option key={ii}>{ac}</option>
                                                                                            )
                                                                                        })
                                                                                    }
                                                                                </select>
                                                                            </td>
                                                                            <td>State:
                                                                                <select className="form-control" value={location.state} onChange={(e) => { location.state = e.target.value; this.onLocationChanges() }}>
                                                                                    {
                                                                                        this.state.availableStates.map((as, ii) => {
                                                                                            return (
                                                                                                <option key={ii} value={as.Name}>{as.Name}</option>
                                                                                            )
                                                                                        })
                                                                                    }
                                                                                </select>
                                                                            </td>
                                                                            <td><br /><button className="btn btn-danger" onClick={() => { this.removeThisLocation(location) }}>Remove</button></td>
                                                                        </tr>
                                                                    )
                                                                })
                                                            }
                                                            <tr>
                                                                <td colSpan="3"><button onClick={this.addNewLocation} className="btn btn-info">Add new</button></td>
                                                            </tr>
                                                        </tbody>

                                                    </table>

                                                    <button className="btn btn-primary" onClick={this.confirmPreview}>Next</button>
                                                </div>
                                            }

                                            {
                                                (this.state.currentStep === 4 && this.state.preferredServiceProviderLevel === 2) &&
                                                <div>
                                                    <h5>Please choose preferred asset type(s):</h5>
                                                    <table className="table table-bordered">
                                                        <tbody>
                                                            {
                                                                this.state.availableAssetTypes.map((type, aa) => {
                                                                    return (

                                                                        <tr key={aa}>
                                                                            <td>
                                                                                <div className="custom-checkbox">
                                                                                    <label>
                                                                                        <input type="checkbox" onChange={() => { type.IsSelected = !type.IsSelected }} value="type.IsSelected" /> <span className="label-text">{type.Name}</span>
                                                                                    </label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    )
                                                                })
                                                            }
                                                        </tbody>
                                                    </table>

                                                    <button className="btn btn-primary" onClick={this.confirmPreview}>Next</button>
                                                </div>

                                            }
                                            {/*<!--end step 4-->*/}


                                            {
                                                (this.state.currentStep === 5) &&
                                                <div>

                                                    <h5>Are sure you want to add the following service provider(s) as your preferred service provider(s):</h5>
                                                    {
                                                        (this.state.preferredServiceProviderLevel === 1) &&
                                                        <p>Selected locations: &nbsp;
                                                        {
                                                                this.state.locations.map((location, ii) => {
                                                                    return (
                                                                        <b key={ii} >{location.state} | </b>
                                                                    )
                                                                })
                                                            }

                                                        </p>
                                                    }
                                                    {
                                                        (this.state.preferredServiceProviderLevel === 2) &&
                                                        <p>Selected asset types: &nbsp;
                                                        {
                                                                //availableAssetTypes
                                                                this.state.availableAssetTypes.filter(x => { return x.IsSelected }).map((selectType, ii) => {
                                                                    return (
                                                                        <b key={ii} >{selectType.Name} | </b>
                                                                    )
                                                                })
                                                            }

                                                        </p>
                                                    }

                                                    <br />
                                                    <p>Selected service provider(s):</p>
                                                    <ul>
                                                        {
                                                            this.state.selectedSPs.filter(x => { return x.IsSelected; }).map((sp, ii) => {
                                                                return (
                                                                    <li key={ii}>
                                                                        <div className="pull-left" style={{ minWidth: '200px' }}><b>{sp.FullName}</b></div>
                                                                        {
                                                                            this.state.preferredServiceProviderLevel === 0 &&
                                                                            <div className="pull-left">
                                                                                <div>(Industry Type - {sp.ServiceProviderTypeDescription})</div>
                                                                            </div>
                                                                        }
                                                                    </li>
                                                                )
                                                            })
                                                        }
                                                    </ul>
                                                    <hr />
                                                    <button className="btn btn-primary" onClick={this.save}>Confirm</button>

                                                </div>
                                            }
                                            {/*<!--end step 5-->*/}


                                        </div>


                                        {/* Cancel and Previous*/}
                                        <div className="modal-footer">
                                            <button className="btn modal-default-button" onClick={this.closeModal}>Cancel</button>

                                            {
                                                ((this.state.newPreferredSPModalPresetIds.length === 0 && this.state.currentStep > 1 && !this.isLoading) ||
                                                    (this.state.newPreferredSPModalPresetIds.length > 0 && this.state.currentStep > 3 && !this.isLoading))

                                                &&

                                                <button className="margin-right10px btn btn-warning modal-default-button" onClick={() => {
                                                    if (this.state.preferredServiceProviderLevel === 0 && this.state.currentStep === 5)
                                                        this.setState({ currentStep: 3 }) //go back to step 3 if you chose industry level
                                                    else
                                                        this.setState({ currentStep: this.state.currentStep - 1 })
                                                }}>Previous</button>
                                            }

                                        </div>
                                    </div>
                                </div>
                        }

                    </Modal>
                }
            </React.Fragment>
        )
    }

}


