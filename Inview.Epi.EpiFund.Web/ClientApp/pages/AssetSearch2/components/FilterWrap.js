import React, { Component } from "react";
import AdvancedSearchInput from './AdvancedSearchInput';
import BSInput from "./BSInput";

class FilterWrap extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <>
                <div style={{ display: "flex", justifyContent: "space-between" }}>
                    <div className="AssetSearch_FlexTitle">{this.props.name}</div>
                    <div className="AssetSearch_DividerLine"></div>
                </div>
                <div className="AssetSearch_Filter_wrap">
                    {
                        this.props.data.map((item, index) => {
                            if (!item.basiconly) {
                                return <BSInput
                                    key={index}
                                    id={item.id} />
                            }
                        })
                    }
                </div>
            </>
        );
    }
}

export default FilterWrap;