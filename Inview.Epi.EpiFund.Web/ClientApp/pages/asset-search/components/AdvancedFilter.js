import React, { Component } from "react"
import ReactDOM from 'react-dom';

import SelectList from './InputElements/SelectList.js'
import *  as Util from '../assetSearchUtils';
const util = Util.util

import AdvancedFilterAssetTypes from '../AdvancedFilterAssetTypes.js';
import ListGroup from './InputElements/ListGroup/ListGroup.js';
import ListGroupItem from './InputElements/ListGroup/ListGroupItem.js';
import DropdownButton from './InputElements/DropdownButton.js';
import NestedCheckBoxListGroup from './InputElements/NestedListGroup/NestedCheckBoxListGroup.js'

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

let mapUtils = (utilEnum, classname) => {
  var inputs = [];
  Object.keys(utilEnum).forEach((key, ) =>
    inputs.push({
      label: utilEnum[key].name,
      class: classname,
      'data-id': key,
    }));
  return inputs;
}
let utilities = [
  {
    label: 'Interior Road Type',
    class: 'as-int-road',
    'data-id': 'irt',
    inputs: mapUtils(util.enum.gasMeteringMethods, 'as-int-road-types')
  },
  {
    label: 'Access Road Type',
    class: 'as-access-road',
    'data-id': 'art',
    inputs: mapUtils(util.enum.gasMeteringMethods, 'as-access-road-types')
  },
  {
    label: 'Waste Water Type',
    class: 'as-waste-water',
    'data-id': 'wwt',
    inputs: mapUtils(util.enum.gasMeteringMethods, 'as-waste-water-types')
  },
  {
    label: 'Water Service Type',
    class: 'as-water-serv',
    'data-id': 'wst',
    inputs: mapUtils(util.enum.gasMeteringMethods, 'as-water-serv-types')
  },
  {
    label: 'Gas Meter Type',
    class: 'as-gas-meter',
    'data-id': 'gmt',
    inputs: mapUtils(util.enum.gasMeteringMethods, 'as-gas-meter-methods')
  },
  {
    label: 'Electric Meter Type',
    class: 'as-elec-meter',
    'data-id': 'emt',
    inputs: mapUtils(util.enum.gasMeteringMethods, 'as-elec-meter-methods')
  },
];

const PropertyAgeRange = [
  { label: 'Select Property Age Range', value: '' },
  { label: '5 Years', value: 5 },
  { label: '10 Years', value: 10 },
  { label: '15 Years', value: 15 },
  { label: '20 Years', value: 20 },
  { label: '30 Years', value: 30 },
  { label: '40 Years', value: 40 },
]
const OperatingStatus = [
  { label: 'Select Operating Status', value: '' },
  { label: 'FDIC', value: 1 },
  { label: 'In Default - FDIC Control', value: 2 },
  { label: 'Pending Forclosure', value: 3 },
  { label: 'Private - In Default', value: 4 },
  { label: 'Private - Not In Default', value: 0 },
  { label: 'REO', value: 5 },

]
const PropOfferingAssumFinance = [
  { label: '', value: '0' },
  { label: 'Required', value: '1' },
  { label: 'Not Required', value: '2' },
]

const GradeClassVals = [
  "A+",
  "A",
  "A-",
  "B+",
  "B",
  "B-",
  "C+",
  "C",
  "C-",
  "D+",
  "D",
];

const NoteOriginalAmortization = [
  { label: 'Interest Only', dataId: 0 },
  { label: '10 Year Amortization', dataId: 10 },
  { label: '15 Year Amortization', dataId: 15 },
  { label: '20 Year Amortization', dataId: 20 },
  { label: '25 Year Amortization', dataId: 25 },
  { label: '30 Year Amortization', dataId: 30 },
];

const NotePosition = [
  { label: '1st Position, Institutional', dataId: '' },
  { label: '1st Position, Private Party Beneficiary', dataId: '' },
  { label: '2nd Position, Institutional', dataId: '' },
  { label: '2nd Position, Private Party Beneficiary', dataId: '' },
  { label: 'Wrap Around, encompossing 1 Mortgage', dataId: '' },
  { label: 'Wrap Around, encompossing 2 Mortgages', dataId: '' },
]
const NoteMaturitySchedule = [
  { label: 'Fully Amortized – No Maturity Date is Acceptable', dataId: '' },
  { label: '3 Years from date of Search', dataId: '' },
  { label: '3 → 6 Years from date of Search', dataId: '' },
  { label: '6 → 10 Years from date of Search', dataId: '' },
  { label: '10 Years from date of Search', dataId: '' },
];

