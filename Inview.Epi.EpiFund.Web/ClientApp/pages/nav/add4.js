import React, { Component } from "react";
import SuccessModal from "./successmodal";

class Add4 extends Component {
    constructor(props) {
        super(props);

        this.state = {
            status: null,
            link: null
        }

        this.back = this.back.bind(this);
        this.nextStep = this.nextStep.bind(this);
    }

    back() {
        this.props.goback();
    }

    nextStep() {
        console.log(this.props);

        let payload = {
            AssetName: this.props.summarydata.assetname,
            AssetType: parseInt(this.props.summarydata.type),
            City: this.props.summarydata.city,
            State: this.props.summarydata.State,
            Address1: this.props.summarydata.address,
            County: this.props.summarydata.county,
            IsNote: this.props.summarydata.note,
            OperatingCompanyId: api.tempGlobal.OperatingCompanyId,
            HoldingCompanyId: api.tempGlobal.HoldingCompanyId,
            AquisitionDate: this.props.summarydata.aquisition,
            SellerTerm: parseInt(this.props.summarydata.terms),
            PurchasePrice: parseInt(this.props.summarydata.purchaseprice.replace(/,/g, '').replace(/\$/g, '')),
            Apns: this.props.summarydata.apns.map(apn => apn.value),
        }
        console.log('create asset payload', payload);
        api.createAsset(payload, (err, result) => {
            if (err) throw err;
            else {
                if (result.success) {
                    //alert('successfully created asset');
                    let docPayload = new FormData();
                    docPayload.append('assetId', result.assetId);
                    docPayload.append('stateDocumentation', api.tempGlobal.docStateDocumentation);
                    docPayload.append('vestingDeed', api.tempGlobal.docVestingDeed);
                    docPayload.append('titleInsurancePolicy', api.tempGlobal.docTitleInsurancePolicy);
                    docPayload.append('other', api.tempGlobal.docOther);
                    docPayload.append('om', api.tempGlobal.docOm);

                    api.uploadFilesForNewlyCreatedAsset(docPayload, (docErr, docResult) => {
                        if (docErr) throw docErr;
                        else {
                            if (docResult.success) {
                              //alert('successfully uploaded documents')
                              //document.location.href = `/Admin/UpdateAssetVersion/${result.assetId}`;
                              if (!api.tempGlobal.docOm) {
                                this.setState({ status: "success_noOm", link: result.assetId });
                              }
                              else {
                                this.setState({ status: "success_Om", link: result.assetId });
                              }
                            } else {
                              alert('failed to upload files and/or create asset version(feels like too much processing for this method)')
                            }
                        }
                    });
                } else {
                    alert(`failed to create asset, try again in the past. ${result.message}`); // messages arent guaranteed to be populated in this method
                }
            }
        });
    }

    componentWillMount() {
        this.props.getsummary();
    }

    handleSlide(statestatus) {
        switch(statestatus) {
            case (null):
                return (
                    <div className="modalwrap modal_big animated slideInRight faster">
                        <div className="topbar topbar_orange">
                            Add Asset <a>- Summary</a>
                            <div className="closebutton" onClick={() => this.props.close()}>×</div>
                        </div>
                        <div className="modal_contentwrap">
                            <div className="summarytitle">{this.props.summarydata.assetname}</div>
                            <div className="summarymain">
                                <div className="summarymain_top">
                                    <div className="summarymain_pic">
                                        ?
                            <div>{this.props.summarydata.city}, {this.props.summarydata.county}</div>
                                    </div>
                                    <div className="summarymain_top_right">
                                        <div className="summarymain_top_right_left">
                                            <h2>Property Address</h2>
                                            <h3>{this.props.summarydata.address}</h3>
                                            <h3>{this.props.summarydata.city}, {this.props.summarydata.State}</h3>
                                            <div>
                                                <h2>{this.props.summarydata.county}</h2>
                                            </div>
                                        </div>
                                        <div className="summarymain_top_right_right">
                                            <div>
                                                <h2>Original Aquisition Date</h2>
                                                <h3>{this.props.summarydata.aquisition}</h3>
                                            </div>
                                            <div>
                                                <h2>Original Purchase Price</h2>
                                                <h1>{(this.props.summarydata.purchaseprice) && this.props.summarydata.purchaseprice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}</h1>
                                            </div>
                                            <div>
                                                {this.props.summarydata.note && <h4><strong>Note -</strong> Secured by Mixed Use Commercial Property</h4>}
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

                                {
                                    (!api.tempGlobal.docOther) ? 
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

                                {
                                    (!api.tempGlobal.docOm) ? 
                                    <div className="documentitem di_nouploaded di_big">
                                        <div>Offering Memorandum</div>
                                    </div> :
                                    <div className="documentitem di_uploaded di_big">
                                        <div>Offering Memorandum</div>
                                        <img alt="" src='https://i.imgur.com/zi2xAOz.png' />
                                    </div>
                                }

                            </div>
                            <div className="modalfooter">
                                <div className="footerextra footerapn">
                                    <strong>APN #: </strong> {this.props.summarydata.apns && this.props.summarydata.apns.map(item => {
                                        return item.value
                                    }).join(", ")}
                                </div>
                                <div className="buttonwrap">
                                    <div className="submitbtn submitorange top20 hvr-float-shadow" onClick={this.back}>Back</div>
                                    <div className="submitbtn top20 hvr-float-shadow" onClick={this.nextStep}>Create Asset</div>
                                </div>
                            </div>
                        </div>
                    </div>
                );
                break;
            case ("success_noOm"):
                return <SuccessModal omStatus={false} link={this.state.link} type={"add"}/>
                break;
            case ("success_Om"):
                return <SuccessModal omStatus={true} link={this.state.link} type={"add"}/>
                break;
            default: 
                break;
        }
    }

    render() {
        //console.log(this.props.summarydata);
        return (
            <>
                {this.handleSlide(this.state.status)}
            </>
        );
    }
  }

export default Add4;
