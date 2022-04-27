import React, { Component } from "react";
import NumberFormat from 'react-number-format';
import DownArrow from '../img/downarrow.png';
import DropModal from './DropModal';
import { connect } from 'react-redux'
import { getOrUpdateCurrentSearchModel } from '../../../redux/actions/asset-search-action'
import { helpers } from "../../../helpers/helpers";


class BasicSearchInput extends Component {
    constructor(props) {
        super(props);

        this.state = {
            mainOptions: [],
            showDrop: false,
            selectedDrop: this.props.value,

            lists: [
                ...this.props.searchModel.basicSearchArray,
                ...this.props.searchModel.creSearchArray,
                ...this.props.searchModel.mhpSearchArray,
                ...this.props.searchModel.spnSearchArray,
                ...this.props.searchModel.mfSearchArray,
            ]
        }

        this.updateShowDrop = this.updateShowDrop.bind(this);
        this.updateValue = this.updateValue.bind(this);
    }



    componentWillMount() {
        const foundInput = helpers.findInputInLists(this.props.id, this.state.lists);
        if (foundInput.inputtype === "dropdown") {
            let optionsArray = [];
            for (var property in foundInput.options) {
                optionsArray.push(property);
            }
            this.setState({ mainOptions: optionsArray });
        }
    }

    updateShowDrop() {
        this.setState({ showDrop: !this.state.showDrop });
    }

    updateValue(value) {
        const foundInput = helpers.findInputInLists(this.props.id, this.state.lists);
        const oldValue = foundInput.value;

        let inputValue = value;
        if (inputValue.toString().trim() != "") {
            switch (foundInput.type) {
                case ("money"):
                case ("number"):
                case ("percent"):
                    //these are the types we only care about the numbers
                    inputValue = value.replace(/\D/g, '');
                    break;
            }
        }
        foundInput.value = inputValue;

        if (foundInput.name === "Saved Search") {
            this.props.searchModel.savedSearchTitle = value.trim();
            this.props.searchModel.savedSearchTitleChanged = true;
        } else {
            this.props.searchModel.savedSearchTitleChanged = false;
        }


        //limit the updates here, if old value is the same as new value, no need to update
        if (foundInput.inputtype === "dropdown") {
            if ([...oldValue].join() !== [...inputValue].join()) {
                this.props.getOrUpdateCurrentSearchModel(this.props.searchModel);
            }
        }
        else if (oldValue !== inputValue)
            this.props.getOrUpdateCurrentSearchModel(this.props.searchModel);
    }

    render() {
        const foundInput = helpers.findInputInLists(this.props.id, this.state.lists);
        let itemWidth = foundInput.width;
        if (foundInput.advwidth) {
            itemWidth = foundInput.advwidth;
        }
        if (foundInput.isbasic && !this.props.isShowingAdvanced) {
            itemWidth = foundInput.width;
        }
        return (
            <>
                <div className="AssetSearch_BasicSearchInput_wrap"
                    style={{ width: itemWidth }}
                    tabIndex="1" onFocus={(foundInput.inputtype === "dropdown") ? this.updateShowDrop : null}
                    onBlur={(foundInput.inputtype === "dropdown") ? this.updateShowDrop : null}>

                    {foundInput.name}
                    {
                        //currency input
                        (foundInput.inputtype === "field" && foundInput.type === "money") &&
                        <NumberFormat type="text"
                            prefix={'$'}
                            placeholder={foundInput.placeholder}
                            value={foundInput.value}
                            thousandSeparator={true}
                            onChange={(e) => this.updateValue(e.target.value)} />
                    }

                    {
                        //percent input
                        (foundInput.inputtype === "field" && foundInput.type === "percent") &&
                        <NumberFormat type="text"
                            suffix={'%'}
                            placeholder={foundInput.placeholder}
                            value={foundInput.value}
                            thousandSeparator={true}
                            onChange={(e) => this.updateValue(e.target.value)} />
                    }

                    {
                        //text input
                        (foundInput.inputtype === "field" && foundInput.type !== "money" && foundInput.type !== "percent") &&
                        <input type="text"
                            placeholder={foundInput.placeholder}
                            value={foundInput.value}
                            onChange={(e) => this.updateValue(e.target.value)}
                            autoComplete="off" />
                    }

                    {/*dropdown input*/}
                    {foundInput.inputtype === "dropdown" &&
                        <>
                            <div className="AssetSearch_DropDown_main">
                                <div>
                                    {
                                        //no value = placeholder; 1 value = show value; 1+ = show "Multiple"
                                        (() => {
                                            switch (foundInput.value.length) {
                                                case 0:
                                                    return foundInput.dropplaceholder;
                                                    break;
                                                case 1:
                                                    return foundInput.value[0];
                                                    break;
                                                default:
                                                    return "Multiple";
                                                    break;
                                            }
                                        }
                                        )()}
                                </div>
                                <div><img src={DownArrow} alt="" /></div>
                            </div>

                            {this.state.showDrop &&
                                <DropModal mainoptions={this.state.mainOptions} updateValue={this.updateValue} selectedoptions={foundInput.value} />
                            }

                        </>
                    }
                </div>
            </>
        );
    }
}


const mapStateToProps = state => ({
    searchModel: state.assetSearchReducer.getOrUpdateCurrentSearchModel.searchModel,
    isShowingAdvanced: state.assetSearchReducer.showAdvanced.showAdvanced
});

export default connect(mapStateToProps, { getOrUpdateCurrentSearchModel })(BasicSearchInput);
