import React, { Component } from "react";
import BasicSearchInput from './BSInput';
import { helpers } from '../../../helpers/helpers'
import { util } from '../../asset-search/assetSearchUtils'
import { toast } from 'react-toastify';
import { getSavedSearchesForUser } from '../../../redux/actions/asset-search-action'

import { connect } from 'react-redux'
import { doAssetSearch, saveSearch } from '../../../redux/actions/asset-search-action'

class BasicSearch extends Component {
    constructor(props) {
        super(props);

        this.saveSearch = this.saveSearch.bind(this);

        this.doSearch = helpers.debounce(async () => {
            const model = helpers.getSearchModel(this.props.searchModel);
            await this.props.doAssetSearch(model);
        }, 1000);
    }

    async saveSearch() {
        await this.props.saveSearch(this.props.searchModel);
        this.props.getSavedSearchesForUser();
    }

    componentDidUpdate() {
        //updating the saved search name shouldn't trigger a new search
        if (!this.props.searchModel.savedSearchTitleChanged && !this.props.showAdvanced) {
            this.doSearch();
        }
    }

    render() {
        return (
            <div className="AssetSearch_BasicSearch_wrap">
                {
                    this.props.searchModel.basicSearchArray.map((item, index) => {
                        if (!item.advonly) {
                            return <BasicSearchInput
                                key={index}
                                id={item.id} />
                        }
                    })
                }
                <div onClick={this.saveSearch} className="AssetSearch_Searchbar_button AssetSearch_GreyGradient AssetSearch_BasicSearch_Savebutton">SAVE</div>
            </div>
        );
    }
}

const mapStateToProps = state => ({
    searchModel: state.assetSearchReducer.getOrUpdateCurrentSearchModel.searchModel,
    showAdvanced: state.assetSearchReducer.showAdvanced.showAdvanced,
});

export default connect(mapStateToProps, { doAssetSearch, saveSearch, getSavedSearchesForUser })(BasicSearch);