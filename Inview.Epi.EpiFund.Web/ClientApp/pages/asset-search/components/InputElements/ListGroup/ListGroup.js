import React, { Component } from "react"
import ReactDOM from 'react-dom';

import ListGroupItem from './ListGroupItem.js';

let e = React.createElement;

/* props:
    id: string
    className: string
    listItemClassName: string
    type: string ('none', 'li', 'a', 'button') // sets list-group-item type
    onClick: function // called when a list item is clicked
    Children:array of items
*/

export class ListGroup extends React.Component {
    constructor(props) {
        super(props);
        this.getClass = this.getClass.bind(this);
        this.generateListItems = this.generateListItems.bind(this);
    }

    getClass(){
        return this.props.className ? `list-group ${this.props.className}` : 'list-group';
    }
    generateListItems(){
        if(this.props.children == null) return;

        let children = this.props.children;
        let type = this.props.type ? this.props.type : 'none';

        if(type == 'none') return children;

        let className = this.props.listItemClassName ? this.props.listItemClassName : null;
        let onClick = this.props.onClick ? this.props.onClick : null;

        return children.map((child, index) => 
            e(ListGroupItem, {key:child.key || index , type:type, className:className, onClick:onClick}, 
            child))
    }
    render() {
        const container = (this.props.type === 'li' ? 'ul' : 'div')
        return e(container, 
                {id:this.props.id, className:this.getClass()},
            this.generateListItems()
        )
    }
}

export default ListGroup
