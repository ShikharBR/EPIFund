import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../assetSearchUtils';
const util = Util.util;
import * as AdvancedSearchFunctions from '../advancedSearchFunctions';

import NestedCheckBoxListGroup from './InputElements/NestedListGroup/NestedCheckBoxListGroup.js';
import SavedSearches from './SavedSearches.js'
import DropdownButton from './InputElements/DropdownButton.js';
import SelectList from './InputElements/SelectList.js';

let e = React.createElement;

let qfAssetTypeClassMap = (item) => {
  switch (item.class) {
    case '1': return 'qs-asset-type';
    case '2': return 'qs-asset-sub-type'
  }
}

export class QuickFilter extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      isList: false,
      isFullScreen: false
    }

    this.handleQuickFilterSearch = this.handleQuickFilterSearch.bind(this);
    this.handleAdvancedFilterClick = this.onAdvancedFilterClick.bind(this);
    this.handleHome = this.handleHome.bind(this);
    this.handleToggleListClick = this.handleToggleListClick.bind(this);
    this.generatePriceTypeSelect = this.generatePriceTypeSelect.bind(this);
    this.handleToggleFullScreenClick = this.handleToggleFullScreenClick.bind(this);
    this.handleEnter = this.handleEnter.bind(this);
    this.handleToggleOneLineSummaries = this.handleToggleOneLineSummaries.bind(this);
    this.clearFiltersClick = this.clearFiltersClick.bind(this);
  }

  handleEnter(e) {
    if (e.key === "Enter") {
      this.handleQuickFilterSearch();
    }
  }

  handleQuickFilterSearch(e) { this.props.onSearch(e); }
  handleHome(e) { window.top.location = "/"; }
  onAdvancedFilterClick() {
    setTimeout(() => {
      // update push all values from quick search to advanced search
      document.getElementById('txtAdvCity').value = document.getElementById('txtCity').value;
      document.getElementById('txtAdvState').value = document.getElementById('txtState').value;
      document.getElementById('txtAdvCounty').value = document.getElementById('txtCounty').value;

      document.getElementById('txtAdvancedSearchMin').value = document.getElementById('txtQuickSearchMin').value;
      document.getElementById('txtAdvancedSearchMax').value = document.getElementById('txtQuickSearchMax').value;
      document.getElementById('txtCapRate').value = document.getElementById('txtQuickSearchCapRate').value;
      document.getElementById('txtYearBuilt').value = document.getElementById('txtQuickSearchYearBuilt').value;
      document.getElementById('txtAdvancedSearchAssetName').value = document.getElementById('txtQuickSearchAssetName').value;
      setTimeout(() => { util.copyCheckboxList('.qs-asset-sub-type', '.as-asset-sub-type', true) }, 0);;
      setTimeout(() => { util.copyCheckboxList('.qs-asset-type', '.as-asset-type', true) }, 1);


      AdvancedSearchFunctions.renderAdvancedSearch(util.global.flags.assetType);
      util.global.flags.assetType = false;
    }, 2000);

    $('#myModal').modal({})
  }
  generatePriceTypeSelect() {
    if (this.props.priceTypeEnum == null)
      return null;

    let priceTypeEnum = this.props.priceTypeEnum;
    let SelectOptions = [];

    Object.keys(this.props.priceTypeEnum).forEach(key => {
      SelectOptions.push({ label: priceTypeEnum[key].name, value: key });
    });


    return e(SelectList, {
      id: 'ddlPriceSearchType', options: SelectOptions, defaultValue: AdvancedSearchFunctions.filterFields['ddlPriceSearchType'].default,
      className: "generalinputstyle", onChange: () => this.props.onSearch()
    })
  }
  // When the user clicks on the toggle layout icon, switch to full screen map
  // with the one line summaries at the bottom
  handleToggleListClick(e) {
    this.setState({ isList: !this.state.isList });
    // Get the element with the asset cards
    var assetSearchCards = document.getElementById('asl');
    var assetSearchMap = document.getElementById('asm');
    var oneLineSummaries = document.getElementById('oneLineSummariesContainer');

    if (!this.state.isFullScreen) {
      var assetSearch = parent.document.getElementById('assetSearchIFrame');
      if (assetSearch != null) {
        assetSearch.style.position = "static";
        assetSearch.style.width = "850px";
        assetSearch.style.height = "1650px";
      }
    }

    // Hide the asset cards
    assetSearchCards.style.display = "none";

    // Show the one line summaries
    oneLineSummaries.style.display = "block";

    // Update map height to 60%
    assetSearchMap.classList.remove('asset-search-map-container');
    assetSearchMap.classList.remove('asset-search-map-container-only-one-line-summaries');
    assetSearchMap.classList.add('asset-search-map-container-one-line-summaries');
  }
  handleToggleOneLineSummaries(e) {
    // Get the element with the asset cards
    var assetSearchCards = document.getElementById('asl');
    var assetSearchMap = document.getElementById('asm');
    var oneLineSummaries = document.getElementById('oneLineSummariesContainer');

    // Hide the one line summaries
    oneLineSummaries.style.display = "none";

    // Show the asset cards
    assetSearchCards.style.display = "block";

    // Update map height to 100%
    assetSearchMap.classList.remove('asset-search-map-container-one-line-summaries');
    assetSearchMap.classList.remove('asset-search-map-container-only-one-line-summaries');
    assetSearchMap.classList.add('asset-search-map-container');
    if (!this.state.isFullScreen) {
      var assetSearch = parent.document.getElementById('assetSearchIFrame');
      assetSearch.style.position = "static";
      assetSearch.style.width = "850px";
      assetSearch.style.height = "950px";
    }
    if (assetSearch.style.position === "fixed") {
      var assetSearch = parent.document.getElementById('assetSearchIFrame');
      assetSearch.style.position = "static";
      assetSearch.style.width = "850px";
      assetSearch.style.height = "950px";
    }
  }
  handleToggleMap(e) {
    // Get the element with the asset cards
    var assetSearchCards = document.getElementById('asl');
    var assetSearchMap = document.getElementById('asm');
    var oneLineSummaries = document.getElementById('oneLineSummariesContainer');

    // Show the one line summaries
    oneLineSummaries.style.display = "block";

    // Hide the asset cards
    assetSearchCards.style.display = "none";

    // Update map height to 0%
    assetSearchMap.classList.remove('asset-search-map-container');
    assetSearchMap.classList.remove('asset-search-map-container-one-line-summaries');
    assetSearchMap.classList.add('asset-search-map-container-only-one-line-summaries');
  }
  handleToggleFullScreenClick(e) {
    this.setState({ isFullScreen: !this.state.isFullScreen });
    var assetSearch = parent.document.getElementById('assetSearchIFrame');

    // If we're in fullscreen mode
    if (assetSearch.style.position === "fixed") {

      // Revert the iframe back to normal
      assetSearch.style.position = "static";
      assetSearch.style.width = "850px";
      assetSearch.style.height = "1650px";

      // Hide the other elements by setting overflow to hidden
      parent.document.getElementById('main-body').style.overflow = "scroll";
    }
    else {
      // Set iframe to full screen view
      assetSearch.style.position = "fixed";
      assetSearch.style.width = "100vw";
      assetSearch.style.height = "100vh";
      assetSearch.style.top = "0";
      assetSearch.style.left = "0";
      assetSearch.style.zIndex = "9999";

      // Hide the other elements by setting overflow to hidden
      parent.document.getElementById('main-body').style.overflow = "hidden";
    }

    var assetSearchMap = document.getElementById('asm');
    if (assetSearchMap.classList.value === "asset-search-map-container" && this.state.isFullScreen) {
      assetSearch.style.height = "950px";
    }
  }
  clearFiltersClick(e) {
    setTimeout(() => AdvancedSearchFunctions.clearFilterValues(), 0);
    let promise = new Promise((resolve, reject) => {
      resolve(this.props.onSearch());
    }).then(() => {
      this.props.onSearch();
    })
  }

  componentDidMount() {
    this.handleToggleListClick();
  }

  render() {
    let flexstyle = {};
    if (this.state.isFullScreen) {
      flexstyle = { display: "flex", flexWrap: "wrap", justifyContent: "center" }
    }
    return e('div', { className: 'asset-search-quick-filters' },
      e('div', { style: flexstyle },

        // Top row
        e('div', { style: { display: 'flex', justifyContent: 'space-between' } },
          util.addQSLabel(e('input', { id: 'txtQuickSearchAssetName', placeholder: 'Asset Name', type: 'text', className: 'biginput', defaultValue: this.props.defaultValue, onBlur: () => this.props.onSearch(), onKeyDown: this.handleEnter }), 'Asset name'),
          util.addQSLabel(e('input', { type: 'text', className: 'biginput input-group', id: 'txtCity', placeholder: "City", onBlur: () => this.props.onSearch(), onKeyDown: this.handleEnter }), 'City'),
          util.addQSLabel(e('input', { type: 'text', className: 'biginput input-group', id: 'txtState', placeholder: "State", onBlur: () => this.props.onSearch(), onKeyDown: this.handleEnter }), 'State'),
          util.addQSLabel(e('input', { type: 'text', className: 'biginput input-group', id: 'txtCounty', placeholder: "County", onBlur: () => this.props.onSearch(), onKeyDown: this.handleEnter }), 'County')
        ),

        // Middle row
        e('div', { style: { display: 'flex', justifyContent: 'space-between' } },
          util.addQSLabel(e(DropdownButton, { label: 'Asset Type', id: 'qsAssetTypeDD', className: "generalinputstyle" }, e(NestedCheckBoxListGroup, { id: 'qs-asset-type-list', inputs: this.props.assetTypes, hasSelectAll: false, mapClass: qfAssetTypeClassMap, handleChange: () => this.props.onSearch() })), "Asset Type"),
          util.addQSLabel(e('input', { type: 'text', maxLength: 19, onKeyUp: util.onMoneyKeyUp, onChange: util.onMoneyChange, className: 'biginput input-group', id: 'txtQuickSearchMin', placeholder: "Minimum", defaultValue: this.props.Min, onBlur: () => this.props.onSearch(), onKeyDown: this.handleEnter }), "Min Price"),
          util.addQSLabel(e('input', { type: 'text', maxLength: 19, onKeyUp: util.onMoneyKeyUp, onChange: util.onMoneyChange, className: 'biginput input-group', id: 'txtQuickSearchMax', placeholder: "Maximum", defaultValue: this.props.Max, onBlur: () => this.props.onSearch(), onKeyDown: this.handleEnter }), "Max Price"),
          util.addQSLabel(this.generatePriceTypeSelect(), "CMV/LP"),
          util.addQSLabel(e('input', { type: 'text', className: 'smallinput', id: 'txtQuickSearchCapRate', placeholder: "Cap Rate", style: { marginLeft: "5px" }, onBlur: () => this.props.onSearch(), onKeyDown: this.handleEnter }), "Min Cap Rate %"),
          util.addQSLabel(e('input', { type: 'text', className: 'smallinput', id: 'txtQuickSearchYearBuilt', placeholder: "Year Built", onBlur: () => this.props.onSearch(), onKeyDown: this.handleEnter }), "Min Year Built"),
        ),

        // Last Row (saved searches and buttons)
        e('div', { style: { display: "flex", alignItems: "center", justifyContent: "center", marginTop: "5px" } },
          e('div', { style: { marginTop: "5px" } },
            e(SavedSearches, { searches: util.global.savedSearches })
          ),
          e('button', { id: "searchBtn", 'data-target': '#myModel', 'data-toggle': 'modal', 'type': 'button', 'className': 'btn btn-primary buttonstyle generalinputstyle_blue', onClick: this.handleQuickFilterSearch, style: { margin: "0px", marginLeft: "5px", height: "40px" } }, "Search"),
          e('button', { id: "advFilter", 'data-target': '#myModel', 'data-toggle': 'modal', 'type': 'button', 'className': 'btn btn-primary buttonstyle generalinputstyle_blue', onClick: this.handleAdvancedFilterClick, style: { margin: "0px", marginLeft: "5px", height: "40px" } }, "Advanced Filter"),
          e('button', { 'type': 'button', 'className': 'btn btn-danger buttonstyle generalinputstyle_red', onClick: this.clearFiltersClick, style: { margin: "0px", marginLeft: "5px", height: "40px" } }, "Clear Filters"),
          e('i', { id: "toggleOneLineSummariesIcon", className: 'fa fa-th-large fa-2x', style: { cursor: 'pointer', color: '#777', paddingLeft: '10px', fontSize: "36px" }, onClick: this.handleToggleOneLineSummaries }),
          e('i', { id: "toggleListIcon", className: 'fa fa-list-alt fa-2x', style: { cursor: 'pointer', color: '#777', paddingLeft: '10px', fontSize: "36px" }, onClick: this.handleToggleListClick }),
          e('i', { id: "toggleMapIcon", className: 'fa fa-align-justify fa-2x', style: { cursor: 'pointer', color: '#777', paddingLeft: '10px', fontSize: "36px" }, onClick: this.handleToggleMap }),
          e('i', { id: "toggleFullScreenIcon", className: '', style: { cursor: 'pointer', paddingLeft: '10px' }, onClick: this.handleToggleFullScreenClick }, (this.state.isFullScreen) ? e('img', { src: '/Content/images/minimize.png' }) : e('img', { src: '/Content/images/maximize.png' }))
        )
      )
    )
    //   util.addQSLabel(util.bsDropdown.generateDDCB(util.enum.assetType, 'Asset Type', 'qs-asset-type', 'ddcbQuickSearchAssetTypes', "inputstyle-dropdownmenu"), "Asset Type"),
  }
}

export default QuickFilter;
