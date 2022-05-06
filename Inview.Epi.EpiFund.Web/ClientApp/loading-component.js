import React, { Component } from "react"
import ReactDOM from 'react-dom';

export class LoadingComponent extends Component {
    constructor(props) {
        super(props);
    }

    render() {

        return (
            <React.Fragment>

                <div className="row">
                    <div className="col-lg-12">
                        <div style={{ minHeight: '150px' }}>
                            <div className="loading-gif"></div>
                        </div>
                    </div>
                </div>

            </React.Fragment>
        )
    }
}

