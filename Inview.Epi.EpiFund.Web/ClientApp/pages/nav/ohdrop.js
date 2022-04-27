import React, { Component } from "react";

class Ohdrop extends Component {
    constructor(props) {
        super(props);
  
        this.state = {
            operating: [
                { 'Operating Company': '' }, { 'Operating Company E-Mail': '' }, { 'Officer First Name': '' }, { 'Officer Last Name': '' }, { 'Address': '' }, { 'Address 2': '' },
                { 'City': '' }, { 'State': '' }, { 'Zip Code': '' }, { 'Country': '' }, { 'Work Number': '' }, { 'Cell Number': '' }, { 'Fax Number': '' }, { 'OperatingCompanyId': '' }
            ],
            holding: [
                { 'Contract Owner': '' }, { 'E-Mail': '' }, { 'Officer First Name': '' }, { 'Officer Last Name': '' }, { 'Address': '' }, { 'Address 2': '' },
                { 'City': '' }, { 'State': '' }, { 'Zip Code': '' }, { 'Country': '' }, { 'Work Number': '' }, { 'Cell Number': '' }, { 'Fax Number': '' }, { 'OperatingCompanyId': '' }, { 'HoldingCompanyId': '' }
            ]
        }
  
        this.sendClose = this.sendClose.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.saveData = this.saveData.bind(this);
        this.updateCompany = this.updateCompany.bind(this);
        this.operatingFromHolding = this.operatingFromHolding.bind(this);
    }
  
    sendClose() {
        this.props.closed();
    }
  
    handleChange(e, itemindex) {
        var data = [];
        if (this.props.type === "operating") {
            data = [...this.state.operating];
        }
  
        if (this.props.type === "holding") {
            data = [...this.state.holding];
        }
  
        data[itemindex] = { [e.target.name]: e.target.value };
  
        this.setState({ [this.props.type]: data });
    }
  
    async saveData() {
        var data = [];
  
        if (this.props.type === "operating") {
            data = [...this.state.operating];
        }
  
        if (this.props.type === "holding") {
            data = [...this.state.holding];
        }
  
        data = this.state[this.props.type];
  
        await this.props.savedata(this.props.type, data);
        await this.sendClose();
    }
  
    updateCompany(e) {
        let selectedindex = e.target.selectedIndex;
        let optionElement = e.target.childNodes[selectedindex];
        let type = optionElement.getAttribute('type');
  
  
        if (type === "operating") {
            let companyObject = api.tempGlobal.operating[e.target.value];
            let newOperatingData = [
                { 'Operating Company': companyObject.CompanyName }, { 'Operating Company E-Mail': companyObject.Email }, { 'Officer First Name': companyObject.FirstName }, { 'Officer Last Name': companyObject.LastName }, { 'Address': companyObject.AddressLine1 }, { 'Address 2': companyObject.AddressLine2 },
                { 'City': companyObject.City }, { 'State': companyObject.State }, { 'Zip Code': companyObject.Zip }, { 'Country': companyObject.Country }, { 'Work Number': companyObject.WorkNumber }, { 'Cell Number': companyObject.CellNumber }, { 'Fax Number': companyObject.FaxNumber }, { 'OperatingCompanyId': companyObject.Id }
            ]
            api.tempGlobal.OperatingCompanyId = companyObject.Id;
            this.setState({ operating: newOperatingData });
            this.props.updateCurrentOperatingData(newOperatingData, companyObject.CompanyName);
        }
  
        if (type === "holding") {
            if (api.tempGlobal.OperatingCompanyId && api.tempGlobal.OperatingCompanyId.length) {
                let holdingObject = api.tempGlobal.holding[e.target.value];

                let newHoldingData = [
                    { 'Contract Owner': holdingObject.CompanyName }, { 'E-Mail': holdingObject.Email }, { 'Officer First Name': holdingObject.FirstName }, { 'Officer Last Name': holdingObject.LastName }, { 'Address': holdingObject.AddressLine1 }, { 'Address 2': holdingObject.AddressLine2 },
                    { 'City': holdingObject.City }, { 'State': holdingObject.State }, { 'Zip Code': holdingObject.Zip }, { 'Country': holdingObject.Country }, { 'Work Number': holdingObject.WorkNumber }, { 'Cell Number': holdingObject.CellNumber }, { 'Fax Number': holdingObject.FaxNumber }, { 'OperatingCompanyId': api.tempGlobal.OperatingCompanyId }, { 'HoldingCompanyId': holdingObject.Id }
                ]
                api.tempGlobal.HoldingCompanyId = holdingObject.Id;
                this.setState({ holding: newHoldingData });
                this.props.updateCurrentHoldingData(newHoldingData, holdingObject.CompanyName);
            } else {
                alert('Please select an operating company.')
            }
        }
    }

