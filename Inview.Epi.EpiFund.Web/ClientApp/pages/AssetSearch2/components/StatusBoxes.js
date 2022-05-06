import React, { Component } from "react";
import StatusBox from "./StatusBox";
import { connect } from 'react-redux';

let lightBlue = "#58b2f4";
let moneyGreen = "#0dbb26";

class StatusBoxes extends Component {
    constructor(props) {
        super(props);

        this.state = {
            StatusBoxes: [
                {title: "Total Assets", total: this.props.resultOptions.Total, color: lightBlue},
                {title: "Published Assets", total: this.props.resultOptions.PublishedAssets, color: lightBlue},
                {title: "Market Value", total: this.props.resultOptions.TotalAssetVal, color: moneyGreen},
                {title: "Multi-Family", total: this.props.resultOptions.MultiFamUnits, color: lightBlue},
                {title: "Leasable Sq. Ft", total: this.props.resultOptions.TotalSqFt, color: lightBlue}
            ]
        }

        this.totalRouting = this.totalRouting.bind(this);
    }

    totalRouting(title) {
        switch(title) {
            case "Total Assets": 
                return this.props.resultOptions.Total;
            case "Published Assets":
                return this.props.resultOptions.PublishedAssets;
            case "Market Value":
                return this.props.resultOptions.TotalAssetVal;
            case "Multi-Family":
                return this.props.resultOptions.MultiFamUnits;
            case "Leasable Sq. Ft":
                return this.props.resultOptions.TotalSqFt;
            default:
                return 0;
        }
    }

    render() {
        return(
            <div className="AssetSearch_StatusBoxes_wrap">
                {
                    this.state.StatusBoxes.map((item, index) => {
                        return <StatusBox key={index} title={item.title} total={this.totalRouting(item.title)} color={item.color}/>
                    })
                }
            </div>
        );
    }
}

const mapStateToProps = state => ({
    resultOptions: state.assetSearchReducer.assetSearch.searchResult
});

export default connect(mapStateToProps)(StatusBoxes);