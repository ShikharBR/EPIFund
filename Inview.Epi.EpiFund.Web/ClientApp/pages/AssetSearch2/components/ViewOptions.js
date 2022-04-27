import React, { Component } from "react";
import DownArrow from '../img/downarrow.png';
import { connect } from 'react-redux'
import { updateViewOptions } from '../../../redux/actions/asset-search-action'
import DropModal from './DropModal';

class ViewOptions extends Component {
    constructor(props) {
        super(props);

        this.updateShowDrop = this.updateShowDrop.bind(this);
        this.changeScreenSize = this.changeScreenSize.bind(this);
        this.changeViewOption = this.changeViewOption.bind(this);
    }

    updateShowDrop() {
        this.props.viewOptions.showDrop = !this.props.viewOptions.showDrop;
        this.props.updateViewOptions(this.props.viewOptions);
    }

    changeScreenSize(value) {
        this.props.viewOptions.isFullScreen = value;
        this.props.updateViewOptions(this.props.viewOptions);
    }

    changeViewOption(value) {
        this.props.viewOptions.isLineView = value;
        this.props.updateViewOptions(this.props.viewOptions);
    }

    render() {
        return (
            <div className="AssetSearch_BasicSearchInput_wrap" style={{ width: "125px", marginTop: "10px" }} tabIndex="0" onFocus={this.updateShowDrop} onBlur={this.updateShowDrop}>
                View Options
                <div className="AssetSearch_DropDown_main">
                    <div>View Options</div>
                    <div><img src={DownArrow} alt="" /></div>
                </div>
                {this.props.viewOptions.showDrop && <div className="AssetSearch_DropModal_wrap animated fadeIn superfast">

                    <div style={{ paddingLeft: "5px", paddingRight: "5px", fontWeight: "bold" }}>Screen Size</div>
                    <div className={this.props.viewOptions.isFullScreen ? "AssetSearch_Option_wrap AssetSearch_Option_selected" : "AssetSearch_Option_wrap"} onClick={() => this.changeScreenSize(true)}>Fullscreen</div>
                    <div className={!this.props.viewOptions.isFullScreen ? "AssetSearch_Option_wrap AssetSearch_Option_selected" : "AssetSearch_Option_wrap"} onClick={() => this.changeScreenSize(false)}>In-Frame</div>

                    <div style={{ paddingLeft: "5px", paddingRight: "5px", fontWeight: "bold", marginTop: "15px" }}>Item Options</div>
                    <div className={this.props.viewOptions.isLineView ? "AssetSearch_Option_wrap AssetSearch_Option_selected" : "AssetSearch_Option_wrap"} onClick={() => this.changeViewOption(true)}>Line View</div>
                    <div className={!this.props.viewOptions.isLineView ? "AssetSearch_Option_wrap AssetSearch_Option_selected" : "AssetSearch_Option_wrap"} onClick={() => this.changeViewOption(false)}>Tile View</div>

                </div>}
            </div>
        );
    }
}

const mapStateToProps = state => ({
    viewOptions: state.assetSearchReducer.viewOptions.viewOptions
});

export default connect(mapStateToProps, { updateViewOptions })(ViewOptions);
