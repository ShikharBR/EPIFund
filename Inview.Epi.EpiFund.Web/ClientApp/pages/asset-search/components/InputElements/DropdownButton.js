import React, { Component } from "react"
import ReactDOM from 'react-dom';

// props.label: string (text inside button)
// children: inner content of dropdown
// props.id: optional ID of dropdown menu
// props.closeOnClickInside : bool 
// props.buttonClass : string
// props.dropdownMenuClass : string
// children will be put inside the dropdown menu.
export class DropdownButton extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
          ddOpen: false,
        };
    
        this.handleClick = this.handleClick.bind(this);
        this.toggleMenuDisplay = this.toggleMenuDisplay.bind(this);
        this.getClassName = this.getClassName.bind(this);
      }
      componentWillUnmount(){
        $('body').unbind('click', this.handleClick);
      }
      getClassName(){
        return this.state.ddOpen ? 'dropdown open' : 'dropdown';
      }
      getButtonClass(){
          return this.props.buttonClass ? `btn btn-default ${this.props.buttonClass} generalinputstyle` : 'btn btn-default generalinputstyle';
      }
      getDropdownClass(){
          return this.props.dropdownClass ? `dropdown-menu ${this.props.dropdownClass}` : 'dropdown-menu';
      }
      handleClick(){
        let clickedDropdown = $('ul.dropdown-menu').is(event.target);
        
        if((this.props.closeOnClickInside && clickedDropdown) ||
            (!clickedDropdown
            && $('div.dropdown.open').has(event.target).length === 0
            && $('.open').has(event.target).length === 0
            )) {
            $('body').unbind('click', this.handleClick);
            this.setState({ddOpen:false});
        }
      }
      toggleMenuDisplay(){
        this.setState(previousState => ({
          ddOpen: !previousState.ddOpen
        }));
    
        if(this.state.ddOpen){
          $('body').unbind('click', this.handleClick);
        } else {
          $('body').on('click', this.handleClick);
        }
      }

    
      render(){
        return React.createElement('div', {id:this.props.id, className:this.getClassName()},
          React.createElement('button', {className:this.getButtonClass(), style: {width: "100%" }, onClick:this.toggleMenuDisplay},
            React.createElement('span', {id: 'qsAssetTypeButtonLabel'}, this.props.label + ' '), React.createElement('span', {className:'caret'})),
            React.createElement('div', { className:this.getDropdownClass()}, 
              this.props.children)
        );
      }
}

export default DropdownButton
