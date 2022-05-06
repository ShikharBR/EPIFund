import { comm } from '../../pages/AssetSearch2/comm'
import { util } from '../../pages/asset-search/assetSearchUtils'
import { toast } from 'react-toastify'
import { initState } from '../reducers/mapReducers/assetSearchReducer';
import { helpers } from '../../helpers/helpers';



export const ASSET_SEARCH = 'ASSET_SEARCH';
export const SAVE_SEARCH = 'SAVE_SEARCH';
export const GET_SAVED_SEARCH = 'GET_SAVED_SEARCH';
export const Filter_SAVED_SEARCH = 'Filter_SAVED_SEARCH';
export const SHOW_ADVANCED = 'SHOW_ADVANCED';
export const GET_OR_UPDATE_CURRENT_SEARCHMODEL = 'GET_OR_UPDATE_CURRENT_SEARCHMODEL';
export const VIEW_OPTIONS = 'VIEW_OPTIONS';

export const updateShowAdvanced = (show) => async dispatch => {
    dispatch({
        type: SHOW_ADVANCED,
        payload: show
    });
}

export const updateViewOptions = (options) => async dispatch => {
    dispatch({
        type: VIEW_OPTIONS,
        payload: options
    });
}


export const doAssetSearch = (searchModel) => async dispatch => {
    toast.dismiss();
    toast.info("Searching...", {  });
    const result = await comm.getAssets(searchModel);
    dispatch({
        type: ASSET_SEARCH,
        payload: result
    });
}

export const getSavedSearchesForUser = () => async dispatch => {
    const result = await comm.getSavedSearchesForUser();
    dispatch({
        type: GET_SAVED_SEARCH,
        payload: result
    });
}

export const saveSearch = (searchModel) => async dispatch => {

    //clean the search model before it saves
    searchModel = Object.assign({}, searchModel);

    //no need to save everything, just need the name, id, and value
    const cleanList = list => list.map(x => ({
        id: x.id,
        name: x.name,
        value: x.value,
    }));

    searchModel.basicSearchArray = cleanList(searchModel.basicSearchArray);
    searchModel.creSearchArray = cleanList(searchModel.creSearchArray);
    searchModel.mhpSearchArray = cleanList(searchModel.mhpSearchArray);
    searchModel.spnSearchArray = cleanList(searchModel.spnSearchArray);
    searchModel.mfSearchArray = cleanList(searchModel.mfSearchArray);
    toast.dismiss();
    toast.info("Saving search...");
    const result = await comm.saveAssetSearch(searchModel, searchModel.savedSearchTitle);
    dispatch({
        type: SAVE_SEARCH,
        payload: result
    });
}

export const filterSavedSearches = (name) => async dispatch => {
    dispatch({
        type: Filter_SAVED_SEARCH,
        payload: name
    });
}

