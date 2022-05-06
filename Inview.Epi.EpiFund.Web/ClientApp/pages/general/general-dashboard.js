import React, { Component } from "react"
import ReactDOM from 'react-dom';

import { epiService } from '../../services/epiService'

import { LoadingComponent } from '../../loading-component'
import { GeneralDashboardProfileOverviewComponent } from './general-dashboard-profile-overview'
import { UnreadMessageOverviewComponent } from './unread-message-overview'
import { UnreadNotificationOverviewComponent } from './unread-notification-overview'

class GeneralDashboardComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;

        this.refresh();

        this.state = {
            isLoading: true,
        };
    }

    async refresh() {
        const self = this;

        const profile = await epiService.getCurrentUserInfoAndProfile(true);
        profile.Contacts.map(x => { x.Hidden = false; });
        this.setState({
            currentUserProfile: profile,
            isLoading: false
        });

    }

    componentDidMount() {
        const self = this;

        //bootstrap tooltip
        setTimeout(function () { $('[data-toggle="tooltip"]').tooltip(); }, 500);
    }

    render() {

        return (
            <React.Fragment>
                {
                    this.state.isLoading ?
                        <LoadingComponent />
                        :
                        <React.Fragment>
                            <GeneralDashboardProfileOverviewComponent currentUser={this.state.currentUserProfile} />
                            <UnreadMessageOverviewComponent />
                            <UnreadNotificationOverviewComponent />
                        </React.Fragment>
                }
            </React.Fragment>
        )
    }
}

export default GeneralDashboardComponent

const element = document.getElementById("general-communicationcenter-app");
if (element != null)
    ReactDOM.render(<GeneralDashboardComponent />, element);
