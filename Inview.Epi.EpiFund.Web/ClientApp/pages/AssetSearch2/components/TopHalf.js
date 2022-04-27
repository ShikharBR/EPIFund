import React, { Component } from "react";
import TopBar from './TopBar';
import BasicSearch from './BasicSearch';
import { connect } from 'react-redux'

class TopHalf extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <>
                <TopBar />
                <BasicSearch />
            </>
        );
    }
}

const mapStateToProps = state => ({

});

export default connect(mapStateToProps, {})(TopHalf);
