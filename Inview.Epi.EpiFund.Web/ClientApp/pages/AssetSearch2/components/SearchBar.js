import React, { Component } from "react";
import SpyGlass from "../img/spyglass_icon.png";
import { connect } from 'react-redux'
import { getOrUpdateCurrentSearchModel } from '../../../redux/actions/asset-search-action'

class SearchBar extends Component {
    constructor(props) {
        super(props);

        this.updateSearchBarText = this.updateSearchBarText.bind(this);
    }

    updateSearchBarText(e) {
        this.props.searchModel.searchBarInput = e.target.value;
        this.props.getOrUpdateCurrentSearchModel(this.props.searchModel);
    }

    render() {
        return (
            <div className="AssetSearch_Searchbar_wrap">
                <img src={SpyGlass} alt="Search Spyglass" />
                <input type="text" className="AssetSearch_Searchbar" placeholder="Search by Address, Asset Name, or Asset ID" value={this.props.searchModel.searchBarInput} onChange={this.updateSearchBarText} />
                {
                    (this.props.showsuggestion && this.props.currentsuggestion[0]) &&
                    <div className="AssetSearch_Searchbar_addressmodal" onClick={() => this.props.clickedsuggestion()}>{this.props.currentsuggestion[0].formatted_address}</div>
                }
            </div>
        );
    }
}


const mapStateToProps = state => ({
    searchModel: state.assetSearchReducer.getOrUpdateCurrentSearchModel.searchModel,
});

export default connect(mapStateToProps, { getOrUpdateCurrentSearchModel })(SearchBar);