import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../assetSearchUtils';
const util = Util.util;
import * as Comm from '../comm';
const comm = Comm.comm;

import AdvancedSavedSearches from './AdvancedSavedSearches.js'
import * as AdvancedSearchFunctions from '../advancedSearchFunctions';

let e = React.createElement;

// This button will update the selected saved search
export class AdvancedSavedSearchesButton extends React.Component {
  constructor(props) {
    super(props);
    this.handleUpdateSearchClick = this.handleUpdateSearchClick.bind(this);
  }

  async handleUpdateSearchClick() {
    if (await util.isLoggedIn()) {
      let searchTitle;

      for (var i = 0; i < util.global.savedSearches.length; i++) {
        if (util.global.savedSearches[i].Id === util.global.savedSearchTabId) {
          searchTitle = util.global.savedSearches[i].Title;
        }
      }

      let searchCriteria = AdvancedSearchFunctions.getFilterValues();

      // Update the asset search with the new criteria
      var response = await comm.updateAssetSearch(util.global.savedSearchTabId, JSON.stringify(searchCriteria), searchTitle);

      if (response.success) {

        // Get the user's saved searches
        response = await comm.getSavedSearchesForUser();

        if (response && response.success) {

          // Update this info
          util.global.savedSearches = response.searches;

          // Render this info for the advanced search modal
          await util.drawAdvancedSavedSearches(AdvancedSavedSearches, AdvancedSavedSearchesButton);

          alert('Updated Saved Search');
        }
        else {
          alert('Error Updating Saved Search');
        }
      }
      else {
        alert('Error Updating Saved Search');
      }
    }
    else {
      alert('You must be logged in to do that');
    }
  }

  render() {
    return e('button', { className: 'btn btn-primary', type: 'button', onClick: this.handleUpdateSearchClick, style: { float: 'left' } }, 'Update Search');
  }
}

export default AdvancedSavedSearchesButton;
