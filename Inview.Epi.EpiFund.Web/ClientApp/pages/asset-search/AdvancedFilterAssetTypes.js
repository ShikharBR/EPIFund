import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from './assetSearchUtils';
const util = Util.util;

import NestedCheckBoxListGroup from './components/InputElements/NestedListGroup/NestedCheckBoxListGroup.js';

let assetType = [];
let atEnum = util.enum.assetType;
let astEnum = util.enum.assetSubType;

let mapSubtypes = (subType) => {
  return { 
    label:astEnum[subType].name,
    class:'2',
    'data-id':subType
  }
};
Object.keys(atEnum).forEach((key, index) => {
  assetType.push({
    label:atEnum[key].name,
    class: '1',
    'data-id': key,
    inputs: atEnum[key].subTypes ? atEnum[key].subTypes.map(mapSubtypes) : null 
  });
});

let afAssetTypeClassMap = (item) => {
  switch(item.class){
    case '1': return 'as-asset-type';
    case '2': return 'as-asset-sub-type'
  }
}
export class AdvancedFilterAssetTypes extends React.Component {
  render() {
    return React.createElement(NestedCheckBoxListGroup, {id:'as-asset-type-list', checkState:this.props.checkState, inputs:this.props.assetType, hasSelectAll:true, mapClass:afAssetTypeClassMap})
  }
}

export default AdvancedFilterAssetTypes
