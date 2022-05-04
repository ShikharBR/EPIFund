import React, { Component } from "react";


// This is calling an API obj located on _Layout/_LayoutAdmin
api.getICAPendingCounts();

class Addasset extends Component {

    constructor(props) {
        super(props);
  
        this.state = {
            assetname: "",
            address: "",
            city: "",
            State: "",
            county: "",
            type: null,
            apns: [{ value: "" }],
            note: false,
            claimasset: false,
            apnresults: ['123', '234'],
            timeoutId: null
        }
  
        this.handleInput = this.handleInput.bind(this);
        this.addApn = this.addApn.bind(this);
        this.delApn = this.delApn.bind(this);
        this.updateApn = this.updateApn.bind(this);
        this.submit = this.submit.bind(this);
        this.handleChecked = this.handleChecked.bind(this);
        this.search = this.search.bind(this);
    }
  
    handleInput(e) {
        this.setState({
            [e.target.name]: e.target.value
        });
        console.log(`<handleInput> ${e.target.name}`, e.target.value);
        this.search();
    }
  
    search() {
        /*
        4/25/19
        Talked to Aaron.
        Only trigger a search when the county, state and an apn or 10 is/are provided.
        If it matches, return the asset properties and update the popup with asset vals.
        If it doesnt match, I still dont know
      */

        if (this.state.timeoutId) {
            clearTimeout(this.state.timeoutId);
            this.state.timeoutId = null;
        }
        this.state.timeoutId = setTimeout(() => {
            let requestData = {};
            if (this.state.State && this.state.county) {
                requestData.State = this.state.State;
                requestData.County = this.state.county;
                let apns = [];
                for (let i = 0; i < 10; i++) {
                    let el = document.getElementById(`${i}-apn`);
                    if (el) {
                        let apn = el.value;
                        if (apn.length && apn.trim().length) {
                            apns.push({ Key: i, Value: apn.trim().toLowerCase() })
                        }
                    } else { break; }
                }
                if (!apns.length) return;

                requestData.Apns = apns;
                // trigger search
                api.searchAssets(requestData, (err, res) => {
                    if (err) throw err;
                    if (res.Assets.length) {
                        if (res.Assets.length === 1) {
                            // check if this user has an active claim on this asset in progress
                            console.log('assetId: ', res.Assets[0].AssetId)
                            api.hasUserAlreadyClaimedAsset({ assetId: res.Assets[0].AssetId }, (err1, res1) => {
                                if (err1) throw err1;
                                if (res1.isAlreadyClaimed) {
                                    alert('You are already claiming this asset');
                                } else {
                                    api.tempGlobal.assets = [res.Assets[0]];
                                    this.setState({
                                        assetname: res.Assets[0].Name,
                                        city: res.Assets[0].City,
                                        State: res.Assets[0].State,
                                        county: res.Assets[0].County,
                                        address: res.Assets[0].Address1,
                                        type: parseInt(res.Assets[0].AssetType),
                                        note: res.Assets[0].IsNote,
                                        claimasset: true,
                                    });
                                    this.submit("claim");
                                }
                            });
                        } else {
                            // TODO: get requirements for this path
                            api.tempGlobal.assets = res.Assets;
                            console.log('multiple assets found. no idea what to do')
                        }
                    } else {
                        // create new path
                        this.setState({ claimasset: false });
                    }
                });
            }
        }, 1000);
    }
  
    handleChecked(e) {
        this.setState({
            [e.target.name]: !this.state[e.target.name]
        })
    }
  
    addApn() {
        let apnobj = { value: "" };
        let stateapns = this.state.apns;
        stateapns.push(apnobj);
        this.setState({ apns: stateapns });
    }
  
    delApn(index) {
        let stateapns = this.state.apns;
        delete stateapns[index];
        this.setState({ apns: stateapns });
    }
  
    updateApn(e) {
        let index = e.target.name;
        let stateapns = this.state.apns;
        stateapns[index].value = e.target.value;
        console.log('state apns', stateapns);
        this.setState({ apns: stateapns });
        this.search();
    }
  
    submit(type) {
        var submittype = 0;
        if (type === "add") {
            submittype = 1;
            api.tempGlobal.newAsset = { // delete this, its already obsolete(I think)
                assetName: this.state.assetname,
                address: this.state.address,
                city: this.state.city,
                state: this.state.State,
                type: this.state.type,
                apn: this.state.apns,
                isNote: this.state.note,
            };
            api.tempGlobal.claimedAsset = null;
            if (this.state.type) { // feels wrong and hackish
                this.props.addinfo({ addasset: this.state }, submittype);
            }
        }
        else {
            submittype = 2;
            api.tempGlobal.newAsset = null;
            if (api.tempGlobal.assets && api.tempGlobal.assets.length) {
                api.tempGlobal.claimedAsset = api.tempGlobal.assets[0]; // delete this once below is proven
                this.props.addinfo({ asset: api.tempGlobal.assets[0] }, submittype);
                if (api.tempGlobal.assets.length > 1) console.log('****IDK HOW MULTI ASSET CLAIM IS SUPPOSED TO WORK*****')
            } else {
                alert('nothing to claim')
            }
        }
  
    }
  
    componentDidMount() {
        let dataobj = Object.assign({}, this.props.data.addasset);
        this.setState(dataobj);
        // fetch user companies here? sure why not, use them for both claim and add paths
        api.getUserCompanies(function (err, result) {
            if (err) throw err;
            else {
                api.tempGlobal.holding = result.holdingCompanies;
                api.tempGlobal.operating = result.operatingCompanies;
            }
        });
    }
  
