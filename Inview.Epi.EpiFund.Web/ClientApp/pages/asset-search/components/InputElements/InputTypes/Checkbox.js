import React, { Component } from "react"
import ReactDOM from 'react-dom';

let e = React.createElement;
const Checkbox = props => e('span', { className:'checkbox-container' },
        e('input', {key: 0, 'data-id':props.dataID, type:'checkbox', 
            onChange: props.onChange, onClick: props.onClick, className: props.className, checked: props.checked}), 
        props.label );

export default Checkbox
