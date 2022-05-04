import React, { Component } from "react"
import ReactDOM from 'react-dom';

import *  as Util from './assetSearchUtils';
const util = Util.util;
import AdvancedFilter from "./components/AdvancedFilter";

$(window).load(function() {
  ReactDOM.render(React.createElement(AdvancedFilter, {}), document.getElementById('AdvFilterReactRoot'));
  setupBindings();
  renderAdvancedSearch();
  setMoneyListeners();
})
export const fieldLocation = { quick: 0, advanced: 1 };
export const advFieldSection = { all: 0, fsa: 1, cpn: 2, mf: 3, com: 4 };

export const filterFields = {
  /* --- Quick Filters --- */
  'txtCity':               { active: true, loc: fieldLocation.quick, type: 'string', name: 'city' },
  'txtState':               { active: true, loc: fieldLocation.quick, type: 'string', name: 'state' },
  'txtCounty':               { active: true, loc: fieldLocation.quick, type: 'string', name: 'county' },

  'txtQuickSearchMin':         { active: true, loc: fieldLocation.quick, type: 'money',  name: 'Min' },
  'txtQuickSearchMax':         { active: true, loc: fieldLocation.quick, type: 'money',  name: 'Max' },
  'txtQuickSearchCapRate':     { active: true, loc: fieldLocation.quick, type: 'float',  name: 'CapRate' },
  'ddlPriceSearchType':        { active: true, loc: fieldLocation.quick, type: 'ddl',    name: 'PriceSearchType', default: 2},
  '.qs-asset-type':            { active: true, loc: fieldLocation.quick, type: 'cb-list',    name: 'AssetTypes', listType: 'int',   listId: 'data-id' },
  '.qs-asset-sub-type':        { active: true, loc: fieldLocation.quick, type: 'cb-list',    name: 'AssetSubTypes', listType: 'int',   listId: 'data-id' },
  'txtQuickSearchYearBuilt':   { active: true, loc: fieldLocation.quick, type: 'int',    name: 'YearBuilt' },
  'txtQuickSearchAssetName':   { active: true, loc: fieldLocation.quick, type: 'string', name: 'AssetName'},

  /* --- Advanced Filters --- */
  'cbYearBuiltUpdated':       { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'IsUpdatedChecked',      type: 'cb', },
  'cbYearBuiltByOwner':       { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'IsUpdatedByOwner',      type: 'cb', },
  // Only show when 'updated' check
  'txtMinYearUpdated':        { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'MinYearUpdated',        type: 'string', },
  // Leaseable square feet === SqFt?

  // Major Tenant
  'txtMajorTenant':           { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'MajorTenant',           type: 'string', },
  // Major Tenant Occ%
  'txtMajorTenantOccuPerc':   { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'MajorTenantOccuPerc',   type: 'int', },
  // Major tenant lease expiration
  'txtMajorTenantLeaseExp':   { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'MajorTenantLeaseExp',   type: 'int' },

  // UNIT MIX RATIO GROUP (only multi-family?)
    // Studio/1BD/2BD-1BA/2BD-2BA/3BD
  'txtUmrStudioPct':          { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'UmrStudioPct',          type: 'int', },
  'txtUmr1BdPct':             { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'Umr1BdPct',             type: 'int', },
  'txtUmr2Bd1BaPct':          { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'Umr2Bd1BaPct',          type: 'int', },
  'txtUmr2Bd2BaPct':          { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'Umr2Bd2BaPct',          type: 'int', },
  'txtUmr3BdPct':             { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'Umr3BdPct',             type: 'int', },
    // OCC %
  'txtUmrOccuPerc':           { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'UmrOccuPerc',           type: 'int', },
    // FC Max %
  'txtUmrMaxFCPerc':          { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'UmrMaxFCPerc',          type: 'int', },
    // Utilites
  '.as-int-road-types':       { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'InteriorRoadTypes',     type: 'cb-list', listType: 'int',   listId: 'data-id', },
  '.as-access-road-types':    { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'AccessRoadTypes',       type: 'cb-list', listType: 'int',   listId: 'data-id', },
  '.as-waste-water-types':    { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'WasteWaterTypes',       type: 'cb-list', listType: 'int',   listId: 'data-id', },
  '.as-water-serv-types':     { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'WaterServTypes',        type: 'cb-list', listType: 'int',   listId: 'data-id', },
  '.as-gas-meter-methods':    { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'ElectricMeteringMethods', type: 'cb-list', listType: 'int',   listId: 'data-id', },
  '.as-elec-meter-methods':   { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'GasMeteringMethods', type: 'cb-list', listType: 'int',   listId: 'data-id', },


  // SPACE MIX RATIO GROUP
    // S-Wide/D-Wide/T-Wide PCT
  'txtSmrSWidePct':           { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SmrSWidePct',           type: 'int', },
  'txtSmrDWidePct':           { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SmrDWidePct',           type: 'int', },
  'txtSmrTWidePct':           { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SmrTWidePct',           type: 'int', },
    // Park Owned Units/Max %
  'ddlSmrParkOwnedUnits':     { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SmrParkOwnedUnits',    type: 'ddl', },
  'txtSmrParkOwnedMaxPct':    { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SmrParkOwnedMaxPct',    type: 'int', },
    // Property w/ Undeveloped Acres
  //'cbSmrPropWithUndevAcres':  { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SmrPropWithUndevAcres', type: 'cb-list', listType: 'int',   listId: 'data-id' },
    // % of undev land to total acres
  'txtSmrUndevTotAcrePct':    { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SmrUndevToTotalAcreRatioPct', type: 'int', },

  // GRADE CLASS GOES HERE
  // Probably more that are missing

  // Prev

  'txtAdvancedSearchAssetName':{ active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'AssetName',            type: 'string', },
  '.as-asset-type':            { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'AssetTypes',           type: 'cb-list', listType: 'int',   listId: 'data-id', },
  '.as-asset-sub-type':        { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'AssetSubTypes',        type: 'cb-list', listType: 'int',   listId: 'data-id', },
  'txtAdvancedSearchMin':      { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'Min',                  type: 'money', },
  'txtAdvancedSearchMax':      { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'Max',                  type: 'money', },
  'txtCapRate':                { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'CapRate',              type: 'float', },
  '.as-grade-classification':  { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'GradeClassifications', type: 'cb-list', listType: 'string',listId: 'data-id', },
  //'ddlListingType':            { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'ListingType',          type: 'string', },
  'txtYearBuilt':              { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'YearBuilt',            type: 'int', },

  'ddlOperatingStatus':        { active: true, loc: fieldLocation.advanced, section: advFieldSection.fsa, name: 'OperatingStatus',      type: 'ddl', },
  'ddlAssumableFinancing':     { active: true, loc: fieldLocation.advanced, section: advFieldSection.fsa, name: 'AssumableFinancing',   type: 'string', },

  'txtUnitsMin':               { active: true, loc: fieldLocation.advanced, section: advFieldSection.mf,  name: 'UnitsMin',             type: 'int', },
  'txtUnitsMax':               { active: true, loc: fieldLocation.advanced, section: advFieldSection.mf,  name: 'UnitsMax',             type: 'int', },

  'txtSquareFeetMin':          { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SquareFeetMin',        type: 'int', },
  'txtSquareFeetMax':          { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SquareFeetMax',        type: 'int', },
  'txtSGI':                    { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'SGI',                  type: 'money', },
  'txtNOI':                    { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'NOI',                  type: 'money', },
  'txtOccuPerc':               { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'OccPerc',              type: 'int', },
  '.as-vacancy-factor':        { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'VacancyFactor',        type: 'cb-list', listType: 'int',   listId: 'data-id', },
  '.as-rentable-space':        { active: true, loc: fieldLocation.advanced, section: advFieldSection.all, name: 'RentableSpace',        type: 'cb-list', listType: 'int',   listId: 'data-id', },

  '.as-amortization-schedule': { active: true, loc: fieldLocation.advanced, section: advFieldSection.cpn, name: 'AmortizationSchedule', type: 'cb-list', listType: 'int',   listId: 'data-id', },
};

export function setMoneyListeners() {
 $('.non-react-money').on('change keyup paste', (e) =>
 {
     if (util != null) {
         util.onMoneyChange(e); util.onMoneyKeyUp(e);
     }
 });
}

export function setupBindings() {
  $('#btnAdvancedSearch').on('click', function() {
    $('#myModal').modal('hide');
  })
  $('.as-asset-type').change(renderAdvancedSearch);
}

export function renderAdvancedSearch() {
  // cpn = Certified Private Note, fsa = Fee Simple Asset
  // hide cpn if 16 isnt checked
  // hide fsa if cpn is checked
  // show cpn and fsa if 16 and anything else is checked?
  // show absolutely everything if all asset types are checked?
  const groups = aggregatePopulatedAssetTypes();
  let elements = document.querySelectorAll('.as-dp');
  Array.prototype.forEach.call(elements, (el, i) => {
    switch(el.attributes['data-at'].value) {
      case "fsa":
        if (groups.fsa) el.style.display = '';
        else el.style.display = 'none';
        break;
      case "cpn":
        if (groups.cpn) el.style.display = '';
        else el.style.display = 'none';
        break;
      case "mf":
        if (groups.mf) el.style.display = '';
        else el.style.display = 'none';
        break;
      case "com":
        if (groups.com) el.style.display = '';
        else el.style.display = 'none';
        break;
      case "mhp":
        if (groups.mhp) el.style.display = '';
        else el.style.display = 'none';
        break;
      case "all":
        // do nothing
        break;
      default:
        throw `attribute 'data-at' value ${el.attributes['data-at'].value} not supported`;
    }
  });
}

export function aggregatePopulatedAssetTypes() {
  let assetTypeGroups = { fsa: false, cpn: false, mf: false, com: false, mhp: false };
  let elements = document.querySelectorAll('.as-asset-type');
  Array.prototype.forEach.call(elements, (el, i) => {
    if (el.checked) {
      if (el.attributes['data-id'].value == 3) assetTypeGroups.mf = true;
      else if(el.attributes['data-id'].value == 5) assetTypeGroups.mhp = true;
      else if (el.attributes['data-id'].value == 16) assetTypeGroups.cpn = true;
      else {
        // assume commercial and fsa is all thats left
        assetTypeGroups.fsa = true;
        assetTypeGroups.com = true;
      }
    }
  });
  return assetTypeGroups;
}

export function clearFilterValues() {
    Object.keys(filterFields).forEach((key, index) => {
        if (document.getElementById(key) != null) {
            switch (filterFields[key].type) {
                case "string":
                case "int":
                case "money":
                case "float":
                    document.getElementById(key).value = "";
                    break;
                case "cb":
                    $(document.getElementById(key)).prop('checked', false).change();
                    break;
                case "cb-list":
                    // Fire click event to so that Nested checkboxes with "Select All" buttons update
                    $(document.querySelectorAll(key)).each((i, ele) => {
                        if (ele.checked)
                            ele.click();
                    });
                    break;
                case "ddl":
                    // Probably better to set some default value within each ddl filter field
                    document.getElementById(key).selectedIndex = filterFields[key].default || 0;
                    
                    break;
                default: console.warn(`Field '${key}' cannot be cleared`)
            }
        }    
  });
}

export function setFilterValues(data) {
  if (data && typeof data === 'object') {
    Object.keys(filterFields).forEach((key, index) => {
      if (data[filterFields[key].name] !== undefined) {
        switch(filterFields[key].type) {
          case "string":
            document.getElementById(key).value = data[filterFields[key].name];
            break;
          case "ddl":
            document.getElementById(key).value = data[filterFields[key].name];
            break;
          case "int":
            document.getElementById(key).value = data[filterFields[key].name];
            break;
          case "float":
            document.getElementById(key).value = data[filterFields[key].name];
            break;
          case "money":
            if (data[filterFields[key].name] != null) {
              document.getElementById(key).value = util.addCommaSeparators(data[filterFields[key].name].toString());
            }
            else {
              document.getElementById(key).value = data[filterFields[key].name];
            }
            break;
          case "cb":
            document.getElementById(key).value = data[filterFields[key].name];
            break;
          case "cb-list":
            const elements = document.querySelectorAll(key);
            // To get the checkboxes populated, we need to create another key
            var tempKey;
            switch (key) {
              case ".qs-asset-type":
                tempKey = "AssetTypes";
                break;
              case ".qs-asset-sub-type":
                tempKey = "AssetSubTypes";
                break;
              case ".as-asset-type":
                tempKey = "AssetTypes";
                break;
              case ".as-asset-sub-type":
                tempKey = "AssetSubTypes";
                break;
              case ".as-grade-classification":
                tempKey = "GradeClassifications";
                break;
              case ".as-vacancy-factor":
                tempKey = "VacancyFactor";
                break;
              case ".as-rentable-space":
                tempKey = "RentableSpace";
                break;
              case ".as-amortization-schedule":
                tempKey = "AmortizationSchedule";
                break;
              case ".as-int-road-types":
                tempKey = "InteriorRoadTypes";
                break;
              case ".as-access-road-types":
                tempKey = "AccessRoadTypes";
                break;
              case ".as-waste-water-types":
                tempKey = "WasteWaterTypes";
                break;
              case ".as-water-serv-types":
                tempKey = "WaterServTypes";
                break;
              case ".as-gas-meter-methods":
                tempKey = "ElectricMeteringMethods";
                break;
              case ".as-elec-meter-methods":
                tempKey = "GasMeteringMethods";
                break;
            }

            if (Array.isArray(data[tempKey])) {
              let washedArray = data[tempKey].map(x => {
                switch(typeof x) {
                  case "number":
                    return x.toString();
                  case "string":
                    return x;
                  default:
                    // literally poison, we dont care
                    break;
                }
              })
              Array.prototype.forEach.call(elements, (el, i) => {
                if (~washedArray.indexOf(el.attributes[filterFields[key].listId].value)) {
                  el.checked = true;
                }
                else
                {
                  el.checked = false;
                }
              });
            }
            break;
          default:
            // we shouldnt choke on bad data from who knows where populated by who knows what
            console.log(`Field '${key}' does not have a supported type`);
        }
      }
    });
  } else {
    console.log('<setFilterValues> received garbage!')
  }

    if (util != null) {
      util.updateAssetTypeBoxLabel();
    }
}

export function getFilterValues(onlyQuick) {
  const data = {};
  const activeAssetTypeGroups = aggregatePopulatedAssetTypes();
  Object.keys(filterFields).forEach((key, index) => {
    let canParseField = filterFields[key].active;
    if (onlyQuick === true && filterFields[key].loc !== fieldLocation.quick) canParseField = false;
    if (!activeAssetTypeGroups.com && filterFields[key].section === advFieldSection.com) canParseField = false;
    if (!activeAssetTypeGroups.fsa && filterFields[key].section === advFieldSection.fsa) canParseField = false;
    if (!activeAssetTypeGroups.cpn && filterFields[key].section === advFieldSection.cpn) canParseField = false;
    if (!activeAssetTypeGroups.mf && filterFields[key].section === advFieldSection.mf) canParseField = false;
    if (canParseField) {
      switch(filterFields[key].type) {
        case "string":
        case "ddl":
          data[filterFields[key].name] = fetchGeneric(key);
          break;
        case "int":
          data[filterFields[key].name] = fetchGeneric(key, 'int');
          break;
        case "float":
          data[filterFields[key].name] = fetchGeneric(key, 'float');
          break;
        case "money":
          data[filterFields[key].name] = fetchGeneric(key, 'int', true);
          break;
        case "cb":
          data[filterFields[key].name] = fetchGeneric(key, 'bool');
          break;
        case "cb-list":
          data[filterFields[key].name] = fetchListInputValues(key, filterFields[key].listId, filterFields[key].listType);
          break;
		case "bool":
          data[filterFields[key].name] = fetchGeneric(key, 'bool');
          break;
        default:
          throw `Field '${key}' does not have a supported type`;
      }
    }
  });
  return data;
}

export function fetchListInputValues(cssClass, idProp, type) {
  const elements = document.querySelectorAll(cssClass);
  const list = [];
  Array.prototype.forEach.call(elements, (el, i) => {
    if (el.checked) {
      let attr = el.attributes[idProp];
      if (attr) {
        if (type === 'int') {
          let int = parseInt(attr.value);
          if (!isNaN(int)) list.push(int);
          else {
            console.error(`Expected integer value, encountered '${attr.value}', id: ${idProp} class: ${cssClass}`);
          }
        } else {
          list.push(el.attributes[idProp].value);
        }
      } else {
        console.error(`Could not find data id attribute and/or value for list '${cssClass}', data key: '${idProp}', type: '${type}'`);
      }
    }
  })
  return list;
}

export function fetchGeneric(id, type, isMoney) {
    if (util != null) {
        // TODO: add commas and unadd commas(other countries add periods)
        let el = document.getElementById(id);
        if (el) {
            switch (type) {
                case 'int':
                    let tempVal = el.value.trim();
                    let val
                    tempVal = util.stripNonNumericChars(tempVal);
                    if (tempVal.length > 0) {
                        val = parseInt(tempVal);
                        if (!isNaN(val)) {
                            return val;
                        } else {
                            console.error(`Failed to convert '${el.value}' to an int`);
                        }
                    }
                    return null;
                    break;
                case 'float':
                    let tempValue = el.value;
                    let value
                    if (tempValue.length > 0) {
                        value = parseFloat(tempValue);
                        if (!isNaN(value)) {
                            return value;
                        } else {
                            console.error(`Failed to convert '${el.value}' to an float`);
                        }
                    }
                    return null;
                    break;
                case 'bool':
                    return el.checked;
                    break;
                default:
                    return el.value;
            }
        } else {
            //throw `Could not find any element by id '${id}'`;
            console.error(`Could not find any element by id '${id}'`);
        }
    }
}
