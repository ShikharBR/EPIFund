import React, { Component } from "react";
import Ohdrop from './ohdrop';

class Add1 extends Component {
    constructor(props) {
        super(props);
  
        this.state = {
            show: 0,
            type: "",
            aquisition: "",
            purchaseprice: "",
            terms: "",
            operating: [],
            holding: [],
            currentoperating: "Operating Company",
            currentholding: "Holding Company"
        }
  
        this.back = this.back.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.showDrop = this.showDrop.bind(this);
        this.hideDrop = this.hideDrop.bind(this);
        this.handleInput = this.handleInput.bind(this);
        this.nextStep = this.nextStep.bind(this);
        this.saveCompanyData = this.saveCompanyData.bind(this);
        this.updateCurrentOperatingData = this.updateCurrentOperatingData.bind(this);
        this.updateCurrentHoldingData = this.updateCurrentHoldingData.bind(this);
  
        this.dateInput = React.createRef();
    }
  
    async nextStep() {
        let dateString = this.dateInput.current.value.toString();
        await this.setState({ aquisition: dateString });
        api.validateThatOnePopupWithCompanyInfoAndStuff(this.state, api.tempGlobal.newAsset, (success) => {
            if (success) {
                this.props.gonext(this.state, "add1");
            }
        });
    }
  
    handleInput(e) {
        let evalue = e.target.value;
        if (e.target.name === "purchaseprice") {
            evalue = evalue.replace(/,/g, '');
            evalue = evalue.replace(/\$/g, '');
            evalue = "$" + Number(evalue).toLocaleString();
        }

        if (e.target.name === "terms") {
            this.setState({ terms: e.target[e.target.selectedIndex].id });
        }
        else {
            this.setState({ [e.target.name]: evalue });
        }
    }
  
    handleClose() {
        this.props.close();
    }
  
    async back() {
        await this.setState({ aquisition: this.dateInput.current.value });
        await this.props.goback();
    }
  
    showDrop(type) {
        if (this.state.show) {
            this.setState({ show: 0, type: "" });
        }
        else {
            this.setState({ show: 1, type: type });
        }
    }
  
    hideDrop(e) {
        this.setState({ show: 0, type: "" });
    }
  
    componentDidMount() {
        let dataobj = Object.assign({}, this.props.data);
        if (this.props.data['aquisition'] && this.props.data['aquisition'] !== "undefined") {
            this.dateInput.current.value = this.props.data['aquisition'];
        }
        this.setState(dataobj);
        $('.popup-aq-date').datepicker({
          maxDate: new Date()
        });
    }

    saveCompanyData(type, data) {
        var updateState = false;
        let currentname = data[0][Object.keys(data[0])];
        let current = "current" + this.state.type;
        if (!currentname) {
            alert('Company name required.');
        } else {
            const companyType = type === 'holding' ? 0 : 1;
            const payload = {
                CompanyName: currentname,
                Email: data[1][Object.keys(data[1])],
                FirstName: data[2][Object.keys(data[2])],
                LastName: data[3][Object.keys(data[3])],
                AddressLine1: data[4][Object.keys(data[4])],
                AddressLine2: data[5][Object.keys(data[5])],
                City: data[6][Object.keys(data[6])],
                State: data[7][Object.keys(data[7])],
                Zip: data[8][Object.keys(data[8])],
                Country: data[9][Object.keys(data[9])],
                WorkNumber: data[10][Object.keys(data[10])],
                CellNumber: data[11][Object.keys(data[11])],
                FaxNumber: data[12][Object.keys(data[12])],
                Type: companyType
            };
            var updateCompany;
            if (type === 'operating') {
                if (data[13][Object.keys(data[13])]) {
                    // update operating
                    payload.OperatingCompanyId = data[13][Object.keys(data[13])];
                    updateCompany = true;
                } else {
                    // create operating
                    updateCompany = false;
                }
                this.setState({ [type]: data, [current]: currentname });
            }
            if (type === 'holding') {
                if (api.tempGlobal.OperatingCompanyId) {
                    payload.OperatingCompanyId = api.tempGlobal.OperatingCompanyId;
                    if (data[14][Object.keys(data[14])]) {
                        // update holding
                        payload.HoldingCompanyId = data[14][Object.keys(data[14])];
                        updateCompany = true;
                    } else {
                        // create holding
                        updateCompany = false;
                    }
                    updateState = true;
                } else {
                    alert('Please select an operating company.');
                }
            }
            if (updateCompany === true) {
                api.updateUserCompany(payload, function (err, result) {
                    if (err) throw err;
                    else {
                        alert('Company updated.')
                        updateState = true;
                    }
                });
            }
            if (updateCompany === false) {
                // new company, check for duplicates, create
                api.doesUserCompanyExist({
                    CompanyName: currentname,
                    Type: companyType,
                }, function (err, result) {
                    if (err) throw err;
                    else {
                        if (result.alreadyExists === true) {
                            alert('Company currently exists.');
                        } else {
                            api.createUserCompany(payload, function (err1, res) {
                                if (err1) throw err1;
                                else {
                                    if (res.Success === true) {
                                        // set both for now and see if fetching from data works, fall back to global if need be(like I always do)
                                        if (type === 'holding') {
                                            data[14].HoldingCompanyId = res.id;
                                            api.tempGlobal.HoldingCompanyId = res.id;
                                        } else if (type === 'operating') {
                                            data[13].OperatingCompanyId = res.id;
                                            api.tempGlobal.OperatingCompanyId = res.id;
                                        }
                                        alert('Successfully Created Company');
                                        updateState = true;
                                    } else {
                                        alert('Error creating company, please contact support for assistance.');
                                    }
                                    
                                }
                            })
                        }
                    }
                });
            }
        }
        if (updateState) this.setState({ [type]: data, [current]: currentname });
    }
  
