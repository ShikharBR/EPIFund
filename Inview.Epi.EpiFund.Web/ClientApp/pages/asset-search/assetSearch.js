import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from './assetSearchUtils';
const util = Util.util;
import * as Comm from './comm';
const comm = Comm.comm;
import * as CookieChecker from './AssetSearchCookieChecker';
import * as AutoComplete from './AutoComplete';
import * as AdvancedSearchFunctions from './advancedSearchFunctions';

'use strict'
import QuickFilter from './components/QuickFilter.js';
import AssetSearchResultStats from './components/AssetSearchResultStats.js';
import AssetList from './components/AssetList.js'
import AdvancedSavedSearches from './components/AdvancedSavedSearches.js';
import AdvancedSavedSearchesButton from './components/AdvancedSavedSearchesButton.js';
import { OneLineSummariesComponent } from './components/oneLineSummaries';

//import { Button } from 'react-bootstrap';
/*
  - (BUG) its possible to encounter a state where the pin wont stop bouncing, ran into it once. Fortify asset hover events
*/
const e = React.createElement;

var assetType = [];
let atEnum = util.enum.assetType;

let mapSubtypes = (subType) => {
  return {
    label: util.enum.assetSubType[subType].name,
    class: '2',
    'data-id': subType
  }
};
Object.keys(atEnum).forEach((key, index) => {
  assetType.push({
    label: atEnum[key].name,
    class: '1',
    'data-id': key,
    inputs: atEnum[key].subTypes ? atEnum[key].subTypes.map(mapSubtypes) : null
  });
});



