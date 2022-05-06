import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'

import { LoadingComponent } from '../../loading-component'

export class SetSampleAssetButton extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,
            assetTypes: [
                { "selectedAssetId": null, "id": "1", "name": "Retail", "assets": [] },
                { "selectedAssetId": null, "id": "2", "name": "Office", "assets": [] },
                { "selectedAssetId": null, "id": "7", "name": "Medical", "assets": [] },
                { "selectedAssetId": null, "id": "6", "name": "Fuel/Service", "assets": [] },
                { "selectedAssetId": null, "id": "4", "name": "Industrial", "assets": [] },
                { "selectedAssetId": null, "id": "3", "name": "MF", "assets": [] },
                { "selectedAssetId": null, "id": "5", "name": "MHP", "assets": [] },
                { "selectedAssetId": null, "id": "11", "name": "Hotel/Motel", "assets": [] },
                { "selectedAssetId": null, "id": "8", "name": "Mixed", "assets": [] },
                { "selectedAssetId": null, "id": "14", "name": "Mini", "assets": [] },
                { "selectedAssetId": null, "id": "15", "name": "Parking Garage", "assets": [] },
                { "selectedAssetId": null, "id": "13", "name": "Fractured Condos", "assets": [] },
                { "selectedAssetId": null, "id": "17", "name": "Land", "assets": [] },
                { "selectedAssetId": null, "id": "16", "name": "Notes", "assets": [] }
            ]
        };


        this.afterOpenModal = this.afterOpenModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.save = this.save.bind(this);
    }

    async refresh() {
        const self = this;

        for (var i = 0; i < self.state.assetTypes.length; i++) {
            //reset it...
            var currentType = self.state.assetTypes[i];
            var placeHolder = {
                "AssetId": 0,
                "AssetNumber": " -- ",
                "ProjectName": "Please select an asset - --"
            };
            currentType.selectedAssetId = placeHolder.AssetId;

            currentType.assets = [];
            currentType.assets.push(placeHolder);
        }


        //just call the api here
        const data = await epiService.getManageSampleAssetData();
        if (data != null) {

            for (var i = 0; i < data.length; i++) {
                var currentAsset = data[i];
                var foundList = self.state.assetTypes.filter(function (v, index) {
                    if (v.id == currentAsset.AssetType) {
                        return true;
                    }
                })[0];

                //Chi: in the old code, if an asset is "IsPaper", then it gets put in Asset Type 16 ("Secured CRE Paper" / Note)
                if (currentAsset.IsPaper == true) {
                    foundList = self.state.assetTypes.filter(function (v, index) {
                        if (v.id == "16") {
                            return true;
                        }
                    })[0];
                }

                if (foundList != null) {
                    foundList.assets.push(currentAsset);
                    if (currentAsset.IsSampleAsset == true) {
                        foundList.selectedAssetId = currentAsset.AssetId;
                    }
                }
            } 1
            self.setState({
                isLoading: false,
                assetTypes: self.state.assetTypes
            });
        }
    }

    afterOpenModal() {
        this.refresh();
    }

    closeModal() {
        this.setState({ isOpen: false, isLoading: true });
    }

    save() {
        var self = this;

        //pass in the whole list back
        var list = [];
        for (var i = 0; i < self.state.assetTypes.length; i++) {
            var currentType = $.extend({}, self.state.assetTypes[i]);

            //unset the assets list, no need to pass everything back
            currentType.assets = [];
            if (currentType.selectedAssetId == null || currentType.selectedAssetId == 0) {
                currentType.selectedAssetId = null;
            }

            list.push(currentType);
        }

        epiService.saveManageSampleAssetData(list).then(function (data) {
            notificationService.info("Updated sample asset(s).");
        });
    }

    render() {

        return (
            <React.Fragment>
                <button id="setSampleAssetsButton" onClick={() => { this.setState({ isOpen: true }) }} className="btn btn-primary btn-block">
                    Set Sample Assets
                </button>

                <Modal
                    isOpen={this.state.isOpen}
                    onAfterOpen={this.afterOpenModal}
                    onRequestClose={this.closeModal}
                    shouldCloseOnOverlayClick={true}
                    overlayClassName="modal-mask"
                    className="modal-wrapper"
                    closeTimeoutMS={200}>

                    <div className="modal-container modal-70">

                        <div className="modal-body">
                            {
                                this.state.isLoading ?
                                    <LoadingComponent /> :
                                    <div>
                                        {
                                            this.state.assetTypes.map((asset, iii) => {
                                                const self = this;
                                                return (
                                                    <div key={iii} className="row" style={{ marginBottom: '10px' }}>
                                                        <div className="col-md-3 col-lg-2">
                                                            {asset.name}
                                                        </div>
                                                        <div className="col-md-6 col-lg-7">
                                                            <select className="form-control" value={asset.selectedAssetId} onChange={(e) => {
                                                                asset.selectedAssetId = e.target.value;
                                                                this.setState({
                                                                    assetTypes: this.state.assetTypes
                                                                })
                                                            }}>
                                                                {
                                                                    asset.assets.map((item, ix) => {
                                                                        return (
                                                                            <option key={ix} onChange={() => { }} value={item.AssetId}>{item.AssetNumber} - {item.ProjectName}</option>
                                                                        )
                                                                    })

                                                                }
                                                            </select>
                                                        </div>
                                                        {
                                                            asset.selectedAssetId !== 0 ?
                                                                <div className="col-md-3 col-lg-3">
                                                                    <a target="_blank" href={'/DataPortal/ViewAsset/' + asset.selectedAssetId + '?fromManageAssets=True'} className="btn btn-primary pull-left">View Asset</a>
                                                                    <button className="btn" onClick={() => {
                                                                        asset.selectedAssetId = 0;
                                                                        this.setState({
                                                                            assetTypes: this.state.assetTypes
                                                                        })
                                                                    }}><span className="glyphicon glyphicon-trash"></span></button>
                                                                </div> :
                                                                <div className="col-md-3 col-lg-3 disabled" v-if="asset.selectedAssetId == 0">
                                                                    <button disabled className="btn btn-primary pull-left disabled">View Asset</button>
                                                                    <button disabled className="btn disabled" onClick={() => { this.reset(asset) }}><span className="glyphicon glyphicon-trash"></span></button>
                                                                </div>
                                                        }

                                                    </div>
                                                )

                                            })

                                        }
                                    </div>

                            }

                        </div>

                        <div className="modal-footer">
                            {
                                !this.state.isLoading &&
                                <button onClick={this.save} className="btn btn-success">Save changes</button>
                            }
                            <button onClick={this.closeModal} className="btn">Cancel</button>
                        </div>

                    </div>

                </Modal >

            </React.Fragment >
        )
    }
}

const element = document.getElementById("set-sample-asset-button");
if (element != null)
    ReactDOM.render(<SetSampleAssetButton />, element);
