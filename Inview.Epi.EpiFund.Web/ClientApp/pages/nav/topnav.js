import React, { Component } from "react"
import ReactDOM from 'react-dom';

// Top Navigation
class TopNav extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            user: null
        }
    }

    componentDidMount() {
        fetch('/api/userInfo/GetCurrentUserInfoAndProfile', {
            method: 'post',
            body: JSON.stringify()
        }).then(response => {
            return response.json();
        }).then(data => {
            if (!data.Message) {
                this.setState({ user: data });
            }
            else {
                this.setState({ user: null });
            }
        });
    }

    render() {
        return (
            <div className="topnavwrap">
                {this.state.user ? <TopNavLoggedIn user={this.state.user} /> : <TopNavLoggedOut />}
            </div>
        );
    }
}

class TopNavLoggedIn extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            showPop: false,
            showPop2: false
        }

        this.togglePop = this.togglePop.bind(this);
        this.togglePop2 = this.togglePop2.bind(this);
        this.clickPop = this.clickPop.bind(this);
        this.clickItem = this.clickItem.bind(this);
        this.handleClickOutside = this.handleClickOutside.bind(this);
    }

    togglePop() {
        this.setState({ showPop: !this.state.showPop });
    }

    togglePop2() {
        this.setState({ showPop2: !this.state.showPop2 });
    }

    componentWillMount() {
        document.addEventListener('click', this.clickPop, false);
    }

    componentWillUnmount() {
        document.removeEventListener('click', this.clickPop, false);
    }

    handleClickOutside() {
        this.setState({ showPop: false, showPop2: false });
    }

    clickPop(e, data) {
        if (data) {
            document.location.href = data.link;
        }

        if (this.pop1.contains(e.target)) {
            this.togglePop();
            return;
        }

        if (this.pop2.contains(e.target)) {
            this.togglePop2();
            return;
        }

        this.handleClickOutside();
    }

    clickItem(data) {
        document.location.href = data.link;
    }

    render() {
        return (
            <React.Fragment>
                <div className="userinfotopwrap" ref={node => this.pop1 = node}>
                    <div style={{ height: "20px", fontSize: "54px", lineHeight: "38px" }}>
                        <img alt="" src="https://s3-us-west-1.amazonaws.com/dev-uscreonline-content/arrowdown.png" style={{ marginTop: "5px" }} />
                    </div>
                    <div>
                        {
                            this.props.user != null &&
                            <img alt="" src={this.props.user.ProfileImageUrl} />
                        }
                    </div>
                    <div>
                        Welcome

                        {
                            this.props.user != null &&
                            ", " + this.props.user.User.FullName
                        }
                    </div>

                    {
                        this.state.showPop &&
                        <div className="minipopdown">
                            <div className="minipopitem" onMouseDown={(e) => this.clickPop(e, { link: '/Home/EditProfile' })}>Edit Profile</div>
                            {
                                (this.props.user != null && this.props.user.User.UserType === 14) &&
                                <div className="minipopitem" onMouseDown={(e) => this.clickPop(e, { link: '/General/UserProfile' })}>Edit SP Profile</div>
                            }
                            <div className="minipopitem" onMouseDown={(e) => this.clickPop(e, { link: '/Home/ChangePassword' })}>Account Settings</div>
                            <div className="minipopitem" onMouseDown={(e) => this.clickPop(e, { link: '/Home/RegistrantRecords' })}>Registrant Records</div>
                        </div>
                    }
                </div>
                <div className="topnavbuttonwrap" ref={this.setWrapperRef}>
                    <img alt="" src="https://i.imgur.com/bVEpvrv.png" ref={node => this.pop2 = node} />
                    {
                        this.state.showPop2 &&
                        <div className="minipopdown">
                            <div className="minipopitem" onMouseDown={(e) => this.clickPop(e, { link: '/General/CommunicationCenter' })}>Communication Center</div>
                            <div className="minipopitem" onMouseDown={(e) => this.clickPop(e, { link: '/General/UserContacts' })}>My Contacts</div>
                        </div>
                    }
                    <div className="topnavbutton" onClick={() => this.clickItem({ link: '/Home/Logout' })}>Logout</div>
                </div>
            </React.Fragment>
        );
    }
}

class TopNavLoggedOut extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            user: {},
            showPop: false
        }

        this.openPage = this.openPage.bind(this);
        this.togglePop = this.togglePop.bind(this);
        this.clickItem = this.clickItem.bind(this);
    }

    openPage(link) {
        document.location.href = link;
    }

    togglePop() {
        this.setState({ showPop: !this.state.showPop });
    }

    clickItem(data) {
        if (data.link) {
            document.location.href = data.link;
        }
    }

    render() {
        return (
            <React.Fragment>
                <div>Please Login or Register</div>
                <div>
                    <div className="topnavbutton" onClick={() => this.openPage('/Home/Login')}>Login</div>
                    <div className="topnavbutton" onMouseEnter={this.togglePop} onMouseLeave={this.togglePop}>
                        Register
            {
                            this.state.showPop &&
                            <div className="minipopdown" onMouseDown={this.clickItem}>
                                <div className="minipopitem" onClick={() => this.clickItem({ link: '/Home/RegistrationIntro' })}>Principal Investor</div>
                                <div className="minipopitem" onClick={() => this.clickItem({ link: '/Home/JointVentureMarketing' })}>Service Provider</div>
                                <div className="minipopitem" onClick={() => this.clickItem({ link: '/Home/EmploymentOpportunities' })}>Independent Contractor</div>
                            </div>
                        }
                    </div>
                </div>
            </React.Fragment>
        );
    }
}


ReactDOM.render(<TopNav />, document.getElementById('topnav'));