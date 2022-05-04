
class AddAssetPopup extends React.Component {
  render() {
    return (
      <div className="popup add-asset">
        <div className="inner">
          <h1>Add Asset</h1>
          <button onClick={this.props.closePopup}>close me</button>
          <label>Asset Name</label>
          <input type="text" id="txtName" /><br />
          <label>Address</label>
          <input type="text" id="txtAddress" /><br />
          <label>City</label>
          <input type="text" id="txtCity" /><br />
          <label>State</label>
          <select id="ddlState">
            <option value=""></option>
            <option>NV</option>
          </select><br />
          <label>County</label>
          <input type="text" id="txtCounty" /><br />
          <label>APNS</label>
          <input type="text" id="txtApn0" />
          <input type="text" id="txtApn1" />
          <input type="text" id="txtApn2" />
          <input type="text" id="txtApn3" />
          <input type="text" id="txtApn4" />
          <label>Asset Type</label>
          <select id="ddlAssetType">
            <option value=""></option>
            <option value="3">Multi Family</option>
          </select><br />
          <input type="checkbox" />
          <label><i>Note Asset</i></label><br />
          <button className="btn" id="btnAddAsset" onClick={this.props.onAddAssetClick}>Add Asset</button>
        </div>
      </div>
    )
  }
}
// create asset popups
class AddAssetStep1Popup extends React.Component {
  render() {
    return (
      <div className="popup add-asset-step-1">
        <div className="inner">
          <h1>Add Asset</h1>
          <button onClick={this.props.closePopup}>close</button>
          <h4>Please select your Operating Company and Holding Company</h4>
          <div className="a-row">
            <div className="oc">
              <select id="ddlAssetDetailsOperatingCompany" class="ddlOperatingCompany"></select>
            </div>
            <div className="hc">
              <select id="ddl.AssetDetailsHoldingCompany" class="ddlHoldingCompany"></select>
            </div>
          </div>
          <div className="a-row">
            <div className="aq-date">
              <label>Original Aquisition Date</label>
              <input id="txtAquisitionDate" />
            </div>
            <div className="price">
              <label>Original Purchase Price</label>
              <input id="txtOriginalPurchasePrice" type="number" />
            </div>
          </div>
          <div className="terms">
            <label>Terms</label>
            <select id="ddlTerms"></select>
          </div>
          <button id="btnContinue" onClick={this.props.onContinueClick}>Continue</button>
        </div>
      </div>
    )
  }
}
class AddAssetStep2Popup extends React.Component {
  render() {
    return (
      <div className="popup add-asset-step-2">
        <div className="inner">
          <h1>Please upload your proof of title documents here</h1>
          <p>Maximum upload size 25MB per Document</p>
          <button>Upload File</button>
          <p>Title Insurance Policy</p>
          <button>Upload File</button>
          <p>Vesting Deed</p>
          <button>Upload File</button>
          <p>State Requisite Corporate<br/ >Documentation</p>
          <button>Upload File</button>
          <p>Other</p>
          <button>Upload File</button>
          <hr />
          <button onClick={this.props.onContinueClick}>Continue</button>
          <button onClick={this.props.onBackClick}>Back</button>
        </div>
      </div>
    )
  }
}
class AddAssetStep3Popup extends React.Component {
  render() {
    return (
      <div className="popup add-asset-step-3">
        <div className="inner">
          <h5>Let us do the work for you! Upload your Asset's Offering Memorandum and we will populate your new Asset File at no charge. We will notify you when it's ready.</h5>
          <p>Maximum upload size 25MB per Document</p>
          <b>OM</b>
          <button>Upload File</button>
          <hr />
          <button onClick={this.props.onBackClick}>Back</button>
          <button onClick={this.props.onSkipClick}>Skip</button>
          <button onClick={this.props.onContinueClick}>Continue</button>

        </div>
      </div>
    )
  }
}
class AddAssetSummaryPopup extends React.Component {
  render() {
    return (
      <div className="popup add-asset-summary">
        <div className="inner">
          <h3>{this.props.ProjectName}</h3>
          <div className="asset-data-0">

            <div className="0-top">
              <div className="image-wrapper">
                <div className="image">
                </div>
                <div className="image-address-wrapper">
                  <div className="image-address">
                    <span className="city">{this.props.City}</span>, <span className="state">{this.props.State}</span> <span className="zip">{this.props.Zip}</span>
                  </div>
                </div>
              </div>
            </div>

            <div className="0-bottom">
              <div className="holding-wrapper">
                <div className="holding">
                  {this.props.HoldingCompany.Title}
                </div>
              </div>
              <div className="operating-wrapper">
                <div className="operating">
                  {this.props.OperatingCompany.Title}
                </div>
              </div>
            </div>
          </div>

          <div className="asset-data-1">
            bunch of documents coming soon
          </div>
          <div className="asset-data-2">
            <div className="apns">
              <b>APN #:</b> {this.props.apns}
            </div>
            <div className="buttons">
              <button onClick={this.props.onBackClick}>Back</button>
              <button onClick={this.props.onCreateAssetClick}>Create Asset</button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}
// end create asset popups

// claim asset popups
class AssetFoundPopup extends React.Component {
  render() {
    return (
      <div className="popup asset-found">
        <div className="inner">
          <h1>Asset Found</h1>
          <button onClick={this.props.closePopup}>close me</button>
          <p>Name: {this.props.Name}</p>
          <div className="asset-outer">
            <div className="asset-inner">
              <div className="image-container">
                <div className="image">
                </div>
                <div className="image-address">
                </div>
              </div>
              <div className="info-container">
                <span className="street-address">{this.props.Address}</span>
                <span className="cityAndState">{this.props.City}, {this.props.State}</span>
                <span className="county">{this.props.County}</span>
                <span className="dp0">{this.props.DataPoint0}</span>
                <span className="dp1">{this.props.DataPoint1}</span>
              </div>
            </div>
          </div>

          <div className="moreDataAndAButton">
            stirng of data and a label x2
            <button onClick={this.props.onClaimAssetClick}>Claim Asset</button>
          </div>
        </div>
      </div>
    )
  }
}
class ClaimAssetStep1Popup extends React.Component {
  render() {
    return (
      <div className="popup claim-asset">
        <div className="inner">
          <h1>Claim Asset</h1>
          <button onClick={this.props.closePopup}>close</button>
          <h4>Please select your Operating Company and Holding Company</h4>
          <div className="a-row">
            <div className="oc">
              <select id="ddlOperatingCompany"></select>
            </div>
            <div className="hc">
              <select id="ddlHoldingCompany"></select>
            </div>
          </div>
          <div className="a-row">
            <div className="aq-date">
              <label>Original Aquisition Date</label>
              <input id="txtAquisitionDate" />
            </div>
            <div className="price">
              <label>Original Purchase Price</label>
              <input id="txtOriginalPurchasePrice" type="number" />
            </div>
          </div>
          <div className="terms">
            <label>Terms</label>
            <textarea id="txtTerms"></textarea>
          </div>
          <button id="btnContinue" onClick={this.props.onContinueClick}>Continue</button>
        </div>
      </div>
    )
  }
}
class ClaimAssetStep2Popup extends React.Component {
  render() {
    return (
      <div className="popup claim-asset-step-2">
        <div className="inner">
          <h1>Please upload your proof of title documents here</h1>
          <p>Maximum upload size 25MB per Document</p>
          <button>Upload File</button>
          <p>Title Insurance Policy</p>
          <button>Upload File</button>
          <p>Vesting Deed</p>
          <button>Upload File</button>
          <p>State Requisite Corporate<br/ >Documentation</p>
          <button>Upload File</button>
          <p>Other</p>
          <button>Upload File</button>
          <hr />
          <button onClick={this.props.onBackClick}>Back</button>
          <button onClick={this.props.onContinueClick}>Continue</button>
        </div>
      </div>
    )
  }
}
class ClaimAssetSummaryPopup extends React.Component {
  render() {
    return (
      <div className="popup claim-asset-summary">
        <div className="inner">
          <h3>{this.props.ProjectName}</h3>
          <div className="asset-data-0">

            <div className="0-top">
              <div className="image-wrapper">
                <div className="image">
                </div>
                <div className="image-address-wrapper">
                  <div className="image-address">
                    <span className="city">{this.props.City}</span>, <span className="state">{this.props.State}</span> <span className="zip">{this.props.Zip}</span>
                  </div>
                </div>
              </div>
            </div>

            <div className="0-bottom">
              <div className="holding-wrapper">
                <div className="holding">
                  {this.props.HoldingCompany.Title}
                </div>
              </div>
              <div className="operating-wrapper">
                <div className="operating">
                  {this.props.OperatingCompany.Title}
                </div>
              </div>
            </div>
          </div>

          <div className="asset-data-1">
            bunch of documents coming soon
          </div>
          <div className="asset-data-2">
            <div className="apns">
              <b>APN #:</b> {this.props.apns}
            </div>
            <div className="buttons">
              <button onClick={this.props.onBackClick}>Back</button>
              <button onClick={this.props.onCreateAssetClick}>Create Asset</button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}
// end claim asset popups

// both
class SuccessPopup extends React.Component {
  render() {
    return (
      <div className="popup success">
        <div className="inner">
          <div className="popup-message-wrapper">
            <div className="notification-image-wrapper">
              check mark!
            </div>
            <div className="header">
              {this.props.header} <span className="success-part">Success!</span>
            </div>
            <div className="message">
              <p>Thank you for completing our Proof of Ownership protocol. USCRE will review your documentation and get back to you withing 3(three) business days. In the meantime, you are welcome to edit your Asset fiel, including adjusting any of teh data fields and adding or removing images and videos as you please . Once your proof of Ownership is approved, you will also be able to publish the Asset to your preferred audience.</p>
            </div>
            <div className="button">
              <button onClick={this.props.onViewAssetClick}>VIEW ASSET</button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}
class ErrorPopup extends React.Component {
  render() {
    return (
      <div className="popup error">
        <div className="inner">
          <div className="popup-message-wrapper">
            <div className="notification-image-wrapper">
              EROR!
            </div>
            <div className="header">
              {this.props.header} <span className="error-part">Error</span>
            </div>
            <div className="message">
              {this.props.message}
            </div>
            <div className="button">
              <button onClick={this.props.onBackClick}>BACK</button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}
class ExamplePopup extends React.Component {
  render() {
    return (
      <div className="example-popup">
        <div className="popup_inner">
          <h1>{this.props.text}</h1>
          <button onClick={this.props.closePopup}>close me</button>
        </div>
      </div>
    )
  }
}
class Home extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      showExamplePopup: false,
      showAddAssetPopup: false
    };
  }
  toggleExamplePopup() { this.setState({ showExamplePopup: !this.state.showExamplePopup }); }
  toggleAddAssetPopup() { this.setState({ showAddAssetPopup: !this.state.showAddAssetPopup }); }
  render() {
    return (
      <div className="classname">
        <p>working!</p>
        <button onClick={this.toggleExamplePopup.bind(this)}>Example Popup</button>
        <button onClick={this.toggleAddAssetPopup.bind(this)}>Add Asset</button>
        {this.state.showExamplePopup ?
          <ExamplePopup
            text="Close Me"
            closePopup={this.toggleExamplePopup.bind(this)}
          />
          : null
        }
        { this.state.showAddAssetPopup ? <AddAssetPopup closePopup={this.toggleAddAssetPopup.bind(this)} /> : null }
      </div>
    )
  }
}
ReactDOM.render(
  <Home />,
  document.getElementById('home')
);
