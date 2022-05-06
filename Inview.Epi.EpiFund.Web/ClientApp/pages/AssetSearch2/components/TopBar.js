import React, { Component } from "react";
import SearchBar from './SearchBar';
import {
    geocodeByAddress,
    geocodeByPlaceId,
    getLatLng,
} from 'react-places-autocomplete';
import { connect } from 'react-redux'
import { updateShowAdvanced, getOrUpdateCurrentSearchModel } from '../../../redux/actions/asset-search-action'
import { initState } from "../../../redux/reducers/mapReducers/assetSearchReducer";


class TopBar extends Component {
    constructor(props) {
        super(props);

        this.state = {
            currentSuggestion: [],
            showSuggestion: false
        }

        this.handleSearchBarChange = this.handleSearchBarChange.bind(this);
        this.clearAddress = this.clearAddress.bind(this);
        this.handleClickedSuggestion = this.handleClickedSuggestion.bind(this);
    }

    handleSearchBarChange(value) {
        if (value) {
            geocodeByAddress(value)
                .then(results => {
                    this.setState({ currentSuggestion: results });
                }).catch(e => {
                    console.log(e);
                });

            this.setState({ showSuggestion: true });
        }
        else {
            this.setState({ showSuggestion: false, currentSuggestion: [] });
        }
        this.props.updateSearchBar();
    }

    clearAddress() {
        this.props.searchModel.searchBarInput = "";
        this.props.getOrUpdateCurrentSearchModel(this.props.searchModel);

        this.setState({ showSuggestion: false, currentSuggestion: [] });
    }

    clearAllFields() {
        this.props.getOrUpdateCurrentSearchModel(null, null, true);
    }

    handleClickedSuggestion() {
        this.setState({ showSuggestion: false }, () => {
            this.props.updateSearchBar(this.state.currentSuggestion[0].formatted_address);
        })
    }

    render() {
        return (
            <div className="AssetSearch_TopBar animated fadeIn faster">
                <SearchBar showsuggestion={this.state.showSuggestion} currentsuggestion={this.state.currentSuggestion} clickedsuggestion={this.handleClickedSuggestion} />
                <div className="AssetSearch_Searchbar_button AssetSearch_GreyGradient AssetSearch_Xfont" onClick={this.clearAddress}>×</div>
                <div className="AssetSearch_Searchbar_button AssetSearch_OrangeGradient" onClick={() => this.clearAllFields()}>CLEAR</div>
                <div className="AssetSearch_Searchbar_button AssetSearch_BlueGradient" onClick={() => this.props.updateShowAdvanced(!this.props.showAdvanced)}>ADVANCED</div>
            </div>
        );
    }
}

const mapStateToProps = state => ({
    showAdvanced: state.assetSearchReducer.showAdvanced.showAdvanced,
    searchModel: state.assetSearchReducer.getOrUpdateCurrentSearchModel.searchModel,
});

export default connect(mapStateToProps, { updateShowAdvanced, getOrUpdateCurrentSearchModel })(TopBar);
