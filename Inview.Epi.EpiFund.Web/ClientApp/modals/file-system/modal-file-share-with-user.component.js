import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const PubSub = require('pubsub-js');

import { helpers } from '../../helpers/helpers'
import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'
import { LoadingComponent } from '../../loading-component'

export class ModalFileShareWithUserComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
        this.state = {
            isOpen: false,
            isLoading: true,
            currentFolderName: "",
            currentFolderS3ObjectId: "",
            epiS3Object: { SubFolderOrFiles: [] },
            orderedSubFolderOrFiles: [],

            showPermission: false,
            targetShareUserName: "",
            targetFolderName: "",

            permission: {
                EpiS3ObjectId: "",
                SharedWithUserId: "",
                CanWrite: true,
                CanRead: false,
                CanModify: false,
                IncludeSubFolders: false,
                Timestamp: ""
            },

            canRemove: false
        }

        epiService.getCurrentUserInfoAndProfile(true).then(profile => {
            self.setState({
                currentUserProfile: profile
            })

        });

        this.openModal = this.openModal.bind(this);
        this.afterOpenModal = this.afterOpenModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.goToFolder = this.goToFolder.bind(this);
        this.setPermission = this.setPermission.bind(this);
        this.savePermission = this.savePermission.bind(this);
        this.removeAccess = this.removeAccess.bind(this);

        ///////////////events/////////////////////////////////////

        PubSub.subscribe("shareFilesWithUser", (type, selectedUserProfile) => {
            this.refresh();

            this.setState({
                isOpen: true,
                showMessageAlert: false,
                selectedUserProfile: selectedUserProfile,
                targetShareUserName: selectedUserProfile.User.FullName
            });
        });
    }

    async refresh() {
        const self = this;

        epiService.getUserFolder().then(function (response) {

            const folders = response.SubFolderOrFiles.filter((e, i) => {
                if (e.IsFolder)
                    return true;
            });

            const orderedSubFolderOrFiles =
                _.orderBy(folders,
                    ['IsFolder', 'Name'],
                    ['desc', 'asc']
                );

            self.setState({
                orderedSubFolderOrFiles: orderedSubFolderOrFiles,
                currentFolderName: self.name,
                epiS3Object: response,
                isLoading: false
            })

            //bootstrap tooltip
            setTimeout(function () { $('[data-toggle="tooltip"]').tooltip(); }, 500);

        }).fail((r) => {
            //failed
        });


    }

    goToFolder(epiS3ObjectId) {
        console.log("GOING TO :" + epiS3ObjectId);
    }

    setPermission(epiS3Object) {
        var self = this;
        //get permission if there is one
        epiService.getPermissionBySharedWithUserId(epiS3Object.EpiS3ObjectId, self.state.selectedUserProfile.UserId)
            .then(function (permission) {

                let canRemove = false;

                //if null, create a new one
                if (permission.EpiS3ObjectPermissionId === helpers.GuidEmpty) {
                    //reset it
                    permission.EpiS3ObjectPermissionId = helpers.GuidEmpty;
                    permission.EpiS3ObjectId = epiS3Object.EpiS3ObjectId;
                    permission.SharedWithUserId = self.state.selectedUserProfile.UserId;
                    permission.OwnerUserId = self.state.currentUserProfile.UserId;
                    permission.CanWrite = true;
                    permission.CanRead = false;
                    permission.CanModify = false;
                    permission.IncludeSubFolders = false;
                    permission.Timestamp = moment().format("YYYY-MM-DD");

                    canRemove = false;
                } else {
                    canRemove = true;
                }

                self.setState({
                    targetFolderName: epiS3Object.Name,
                    permission: permission,
                    showPermission: true,
                    canRemove: canRemove
                });

            });
    }

    savePermission() {
        var self = this;
        self.setState({ isLoading: true });
        epiService.givePermission(
            self.state.permission
        ).then(response => {
            notificationService.info("Shared folder with " + self.state.selectedUserProfile.FullName);
            self.closeModal();
        });
    }

    removeAccess() {
        var self = this;
        self.setState({ isLoading: true });

        epiService.removePermission(self.state.permission).then(function (r) {
            if (r == true) {
                notificationService.info("Removed " + self.state.selectedUserProfile.FullName + " access right on folder");
                self.closeModal();
            }
        });

    }

    openModal() {

    }

    afterOpenModal() {

    }

    closeModal() {

        this.setState({
            isOpen: false,
            isLoading: true,
            currentFolderName: "",
            currentFolderS3ObjectId: "",
            epiS3Object: { SubFolderOrFiles: [] },
            orderedSubFolderOrFiles: [],

            showPermission: false,
            targetShareUserName: "",
            targetFolderName: "",

            permission: {
                EpiS3ObjectId: "",
                SharedWithUserId: "",
                CanWrite: true,
                CanRead: false,
                CanModify: false,
                IncludeSubFolders: false,
                Timestamp: ""
            },

            canRemove: false
        });
    }

    onChange() {

    }

    render() {
        return (
            <React.Fragment>
                {
                    this.state.currentUserProfile != null &&
                    <Modal
                        isOpen={this.state.isOpen}
                        onAfterOpen={this.afterOpenModal}
                        onRequestClose={this.closeModal}
                        shouldCloseOnOverlayClick={true}
                        overlayClassName="modal-mask"
                        className="modal-wrapper share-folder-modal"
                        closeTimeoutMS={200}>

                        <div className="modal-container">
                            <div className="modal-header">
                                <h3>Choose a folder to share</h3>
                            </div>

                            <div className="modal-body">
                                {
                                    this.state.isLoading ?
                                        <LoadingComponent /> :
                                        <React.Fragment>
                                            {
                                                this.state.showPermission === false &&
                                                <div className="row">
                                                    <div className="col-sm-12 choosing-folder">

                                                        <p className="header">{this.state.currentFolderName}</p>

                                                        <table className="table manage-table">
                                                            <tbody>
                                                                {
                                                                    this.state.orderedSubFolderOrFiles.map((item, i) => {
                                                                        return (
                                                                            <tr key={i} className={"manage-table-item " + (item.IsFolder ? 'folder-item' : null) + (item.IsFile ? 'folder-item' : null)} onMouseOver={() => {
                                                                                item.IsActive = true;
                                                                                this.setState({
                                                                                    orderedSubFolderOrFiles: this.state.orderedSubFolderOrFiles
                                                                                })
                                                                            }} onMouseLeave={() => {
                                                                                item.IsActive = false;
                                                                                this.setState({
                                                                                    orderedSubFolderOrFiles: this.state.orderedSubFolderOrFiles
                                                                                })
                                                                            }}>
                                                                                <td width="70px" className="file-icon" onClick={() => { this.goToFolder(item.EpiS3ObjectId) }}>
                                                                                    <div className={(item.IsFolder ? 'folder' : null) + (item.IsFile ? 'file' : null)}>
                                                                                        <i className={(item.IsFolder ? 'fas fa-folder' : null) + (item.IsFile ? 'far fa-file' : null)}></i>
                                                                                    </div>
                                                                                </td>
                                                                                <td className="name" onClick={() => { this.goToFolder(item.EpiS3ObjectId) }} >
                                                                                    <div>{item.Name}</div>
                                                                                </td>
                                                                                <td className="action">
                                                                                    {
                                                                                        item.IsActive &&
                                                                                        <div>
                                                                                            <button onClick={() => { this.setPermission(item) }} title="Share this folder" data-toggle="tooltip" data-placement="left" className="pull-right btn btn-sm btn-info margin-right10px">
                                                                                                <i className="fas fa-share"></i>
                                                                                            </button>
                                                                                        </div>
                                                                                    }
                                                                                </td>
                                                                            </tr>
                                                                        )
                                                                    })

                                                                }

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            }

                                            {
                                                this.state.showPermission &&
                                                <div className="row">
                                                    <div className="col-sm-12 setting-permission">
                                                        <p className="header">Sharing <span className="target-folder-name">{this.state.targetFolderName}</span> with <span className="target-user-name">{this.state.targetShareUserName}</span></p>

                                                        <ul className="list-group list-group-flush list-group-permission">
                                                            <li className="list-group-item">
                                                                <div className="item-name">Allow {this.state.targetShareUserName} to upload</div>
                                                                <label className="switch">
                                                                    <input type="checkbox" className="primary" checked={this.state.permission.CanWrite} onChange={() => {
                                                                        this.state.permission.CanWrite = !this.state.permission.CanWrite;
                                                                        this.setState({ permission: this.state.permission });
                                                                    }} />
                                                                    <span className="slider"></span>
                                                                </label>
                                                            </li>
                                                            <li className="list-group-item">
                                                                <div className="item-name">Allow {this.state.targetShareUserName} to download</div>
                                                                <label className="switch ">
                                                                    <input type="checkbox" className="primary" checked={this.state.permission.CanRead} onChange={() => {
                                                                        this.state.permission.CanRead = !this.state.permission.CanRead;
                                                                        this.setState({ permission: this.state.permission });
                                                                    }} />
                                                                    <span className="slider"></span>
                                                                </label>
                                                            </li>
                                                            <li className="list-group-item">
                                                                <div className="item-name">Allow {this.state.targetShareUserName} to view uploaded files / folders</div>
                                                                <label className="switch ">
                                                                    <input type="checkbox" className="primary" checked={this.state.permission.IncludeSubFolders} onChange={() => {
                                                                        this.state.permission.IncludeSubFolders = !this.state.permission.IncludeSubFolders;
                                                                        this.setState({ permission: this.state.permission });
                                                                    }} />
                                                                    <span className="slider"></span>
                                                                </label>
                                                            </li>
                                                            <li className="list-group-item disabled">
                                                                <div className="item-name">Allow {this.state.targetShareUserName} to modify files / folders</div>
                                                                <label className="switch ">
                                                                    <input disabled type="checkbox" className="primary disabled" checked={this.state.permission.CanModify} onChange={() => {
                                                                        this.state.permission.CanModify = !this.state.permission.CanModify;
                                                                        this.setState({ permission: this.state.permission });
                                                                    }} />
                                                                    <span className="slider"></span>
                                                                </label>
                                                            </li>
                                                            {
                                                                this.state.canRemove &&
                                                                <li className="list-group-item">
                                                                    <button className="btn btn-danger" onClick={this.removeAccess}>Stop Sharing this folder with {this.state.targetShareUserName}</button>
                                                                </li>
                                                            }
                                                        </ul>


                                                        <div>
                                                            <button onClick={() => { this.setState({ showPermission: false }) }} className="btn btn-default pull-right">
                                                                <i className="fas fa-long-arrow-alt-left"></i> Go Back</button>

                                                            <button onClick={this.savePermission} className="btn btn-primary pull-right margin-right10px">
                                                                <i className="fas fa-share"></i> Share</button>
                                                        </div>

                                                    </div>
                                                </div>

                                            }
                                        </React.Fragment>
                                }

                            </div>
                            <div className="modal-footer">
                                <button className="btn pull-right" onClick={this.closeModal}>Cancel</button>
                            </div>
                        </div>

                    </Modal>
                }
            </React.Fragment>
        )
    }
}
