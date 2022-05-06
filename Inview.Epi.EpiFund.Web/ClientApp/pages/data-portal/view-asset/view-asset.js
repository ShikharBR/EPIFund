import React, { Component } from "react"
import ReactDOM from 'react-dom';
import { Helmet, HelmetProvider } from 'react-helmet-async';

import * as ViewAssetService from '../../../services/viewAssetService';
const viewAssetService = ViewAssetService.viewAssetService;

class ViewAssetComponent extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isLoading: true,
            isSampleAssetView: false,

            //assetId is from url
            assetId: location.pathname.split("/")[location.pathname.split("/").length - 1],

            headerLoaded: false,
            imagesLoaded: false,
            calculatorLoaded: false,
            detailsLoaded: false,
            additionalInformationLoaded: false,
        };
    }

    componentDidMount() {
        const self = this;
        self.refresh();
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
            });

            //bootstrap tooltip
            setTimeout(function () { $('[data-toggle="tooltip"]').tooltip(); }, 500);

            return true;
        }
        return false;
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
            viewAssetService.getAsset(
                self.state.assetId
            ).then(function (data) {
                if (data != null) {
                    // load header
                    viewAssetService.getViewAssetPageHeader(
                        self.state.assetId,
                        self.state.isSampleAssetView
                    ).then(function (data) {
                        self.setState({
                            headerLoaded: true,
                            headerHtml: data
                        });

                        // details
                        viewAssetService.getViewAssetPageDetail(
                            self.state.assetId,
                            self.state.isSampleAssetView
                        ).then(function (data) {
                            self.setState({
                                detailsLoaded: true,
                                detailsHtml: data
                            });

                            // calucators
                            viewAssetService.getViewAssetPageCalculators(
                                self.state.assetId,
                                self.state.isSampleAssetView
                            ).then(function (data) {
                                self.setState({
                                    calculatorLoaded: true,
                                    calculatorsHtml: data
                                });


                                // additional information
                                viewAssetService.getViewAssetPageAdditionalInformation(
                                    self.state.assetId,
                                    self.state.isSampleAssetView
                                ).then(function (data) {
                                    self.setState({
                                        additionalInformationLoaded: true,
                                        informationHtml: data
                                    });

                                    // images
                                    viewAssetService.getViewAssetPageImagesAndVideos(
                                        self.state.assetId,
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

export default ViewAssetComponent

ReactDOM.render(<ViewAssetComponent />, document.getElementById("view-asset-app"));