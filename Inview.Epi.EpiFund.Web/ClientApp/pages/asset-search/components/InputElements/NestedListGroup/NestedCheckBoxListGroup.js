import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from '../../../assetSearchUtils';
const util = Util.util;

import Checkbox from '../InputTypes/Checkbox.js';
import ListGroup from '../ListGroup/ListGroup.js';
import ListGroupItem from '../ListGroup/ListGroupItem.js';

let e = React.createElement;
/* props:
    inputs: [array of type item:
              item: { label: string
                      data-id: string
                      [optional
                        class: string
                        inputs: [array of type 'item']]
              }],
    [optional:
      checkState: array matching structure of
      onChange: onchange of checkstate
      mapClass: function that will be called on every item to generate a class.
      id: string,
      className: string,
      hasSelectAll: bool = false]
*/

export class NestedCheckBoxListGroup extends React.Component {
  constructor(props) {
    super(props);
    this.mapInputs = this.mapInputs.bind(this);
    this.onCheckboxChange = this.onCheckboxChange.bind(this);
    this.createSelectAll = this.createSelectAll.bind(this);
    this.onCBClick = this.onCBClick.bind(this);
  }

  getClass() {
    return this.props.className ? `list-group-root ${this.props.className}` : 'list-group-root'
  }
  createSelectAll(children) {
    if (this.props.hasSelectAll == null || this.props.hasSelectAll == false)
      return children;

    let item = { label: 'Select All', class: 'qs-select-all' }
    return e('div', { className: 'list-group-item-container' },
      e(ListGroupCheckboxItem, { item: item }),
      e(ListGroup, { className: 'list-group-select-all', padding: '0', margin: '0' },
        children
      )
    );
  }
  setAllChildren(listGroupContainer, state) {
    listGroupContainer.children().find('[type=checkbox]').prop('checked', checked);
  }
  onCBClick(event) {
    if (typeof this.props.onClick !== 'undefined')
      this.props.onClick(event);
  }
  onCheckboxChange(event) {
    if (typeof this.props.onChange !== 'undefined') {
      this.props.onChange(event);
    }
    if (this.props.handleChange) {
      this.props.handleChange();
    }
  }
  mapInputs(inputs, index = 0) {
    return inputs.map(input =>
      e('div', { key: input['data-id'], className: 'list-group-item-container' },
        e(ListGroupCheckboxItem, {
          key: 'CB', item: input,
          onClick: this.onCheckBoxClick, onChange: this.onCheckboxChange,
          mapClass: this.props.mapClass
        }),
        input.inputs ? e(ListGroup, { className: 'collapse' }, this.mapInputs(input.inputs, index + 1)) : null
      )
    );
  }
  render() {
    return e(ListGroup, { id: this.props.id, className: this.getClass() },
      this.createSelectAll(this.mapInputs(this.props.inputs)));
  }
}

export default NestedCheckBoxListGroup


/* props:
    item: { label: string,
            class: string,
            data-id: string,
    }

    onClick: callback fn, called on checkbox click
*/
class ListGroupCheckboxItem extends React.Component {
  constructor(props) {
    super(props);

    this.getCaret = this.getCaret.bind(this);
    this.onListItemClick = this.onListItemClick.bind(this);
    this.onCaretClick = this.onCaretClick.bind(this);
    this.getCheckBoxClass = this.getCheckBoxClass.bind(this);
    this.onCBClick = this.onCBClick.bind(this);
    this.onCBChange = this.onCBChange.bind(this);
  }
  getCaret() {
    const caretStyle = {
      outline: 'none',
      display: 'inline-block',
      height: '100%',
      float: 'right',
      backgroundColor: 'transparent',
      padding: '0 10px 0 10px',
      border: 'none',
    };
    return this.props.item.inputs == null ? null :
      React.createElement('div', { className: 'list-group-dropdown-btn-container' },
        React.createElement('button', {
          onClick: this.onCaretClick,
          className: 'list-group-dropdown-btn', 'style': caretStyle
        },
          React.createElement('span', { className: 'caret' })));
  }

  getCheckBoxClass() {
    if (this.props.mapClass) {
      return this.props.mapClass(this.props.item);
    }
    return this.props.item.class
  }
  setAllChildren($listGroupContainer, checked) {
    let $children = $listGroupContainer.find('[type=checkbox]');

    $children.each((i, element) => {
      if (element.checked != checked) {
        element.checked = checked;
        $(element).change();
      }
    })
  }
  setParentCheckBox($listGroupContainer, checkState) {
    let parentListGroup = $listGroupContainer.closest('.list-group');

    if (parentListGroup.hasClass('list-group-root'))
      return;

    let parentCheckbox = parentListGroup.siblings('.list-group-item').find('[type=checkbox]');

    let allChildrenSet = true;
    let setCount = 0;

    parentListGroup.find('[type=checkbox]').each((item, checkbox) => {
      allChildrenSet &= checkbox.checked;
      if (checkbox.checked || checkbox.indeterminate)
        setCount++;
      else if (setCount > 0)
        return false;
    });

    parentCheckbox.prop('indeterminate', setCount > 0 && !allChildrenSet);

    if (parentCheckbox.prop('checked') == checkState)
      return;

    if (allChildrenSet || !checkState) {
      parentCheckbox.prop('checked', checkState);
      this.setParentCheckBox(parentListGroup.closest('.list-group-item-container'), checkState);
    }
  }
  onCBClick(event) {
    if (this.props.onClick) this.props.onClick(event);
    let currentState = event.currentTarget.checked;
    let $listGroupContainer = $(event.currentTarget).closest('.list-group-item-container');
    this.setAllChildren($listGroupContainer, currentState)
    this.setParentCheckBox($listGroupContainer, currentState)
    event.stopPropagation();
  }
  onCBChange(event) {
    util.global.selectedAssetType = undefined;
    if (this.props.onChange) this.props.onChange(event);
    util.updateAssetTypeBoxLabel();
  }
  onListItemClick(event) {
    $(event.currentTarget).find('input').click();
    event.stopPropagation();
  }
  onCaretClick(event) {
    let $listGroupContainer = $(event.currentTarget).closest('.list-group-item-container');
    $listGroupContainer.children('.list-group').toggleClass('collapse');
    event.stopPropagation();
  }

  render() {
    const lgeadStyle = {
      display: 'flex',
      flexDirection: 'row',
      width: '100%',
    }
    var lgecStyle = {
      flexGrow: '1',
    }
    return React.createElement(ListGroupItem, { type: 'a', onClick: this.onListItemClick },
      React.createElement('div', { className: 'list-group-element-and-dropdown', style: lgeadStyle },
        React.createElement('div', { className: 'list-group-element-container', style: lgecStyle },
          React.createElement(Checkbox, {
            label: this.props.item.label, onClick: this.onCBClick, onChange: this.onCBChange,
            dataID: this.props.item['data-id'], className: this.getCheckBoxClass()
          }, null)),
        this.getCaret()));
  }
}