const NoteSecuringMortageInstrument = [
  { label: 'Deed of Trust', dataId: '' },
  { label: 'Mortgage', dataId: '' },
  { label: 'Land Contract', dataId: '' },
  { label: 'Contract for Deed', dataId: '' },
  { label: 'All Inclusive Wrap Around', dataId: '' },
  { label: 'All Types', dataId: '' },
]

const NoteLoanBalanceToLPCMV = [
  { label: '90%-100%+', dataId: '' },
  { label: '80%-89.99%', dataId: '' },
  { label: '70%-79.99%', dataId: '' },
  { label: '60%-69.99%', dataId: '' },
  { label: '50%-59.99%', dataId: '' },
  { label: 'N/A', dataId: '' },
]

const VacancyFactor = [
  { label: 'less than or equal to 9.90%', dataId: '0' },
  { label: '10% to 14.99%', dataId: '10' },
  { label: '15% to 19.99%', dataId: '15' },
  { label: '20% to 29.99%', dataId: '20' },
  { label: '30% to 39.99%', dataId: '30' },
  { label: 'greater than or equal to 40%', dataId: '40' },
]
const RentableSpace = [
  { label: 'less than or equal to 99', dataId: '0' },
  { label: '100-199', dataId: '100' },
  { label: '200-399', dataId: '200' },
  { label: '400 or more', dataId: '400' },
]

