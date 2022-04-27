import React, { Component } from "react";

class SuccessModal extends Component {
    constructor(props) {
        super(props);
  
        this.redirectTo = this.redirectTo.bind(this);
    }
  
    redirectTo(link) {
        document.location.href = link;
    }
  
    render() {
        return (
            <div className="statusmodal_wrap">
                <div className="statusmodal_title">
                    <img alt="" src="https://s3-us-west-1.amazonaws.com/dev-uscreonline-content/claimasset_statusmodalicons/checkmark.png" />
                    <h1 style={{ color: "#41ad49" }}>
                    {(this.props.type === "add") ? "Add Asset" : "Asset Claim"} <strong>Success!</strong>
                    </h1>
                </div>
                <div className="statusmodal_textwrap">
                    {(!this.props.omStatus) && "Thank you for completing our Proof of Ownership protocol. USCRE will review your documentation and get back to you within 3 (three) business days. In the meantime, you are welcome to edit your Asset file, including adjusting any of the data fields and adding or remove images and videos as you please. Once your Proof of Ownership is approved, you will also be able to publish the Asset to your preferred audience."}
                    {(this.props.omStatus) && "Thank you for completing our Proof of Ownership protocol and for uploading the Offering Memorandum. USCRE will review your documentation, populate your Asset file with data from your Offering Memorandum, and get back to you within 3 (three) business days. Please note that in the meantime, you will be able to view, but not edit, your new Asset file until data entry is complete. We will notify you as soon as it’s ready!"}
                </div>
                <div className="statusmodal_controlwrap">
                    <div className="statusmodal_greenbutton" onClick={() => this.redirectTo(`/Admin/UpdateAssetVersion/${this.props.link}`)}>VIEW ASSET</div>
                </div>
            </div>
        );
    }
  }

export default SuccessModal;