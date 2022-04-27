import React, { Component } from "react";
import WhiteCheck from "../img/whitecheck.png";
import { connect } from 'react-redux'
import { filterSavedSearches } from '../../../redux/actions/asset-search-action'

class SaveSearchInput extends Component {
    constructor(props) {
        super(props);

        this.state = {
            searchText: ""
        }

        this.updateSearchText = this.updateSearchText.bind(this);
        this.sendSearchText = this.sendSearchText.bind(this);
    }

    updateSearchText(e) {
        this.setState({searchText: e.target.value});
    }

    sendSearchText() {
        this.props.filterSavedSearches(this.state.searchText);
    }

    render() {
        return(
            <div className="AdvancedSearch_SaveSearchWrap">
                <input type="text" placeholder="Search Name..." value={this.state.searchText} onChange={this.updateSearchText} onKeyDown={(e) => {
                    if (e.key === "Enter")
                        this.sendSearchText()
                }} />

                <div className="AssetSearch_OrangeGradient AdvancedSearch_SaveSearchSaveButton"onClick={this.sendSearchText}>
                    <img alt="" src={WhiteCheck} />
                </div>
            </div>
        );
    }
}


const mapStateToProps = state => ({

});

export default connect(mapStateToProps, { filterSavedSearches })(SaveSearchInput);