const advancedToQuickMap = {
  'txtAdvCity': 'txtCity',
  'txtAdvState': 'txtState',
  'txtAdvCounty': 'txtCounty',
  'txtAdvancedSearchAssetName': 'txtQuickSearchAssetName',
  'txtAdvancedSearchMin': 'txtQuickSearchMin',
  'txtAdvancedSearchMax': 'txtQuickSearchMax',
  'txtCapRate': 'txtQuickSearchCapRate',
  'txtYearBuilt': 'txtQuickSearchYearBuilt',

}
export class AdvancedFilter extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isUpdatedChecked: false,
    };
    this.onMYUpdatedClick = this.onMYUpdatedClick.bind(this);
  }
  stopPropEvent(e) {
    e.stopPropagation();
  }
  onTextBoxBlur(e) {
    let target = e.currentTarget;
    let ele = document.getElementById(advancedToQuickMap[target.id]);
    if (ele != null)
      ele.value = target.value;
  }
  onMYUpdatedClick(e) {
    // this.setState({
    //   isUpdatedChecked: e.currentTarget.checked
    // }) 
    let minYearUpdated = document.getElementById('txtMinYearUpdated').parentElement.parentElement;
    let updatedByOwnerCB = document.getElementById('cbYearBuiltByOwner');
    if (!e.currentTarget.checked) {
      minYearUpdated.style.display = 'none';
      updatedByOwnerCB.disabled = true;
    } else {
      minYearUpdated.style.display = '';
      updatedByOwnerCB.disabled = false;
    }

  }

  render() {
    return [
      // Saved Searches
      e(AdvancedFilterColumn, { key: 'savedSearches', className: 'saved-searches' },
        e('div', { id: 'advancedSavedSearchList', className: 'saved-searches-list' })
      ),
      // Column 1
      e(AdvancedFilterColumn, { key: 'col1', className: 'section-0' },
        [
          // Location
          e(AdvancedFilterElement, { label: 'City', key: 'as-city' },
            e('input', { id: 'txtAdvCity', placeholder: 'City', onBlur: this.onTextBoxBlur })),
          e(AdvancedFilterElement, { label: 'State', key: 'as-state' },
            e('input', { id: 'txtAdvState', placeholder: 'State', onBlur: this.onTextBoxBlur })),
          e(AdvancedFilterElement, { label: 'County', key: 'as-county' },
            e('input', { id: 'txtAdvCounty', placeholder: 'County', onBlur: this.onTextBoxBlur })),
          // Asset Name
          e(AdvancedFilterElement, { label: 'Asset Name', key: 'as-assetname' },
            e('input', { id: 'txtAdvancedSearchAssetName', placeholder: 'Asset Name', onBlur: this.onTextBoxBlur })),
          // Asset Types
          e(AdvancedFilterElement, { label: 'Asset Type', key: 'as-assettype' },
            e(AdvancedFilterAssetTypes, { assetType: assetType })),
        ]),
      // Column 2
      e(AdvancedFilterColumn, { key: 'col2', className: 'section-1' },
        [
          // Price (min/max)
          e(AdvancedFilterElement, { label: 'Price Range', key: 'Price Range' },
            e('input', { id: 'txtAdvancedSearchMin', key: 'txtAdvancedSearchMin', placeholder: 'Min', className: 'non-react-money int-i', onBlur: this.onTextBoxBlur }),
            'to',
            e('input', { id: 'txtAdvancedSearchMax', key: 'txtAdvancedSearchMax', placeholder: 'Max', className: 'non-react-money int-i', onBlur: this.onTextBoxBlur }),
          ),
          // Proprty Age Range
          e(AdvancedFilterElement, { label: 'Property Age Range', dataAt: 'fsa', key: 'Property Age Range' },
            e(SelectList, { id: 'ddlPropertyAge', className: 'ddl-i', options: PropertyAgeRange })
          ),
          // Listed Asset Min CAP Rate
          e(AdvancedFilterElement, { label: 'Listed Asset Min CAP Rate', key: 'Listed Asset Min CAP Rate' },
            e('div', { className: 'input-group' },
              e('input', { placeholder: 'CAP Rate', id: 'txtCapRate', className: 'int-i form-control', style: { marginTop: '0px' }, onBlur: this.onTextBoxBlur })),
            e('span', { className: 'input-group-addon greystyle', height: '35px' }, '%')
          ),
          // sqft
          e(AdvancedFilterElement, { label: 'Square Feet', key: 'Square Feet' },
            e('input', { id: 'txtSquareFeetMin', placeholder: 'Min', className: 'int-i' }),
            'to',
            e('input', { id: 'txtSquareFeetMax', placeholder: 'Max', className: 'int-i' }),
          ),
          // Num Units
          e(AdvancedFilterElement, { label: 'Number of Units', dataAt: 'mf', key: 'Number of Units' },
            e('input', { id: 'txtUnitsMin', placeholder: 'Min', className: 'int-i' }),
            'to',
            e('input', { id: 'txtUnitsMax', placeholder: 'Max', className: 'int-i' }),
          ),
          // Unit Mix Ratio
          e(AdvancedFilterElement, { label: 'Unit Mix Ratio', dataAt: 'mf', className: 'as-umr-group-cont', key: 'Unit Mix Ratio' },
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtUmrStudioPct' }), 'Studio'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtUmr1BdPct' }), '1BD'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtUmr2Bd1BaPct' }), '2BD/1BA'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtUmr2Bd2BaPct' }), '2BD/2BA'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtUmr3BdPct' }), '3BD'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtUmrOccuPerc' }), 'Occ. %'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtUmrMaxFCPerc' }), 'FC Max %'),
            util.addQSLabel(e(DropdownButton, { label: 'Utilities', id: 'as-utils-dd' },
              e(NestedCheckBoxListGroup, { inputs: utilities })), 'Utilities'),

          ),
          // Space Mix Ratio
          e(AdvancedFilterElement, { label: 'Space Mix Ratio', dataAt: 'mhp', key: 'Space Mix Ratio' },
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtSmrSWidePct' }), 'S-Wide'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtSmrDWidePct' }), 'D-Wide'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtSmrTWidePct' }), 'T-Wide'),
            //util.addQSLabel(e('input', { type:'text', className:'smallinput', id:'cbSmrParkOwnedUnits' }),'Park Owned Units'),
            util.addQSLabel(e('select', { id: 'ddlSmrParkOwnedUnits' },
              e('option', { value: '1' }, "N/A"),
              e('option', { value: '2' }, "Yes"),
              e('option', { value: '3' }, "No")),
              'Park Owned Units'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtSmrParkOwnedMaxPct', placeholder: 'Max' }), ''),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'cbSmrPropWithUndevAcres' }), 'Property with Undeveloped Acres'),
            util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtSmrUndevTotAcrePct' }), '% of unDev. Land to total acreage'),
          ),
          // grade class
          e(AdvancedFilterElement, { label: 'Grade Classification', key: 'Grade Classification' },
            React.createElement(DropdownButton, { label: 'Grade Classification' },
              React.createElement(ListGroup, {},
                GradeClassVals.map(gradeClass => React.createElement(ListGroupItem, { type: 'a', key: gradeClass, onClick: util.listItemClick },
                  React.createElement('input', { className: "as-grade-classification", type: 'checkbox', 'data-id': gradeClass, onClick: this.stopPropEvent }), gradeClass))
              ))
          ),
          //Min Year Built / Updated
          e(AdvancedFilterElement, { key: 'Year' },

            e('div', { key: 'yearbuiltcont', className: 'as-yearbuilt-cont', 'data-at': 'all' },
              util.addQSLabel(e('input', { type: 'text', id: 'txtYearBuilt', className: 'smallinput greystyle', onBlur: this.onTextBoxBlur }), 'Min. Year Built'),
              e('div', { className: 'yb-checkbox' },
                e('div', {},
                  e('input', { type: 'checkbox', onChange: this.onMYUpdatedClick, id: 'cbYearBuiltUpdated' }),
                  'Updated'),
                e('div', {},
                  e('input', { type: 'checkbox', disabled: true, id: 'cbYearBuiltByOwner' }),
                  'By Owner')
              ),
              e('div', { style: { display: 'none' } },
                util.addQSLabel(e('input', { type: 'text', id: 'txtMinYearUpdated', className: 'smallinput greystyle' }), 'Min Year Updated')
              ),
            ),
          ),
          // original Amortization Schedule of Note
          e(AdvancedFilterElement, { label: 'Original Amortization Schedule of Note', dataAt: 'cpn', key: 'Original Amortization Schedule of Note' },
            React.createElement(ListGroup, {},
              NoteOriginalAmortization.map(option => React.createElement(ListGroupItem, { type: 'a', key: option.dataId, onClick: util.listItemClick },
                React.createElement('input', { className: "as-amortization-schedule", type: 'checkbox', 'data-id': option.dataId, onClick: this.stopPropEvent }), option.label))
            ),
          ),
          // Major Tenant
          e(AdvancedFilterElement, { key: 'Major Tenant' },
            e('div', { key: 'majortenantcont', className: 'as-dp as-major-tenant-cont', 'data-at': 'com' },
              util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtOccuPerc' }), 'Min. Occ %'),
              util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtMajorTenant' }), 'Major Tenant'),
              util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtMajorTenantOccuPerc' }), 'Major Tenant % Occ'),
              util.addQSLabel(e('input', { type: 'text', className: 'smallinput greystyle', id: 'txtMajorTenantLeaseExp' }), 'Major Tenant Lease Exp')
            ),
          ),
        ]),
      // Column 3
      e(AdvancedFilterColumn, { key: 'col3', className: 'section-2', },
        [
          // Operating Status
          e(AdvancedFilterElement, { label: 'Operating Status', dataAt: 'fsa', key: 'Operating Status' },
            e(SelectList, { id: 'ddlOperatingStatus', className: 'ddl-i', options: OperatingStatus })
          ),

          // Property Offering Assumable Financing
          e(AdvancedFilterElement, { label: 'Property Offering Assumable Financing', dataAt: 'fsa', key: 'Property Offering Assumable Financing' },
            e(SelectList, { id: 'ddlAssumableFinancing', className: 'ddl-i', options: PropOfferingAssumFinance })
          ),
          // PSGI
          e(AdvancedFilterElement, { label: 'PSGI', dataAt: 'com', key: 'PSGI' },
            e('div', { className: 'input-group' },
              e('input', { id: 'txtSGI', className: 'non-react-money int-i form-control', placeholder: 'PSGI' })
            ),
          ),
          // PNOI
          e(AdvancedFilterElement, { label: 'PNOI', dataAt: 'com', key: 'PNOI' },
            e('div', { className: 'input-group' },
              e('input', { id: 'txtNOI', className: 'non-react-money int-i form-control', placeholder: 'PNOI' })
            ),
          ),
          // Maturity Schedule/Ballon Date
          e(AdvancedFilterElement, { label: 'Maturity Schedule (Ballon Date) of Note', dataAt: 'cpn', key: 'MSON' },
            React.createElement(ListGroup, {},
              NoteMaturitySchedule.map((option, index) => React.createElement(ListGroupItem, { type: 'a', key: 'MSON-option--' + index, onClick: util.listItemClick },
                React.createElement('input', { className: "as-maturity-schedule", type: 'checkbox', 'data-id': option.dataId, onClick: this.stopPropEvent }), option.label))
            ),
          ),
          // Position of Note
          e(AdvancedFilterElement, { label: 'Position of Note', dataAt: 'cpn', key: 'PON' },
            React.createElement(ListGroup, {},
              NotePosition.map((option, index) => React.createElement(ListGroupItem, { type: 'a', key: 'PON-option--' + index, onClick: util.listItemClick },
                React.createElement('input', { className: "as-note-position", type: 'checkbox', 'data-id': option.dataId, onClick: this.stopPropEvent }), option.label))
            ),
          ),
          // Type of Securing Mortage Instrument
          e(AdvancedFilterElement, { label: 'Type of Securing Mortage Instrument', dataAt: 'cpn', key: 'YOSMI' },
            React.createElement(ListGroup, {},
              NoteSecuringMortageInstrument.map((option, index) => React.createElement(ListGroupItem, { type: 'a', key: 'YOSMI-option--' + index, onClick: util.listItemClick },
                React.createElement('input', { className: "as-note-mortgage-instrument", type: 'checkbox', 'data-id': option.dataId, onClick: this.stopPropEvent }), option.label))
            ),
          ),
          // Loan Balance of Note % to Listed Price/CMV
          e(AdvancedFilterElement, { label: 'Loan Balance of Note % to Listed Price/CMV', dataAt: 'cpn', key: 'LBON' },
            React.createElement(ListGroup, {},
              NoteLoanBalanceToLPCMV.map((option, index) => React.createElement(ListGroupItem, { type: 'a', key: 'LBON-option--' + index, onClick: util.listItemClick },
                React.createElement('input', { className: "as-loan-balance-lpcmv", type: 'checkbox', 'data-id': option.dataId, onClick: this.stopPropEvent }), option.label))
            ),
          ),
          // Vacancy Factor
          e(AdvancedFilterElement, { label: 'Vacancy Factor', dataAt: 'com', key: 'VF' },
            React.createElement(ListGroup, {},
              VacancyFactor.map((option, index) => React.createElement(ListGroupItem, { type: 'a', key: 'VF-option--' + index, onClick: util.listItemClick },
                React.createElement('input', { className: "as-vacancy-factor", type: 'checkbox', 'data-id': option.dataId, onClick: this.stopPropEvent }), option.label))
            )
          ),
          // Rentable Space
          e(AdvancedFilterElement, { label: 'Rentable Space', dataAt: 'com', key: 'RS' },
            React.createElement(ListGroup, {},
              RentableSpace.map((option, index) => React.createElement(ListGroupItem, { type: 'a', key: 'RS-option--' + index, onClick: util.listItemClick },
                React.createElement('input', { className: "as-rentable-space", type: 'checkbox', 'data-id': option.dataId, onClick: this.stopPropEvent }), option.label))
            ),
          ),
        ]),
    ];
  }
}

export default AdvancedFilter;

class AdvancedFilterColumn extends React.Component {
  constructor(props) {
    super(props);
    this.getClass = this.getClass.bind(this);
  }
  getClass() {
    if (typeof this.props.className === 'undefined')
      return 'flex-section'
    return `flex-section ${this.props.className}`;
  }
  render() {
    return e('div', { className: this.getClass() },
      this.props.children);
  }
}
class AdvancedFilterElement extends React.Component {
  constructor(props) {
    super(props);
    this.getClass = this.getClass.bind(this);
    this.getDataAt = this.getDataAt.bind(this);
  }
  getClass() {
    if (typeof this.props.className === 'undefined')
      return 'as-dp'
    return `as-dp ${this.props.className}`;
  }
  getDataAt() {
    if (typeof this.props.dataAt === 'undefined')
      return 'all'
    return this.props.dataAt
  }
  render() {
    return e('div', { className: this.getClass(), 'data-at': this.getDataAt(), 'data-is': ' ', 'data-not': ' ' },
      e('label', { className: 'as-label' }, this.props.label),
      this.props.children,
    )
  }
}