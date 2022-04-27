import React, { Component } from "react";
import AdvancedFiltersLocations from './AdvancedFiltersLocations';
import FilterWrap from './FilterWrap';
import { connect } from 'react-redux'

class AdvancedFilters extends Component {
    constructor(props) {
        super(props);

        this.state = {
            selectedAssetTypes: []
        };
    }

    render() {
        let conditions = [
            "Retail Tenant Property",
            "Office Tenant Property",
            "Industrial Tenant Property",
            "Fuel Service Retail Property",
            "Medical Tenant Property",
            "Mixed Use Commercial Property",
            "Fractured Condominium Portfolios",
            "Mini-Storage Property",
            "Parking Garage Property"
        ]

        let selectedAssetTypes = [];
        const selectedAssetTypesInput = this.props.searchModel.basicSearchArray.filter(x => { return x.name === "Asset Type" })[0];
        if (selectedAssetTypesInput != null) {
            selectedAssetTypes = selectedAssetTypesInput.value;
        }

        let creConditionCheck = conditions.some(item => selectedAssetTypes.includes(item));
        return (
            <>
                <AdvancedFiltersLocations />

                <FilterWrap data={this.props.searchModel.basicSearchArray} name={"General"} />

                {
                    (creConditionCheck) &&
                    <FilterWrap data={this.props.searchModel.creSearchArray} type={"creOptions"} name={"CRE Assets"} />
                }

                {
                    selectedAssetTypes.includes("MHP") &&
                    <FilterWrap data={this.props.searchModel.mhpSearchArray} name={"MHP Assets"} />
                }

                {
                    selectedAssetTypes.includes("Secured Privated Notes") &&
                    <FilterWrap data={this.props.searchModel.spnSearchArray} name={"Secured Private Notes"} />
                }

                {
                    selectedAssetTypes.includes("Multi-Family") &&
                    <FilterWrap data={this.props.searchModel.mfSearchArray} name={"Multi-Family"} />
                }
            </>
        );
    }
}

const mapStateToProps = state => ({
    searchModel: state.assetSearchReducer.getOrUpdateCurrentSearchModel.searchModel,
});

export default connect(mapStateToProps, {})(AdvancedFilters);
