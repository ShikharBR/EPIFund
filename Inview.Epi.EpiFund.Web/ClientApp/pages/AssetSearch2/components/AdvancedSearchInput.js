import React, { Component } from "react";
import DropModal from './DropModal';
import DownArrow from '../img/downarrow.png';

class AdvancedSearchInput extends Component {
    //constructor(props) {
    //    super(props);

    //    this.state = {
    //        mainOptions: [],
    //        showDrop: false
    //    }

    //    this.updateAdvanced = this.updateAdvanced.bind(this);
    //    this.updateShowDrop = this.updateShowDrop.bind(this);
    //    this.updateSelected = this.updateSelected.bind(this);
    //}

    //updateAdvanced(e, index, type) {
    //    //console.log(e.target.value);
    //    this.props.updateAdvancedValue(e.target.value, index, type);
    //}

    //updateShowDrop() {
    //    this.setState({showDrop: !this.state.showDrop});
    //}

    //componentWillMount() {
    //    if (this.props.data.inputtype === "dropdown") {
    //        let optionsArray = [];
    //        for (var property in this.props.data.options) {
    //            optionsArray.push(property);
    //        }
    //        this.setState({mainOptions: optionsArray});
    //    }
    //}

    //updateSelected(data) {
    //    //Data is passed differently here, this function has to exist because my code blows
    //    this.props.sendUpdate();
    //}

    //render() {
    //    //console.log(this.props.data)

    //    let inputValue = this.props.data.value;

    //    // this.state.mainOptions.map(item => {
    //    //     console.log(item);
    //    // })
    //    switch(this.props.data.type) {
    //        case ("money"):
    //            //Regex for adding commas in each 3rd position
    //            inputValue = inputValue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //            //Force add dollar sign to beginning of string
    //            inputValue = "$" + inputValue;
    //            break;
    //        case("percent"):
    //            //Force add percentage sign to beginning of string
    //            inputValue = inputValue + "%";
    //            break;
    //        case("number"):
    //            inputValue = inputValue.toString().replace(/\D/g,'');
    //            break;
    //    }

    //    return(
    //        <div className="AssetSearch_AdvancedSearchInput_wrap" style={(this.props.data.advwidth) ? {width: this.props.data.advwidth} : {width: this.props.data.width}} tabIndex="0" onFocus={(this.props.data.inputtype === "dropdown") ? this.updateShowDrop : null} onBlur={(this.props.data.inputtype === "dropdown") ? this.updateShowDrop : null}>
    //        {this.props.data.name}
    //        {this.props.data.inputtype === "field" && <input type="text" value={inputValue} onChange={(e) => this.updateAdvanced(e, this.props.index, this.props.data.type)}/>}
    //        {this.props.data.inputtype === "dropdown" && 
    //            <>

    //                <div className="AssetSearch_DropDown_main">
    //                    {(this.props.data.value.length === 0) && this.props.data.dropplaceholder}
    //                    {(this.props.data.value.length === 1) && this.props.data.value}
    //                    {(this.props.data.value.length > 1) && "Multiple"}
    //                    <div><img src={DownArrow} alt="" /></div>
    //                </div>

    //                {this.state.showDrop && 
    //                    <DropModal mainoptions={this.state.mainOptions} updateoptions={(this.props.sendUpdate) ? this.updateSelected : null} selectedoptions={this.props.data.value}/>
    //                }

    //            </> 
    //        }
    //        </div>
    //    );
    //}
}

export default AdvancedSearchInput;