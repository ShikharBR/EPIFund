import React, { Component } from "react";
import WhiteGlobe from '../img/whiteglobe.png'
import { helpers } from '../../../helpers/helpers'
import { connect } from 'react-redux'
import { getOrUpdateCurrentSearchModel } from '../../../redux/actions/asset-search-action'

class SavedSearch extends Component {
    constructor(props) {
        super(props);
    }

    formatDateTime(d) {
        //time is saved in UTC time
        return helpers.formatDateTime(d, true);
    }

    render() {
        return (
            <div className="AssetSearch_SavedSearchItemWrap" onClick={() => {
                this.props.getOrUpdateCurrentSearchModel(null, this.props.search);
            }}>
                <div>
                    <h5>{this.props.search.Title}</h5>
                    <h6>{this.formatDateTime(this.props.search.Updated)}</h6>
                </div>
                <div style={{ position: "relative" }}>
                    <img src={WhiteGlobe} alt="" />
                </div>
            </div>
        );
    }
}


const mapStateToProps = state => ({
    searchModel: state.assetSearchReducer.getOrUpdateCurrentSearchModel.searchModel
});

export default connect(mapStateToProps, { getOrUpdateCurrentSearchModel })(SavedSearch);
