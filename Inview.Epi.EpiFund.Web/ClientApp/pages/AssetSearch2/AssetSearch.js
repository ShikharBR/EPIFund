import React, { Component } from "react";
import { connect } from 'react-redux'
import TopHalf from './components/TopHalf';
import BotHalf from './components/BotHalf';
import MapContainer from './components/MapContainer';
import AdvancedSearch from "./components/AdvancedSearch";

class AssetSearch extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        let wrapStyle = {}
        if (this.props.viewOptions.isFullScreen) {
            wrapStyle = { position: "fixed", top: "0", left: "0", width: "100vw", zIndex: "20", marginTop: "0px", marginLeft: "0px", overflow: "scroll", height: "100%" }
        }

        return (
            <div className="assetsearchwrap animated fadeIn faster" style={wrapStyle}>

                <div className={(this.props.showAdvanced ? "hidden" : null)}>
                    <TopHalf />
                    <MapContainer />
                    <BotHalf />
                </div>
                <div className={(!this.props.showAdvanced ? "hidden" : null)}>
                    <AdvancedSearch />
                </div>
            </div>
        );
    }
}



const mapStateToProps = state => ({
    showAdvanced: state.assetSearchReducer.showAdvanced.showAdvanced,
    viewOptions: state.assetSearchReducer.viewOptions.viewOptions
});

export default connect(mapStateToProps, {})(AssetSearch);

