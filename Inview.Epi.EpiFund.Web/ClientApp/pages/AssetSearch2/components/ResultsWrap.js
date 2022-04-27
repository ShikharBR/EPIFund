import React, { Component } from "react";
import LineItem from "./LineItem";
import 'react-table/react-table.css'
import { util } from "../../asset-search/assetSearchUtils";
import { connect } from 'react-redux';
import TileItem from './TileItem';
import store from '../../../ /../redux/epiStore'
import ReactHoverObserver from 'react-hover-observer';

//https://github.com/tannerlinsley/react-table/tree/v6
class ResultsWrap extends Component {

    constructor(props) {
        super(props);

        this.state = {
            pageShowCount: 21,
            pageIndex: 1,

            mappedSearchResults: [],
            filteredResults: []
        }

        this.showPage = this.showPage.bind(this);
        this.searchResults = this.searchResults.bind(this);
    }

    searchResults() {
        if (this.props.searchResult != null &&
            this.state.currentSearchId != this.props.searchResult.SearchId) {
            const mappedResults = util.parseSearchResultAssets(this.props.searchResult.Assets);
            this.setState({
                currentSearchId: this.props.searchResult.SearchId,
                mappedSearchResults: mappedResults
            }, () => { this.showPage(1); });
        }
    }

    componentDidUpdate() {
        this.searchResults();
    }

    componentWillMount() {
        this.searchResults();
    }

    showPage(pageIndex) {
        //can't go negative
        if (pageIndex < 1)
            return;

        //end of pages
        const page = this.pagination(this.state.mappedSearchResults.length, pageIndex, this.state.pageShowCount);
        if (pageIndex > page.last_page)
            return;

        this.setState({
            pageIndex: pageIndex
        }, () => {
            const items = this.state.mappedSearchResults.slice(page.from, page.to);
            this.setState({
                filteredResults: items
            });
        });
    }

    pagination(length, currentPage, itemsPerPage) {
        return {
            total: length,
            per_page: itemsPerPage,
            current_page: currentPage,
            last_page: Math.ceil(length / itemsPerPage),
            from: ((currentPage - 1) * itemsPerPage),
            to: (currentPage * itemsPerPage) - 1
        };
    };

    render() {

        let flexStyle = {};

        if (this.props.viewType) {
            flexStyle = { display: "flex", flexWrap: "wrap" };
        }
        else {
            flexStyle = { display: "flex", flexWrap: "wrap" };
        }

        const state = store.getState();

        let backForward = () => {
            return <div className="AssetSearch2_BackForwardButtonWrap">
                <div className={'AssetSearch2_BackForwardButton AssetSearch_BlueGradient'} onClick={() => { this.showPage(this.state.pageIndex - 1) }}>BACK</div>
                <div className={'AssetSearch2_BackForwardButton AssetSearch_BlueGradient'} onClick={() => { this.showPage(this.state.pageIndex + 1) }}>NEXT</div>
            </div>
        }

        return (
            <>
                {
                    <div style={flexStyle}>
                        {(this.state.filteredResults.length > 1) && backForward()}
                        {this.props.viewOptions.isLineView && <div className="AssetSearch_Linewrap" style={(this.props.viewOptions.isFullScreen) ? {  } : {}}>
                            <div className="AssetSearch_line_cr" title="Assumable Financing"></div>
                            <div className="AssetSearch_line_name"><span>Name</span></div>
                            <div className="AssetSearch_line_citystate"><span>City/State</span></div>
                            <div className="AssetSearch_line_type"><span>Type</span></div>
                            <div className="AssetSearch_line_size"><span>Size</span></div>
                            <div className="AssetSearch_line_pricingindic" title="Scheduled Gross Income"><span>SGI</span></div>
                            <div className="AssetSearch_line_pricingindic" title="Net Operating Income"><span>NOI</span></div>
                            <div className="AssetSearch_line_cr" title="Capitalization Rate" style={{width: "42px"}}><span>CAP %</span></div>
                            <div className="AssetSearch_line_cr" title="Assumable Financing"><span>AF</span></div>
                            <div className="AssetSearch_line_occyr"><span title="Occupancy / Year">Occ/Yr</span></div>
                            <div className="AssetSearch_line_price"><span title="List Price / Comparable Market Value">Price</span></div>
                            <div className="AssetSearch_line_act" title="Action"><span>ACT</span></div>
                        </div>}

                        {this.props.viewOptions.isLineView &&
                            this.state.filteredResults.map(result => {
                                return (
                                    <LineItem key={result.assetId} currentAsset={result} />
                                )
                            })
                        }

                        {!this.props.viewOptions.isLineView &&
                            <div style={{ paddingRight: "20px", paddingLeft: "20px", display: "flex", flexWrap: "wrap", justifyContent: "space-between", paddingBottom: "20px" }}>{
                                this.state.filteredResults.map(result => {
                                    return (
                                        <TileItem key={result.assetId} currentAsset={result} />
                                    )
                                })
                            }
                            </div>
                        }

                        {(this.state.filteredResults.length > 1) && backForward()}
                        {(this.state.filteredResults.length < 1) && <div className="AssetSearch2_Placeholder">
                            Please enter a location above to start your asset search.
                        </div>}
                    </div>
                }
            </>
        );
    }
}

const mapStateToProps = state => ({
    currentSearchId: state.assetSearchReducer.assetSearch.currentSearchId,
    searchResult: state.assetSearchReducer.assetSearch.searchResult,
    viewOptions: state.assetSearchReducer.viewOptions.viewOptions
});

export default connect(mapStateToProps)(ResultsWrap);