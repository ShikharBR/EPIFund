import React, { Component } from 'react';

class LO extends Component {
    render() {
        const directory = [
            {page: "home", subpages: [], link: "#"},
            {page: "cre asset search", subpages: [], link: "#"},
            {page: "principal investor intro", subpages: [], link: "#"},
            {page: "service provider intro", subpages: [], link: "#"},
            {page: "ica employment opp.", subpages: [], link: "#"},
            {page: "global service provider db", subpages: [], link: "#"},
            {page: "global membership orgs", subpages: [], link: "#"}
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

export default LO;