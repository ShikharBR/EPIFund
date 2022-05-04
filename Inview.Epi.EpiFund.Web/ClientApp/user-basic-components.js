import React, { Component } from "react"
import ReactDOM from 'react-dom';

//Chi:
//This is a little trick, import all the basic modals and use PubSub to push open events
//so you don't have to import it into all components that use basic modals
import { ModalManagerComponent } from './basic-modal-manager-component'

import * as UserContactComponent from './pages/user-contacts/user-contacts'
import * as GeneralDashboardComponent from './pages/general/general-dashboard'
import * as UserProfileComponent from './pages/general/general-user-profile'
import { ManageFileComponent } from './pages/general/general-manage-upload'
import { UserReferralTrackingComponent } from './pages/user-referral/user-referral-tracking'
import { SetSampleAssetButton } from './pages/admin/manage-assets' 