    updateCurrentOperatingData(data, name) {
        api.tempGlobal.OperatingCompanyId = data[13][Object.keys(data[13])];
        this.setState({ operating: data, currentoperating: name });
    }
  
    updateCurrentHoldingData(data, name) {
        this.setState({ holding: data, currentholding: name });
    }
  
    render() {
        return (
            <div className="modalwrap modal_big animated slideInRight faster">
                <div className="topbar topbar_orange">
                    Add Asset <a>- Step 1</a>
                    <div className="closebutton" onClick={() => this.props.close()}>×</div>
                </div>
                <div className="modal_contentwrap bigpad">
                    <h1>Please select your Operating Company and Holding Company</h1>
                    <div className="pad40"></div>
                    <div className="dualwrap">
                        <div className="width48">
                            <div className="orangeselect" onClick={() => this.showDrop("operating")}>
                                <div>{(this.state.currentoperating.length < 25) ? this.state.currentoperating : (this.state.currentoperating.substr(0, 22) + "...")}</div><div><img src="https://s3-us-west-1.amazonaws.com/dev-uscreonline-content/toggleicons/downarrow.png" /></div>
                            </div>
                            {(this.state.type === "operating") && <Ohdrop type="operating" savedata={this.saveCompanyData} closed={this.hideDrop} updateCurrentOperatingData={this.updateCurrentOperatingData} currentdata={this.state.operating} currentoperatingname={this.state.currentoperating} holdingdata={this.state.holding} />}
                        </div>
                        <div className="width48">
                            <div className="orangeselect" onClick={() => this.showDrop("holding")}>
                                <div>{(this.state.currentholding.length < 25) ? this.state.currentholding : (this.state.currentholding.substr(0, 22) + "...")}</div><div><img src="https://s3-us-west-1.amazonaws.com/dev-uscreonline-content/toggleicons/downarrow.png" /></div>
                            </div>
                            {(this.state.type === "holding") && <Ohdrop type="holding" savedata={this.saveCompanyData} closed={this.hideDrop} updateCurrentHoldingData={this.updateCurrentHoldingData} currentdata={this.state.holding} currentholdingname={this.state.currentholding} />}
                        </div>
                    </div>
  
                    <div className="dualwrap">
                        <div className="width48">
                            <div className="orangetitle">Original Aquisition Date</div>
                            <input className="orangeselect popup-aq-date" name="aquisition" autocomplete="off" ref={this.dateInput}/>
                        </div>
                        <div className="width48">
                            <div className="orangetitle">Original Purchase Price</div>
                            <input className="orangeselect" type="text" value={this.state.purchaseprice} autocomplete="off" onChange={this.handleInput} name="purchaseprice" />
                        </div>
                    </div>
  
                    <div className="width100">
                        <div className="orangetitle">Terms</div>
                        <select className="orangeselect width100" onChange={this.handleInput} name="terms">
                            {
                                api.tempGlobal.sellerTerms.map(item => {
                                    if (item.val === this.state.terms) {
                                        return <option id={item.id} key={item.id} name={item.id} selected>{item.val}</option>
                                    }
                                    else {
                                        return <option id={item.id} key={item.id} name={item.id}>{item.val}</option>
                                    }
                                })
                            }
                        </select>
                    </div>
  
                    <div className="modalfooter">
                        <div></div>
                        <div className="buttonwrap">
                            <div className="submitbtn submitorange top20 hvr-float-shadow" onClick={this.back}>Back</div>
                            <div className="submitbtn top20 hvr-float-shadow" onClick={this.nextStep}>Next</div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
  }

export default Add1;