class AssetSearch extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      locationText: '',
      assets: [],
      scrollToAssetId: undefined,
    };
    this.handleQuickFilterSearch = this.handleQuickFilterSearch.bind(this);
    this.handleScroll = this.handleScroll.bind(this);
    window.scrollToAsset = this.scrollToAsset.bind(this);
  }
  componentDidMount() {
    AutoComplete.populateAutocomplete();
    if (!util.global.executedFirstRunReactComponentLogic) {
      util.global.executedFirstRunReactComponentLogic = true;
      AdvancedSearchFunctions.setFilterValues(util.global.latestSearchCriteria);

      // document.getElementById('summary').addEventListener('click', () => {
      //   var assetSummaryButton = document.getElementById('summary');
      //   assetSummaryButton.classList.toggle('active');
      //   var assetSummaryContent = assetSummaryButton.nextElementSibling;
      //   if (assetSummaryContent.style.maxHeight) {
      //     assetSummaryContent.style.maxHeight = null;
      //     document.getElementById('button-control').classList.remove('down');
      //     document.getElementById('button-control').classList.add('up');
      //   } else {
      //     assetSummaryContent.style.maxHeight = assetSummaryContent.scrollHeight + "px";
      //     document.getElementById('button-control').classList.remove('up');
      //     document.getElementById('button-control').classList.add('down');
      //   }
      // });

      // activate asset summary content immediately

      // var assetSummaryButton = document.getElementById('summary');
      // assetSummaryButton.classList.add('active');
      // var assetSummaryContent = assetSummaryButton.nextElementSibling;
      // assetSummaryContent.style.maxHeight = assetSummaryContent.scrollHeight + "px";

    }
  }
  componentDidUpdate() {
    // this event is often called rapidly, only allow one timeout at a time
    if (util.global.componentUpdatedTimeoutId) {
      window.clearTimeout(util.global.componentUpdatedTimeoutId);
      util.global.componentUpdatedTimeoutId = undefined;
    }
    // extemely loosely clear the map update flag when the controls are updated
    util.global.componentUpdatedTimeoutId = setTimeout(function () {
      util.global.flags.ignoreMapUpate = false;
    }, 1000);
    // map pin click logic
    if (this.state.scrollToAssetId) {
      util.scrollToElement(document.getElementById(`A${this.state.scrollToAssetId}`))
      this.state.scrollToAssetId = undefined;
    }
  }
  handleQuickFilterSearch(e) {
    /* 4/11/2020 Chi
    Have to comment this out because in QuickFilter.js, this and the backend gets hit on "OnKeyDown" and "onBlur" on Asset Name, Location, Prices, etc.
    The map keeps refreshing and it bugs out.     

    Only allow to search when search button is clicked
    */
    if (e != null && e.target.id === "searchBtn")
      this.search('quick');
  }
  handleAdvancedFilterSearch() { this.search('advanced'); }
  handleScroll(detail, view) {
    // im not sure how to get the current scroll position without jquery *accurately*, forgive me

    // disabling scroll data logs for now, working as expected, no need to debug atm
    /*console.log('<handleScroll> scroll data: ', {
        iHeight: $('.asset-search-asset-list-wrapper').innerHeight(),
        oHeight: $('.asset-search-asset-list-wrapper').outerHeight(),
        sHeight: $('.asset-search-asset-list-wrapper')[0].scrollHeight,
        height: $('.asset-search-asset-list-wrapper').height(),
        currentLoc: $('.asset-search-asset-list-wrapper').scrollTop()
    });*/
    // how about we add more assets when they hit the approximate middle
    // the approximate middle is scrollheight - height/inner/outer(going to use height)
    if ($('.asset-search-asset-list-wrapper').scrollTop() >= ($('.asset-search-asset-list-wrapper')[0].scrollHeight - $('.asset-search-asset-list-wrapper').height()) / 2) {
      //console.log('<handleScroll> past half');
      if (!util.global.appendingAssetsToList) {
        util.global.appendingAssetsToList = true;
        let newAssetCount = this.state.assets.length + util.global.assetDisplayCount;
        this.setState({
          assets: util.global.assets.slice(0, newAssetCount)
        })
        setTimeout(() => { util.global.appendingAssetsToList = false; }, 500);
      }
    }
  }
  scrollToAsset(assetId) {
    let matchingIndex;
    util.global.assets.forEach((a, i) => {
      if (a.AssetId === assetId) { matchingIndex = i; }
    });
    if (matchingIndex != undefined) {
      let transplantedAsset = util.global.assets.splice(matchingIndex, 1);
      util.global.assets.splice(this.state.assets.length, 0, transplantedAsset[0]);

      let newAssetCount = this.state.assets.length + util.global.assetDisplayCount;
      this.setState({
        scrollToAssetId: assetId,
        assets: util.global.assets.slice(0, newAssetCount)
      });
    }
  }
  async search(type) {
    let searchCriteria;
    // dont recognize any user interactions on the map while we are processing their last search request
    util.global.flags.ignoreMapUpate = true;
    if (type === 'advanced') {
      searchCriteria = AdvancedSearchFunctions.getFilterValues();
      // set the quick search cap rate from advanced search parameter
      document.getElementById('txtQuickSearchCapRate').value = document.getElementById('txtCapRate').value;
      document.getElementById('txtQuickSearchYearBuilt').value = document.getElementById('txtYearBuilt').value;
      document.getElementById('txtQuickSearchAssetName').value = document.getElementById('txtAdvancedSearchAssetName').value;
      setTimeout(() => { util.copyCheckboxList('.as-asset-sub-type', '.qs-asset-sub-type', true) }, 0);;
      setTimeout(() => { util.copyCheckboxList('.as-asset-type', '.qs-asset-type', true) }, 1);
    } else {
      // assume quick
      searchCriteria = AdvancedSearchFunctions.getFilterValues(true);
    }
    searchCriteria.city = document.getElementById('txtCity').value;
    searchCriteria.state = document.getElementById('txtState').value;
    searchCriteria.county = document.getElementById('txtCounty').value;

    searchCriteria.from = 'search';
    let parsedLoc = util.parseLocation(searchCriteria.location);
    if (parsedLoc) {
      searchCriteria.Locations = [
        {
          city: parsedLoc.city,
          county: parsedLoc.county,
          state: util.convertState(parsedLoc.state),
          zip: parsedLoc.zip
        }
      ];
    }
    let response = await comm.getAssets(searchCriteria);
    util.global.assets = response.Assets;
    util.getStatsFromAssetSearch(response);

    // clear markers
    util.clearMarkers();
    util.placeMarkers(function () {
      util.updateMapBoundaries();
      util.draw(AssetSearch, searchCriteria);
      util.reDrawOneLineSummaries();
    });
  }
  render() {
    if (!util.global.appendingAssetsToList && !this.state.scrollToAssetId) {
      this.state.assets = this.props.assets;
      // scroll to top
      let assetList = document.getElementById('asl');
      if (assetList) assetList.scrollTo(0, 0);
    }
    //else this.state.assets = this.state.assets.concat(this.prop.assets);

    if (!util.global.executedFirstRunAssetSearchLogic) {
      // bind advanced search event to window
      window.advancedSearch = this.handleAdvancedFilterSearch.bind(this);
      // now that the event is publicly available, bind it to the desired element
      document.getElementById('btnAdvancedSearch').addEventListener('onclick', window.advancedSearch);
      util.runOnLoad();
    }
    return e('div', { className: 'asset-search-container' },
      e(AssetSearchResultStats, {
        totalAssets: util.global.dbTotal,
        publishedAssets: util.global.publishedAssets,
        valInLPCMVAssets: util.global.valInLPCMVAssets,
        multiFamUnitCnt: util.global.numMultiFamUnits,
        creSqFt: util.global.totalCRESqFt,
      }),
      e(QuickFilter, {
        location: this.props.searchCriteria.location,
        min: this.props.searchCriteria.Min,
        max: this.props.searchCriteria.Max,
        onSearch: this.handleQuickFilterSearch,
        priceTypeEnum: util.enum.priceSearchType,
        assetTypes: assetType,
      }),
      e('div', { className: 'asset-search-listmap-container' },
        e(AssetList, { assets: this.state.assets, onScroll: this.handleScroll }),
        e('div', { className: 'asset-search-map-container', id: 'asm' },
          e('div', { className: 'map-wrapper' },
            e('div', { className: 'map', id: 'epi-map' }, null)
          )
        )
      )
    );
  }
}