    render() {
  
        const stateAbbreviations = [
            '', 'AL', 'AK', 'AS', 'AZ', 'AR', 'CA', 'CO', 'CT', 'DE', 'DC', 'FM', 'FL', 'GA',
            'GU', 'HI', 'ID', 'IL', 'IN', 'IA', 'KS', 'KY', 'LA', 'ME', 'MH', 'MD', 'MA',
            'MI', 'MN', 'MS', 'MO', 'MT', 'NE', 'NV', 'NH', 'NJ', 'NM', 'NY', 'NC', 'ND',
            'MP', 'OH', 'OK', 'OR', 'PW', 'PA', 'PR', 'RI', 'SC', 'SD', 'TN', 'TX', 'UT',
            'VT', 'VI', 'VA', 'WA', 'WV', 'WI', 'WY'
        ];
  
        let assetTypes = [
            { key: null, val: null }, { key: 1, val: 'Retail Tenant Property' }, { key: 2, val: 'Office Tenant Property' }, { key: 3, val: 'Multi-Family' }, { key: 4, val: 'Industrial Tenant Property' }, { key: 5, val: 'MHP' },
            { key: 6, val: 'Fuel Service Retail Property' }, { key: 7, val: 'Medical Tenant Property' }, { key: 8, val: 'Mixed Use Commercial Property' }, { key: 13, val: 'Fractured Condominium Portfolios' },
            { key: 14, val: 'Mini-Storage Property' }, { key: 15, val: 'Parking Garage Property' }, { key: 16, val: 'Secured Private Notes' }
        ];
        return (
            <div className="modalwrap modal_addasset animated slideInRight faster">
                <div className="topbar topbar_blue">
                    Add Asset
                <div className="closebutton" onClick={() => this.props.close()}>×</div>
                </div>
                <div className="modal_contentwrap">
  
                    <div className="inputwrap">
                        <div className="inputtitle">Asset Name</div>
                        <input type="text" placeholder="Asset Name" value={this.state.assetname} onChange={this.handleInput} name="assetname" id="caAssetName" />
                    </div>
  
                    <div className="inputwrap">
                        <div className="inputtitle">Address</div>
                        <input type="text" placeholder="Address" value={this.state.address} onChange={this.handleInput} name="address" id="caAddress" />
                    </div>
  
                    <div className="inputwrap flexdir_row">
                        <div className="inputmid">
                            <div className="inputtitle">City</div>
                            <input type="text" placeholder="City" value={this.state.city} onChange={this.handleInput} name="city" id="caCity" />
                        </div>
                        <div className="inputsmall">
                            <div className="inputtitle">State</div>
                            <select className="inputstyle" onChange={this.handleInput} name="State" id="caState">
                                {
                                    stateAbbreviations.map(item => {
                                        if (this.state.State === item) {
                                            return <option value={item} key={item} selected>{item}</option>;
                                        }
                                        else {
                                            return <option value={item} key={item}>{item}</option>
                                        }
                                    })
                                }
                            </select>
                        </div>
                        <div className="inputmid">
                            <div className="inputtitle">County</div>
                            <input type="text" placeholder="County" value={this.state.county} onChange={this.handleInput} name="county" id="caCounty" />
                        </div>
                    </div>
  
                    <div className="apnwrap">
                        <div className="apntop">
                            <h3>Assessor’s Parcel # (APNs)</h3>
                            <div className="bluebtn" onClick={this.addApn}>+</div>
                        </div>
                        {
                            this.state.apns.map((item, index) => {
                                let id = `${index}-apn`;
                                return <div className="apnitemwrap" key={index}>
                                    <input /*This is red style to indicate error for input --- className="inputred"*/ type="text" placeholder="Apn #" value={this.state.apns[index].value} onChange={this.updateApn} name={index} id={id} />
                                    <div className="delbtn" onClick={() => this.delApn(index)}>×</div>
                                </div>
                            })
                        }
                        {/* Here's the error message, need logic ---- <p>Error: This APN is currently registered to another property in our system. If you believe this to be an error, please contact us at admin@uscreonline.com</p> <p></p>*/}
                    </div>
                    <div className="inputwrap">
                        <div className="inputtitle">Asset Type</div>
  
                        <select className="inputstyle maxwidth select40" name="type" onChange={this.handleInput}>
                            {
                                assetTypes.map(item => {
                                    if (!item.val) {
                                        return <option value="" key="0" selected></option>;
                                    }
                                    else {
                                        if (this.state.type === item.key) {
                                            return <option value={item.key} key={item.key} selected>{item.val}</option>;
                                        }
                                        else {
                                            return <option value={item.key} key={item.key}>{item.val}</option>
                                        }
                                    }
                                })
                            }
                        </select>
                    </div>
                    <div className="modalfooter">
                        <div className="footerextra">
                            <input type="checkbox" name="note" onChange={this.handleChecked} checked={this.state.note} />
                            <div>Note Asset</div>
                        </div>
                        <div className="buttonwrap">
                            {!this.state.claimasset && <div className="submitbtn hvr-float-shadow" onClick={() => this.submit("add")}>Add Asset</div>}
                            {this.state.claimasset && <div className="submitbtn submitorange hvr-float-shadow" onClick={() => this.submit("claim")}>Claim Asset</div>}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
  }

export default Addasset;
