import React, { Component } from "react";
import SavedSearch from "./SavedSearch";
import SaveSearchInput from "./SaveSearchInput";
import { LoadingComponent } from "../../../loading-component";
import { connect } from 'react-redux'
import { getSavedSearchesForUser } from '../../../redux/actions/asset-search-action'

class SavedSearches extends Component {
    constructor(props) {
        super(props);

        this.state = {
            savedSearches: [],
            isLoading: true,
            searchTerm: null
        }

        this.getSavedSearch = this.getSavedSearches.bind(this);
        this.removedSavedSearch = this.removedSavedSearch.bind(this);
    }

    async componentWillMount() {
        await this.props.getSavedSearchesForUser();
        await this.getSavedSearch();
    }

    async getSavedSearches() {
        //await this.props.getSavedSearchesForUser();
        const searches = this.props.savedSearches;
        for (var i = 0; i < searches.length; i++) {
            //saved search is a json string within an object...
            const currentSearch = searches[i];
            currentSearch.JsonObject = JSON.parse(currentSearch.Json);
        }

        const sorted = searches.sort((a, b) => {
            if (a.Title.toLowerCase().trim() < b.Title.toLowerCase().trim()) { return -1; }
            if (a.Title.toLowerCase().trim() > b.Title.toLowerCase().trim()) { return 1; }
            return 0;
        });

        this.setState({
            savedSearches: this.props.savedSearches,
            allSavedSearches: sorted,
            filteredSavedSearches: sorted,
            isLoading: false
        });
    }

    async componentDidUpdate() {

        //if there is a new search term
        if (this.state.searchTerm !== this.props.filterSavedSearchesSearchTerm) {
            this.setState({ searchTerm: this.props.filterSavedSearchesSearchTerm });

            const searchTerm = this.props.filterSavedSearchesSearchTerm;
            if (searchTerm == null || searchTerm === "") {
                this.setState({
                    filteredSavedSearches: this.state.allSavedSearches
                });
            } else {
                const filtered = this.state.allSavedSearches.filter(x => {
                    return x.Title.toLowerCase().trim().indexOf(searchTerm.toLowerCase().trim()) > -1;
                });

                this.setState({
                    filteredSavedSearches: filtered
                });
            }
        }

        //if the list is different
        if (this.state.savedSearches !== this.props.savedSearches) {
            await this.getSavedSearch();
        }

    }

    removedSavedSearch(search) {
        ////pass in your search id here...
        //savedSearchService.removeSavedSearch(search.Id).then(r => {
        //    debugger;
        //    //do your things
        //});
    }

    render() {
        return (
            <div className="AdvancedSearch_SavedSearchesBar">
                <h1>SAVED SEARCHES</h1>

                {
                    this.state.isLoading ?

                        <div style={{ background: "white", opacity: "0.7" }}>
                            <LoadingComponent />
                        </div> :

                        this.state.filteredSavedSearches.map((item, index) => {
                            return <SavedSearch key={index} search={item} />
                        })
                }

                <h1>SAVE SEARCH</h1>

                <SaveSearchInput />

            </div>
        );
    }
}

const mapStateToProps = state => ({
    savedSearches: state.assetSearchReducer.getSavedSearchesForUser.savedSearches || [],
    filterSavedSearchesSearchTerm: state.assetSearchReducer.filterSavedSearches.searchTerm,
});

export default connect(mapStateToProps, { getSavedSearchesForUser })(SavedSearches);