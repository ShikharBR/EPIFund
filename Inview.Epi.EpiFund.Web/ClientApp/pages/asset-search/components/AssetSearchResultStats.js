import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../assetSearchUtils';
const util = Util.util;

export class AssetSearchResultStats extends React.Component {
  generateBox(content, title, special) {
    return React.createElement('div', {className:'dashboard-box col'},
        React.createElement('h4', { className: (special === "green") ? "dashboard-box__content greenmoneytext" : 'dashboard-box__content'}, content),
            React.createElement('p', {className:'dashboard-box__title'}, title));
  }

  formatAssetsCount(count) {
    return Number(count).toLocaleString();
  }
  formatParticipants(count) {
    return Number(count).toLocaleString();
  }
  formatAssetVal(value) {
    return '$' + util.intToShortString(value);
  }
  formatMFU (count) {
    return Number(count).toLocaleString();
  }
  formatSqFt(value) {
    return Number(value).toLocaleString();
  }

  render(){
    return React.createElement('div', {className:'asset-search-result-stats row'} ,
                this.generateBox(this.formatAssetsCount(this.props.totalAssets), "Total Assets"),
                this.generateBox(this.formatAssetsCount(this.props.publishedAssets), "Published Assets"),
                this.generateBox(this.formatAssetVal(this.props.valInLPCMVAssets), "Market Value", "green"),
                this.generateBox(this.formatMFU(this.props.multiFamUnitCnt), "Multi-Family Assets"),
                this.generateBox(this.formatSqFt(this.props.creSqFt), "Leasable CRE Sq. Ft."));
  }
}

export default AssetSearchResultStats;
