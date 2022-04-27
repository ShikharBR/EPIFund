import React, { Component } from "react"
import ReactDOM from 'react-dom'
import partners from 'APP/pages/nav/partners'

class Affiliations extends React.Component {
  constructor(props) {
    super(props)

    this.state = {
      currentItem: ''
    }

    this.openAffiliateModal = this.openAffiliateModal.bind(this);
    this.closeAffiliateModal = this.closeAffiliateModal.bind(this);
    this.openLink = this.openLink.bind(this);
  }

  openAffiliateModal(name) {
    this.setState({ currentItem: name });
  }

  closeAffiliateModal(event) {
    event.stopPropagation();
    this.setState({ currentItem: null });
  }

  openLink(url) {
    window.open(url, '_blank');
  }

  render() {
    return (
      <>
        <h1 className="affiliations_pagetitle">
          USCRE is proud to be a member of these world-renowned CRE Membership Organizations
                </h1>
        <div className="affiliations_mainwrap">
          {
            partners.map(item => {
              let style;
              if (item.name !== this.state.currentItem) {
                style = {
                  backgroundImage: `url("${item.imglink}")`,
                  backgroundRepeat: 'no-repeat',
                  backgroundPosition: 'center'
                }
              } else {
                style = {
                  backgroundImage: `url("${item.imglink}")`,
                  backgroundRepeat: 'no-repeat',
                  backgroundPosition: 'center',
                  backgroundColor: 'rgba(81,76,70,1)'
                }
              }
              return <div key={item.key} name={item.name} className="affiliations_imagetilewrap" style={style} onMouseEnter={() => this.openAffiliateModal(item.name)}>
                {(item.name === this.state.currentItem) &&
                  <div className="affiliations_modalopen" onMouseLeave={this.closeAffiliateModal}>
                    <div className="affiliations_topbar">
                      <div onClick={this.closeAffiliateModal}>×</div>
                    </div>
                    <div className="affiliations_modal_companyinfo">
                      <h4>{item.name}</h4>
                      <div>Members: {Number(item.memnum).toLocaleString()}</div>
                      {(item.phone) && <div>Phone Number: {item.phone}</div>}
                      <div>{item.address1}</div>
                      <div>{item.address2}</div>
                      <div>{item.city}{(item.state) && `, ${item.state}`} {(item.zip) && item.zip}</div>
                      {(!item.city && item.state) && <div>{item.state}</div>}
                    </div>
                    <div className="affiliations_modal_botbar" onClick={() => this.openLink(item.sitelink)}>VISIT WEBSITE</div>
                  </div>
                }
              </div>
            })
          }
        </div>
      </>
    );
  }

}

const element = document.getElementById('affiliations');
if (element != null)
  ReactDOM.render(<Affiliations />, element);
