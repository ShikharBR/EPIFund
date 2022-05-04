import React, { Component } from "react";
import ReactDOM from 'react-dom';
import ClaimAsset from './claimasset';

let pendingCounts = {
    ICAPendingAssetCount: 5,
    ICAPendingInvoiceCount: 5
};

fetch('/Home/GetICAPendingCounts')
    .then(res => res.json())
    .then(result => {
        pendingCounts = {
            ICAPendingAssetCount: result.ICAPendingAssetCount,
            ICAPendingInvoiceCount: result.ICAPendingInvoiceCount
        };
});

class LO extends Component {
    constructor(props) {
        super(props);

        this.state = {
            directory: [
                { page: "home", subpages: [], link: "/", showsub: false, highlighted: false },
                { page: "cre asset search", subpages: [], link: "/DataPortal/AssetSearchView", showsub: false, highlighted: false },
                { page: "principal investor introduction", subpages: [], link: "/Home/RegistrationIntro", showsub: false, highlighted: false },
                { page: "cre industry service provider introduction", subpages: [], link: "/Home/JointVentureMarketing", showsub: false, highlighted: false },
                { page: "Independent Contractor Employment Opportunity", subpages: [], link: "/Home/EmploymentOpportunities", showsub: false, highlighted: false },
                { page: "uscre: global data portal", subpages: [], link: "/DataPortal/DataPortal", showsub: false, highlighted: false },
                { page: "Global Service Provider Members", subpages: [], link: "/General/UnderConstruction", showsub: false, highlighted: false },
                { page: "Global CRE Membership Organizations", subpages: [], link: "/Home/Affiliations", showsub: false, highlighted: false }
            ],
            currentpage: ""
        }

        this.showSub = this.showSub.bind(this);
    }

    showSub(index) {

        let copyarr = [...this.state.directory];
        copyarr[index].showsub = !copyarr[index].showsub;
        this.setState({
            directory: copyarr
        });
        this.props.getpage(copyarr[index].page, copyarr[index].link);
    }

    componentDidMount() {
        for (let i = 0; i < this.state.directory.length; i++) {
            let copystate = Object.assign({}, this.state);
            if (copystate.directory[i].link === window.location.pathname) {
                copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
            }
            if (this.state.directory[i].subpages.length > 0) {
                this.state.directory[i].subpages.map((item, index) => {
                    if (item.link === window.location.pathname) {
                        copystate.directory[i].subpages[index].highlighted = !copystate.directory[i].subpages[index].highlighted;
                        copystate.directory[i].showsub = !copystate.directory[i].showsub;
                        copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
                    }
                })
            }
            this.setState(copystate);
        }
    }

    render() {
        return (
            <div>
                {
                    this.state.directory.map((tab, index) => {
                        let style = { fontSize: '13px' };
                        if (tab.page.length > 25) {
                            style = { fontSize: '13px' };
                        }
                        if (tab.page.length > 30) {
                            style = { fontSize: '13px', lineHeight: '16px', paddingTop: '16px' };
                        }
                        return <div key={tab.page}>
                            <div className={(tab.highlighted) ? "mainbuttonwrap navselected" : "mainbuttonwrap"} style={style} key={tab.page} onClick={() => this.showSub(index)}>{tab.page.toUpperCase()}</div>
                            {(tab.subpages.length > 0)
                                &&
                                <div className={(this.state.directory[index].showsub) ? "" : "hidden"}>{
                                    tab.subpages.map(subtab => {
                                        return <div className={(subtab.highlighted) ? "subbuttonwrap navselected" : "subbuttonwrap"} key={subtab.page} onClick={() => this.props.getpage(tab.page, subtab.link, subtab.page)}>{subtab.page.toUpperCase()}</div>
                                    })
                                }</div>
                            }
                        </div>
                    })
                }
            </div>
        );
    }
}


