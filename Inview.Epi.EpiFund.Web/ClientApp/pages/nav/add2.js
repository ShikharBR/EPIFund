import React, { Component } from "react";

class Add2 extends Component {
    constructor(props) {
        super(props);
  
        this.state = {
            documents: [{ 'Title Insurance Policy': 'File Upload' }, { 'Vesting Deed': 'File Upload' }, { 'State Documentation': 'File Upload' }, { 'Other': 'File Upload' }]
        }
  
        this.back = this.back.bind(this);
        this.nextStep = this.nextStep.bind(this);
        this.handleDrop = this.handleDrop.bind(this);
        this.handleDragOver = this.handleDragOver.bind(this);
        this.handleFileUpload = this.handleFileUpload.bind(this);
    }
  
    back() {
        this.props.goback();
    }
  
    nextStep() {
        if (api.tempGlobal.docTitleInsurancePolicy &&
            api.tempGlobal.docVestingDeed &&
            api.tempGlobal.docStateDocumentation) {
            this.props.gonext(this.state, "add2");
        } else {
            alert('You didnt upload all the required documents! SHAME!!!!');
        }
  
    }
    handleFileUpload(files, index, key) {
        //console.log(files[0].name);
  
        let copyState = Object.assign({}, this.state);
        copyState.documents[index][key] = files[0].name;
        this.setState(copyState);
  
        api.uploadFile(files, event.target.getAttribute('data-type'));
    }
    handleDragOver(event) {
        event.stopPropagation();
        event.preventDefault();
    }
  
    handleDrop(event, index, key) {
        event.stopPropagation();
        event.preventDefault();
  
        let copyState = Object.assign({}, this.state);
        copyState.documents[index][key] = event.dataTransfer.files[0].name;
        this.setState(copyState);
  
        api.uploadFile(event.dataTransfer.files, event.target.getAttribute('data-type'));
    }
  
    render() {
        return (
            <div className="modalwrap modal_big animated slideInRight faster">
                <div className="topbar topbar_orange">
                    Claim Asset <a>- Step 2</a>
                    <div className="closebutton" onClick={() => this.props.close()}>×</div>
                </div>
                <div className="modal_contentwrap bigpad">
                    <h1 className="h1bigfont">Please upload your proof of title documents here</h1>
                    <h2 className="h1bigfont">Maximum upload size 25MB</h2>
                    <div className="pad40"></div>
                    <div className="flexwrap_spacebetween">
                        {
                            this.state.documents.map((item, index) => {
                                let key = Object.keys(item)[0];
                                return <div key={key}>
                                    <h3>{key}</h3>
                                    <div
                                        className="uploadwrap"
                                        draggable="true"
                                        data-type={key}
                                        onDragOver={this.handleDragOver}
                                        onDrop={() => this.handleDrop(index, key)}>
                                        <div>Drag & Drop Files Here</div>
                                        <input className="addclaim_fileinput" type="file" name={key} data-type={key} onChange={(e) => this.handleFileUpload(e.target.files, index, key)} />
                                        <div className="addclaim_filebutton">{this.state.documents[index][key]}</div>
                                    </div>
                                </div>
                            })
                        }
                    </div>
                    <div className="modalfooter">
                        <div>
  
                        </div>
                        <div className="buttonwrap">
                            <div className="submitbtn submitorange top20 hvr-float-shadow" onClick={this.back}>Back</div>
                            <div className="submitbtn top20 hvr-float-shadow" onClick={this.nextStep}>Next</div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
  }

export default Add2;