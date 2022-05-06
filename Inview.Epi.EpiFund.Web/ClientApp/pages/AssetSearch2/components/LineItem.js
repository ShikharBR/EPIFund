import React, { Component } from "react";
import EmptyStar from "../img/starempty.png";
import FullStar from "../img/starfilled.png";
import EmptyStarSmall from "../img/starempty_small.png";
import FullStarSmall from "../img/starempty_big.png";
import LoiIcon from "../img/loiicon.png";
import FpaIcon from "../img/fpaicon.png";
import ReactHoverObserver from 'react-hover-observer';
import { connect } from 'react-redux';
import TinyLoiIcon from '../img/tinyblueloi.png';
import TinyOfferIcon from '../img/tinyoffericon.png';
import PortfolioIcon from '../img/portfolioicon.png';
import BusIcon from '../img/busicon.png';

class LineItem extends Component {
    constructor(props) {
        super(props);

        this.state = {
            showHover: false,
            status: this.getSymbolAndColorByListingStatus(this.props.currentAsset.asset.ListingStatusDescription),
            isFavorited: false,
            isScrolling: false
        }

        this.changeHoverState = this.changeHoverState.bind(this);
        this.openPage = this.openPage.bind(this);
        this.forceHover = this.forceHover.bind(this);
        this.changeHoverStateTwo = this.changeHoverStateTwo.bind(this);
    }

    changeHoverState(e) {
        this.setState({ showHover: true });
    }

    changeHoverStateTwo(e) {
        this.setState({ showHover: false });
    }

    getSymbolAndColorByListingStatus(listingStatusDescription) {
        const result = {
            color: "",
            symbol: ""
        }

        switch (listingStatusDescription) {
            case "Available":
                result.color = "lightgreen";
                result.symbol = "A";
                break;
            case "Pending":
                result.color = "lightgray";
                result.symbol = "P";
                break;
            case "Sold Not Closed":
            case "Sold And Closed":
                result.color = "lightgray";
                result.symbol = "S";
                break;
        }

        return result;
    }

    openPage(pagetype) {
        switch (pagetype) {
            case "loi":
                window.open(`/LOI/LOI/${this.props.currentAsset.assetId}`);
                break;
            case "fpa":
                alert("No FPA available for this asset at this time");
                break;
        }
    }

    forceHover() {
        this.setState({ showHover: true });
    }

    render() {
        let nameFontBig = { fontSize: "14px" };
        console.log(this.props.currentAsset)
        return (
            <div className="AssetSearch_Lineitemwrap">
                <div className="AssetSearch_line_cr" style={{marginTop: "-8px"}}>
                    <img alt="" src={EmptyStarSmall}/>
                </div>
                <div className="AssetSearch_line_name" style={(this.props.viewOptions.isFullScreen) ? nameFontBig : {}}>{this.props.currentAsset.assetName}</div>
                <div className="AssetSearch_line_citystate">{this.props.currentAsset.assetCityState}</div>
                <div className="AssetSearch_line_type">{this.props.currentAsset.asset.AssetTypeAbbreviation}</div>
                <div className="AssetSearch_line_size">{(this.props.currentAsset.assetType === "Multi Family" || this.props.currentAsset.assetType === "MHP") ? this.props.currentAsset.units + " Units" : this.props.currentAsset.squareFeet + " SqFt"}</div>
                <div className="AssetSearch_line_pricingindic">{this.props.currentAsset.proformaSGI}</div>
                <div className="AssetSearch_line_pricingindic">{this.props.currentAsset.proformaSGI}</div>
                <div className="AssetSearch_line_cr">{this.props.currentAsset.capRate}</div>
                <div className="AssetSearch_line_cr">N/A</div>
                <div className="AssetSearch_line_occyr">{this.props.currentAsset.occupancyPercentage} {this.props.currentAsset.occupancyDate}</div>
                <div className="AssetSearch_line_price">{this.props.currentAsset.pricingCMV}<br />{this.props.currentAsset.lpCMV}</div>
                <div className="AssetSearch_line_act">
                    <img alt="" title="Letter of Intent" src={TinyLoiIcon} onClick={() => this.openPage("loi")}/>
                    <img alt="" title="Formal Purchase Agreement" src={TinyOfferIcon} onClick={() => this.openPage("fpa")}/>
                    {this.props.currentAsset.isPartOfPortfolio && <img alt="" src={PortfolioIcon}/>}
                </div>
            </div>
        );
    }
}

const mapStateToProps = state => ({
    viewOptions: state.assetSearchReducer.viewOptions.viewOptions
});

export default connect(mapStateToProps)(LineItem);