class SP extends Component {
    constructor(props) {
        super(props);

        this.state = {
            directory: [
                {
                    page: "Joint Venture Marketing Program", subpages: [
                        { page: "Introduction", link: "/Home/JointVentureMarketing" },
                        { page: "Referral Tracking", link: "/referral/referraltracking" }
                    ], link: "#", showsub: false, highlighted: false
                },
                { page: "communication center", subpages: [], link: "/General/CommunicationCenter", showsub: false, highlighted: false },
                { page: "document center", subpages: [], link: "/General/ManageUploads", showsub: false, highlighted: false },
                { page: "global service provider members", subpages: [], link: "/General/UnderConstruction", showsub: false, highlighted: false },
                { page: "global cre membership organizations", subpages: [], link: "/Home/Affiliations", showsub: false, highlighted: false },
                { page: "accounting", subpages: [{ page: "referral revenue", link: "/Admin/ManageRevenue", highlighted: false }], link: "#", showsub: false, highlighted: false }
            ],
            currentpage: ""
        }

        this.showSub = this.showSub.bind(this);
    }

    showSub(index) {
        let copyarr = [...this.state.directory];
        copyarr[index].showsub = !copyarr[index].showsub;
        this.setState({
            directory: copyarr
        });
        this.props.getpage(copyarr[index].page, copyarr[index].link);
    }


    componentWillMount() {
        fetch('/Home/IsUserInJVMP')
            .then(res => res.json())
            .then(result => {
                if (result.IsInJVMP) {
                    let copyState = Object.assign({}, this.state);

                    copyState.directory[1] = { page: "Referrals", subpages: [{ page: "Monitoring", link: "/referral/referraltracking", highlighted: false }, { page: "Activity", link: "/serviceproviders/PIManagement", highlighted: false }, { page: "Accounting", link: "#", highlighted: false }], link: "#", showsub: false, highlighted: false };
                    this.setState({ directory: copyState.directory });
                }
            });
    }

    componentDidMount() {

        for (let i = 0; i < this.state.directory.length; i++) {
            let copystate = Object.assign({}, this.state);
            if (copystate.directory[i].link === window.location.pathname) {
                copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
            }
            if (this.state.directory[i].subpages.length > 0) {
                this.state.directory[i].subpages.map((item, index) => {
                    if (item.link === window.location.pathname) {
                        copystate.directory[i].subpages[index].highlighted = !copystate.directory[i].subpages[index].highlighted;
                        copystate.directory[i].showsub = !copystate.directory[i].showsub;
                        copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
                    }
                })
            }
            this.setState(copystate);
        }
    }

    render() {
        return (
            <div>
                {
                    this.state.directory.map((tab, index) => {
                        let style = { fontSize: '13px' };
                        if (tab.page.length > 25) {
                            style = { fontSize: '13px' };
                        }
                        if (tab.page.length > 32) {
                            style = { fontSize: '13px', lineHeight: '16px', paddingTop: '16px' };
                        }
                        return <div key={tab.page}>
                            <div className={(tab.highlighted) ? "mainbuttonwrap navselected" : "mainbuttonwrap"} style={style} key={tab.page} onClick={() => this.showSub(index)}>{tab.page.toUpperCase()}</div>
                            {(tab.subpages.length > 0)
                                &&
                                <div className={(this.state.directory[index].showsub) ? "" : "hidden"}>{
                                    tab.subpages.map(subtab => {
                                        return <div className={(subtab.highlighted) ? "subbuttonwrap navselected" : "subbuttonwrap"} key={subtab.page} onClick={() => this.props.getpage(tab.page, subtab.link, subtab.page)}>{subtab.page.toUpperCase()}</div>
                                    })
                                }</div>
                            }
                        </div>
                    })
                }
            </div>
        );
    }
}

class ICA extends Component {
    constructor(props) {
        super(props);

        this.state = {
            directory: [
                { page: "my cache", subpages: [{ page: "current cache", link: "/ICA/ICACache", highlighted: false }, { page: "manage portfolios", link: "/Portfolio/ManagePortfolios", highlighted: false }], link: "#", showsub: false, highlighted: false },
                { page: "create asset", subpages: [], link: "#", showsub: false, highlighted: false },
                { page: "extract images", subpages: [], link: "/Admin/ExtractImagesFromBrochure", showsub: false, highlighted: false },
                { page: "accounting", subpages: [], link: "/Admin/ICAccountingReportDisplay", showsub: false, highlighted: false }
            ],
            currentpage: ""
        }

        this.showSub = this.showSub.bind(this);
    }

    showSub(index) {
        let copyarr = [...this.state.directory];
        copyarr[index].showsub = !copyarr[index].showsub;
        this.setState({
            directory: copyarr
        });
        this.props.getpage(copyarr[index].page, copyarr[index].link);
    }