function updateQuickSearchYearBuilt() {
  document.getElementById('txtQuickSearchYearBuilt').value = document.getElementById('txtYearBuilt').value;
}

async function getAssetsFromCookie() {
  let parsedObject;
  let response;
  let obj = {};
  var cookie = CookieChecker.getCookieAssetSearch("asData");
  try {
    obj = JSON.parse(cookie);
    parsedObject = true;
  } catch (err) { }
  if (parsedObject) {
    let parsedLoc = util.parseLocation(obj.location);
    if (parsedLoc) {
      obj.City = parsedLoc.city;
      obj.State = util.convertState(parsedLoc.state);
    }
    response = await comm.getAssets(obj);
    util.global.assets = response.Assets;
    util.getStatsFromAssetSearch(response)
    util.draw(AssetSearch, obj);
    util.drawOneLineSummaries();
    AdvancedSearchFunctions.setFilterValues(obj);
    CookieChecker.deleteCookieAssetSearch("asData");
  }
  else {
    getAssetsFromUserLocation();
  }
}

async function getAssetsFromUserLocation() {
  let response;
  var searchCriteria = {};

  if (navigator.geolocation) {

    navigator.geolocation.getCurrentPosition(async function (position) {
      var searchCriteria = {
        Latitude: position.coords.latitude,
        Longitude: position.coords.longitude,
        SearchRadius: 25, // Kilometers
      };

      response = await comm.getAssets(searchCriteria);
      util.global.assets = response.Assets;
      util.getStatsFromAssetSearch(response)
      util.global.latestSearchCriteria = searchCriteria;
      util.draw(AssetSearch, searchCriteria);
      util.drawOneLineSummaries();

      // Hacky, but map gets initialized pretty late
      var waitForMaps = setInterval(function () {
        if (util.global.map == null) return;
        const geocoder = new google.maps.Geocoder;
        let pos = {
          lat: searchCriteria.Latitude,
          lng: searchCriteria.Longitude,
        }
        geocoder.geocode({ 'location': pos }, async function (results, status) {
          if (status === 'OK') {
            if (results[0]) {
              // Use the first result from search and set city, state, and county
              let firstResult = results[0];

              // City
              let cityElement = document.getElementById('txtCity');
              if (cityElement)
                cityElement.value = firstResult.address_components[2].long_name;

              // State
              let stateElement = document.getElementById('txtState');
              if (stateElement)
                stateElement.value = firstResult.address_components[4].short_name;

              // County
              let countyElement = document.getElementById('txtCounty');
              if (countyElement)
                countyElement.value = firstResult.address_components[3].long_name;
            }
            else {
              window.alert('No results found');
            }
          }
          else {
            window.alert('Geocoder failed due to: ' + status);
          }
        });
        clearInterval(waitForMaps);
      }, 50);

      $(".qs-asset-type").attr("checked", "true");
      $(".qs-select-all").attr("checked", "true");
      util.updateAssetTypeBoxLabel();
    }, function () {
      // Blank fields and no initial search results
      util.draw(AssetSearch, searchCriteria);
      util.drawOneLineSummaries();
    });
  }
  else {
    // Blank fields and no initial search results
    util.draw(AssetSearch, searchCriteria);
    util.drawOneLineSummaries();
  }
}

