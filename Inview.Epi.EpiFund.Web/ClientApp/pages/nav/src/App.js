import React, { Component } from 'react';
import './App.scss';
import PI from './components/pi.js';
import LO from './components/lo.js';
import SP from './components/sp.js';
import ICA from './components/ica.js';

class App extends Component {
  constructor(props) {
    super(props);

    this.state = {
      type: 'PI',
      page: {}
    }

    this.handleType = this.handleType.bind(this);
    this.getPage = this.getPage.bind(this);
  }

  handleType(typename) {
    this.setState({type: typename});
  }

  getPage(pagename, link, submain) {
    console.log("------");
    console.log("Main Page: " + pagename);
    console.log("Link: " + link);
    if (submain) {
      console.log("Subpage: " + submain);
    }
    console.log("------");
    this.setState({page: pagename});
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
    var request = new XMLHttpRequest();
    request.open('get', 'http://localhost:58131/Home/GetLoggedInUserType', true);
    request.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');
    request.send(request.data);
  }

  render() {
    return (
      <div className="App">
      <div className="tempswitch">
        <li onClick={() => this.handleType("LO")}>Logged Out</li>
        <li onClick={() => this.handleType("PI")}>PI - Logged In</li>
        <li onClick={() => this.handleType("SP")}>SP - Logged In</li>
        <li onClick={() => this.handleType("ICA")}>ICA - Logged In</li>
      </div>
        <div className="navwrap">
          {(this.state.type === "PI") && <PI getpage={this.getPage} currentpage={this.state.page}/>}
          {(this.state.type === "LO") && <LO getpage={this.getPage} currentpage={this.state.page}/>}
          {(this.state.type === "SP") && <SP getpage={this.getPage} currentpage={this.state.page}/>}
          {(this.state.type === "ICA") && <ICA getpage={this.getPage} currentpage={this.state.page}/>}
        </div>
      </div>
    );
  }

}

export default App;
