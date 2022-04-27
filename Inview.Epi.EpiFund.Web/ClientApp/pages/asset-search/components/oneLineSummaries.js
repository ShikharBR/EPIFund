import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');

import *  as Util from '../assetSearchUtils';
const util = Util.util;
import * as Comm from '../comm';
const comm = Comm.comm;
import { LoadingComponent } from '../../../loading-component'
import { randomBytes } from "crypto";
import * as CookieChecker from '../AssetSearchCookieChecker';


export class OneLineSummariesComponent extends Component {
  constructor(props) {
    super(props);
    const self = this;

    this.state = {
      isLoading: true,
      isLoggedIn: false,
    }

    this.drawOneLineSummaries = this.drawOneLineSummaries.bind(this);

    PubSub.subscribe("drawOneLineSummaries", (type, data) => {
      this.drawOneLineSummaries();
    });

    PubSub.subscribe("drawFavStars", (type, data) => {
      this.drawFavStars();
    });
  }

  async componentDidMount() {
    if (await util.isLoggedIn()) {
      this.state.isLoggedIn = true;
    }
  }

  drawFavStars() {
    var data = util.getDataForDataTable();

    //re-init the Fav stars component
    for (var i = 0; i < document.getElementsByClassName('favStar').length; i++) {
      const star = document.getElementsByClassName('favStar')[i];
      const asset = data.find(x => { return x.assetId === $(star).attr('assetId') });
      var favorite = false;

      if (util.global.favoriteGroupAssets.length != 0) {
        for (var a = 0; a < util.global.favoriteGroupAssets.length; a++) {
          if (asset.assetId === util.global.favoriteGroupAssets[a].AssetId) {
            favorite = true;
          }
        }
      }
      ReactDOM.render(<FavStar asset={asset} favorited={favorite} />, star);
    }
  }

  drawOneLineSummaries() {
    var self = this;
    var data = util.getDataForDataTable();

    // Initialize the DataTable
    var table = $('#assetSearchOneLineSummariesTable').DataTable({
      searching: false, 
      info: false,
      paging: true,
      ordering: true,
      select: true,
      data: data,
      columns: [
        { data: 'assetId' },
        { data: 'portfolioId' },
        {
          data: 'asset',
          "render": function (data, type, row, meta) {
            return '<div assetID="' + row.assetId + '" class="favStar"></div>';
          }
        },
        {
          data: 'assetName',
          "render": function (data, type, row, meta) {
            return `<a class="viewAsset" href="/DataPortal/ViewAsset/${row.assetId}" target="_parent" style="cursor:pointer;color:#428bca;text-align:center;" title="view">${data}</a>`;
          }
        },
        { data: 'assetCityState' },
        { data: 'assetType' },
        {
          data: null,
          "render": function (data, type, row, meta) {
            if (row.assetType == "Multi Family") {
              return row.units + " U";
            }
            else {
              return row.squareFeet + " SqFt";
            }
          }
        },
        {
          data: null,
          "render": function (data, type, row, meta) {
            return '<span>' + row.occupancyPercentage + '</span><br/><span>' + row.occupancyDate + '</span>';
          }
        },
        {
          data: 'proformaSGI',
          style: '{text-align: right}',
        },
        { data: 'proformaNOI' },
        { data: 'capRate' },
        {
          data: 'assmFin',
          "render": function (data, type, row, meta) {
            if (data == null || data == "Unknown" || data == "") {
              return 'N/A';
            }
            else {
              return data;
            }
          }
        },
        { data: 'pricingCMV' },
        { data: 'lpCMV' },
        {
          data: 'isPartOfPortfolio',
          "className": "center",
          "render": function (data, type, row, meta) {
            if (data == true) {
              return '<a class="editor_portfolioView" style="cursor:pointer" data-item="' + row.portfolioId + '" title="Portfolio"><span class="glyphicon glyphicon-book"></span></a>';
            }
            else {
              return '---';
            }
          }
        },
        { data: 'availability' },
        {
          data: 'callForOffersDate',
          "render": function (data, type, row, meta) {
            if (row.callForOffersDateSoon) {
              return '<span style="color: red;">' + data + '</span>';
            }
            else {
              return '<span>' + data + '</span>';
            }
          }
        },
        { data: 'auctionDate' },
        {
          data: 'auctionDate', // disable LOIFPA
          "render": function (data, type, row, meta) {
            if (data == "N/A") {
              return '<a class="submitLOI" data-asset="' + row.assetId + '" ><i class="fa fa-file-o fa-lg" title="Letter of Intent to Purchase" style="cursor:pointer";></i></a > <a class="submitFPA" data-asset="' + row.assetId + '" ><i class="fa fa-shopping-cart fa-lg" title="Formal Purchase Agreement" style="cursor:not-allowed";></i></a >';
            }
            else {
              return '<a class="" data-asset="' + row.assetId + '"><i class="fa fa-file-o fa-lg" title="Letter of Intent to Purchase" style="cursor:not-allowed";></i></a > <a class="" data-asset="' + row.assetId + '" ><i class="fa fa-shopping-cart fa-lg" title="Formal Purchase Agreement" style="cursor:not-allowed";></i></a >';
            }
          }
        },
      ],
      columnDefs: [
        {
          "targets": [0, 1],
          "visible": false,
          "searchable": true,
        },
        {
          "targets": [6, 8, 9, 12],
          className: 'dt-body-right',
        },
        {
          "orderData": [12, 11],
          "targets": 12,
        },
        {
          "targets": [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18],
          className: 'dt-head-right',
        },
      ],
      order: [[1, 'asc']]
    });

    // If there is a favorite asset cookie, jump to that page in the one-line summaries
    var favoriteAssetCookie = CookieChecker.getCookieAssetSearch("favoriteAssetData");
    favoriteAssetCookie = favoriteAssetCookie.replace(/\"/g, "");
    if (favoriteAssetCookie) {
      table.page.jumpToData(favoriteAssetCookie, 0);
    }

    $('#assetSearchOneLineSummariesTable tbody').on('click', 'tr', function () {
      $(this).toggleClass('selected');
    });

    $('#assetSearchOneLineSummariesTable').on('mouseenter', 'tbody tr', function () {
      var rowData = table.row(this).data();

      if (util.global.markerArray && util.global.markerArray.length > 0) {
        for (var i = 0; i < util.global.markerArray.length; i++) {
          if (util.global.markerArray[i].title.trim() === rowData.asset.ProjectName.trim()) {
            util.global.markerArray[i].setAnimation(google.maps.Animation.BOUNCE);
            break;
          }
        }
      }
    });

    $('#assetSearchOneLineSummariesTable').on('mouseleave', 'tbody tr', function () {
      var rowData = table.row(this).data();

      if (util.global.markerArray && util.global.markerArray.length > 0) {
        for (var i = 0; i < util.global.markerArray.length; i++) {
          if (util.global.markerArray[i].title.trim() === rowData.asset.ProjectName.trim()) {
            util.global.markerArray[i].setAnimation(null);
            break;
          }
        }
      }
    });

    // LOI
    $('#assetSearchOneLineSummariesTable').on('click', 'a.submitLOI', function (e) {
      e.preventDefault();
      var nid = $(this).attr("data-asset");
      if (self.state.isLoggedIn == true) {
        window.open('/LOI/LOI/' + nid, "_parent");
      }
      else {
        document.getElementById('assetSearchPromptText').innerHTML = 'Please ' + '<a href="../Home/Login?submitLOI=' + nid + '" target="_parent">' + 'login' + '</a>' + ' to activate';
        $('#assetSearchPrompt').modal({});
      }
    });

    $('#oneLineSummariesContainer').appendTo("#asm");

    //re-init the Fav stars component
    this.drawFavStars();
  }

  render() {

    return (
      <React.Fragment>
        {
          this.state.isLoading ?
            < LoadingComponent />
            :
            <React.Fragment>

            </React.Fragment>
        }
      </React.Fragment>
    )
  }
}

class FavStar extends Component {
  constructor(props) {
    super(props);
    const self = this;
    this.checkForFavorites = this.checkForFavorites.bind(this);
    this.state = {
      assetFavorited: this.props.favorited,
      asset: this.props.asset,
    };
  }

