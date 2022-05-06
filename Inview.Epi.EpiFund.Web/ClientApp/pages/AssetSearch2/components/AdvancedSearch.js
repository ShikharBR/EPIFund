import React, { Component } from "react";
import AdvancedFilters from './AdvancedFilters';
import SavedSearches from './SavedSearches';
import AdvancedImport from './AdvancedImport';
import XIcon from '../img/xicon.png';
import { helpers } from "../../../helpers/helpers";
import { connect } from 'react-redux'
import { updateShowAdvanced } from '../../../redux/actions/asset-search-action'

class AdvancedSearch extends Component {
    constructor(props) {
        super(props);

        this.state = {
            currentPage: "Advanced Filters"
        }

        this.setPage = this.setPage.bind(this);
        this.importItems = this.importItems.bind(this);
    }

    setPage(page) {
        this.setState({currentPage: page});
    }
    
    importItems(type) {
        this.props.getImportType(type);
    }

    render() {
        return(
            <div className="AssetSearch_Displayflex animated fadeIn faster" style={{height: "auto", width: "100%", backgroundColor: "#fff", alignItems: "stretch", minHeight: "1000px"}}>

                <SavedSearches />

                <div className="AdvancedSearch_Main">
                    <div className="AdvancedSearch_Main_Topbar">
                        <div style={{display: "flex"}}>
                            <div className={(this.state.currentPage === "Advanced Filters") ? "AdvancedSearch_Topbar_button AdvancedSearch_Topbar_button_active" : "AdvancedSearch_Topbar_button"} onClick={() => this.setPage("Advanced Filters")}>ADVANCED FILTERS</div>
                            <div className={(this.state.currentPage === "Import") ? "AdvancedSearch_Topbar_button AdvancedSearch_Topbar_button_active" : "AdvancedSearch_Topbar_button"} onClick={() => this.setPage("Import")}>IMPORT</div>
                        </div>
                        <div className="AdvancedSearch_xIconButton" onClick={() => this.props.updateShowAdvanced(false)}>
                            <img alt="xicon" src={XIcon} />
                        </div>
                    </div>
                    {
                        <div className="AdvancedSearch_AdvancedSearch_Contentwrap">
                            {this.state.currentPage === "Advanced Filters" && <AdvancedFilters />}
                            {this.state.currentPage === "Import" && <AdvancedImport getImportItems={this.importItems} />}
                        </div>
                    }
                </div>
            </div>
        );
    }
}

const mapStateToProps = state => ({
    searchModel: state.assetSearchReducer.getOrUpdateCurrentSearchModel.searchModel,
});

export default connect(mapStateToProps, { updateShowAdvanced })(AdvancedSearch);
