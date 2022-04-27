import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../assetSearchUtils';
const util = Util.util;
import * as Comm from '../comm';
const comm = Comm.comm;
import * as AdvancedSearchFunctions from '../advancedSearchFunctions';
import * as CookieChecker from '../AssetSearchCookieChecker';
import SavedSearch from './SavedSearch.js'

let e = React.createElement;

export class SavedSearches extends React.Component {
  constructor(props) {
    super(props);
    this.handleSaveSearchClick = this.onSaveSearchClick.bind(this);
  }
  async onSaveSearchClick() {
    // Get the latest list of saved searches
    let response = await comm.getSavedSearchesForUser();
    if (response && response.success) {
      util.global.savedSearches = response.searches;
    }
    if (await util.isLoggedIn()) {
      let el = document.getElementById('txtSaveSearch');
      if (util.stringHasValue(el.value)) {
        var duplicateTitleFound = false;
        var ssLowerCaseDeSpaced = el.value.toLowerCase();
        ssLowerCaseDeSpaced = ssLowerCaseDeSpaced.replace(/\s/g, ""); // Remove all spaces
        if (util.isAlphaNumeric(ssLowerCaseDeSpaced)) {
          var inputSavedSearchTitle = el.value.toLowerCase();
          inputSavedSearchTitle = inputSavedSearchTitle.trim(); // Trim whitespace on the sides
          inputSavedSearchTitle = inputSavedSearchTitle.replace(/\s{2,}/g, ' '); // Remove excess spacing on the inside
          for (var i = 0; i < util.global.savedSearches.length; i++) {
            var savedSearchTitle = util.global.savedSearches[i].Title.toLowerCase();
            if (inputSavedSearchTitle == savedSearchTitle) {
              duplicateTitleFound = true;
            }

          }
        }
        else {
          alert("Please enter a saved search name consisting of only letters and numbers");
          return;
        }

        if (!duplicateTitleFound) {
          var newSavedSearchTitle = el.value.trim();
          newSavedSearchTitle = newSavedSearchTitle.replace(/\s{2,}/g, ' ');
          // Have all filter values be consistent prior to saving
          document.getElementById('txtAdvancedSearchMin').value = document.getElementById('txtQuickSearchMin').value;
          document.getElementById('txtAdvancedSearchMax').value = document.getElementById('txtQuickSearchMax').value;
          document.getElementById('txtCapRate').value = document.getElementById('txtQuickSearchCapRate').value;
          document.getElementById('txtYearBuilt').value = document.getElementById('txtQuickSearchYearBuilt').value;
          document.getElementById('txtAdvancedSearchAssetName').value = document.getElementById('txtQuickSearchAssetName').value;
          setTimeout(() => { util.copyCheckboxList('.qs-asset-sub-type', '.as-asset-sub-type', true) }, 0);
          setTimeout(() => { util.copyCheckboxList('.qs-asset-type', '.as-asset-type', true) }, 1);
          setTimeout(async function () {
            let searchCriteria = AdvancedSearchFunctions.getFilterValues();
            var response = await comm.saveAssetSearch(JSON.stringify(searchCriteria), newSavedSearchTitle);
            if (response.success) {
              document.getElementById('savedSearchPromptText').innerHTML = "Search saved successfully!";
              $('#savedSearchPrompt').modal({});
            } else {
              document.getElementById('savedSearchPromptText').innerHTML = "There was a problem saving this search";
              $('#savedSearchPrompt').modal({});
            }
          }, 3);
        }
        else {
          document.getElementById('savedSearchPromptText').innerHTML = "Saved Search name already exists!";
          $('#savedSearchPrompt').modal({});
        }
      }
      else {
        //el.classList.add('null-field');
        el.style.border = '1px solid red';
        setTimeout(function () {
          //el.classList.remove('null-field');
          el.style.border = '';
        }, 1500);
      }
    } else {
      document.getElementById('savedSearchPromptText').innerHTML = 'Please ' + '<a id="savedSearchLoginLink" href="../Home/Login?savedSearchLogin=true" target="_parent">' + 'login' + '</a>' + ' to create a Saved Search';
      $('#savedSearchLoginLink').click((e) => {
        this.saveFilterValues();
      });
      $('#savedSearchPrompt').modal({});
    }
  }
  saveFilterValues() {
    // Have all filter values be consistent prior to saving
    document.getElementById('txtAdvancedSearchMin').value = document.getElementById('txtQuickSearchMin').value;
    document.getElementById('txtAdvancedSearchMax').value = document.getElementById('txtQuickSearchMax').value;
    document.getElementById('txtCapRate').value = document.getElementById('txtQuickSearchCapRate').value;
    document.getElementById('txtYearBuilt').value = document.getElementById('txtQuickSearchYearBuilt').value;
    document.getElementById('txtAdvancedSearchAssetName').value = document.getElementById('txtQuickSearchAssetName').value;
    setTimeout(() => { util.copyCheckboxList('.qs-asset-sub-type', '.as-asset-sub-type', true) }, 0);
    setTimeout(() => { util.copyCheckboxList('.qs-asset-type', '.as-asset-type', true) }, 1);
    setTimeout(async function () {
      let searchCriteria = AdvancedSearchFunctions.getFilterValues();
      CookieChecker.setCookieAssetSearch(JSON.stringify(searchCriteria));
      console.log("searchCriteria", searchCriteria);
    }, 3);
  }
  render() {
    // if the user has no saved searches, just render a button to save the search
    // if the user has searches saved, render a dropdown button list instead with the saved searches there and what not
    let savedSearchesDropdown;
    let savedSearches;
    const advancedSavedSearches = [];

    if (this.props.searches && this.props.searches.length > 0) {
      const searches = [];
      this.props.searches.forEach((savedSearch) => {
        searches.push(e(SavedSearch, { search: savedSearch, key: savedSearch.Id }));
      })

      savedSearches = e('ul', { className: 'dropdown-menu dropdown-menu-right' }, searches);
      savedSearchesDropdown = e('button', {
        className: 'btn btn-default dropdown-toggle',
        type: 'button',
        'data-toggle': 'dropdown',
        'aria-haspopup': 'true',
        'aria-expanded': 'false'
      }, e('span', { className: 'caret' }, null));
    }

    let saveSearchButton = e('button', { className: 'btn btn-default generalinputstyle_dark', type: 'button', onClick: this.handleSaveSearchClick }, 'Save Search');
    let saveSearchControls = e('div', { className: 'input-group-btn' }, saveSearchButton, savedSearchesDropdown, savedSearches);
    let saveSearchTextbox = e('input', { className: 'form-control null-field', type: 'text', id: "txtSaveSearch", placeholder: "Give your search a name" }, null);
    return e('div', { className: 'qf-save-search input-group' }, saveSearchTextbox, saveSearchControls);
  }
}

export default SavedSearches;
