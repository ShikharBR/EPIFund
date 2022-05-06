import React, { Component } from "react";

class DropModal extends Component {
    constructor(props) {
        super(props);

        this.state = {
            selectedOptions: Object.assign([], this.props.selectedoptions) //do this so it doesn't hook directly into the value
        }



        this.optionClick = this.optionClick.bind(this);
    }

    optionClick(item) {
        if (this.state.selectedOptions.includes(item)) {
            let itemindex = this.state.selectedOptions.indexOf(item);
            let copySelectedOptions = this.state.selectedOptions;
            copySelectedOptions.splice(itemindex, 1);
            this.setState({ selectedOptions: copySelectedOptions });
        }
        else {
            let copySelectedOptions = this.state.selectedOptions;
            copySelectedOptions.push(item);
            this.setState({ selectedOptions: copySelectedOptions });
        }
    }

    componentWillUnmount() {
        this.props.updateValue(this.state.selectedOptions); //this will push the change up to BSInput updateValue()
    }

    render() {
        return (
            <div className="AssetSearch_DropModal_wrap animated fadeIn superfast">
                {this.props.mainoptions.map((item, index) => {
                    return <div key={index} className={(this.state.selectedOptions.includes(item)) ? "AssetSearch_Option_wrap AssetSearch_Option_selected" : "AssetSearch_Option_wrap"} onClick={() => this.optionClick(item)}>{item}</div>
                })}
            </div>
        );
    }
}

export default DropModal;