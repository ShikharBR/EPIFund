import React, { Component } from "react"
import ReactDOM from 'react-dom';

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'

import { LoadingComponent } from '../../loading-component'

class UserProfileComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,

            LicenseState: null,
            currentImageData: null,

            LicenseStates: [],
            availableServiceProviderTypes: [],
            availableStates: [],
            availableAssetTypes: []
        };

        this.refresh();


        this.save = this.save.bind(this);
        this.onChange = this.onChange.bind(this);
        this.onLicenseStateChange = this.onLicenseStateChange.bind(this);
        this.checkAllStates = this.checkAllStates.bind(this);
        this.checkAllTypes = this.checkAllTypes.bind(this);
    }

    componentDidMount() {
        const self = this;
        setTimeout(() => {
            $('#fileupload').fileupload({
                url: '/epis3/UpdateProfileImage',
                autoUpload: false,
                singleFileUploads: true,
                add: function (e, data) {
                    self.setState({
                        currentImageData: data
                    });

                    var tempImageUrl = URL.createObjectURL(event.target.files[0]);

                    const profile = self.state.currentUserProfile;
                   
                    profile.ProfileImageUrl = tempImageUrl;
                    self.setState({
                        currentUserProfile: profile
                    });

                    notificationService.info("Please click save changes to update profile.");
                },
                done: function (e, data) {
                    //do nothing
                }
            });
        }, 500);
    }

    async refresh() {
        const self = this;

        const profile = await epiService.getCurrentUserInfoAndProfile(true);
        if (profile.LicenseState == null)
            profile.LicenseState = "Select a state"

        this.setState({
            currentUserProfile: profile,
            isLoading: false,
            LicenseState: profile.LicenseState
        });




        //get available industry types
        epiService.getAvailableServiceProviderTypes().then(function (types) {
            $.each(types, function (i, e) {
                self.state.availableServiceProviderTypes.push({
                    name: e,
                    isSelected: true
                });
            });
        });

        //states
        epiService.getAvailableStates().then(function (types) {
            self.setState({
                LicenseStates: types
            });

            //check UserProfileAvailableLocation
            var allChecked = false;
            if (self.state.currentUserProfile.IsAllAvailableLocationsChecked) {
                allChecked = true;
            }

            const states = [];
            $.each(types, function (i, e) {
                if (allChecked) {
                    states.push({
                        name: e,
                        isSelected: true
                    });
                } else {
                    //find which one got checked
                    var found = self.state.currentUserProfile.AvailableLocations.filter(function (a, b) {
                        if (a.State == e.toUpperCase())
                            return a;
                    })[0];

                    states.push({
                        name: e,
                        isSelected: found == null ? false : true
                    });
                }
            });
            self.setState({ availableStates: states });
        });

        //asset types
        epiService.getAvailableAssetTypes().then(function (types) {
            //check UserProfileAvailableAssetType
            var allChecked = false;
            if (self.state.currentUserProfile.IsAllAvailableAssetTypesChecked) {
                allChecked = true;
            }

            const assetTypes = [];
            $.each(types, function (i, e) {
                if (allChecked) {
                    assetTypes.push({
                        name: e,
                        isSelected: true
                    });
                } else {
                    //find which one got checked
                    var found = self.state.currentUserProfile.AvailableAssetTypes.filter(function (a, b) {
                        if (a.AssetTypeDescription == e)
                            return assetTypes;
                    })[0];

                    assetTypes.push({
                        name: e,
                        isSelected: found == null ? false : true
                    });
                }
            });
            self.setState({ availableAssetTypes: assetTypes });
        });

        //bootstrap tooltip
        setTimeout(function () { $('[data-toggle="tooltip"]').tooltip(); }, 500);
    }

    save() {
        var self = this;

        //upload image if there is a new uploaded one
        if (self.state.currentImageData != null)
            self.state.currentImageData.submit();

        //save locations and types here....
        var selectedStates = self.state.availableStates.filter(function (e, i) {
            if (e.isSelected == true) { return e; }
        }).map(function (e, i) { return e.name; });

        var selectedAssetTypes = self.state.availableAssetTypes.filter(function (e, i) {
            if (e.isSelected == true) { return e; }
        }).map(function (e, i) { return e.name; });

        //if everything is checked... update IsAllAvailableLocationsChecked and IsAllAvailableAssetTypesChecked
        if (selectedStates.length == self.state.availableStates.length)
            self.state.currentUserProfile.IsAllAvailableLocationsChecked = true;
        else
            self.state.currentUserProfile.IsAllAvailableLocationsChecked = false;
        if (selectedAssetTypes.length == self.state.availableAssetTypes.length)
            self.state.currentUserProfile.IsAllAvailableAssetTypesChecked = true;
        else
            self.state.currentUserProfile.IsAllAvailableAssetTypesChecked = false;

        epiService.saveUserProfile(self.state.currentUserProfile,
            selectedStates,
            selectedAssetTypes).then(() => {
                notificationService.info("User profile updated");
            });
    }

    checkAllStates(check) {
        this.state.availableStates.forEach((x, i) => {
            x.isSelected = check
        });

        this.setState({
            availableStates: this.state.availableStates
        });
    }

    checkAllTypes(check) {
        this.state.availableAssetTypes.forEach((x, i) => {
            x.isSelected = check
        });

        this.setState({
            availableAssetTypes: this.state.availableAssetTypes
        });
    }

    onLicenseStateChange() {
        if (event.target.name == null)
            console.warn("Event input without name");

        const profile = this.state.currentUserProfile;
        profile[event.target.name] = event.target.value;

        this.setState({
            currentUserProfile: profile,
            LicenseState: event.target.value
        })
    }

    onChange() {
        if (event.target.name == null)
            console.warn("Event input without name");

        const profile = this.state.currentUserProfile;
        profile[event.target.name] = event.target.value;

        this.setState({
            currentUserProfile: profile
        })
    }

    render() {

        return (
            <React.Fragment>
                {
                    this.state.isLoading ?
                        <LoadingComponent />
                        :
                        <React.Fragment>
                            <div className="panel">
                                <div className="panel-body">

                                    {/*basic info*/}
                                    <div className="row">
                                        <div className="col-sm-4">
                                            <div className="profile-image">
                                                <img src={this.state.currentUserProfile.ProfileImageUrl} alt="" className="img-thumbnail" />
                                            </div>
                                            <hr />

                                            <span className="btn btn-primary fileinput-button btn-block">
                                                <i className="fas fa-image"></i>&nbsp;
                                                <span>Upload Profile Image</span>
                                                <input id="fileupload" type="file" name="files[]" />
                                            </span>
                                            <button id="uploadBtn" className="hidden"></button>

                                        </div>
                                        <div className="col-sm-8">
                                            <h3>{this.state.currentUserProfile.FullName}</h3>
                                            <div className="form-group">
                                                <label>Job TItle</label>
                                                <input className="form-control" placeholder="Job Title" name="JobTitle" onChange={this.onChange} value={this.state.currentUserProfile.JobTitle || ""} />
                                            </div>
                                            <hr />
                                            <h4>Public Contact Info</h4>
                                            <div className="row">
                                                <div className="col-lg-12">
                                                    <div className="col-lg-12">
                                                        <div className="form-horizontal">
                                                            <div className="form-group">
                                                                <label>Phone Number</label>
                                                                <input className="form-control" placeholder="Phone #" name="Phone" onChange={this.onChange} value={this.state.currentUserProfile.Phone || ""} />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    {/*social media*/}
                                    <div className="row social-media">
                                        <div className="col-sm-12">
                                            <h4>Social Media</h4>
                                            <div className="form-group">
                                                <div className="row">
                                                    <div className="col-sm-1">
                                                        <i className="fab fa-linkedin"></i>
                                                        <i className="fab fa-twitter"></i>
                                                        <i className="fab fa-facebook"></i>
                                                        <i className="fab fa-instagram"></i>
                                                    </div>
                                                    <div className="col-sm-11">
                                                        <input className="form-control" placeholder="http://" name="LinkedIn" onChange={this.onChange} value={this.state.currentUserProfile.LinkedIn || ""} />
                                                        <input className="form-control" placeholder="http://" name="Twiiter" onChange={this.onChange} value={this.state.currentUserProfile.Twiiter || ""} />
                                                        <input className="form-control" placeholder="http://" name="Facebook" onChange={this.onChange} value={this.state.currentUserProfile.Facebook || ""} />
                                                        <input className="form-control" placeholder="http://" name="Instagram" onChange={this.onChange} value={this.state.currentUserProfile.Instagram || ""} />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    {/*locations / asset types*/}
                                    {
                                        this.state.currentUserProfile.User.UserType === 14 &&
                                        <div className="row">
                                            <div className="col-sm-12">
                                                <p>Available Business Locations and Asset Types</p>
                                                <div className="panel-group default-accordion" id="search-sp-accordion" role="tablist" aria-multiselectable="true">
                                                    <div className="panel panel-default">
                                                        <div className="panel-heading">
                                                            <h4 className="panel-title">
                                                                <a className="collapsed" role="button" data-toggle="collapse" data-parent="#search-sp-accordion" data-target="#locations">
                                                                    Available Business Locations</a>
                                                            </h4>
                                                        </div>
                                                        <div id="locations" className="panel-collapse collapse">
                                                            <div className="panel-body">
                                                                <button className="btn btn-default" onClick={() => { this.checkAllStates(true) }}><i className="fas fa-check-square"></i> Check all</button>
                                                                <button className="btn btn-default" onClick={() => { this.checkAllStates(false) }}><i className="fas fa-ban"></i> Uncheck all</button>
                                                                <hr />

                                                                {
                                                                    this.state.availableStates.map((type, i) => {
                                                                        return (
                                                                            <div key={i} className="custom-checkbox" style={{ float: 'left', minWidth: '180px' }}>
                                                                                <label><input type="checkbox" checked={type.isSelected} onChange={() => { type.isSelected = !type.isSelected; this.setState({ availableStates: this.state.availableStates }) }} /> <span className="label-text">{type.name}</span></label>
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
                                                                    Available Business Asset Types</a>
                                                            </h4>
                                                        </div>
                                                        <div id="assetTypes" className="panel-collapse collapse">
                                                            <div className="panel-body">
                                                                <button className="btn btn-default" onClick={() => { this.checkAllTypes(true) }}><i className="fas fa-check-square"></i> Check all</button>
                                                                <button className="btn btn-default" onClick={() => { this.checkAllTypes(false) }}><i className="fas fa-ban"></i> Uncheck all</button>
                                                                <hr />

                                                                {
                                                                    this.state.availableAssetTypes.map((type, i) => {
                                                                        return (
                                                                            <div key={i} className="custom-checkbox" style={{ float: 'left', minWidth: '290px' }}>
                                                                                <label><input type="checkbox" checked={type.isSelected} onChange={() => { type.isSelected = !type.isSelected; this.setState({ availableAssetTypes: this.state.availableAssetTypes }) }}  /> <span className="label-text">{type.name}</span></label>
                                                                            </div>
                                                                        )
                                                                    })
                                                                }

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    }


                                    {/*biography*/}
                                    <div className="row">
                                        <div className="col-sm-12">
                                            <h4>Biography</h4>
                                            <textarea rows="10" className="form-control" name="Biography" onChange={this.onChange} value={this.state.currentUserProfile.Biography || ""}></textarea>
                                        </div>
                                    </div>

                                    {/*skills*/}
                                    <div className="row">
                                        <div className="col-sm-12">
                                            <h4>Specialized Skills</h4>
                                            <textarea rows="10" className="form-control" name="Skills" onChange={this.onChange} value={this.state.currentUserProfile.Skills || ""}></textarea>
                                        </div>
                                    </div>

                                    {/*license info*/}
                                    <div className="row">
                                        <div className="col-sm-12">
                                            <h4>License</h4>
                                        </div>
                                        <div className="col-sm-4">
                                            <label>Operating License Number</label>
                                            <input className="form-control" name="LicenseNumber" onChange={this.onChange} value={this.state.currentUserProfile.LicenseNumber || ""} />
                                        </div>
                                        <div className="col-sm-8">
                                            <label>Operating License State</label>
                                            {
                                                <select className="form-control" name="LicenseState" onChange={this.onLicenseStateChange} value={this.state.LicenseState}>
                                                    <option>Select a state</option>
                                                    {
                                                        this.state.LicenseStates.map((state, i) => {
                                                            return (
                                                                <option key={i} value={state}>{state}</option>
                                                            )
                                                        })
                                                    }
                                                </select>
                                            }
                                        </div>
                                        <div className="col-sm-12">
                                            <label>Operating License Description</label>
                                            <textarea className="form-control" rows="10" name="LicenseDescription" onChange={this.onChange} value={this.state.currentUserProfile.LicenseDescription || ""}></textarea>
                                        </div>
                                        <div className="col-sm-12">
                                            <label>Professional Organization Members</label>
                                            <textarea className="form-control" rows="10" name="ProfessionalOrganizationMembers" onChange={this.onChange} value={this.state.currentUserProfile.ProfessionalOrganizationMembers || ""}></textarea>
                                        </div>
                                        <div className="col-sm-12">
                                            <label>Client Relationships</label>
                                            <textarea className="form-control" rows="10" name="ClientRelationships" onChange={this.onChange} value={this.state.currentUserProfile.ClientRelationships || ""}></textarea>
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col-sm-12">
                                            <a href="/Home/MyUSCPage" className="btn btn-default pull-right">Cancel</a>
                                            <button className="btn btn-primary pull-right margin-right10px" onClick={this.save}>Save Changes</button>
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

export default UserProfileComponent

const element = document.getElementById("general-user-profile-app");
if (element != null)
    ReactDOM.render(<UserProfileComponent />, element);
