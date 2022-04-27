import React, { Component } from "react";
import PortfolioIcon from '../img/portfolioicon.png';
import TimerIcon from '../img/timericon.png';

class TileItem extends Component {
    constructor(props) {
        super(props);

        this.state = {
        }

        this.checkLength = this.checkLength.bind(this);
        this.openAssetPage = this.openAssetPage.bind(this);
    }

    checkLength(value) {
        if (value != null && value.length > 24) {
            return value.slice(0, 24) + "...";
        }
        return value;
    }

    openAssetPage(id) {
        window.open(`/DataPortal/ViewAsset/${id}`);
    }

    render() {
        return (
            <div className="AssetSearch2_Tile_Wrap animated fadeIn faster" onClick={() => this.openAssetPage(this.props.currentAsset.assetId)}>
                <div className="AssetSearch2_Tile_Header AssetSearch_BlueGradient"></div>
                <div className="AssetSearch2_Tile_Top">
                    <div>
                        <h1>{this.checkLength(this.props.currentAsset.assetName)}</h1>
                        <h4>{this.props.currentAsset.assetCityState} - {this.props.currentAsset.asset.Zip}</h4>
                        <h4><strong>{this.props.currentAsset.asset.AssetTypeDescription} {(this.props.currentAsset.squareFeet) && this.props.currentAsset.squareFeet}</strong></h4>
                    </div>
                    {(this.props.currentAsset.units > 0 || this.props.currentAssets) &&
                        <div>
                            <h2>{this.props.currentAsset.units}</h2>
                            <div style={{ color: "#35a6d7", fontSize: "16px", width: "100%", textAlign: "right", letterSpacing: "1px", paddingTop: "2px" }}>Units</div>
                        </div>}
                </div>

                <div className="AssetSearch2_Tile_picturewrap" style={{ backgroundImage: `url(${this.props.currentAsset.image})` }}>
                    <div className="AssetSearch2_Tile_iconswrap">
                        <div>{this.props.currentAsset.isPartOfPortfolio && <img alt="" src={PortfolioIcon} />}</div>
                        <div>{this.props.currentAsset.callForOffersDateSoon && <img alt="" title={this.props.currentAsset.callForOffersDate} src={TimerIcon} />}</div>
                    </div>
                </div>

                <div className="AssetSearch2_Tile_Bot">
                    <div style={{ width: "50%" }}>
                        <div><strong>SGI:</strong> {this.props.currentAsset.proformaSGI}</div>
                        <div><strong>NOI:</strong> {this.props.currentAsset.proformaNOI}</div>
                        <div><strong>CAP:</strong> {this.props.currentAsset.capRate}</div>
                    </div>
                    <div style={{ width: "50%" }}>
                        <div><strong>Pricing:</strong> {this.props.currentAsset.lpCMV}</div>
                        <div><strong>Occ %:</strong> {this.props.currentAsset.occupancyPercentage}</div>
                        <div><strong>Year Built:</strong> {this.props.currentAsset.asset.YearBuilt}</div>
                    </div>
                </div>
            </div>
        );
    }
}

export default TileItem;