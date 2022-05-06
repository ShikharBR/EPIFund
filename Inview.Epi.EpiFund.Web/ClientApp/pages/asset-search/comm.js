import React, { Component } from "react"
import ReactDOM from 'react-dom';

export const comm = {
  getAssets: async (payload) => {
    //let body = { Skip: 0, Take: 10, City: sc.City, State: convertState(sc.State), Zip: sc.Zip, Min: sc.Min, Max: sc.Max, AssetType: sc.AssetType };
    if (payload && typeof payload === 'object') {
      payload.Take = 500;
      payload.State = payload.State;
      return fetch('/DataPortal/SearchAssets', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json; charset=utf-8', },
        body: JSON.stringify(payload),
      }).then(response => response.json());
    } else {
      throw 'GetAssets-Error: Expected object, received something else';
    }
  },
  getFavoriteGroups: async () => {
    return fetch('/DataPortal/GetFavoriteGroups', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json; charset=utf-8' }
    }).then(response => response.json());
  },
  getFavoriteGroupAssets: async (favGroupId) => {
    return fetch('/DataPortal/GetFavoriteGroupAssets', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json; charset=utf-8', },
      body: JSON.stringify({favGroupId})
    }).then(response => response.json());
  },
  saveFavoriteGroupAsset: async (favoriteGroupId, assetId) => {
    return fetch('/DataPortal/SaveFavoriteGroupAsset', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json; charset=utf-8' },
      body: JSON.stringify({favoriteGroupId, assetId})
    }).then(response => response.json());
  },
  createFavoriteGroup: async (favoriteGroupName, favoriteGroupDescription) => {
    return fetch('/DataPortal/CreateFavoriteGroup', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json; charset=utf-8' },
      body: JSON.stringify({ favoriteGroupName, favoriteGroupDescription })
    }).then(response => response.json());
  },
  getSavedSearchesForUser: async () => {
    return fetch('/DataPortal/GetUserSavedSearches', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json; charset=utf-8', },
    }).then(response => response.json());
  },
  saveAssetSearch: async (data, title) => {
    return fetch('/DataPortal/SaveAssetSearch', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json; charset=utf-8', },
      body: JSON.stringify({data, title}),
    }).then(response => response.json());
  },
  updateAssetSearch: async (id, data, title) => {
    return fetch('/DataPortal/UpdateAssetSearch', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json; charset=utf-8', },
      body: JSON.stringify({id, data, title}),
    }).then(response => response.json());
  },
  fetchSearchTerms: (term) => {
      return fetch('/Asset/SearchLocations', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json; charset=utf-8', },
          body: JSON.stringify({ term }),
      }).then(response => response.json());
  },
  isLoggedIn: async () => {
    return fetch('/Home/IsLoggedIn', {
      method: 'POST',
      headers: { 'Content-Type': 'text/plain' },
    }).then(response => response.text());
  },
};
