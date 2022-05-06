import React, { Component } from "react";
import { withGoogleMap, GoogleMap, Marker, InfoWindow } from "react-google-maps"
import { MarkerClusterer } from "react-google-maps/lib/components/addons/MarkerClusterer";
import { connect } from 'react-redux'

import { util } from "../../asset-search/assetSearchUtils";

//https://developers.google.com/maps/documentation/javascript/styling //<---- if you want to go crazy on styling your map...
const defaultMapOptions = {
    styles: [
        {
            "featureType": "all",
            
            "elementType": "labels.text",
            "stylers": [
                {
                    "visibility": "on"
                }
            ]
        },
        {
            "featureType": "poi",
            "elementType": "labels.icon",
            "stylers": [
                {
                    "visibility": "off"
                }
            ]
        }
    ]
};

//https://tomchentw.github.io/react-google-maps/
const AssetSearchMapComponent = withGoogleMap((props) => {
    return (<GoogleMap
        ref={props.googleMap}
        defaultZoom={10}
        defaultCenter={props.currentLatLng}
        defaultOptions={defaultMapOptions}
    >
        <MarkerClusterer
            onClick={props.onMarkerClustererClick}
            averageCenter
            enableRetinaIcons
            gridSize={60}
        >
            {
                props.assets.map((currentAsset, index) => {
                    return (
                        <Marker key={index}
                            onClick={() => { props.onToggleOpen(currentAsset.assetId) }}
                            position={{ lat: currentAsset.asset.Latitude, lng: currentAsset.asset.Longitude }}>
                            {
                                props.isOpen && currentAsset.assetId === props.targetId &&
                                <InfoWindow onCloseClick={props.closeInfoBox}
                                    options={{ maxWidth: 200 }}
                                >
                                    <div className="asset-window-wrapper">
                                        <a className="asset-window-link" href={'/DataPortal/ViewAsset/' + currentAsset.assetId}>
                                            <div className="asset-window-image-container">
                                                <img src={currentAsset.image} style={{ width: '100%' }} /><br/>
                                            </div>
                                        </a>
                                        <div className="asset-window-data-container">
                                            <div className="adp asset-window-data-p0">
                                                {currentAsset.assetName}
                                            </div>
                                            <div className="adp asset-window-data-p1">
                                                {currentAsset.pricingCMV}
                                            </div>
                                            <div className="adp asset-window-data-p2">
                                                SqFt: {currentAsset.squareFeet}
                                            </div>
                                        </div>
                                    </div>
                                </InfoWindow>
                            }
                        </Marker>
                    )
                })
            }
        </MarkerClusterer>
    </GoogleMap>)
}
)


class MapContainer extends Component {
    constructor(props) {
        super(props);

        this.state = {
            currentSearchId: null,
            mappedSearchResults: [],
            isLoading: true
        }
        this.googleMap = React.createRef();
        this.getCurrentLocation = this.getCurrentLocation.bind(this);
        this.onToggleOpen = this.onToggleOpen.bind(this);
    }

    componentDidMount() {
        this.getCurrentLocation();
    }

    componentDidUpdate() {
        if (this.props.searchResult != null &&
            this.state.currentSearchId != this.props.searchResult.SearchId) {

            const data = [];
            //copy the logic in assetSearchUtils.js #333
            const mappedResults = util.parseSearchResultAssets(this.props.searchResult.Assets);

            this.setState({
                currentSearchId: this.props.searchResult.SearchId,
                mappedSearchResults: mappedResults
            });

            if (mappedResults.length > 0) {
                const mapBounds = new google.maps.LatLngBounds();
                //just add markers position into mapBounds
                mappedResults.forEach((result) => {
                    mapBounds.extend({ lat: result.asset.Latitude, lng: result.asset.Longitude });
                });

                setTimeout(() => {
                    this.googleMap.current.fitBounds(mapBounds);
                }, 1000);
            }
        }
    }

    onMarkerClustererClick(marker) {

    }
    
    onToggleOpen(id) {
        this.setState({
            isOpen: true,
            targetId: id
        });
    }

    getCurrentLocation() {
        const currentLatLng = {
            lat: 37.0902,
            lng: -95.7129
        }

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                position => {
                    currentLatLng.lat = position.coords.latitude;
                    currentLatLng.lng = position.coords.longitude;

                    this.setState({
                        currentLatLng: {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude
                        },
                        isLoading: false
                    })

                }
            )
        }

        return currentLatLng;
    }

    render() {
        return (
            <>
                {this.state.isLoading === false &&
                    <div className="AssetSearch_MapWrap">
                        <AssetSearchMapComponent
                            googleMap={this.googleMap}
                            onMarkerClustererClick={this.onMarkerClustererClick}

                            onToggleOpen={this.onToggleOpen}
                            targetId={this.state.targetId}
                            isOpen={this.state.isOpen}
                            closeInfoBox={() => { this.setState({ isOpen: false }); }}

                            assets={this.state.mappedSearchResults}
                            currentLatLng={this.state.currentLatLng}
                            googleMapURL="https://maps.googleapis.com/maps/api/js?v=3.exp&libraries=geometry,drawing,places"
                            loadingElement={<div style={{ height: `100%` }} />}
                            containerElement={<div style={{ height: `400px` }} />}
                            mapElement={<div style={{ height: `100%` }} />}
                        />
                    </div>
                }
            </>
        );
    }
}


const mapStateToProps = state => ({
    currentSearchId: state.assetSearchReducer.assetSearch.currentSearchId,
    searchResult: state.assetSearchReducer.assetSearch.searchResult
});

export default connect(mapStateToProps)(MapContainer);