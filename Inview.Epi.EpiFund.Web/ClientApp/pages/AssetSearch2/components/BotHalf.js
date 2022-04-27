import React, { Component } from "react";
import { connect } from 'react-redux'
import StatusBoxes from './StatusBoxes';
import ViewOptions from './ViewOptions';
import ResultsWrap from './ResultsWrap';

class BotHalf extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return(
            <>
                <div className="AssetSearch_Displayflex" style={{ justifyContent: "space-between", paddingRight: "25px" }}>
                    <StatusBoxes />
                    <ViewOptions />
                </div>

                <ResultsWrap />
            </>
        );
    }
}

const mapStateToProps = state => ({
});

export default connect(mapStateToProps, {})(BotHalf);
