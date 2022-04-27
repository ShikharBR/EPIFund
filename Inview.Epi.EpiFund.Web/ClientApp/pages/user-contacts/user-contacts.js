import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');


import { epiService } from 'APP/services/epiService'
import { LoadingComponent } from "APP/loading-component";
import { QuickViewComponent } from '../general/general-quick-view'

class UserContactComponent extends Component {
  constructor(props) {
    super(props);
    const self = this;

    this.state = {
      isLoading: true,
      currentUserProfile: { Contacts: [] },
      searchTerm: "",
      selectedContact: null
    };

    self.searchContact = self.searchContact.bind(this);
    self.refresh = self.refresh.bind(this);
    self.quickViewUser = self.quickViewUser.bind(this);

    self.refresh();
  }

  async refresh() {

    const profile = await epiService.getCurrentUserInfoAndProfile(true);
    profile.Contacts.map(x => { x.Hidden = false; });
    this.setState({
      currentUserProfile: profile,
      selectedContact: null,
      isLoading: false
    }, () => {
      //bootstrap tooltip
      setTimeout(function () { $('[data-toggle="tooltip"]').tooltip(); }, 500);
    });
  }

  searchContact(e) {
    const self = this;
    this.setState({ searchTerm: e.target.value },
      () => {
        const profile = this.state.currentUserProfile;
        for (var i = 0; i < profile.Contacts.length; i++) {
          var c = profile.Contacts[i];
          if (c.FullName.toLowerCase().indexOf(self.state.searchTerm.toLowerCase()) > -1 || self.state.searchTerm.trim() == "") {
            c.Hidden = false;
          } else {
            c.Hidden = true;
          }
        }
        this.setState({
          currentUserProfile: profile
        });
      });
  }

  quickViewUser(user) {

    this.setState({ selectedContact: user });
  }

  render() {

    return (
      <React.Fragment>
        {
          this.state.isLoading ? <LoadingComponent /> :

            <React.Fragment>
              <div>
                <div className="row">
                  <div className="col-sm-12">
                    <h3>
                      My Contacts
                                            <button onClick={() => { PubSub.publish("newMessageToCropAdmin"); }} className="btn btn-primary pull-right"><i className="fa fa-envelope"></i> Contact Corp Admin</button>
                    </h3>
                  </div>
                </div>

                <div className="panel contacts-panel">
                  <div className="panel-body">
                    <div className="row wrapper">
                      <div className="col-sm-5 contacts-list">
                        <div className="list-wrapper">
                          <div className="form-group">
                            <h4>Quick search:</h4><br />
                            <input className="form-control" type="text" placeholder="Search" value={this.searchTerm} onChange={this.searchContact} />
                          </div>
                          <hr />
                          <ul>
                            {
                              this.state.currentUserProfile.Contacts.map(x => {
                                if (x.Hidden === false)
                                  return (
                                    <li key={x.UserId} className="" onClick={() => { this.quickViewUser(x) }}>
                                      <div className="col-xs-12 col-sm-4">
                                        <img src={x.ProfileImageUrl} className="img-responsive img-rounded profile-image" />
                                      </div>
                                      <div className="col-xs-12 col-sm-8">
                                        <span className="name">{x.FullName}</span>
                                        <hr />
                                        <span className="email" title={x.Username} data-toggle="tooltip" data-placement="top"><i className="fas fa-envelope"></i></span>
                                      </div>
                                      <div className="clearfix"></div>
                                    </li>
                                  );
                              })
                            }

                          </ul>
                        </div>
                      </div>

                      <div className="col-sm-7 contact-details">
                        {
                          this.state.selectedContact != null &&
                          <QuickViewComponent refreshList={this.refresh} selectedUserProfile={this.state.selectedContact} />
                        }
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </React.Fragment>
        }
      </React.Fragment>
    )
  }
}

export default UserContactComponent

const element = document.getElementById("user-contacts-app");
if (element != null)
  ReactDOM.render(<UserContactComponent />, element);
