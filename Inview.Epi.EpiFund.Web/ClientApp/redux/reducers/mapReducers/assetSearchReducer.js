import { combineReducers } from 'redux'
import {
    ASSET_SEARCH,
    SAVE_SEARCH,
    GET_SAVED_SEARCH,
    Filter_SAVED_SEARCH,
    SHOW_ADVANCED,
    GET_OR_UPDATE_CURRENT_SEARCHMODEL,
    VIEW_OPTIONS
} from '../../actions/asset-search-action'
import { assetTypes, pricingTypes, gradeTypes, operatingTypes, binaryTypes, rentableTypes, amortizationTypes, maturityScheduleTypes, positionTypes, instrumentTypes, balanceNoteTypes } from '../../../pages/AssetSearch2/components/AssetSearchLib';
import { helpers } from '../../../helpers/helpers';


export const initState = {
    searchModel: {
        savedSearchTitle: "",

        searchBarInput: "",
        currentSearchId: helpers.GuidEmpty, //<-- this come back from search (DataPortal)
        savedSearchId: helpers.GuidEmpty,
        savedSearchTitleChanged: false,
        locations: [{ city: "", state: "", county: "" }],
        isUpdated: false,

        /*
        Careful when you change the array here, make sure you do a find all before you change anything (especially the input name)
        //helpers -> getSearchModel() will grab input values from lists by name
        */
        basicSearchArray: [
            { id: 1, name: "Location", value: "", width: "300px", placeholder: "Las Vegas, NV", type: "text", inputtype: "field", basiconly: true, isbasic: true },
            { id: 2, name: "Min. Price", value: "", width: "132px", placeholder: "$15,000,000", type: "money", inputtype: "field", basiconly: false, advwidth: "152px", isbasic: true },
            { id: 3, name: "Max Price", value: "", width: "132px", placeholder: "$15,000,000", type: "money", inputtype: "field", basiconly: false, advwidth: "152px", isbasic: true },
            { id: 4, name: "Min Cap %", value: "", width: "86px", placeholder: "5.5%", type: "percent", inputtype: "field", basiconly: false, advwidth: "106px", isbasic: true },
            { id: 5, name: "Min Year Built", value: "", width: "125px", placeholder: "1990", type: "number", inputtype: "field", basiconly: false, advwidth: "145px", isbasic: true },
            { id: 6, name: "Asset Type", value: [], width: "300px", placeholder: "Industrial Tenant Property", type: "text", inputtype: "dropdown", options: assetTypes, dropplaceholder: "Select Asset Type", basiconly: false, advwidth: "300px", isbasic: true },
            { id: 7, name: "Pricing Type", value: [], width: "125px", placeholder: "Type", type: "text", inputtype: "dropdown", options: pricingTypes, dropplaceholder: "Type", basiconly: false, advwidth: "135px", isbasic: true },
            { id: 8, name: "Saved Search", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: true, isbasic: true },
            { id: 9, name: "Grade Classification", value: [], width: "125px", placeholder: "Type", type: "text", inputtype: "dropdown", picktype: "single", options: gradeTypes, dropplaceholder: "Grade", advonly: true, isbasic: true },
            { id: 10, name: "Min Sq. Ft.", value: "", width: "125px", placeholder: "Min Sq Ft", type: "number", inputtype: "field", basiconly: false, advwidth: "145px", advonly: true, advwidth: "125px", isbasic: true },
            { id: 11, name: "Max Sq. Ft.", value: "", width: "125px", placeholder: "Max Sq Ft", type: "number", inputtype: "field", basiconly: false, advwidth: "145px", advonly: true, advwidth: "125px", isbasic: true }
        ],
        creSearchArray: [
            { id: 12, name: "Operating Status", value: [], width: "290px", placeholder: "Operating Status", type: "text", inputtype: "dropdown", picktype: "single", options: operatingTypes, dropplaceholder: "Operating Status", advwidth: "300px" },
            { id: 13, name: "PSGI", value: "", width: "133px", placeholder: "PSGI", type: "money", inputtype: "field", basiconly: false },
            { id: 14, name: "PNOI", value: "", width: "133px", placeholder: "PSGI", type: "money", inputtype: "field", basiconly: false },
            { id: 15, name: "Property Offering Assumable Financing", value: [], width: "125px", placeholder: "Type", type: "text", inputtype: "dropdown", options: binaryTypes, dropplaceholder: "Options", basiconly: false, advwidth: "300px" },
            { id: 16, name: "Rentable Space", value: [], width: "125px", placeholder: "Type", type: "text", inputtype: "dropdown", options: rentableTypes, dropplaceholder: "Options", basiconly: false, advwidth: "275px" },
            { id: 17, name: "Min Occupancy", value: "", width: "86px", placeholder: "5.5%", type: "percent", inputtype: "field", basiconly: false, advwidth: "106px" },
            { id: 18, name: "Major Tenant", value: [], width: "125px", placeholder: "Type", type: "text", inputtype: "dropdown", options: binaryTypes, dropplaceholder: "Options", basiconly: false, advwidth: "106px" },
            { id: 19, name: "Major Tenant Lease Exp", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: false, advwidth: "166px" }
        ],
        mhpSearchArray: [
            { id: 20, name: "S-Wide", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: false, advwidth: "116px" },
            { id: 21, name: "D-Wide", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: false, advwidth: "116px" },
            { id: 22, name: "T-Wide", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: false, advwidth: "116px" },
            { id: 23, name: "Max Park Owned Units", value: [], width: "125px", placeholder: "Type", type: "text", inputtype: "dropdown", options: binaryTypes, dropplaceholder: "Options", basiconly: false, advwidth: "210px" },
            { id: 24, name: "# Park Owned Units", value: "", width: "125px", placeholder: "200", type: "number", inputtype: "field", basiconly: false, advwidth: "125px" },
            { id: 25, name: "Properties W/ Undeveloped Acres", value: [], width: "125px", placeholder: "Type", type: "text", inputtype: "dropdown", options: binaryTypes, dropplaceholder: "Options", basiconly: false, advwidth: "240px" },
            { id: 26, name: "# of Properties", value: "", width: "125px", placeholder: "200", type: "number", inputtype: "field", basiconly: false, advwidth: "95px" },
            { id: 27, name: "Undev. to Total", value: "", width: "86px", placeholder: "5.5%", type: "percent", inputtype: "field", basiconly: false, advwidth: "97px" }
        ],
        spnSearchArray: [
            { id: 28, name: "Amortization Schedule", value: [], width: "290px", placeholder: "Operating Status", type: "text", inputtype: "dropdown", picktype: "single", options: amortizationTypes, dropplaceholder: "Options", basiconly: false, advwidth: "287px" },
            { id: 29, name: "Maturity Schedule", value: [], width: "290px", placeholder: "Operating Status", type: "text", inputtype: "dropdown", picktype: "single", options: maturityScheduleTypes, dropplaceholder: "Options", basiconly: false, advwidth: "287px" },
            { id: 30, name: "Position of Note", value: [], width: "290px", placeholder: "Operating Status", type: "text", inputtype: "dropdown", picktype: "single", options: positionTypes, dropplaceholder: "Options", basiconly: false, advwidth: "287px" },
            { id: 31, name: "Securing Mortgage Instrument", value: [], width: "290px", placeholder: "Operating Status", type: "text", inputtype: "dropdown", picktype: "single", options: instrumentTypes, dropplaceholder: "Options", basiconly: false, advwidth: "287px" },
            { id: 32, name: "Loan Balance % to Pricing", value: [], width: "290px", placeholder: "Operating Status", type: "text", inputtype: "dropdown", picktype: "single", options: balanceNoteTypes, dropplaceholder: "Options", basiconly: false, advwidth: "287px" },
        ],
        mfSearchArray: [
            { id: 33, name: "Studio", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: false, advwidth: "110px" },
            { id: 34, name: "1BD", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: false, advwidth: "110px" },
            { id: 35, name: "2BD/1BA", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: false, advwidth: "110px" },
            { id: 36, name: "2BD/2BA", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: false, advwidth: "110px" },
            { id: 37, name: "3BD", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field", basiconly: false, advwidth: "110px" },
            { id: 38, name: "Occupancy %", value: "", width: "86px", placeholder: "5.5%", type: "percent", inputtype: "field", basiconly: false, advwidth: "97px" },
            { id: 39, name: "FC Max %", value: "", width: "86px", placeholder: "5.5%", type: "percent", inputtype: "field", basiconly: false, advwidth: "97px" }
        ]
    },
    viewOptions: { isFullScreen: false, isLineView: false, showDrop: false },
    showAdvanced: false,
    searchResult: {Total: 0, PublishedAssets: 0, TotalAssetVal: 0, MultiFamUnits: 0, TotalSqFt: 0}
};

function viewOptions(state = initState, action) {
    switch (action.type) {
        case VIEW_OPTIONS:
            return {
                viewOptions: Object.assign({}, action.payload)
            }
        default:
            return {
                viewOptions: Object.assign({}, state.viewOptions)
            };
    }
}

function showAdvanced(state = initState, action) {
    switch (action.type) {
        case SHOW_ADVANCED:
            return Object.assign({}, {
                showAdvanced: action.payload
            });
        default:
            return state
    }
}

function assetSearch(state = initState, action) {
    switch (action.type) {
        case ASSET_SEARCH:
            return {
                ...state,
                currentSearchId: action.payload.SearchId,
                searchResult: action.payload
            };
        default:
            return state
    }
}

function getSavedSearchesForUser(state = initState, action) {
    switch (action.type) {
        case GET_SAVED_SEARCH:
            return {
                ...state,
                savedSearches: action.payload.searches
            };
        default:
            return state
    }
}

function saveSearch(state = initState, action) {
    switch (action.type) {
        case SAVE_SEARCH:
            return {
                ...state
            };
        default:
            return state
    }
}

function filterSavedSearches(state = initState, action) {
    switch (action.type) {
        case Filter_SAVED_SEARCH:
            return {
                searchTerm: action.payload
            };
        default:
            return state
    }
}

function getOrUpdateCurrentSearchModel(state = initState, action) {
    switch (action.type) {
        case GET_OR_UPDATE_CURRENT_SEARCHMODEL:
            if (action.payload != null) {
                var newModel = Object.assign({}, action.payload.mappedSearchModel);
                newModel.isUpdated = true;
                return {
                    ...state,
                    searchModel: newModel
                };
            }
        default:
            return state
    }
}

const assetSearchReducer = combineReducers({
    assetSearch,
    getSavedSearchesForUser,
    saveSearch,
    filterSavedSearches,
    showAdvanced,
    getOrUpdateCurrentSearchModel,
    viewOptions
})

export default assetSearchReducer