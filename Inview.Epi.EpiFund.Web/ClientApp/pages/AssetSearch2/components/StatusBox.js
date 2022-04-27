import React, { Component } from "react";

class StatusBox extends Component {
    constructor(props) {
        super(props);

        this.state = {
            total: 0
        }

        this.getShortNum = this.getShortNum.bind(this);
    }

    getShortNum(number) {
        // Nine Zeroes for Billions
        return Math.abs(Number(number)) >= 1.0e+9

        ? (Math.abs(Number(number)) / 1.0e+9).toFixed(1) + " Billion"
        // Six Zeroes for Millions 
        : Math.abs(Number(number)) >= 1.0e+6

        ? (Math.abs(Number(number)) / 1.0e+6).toFixed(1) + " Million"
        // Three Zeroes for Thousands
        : Math.abs(Number(number)) >= 1.0e+3

        ? Math.abs(Number(number)).toLocaleString()

        : Math.abs(Number(number));
    }

    componentWillMount() {
        this.setState({total: this.getShortNum(this.props.total)});
    }

    render() {
        return(
            <div className="AssetSearch_StatusBox">
                <h1 style={{ color: `${this.props.color}`}}>{this.getShortNum(this.props.total)}</h1>
                <h6>{this.props.title}</h6>
            </div>
        );
    }
}

export default StatusBox;