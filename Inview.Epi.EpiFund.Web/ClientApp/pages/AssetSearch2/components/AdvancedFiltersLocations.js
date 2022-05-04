import React, { Component } from "react";
import WhiteXButton from '../img/whitexicon.png';
import WhitePlusButton from '../img/whiteplus.png';
import { getOrUpdateCurrentSearchModel } from '../../../redux/actions/asset-search-action'

import { connect } from 'react-redux'

class AdvancedFiltersLocations extends Component {
    constructor(props) {
        super(props);

        this.removeLocation = this.removeLocation.bind(this);
        this.addLocation = this.addLocation.bind(this);
        this.refresh = this.refresh.bind(this);
    }

    async removeLocation(index) {
        this.props.searchModel.locations.splice(index, 1);
        await this.refresh();
    }

    async addLocation() {
        this.props.searchModel.locations.push({ City: "", State: "", County: "" });
        await this.refresh();
    }

    async refresh() {
        await this.props.getOrUpdateCurrentSearchModel(this.props.searchModel);
    }

    render() {
        return (
            <>
                <div>
                    {this.props.searchModel.locations.map((item, index) => {
                        return <div key={index} className="AssetSearch_Displayflex" style={(index === 0) ? {} : { height: "55px" }}>

                            <div className="AssetSearch_BasicSearchInput_wrap" style={{ width: "208px" }}>
                                {index == 0 && "City"}
                                <input type="text" placeholder="City" value={item.city || ''} onChange={(e) => { item.city = e.target.value; this.refresh(); }} autoComplete="off" />
                            </div>
                            <div className="AssetSearch_BasicSearchInput_wrap" style={{ width: "99px" }}>
                                {index == 0 && "State"}
                                <input type="text" placeholder="State" maxLength="2" value={item.state || ''} onChange={(e) => { item.state = e.target.value; this.refresh(); }} autoComplete="off" />
                            </div>

                            <div className="AssetSearch_BasicSearchInput_wrap" style={{ width: "208px" }}>
                                {index == 0 && "County"}
                                <input type="text" placeholder="County" value={item.county || ''} onChange={(e) => { item.county = e.target.value; this.refresh(); }} autoComplete="off" />
                            </div>

                            <div className="AssetSearch_actbutton AssetSearch_OrangeGradient" style={(index === 0) ? { marginTop: "24px" } : {}} onClick={() => this.removeLocation(index)}>
                                <img alt="" src={WhiteXButton} />
                            </div>
                        </div>
                    })}
                    <div className="AssetSearch_Displayflex" style={{ justifyContent: "space-between", paddingleft: "5px", paddingRight: "4px" }}>
                        <div className="AssetSearch_NewLocationBox">Add another location to search for...</div>
                        <div className="AssetSearch_actbutton AssetSearch_BlueGradient" style={{ marginTop: "5px" }} onClick={this.addLocation}>
                            <img alt="" src={WhitePlusButton} />
                        </div>
                    </div>
                </div>
            </>
        );
    }
}


const mapStateToProps = state => ({
    searchModel: state.assetSearchReducer.getOrUpdateCurrentSearchModel.searchModel,
});

export default connect(mapStateToProps, { getOrUpdateCurrentSearchModel })(AdvancedFiltersLocations);
