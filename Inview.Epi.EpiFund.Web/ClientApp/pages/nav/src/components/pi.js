import React, { Component } from 'react';

class PI extends Component {
    render() {
        const directory = [
            {page: "dashboard", subpages: [], link: "#"},
            {page: "my inventory", subpages: [{page: "assets", link: "#"}, {page: "portfolio", link: "#"}], link: "#"},
            {page: "search", subpages: [{page: "new search", link: "#"}, {page: "favorites", link: "#"}, {page: "saved searches", link: "#"}], link: "#"},
            {page: "sales & aquisitions", subpages: [], link: "#"},
            {page: "global service provider db", subpages: [{page: "search sps", link: "#"}, {page: "my preferred sps", link: "#"}], link: "#"}
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

export default PI;