async function init() {
  try {
    let response;
    let obj = {};

    // If we're coming from a favorite groups page, get the favGroupId variable
    var favGroupId = util.getUrlParam('favGroupId', 'empty');

    // If we're coming from the manage saved searches page, get the savedSearchId variable
    var savedSearchId = util.getUrlParam('savedSearchId', 'empty');
    var filterId = util.getUrlParam('filter', 'empty');
    util.global.savedSearchTabId = savedSearchId;
    
    util.global.selectedAssetType = filterId;
    if (await util.isLoggedIn()) {

      // Get the favorite group information for the user here
      response = await comm.getFavoriteGroups();
      if (response && response.success) {
        util.global.favoriteGroups = response.favGrps;
      }

      for (var i = 0; i < util.global.favoriteGroups.length; i++) {
        var favGroup = util.global.favoriteGroups[i];
        response = await comm.getFavoriteGroupAssets(favGroup.FavoriteGroupId);
        for (var j = 0; j < response.data.length; j++) {
          util.global.favoriteGroupAssets.push(response.data[j]);
        }
      }

      // When the user clicks on the btnCreateFavoriteGroup
      document.getElementById('btnCreateFavoriteGroup').addEventListener('click',
        function () {
          util.clickCreateFavoriteGroup();
        }
      );

      // If the user wasn't directed to this page from favorites or saved searches
      if (savedSearchId === "empty" && favGroupId === "empty") {
        getAssetsFromCookie();

        // Get the user's saved searches
        response = await comm.getSavedSearchesForUser();

        // If operation was successful
        if (response && response.success) {

          // Save this info
          util.global.savedSearches = response.searches;

          // Render this info for the advanced search modal
          await util.drawAdvancedSavedSearches(AdvancedSavedSearches, AdvancedSavedSearchesButton);
        }
      }
      else if (savedSearchId !== "empty") {
        // Get the user's saved searches
        response = await comm.getSavedSearchesForUser();

        if (response && response.success) {
          util.global.savedSearches = response.searches;

          // Match up the saved search variable with the appropriate saved search in the user's list
          for (var i = 0; i < util.global.savedSearches.length; i++) {

            // If we've found the right id
            if (util.global.savedSearches[i].Id === savedSearchId) {

              // Load this search's json into obj
              obj = JSON.parse(util.global.savedSearches[i].Json);

              let parsedLoc = util.parseLocation(obj.location);

              if (parsedLoc) {
                obj.City = parsedLoc.city;
                obj.State = util.convertState(parsedLoc.state);
              }

              let response = await comm.getAssets(obj);
              util.global.assets = response.Assets;
              util.getStatsFromAssetSearch(response);
              util.draw(AssetSearch, obj);
              util.drawOneLineSummaries();
              await util.drawAdvancedSavedSearches(AdvancedSavedSearches, AdvancedSavedSearchesButton);
              AdvancedSearchFunctions.setFilterValues(obj);
              setTimeout(() => { util.copyCheckboxList('.as-asset-sub-type', '.qs-asset-sub-type', true) }, 0);;
              setTimeout(() => { util.copyCheckboxList('.as-asset-type', '.qs-asset-type', true) }, 1);
              setTimeout(function () {
                var advancedSearchPopUp = util.getUrlParam('advancedSearchPopUp', 'False');
                if (advancedSearchPopUp === "True") {
                  // User pressed the edit button from the manage saved searches page, show the advanced search pop up
                  AdvancedSearchFunctions.renderAdvancedSearch();
                  $('#myModal').modal({});
                }
              }, 3);
            }
          }
        }
      }
      else if (favGroupId !== "empty") {
        // Get the favorite group assets for this group
        response = await comm.getFavoriteGroupAssets(favGroupId);
        util.getStatsFromAssetSearch(response);
        util.global.assets = response.data;
        util.draw(AssetSearch, obj);
        util.drawOneLineSummaries();
        await util.drawAdvancedSavedSearches(AdvancedSavedSearches, AdvancedSavedSearchesButton);
      }

      document.getElementById("favGroupListHref").href = "../Asset/ManageFavoriteGroups";
    }
    else {
      getAssetsFromUserLocation();

      // The saved searches section will just show a prompt telling the user 
      // to log in to see their saved searches
      await util.drawAdvancedSavedSearches(AdvancedSavedSearches, AdvancedSavedSearchesButton);

      // Hide the create new favorite group button since you shouldn't be able to do that if you're not logged in
      document.getElementById('btnCreateNewGroup').style.display = 'none';

      // If the user tries to favorite an asset when not logged in, show text
      document.getElementById('favGroupListAlert').innerHTML = '<a class="favoriteAssetLoginLink" href="../Home/Login?favoriteAssetLogin=true" target="_parent" >' + 'Login' + '</a>' + ' to view your favorite groups';

      // Disable the Favorites Group link when the user tries to favorite an asset
      document.getElementById("favGroupListHref").href = "javascript: void(0)";
      document.getElementById('favoritePromptLabel').innerHTML = " ";
      document.getElementById('favoritePromptText').innerHTML = '<a class="favoriteAssetLoginLink" href="../Home/Login?favoriteAssetLogin=true" target="_parent" >' + 'Login' + '</a>' + ' to view your favorite groups';

      $("#favGroupListTitle").mousedown(function () {
        $('#favoritesModal').modal('hide');
        $('#favoritePrompt').modal({});
      });

      $(".favoriteAssetLoginLink").mousedown(function () {
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
          CookieChecker.setCookieFavoriteAsset(JSON.stringify(document.getElementById('hiddenAssetId').value));
        }, 3);
      });
    }
    $(".loading").fadeOut("slow");
  }
  catch (err) {
    throw err;
  }

  // When the user closes the advanced search modal, update the quick search asset type checkboxes
  $('#myModal').on('hidden.bs.modal', function () {
    setTimeout(() => { util.copyCheckboxList('.as-asset-sub-type', '.qs-asset-sub-type', true) }, 0);;
    setTimeout(() => { util.copyCheckboxList('.as-asset-type', '.qs-asset-type', true) }, 1);
  })

  $("#afClearAll").on('click', function () {
    AdvancedSearchFunctions.clearFilterValues()
  });

  $('#assetSearchOneLineSummariesTable').on('page.dt', function () {
    util.drawFavStars();
  });
}
document.addEventListener('DOMContentLoaded', init);

export default AssetSearch;