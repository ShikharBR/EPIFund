import React, { Component } from "react"
import ReactDOM from 'react-dom';
const _ = require('lodash');
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'

import { IntraSiteCommunicationComponent } from '../../intra-site-communication-component'
import { LoadingComponent } from '../../loading-component'

class ManageFileComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.state = {
            isLoading: true,
            orderedSubFolderOrFiles: []
        };

        this.refresh = this.refresh.bind(this);
        this.setDropZones = this.setDropZones.bind(this);

        ///events
        PubSub.subscribe("newFilesUploaded", (type, files) => {
            var self = this;

            if (files[0] == null) {
                notificationService.error("Failed to upload or you don't have the permission to upload.");
            }
            else if (files != null && files.length > 0) {

                $.each(files, function (ii, ff) {
                    var found = $(self.state.epiS3Object.SubFolderOrFiles).filter(function (i, f) {
                        if (f.EpiS3ObjectId == ff.EpiS3ObjectId) {
                            return true;
                        }
                        return false;
                    });

                    if (found.length == 1) {
                        found[0].LastModifiedTime = ff.LastModifiedTime;
                    } else if (found.length == 0) {
                        self.state.epiS3Object.SubFolderOrFiles.push(ff);
                    }
                });

                const orderedSubFolderOrFiles =
                    _.orderBy(self.state.epiS3Object.SubFolderOrFiles,
                        ['IsFolder', 'Name'],
                        ['desc', 'asc']
                    );

                self.setState({
                    orderedSubFolderOrFiles: orderedSubFolderOrFiles
                });

                if (files.length == 1)
                    notificationService.info("File uploaded.");
                else
                    notificationService.info("Files uploaded.");
            }
        });
    }

    async refresh(eid) {
        const self = this;
        const profile = await epiService.getCurrentUserInfoAndProfile();

        epiService.getUserFolder(eid).then(function (response) {
            setTimeout(function () {
                var name = "";
                if (response.Name == null)
                    name = "Home";
                else
                    name = response.Name

                //set isActive = false, true will show actions
                $.each(response.SubFolderOrFiles, function (i, s) {
                    s.IsActive = false;
                });

                const orderedSubFolderOrFiles =
                    _.orderBy(response.SubFolderOrFiles,
                        ['IsFolder', 'Name'],
                        ['desc', 'asc']
                    );

                self.setState({
                    currentFolderName: name,
                    currentFolderS3ObjectId: response.EpiS3ObjectId,
                    epiS3Object: response,
                    orderedSubFolderOrFiles: orderedSubFolderOrFiles,
                    isLoading: false,
                    folderExists: true,
                });

                //bootstrap tooltip
                setTimeout(function () { $('[data-toggle="tooltip"]').tooltip(); }, 500);

                self.setDropZones(self.state.currentFolderS3ObjectId);

            }, 1000); //too fast... i have to slow it down
        }).fail(() => {
            self.setState({
                isLoading: false,
                folderExists: false,
            });

        });
    }

    componentDidMount() {
        const self = this;

        //init
        //1.) get the current user home directory
        //get the current epiS3ObjectId from the url
        const id = epiService.getParameterByName("eid");
        if (id == null) {
            self.refresh(null);
        } else {
            self.refresh(id);
        }

        $("html, body").animate({ scrollTop: 0 }, "fast");

        //uploader
        $(document).bind('drop dragover', function (e) {
            e.preventDefault();
        });

        $(document).bind('dragover', function (e) {
            var dropZones = $('.main-dropzone'),
                timeout = window.dropZoneTimeout;
            if (timeout) {
                clearTimeout(timeout);
            } else {
                dropZones.addClass('in');
            }
            var hoveredDropZone = $(e.target).closest(dropZones);
            dropZones.not(hoveredDropZone).removeClass('hover');
            hoveredDropZone.addClass('hover');
            window.dropZoneTimeout = setTimeout(function () {
                window.dropZoneTimeout = null;
                dropZones.removeClass('in hover');
            }, 100);
        });

        //bootstrap tooltip
        setTimeout(function () { $('[data-toggle="tooltip"]').tooltip(); }, 500);
    }

    setDropZones(folderId) {

        $('#fileupload').fileupload({
            url: '/epis3/UploadFiles',
            singleFileUploads: false, //<-- post everything in one call
            dropZone: $(".main-dropzone"),
            formData: { folderId: folderId },
            autoUpload: true,
            beforeSend: function () {
                notificationService.info("Uploading...");
            },
            done: function (e, data) {
                //done
            },
            fail: function (e, data) {
                // data.errorThrown
                // data.textStatus;
                // data.jqXHR;
                console.log(data);
            }
        }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled');
    }

    noPermissionTodownloadFile(item) {
        notificationService.error("You don't have the permssion to download.");
    }

    render() {

        return (
            <React.Fragment>
                {
                    this.state.isLoading ?
                        <LoadingComponent />
                        :
                        <React.Fragment>
                            <div>
                                <IntraSiteCommunicationComponent />

                                <h3>Document Center</h3>

                                <div className="row">
                                    {
                                        (!this.state.isLoading && !this.state.folderExists) &&
                                        <div className="col-sm-12">
                                            <div className="alert alert-danger">
                                                Folder doesn't exist or you don't have the permission to access.<br /><br />
                                                <a href="/General/ManageUploads" className="btn btn-primary"><i className="fas fa-arrow-circle-left"></i> Back to my home folder</a>
                                            </div>
                                            <hr />
                                        </div>
                                    }
                                    {
                                        (!this.state.isLoading && this.state.folderExists) &&
                                        <div>
                                            <div className="col-sm-9">
                                                <div className="col-sm-12 main-dropzone dropzone fade">

                                                    <h3>{this.state.currentFolderName}</h3>

                                                    <table className="table manage-table">
                                                        <tbody>
                                                            {
                                                                this.state.orderedSubFolderOrFiles.map((item, k) => {
                                                                    return (
                                                                        <tr key={k} className="manage-table-item" onMouseOver={() => {
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
                                                                            <td className="file-icon">
                                                                                {
                                                                                    item.IsFolder &&
                                                                                    <a href={'/General/ManageUploads?eid=' + item.EpiS3ObjectId}>
                                                                                        <div className="folder">
                                                                                            <i className="fas fa-folder"></i>
                                                                                        </div>
                                                                                    </a>
                                                                                }

                                                                                {
                                                                                    (item.IsFile && item.CanDownload) &&
                                                                                    <a target="_blank" href={'/epis3/downloadFileById?eid=' + item.EpiS3ObjectId}>
                                                                                        <div className="file">
                                                                                            <i className="far fa-file"></i>
                                                                                        </div>
                                                                                    </a>
                                                                                }

                                                                                {
                                                                                    (item.IsFile && !item.CanDownload) &&
                                                                                    <div onClick={() => { this.noPermissionTodownloadFile(item) }}>
                                                                                        <div className="file">
                                                                                            <i className="far fa-file"></i>
                                                                                        </div>
                                                                                    </div>
                                                                                }
                                                                            </td>
                                                                            <td className="name">
                                                                                {
                                                                                    item.IsFolder &&
                                                                                    <a href={'/General/ManageUploads?eid=' + item.EpiS3ObjectId}>
                                                                                        <div>{item.Name}</div>
                                                                                    </a>
                                                                                }

                                                                                {
                                                                                    (item.IsFile && item.CanDownload) &&
                                                                                    <a target="_blank" href={'/epis3/downloadFileById?eid=' + item.EpiS3ObjectId}>
                                                                                        <div>{item.Name}</div>
                                                                                    </a>
                                                                                }

                                                                                {
                                                                                    (item.IsFile && !item.CanDownload) &&
                                                                                    <div onClick={() => { this.noPermissionTodownloadFile(item) }}>{item.Name}</div>
                                                                                }
                                                                            </td>
                                                                            <td className="action">
                                                                                {
                                                                                    item.IsActive &&
                                                                                    <div>
                                                                                        <button title="Delete" data-toggle="tooltip" data-placement="right" className="pull-right btn btn-sm btn-danger margin-right10px"><i className="far fa-trash-alt"></i></button>
                                                                                        <button title="View Info" data-toggle="tooltip" data-placement="left" className="pull-right btn btn-sm margin-right10px"><i className="fas fa-info-circle"></i></button>
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
                                            </div >
                                            <div className="col-sm-3 actions">
                                                <div className="col-sm-12">
                                                    <div className="form-group">
                                                        <input type="text" className="form-control" placeholder="search in this folder" disabled />
                                                        <button disabled title="Search" data-toggle="tooltip" data-placement="top" className="disabled btn btn-primary btn-block"><i className="fas fa-search"></i></button>
                                                        <hr />
                                                    </div>
                                                    <button title="Create Folder" data-toggle="tooltip" data-placement="top" className="btn btn-default btn-block"><i className="fas fa-folder-plus"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    }
                                </div >
                                <div className="row hidden">
                                    <div className="col-sm-12">
                                        <input id="fileupload" type="file" name="currentFolderS3ObjectId" multiple />
                                        <div id="progress" className="progress">
                                            <div className="progress-bar progress-bar-success"></div>
                                        </div>
                                        <div id="files" className="files"></div>
                                    </div>
                                </div>
                            </div>
                        </React.Fragment>
                }
            </React.Fragment>
        )
    }
}

export default ManageFileComponent

const element = document.getElementById("service-provider-manage-upload-app");
if (element != null)
    ReactDOM.render(<ManageFileComponent />, element);
