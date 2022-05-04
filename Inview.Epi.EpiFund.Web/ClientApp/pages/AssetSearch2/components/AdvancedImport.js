import React, { Component } from "react";

class AdvancedImport extends Component {
    constructor(props) {
        super(props);

        this.state = {
        }

        this.sendImportType = this.sendImportType.bind(this);
    }

    sendImportType(type) {
        this.props.getImportItems(type);
    }

    render() {
        return(
            <>
                <div className="AssetSearch_BigImportButton" onClick={() => this.sendImportType("controlled")}>Import all assets you control</div>
                <div className="AssetSearch_BigImportButton" onClick={() => this.sendImportType("favorited")}>Import all favorited assets</div>
            </>
        );
    }
}

export default AdvancedImport;