    componentDidMount() {
        for (let i = 0; i < this.state.directory.length; i++) {
            let copystate = Object.assign({}, this.state);
            if (copystate.directory[i].link === window.location.pathname) {
                copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
            }
            if (this.state.directory[i].subpages.length > 0) {
                this.state.directory[i].subpages.map((item, index) => {
                    if (item.link === window.location.pathname) {
                        copystate.directory[i].subpages[index].highlighted = !copystate.directory[i].subpages[index].highlighted;
                        copystate.directory[i].showsub = !copystate.directory[i].showsub;
                        copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
                    }
                })
            }
            this.setState(copystate);
        }
    }

    render() {
        return (
            <div>
                {
                    this.state.directory.map((tab, index) => {
                        let style = { fontSize: '13px' };
                        if (tab.page.length > 25) {
                            style = { fontSize: '13px' };
                        }
                        if (tab.page.length > 32) {
                            style = { fontSize: '13px', lineHeight: '16px', paddingTop: '16px' };
                        }

                        return <div key={tab.page}>
                            {(tab.page === "create asset") ? <div className={(tab.highlighted) ? "mainbuttonwrap navselected" : "mainbuttonwrap"} data-toggle="modal" data-target="#assetTypeModal" style={style} key={tab.page} >{tab.page.toUpperCase()}</div> : <div className={(tab.highlighted) ? "mainbuttonwrap navselected" : "mainbuttonwrap"} style={style} key={tab.page} onClick={() => this.showSub(index)}>{tab.page.toUpperCase()}</div>}
                            {(tab.subpages.length > 0)
                                &&
                                <div className={(this.state.directory[index].showsub) ? "" : "hidden"}>{
                                    tab.subpages.map(subtab => {
                                        return <div className={(subtab.highlighted) ? "subbuttonwrap navselected" : "subbuttonwrap"} key={subtab.page} onClick={() => this.props.getpage(tab.page, subtab.link, subtab.page)}>{subtab.page.toUpperCase()}</div>
                                    })
                                }</div>
                            }
                        </div>
                    })
                }
            </div>
        );
    }
}

class PI extends Component {
    constructor(props) {
        super(props);

        this.state = {
            directory: [
                { page: "dashboard", subpages: [], link: "/Home/MyUSCPage", showsub: false, highlighted: false },
                { page: "my inventory", subpages: [{ page: "assets", link: "/Investors/SellerManageAssets", highlighted: false }, { page: "portfolio", link: "/Portfolio/ManagePortfolios", highlighted: false }, { page: "create asset", link: "#", highlighted: false }], link: "#", showsub: false, highlighted: false },
                { page: "search", subpages: [{ page: "new search", link: "/DataPortal/AssetSearchView" }, { page: "favorites", link: "/Asset/ManageFavoriteGroups", highlighted: false }, { page: "saved searches", link: "/DataPortal/ManageSavedSearches", highlighted: false }], link: "#", showsub: false, highlighted: false },
                { page: "sales & acquisitions", subpages: [], link: "/Investors/SalesAndAcquisitions", showsub: false, highlighted: false },
                { page: "global cre Service Provider Database", subpages: [{ page: "search sps", link: "/ServiceProviders/SearchServiceProviders", highlighted: false }, { page: "my preferred sps", link: "/ServiceProviders/PreferredServiceProviders", highlighted: false }], link: "#", showsub: false, highlighted: false }
            ],
            currentpage: "",
            createasset: false
        }

        this.showSub = this.showSub.bind(this);
    }

    showSub(index) {
        let copyarr = [...this.state.directory];
        copyarr[index].showsub = !copyarr[index].showsub;
        this.setState({
            directory: copyarr
        });
        this.props.getpage(copyarr[index].page, copyarr[index].link);
    }

    componentDidMount() {
        for (let i = 0; i < this.state.directory.length; i++) {
            let copystate = Object.assign({}, this.state);
            if (copystate.directory[i].link === window.location.pathname) {
                copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
            }
            if (this.state.directory[i].subpages.length > 0) {
                this.state.directory[i].subpages.map((item, index) => {
                    if (item.link === window.location.pathname) {
                        copystate.directory[i].subpages[index].highlighted = !copystate.directory[i].subpages[index].highlighted;
                        copystate.directory[i].showsub = !copystate.directory[i].showsub;
                        copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
                    }
                })
            }
            this.setState(copystate);
        }
    }

