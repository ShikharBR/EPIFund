import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../assetSearchUtils';
const util = Util.util;
import * as Comm from '../comm';
const comm = Comm.comm;

let e = React.createElement;

// The individual saved search tab
export class AdvancedSavedSearch extends React.Component {
    constructor(props) {
      super(props);
    }

    render() {
      return e(
        'a',
        {
          // If this tab is active, change the class
          className: this.props.handleIsActive ? 'list-group-item active' : 'list-group-item',
          'role':'tab',
          'data-id':this.props.search.Id,
          onClick: this.props.onActiveTab,
        },
        this.props.search.Title
      );
    }
}

export default AdvancedSavedSearch;
