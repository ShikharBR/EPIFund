import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../assetSearchUtils';
const util = Util.util;

import * as Comm from '../comm';
const comm = Comm.comm;

import AdvancedSavedSearch from './AdvancedSavedSearch.js'
import * as AdvancedSearchFunctions from '../advancedSearchFunctions';
let e = React.createElement;

// This will contain the list of saved search tabs populated within the advanced search modal
export class AdvancedSavedSearches extends React.Component {
  constructor(props) {
    super(props);
    this.handleIsActive = this.handleIsActive.bind(this);
    this.handleSetActiveTab = this.handleSetActiveTab.bind(this);
    this.state = {
      // If the user is coming from the manage saved searches page, get the search id
      // in the address bar and set the selectedTabId as the search id as default
      selectedTabId: util.getUrlParam('savedSearchId', 'empty')
    };
  }

  // True or false depending on whether or not the tab currently selected and the id sent are the same
  handleIsActive(id) {
    return this.state.selectedTabId === id;
  }

  // Gets a tab id and sets this id to be the selected tab, also updates the search filters
  handleSetActiveTab(selectedTabId, search) {

    let parsedObject;
    let obj;
    try {
      obj = JSON.parse(search.Json);
      parsedObject = true;
    }
    catch (err) {
      alert(err);
    }
    if (parsedObject) {
      // Update filter values
      AdvancedSearchFunctions.setFilterValues(obj);
    }

    // Update the global variable
    util.global.savedSearchTabId = selectedTabId;

    // Update the selected tab
    this.setState({
      selectedTabId
    });
  }

  render() {

    var savedSearchContent;

    // If this user has saved searches
    if (this.props.searches) {
      if (this.props.searches.length > 0) {
        // Create an object to hold all the things
        const advancedSavedSearches = [];
        this.props.searches.forEach((savedSearch) => {
          advancedSavedSearches.push(e(AdvancedSavedSearch, { search: savedSearch, key: savedSearch.Id }));
        })

        // Iterate through all the things and do more things to the things then create a thing that contains all the things
        var tabs = advancedSavedSearches.map(function (el, i) {
          // Creating an a element
          return e(AdvancedSavedSearch, {
            key: i,
            handleIsActive: this.handleIsActive(el.props.search.Id),
            onActiveTab: this.handleSetActiveTab.bind(this, el.props.search.Id, el.props.search),
            search: el.props.search,
          });
        }, this);

        // Populate saved searches tabs
        savedSearchContent = e('div', { className: 'list-group', 'role': 'tablist' }, tabs);
      }
      else {
        savedSearchContent = e('span', null, 'You have 0 saved searches');
      }
    }
    else {
      // Nothing to show if the user doesn't log in
      savedSearchContent = e('span', null, 'Login to view your saved searches');
    }

    return savedSearchContent;
  }
}

export default AdvancedSavedSearches;
