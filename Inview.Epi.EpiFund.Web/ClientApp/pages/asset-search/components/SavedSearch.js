import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../assetSearchUtils';
const util = Util.util;
import * as Comm from '../comm';
const comm = Comm.comm;
import * as AdvancedSearchFunctions from '../advancedSearchFunctions';
import * as CookieChecker from '../AssetSearchCookieChecker';

import AssetSearch from '../assetSearch.js'
let e = React.createElement;

export class SavedSearch extends React.Component {
  constructor(props) {
    super(props);
    this.handleSavedSearchClick = this.onSavedSearchClick.bind(this);
  }
  async onSavedSearchClick() {
    let parsedObject;
    let obj;
    try {
      obj = JSON.parse(this.props.search.Json);
      parsedObject = true;
    } catch (err) {
      alert(err);
    }
    if (parsedObject) {
      AdvancedSearchFunctions.setFilterValues(obj);
      let response = await comm.getAssets(obj);
      util.global.assets = response.Assets;
      util.getStatsFromAssetSearch(response);
      util.clearMarkers();
      util.placeMarkers();

      // Give a seconds for markers to be placed
      setTimeout(() => {
        util.updateMapBoundaries();
        util.draw(AssetSearch, obj);
        util.reDrawOneLineSummaries();
      }, 1000);
    }
  }
  render() {
    return e('li', null,
      e('a', {
        className: 'as-ss',
        'data-id': this.props.search.Id,
        onClick: this.handleSavedSearchClick,
      }, this.props.search.Title));
  }
}

export default SavedSearch;