import * as moment from 'moment'
import { util } from 'APP/pages/asset-search/assetSearchUtils'
import { assetyTypesAndIDs } from 'APP/pages/AssetSearch2/components/AssetSearchLib';

export const helpers = {
    GuidEmpty: "00000000-0000-0000-0000-000000000000",
    _validFileExtensions: [".jpg", ".jpeg", ".bmp", ".gif", ".png", ".pdf", ".doc", ".docx"],

    //helper functions
    validateEmail: (email) => {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    },
    GenerateGuid: () => {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    },
    ValidateExtension: (fileName) => {
        if (fileName.length > 0) {
            var blnValid = false;
            for (var j = 0; j < _validFileExtensions.length; j++) {
                var sCurExtension = _validFileExtensions[j];
                if (fileName.substr(fileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                    blnValid = true;
                    break;
                }
            }

            if (!blnValid) {
                return false;
            }
        }

        return blnValid;
    },
    formatDateTime: (dateTime, UTC_to_Local = false) => {
        let output = "";

        const format = 'MM/DD/YYYY hh:mm a';
        let date = moment(dateTime);
        output = date.format(format);

        if (UTC_to_Local) {
            let locatDate = moment.utc(output, format).local();
            output = locatDate.format(format);;
        }

        return output;
    },
    formatDate: (dateTime) => {
        let date = moment(dateTime);
        const format = date.format('MM/DD/YYYY');

        return format;
    },
    isDataTable: function (el) {
        if (el == null || !$.fn.DataTable.isDataTable(el))
            return false;
        return true;
    },
    debounce: function (a, b, c) { var d, e; return function () { function h() { d = null, c || (e = a.apply(f, g)) } var f = this, g = arguments; return clearTimeout(d), d = setTimeout(h, b), c && !d && (e = a.apply(f, g)), e } },

    findInputInLists(inputID, lists) {
        const foundInput = lists.find(x => x.id === inputID);
        if (foundInput == null) {
            throw new Error("Check input id");
        }
            return foundInput;
    },
    getPricingTypeIDsByNames(names) {
        let id = 3;
        //when both selected, it is a 3
        //CMV = 2
        //LP = 1
        if (names.length === 1) {
            if (names[0] === "LP")
                id = 1;
            else if (names[0] === "CMV")
                id = 2;
        }
        return id;
    },
    getAssetTypeIDsByNames(names) {
        const ids = names.map((name, index) => {
            const found = assetyTypesAndIDs.find(a => a.name === name);
            if (found != null)
                return found.id;
        });
        return ids;
    },
    getValueByName(searchModel, name) {
        const foundInput = [
            ...searchModel.basicSearchArray,
            ...searchModel.creSearchArray,
            ...searchModel.mhpSearchArray,
            ...searchModel.spnSearchArray,
            ...searchModel.mfSearchArray,
        ].find(x => x.name === name);
        if (foundInput == null) {
            throw new Error("Check input name");
        }
        return foundInput.value;
    },
    getSearchModel(searchModel) {
        const location = util.parseLocation(this.getValueByName(searchModel, "Location")) || {};
        const selectedAssetTypeIds = this.getAssetTypeIDsByNames(this.getValueByName(searchModel, "Asset Type"));
        const selectedPriceType = this.getPricingTypeIDsByNames(this.getValueByName(searchModel, "Pricing Type"));

        ////Inview.Epi.EpiFund.Domain.ViewModel.SearchAssetModel
        const model = {
            //basicSearchArray
            SearchBarInput: searchModel.searchBarInput,
            SavedSearchTitle: this.getValueByName(searchModel, "Saved Search"),
            Take: 500,
            Locations: [
                {
                    City: location.city,
                    State: location.state,
                    Zip: location.zip,
                    County: location.county,
                }
            ],
            Min: this.getValueByName(searchModel, "Min. Price"),
            Max: this.getValueByName(searchModel, "Max Price"),
            CapRate: this.getValueByName(searchModel, "Min Cap %"),
            YearBuilt: this.getValueByName(searchModel, "Min Year Built"),
            AssetTypes: selectedAssetTypeIds,
            PriceSearchType: selectedPriceType,
            SquareFeetMin: this.getValueByName(searchModel, "Min Sq. Ft."),
            SquareFeetMax: this.getValueByName(searchModel, "Max Sq. Ft."),

            //creSearchArray
            OperatingStatus: this.getValueByName(searchModel, "Operating Status"),
            SGI: this.getValueByName(searchModel, "PSGI"),
            NOI: this.getValueByName(searchModel, "PNOI"),
            AssumableFinancing: this.getValueByName(searchModel, "Property Offering Assumable Financing"), //<-- this doesn't exist in vm
            RentableSpace: this.getValueByName(searchModel, "Rentable Space"),
            OccPerc: this.getValueByName(searchModel, "Min Occupancy"),
            MajorTenant: this.getValueByName(searchModel, "Major Tenant"),
            MajorTenantLeaseExp: this.getValueByName(searchModel, "Major Tenant Lease Exp"),

            //mhpSearchArray
            SmrSWidePct: this.getValueByName(searchModel, "S-Wide"),
            SmrDWidePct: this.getValueByName(searchModel, "D-Wide"),
            SmrTWidePct: this.getValueByName(searchModel, "T-Wide"),
            SmrParkOwnedUnits: this.getValueByName(searchModel, "Max Park Owned Units"),
            SmrParkOwnedMaxPct: this.getValueByName(searchModel, "# Park Owned Units"),
            SmrPropWithUndevAcres: this.getValueByName(searchModel, "Properties W/ Undeveloped Acres"),
            BuildingsCount: this.getValueByName(searchModel, "# of Properties"),
            SmrUndevToTotalAcreRatioPct: this.getValueByName(searchModel, "Undev. to Total"),

            //spnSearchArray
            AmortizationSchedule: this.getValueByName(searchModel, "Amortization Schedule"),
            LeaseholdMaturityDate: this.getValueByName(searchModel, "Maturity Schedule"),
            PositionMortgage: this.getValueByName(searchModel, "Position of Note"),
            MortgageInstruments: this.getValueByName(searchModel, "Securing Mortgage Instrument"),
            LoanBalanceofNote: this.getValueByName(searchModel, "Loan Balance % to Pricing"),

            //mfSearchArray
            UmrStudioPct: this.getValueByName(searchModel, "Studio"),
            Umr1BdPct: this.getValueByName(searchModel, "1BD"),
            Umr2Bd1BaPct: this.getValueByName(searchModel, "2BD/1BA"),
            Umr2Bd2BaPct: this.getValueByName(searchModel, "2BD/2BA"),
            Umr3BdPct: this.getValueByName(searchModel, "3BD"),
            UmrOccuPerc: this.getValueByName(searchModel, "Occupancy %"),
            UmrMaxFCPerc: this.getValueByName(searchModel, "FC Max %"),
        };
        return model;
    }

}
