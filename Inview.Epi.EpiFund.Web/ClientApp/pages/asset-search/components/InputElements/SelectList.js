import React, { Component } from "react"
import ReactDOM from 'react-dom';

let e = React.createElement;
/* options is an array objects
    options: [{label: 'Option Name', value: optionValue} ... ]

    id: string
    className: string
    defaultValue: string, value of default selection
    onChange: callback fn
*/
export class SelectList extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            currentSelection: this.props.defaultValue || '',
        }
        this.onChange = this.onChange.bind(this);
    }
    getClass(){
        return this.props.className ? `form-control ${this.props.className}` : 'form-control';
    }
    componentDidMount() {
        if(this.props.id && this.props.defaultValue)
            document.getElementById(this.props.id).selectedIndex = this.props.defaultValue;
    }
    onChange(event){
        if(this.props.onChange) this.props.onChange(event);
        this.setState({
            currentSelection:event.target.value
        });
    }


    render() {
        return e('select', {id:this.props.id, className:this.getClass(), onChange:this.onChange},
            this.props.options.map((option,i) => e('option', {key:option.value, value:option.value}, option.label)));
    }
}

export default SelectList
