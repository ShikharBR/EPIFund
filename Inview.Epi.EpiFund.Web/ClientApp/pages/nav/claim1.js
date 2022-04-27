import React, { Component } from "react";

class Claim1 extends Component {
    constructor(props) {
        super(props);
  
        this.back = this.back.bind(this);
        this.nextStep = this.nextStep.bind(this);
        this.getAssetType = this.getAssetType.bind(this);
    }
  
    back() {
        this.props.goback();
    }
  
    nextStep() {
        this.props.gonext(this.state, "claim1");
    }

    getAssetType(assetnum) {
        switch (assetnum) {
            case 1:
                return "Retail Tenant Property";
                break;
            case 2:
                return "Office Tenant Property";
                break;
            case 3:
                return "Multi-Family";
                break;
            case 4:
                return "Industrial Tenant Property";
                break;
            case 5:
                return "MHP";
                break;
            case 6:
                return "Fuel Service Retail Property";
                break;
            case 7:
                return "Medical Tenant Property";
                break;
            case 8:
                return "Mixed Use Commercial Property";
                break;
            case 10:
                return "Other";
                break;
            case 11:
                return "Resort/Hotel/Motel Property";
                break;
            case 12:
                return "Single Tenant Property (All Type)";
                break;
            case 13:
                return "Fractured Condominium Portfolio's";
                break;
            case 14:
                return "Mini-Storage Property";
                break;
            case 15:
                return "Parking Garage Property";
                break;
            case 16:
                return "Secured CRE Paper";
                break;
            case 17:
                return "Land";
                break;
            default:
                return "?";
                break;
        }
    }
  
    render() {
        console.log(api.tempGlobal.claimedAsset);
        let image;
        if (api.tempGlobal.claimedAsset.Image) image = `http://images.uscreonline.com/${api.tempGlobal.claimedAsset.AssetId}/${api.tempGlobal.claimedAsset.Image.FileName}`;
        return (
            <div className="modalwrap modal_med animated slideInRight faster">
                <div className="topbar topbar_orange">
                    Asset Found
              <div className="closebutton" /*onClick={this.handleClose}*/ onClick={() => this.props.close()}>×</div>
                </div>
                <div className="modal_contentwrap">
                    <div className="summarytitle">{api.tempGlobal.claimedAsset.Name}</div>
                    <div className="summarymain summarymain_thin">
                        <div className="summarymain_top summarymain_top_small">
                            <div className="summarymain_pic summarymain_pic_small">
                                {image && <img src={image} width="248px" height="168px"></img>}
                                {!image && <span>?</span>}
                            </div>
                            <div className="summarymain_top_right summarymain_top_right_small">
                                <div className="summarymain_top_right_right summarymain_top_right_right_small">
                                    <div className="flexheight100">
                                        <h2>{api.tempGlobal.claimedAsset.Address1}</h2>
                                        <h2>{api.tempGlobal.claimedAsset.Address2}</h2>
                                        <h3>{api.tempGlobal.claimedAsset.City}, {api.tempGlobal.claimedAsset.State}</h3>
                                        <h3>{api.tempGlobal.claimedAsset.County} County</h3>
                                        {api.tempGlobal.claimedAsset.IsMultiFamily && <h2>{api.tempGlobal.claimedAsset.Units} Units</h2>}
                                        {!api.tempGlobal.claimedAsset.IsMultiFamily && <h2>{api.tempGlobal.claimedAsset.LotSize} Lot</h2>}
                                        <h2>{Number(api.tempGlobal.claimedAsset.SqFt).toLocaleString()} Sq. Ft.</h2>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="modalfooter">
                        <div className="footerextra">
                            <div>
                                <h3>{this.getAssetType(api.tempGlobal.claimedAsset.AssetType)}</h3>
                            </div>
                        </div>
                        <div className="buttonwrap pad20">
                            <div className="submitbtn hvr-float-shadow" onClick={this.back}>Back</div>
                            <div className="submitbtn submitorange" onClick={this.nextStep}>Claim Asset</div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
  }

export default Claim1;