export const getOrUpdateCurrentSearchModel = (updatedSearchModel = null, savedSearch = null, reset = false) => async (dispatch, getState) => {
    let payload = null;

    if (reset) {
        //Chi: there is gotta be a better way to write this
        //initState is changed by reducer, can't trust it
        const tempSearchModel = getState().assetSearchReducer.getOrUpdateCurrentSearchModel.searchModel;
        tempSearchModel.savedSearchTitle = "";
        tempSearchModel.searchBarInput = "";
        tempSearchModel.savedSearchTitleChanged = false;
        tempSearchModel.isUpdated = false;
        tempSearchModel.currentSearchId = helpers.GuidEmpty;
        tempSearchModel.locations = [{ city: "", state: "", county: "" }]
        tempSearchModel.savedSearchId = helpers.GuidEmpty;
        const lists = [
            tempSearchModel.basicSearchArray,
            tempSearchModel.creSearchArray,
            tempSearchModel.mhpSearchArray,
            tempSearchModel.spnSearchArray,
            tempSearchModel.mfSearchArray,
        ];
        for (var i = 0; i < lists.length; i++) {
            //foreach list
            const currentList = lists[i];
            for (var a = 0; a < currentList.length; a++) {
                //foreach input
                let currentInput = currentList[a];

                if (currentInput.inputtype !== "dropdown") {
                    currentInput.value = "";
                } else {
                    currentInput.value = [];
                }
            }
        }
        payload = { mappedSearchModel: tempSearchModel };
    } else {
        if (savedSearch != null) {

            const mappedSearchModel = JSON.parse(savedSearch.Json);
            //map it back to match the search model
            //1.) start from the original search model
            const updatedFromSavedSearchSearchModel = initState.searchModel;
            updatedFromSavedSearchSearchModel.savedSearchTitle = mappedSearchModel.savedSearchTitle;
            updatedFromSavedSearchSearchModel.searchBarInput = mappedSearchModel.searchBarInput;
            updatedFromSavedSearchSearchModel.savedSearchTitleChanged = false;
            updatedFromSavedSearchSearchModel.isUpdated = false;
            updatedFromSavedSearchSearchModel.currentSearchId = mappedSearchModel.currentSearchId;
            updatedFromSavedSearchSearchModel.locations = mappedSearchModel.locations;
            updatedFromSavedSearchSearchModel.savedSearchId = savedSearch.Id;
            //2.) get a list of lists that are needed to be mapped
            const updatedFromSavedSearchSearchModelList = [
                updatedFromSavedSearchSearchModel.basicSearchArray,
                updatedFromSavedSearchSearchModel.creSearchArray,
                updatedFromSavedSearchSearchModel.mhpSearchArray,
                updatedFromSavedSearchSearchModel.spnSearchArray,
                updatedFromSavedSearchSearchModel.mfSearchArray,
            ];

            //3.) target is the lists in the mappedSearchModel
            const savedSearchList = [
                ...mappedSearchModel.basicSearchArray,
                ...mappedSearchModel.creSearchArray,
                ...mappedSearchModel.mhpSearchArray,
                ...mappedSearchModel.spnSearchArray,
                ...mappedSearchModel.mfSearchArray,
            ];

            updatedFromSavedSearchSearchModelList.forEach(list => {
                list.forEach(item => {
                    const found = savedSearchList.find(x => x.id === item.id);
                    if (found != null) {
                        item.value = found.value;
                    }
                });
            });

            toast.dismiss();
            toast.info("Loading your saved search data...");
            payload = { mappedSearchModel: updatedFromSavedSearchSearchModel };
        } else if (updatedSearchModel != null) {
            payload = { mappedSearchModel: updatedSearchModel };
        }
    }

    if (payload != null) {
        //depends on showAdvanced...
        const showAdvanced = getState().assetSearchReducer.showAdvanced.showAdvanced;
        const mappedSearchModel = payload.mappedSearchModel;
        const basicLocationInput = mappedSearchModel.basicSearchArray.find(x => x.name === "Location");
        if (showAdvanced) {
            const firstLocation = mappedSearchModel.locations[0];
            if (firstLocation != null) {
                const locStr = [
                    ((firstLocation.city === "" || firstLocation.city == null) ? "" : firstLocation.city),
                    ((firstLocation.state === "" || firstLocation.state == null) ? "" : firstLocation.state),
                    ((firstLocation.county === "" || firstLocation.county == null) ? "" : firstLocation.county)
                ].filter(x => { return x !== ""}).join(", ");
                 
                if (basicLocationInput != null) {
                    basicLocationInput.value = locStr;
                }
            }
        } else {
            //locations, the 1 basic location is linked to the search model locations list
            if (basicLocationInput != null) {
                const basicLocation = util.parseLocation(basicLocationInput.value) || {}
                mappedSearchModel.locations[0] = basicLocation;
            }
        }
    }

    dispatch({
        type: GET_OR_UPDATE_CURRENT_SEARCHMODEL,
        payload: payload
    });
}

