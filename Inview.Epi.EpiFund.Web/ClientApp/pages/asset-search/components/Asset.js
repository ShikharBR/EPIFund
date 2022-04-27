import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../assetSearchUtils';
const util = Util.util;

import * as Comm from '../comm';
const comm = Comm.comm;

let e = React.createElement;

export class Asset extends React.Component {
  constructor(props) {
    super(props);
    this.handleFavoriteGroupClick = this.handleFavoriteGroupClick.bind(this);
    this.handleMouseEnter = this.handleMouseEnter.bind(this);
    this.handleMouseLeave = this.handleMouseLeave.bind(this);
    this.handleMouseOver = this.handleMouseOver.bind(this);
    this.state = {
      favorited: false,
    };
  }
  handleMouseEnter() {
    //console.log('Mouse Enter');
    if (util.global.markerArray && util.global.markerArray.length > 0) {
      for (var i = 0; i < util.global.markerArray.length; i++) {
        if (util.global.markerArray[i].title.trim() === this.props.asset.ProjectName.trim()) {
          util.global.markerArray[i].setAnimation(google.maps.Animation.BOUNCE);
          break;
        }
      }
    }
  }
  handleMouseLeave() {
    //console.log('Mouse Leave');
    if (util.global.markerArray && util.global.markerArray.length > 0) {
      for (var i = 0; i < util.global.markerArray.length; i++) {
        if (util.global.markerArray[i].title.trim() === this.props.asset.ProjectName.trim()) {
          util.global.markerArray[i].setAnimation(null);
          break;
        }
      }
    }
  }
  handleMouseOver() {
    //console.log('Mouse Over (ignore)');
  }
  // When the user clicks on a favorite group link in the favorite asset modal
  async handleFavoriteGroupClick(favoriteGroupId) {
    let favoriteGroupAssets = await comm.getFavoriteGroupAssets(favoriteGroupId);
    var existsInFavGrp = false;

    // Check if asset already exists within this favorite group
    for (var i = 0; i < favoriteGroupAssets.data.length; i++) {
      if (favoriteGroupAssets.data[i].AssetId == this.props.asset.AssetId) {
        existsInFavGrp = true;
      }
    }
    // If asset doesn't already exists in the group, attempt to save to group
    if (!existsInFavGrp) {
      var response = await comm.saveFavoriteGroupAsset(favoriteGroupId, this.props.asset.AssetId);

      if (response.success) {
        util.global.favoriteGroupAssets.push(this.props.asset);

        // Hide the create favorite group modal
        $('#favoritesModal').modal('hide');

        // Show the prompt confirming successful addition to favorite group
        document.getElementById('favoritePromptLabel').innerHTML = "Success";
        document.getElementById('favoritePromptText').innerHTML = "Property successfully added to favorite group!";
        $('#favoritePrompt').modal({});

        // Update the state
        this.setState({ favorited: true });
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
  renderFavoriteImageClass() {
    var assetFavorited = false;
    if (util.global.favoriteGroupAssets.length > 0) {
      for (var i = 0; i < util.global.favoriteGroupAssets.length; i++) {
        if (this.props.asset.AssetId == util.global.favoriteGroupAssets[i].AssetId) {
          assetFavorited = true;
        }
      }
    }
    if (this.state.favorited || assetFavorited) {
      return 'asset-favorite-image fa fa-star favorited';
    }
    else {
      return 'asset-favorite-image fa fa-star-o';
    }
  }
  render() {
    let dataPoints = [];
    let title;
    let someAssetData = util.doAllTheMathForAsset(this.props.asset);
    let image = '/Content/images/no-image-available.jpg';
    let pointlessIndex = 0; // react requires a unique key for any list item, in this instance the li's

    if (this.props.asset.Image) image = `/Image.ashx?id=${this.props.asset.AssetId}&name=${this.props.asset.Image.FileName}&width=200&height=200`;;
    //var price = this.props.asset.AskingPrice > 0 ? this.props.asset.AskingPrice : this.props.asset.CurrentBpo;
    // lets always make the first data point the asset type and the second listing status(one day)
    dataPoints.push(e('li',
      {
        key: pointlessIndex += 1,
        className: `listing-status listing-status-${this.props.asset.ListingStatus.toString()}`
      },
      util.enum.listingStatus[this.props.asset.ListingStatus.toString()]));

    // can both be null/empty?
    if (this.props.asset.AskingPrice)
      dataPoints.push(e('li', { key: pointlessIndex += 1 }, [
        e('span', { key: pointlessIndex += 1, className: "asset-data-title" }, `LP:`),
        e('span', { key: pointlessIndex += 1, className: "asset-data" }, ` $${addCommas(this.props.asset.AskingPrice.toString())}`)
      ]));
    else
      dataPoints.push(e('li', { key: pointlessIndex += 1 }, [
        e('span', { key: pointlessIndex += 1, className: "asset-data-title" }, `CMV:`),
        e('span', { key: pointlessIndex += 1, className: "asset-data" }, ` $${addCommas(this.props.asset.CurrentBpo.toString())}`)
      ]));
    dataPoints.push(e('li', { key: pointlessIndex += 1 }, [
      e('span', { key: pointlessIndex += 1, className: "asset-data-title" }, `SQ FT:`),
      e('span', { key: pointlessIndex += 1, className: "asset-data" }, ` ${addCommas(this.props.asset.SquareFeet.toString())}`)
    ]));

    dataPoints.push(e('li', { key: pointlessIndex += 1 }, [
      e('span', { key: pointlessIndex += 1, className: "asset-data-title" }, `SGI:`),
      e('span', { key: pointlessIndex += 1, className: "asset-data" }, ` $${addCommas(someAssetData.SGI.toString())}`)
    ]));

    dataPoints.push(e('li',
      {
        key: pointlessIndex += 1,
        className: `asset-type`
      },
      util.enum.assetType[this.props.asset.AssetType.toString()].asset));

    dataPoints.push(e('li', { key: pointlessIndex += 1 }, [
      e('span', { key: pointlessIndex += 1, className: "asset-data-title" }, `NOI:`),
      e('span', { key: pointlessIndex += 1, className: "asset-data" }, ` $${addCommas(someAssetData.NOI.toString())}`)
    ]));
    dataPoints.push(e('li', { key: pointlessIndex += 1 }, [
      e('span', { key: pointlessIndex += 1, className: "asset-data-title" }, `CAP Rate:`),
      e('span', { key: pointlessIndex += 1, className: "asset-data" }, ` ${(someAssetData.APY).toFixed(2)}%`)
    ]));

    switch (this.props.asset.AssetType) {
      case 3:
      case 5:
        // MF/MHP
        break;
      default:
        // Commercial
        break;
    }

    return e('li', { onMouseEnter: this.handleMouseEnter, onMouseLeave: this.handleMouseLeave, onMouseOver: this.handleMouseOver },
      e('div', { className: 'asset-search-asset', 'id': `A${this.props.asset.AssetId}` },
        e('div', { className: 'asset-search-asset-header' },
          e('div', { className: 'asset-heading' }, `${this.props.asset.City}, ${this.props.asset.State}`),
          e('div', { className: 'asset-favorite-container' },
            e('a', { className: 'asset-favorite-link' },
              e('span', { className: this.renderFavoriteImageClass(), 'id': `acFavorite${this.props.asset.AssetId}`, onClick: this.handleFavoriteClick }, null)))),
        e('a', { className: 'asset-search-asset-image-link', href: '/DataPortal/ViewAsset/' + this.props.asset.AssetId, target: '_blank' },
          e('div', { className: 'asset-search-asset-image' },
            e('img', { src: image }))),
        e('div', { className: 'asset-search-asset-content' },
          e('div', { className: 'content-section-1' }, this.props.asset.ProjectName),
          e('ul', { className: 'content-section-2' }, dataPoints))));
  }
}

export default Asset;