  componentDidMount() {
    document.getElementById("toggleMapIcon").addEventListener("click", this.checkForFavorites);
    document.getElementById("toggleListIcon").addEventListener("click", this.checkForFavorites);

    const that = this;
    var favoriteAssetCookie = CookieChecker.getCookieAssetSearch("favoriteAssetData");
    favoriteAssetCookie = favoriteAssetCookie.replace(/\"/g, "");
    if (this.state.asset.assetId == favoriteAssetCookie) {
      window.addEventListener('load', that.handleStarClick(this.state.asset));
    }
  }

  checkForFavorites() {
    if (util.global.favoriteGroupAssets.length != 0) {
      for (var a = 0; a < util.global.favoriteGroupAssets.length; a++) {
        if (this.state.asset.assetId === util.global.favoriteGroupAssets[a].AssetId) {
          // Update the favorited state of this component
          this.setState({ assetFavorited: true });
        }
      }
    }
  }

  handleStarClick(obj) {
    $('#favoritePrompt').modal('hide');
    let asset = obj.asset;
    let image = '/Content/images/no-image-available.jpg';
    if (asset.Image)
      image = `/Image.ashx?id=${asset.AssetId}&name=${asset.Image.FileName}&width=200&height=200`;

    let someAssetData = util.doAllTheMathForAsset(asset);

    if (asset.AskingPrice > 0) {
      document.getElementById('txtFavAssetPrice').textContent = `Listing Price: $${addCommas(asset.AskingPrice.toString())}`;
    }
    else {
      document.getElementById('txtFavAssetPrice').textContent = `CMV: $${addCommas(asset.CurrentBpo.toString())}`;
    }
    document.getElementById('hiddenAssetId').value = asset.AssetId;
    document.getElementById('imgFavAsset').setAttribute('src', image);
    document.getElementById('txtFavAssetType').textContent = util.enum.assetType[asset.AssetType.toString()].asset;
    document.getElementById('txtFavAssetTitle').textContent = asset.ProjectName;
    document.getElementById('txtFavAssetAvailability').textContent = util.enum.listingStatus[asset.ListingStatus.toString()];
    document.getElementById('txtFavAssetSGI').textContent = `SGI: $${addCommas(someAssetData.SGI.toString())}`;
    document.getElementById('txtFavAssetNOI').textContent = `NOI: $${addCommas(someAssetData.NOI.toString())}`;
    document.getElementById('txtFavAssetSqFt').textContent = `SqFt: ${addCommas(asset.SquareFeet.toString())}`;
    document.getElementById('txtFavAssetCAP').textContent = `CAP: ${(someAssetData.APY).toFixed(2)}%`;
    $('#favoritesModal').modal({});

    // TODO: fix this crazy logic, it calls back to the cshtml to "inject" the data to the modal...
    // Creating rows out of the favorite group data
    const rowsFavoriteGroups = util.global.favoriteGroups.map(favoriteGroup => React.createElement(
      "tr",
      { key: favoriteGroup.FavoriteGroupId },
      React.createElement(
        "td",
        { key: 'td', style: { border: 0 } },
        React.createElement(
          "a",
          { style: { cursor: 'pointer' }, onClick: (e) => { e.preventDefault(); this.handleFavoriteGroupClick(favoriteGroup.FavoriteGroupId, asset); } }, // On click, add this asset to this favorite group
          favoriteGroup.FavoriteGroupName
        )
      )
    ));

    // Render the table here
    ReactDOM.render(React.createElement(
      'tbody',
      null,
      rowsFavoriteGroups
    ), document.getElementById('favGroupList'));

    var favoriteAssetCookie = CookieChecker.getCookieAssetSearch("favoriteAssetData");
    if (favoriteAssetCookie) {
      CookieChecker.deleteCookieAssetSearch("favoriteAssetData");
    }
  }

  async handleFavoriteGroupClick(favoriteGroupId, asset) {
    let assetId = asset.AssetId;
    let favoriteGroupAssets = await comm.getFavoriteGroupAssets(favoriteGroupId);
    var existsInFavGrp = false;

    // Check if asset already exists within this favorite group
    for (var i = 0; i < favoriteGroupAssets.data.length; i++) {
      if (favoriteGroupAssets.data[i].AssetId == assetId) {
        existsInFavGrp = true;
      }
    }

    // If asset doesn't already exists in the group, attempt to save to group
    if (!existsInFavGrp) {
      var response = await comm.saveFavoriteGroupAsset(favoriteGroupId, assetId);

      if (response.success) {
        // Add this asset to the global
        util.global.favoriteGroupAssets.push(asset);

        // Update the favorited state of this component
        this.setState({ assetFavorited: true });

        // Hide the create favorite group modal
        $('#favoritesModal').modal('hide');

        // Show the prompt confirming successful addition to favorite group
        document.getElementById('favoritePromptLabel').innerHTML = "Success";
        document.getElementById('favoritePromptText').innerHTML = "Property successfully added to favorite group!";
        $('#favoritePrompt').modal({});

        // Update the asset card star
        var assetCardFavoriteStar = document.getElementById("acFavorite" + assetId);
        if (assetCardFavoriteStar) {
          assetCardFavoriteStar.classList.remove("fa-star-o");
          assetCardFavoriteStar.classList.add("fa-star");
        }
      }
    }
    else {
      // Hide the create favorite group modal
      $('#favoritesModal').modal('hide');

      // Asset already exists in favorite group
      document.getElementById('favoritePromptLabel').innerHTML = "Error";
      document.getElementById('favoritePromptText').innerHTML = "This property has already been added to this favorite group";
      $('#favoritePrompt').modal({});
    }
  }

  unFavoriteAsset(event) {
    // Hovering over shows an empty star
    let el = event.target;
    el.setAttribute("class", "fa fa-star-o fa-2x");
  }

  favoriteAsset(event) {
    // Returns star to filled
    let el = event.target;
    el.setAttribute("class", "fa fa-star fa-2x");
  }

  render() {
    if (this.state.assetFavorited) {
      return (
        <React.Fragment>
          <i id={'dtFavorite' + this.props.asset.assetId}
            className="fa fa-star fa-2x" style={{ cursor: 'pointer', color: '#FFCD00' }}
            onClick={() => { this.handleStarClick(this.props.asset) }}></i>
        </React.Fragment >
      )
    }
    else {
      return (
        <React.Fragment>
          <i id={'dtFavorite' + this.props.asset.assetId}
            className="fa fa-star-o fa-2x" style={{ cursor: 'pointer', color: '#FFCD00' }}
            onMouseOver={this.favoriteAsset.bind(this)}
            onMouseOut={this.unFavoriteAsset.bind(this)}
            onClick={() => { this.handleStarClick(this.props.asset) }}></i>
        </React.Fragment >
      )
    }
  }
}

const element = document.getElementById('oneLineSummariesContainerWrapper');
if (element != null) {
  ReactDOM.render(<OneLineSummariesComponent />, element)
}
