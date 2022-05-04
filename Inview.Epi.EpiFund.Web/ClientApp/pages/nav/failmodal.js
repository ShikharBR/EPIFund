import React, { Component } from "react";

class FailModal extends Component {
    constructor(props) {
        super(props);
  
        this.redirectTo = this.redirectTo.bind(this);
    }
  
    redirectTo(link) {
        window.open(link, '_blank');
    }
  
    render() {
        return (
            <div className="statusmodal_wrap">
                <div className="statusmodal_title">
                    <img alt="" src="https://s3-us-west-1.amazonaws.com/dev-uscreonline-content/claimasset_statusmodalicons/warningicon.png" />
                    <h1 style={{ color: "#ad4141" }}>Asset Claim <strong>Error!</strong></h1>
                </div>
                <div className="statusmodal_textwrap">
                    <center>Error message can go right here</center>
                </div>
                <div className="statusmodal_controlwrap">
                    <div className="statusmodal_redbutton" onClick={() => this.redirectTo("#")}>BACK</div>
                </div>
            </div>
        );
    }
  }

export default FailModal;