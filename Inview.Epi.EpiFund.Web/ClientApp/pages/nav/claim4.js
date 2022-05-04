import React, { Component } from "react";
import SuccessModal from "./successmodal";

class Claim4 extends Component {
    constructor(props) {
        super(props);
  
        this.state = {
            status: null
        }
  
        this.back = this.back.bind(this);
        this.nextStep = this.nextStep.bind(this);
        this.getAssetType = this.getAssetType.bind(this);
        this.handleSlide = this.handleSlide.bind(this);
    }
  
    back() {
        this.props.goback();
    }
  
    nextStep() {
        api.claimAsset({
            assetId: api.tempGlobal.claimedAsset.AssetId,
            operatingCompanyId: api.tempGlobal.OperatingCompanyId,
            holdingCompanyId: api.tempGlobal.HoldingCompanyId,
            aquisitionDate: api.tempGlobal.claimedAsset.aquisition,
            sellerTerm: api.tempGlobal.claimedAsset.sellertermId,
            purchasePrice: api.tempGlobal.claimedAsset.purchaseprice,
        }, (err, result) => {
            if (err) throw err;
            else {
                let docPayload = new FormData();
                docPayload.append('assetId', api.tempGlobal.claimedAsset.AssetId);
                docPayload.append('stateDocumentation', api.tempGlobal.docStateDocumentation);
                docPayload.append('vestingDeed', api.tempGlobal.docVestingDeed);
                docPayload.append('titleInsurancePolicy', api.tempGlobal.docTitleInsurancePolicy);
                docPayload.append('other', api.tempGlobal.docOther);
                api.uploadFilesForClaimAsset(docPayload, (docErr, docResult) => {
                    if (docErr) {
                        throw err;
                        this.setState({ status: "error" });
                    }
                    else {
                        //alert('successfully did the things')
                        //document.location.href = `/Admin/UpdateAssetVersion/${api.tempGlobal.claimedAsset.AssetId}`;
                        this.setState({ status: "success" });
                    }
                });
            }
        });
    }
  
    componentWillMount() {
        this.props.getsummary();
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

    handleSlide(statestatus) {
        let image;
        if (api.tempGlobal.claimedAsset.Image) image = `http://images.uscreonline.com/${api.tempGlobal.claimedAsset.AssetId}/${api.tempGlobal.claimedAsset.Image.FileName}`;
        //console.log(statestatus);
        switch (statestatus) {
            case (null):
                return (<div className="modalwrap modal_big animated slideInRight faster">
                    <div className="topbar topbar_orange">
                        Claim Asset <a>- Summary</a>
                        <div className="closebutton" onClick={() => this.props.close()}>×</div>
                    </div>
                    <div className="modal_contentwrap">
                        <div className="summarytitle">{api.tempGlobal.claimedAsset.Name} - {this.getAssetType(api.tempGlobal.assets[0].AssetType)}</div>
                        <div className="summarymain">
                            <div className="summarymain_top">
                                <div className="summarymain_pic">
                                    {image && <img src={image} width="100%" height="100%"></img>}
                                    {!image && <span>?</span>}
                                </div>
                                <div className="summarymain_top_right">
                                    <div className="summarymain_top_right_left">
                                        <h2>Property Address</h2>
                                        <h3>{this.props.summarydata.address}</h3>
                                        {this.props.summarydata.address2 && <h3>{this.props.summarydata.address2}</h3>}
                                        <h3>{this.props.summarydata.city}, {this.props.summarydata.State}</h3>
                                        <h2>{this.props.summarydata.county} County</h2>
                                        <div>
                                            {this.props.summarydata.note && <h4><strong>Note -</strong> Secured by Mixed Use Commercial Property</h4>}
                                        </div>
                                    </div>
                                    <div className="summarymain_top_right_right">
                                        <div>
                                            <h6 style={{ fontSize: "20px !important" }}>Original Purchase Price</h6>
                                            <h1>{this.props.summarydata.purchaseprice && this.props.summarydata.purchaseprice}</h1>
                                            <h1 className="h1extrasmall">Aquisition Date: {this.props.summarydata.aquisition}</h1>
                                            {(api.tempGlobal.assets[0].Units) ? <h6>{api.tempGlobal.assets[0].Units + " Units"}</h6> : <h6> </h6>}
                                            {(api.tempGlobal.assets[0].SqFt) ? <h6>{api.tempGlobal.assets[0].SqFt.toLocaleString() + " Sq. Ft."}</h6> : <h6> </h6>}
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="dualwrap2">
                                <div className="summaryochc">
                                    <strong>Holding Company:</strong> {this.props.summarydata.holding}
                                </div>
                                <div className="summaryochc">
                                    <strong>Operating Company:</strong> {this.props.summarydata.operating}
                                </div>
                            </div>
                        </div>
                        <div className="documentswrap">
                            <div className="documentitem di_uploaded di_small">
                                <div>Title Insurance Policy</div>
                                <img alt="" src='https://i.imgur.com/zi2xAOz.png' />
                            </div>

                            <div className="documentitem di_uploaded di_small">
                                <div>Vesting Deed</div>
                                <img alt="" src='https://i.imgur.com/zi2xAOz.png' />
                            </div>

                            {(!api.tempGlobal.docOther) ?
                                <div className="documentitem di_nouploaded di_small">
                                    <div>Other Documents</div>
                                </div> :

                                <div className="documentitem di_uploaded di_small">
                                    <div>Other Documents</div>
                                    <img alt="" src='https://i.imgur.com/zi2xAOz.png' />
                                </div>
                            }

                            <div className="documentitem di_uploaded di_big">
                                <div>State Documentation</div>
                                <img alt="" src='https://i.imgur.com/zi2xAOz.png' />
                            </div>
                        </div>
                        <div className="modalfooter">
                            <div className="footerextra footerapn">
                                <strong>APN #: </strong> {this.props.summarydata.apns && Object.keys(this.props.summarydata.apns[0]).map(item => {
                                    if (this.props.summarydata.apns[0][item]) {
                                        return this.props.summarydata.apns[0][item];
                                    }
                                }).join(", ")}
                            </div>
                            <div className="buttonwrap">
                                <div className="submitbtn submitorange top20 hvr-float-shadow" onClick={this.back}>Back</div>
                                <div className="submitbtn top20 hvr-float-shadow" onClick={this.nextStep}>Claim Asset</div>
                            </div>
                        </div>
                    </div>
                </div>);
            case ("success"):
                return <SuccessModal link={api.tempGlobal.claimedAsset.AssetId} type={"claim"} />
        }
    }
  
    render() {
        return (
            <>
                {this.handleSlide(this.state.status)}
            </>
        );
    }
  }

export default Claim4;