    operatingFromHolding() {
        let newOperatingData = [
            { 'Operating Company': String(this.state.operating[0]['Operating Company']) }, { 'Operating Company E-Mail': String(this.props.holdingdata[1]['E-Mail']) }, { 'Officer First Name': String(this.props.holdingdata[2]['Officer First Name']) }, { 'Officer Last Name': String(this.props.holdingdata[3]['Officer Last Name']) }, { 'Address': String(this.props.holdingdata[4]['Address']) }, { 'Address 2': String(this.props.holdingdata[5]['Address 2']) },
            { 'City': String(this.props.holdingdata[6]['City']) }, { 'State': String(this.props.holdingdata[7]['State']) }, { 'Zip Code': String(this.props.holdingdata[8]['Zip Code']) }, { 'Country': String(this.props.holdingdata[9]['Country']) }, { 'Work Number': String(this.props.holdingdata[10]['Work Number']) }, { 'Cell Number': String(this.props.holdingdata[11]['Cell Number']) }, { 'Fax Number': String(this.props.holdingdata[12]['Fax Number']) }
        ]

        this.setState({ operating: newOperatingData });
        this.props.updateCurrentOperatingData(newOperatingData, "test");
    }
  
    componentWillMount() {
        if (api.tempGlobal.operating.length && api.tempGlobal.operating[0].CompanyName !== "New Operating Company") {
  
            api.tempGlobal.operating.unshift(
                {
                    "OperatingCompanyId": "",
                    "CompanyName": "New Operating Company",
                    "FirstName": "",
                    "LastName": "",
                    "Email": "",
                    "AddressLine1": "",
                    "AddressLine2": "",
                    "City": "",
                    "State": "",
                    "Country": "",
                    "Zip": "",
                    "WorkNumber": "",
                    "CellNumber": "",
                    "FaxNumber": ""
                }
            );
  
        }
  
        if (api.tempGlobal.holding.length && api.tempGlobal.holding[0].CompanyName !== "New Holding Company") {
            api.tempGlobal.holding.unshift(
                {
                    "OperatingCompanyId": "",
                    "HoldingCompanyId": "",
                    "CompanyName": "New Holding Company",
                    "FirstName": "",
                    "LastName": "",
                    "Email": "",
                    "AddressLine1": "",
                    "AddressLine2": "",
                    "City": "",
                    "State": "",
                    "Country": "",
                    "Zip": "",
                    "WorkNumber": "",
                    "CellNumber": "",
                    "FaxNumber": ""
                }
            );
  
        }
    }
  
    componentDidMount() {
  
        if (this.props.currentdata.length > 0) {
            this.setState({ [this.props.type]: this.props.currentdata });
        }
    }
  
    render() {
        return (
            <div className="ohdrop">
                <div className="topbar topbar_orange ohdrop_title">
                    {(this.props.type === "operating") ? "Operating Company" : "Holding Company"}
                    <div className="closebutton" onClick={this.sendClose}>×</div>
                </div>
                <div className="ohdropcontent">
                    <select onChange={this.updateCompany}>
                        {
                            (this.props.type === "operating")
  
                                ?
  
                                api.tempGlobal.operating.map((item, index) => {
                                    if (item.CompanyName === this.props.currentoperatingname) {
                                        return <option value={index} type="operating" key={item.OperatingCompanyId} selected>{item.CompanyName}</option>;
                                    }
                                    else {
                                        return <option value={index} type="operating" key={item.OperatingCompanyId}>{item.CompanyName}</option>;
                                    }
                                })
  
                                :
  
                                api.tempGlobal.holding.map((item, index) => {
                                    if (item.CompanyName === this.props.currentholdingname) {
                                        return <option value={index} type="holding" key={item.HoldingCompanyId} selected>{item.CompanyName}</option>;
                                    }
                                    else {
                                        return <option value={index} type="holding" key={item.HoldingCompanyId}>{item.CompanyName}</option>;
                                    }
                                })
                        }
                    </select>
                    <h1>
                        {(this.props.type === "operating") && <div className="submitbtn" style={{ width: "100%", marginLeft: "-2px" }} onClick={this.operatingFromHolding}>Auto-Populate from Holding Company</div>}
                    </h1>
                    <div className="ohdropfieldswrap">
                        {
                            (this.props.type === "operating")
  
                                ?
  
                                this.state.operating.map(item => {
                                    let itemindex = this.state.operating.indexOf(item);
                                    let key = Object.keys(item)[0];
                                    if (itemindex == 13) {
                                        return <input type="text" name={key} value={this.state.operating[itemindex][key]} hidden="hidden" />
                                    } else {
                                        return <div className="ohdropfield" key={key}><div>{key}</div><input type="text" name={key} value={this.state.operating[itemindex][key]} onChange={(e) => this.handleChange(e, itemindex)} /></div>
                                    }
                                })
  
                                :
  
                                this.state.holding.map(item => {
                                    let itemindex = this.state.holding.indexOf(item);
                                    let key = Object.keys(item)[0];
                                    if (itemindex == 13) {
                                        return <input type="text" name={key} value={this.state.holding[itemindex][key]} hidden="hidden" />
                                    } else {
                                        return <div className="ohdropfield" key={key}><div>{key}</div><input type="text" name={key} value={this.state.holding[itemindex][key]} onChange={(e) => this.handleChange(e, itemindex)} /></div>
                                    }
                                })
                        }
                    </div>
                    <div className="modalfooter">
                        <div></div>
                        <div className="buttonwrap">
                            <div className="submitbtn smallbtn hvr-float-shadow" style={{ width: "auto" }} onClick={this.saveData}>Save</div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
  }

  export default Ohdrop;