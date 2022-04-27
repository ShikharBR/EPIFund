import React, { Component } from 'react';

class ICA extends Component {
    render() {
        const directory = [
            {page: "my cache", subpages: [{page: "manage portfolios", link: "#"}], link: "#"},
            {page: "create asset", subpages: [], link: "#"},
            {page: "extract images", subpages: [], link: "#"},
            {page: "accounting", subpages: [], link: "#"}
        ];
        return(
            <>  
                {
                    directory.map(tab => {
                        return <React.Fragment key={tab.page}>
                        <div className="mainbuttonwrap" key={tab.page} onClick={() => this.props.getpage(tab.page, tab.link)}>{tab.page.toUpperCase()}</div>
                        {
                            tab.subpages.map(subtab => {
                                return <div className="subbuttonwrap" key={subtab.page} onClick={() => this.props.getpage(tab.page, subtab.link, subtab.page)}>{subtab.page.toUpperCase()}</div>
                            })
                        }
                        </React.Fragment>
                    })
                }
            </>
        );
    }
}

export default ICA;