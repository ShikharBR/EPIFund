import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');
import { epiService } from '../../services/epiService'

export class GeneralDashboardProfileOverviewComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
    }

    render() {

        return (
            <React.Fragment>
                <div className="panel">
                    <div className="panel-body">
                        <div className="row">
                            <div className="col-sm-3">
                                <div className="profile-image">
                                    <img src={this.props.currentUser.ProfileImageUrl} className="img-thumbnail" />
                                </div>
                            </div>
                            <div className="col-sm-9">

                                <table className="table user-profile-table">
                                    <tbody>
                                        <tr>
                                            <td colSpan="2"><h3>{this.props.currentUser.FullName}</h3></td>
                                        </tr>
                                        <tr>
                                            <td>Position:</td>
                                            <td><h4>{this.props.currentUser.JobTitle}</h4></td>
                                        </tr>
                                        <tr>
                                            <td>Phone #:</td>
                                            <td><h4>{this.props.currentUser.Phone}</h4></td>
                                        </tr>
                                        <tr>
                                            <td>USCRE User Type:</td>
                                            <td>
                                                <h4>{this.props.currentUser.UserTypeDescription}</h4>
                                            </td>
                                        </tr>
                                        {
                                            this.props.currentUser.ServiceProviderType != null &&
                                            <tr>
                                                <td>Service Provider Type / Industry:</td>
                                                <td>
                                                    <h4>{this.props.currentUser.ServiceProviderTypeDescription}</h4>
                                                </td>
                                            </tr>
                                        }

                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <hr />
                        <button onClick={() => { PubSub.publish('newMessage') }} className="btn btn-primary margin-right10px">
                            <i className="fas fa-envelope"></i>&nbsp;
                            Compose a New Message
                        </button>
                        <a href="/General/ManageUploads" className="btn btn-info margin-right10px">
                            <i className="fas fa-folder"></i>&nbsp;
                            Manage Uploads
                        </a>
                        <a href="/General/UserProfile" className="btn btn-info margin-right10px">
                            <i className="fas fa-user-alt"></i>&nbsp;
                            Edit Profile
                        </a>
                    </div>
                </div>
            </React.Fragment>
        )
    }
}

 