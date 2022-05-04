import React, { Component } from "react"
import ReactDOM from 'react-dom';
import Modal from 'react-modal';
const PubSub = require('pubsub-js');

import { epiService } from '../../services/epiService'
import { notificationService } from '../../services/notificationService'
import { LoadingComponent } from '../../loading-component'
import { QuickViewComponent } from "../../pages/general/general-quick-view";

export class ModalQuickViewUserProfileComponent extends Component {
    constructor(props) {
        super(props);
        const self = this;
        this.state = {
            isOpen: false,
        }

        this.openModal = this.openModal.bind(this);
        this.afterOpenModal = this.afterOpenModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.send = this.send.bind(this);

        ///////////////events/////////////////////////////////////

        PubSub.subscribe("quickViewUser", (type, selectedUser) => {
            this.setState({
                isOpen: true,
                selectedUser: selectedUser
            });
        });
    }

    componentDidMount() {
        this.refresh();
    }

    async refresh() {
        const profile = await epiService.getCurrentUserInfoAndProfile(true);
        this.setState({
            currentUserProfile: profile
        })
    }

    openModal() {

    }

    afterOpenModal() {

    }

    closeModal() {
        this.setState({ isOpen: false });
    }

    onChange() {

    }

    send() {
        this.closeModal();
    }

    render() {
        return (
            <React.Fragment>
                {this.state.currentUserProfile != null &&
                    <Modal
                        isOpen={this.state.isOpen}
                        onAfterOpen={this.afterOpenModal}
                        onRequestClose={this.closeModal}
                        shouldCloseOnOverlayClick={true}
                        overlayClassName="modal-mask"
                        className="modal-wrapper"
                        closeTimeoutMS={200}>

                        <div className="modal-container">

                            <div className="modal-header">
                                <h3>Quick View User</h3>
                            </div>

                            <div className="modal-body">

                                <QuickViewComponent parentModal={this} selectedUserProfile={this.state.selectedUser} />

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


