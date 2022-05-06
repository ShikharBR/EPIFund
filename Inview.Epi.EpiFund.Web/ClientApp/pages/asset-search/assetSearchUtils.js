import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');
import * as Comm from './comm';
const comm = Comm.comm;
export const util = {
  global: {
    selectedAssetType: undefined,
    executedFirstRunAssetSearchLogic: false,
    executedFirstRunReactComponentLogic: false,
    activeInfoWindow: null,
    dbTotal: null,
    markerArray: [],
    markerCluster: null,
    appendingAssetsToList: false,
    map: null,
    assets: [],
    assetDisplayCount: 5,
    boundsChangedTimeoutId: null,
    componentUpdatedTimeoutId: null,
    latestSearchCriteria: null,
    savedSearches: null,
    favoriteGroups: [],
    favoriteGroupAssets: [],
    savedSearchTabId: null,
    flags: {
      assetType: false,
      ignoreMapUpdate: false,
      boundsChanged: false,
    },
    globalParticipantdb: null,
    valInLPCMVAssets: null,
    numMultiFamUnits: null,
    totalCRESqFt: null,
    publishedAssets: null,
  },
  isLoggedIn: async () => {
    var result = await comm.isLoggedIn();
    return result === 'True';
  },
  stringHasValue: string => {
    if (string && string.length > 0 && string.trim().length > 0) return true;
    return false;
  },

  enum: {
    accessRoadTypes: {
      1: { name: 'Private' },
      2: { name: 'Public' },
    },
    electricMeteringMethods: {
      1: { name: 'Not Available' },
      2: { name: 'Individual Meters' },
      3: { name: 'Master Meter' },
    },
    gasMeteringMethods: {
      1: { name: 'Not Available' },
      2: { name: 'Individual Meters' },
      3: { name: 'Master Meter' },
    },
    interiorRoadTypes: {
      1: { name: 'Paved Road' },
      2: { name: 'Gravel Road' },
    },
    listingStatus: {
      '1': 'Available',
      '2': 'Pending'
    },
    assetType: {
      '1': {
        name: 'Retail Tenant Property', asset: 'Retail', shorthand: '',
        subTypes: [29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39]
      },
      '2': {
        name: 'Office Tenant Property', asset: 'Office', shorthand: '',
        subTypes: [20, 21, 22, 23, 24, 25, 26, 27, 28]
      },
      '3': {
        name: 'Multi-Family', asset: 'Multi Family', shorthand: 'MF',
        subTypes: [48, 49, 50, 51, 52, 53, 54, 55]
      },
      '4': {
        name: 'Industrial Tenant Property', asset: 'Industrial', shorthand: '',
        subTypes: [1, 2, 3, 4, 5, 6, 7, 8, 9,]
      },
      '5': {
        name: 'MHP', asset: 'MHP', shorthand: 'MHP',
        subTypes: [56, 57, 58, 59, 60, 61]
      },
      '6': {
        name: 'Fuel Service Retail Property', asset: 'Fuel Service Retail', shorthand: '',
        subTypes: [40, 41, 42, 43, 44, 45, 46, 47]
      },
      '7': {
        name: 'Medical Tenant Property', asset: 'Medical', shorthand: '',
        subTypes: [10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
      },
      '8': { name: 'Mixed Use Commercial Property', asset: 'Mixed Use', shorthand: '' },
      '13': { name: 'Fractured Condominium Portfolios', asset: 'Fractured Condominium', shorthand: '' },
      '14': { name: 'Mini-Storage Property', asset: 'Mini-Storage', shorthand: '' },
      '15': { name: 'Parking Garage Property', asset: 'Parking Garage', shorthand: '' },
      '16': { name: 'Secured Private Notes', asset: 'Secured Private Notes', shorthand: '' },
    },
    assetSubType: {
      '1': { name: 'Single Tenant' },
      '2': { name: 'Multiple Tenant' },
      '3': { name: 'Publically Traded Single Tenant' },
      '4': { name: 'Publically Traded Multiple Tenant' },
      '5': { name: 'Warehouse - Single Tenant' },
      '6': { name: 'Warehouse - Multiple Tenant' },
      '7': { name: 'Light Manufacturing' },
      '8': { name: 'Heavy Manufacturing' },
      '9': { name: 'Office - Warehouse' },
      '10': { name: 'Other', },
      '11': { name: 'Resort/Hotel/Motel Property', },
      '12': { name: 'Franchised Tenant', },
      '13': { name: 'Publically Traded Franchised Tenant', },
      '14': { name: 'Publically Traded Major Tenant', },
      '15': { name: 'Single Tenant', },
      '16': { name: 'Multiple Tenant', },
      '17': { name: 'Research Facility', },
      '18': { name: 'Rehabilitation Facility', },
      '19': { name: 'Assisted Living Facility', },
      '20': { name: 'Single Tenant', },
      '21': { name: 'Publically Traded Single Tenant', },
      '22': { name: 'Multiple Tenant', },
      '23': { name: 'Publically Traded Major Tenant', },
      '24': { name: 'High Rise', },
      '25': { name: 'Mid Rise', },
      '26': { name: 'Garden Style', },
      '27': { name: 'Office Condominium Development', },
      '28': { name: 'Government Tenant', },
      '29': { name: 'Single Tenant', },
      '30': { name: 'Multiple Tenant with Anchor', },
      '31': { name: 'Multiple Tenant without Anchor', },
      '32': { name: 'Publically Traded Single Tenant', },
      '33': { name: 'Publically Traded Anchor Tenant', },
      '34': { name: 'Restaurant Franchise', },
      '35': { name: 'Grocery Franchise', },
      '36': { name: 'Retail Franchise', },
      '37': { name: 'Factory Outlet Center', },
      '38': { name: 'Government Tenant', },
      '39': { name: 'Regional Mall', },
      '40': { name: 'Convenience Store Franchise - No Fuel', },
      '41': { name: 'Convenience Store Franchise - With Fuel', },
      '42': { name: 'Convenience Store - No Fuel', },
      '43': { name: 'Convenience Store - With Fuel', },
      '44': { name: 'Multi-Purpose Trucking Service Facility', },
      '45': { name: 'Auto Care Facility - With Fuel', },
      '46': { name: 'Auto Care Facility - No Fuel', },
      '47': { name: 'Franchised Auto Care Facility', },
      '48': { name: 'Student Housing Development', },
      '49': { name: 'Retirement Housing Development', },
      '50': { name: 'Garden Style', },
      '51': { name: 'High Rise', },
      '52': { name: 'Mid Rise', },
      '53': { name: 'Section 8 Housing - Government Assisted', },
      '54': { name: 'LIHTC', },
      '55': { name: 'Property with 1+ Community Pool/Spa', },
      '56': { name: 'MHP Properties with or without RV Facilities', },
      '57': { name: 'MHP Properties without RV Facilities', },
      '58': { name: 'MHP Properties with Only Affixed Units', },
      '59': { name: 'MHP Properties with Affixed or Above Ground Units', },
      '60': { name: 'MHP Properties with HOAs', },
      '61': { name: 'RV Parks and Campsites without MHP Spaces', },
    },
    priceSearchType: {
      '1': { name: 'LP Only' },
      '2': { name: 'CMV Only' },
      '3': { name: 'CMV/LP' },
    },
    wasteWaterTypes: {
      1: { name: 'Public' },
      2: { name: 'Septic' },
      3: { name: 'Private' }
    },
    waterServTypes: {
      1: { name: 'Private' },
      2: { name: 'Public' },
      3: { name: 'On-Site' },
    },
  },
  // Getting the url variable
  getUrlVars: () => {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
      vars[key] = value;
    });
    return vars;
  },
  // If there is no savedSearchId on this load
  getUrlParam: (parameter, defaultvalue) => {
    var urlparameter = defaultvalue;
    if (window.location.href.indexOf(parameter) > -1) {
      urlparameter = util.getUrlVars()[parameter];
    }
    return urlparameter;
  },

  doAllTheMathForAsset: asset => {
    if (asset != null) {
      let data = {
        SGI: 0,
        NOI: 0,
        APY: 0,
      };
      let totalIncome = asset.ProformaAI + asset.ProformaMI;
      let pvf = (asset.ProformaVF / 100) * totalIncome;
      let pretax = (totalIncome - pvf) - asset.ProformaAOE;
      data.NOI = Math.round((totalIncome - pvf) - asset.ProformaAOE);
      if (totalIncome > 0) data.SGI = asset.ProformaAI;
      if (asset.AskingPrice > 0) data.APY = (pretax / asset.AskingPrice) * Math.pow(10, 2);
      else if (asset.CurrentBpo > 0) data.APY = (pretax / asset.CurrentBpo) * Math.pow(10, 2);
      switch (asset.AssetType) {
        case 3:
        case 5:
          // MF/MHP

          break;
        case 6:
          // fuel`
          if (!isNaN(asset.SalesPrice) && asset.SalesPrice > 0) data.APY = pretax / asset.SalesPrice;
          break;
        default:
          // Commercial

          break;
      }
      return data;
    }
  },
  convertState: state => {
    if (typeof state === "string" && state.length > 0 && util.temp.states && util.temp.states.length > 0) {
      for (let i = 0; i < util.temp.states.length; i++) {
        if (state === util.temp.states[i].name) return util.temp.states[i].abbreviation
      }
    }
    return state;
  },
  parseLocation: loc => {
    var address = { city: '', state: '', zip: '', county: '' }
    // do minimum processing... this is only temp code
    if (!loc || !loc.length || loc.length === 0) return;
    try {
      if (~loc.indexOf(', ')) {
        // assume city is first, after comma is state and check for a zip at the end
        // if you enter multiple commas it will break, I dont care
        let strings = loc.split(', ');
        if (~loc.indexOf('County')) {
          address.county = strings[0].trim().replace(" County", "");
        }
        else {
          address.city = strings[0].trim();
        }
        if (strings[1].trim().length > 0 && ~strings[1].trim().indexOf(' ')) {
          // space somewhere in the string, assume its between the state and the zip
          strings = strings[1].split(' ');
          address.state = strings[0].trim();
          address.zip = strings[1].trim();
        } else if (strings[1].trim().length > 0) {
          // for now, assume its state
          address.state = strings[1].trim();
        }
      } else {
        // treat as single object
        if (loc.trim().length === 2) address.state = loc;
        else {
          let parsedZip = false;
          if (loc.trim().length === 5) {
            let int = parseInt(loc.trim());
            if (!isNaN(int)) {
              address.zip = loc.trim();
              parsedZip = true;
            }
          }
          if (!parsedZip) address.city = loc;
        }
      }
      return address;
    } catch (err) {
      alert(err);
      return;
    }
  },
  draw: (reactClass, searchCriteria) => {
    let arr = [];
    // for now, just display the first n assets
    if (util.global.assets) {
      let numAssetsToLoad = util.global.assets.length > util.global.assetDisplayCount ? util.global.assetDisplayCount
        : util.global.assets.length;
      arr = util.global.assets.slice(0, numAssetsToLoad);
    }
    //ReactDom.render(React.CreateElement(react))
    ReactDOM.render(React.createElement(reactClass, { assets: arr, searchCriteria: searchCriteria }), document.getElementById('asset-search'));
  },
  drawAdvancedSavedSearches: async (searches, button) => {
    /*
    4/11/2020 Chi
    advancedSavedSearchList element might not be initialized when drawAdvancedSavedSearches is called
    */
    setTimeout(async () => {
      // Draw the saved search list / log in prompt
      ReactDOM.render(React.createElement(searches, { searches: util.global.savedSearches }), document.getElementById('advancedSavedSearchList'));

      // If the user is logged in and has saved searches, draw the update button so the user can update their saved search
      if (await util.isLoggedIn() && util.global.savedSearches != null && util.global.savedSearches.length > 0) {
        ReactDOM.render(React.createElement(button), document.getElementById('myModalFooter'));
      }
    }, 1000);
  },
  drawOneLineSummaries: () => {
    ///5/12/2019
    //Chi
    //this is a work around for now, if you want to do React, please do it the right way, stop adding functions to this monster object / util class
    //this is basically publishing an event to tell the component to render
    PubSub.publish("drawOneLineSummaries");
  },
  drawFavStars: () => {
    PubSub.publish("drawFavStars");
  },
  reDrawOneLineSummaries: () => {
    // Get the data
    var data = util.getDataForDataTable();

    // Get the DataTable
    var table = $('#assetSearchOneLineSummariesTable').DataTable();

    // Clear the DataTable
    table.clear().draw();

    // Add the new data to the DataTable
    table.rows.add(data);

    // Redraw the DataTable
    table.columns.adjust().draw();

    util.drawFavStars();
  },
  parseSearchResultAssets(arr) {
    var data = [];
    var listingStatus;
    var lpCMV = "";
    for (var i = 0; i < arr.length; i++) {

      let assetData = util.doAllTheMathForAsset(arr[i]);
      var price = "";
      if (arr[i].AskingPrice > 0) {
        price = "$" + arr[i].AskingPrice.toLocaleString();
        lpCMV = "LP";
      }
      else {
        price = "$" + arr[i].CurrentBpo.toLocaleString();
        lpCMV = "CMV";
      }

      if (util.enum.listingStatus[arr[i].ListingStatus.toString()] == null) {
        listingStatus = "N/A";
      }
      else {
        listingStatus = util.enum.listingStatus[arr[i].ListingStatus.toString()];
      }

      let image = '/Content/images/no-image-available.jpg';
      if (arr[i].Image != null)
        image = `https://images.uscreonline.com/${arr[i].AssetId}/${arr[i].Image.FileName}`;

      var address = arr[i].AddressLine1;
      if (arr[i].AddressLine2) {
        address = address + " " + arr[i].AddressLine2;
      }

      var item = {
        assetId: arr[i].AssetId,
        asset: arr[i],
        assetName: arr[i].ProjectName,
        assetAddress: address,
        assetCityState: arr[i].City + ", " + arr[i].State + ", " + arr[i].Zip,
        assetType: util.enum.assetType[arr[i].AssetType.toString()].asset,
        availability: listingStatus,
        pricingCMV: price,
        lpCMV: lpCMV,
        squareFeet: arr[i].SquareFeet.toLocaleString(),
        units: arr[i].TotalUnits,
        proformaSGI: "$" + assetData.SGI.toLocaleString(),
        proformaNOI: "$" + assetData.NOI.toLocaleString(),
        capRate: assetData.APY.toFixed(2) + "%",
        callForOffersDate: arr[i].CallforOffersDate,
        callForOffersDateSoon: arr[i].CallforOffersDateSoon,
        auctionDate: arr[i].AuctionDate,
        occupancyPercentage: arr[i].OccupancyRate,
        occupancyDate: arr[i].OccupancyDate,
        assmFin: arr[i].AssmFinancing,
        isPartOfPortfolio: arr[i].IsPartOfPortfolio,
        portfolioId: arr[i].PortfolioId,
        image: image
      }
      data.push(item);
    }
    return data;
  },
  getDataForDataTable: () => {
    let arr = [];
    // Get the assets
    if (util.global.assets) {
      arr = util.global.assets;
    }
    $('table.assetSearchOneLineSummariesTable').DataTable();

    return util.parseSearchResultAssets(arr);
  },
  updateAssetListFromVisiblePins: cb => {
    console.log('<updateAssetListFromVisiblePins> here')
    if (util.global.markerArray && map && util.global.markerArray.length > 0) {
      let visiblePins = [];
      let assetsToShow = [];
      for (let i = 0; i < util.global.markerArray.length; i++) {
        if (util.global.map.getBounds().contains(util.global.markerArray[i].getPosition())) {
          visiblePins.push(util.global.markerArray[i].title);
        }
      }
      console.log('<updateAssetListFromVisiblePins> visible pins', visiblePins);
      for (let i = 0; i < util.global.assets.length; i++) {
        for (let j = 0; j < visiblePins.length; j++) {
          if (i === 0 && j === 0) console.log('first asset names ', { a: util.global.assets[i].ProjectName, b: visiblePins[j].title })
          if (util.global.assets[i].ProjectName === visiblePins[j]) {
            assetsToShow.push(util.global.assets[i]);
            // break;
          }
        }
      }
      //ReactDOM.render(React.createElement(AssetSearch,{assets:assetsToShow,searchCriteria:util.global.latestSearchCriteria}), document.getElementById('asset-search'));
    }
    if (cb && typeof cb === 'function') cb();
  },
  removeGoogleMapScript: () => {
    console.debug('removing google script...');
    let keywords = ['maps.googleapis'];

    //Remove google from BOM (window object)
    window.google = undefined;

    //Remove google map scripts from DOM
    let scripts = document.head.getElementsByTagName("script");
    for (let i = scripts.length - 1; i >= 0; i--) {
      let scriptSource = scripts[i].getAttribute('src');
      if (scriptSource != null) {
        if (keywords.filter(item => scriptSource.includes(item)).length) {
          scripts[i].remove();
        }
      }
    }
  },
  runOnLoad: () => {
    // Cannot have multiple references to the Google Map script
    util.removeGoogleMapScript();
    // bind the initMap function to one defined in the global scope
    window.initMap = util.initMap.bind(this);
    // add google maps script to the page
    const script = document.createElement('script')
    script.async = true
    script.defer = true
    script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyBV3nTZqDmBaifM8IRM_XHMTFeobjfckLw&callback=initMap"
    document.head.appendChild(script);
    util.global.executedFirstRunAssetSearchLogic = true;
  },
  clearMarkers: () => {
    for (var i = 0; i < util.global.markerArray.length; i++) util.global.markerArray[i].setMap(null);
    util.global.markerArray = [];
    if (util.global.markerCluster != null) {
      util.global.markerCluster.clearMarkers();
    }
  },
  initMap: () => {
    util.global.map = new google.maps.Map(
      document.getElementById("epi-map"),
      {
        center: new google.maps.LatLng(37.0902, -95.7129), // Default to zoomed out USA
        zoom: 3,
        zoomControl: true,
        zoomControlOptions: {
          style: google.maps.MapTypeControlStyle.DEFAULT,
          position: google.maps.ControlPosition.RIGHT_TOP,
        },
        mapTypeControl: false, // ROADMAP, SATELLITE, HYBRID, or TERRAIN
        scaleControl: false,
        streetViewControl: false,
        rotateControl: false,
        fullscreenControl: false
      }
    );
    util.global.flags.ignoreMapUpdate = true;
    util.global.map.addListener('bounds_changed', function () {
      if (!util.global.flags.ignoreMapUpdate) {
        if (util.global.boundsChangedTimeoutId) {
          // cancel previous timeout, we only want one at a time
          window.clearTimeout(util.global.boundsChangedTimeoutId);
          util.global.boundsChangedTimeoutId = undefined;
        }
        util.global.boundsChangedTimeoutId = setTimeout(function () {
          util.global.flags.ignoreMapUpdate = true;
          // TODO: fix issue with regarding this update event not playing nice specifically with the search criteria update event
          //updateAssetListFromVisiblePins();
        }, 1000);
      }
      util.global.flags.boundsChanged = false;
    })
    util.global.map.addListener('tilesloaded', function () {
      // lets see how this works. We set this flag when updating assets outside of the map interactions.
      // After the tiles have loaded, assume all those updates have finished and reset the flag. Not bullet
      // proof but should work without degrading perfomance.
      util.global.flags.ignoreMapUpdate = false;
    })

    util.placeMarkers(() => { util.updateMapBoundaries(); });
  },
  placeMarkers: cb => {
    if (util.global.assets && util.global.assets.length > 0) placeMarker(0);
    function placeMarker(index) {
      if (index < util.global.assets.length) {
        let currentAsset = util.global.assets[index];
        // example retail only marker label
        if (currentAsset.Latitude && currentAsset.Longitude) {
          let markerOptions = {
            position: { lat: currentAsset.Latitude, lng: currentAsset.Longitude },
            map: util.global.map,
            title: currentAsset.ProjectName.trim()
          };
          if (currentAsset.AssetType === 2) markerOptions.icon = '/Content/images/google-map-icons/office-building.png';
          // To identify whether an asset on the map has previously been favorited by the user, make the pin a star
          if (util.global.favoriteGroupAssets.length > 0) {
            for (var i = 0; i < util.global.favoriteGroupAssets.length; i++) {
              if (currentAsset.AssetId == util.global.favoriteGroupAssets[i].AssetId) {
                markerOptions.icon = 'https://maps.google.com/mapfiles/kml/pal4/icon47.png';
              }
            }
          }
          var marker = new google.maps.Marker(markerOptions);
          util.global.markerArray.push(marker);
          google.maps.event.addListener(marker, 'click', function () {
            let image = '/Content/images/no-image-available.jpg';
            if (currentAsset.Image) {
              image = `/Image.ashx?id=${currentAsset.AssetId}&name=${currentAsset.Image.FileName}&width=100&height=100`;
            }
            var price = currentAsset.AskingPrice > 0 ? currentAsset.AskingPrice : currentAsset.CurrentBpo;
            // TODO: create info windows on pin click
            var infoWindow = new google.maps.InfoWindow({
              content: `
                      <div class="asset-window-wrapper">
                          <a class="asset-window-link" href="/DataPortal/ViewAsset/${currentAsset.AssetId}" target="_parent">
                            <div class="asset-window-image-container">
                                <img src="${image}" />
                            </div>
                          </a>
                          <div class="asset-window-data-container">
                              <div class="adp asset-window-data-p0">
                                  ${currentAsset.ProjectName}
                              </div>
                              <div class="adp asset-window-data-p1">
                                  $${addCommas(price.toString())}
                              </div>
                              <div class="adp asset-window-data-p2">
                                  sqft: ${addCommas(currentAsset.SquareFeet.toString())}
                              </div>
                          </div>
                      </div>
                          `,
              size: new google.maps.Size(200, 100),
              maxWidth: 200
            });
            if (util.global.activeInfoWindow) { util.global.activeInfoWindow.close(); }
            util.global.activeInfoWindow = infoWindow;
            util.global.activeInfoWindow.open(util.global.map, marker);
            // scroll to asset
            let asset = document.getElementById(`A${currentAsset.AssetId}`);
            if (asset) {
              util.scrollToElement(asset);
            } else {
              // we need to add it to the view
              scrollToAsset(currentAsset.AssetId);
            }
          });
        }
        setTimeout(() => { placeMarker(index + 1); }, 10);
        //placeMarker(index + 1);

      } else {
        // add marker clusterer
        // TODO: customize group cluster icons?
        util.global.markerCluster = new MarkerClusterer(util.global.map, util.global.markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
        if (cb && typeof cb === 'function') cb();
      }
    }
  },
  scrollToElement: el => {
    if (el) {
      let al = document.getElementById('asl');
      if (al) {
        let elRect = el.getBoundingClientRect();
        let alRect = al.getBoundingClientRect();
        al.scrollTo({
          top: (elRect.top + al.scrollTop) - alRect.top,
          left: 0,
          behavior: 'smooth',
        });
      }
    }
  },
  updateMapBoundaries: () => {
    let bounds = new google.maps.LatLngBounds();
    if (util.global.markerArray.length > 0) {
      for (let i = 0; i < util.global.markerArray.length; i++) {
        bounds.extend(util.global.markerArray[i].position);
      }

      // If more than 1, call fit bounds 
      if (util.global.markerArray.length > 1) {
        util.global.map.fitBounds(bounds);
      } else if (util.global.markerArray.length === 1) {
        // Else, set zoom and center to that position
        let x = util.global.markerArray[0];
        util.global.map.setCenter({ lat: x.position.lat(), lng: x.position.lng() });
        util.global.map.setZoom(15);
      }
    }
    else {
      // If there are 0 search results, center the map on the US
      util.global.map.setCenter({ lat: 37.0902, lng: -95.7129 });
      util.global.map.setZoom(3);
    }
  },
  // Handles data when user tries to create a new favorite group
  clickCreateFavoriteGroup: async () => {
    var fgnLowerCaseDeSpaced = document.getElementById('favorite-group-name').value.toLowerCase();
    fgnLowerCaseDeSpaced = fgnLowerCaseDeSpaced.replace(/\s/g, "");

    if (util.isAlphaNumeric(fgnLowerCaseDeSpaced)) {
      let response = await comm.getFavoriteGroups();
      var duplicateNameFound = false;

      if (response && response.success) {
        // User has favorite groups, check to see if there are duplicate names
        var inputFavoriteGroupName = document.getElementById('favorite-group-name').value.toLowerCase();
        inputFavoriteGroupName = inputFavoriteGroupName.trim();
        inputFavoriteGroupName = inputFavoriteGroupName.replace(/\s{2,}/g, ' ');
        // Check for duplicates
        if (util.global.favoriteGroups.length > 0) {
          for (var i = 0; i < util.global.favoriteGroups.length; i++) {
            var favoriteGroupName = util.global.favoriteGroups[i].FavoriteGroupName.toLowerCase();
            if (inputFavoriteGroupName == favoriteGroupName) {
              duplicateNameFound = true;
            }
          }
        }
      }
      // If there are no duplicates
      if (!duplicateNameFound) {
        var newFavoriteGroupName = document.getElementById('favorite-group-name').value.trim();
        newFavoriteGroupName = newFavoriteGroupName.replace(/\s{2,}/g, ' ');
        var newFavoriteGroupDescription = "";
        if (document.getElementById('favorite-group-description').value) {
          newFavoriteGroupDescription = document.getElementById('favorite-group-description').value;
        }

        // Attempt to save asset to favorite group
        response = await comm.createFavoriteGroup(newFavoriteGroupName, newFavoriteGroupDescription);

        if (response && response.success) {
          // Asset successfully saved to favorite group
          // Hide the create favorite group modal
          $('#createFavoriteGroup').modal('hide');

          // Show the prompt informing user that group was successfully created
          document.getElementById('favoritePromptLabel').innerHTML = "Success";
          document.getElementById('favoritePromptText').innerHTML = "Successfully created favorite group!";
          $('#favoritePrompt').modal({});

          // Update favorite groups
          response = await comm.getFavoriteGroups();
          if (response && response.success) {
            util.global.favoriteGroups = response.favGrps;
          }
        }
      }
      else {
        // A favorite group already exists with this name
        // Hide the create favorite group modal
        $('#createFavoriteGroup').modal('hide');

        // Show the prompt informing user that group already exists with this name
        document.getElementById('favoritePromptLabel').innerHTML = "Error";
        document.getElementById('favoritePromptText').innerHTML = "A favorite group already exists with this name";
        $('#favoritePrompt').modal({});
      }
    }
    else {
      // User can only use letters and numbers
      // Hide the create favorite group modal
      $('#createFavoriteGroup').modal('hide');

      // Show the prompt informing user can only use letters and numbers
      document.getElementById('favoritePromptLabel').innerHTML = "Error";
      document.getElementById('favoritePromptText').innerHTML = "Please enter a favorite group name consisting of only letters and numbers";
      $('#favoritePrompt').modal({});
    }

    // Reset the value of these fields to nothing so that the placeholder text can appear
    document.getElementById('favorite-group-name').value = "";
    document.getElementById('favorite-group-description').value = "";
  },
  temp: {
    states: [{ "name": "Alabama", "abbreviation": "AL" }, { "name": "Alaska", "abbreviation": "AK" }, { "name": "American Samoa", "abbreviation": "AS" }, { "name": "Arizona", "abbreviation": "AZ" }, { "name": "Arkansas", "abbreviation": "AR" }, { "name": "California", "abbreviation": "CA" }, { "name": "Colorado", "abbreviation": "CO" }, { "name": "Connecticut", "abbreviation": "CT" }, { "name": "Delaware", "abbreviation": "DE" }, { "name": "District Of Columbia", "abbreviation": "DC" }, { "name": "Federated States Of Micronesia", "abbreviation": "FM" }, { "name": "Florida", "abbreviation": "FL" }, { "name": "Georgia", "abbreviation": "GA" }, { "name": "Guam", "abbreviation": "GU" }, { "name": "Hawaii", "abbreviation": "HI" }, { "name": "Idaho", "abbreviation": "ID" }, { "name": "Illinois", "abbreviation": "IL" }, { "name": "Indiana", "abbreviation": "IN" }, { "name": "Iowa", "abbreviation": "IA" }, { "name": "Kansas", "abbreviation": "KS" }, { "name": "Kentucky", "abbreviation": "KY" }, { "name": "Louisiana", "abbreviation": "LA" }, { "name": "Maine", "abbreviation": "ME" }, { "name": "Marshall Islands", "abbreviation": "MH" }, { "name": "Maryland", "abbreviation": "MD" }, { "name": "Massachusetts", "abbreviation": "MA" }, { "name": "Michigan", "abbreviation": "MI" }, { "name": "Minnesota", "abbreviation": "MN" }, { "name": "Mississippi", "abbreviation": "MS" }, { "name": "Missouri", "abbreviation": "MO" }, { "name": "Montana", "abbreviation": "MT" }, { "name": "Nebraska", "abbreviation": "NE" }, { "name": "Nevada", "abbreviation": "NV" }, { "name": "New Hampshire", "abbreviation": "NH" }, { "name": "New Jersey", "abbreviation": "NJ" }, { "name": "New Mexico", "abbreviation": "NM" }, { "name": "New York", "abbreviation": "NY" }, { "name": "North Carolina", "abbreviation": "NC" }, { "name": "North Dakota", "abbreviation": "ND" }, { "name": "Northern Mariana Islands", "abbreviation": "MP" }, { "name": "Ohio", "abbreviation": "OH" }, { "name": "Oklahoma", "abbreviation": "OK" }, { "name": "Oregon", "abbreviation": "OR" }, { "name": "Palau", "abbreviation": "PW" }, { "name": "Pennsylvania", "abbreviation": "PA" }, { "name": "Puerto Rico", "abbreviation": "PR" }, { "name": "Rhode Island", "abbreviation": "RI" }, { "name": "South Carolina", "abbreviation": "SC" }, { "name": "South Dakota", "abbreviation": "SD" }, { "name": "Tennessee", "abbreviation": "TN" }, { "name": "Texas", "abbreviation": "TX" }, { "name": "Utah", "abbreviation": "UT" }, { "name": "Vermont", "abbreviation": "VT" }, { "name": "Virgin Islands", "abbreviation": "VI" }, { "name": "Virginia", "abbreviation": "VA" }, { "name": "Washington", "abbreviation": "WA" }, { "name": "West Virginia", "abbreviation": "WV" }, { "name": "Wisconsin", "abbreviation": "WI" }, { "name": "Wyoming", "abbreviation": "WY" }]
  },
  findElementInListByAttributeVal: (nodeList, attrib, val) => {
    nodeList.forEach(function (elem) {
      if (elem.getAttribute(attrib) == val)
        return elem;
    });
    return null;
  },
  // copy checkbox list
  // @oarams: srcSelector is source list, destSelector is destination list
  copyCheckboxList: (srcSelector, destSelector, fireOnClick = true, displayCheckboxNotFoundError = true) => {
    let srcList = document.querySelectorAll(srcSelector);
    let destList = document.querySelectorAll(destSelector);
    let i = 0;
    srcList.forEach(function (srcCB) {
      let srcDataID = srcCB.attributes['data-id'].value;
      let destCB = destList[i].getAttribute('data-id') == srcDataID ?
        destList[i] : findElementInListByAttributeVal(destList, 'data-id', srcDataID);
      if (destCB != null) {
        if (destCB.checked != srcCB.checked)
          if (fireOnClick)
            destCB.click();
          else
            destCB.checked = srcCB.checked;
      } else if (displayCheckboxNotFoundError)
        console.error(`Could not find checkbox in '${destSelector}' with dataid '${srcDataID}'`);
      i++;
    });
  },

  // Adds a label above an quicksearch text input
  addQSLabel: (qsItem, labelText) => {
    return React.createElement('div', { className: 'quick-asset-search-input-container' },
      React.createElement('label', { className: 'quick-asset-search-input' }, labelText),
      qsItem);
  },
  // Create a nested object for maintaining all checkbox states
  // in a NestCheckBox, matches structure of the 'inputs'
  checkStateCreater: (checkState, inputs, parent = null) => {
    inputs.forEach(input => {
      let id = input['data-id'];
      checkState[id] = { checked: (parent && parent.checked) || false, parent: parent }
      if (input.inputs) {
        checkState[id].children = {};
        util.checkStateCreater(checkState[id].children, input.inputs, checkState[id]);
      }
    });
  },
  // Converts an int (if greater than 999,999) to  ##.# <Magnitude Suffix>
  // i.e, 10,500,000 -> "10.5 Million"
  //      53,432 -> "53,432"
  intToShortString: (value) => {
    let suffix = ["", "", "Million", "Billion", "Trillion"];
    let sufIndex = Math.floor((('' + value).length - 1) / 3);
    if (sufIndex > 1) {
      let shortenedValue = parseFloat((value / Math.pow(1000, sufIndex)).toPrecision(3));
      if (shortenedValue % 1 != 0) {
        shortenedValue = shortenedValue.toFixed(1)
      }
      return shortenedValue + ' ' + suffix[sufIndex];
    } else {
      return Number(value).toLocaleString();
    }
  },

  listItemClick: (event) => {
    $(event.currentTarget).find('input').click();
    event.stopPropagation();
  },

  stripNonNumericChars: (string) => {
    return string.replace(/\D/g, '');

  },
  addCommaSeparators: (numString) => {
    return numString.replace(/(?!^)(?=(\d{3})+(?!\d))/g, ",");
    //return parseInt(numString).toLocaleString();
  },
  addSymbolToTextbox: (textbox, symbol) => {
    return React.createElement('div', { className: 'input-group' },
      [React.createElement('span', { className: 'input-group-addon' }, symbol),
        textbox]);
  },
  // strips invalid characters and formats with commas
  onMoneyChange: (event) => {
    let target = event.currentTarget;
    let targetVal = target.value;
    let cleanedVal = util.stripNonNumericChars(target.value);//.replace(/^0*/g, '');
    let formattedVal = util.addCommaSeparators(cleanedVal);


    let oldCursorPosition = target.selectionStart;
    target.value = formattedVal;

    let newCursorPosition = oldCursorPosition - targetVal.length + formattedVal.length;
    target.selectionStart = newCursorPosition;
    target.selectionEnd = newCursorPosition;
  },
  // Handles user pressing backspace on a comma
  onMoneyKeyUp: (event) => {
    let target = event.currentTarget;
    let selectionStart = target.selectionStart;
    if (event.keyCode == 8 // backspace
      && target.value.charAt(selectionStart - 1) == ',') {

      let newVal = target.value.substr(0, selectionStart - 2) + '' + target.value.substr(selectionStart - 1);
      newVal = util.addCommaSeparators(util.stripNonNumericChars(newVal));

      target.value = newVal;
      let newSelection = selectionStart - 1;
      // Handle user pressing backspace when comma is the second character
      if (selectionStart === 2) {
        newSelection = 0
      }
      // Handle user pressing backspace after a comma is deleted due to a decrease in magnitude
      else if (newVal.length % 4 == 3) {
        newSelection--;
      }

      target.selectionStart = newSelection;
      target.selectionEnd = newSelection;
    }
  },
  updateAssetTypeBoxLabel: () => {
    if (util.global.selectedAssetType != undefined) {
      $('input.qs-asset-type:checkbox').each(function (x, item) {
        if ($(item)[0].dataset.id != util.global.selectedAssetType) {
          $(item)[0].checked = false;
        }
        else{
          $(item)[0].checked = true;
        }
      });
    }
    // On any checkbox change, check to see what's been selected and then update the Asset Type box label
    if ($('.qs-asset-type:checked').length == $('.qs-asset-type').length) {
      // If everything's been selected, text will say "All"
      document.getElementById('qsAssetTypeButtonLabel').innerHTML = "All";
    }
    else if ($('.qs-asset-type:checked').length == 1 && $('.qs-asset-type:indeterminate').length == 0) {
      // If one asset type is checked and none are indeterminate, text will say the asset type
      var checkedValue = document.querySelector('.qs-asset-type:checked').parentNode.innerText;
      document.getElementById('qsAssetTypeButtonLabel').innerHTML = util.truncateString(checkedValue, 15);
    }
    else if ($('.qs-asset-type:checked').length == 0 && $('.qs-asset-type:indeterminate').length == 1) {
      // If no asset types is checked but one is indeterminate, text will say the asset type of the indeterminate 
      var checkedValue = document.querySelector('.qs-asset-type:indeterminate').parentNode.innerText;
      document.getElementById('qsAssetTypeButtonLabel').innerHTML = util.truncateString(checkedValue, 15);
    }
    else if ($('.qs-asset-type:checked').length > 0 || $('.qs-asset-type:indeterminate').length > 0) {
      // If multiple asset types are selected or multiple asset types are indeterminate, set the text to multiple
      document.getElementById('qsAssetTypeButtonLabel').innerHTML = "Multiple";
    }
    else {
      // If the user selects nothing, text will just say "Asset Type"
      document.getElementById('qsAssetTypeButtonLabel').innerHTML = "Asset Type";
    }
  },
  truncateString: (str, num) => {
    if (str.length > num) {
      return str.slice(0, num) + "...";
    }
    else {
      return str;
    }
  },
  isAlphaNumeric: (str) => {
    var code, i, len;
    for (i = 0, len = str.length; i < len; i++) {
      code = str.charCodeAt(i);
      if (!(code > 47 && code < 58) && // (0-9)
        !(code > 64 && code < 91) && // (A-Z)
        !(code > 96 && code < 123)) { // (a-z)
        return false;
      }
    }
    return true;
  },
  getStatsFromAssetSearch: (response) => {
    util.global.dbTotal = response.Total;
    util.global.publishedAssets = response.PublishedAssets;
    util.global.valInLPCMVAssets = response.TotalAssetVal,
      util.global.numMultiFamUnits = response.MultiFamUnits,
      util.global.totalCRESqFt = response.TotalSqFt;
  },
}
