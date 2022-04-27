import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../assetSearchUtils';
const util = Util.util;

import Asset from './Asset.js';

let e = React.createElement;

export class AssetList extends React.Component {
    render() {
      const rows = [];
      this.props.assets.forEach((asset) => {
        rows.push( e(Asset,{asset:asset,key:asset.AssetId}) );
      })
      if (rows.length === 0) {
        // remove all the pins
        util.clearMarkers();
        return e('div', {className:'asset-search-asset-list-wrapper',onScroll:this.props.onScroll,id:'asl', style:{display:"block"}},
          e('div',{className:'asset-search-asset-list'},
            e('ul',null,
              e('li',{className:'asset-search-asset-list-empty'},"NO ASSETS, try again"))));
      }
      return e('div', {className:'asset-search-asset-list-wrapper',onScroll:this.props.onScroll,id:'asl', style:{display:"block"}},
        e('div',{className:'asset-search-asset-list'},
          e('ul',null, rows)));
    }
  }

export default AssetList;
