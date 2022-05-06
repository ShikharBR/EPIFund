import React, { Component } from "react"
import ReactDOM from 'react-dom';
import { Helmet, HelmetProvider } from 'react-helmet-async';

import Button from 'react-bootstrap/Button';

import * as ViewAssetService from '../../../services/viewAssetService';
const viewAssetService = ViewAssetService.viewAssetService;

class ViewSampleAssetComponent extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isLoading: true,
            isSampleAssetView: true,

            selectedAssetType: {
                id: "",
                name: ""
            },
            availableSampleAssetTypes: [],

            headerLoaded: false,
            imagesLoaded: false,
            calculatorLoaded: false,
            detailsLoaded: false,
            additionalInformationLoaded: false,
        };

        this.handleSelectedAssetTypeChange = this.handleSelectedAssetTypeChange.bind(this);
    }

    componentDidMount() {
        const self = this;
        viewAssetService.getAvailableSampleAssetTypes().then(function (result) {
            let sampleAsset;
            if (result != null && result.length) {
                const mf = result.find((r, i) => {
                    if (r.id === 3) { return r }
                });
                if (mf != null) {
                    //use the first one
                    sampleAsset = mf;
                } else {
                    //use mf
                    sampleAsset = result[0];
                }

                self.setState({
                    selectedAssetType: {
                        id: sampleAsset.id,
                        name: sampleAsset.name,
                        assetId: sampleAsset.assetId
                    }
                });

                self.setState({
                    availableSampleAssetTypes: result
                });

                self.refresh();
            }
        });
    }

    isPageLoad() {
        var self = this;

        if (self.state.headerLoaded &&
            self.state.imagesLoaded &&
            self.state.calculatorLoaded &&
            self.state.detailsLoaded &&
            self.state.additionalInformationLoaded) {
            self.setState({
                isLoading: false
            })

            return true;
        }
        return false;
    }

    handleSelectedAssetTypeChange(event) {
        const selectType = this.state.availableSampleAssetTypes.find((x) => {
            if (x.id == event.target.value)
                return x;
        });

        if (selectType != null) {
            this.setState({ selectedAssetType: selectType });
            this.refresh();
        }
    }

    refresh() {
        const self = this;

        self.setState({
            isLoading: true,
            headerLoaded: false,
            imagesLoaded: false,
            calculatorLoaded: false,
            detailsLoaded: false,
            additionalInformationLoaded: false,
        });

        setTimeout(function () {
            //get asset
            viewAssetService.getSampleAsset(
                self.state.selectedAssetType
            ).then(function (data) {
                if (data != null) {
                    // load header
                    viewAssetService.getViewAssetPageHeader(
                        self.state.selectedAssetType.assetId,
                        self.state.isSampleAssetView
                    ).then(function (data) {
                        self.setState({
                            headerLoaded: true,
                            headerHtml: data
                        });

                        // details
                        viewAssetService.getViewAssetPageDetail(
                            self.state.selectedAssetType.assetId,
                            self.state.isSampleAssetView
                        ).then(function (data) {
                            self.setState({
                                detailsLoaded: true,
                                detailsHtml: data
                            });

                            // calucators
                            viewAssetService.getViewAssetPageCalculators(
                                self.state.selectedAssetType.assetId,
                                self.state.isSampleAssetView
                            ).then(function (data) {
                                self.setState({
                                    calculatorLoaded: true,
                                    calculatorsHtml: data
                                });


                                // additional information
                                viewAssetService.getViewAssetPageAdditionalInformation(
                                    self.state.selectedAssetType.assetId,
                                    self.state.isSampleAssetView
                                ).then(function (data) {
                                    self.setState({
                                        additionalInformationLoaded: true,
                                        informationHtml: data
                                    });

                                    // images
                                    viewAssetService.getViewAssetPageImagesAndVideos(
                                        self.state.selectedAssetType.assetId,
                                        self.state.isSampleAssetView
                                    ).then(function (data) {
                                        self.setState({
                                            imagesLoaded: true,
                                            imageHtml: data
                                        });

                                        self.isPageLoad();
                                    });
                                });
                            });
                        });
                    });
                }
            });
        }, 1000);
    }

    render() {

        const select = this.state.availableSampleAssetTypes;


        return (
            <React.Fragment>
                <div className="view-asset-page">

                    {
                        this.state.isLoading ?
                            <div className="alert alert-info">
                                Loading Asset...
                            </div> :
                            <HelmetProvider>
                                <Helmet>
                                    <script src="/Scripts/AssetCalcs.js"></script>
                                </Helmet>
                                {this.state.availableSampleAssetTypes.length > 0 &&
                                    <div className="row assetTypeDropdown">
                                        <div className="col-sm-9"></div>
                                        <div className="col-sm-3">
                                            <div className="alert alert-info sample-dropdown">
                                                <label>Sample Asset Type:</label>
                                                <select className="form-control" value={this.state.selectedAssetType.id} onChange={this.handleSelectedAssetTypeChange}>
                                                    {
                                                        this.state.availableSampleAssetTypes.map((type, index) => {
                                                            return <option key={type.id} value={type.id}>{type.name}</option>
                                                        })
                                                    }
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                }
                            </HelmetProvider>
                    }

                    {
                        !this.state.headerLoaded ?
                            <div className="ph-item">
                                <div className="ph-col-12">
                                    <div className="ph-row">
                                        <div className="ph-col-12 big"></div>
                                        <div className="ph-col-4 big"></div>
                                        <div className="ph-col-8 empty"></div>
                                        <div className="ph-col-2 "></div>
                                        <div className="ph-col-8 empty"></div>
                                    </div>
                                </div>
                            </div> :
                            <div>
                                <div className="view-asset-header" dangerouslySetInnerHTML={{ __html: this.state.headerHtml }}>

                                </div>
                            </div>
                    }

                    {
                        !this.state.imagesLoaded ?
                            <div className="row">
                                <div className="col-sm-12">
                                    <div className="ph-item">
                                        <div className="col-sm-4"><div className="ph-picture"></div></div>
                                        <div className="col-sm-4"><div className="ph-picture"></div></div>
                                        <div className="col-sm-4"><div className="ph-picture"></div></div>
                                    </div>
                                </div>
                            </div> :
                            <div v-show="imagesLoaded">
                                <div className="view-asset-images-videos" dangerouslySetInnerHTML={{ __html: this.state.imageHtml }}>

                                </div>
                            </div>
                    }

                    {
                        !this.state.detailsLoaded ?
                            <div className="">
                                <div className="row">
                                    <div className="col-sm-6">
                                        <div className="ph-item">
                                            <div className="ph-col-12">
                                                <div className="ph-row">
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="col-sm-6">
                                        <div className="ph-item">
                                            <div className="ph-col-12">
                                                <div className="ph-row">
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                    <div className="ph-col-6 big"></div>
                                                    <div className="ph-col-4 empty big"></div>
                                                    <div className="ph-col-2 big"></div>
                                                    <div className="ph-col-4"></div>
                                                    <div className="ph-col-8 empty"></div>
                                                    <div className="ph-col-6"></div>
                                                    <div className="ph-col-6 empty"></div>
                                                    <div className="ph-col-12"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div> :
                            <div>
                                <div className="view-asset-details" dangerouslySetInnerHTML={{ __html: this.state.detailsHtml }}>

                                </div>
                            </div>
                    }

                    {
                        !this.state.calculatorLoaded ?
                            <div className="ph-item">
                                <div className="ph-col-12">
                                    <div className="ph-row">
                                        <div className="ph-col-12 big"></div>
                                        <div className="ph-col-4 big"></div>
                                        <div className="ph-col-8 empty"></div>
                                        <div className="ph-col-2 "></div>
                                        <div className="ph-col-8 empty"></div>
                                    </div>
                                </div>
                            </div> :
                            <div>
                                <div className="view-asset-calculators" dangerouslySetInnerHTML={{ __html: this.state.calculatorsHtml }}></div>
                            </div>
                    }

                    {
                        !this.state.additionalInformationLoaded ?
                            <div className="ph-item">
                                <div className="ph-col-12">
                                    <div className="ph-row">
                                        <div className="ph-col-12 big"></div>
                                        <div className="ph-col-4 big"></div>
                                        <div className="ph-col-8 empty"></div>
                                        <div className="ph-col-2 "></div>
                                        <div className="ph-col-8 empty"></div>
                                    </div>
                                </div>
                            </div> :
                            <div>
                                <div className="view-asset-additional-information" dangerouslySetInnerHTML={{ __html: this.state.informationHtml }}></div>
                            </div>
                    }

                </div>
            </React.Fragment>
        )
    }
}

export default ViewSampleAssetComponent

ReactDOM.render(<ViewSampleAssetComponent />, document.getElementById("view-asset-app"));