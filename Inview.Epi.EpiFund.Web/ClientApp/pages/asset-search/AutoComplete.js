import React, { Component } from "react"
import ReactDOM from 'react-dom';
import * as Comm from './comm';
const comm = Comm.comm;
import * as CookieChecker from './AssetSearchCookieChecker';

export function populateAutocomplete() {
    $('#txtLocation').autocomplete({
        source: function (request, response) {
            comm.fetchSearchTerms(request.term).then(data => {
                console.log("fetchSearchTerms data ", data);
                response(data.payload);
            })
        },
        minLength: 3,
        select: function (event, ui) {
            console.log("Selected: " + ui.item.value + " aka " + ui.item.res + " type " + ui.item.type);
            if (~window.location.href.indexOf('/DataPortal/AssetSearchLanding')) {
                CookieChecker.setCookieAssetSearch(JSON.stringify({ location: ui.item.value, locationType: ui.item.type, from: 'landing' }));
                window.location.href = '/DataPortal/AssetSearch';
            } else {
                // assume we are on /AssetSearch page. clearly not that flexible atm

            }
        }
    })
}