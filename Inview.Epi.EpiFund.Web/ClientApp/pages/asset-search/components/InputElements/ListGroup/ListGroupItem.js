import React, { Component } from "react"
import ReactDOM from 'react-dom';

let e = React.createElement;

/* props:
    className: string
    onClick: function
    type: string
    context: string
    badge: string
*/

export class ListGroupItem extends React.Component {
    constructor(props) {
        super(props);
        this.getClass = this.getClass.bind(this);
    }
    getClass(){
        let className = this.props.context ? `list-group-item list-group-item-${this.props.context}`: 'list-group-item'
        return this.props.className ? `${className} ${this.props.className}` : `${className}`;
    }
    render() {
        const href = this.props.type == 'a' ? this.props.href || null : null;
        const badge = this.props.badge ? e('span', {className:'badge'}, this.props.badge) : null;
        return e(this.props.type, {className:this.getClass(), href: href, onClick:this.props.onClick || null}, 
            badge, 
            this.props.children
        );
    }
}

export default ListGroupItem

export const ListGroupItemTypes = {
    ListItem: 'li',
    Button: 'button',
    Anchor: 'a'
}
export const ListGroupItemContexts = {
    Success: 'success',
    Danger: 'danger',
    Warning: 'warning',
    Info: 'info',
}