    render() {
        return (
            <div>
                {
                    this.state.directory.map((tab, index) => {
                        let style = { fontSize: '13px' };
                        if (tab.page.length > 25) {
                            style = { fontSize: '13px' };
                        }
                        if (tab.page.length > 32) {
                            style = { fontSize: '13px', lineHeight: '16px', paddingTop: '16px' };
                        }
                        return <div key={tab.page}>
                            <div className={(tab.highlighted) ? "mainbuttonwrap navselected" : "mainbuttonwrap"} style={style} key={tab.page} onClick={() => this.showSub(index)}>{tab.page.toUpperCase()}</div>
                            {(tab.subpages.length > 0)
                                &&
                                <div className={(this.state.directory[index].showsub) ? "" : "hidden"}>{
                                    tab.subpages.map(subtab => {
                                        return <div className={(subtab.highlighted) ? "subbuttonwrap navselected" : "subbuttonwrap"} key={subtab.page} onClick={() => this.props.getpage(tab.page, subtab.link, subtab.page)}>{subtab.page.toUpperCase()}</div>
                                    })
                                }</div>
                            }
                        </div>
                    })
                }
            </div>
        );
    }
}

class CA extends Component {
    constructor(props) {
        super(props);

        this.state = {
            directory: [
                { page: "my uscre page", subpages: [], link: "/Home/MyUSCPage", showsub: false, highlighted: false },
                { page: "search", subpages: [{ page: "new search", link: "/DataPortal/AssetSearchView" }, { page: "favorites", link: "/Asset/ManageFavoriteGroups", highlighted: false }, { page: "saved searches", link: "/DataPortal/ManageSavedSearches", highlighted: false }], link: "#", showsub: false, highlighted: false },
                { page: "manage cre master db", subpages: [{ page: "Assets", link: "/Admin/ManageAssets", highlighted: false }, { page: "Portfolios", link: "/Portfolio/ManagePortfolios", highlighted: false }, { page: "Proof of Ownerships", link: "/Admin/ManageProofOfOwnerships", highlighted: false }], link: "#", showsub: false, highlighted: false },
                { page: "Manage PI Master db", subpages: [{ page: "principal investors", link: "/Admin/ManagePrincipalInvestorsReg" }, { page: "Manage Holding Companies", link: "/Admin/ManageHoldingCompanies", highlighted: false }], link: "#", showsub: false, highlighted: false },
                { page: "manage ica master db", subpages: [{ page: "ic admins", link: "/Admin/ManageICAdmins" }, { page: `ICA Pending Assets (${pendingCounts.ICAPendingAssetCount})`, link: "/Admin/ICAPendingAssets", highlighted: false }, { page: `ICA Pending Invoices (${pendingCounts.ICAPendingInvoiceCount})`, link: "/Admin/ICAPendingInvoices", highlighted: false }], link: "#", showsub: false, highlighted: false },
                { page: "manage sp master db", subpages: [{ page: "service providers", link: "#", highlighted: false }], link: "#", showsub: false, highlighted: false },
                { page: "accounting", subpages: [{ page: "manage revenue", link: "/Admin/ManageRevenue", highlighted: false }], link: "#", showsub: false, highlighted: false }
            ],
            currentpage: "",
            createasset: false
        }

        this.showSub = this.showSub.bind(this);
    }

    showSub(index) {
        let copyarr = [...this.state.directory];
        copyarr[index].showsub = !copyarr[index].showsub;
        this.setState({
            directory: copyarr
        });
        this.props.getpage(copyarr[index].page, copyarr[index].link);
    }

    componentDidMount() {
        for (let i = 0; i < this.state.directory.length; i++) {
            let copystate = Object.assign({}, this.state);
            if (copystate.directory[i].link === window.location.pathname) {
                copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
            }
            if (this.state.directory[i].subpages.length > 0) {
                this.state.directory[i].subpages.map((item, index) => {
                    if (item.link === window.location.pathname) {
                        copystate.directory[i].subpages[index].highlighted = !copystate.directory[i].subpages[index].highlighted;
                        copystate.directory[i].showsub = !copystate.directory[i].showsub;
                        copystate.directory[i].highlighted = !copystate.directory[i].highlighted;
                    }
                })
            }
            this.setState(copystate);
        }
    }

