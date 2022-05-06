import React, { Component } from "react";
import ReactDOM from 'react-dom';
import Add1 from "./add1";
import Add2 from "./add2";
import Add3 from "./add3";
import Add4 from "./add4";
import Addasset from "./addasset";
import Claim1 from "./claim1";
import Claim2 from "./claim2";
import Claim3 from "./claim3";
import Claim4 from "./claim4";

class ClaimAsset extends Component {
  constructor(props) {
      super(props);

      this.state = {
          step: 0,
          addasset: {},
          add1: {},
          add2: {},
          add3: {},
          add4: {},
          addsummarydata: {},
          claim1: {},
          claim2: {},
          claim3: {},
          claim4: {},
          claimsummarydata: {}
      }

      this.addInfo = this.addInfo.bind(this);
      this.backStep = this.backStep.bind(this);
      this.goNext = this.goNext.bind(this);
      this.skipStep = this.skipStep.bind(this);
      this.getAddSummary = this.getAddSummary.bind(this);
      this.getClaimSummary = this.getClaimSummary.bind(this);
      this.sendClose = this.sendClose.bind(this);
  }

  sendClose() {
      this.props.closepop()
  }

  goNext(data, page) {
      // Need this to fix the decimal because floating point numbers make computers upset
      let fixednumber = Number((this.state.step + 0.1).toFixed(1));
      this.setState({ step: fixednumber, [page]: data });
  }

  addInfo(data, type) {
      this.setState({ addasset: data, step: type });
  }

  backStep() {
      if (this.state.step === 1 || this.state.step === 2) {
          this.setState({ step: 0 });
      }
      if (this.state.step % 1 !== 0) {
          // Need this to fix the decimal because floating point numbers make computers upset
          let fixednumber = Number((this.state.step - 0.1).toFixed(1));
          this.setState({ step: fixednumber });
      }
  }

  skipStep() {
      //Same as next, just no data save
      let fixednumber = Number((this.state.step + 0.1).toFixed(1));
      this.setState({ step: fixednumber });
  }

  getAddSummary() {
       console.log('add1: ', this.state.add1);
       console.log('addasset: ', this.state.addasset);
      let summarydata = {
          'State': this.state.addasset.addasset.State,
          'address': this.state.addasset.addasset.address,
          'apns': this.state.addasset.addasset.apns,
          'assetname': this.state.addasset.addasset.assetname,
          'address': this.state.addasset.addasset.address,
          'city': this.state.addasset.addasset.city,
          'county': this.state.addasset.addasset.county,
          'note': this.state.addasset.addasset.note,
          'type': this.state.addasset.addasset.type,
          'aquisition': this.state.add1.aquisition,
          'purchaseprice': this.state.add1.purchaseprice,
          'holding': this.state.add1.currentholding,
          'operating': this.state.add1.currentoperating,
          'holdingid': this.state.add1.holding[14].HoldingCompanyId,
          'operatingid': this.state.add1.operating[13].OperatingCompanyId,
          'terms': this.state.add1.terms
      }
      this.setState({
          addsummarydata: summarydata
      })
  }

  getClaimSummary() {
      let summarydata = {
          'State': this.state.addasset.asset.State,
          'address': this.state.addasset.asset.Address1,
          'address2': this.state.addasset.asset.Address2,
          'apns': this.state.addasset.asset.Apns,
          'assetname': this.state.addasset.asset.Name,
          'city': this.state.addasset.asset.City,
          'county': this.state.addasset.asset.County,
          'note': this.state.addasset.asset.IsNote,
          'type': this.state.addasset.asset.AssetType,
          'aquisition': this.state.claim2.aquisition,
          'purchaseprice': this.state.claim2.purchaseprice,
          'holding': this.state.claim2.currentholding,
          'operating': this.state.claim2.currentoperating
      }
      this.setState({
          claimsummarydata: summarydata
      })
  }

  render() {
      return (
          <div className="App">
              <div className="modal_backdrop">
                  {!this.state.step && <Addasset addinfo={this.addInfo} data={this.state.addasset} close={this.sendClose} />}

                  {this.state.step === 1 && <Add1 goback={this.backStep} gonext={this.goNext} data={this.state.add1} close={this.sendClose}/>}
                  {this.state.step === 1.1 && <Add2 goback={this.backStep} gonext={this.goNext} data={this.state.add2} close={this.sendClose}/>}
                  {this.state.step === 1.2 && <Add3 goback={this.backStep} gonext={this.goNext} data={this.state.add3} skipstep={this.skipStep} close={this.sendClose}/>}
                  {this.state.step === 1.3 && <Add4 goback={this.backStep} data={this.state.add4} getsummary={this.getAddSummary} summarydata={this.state.addsummarydata} close={this.sendClose}/>}

                  {this.state.step === 2 && <Claim1 goback={this.backStep} gonext={this.goNext} close={this.sendClose}/>}
                  {this.state.step === 2.1 && <Claim2 goback={this.backStep} gonext={this.goNext} data={this.state.claim2} close={this.sendClose}/>}
                  {this.state.step === 2.2 && <Claim3 goback={this.backStep} gonext={this.goNext} data={this.state.claim3} close={this.sendClose}/>}
                  {this.state.step === 2.3 && <Claim4 goback={this.backStep} data={this.state.claim4} getsummary={this.getClaimSummary} summarydata={this.state.claimsummarydata} close={this.sendClose}/>}
              </div>
          </div>
      );
  }
}

String.prototype.trunc = String.prototype.trunc ||
  function (n) {
      return (this.length > n) ? this.substr(0, n - 1) + '&hellip;' : this;
  };

export default ClaimAsset;