    render() {
        return (
            <div>
                {
                    this.state.directory.map((tab, index) => {
                        let style = { fontSize: '13px' };
                        if (tab.page.length > 25) {
                            style = { fontSize: '13px' };
                        }
                        if (tab.page.length > 32) {
                            style = { fontSize: '13px', lineHeight: '16px', paddingTop: '16px' };
                        }
                        return <div key={tab.page}>
                            <div className={(tab.highlighted) ? "mainbuttonwrap navselected" : "mainbuttonwrap"} style={style} key={tab.page} onClick={() => this.showSub(index)}>{tab.page.toUpperCase()}</div>
                            {(tab.subpages.length > 0)
                                &&
                                <div className={(this.state.directory[index].showsub) ? "" : "hidden"}>{
                                    tab.subpages.map(subtab => {
                                        return <div className={(subtab.highlighted) ? "subbuttonwrap navselected" : "subbuttonwrap"} key={subtab.page} onClick={() => this.props.getpage(tab.page, subtab.link, subtab.page)}>{subtab.page.toUpperCase()}</div>
                                    })
                                }</div>
                            }
                        </div>
                    })
                }
            </div>
        );
    }
}

class App extends Component {
    constructor(props) {
        super(props);

        this.state = {
            type: 'ICAdmin',
            page: {},
            showPopup: false
        }

        this.getPage = this.getPage.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    handleClose() {
        this.setState({ showPopup: false });
    }

    getPage(pagename, link, submain) {

        if (submain === "create asset" || pagename === "create asset") {
            this.setState({ showPopup: !this.state.showpopup });
        }
        if (link !== "#") {
            document.location.href = link;
        }
        this.setState({ page: pagename });
    }

    componentDidMount() {
        //need to make api call here, need page type and current page so i can pass it down. promise and switch should do
        // fetch('http://localhost:58131/Home', {
        //   method: 'POST',
        //   headers: {
        //     'Accept': 'application/json',
        //     'Content-Type': 'application/json',
        //   },
        //   body: JSON.stringify({
        //     firstParam: 'yourValue',
        //     secondParam: 'yourOtherValue',
        //   })
        // })
        fetch('/Home/GetICAPendingCounts')
            .then(res => res.json())
            .then(result => {
                pendingCounts = {
                    ICAPendingAssetCount: result.ICAPendingAssetCount,
                    ICAPendingInvoiceCount: result.ICAPendingInvoiceCount
                };
            });

        fetch('/Home/GetLoggedInUserType')
            .then(res => res.json())
            .then(result => {
                this.setState({
                    type: result.UserType
                })
            })
    }

    render() {
        return (
            <div className="App">
                <div className="navwrap">
                    {(this.state.type === "Investor") && <PI getpage={this.getPage} currentpage={this.state.page} />}
                    {(this.state.type === "") && <LO getpage={this.getPage} currentpage={this.state.page} />}
                    {(this.state.type === "ServiceProvider") && <SP getpage={this.getPage} currentpage={this.state.page} />}
                    {(this.state.type === "ICAdmin") && <ICA getpage={this.getPage} currentpage={this.state.page} />}
                    {(this.state.type === "CorpAdmin") && <CA getpage={this.getPage} currentpage={this.state.page} />}
                </div>
                {this.state.showPopup && <ClaimAsset closepop={this.handleClose} />}
            </div>
        );
    }

}


fetch('/Home/GetLoggedInUserType')
    .then(res => res.json())
    .then(result => {
        if (result.UserType === "CorpAdmin" || result.UserType === "" || result.UserType === "Investor" || result.UserType === "ServiceProvider" || result.UserType === "ICAdmin") {
            ReactDOM.render(<App />, document.getElementById('nav'));
        }
    })


class HeresYourButton extends Component {
    constructor(props) {
        super(props);

        this.state = {
            showPop: false
        }

        this.showClaimPop = this.showClaimPop.bind(this);
        this.handleClose = this.showClaimPop.bind(this);
    }

    showClaimPop() {
        this.setState({ showPop: !this.state.showPop });
    }

    handleClose() {

        this.setState({ showPop: false });
    }

    render() {

        return (

            <React.Fragment>
                <button id="createAssetButton" className="btn btn-primary" onClick={this.showClaimPop} >
                    Create New Asset
                </button>

                {this.state.showPop && <ClaimAsset closepop={this.handleClose} />}
            </React.Fragment>
        );
    }
}

const element = document.getElementById('dummyID');
if (element != null) {
    ReactDOM.render(<HeresYourButton />, element);
}
