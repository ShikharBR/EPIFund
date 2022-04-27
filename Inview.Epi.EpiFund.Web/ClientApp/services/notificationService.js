import React, { Component } from "react"
import ReactDOM from 'react-dom';

import { ReplyMessageNotificationComponent } from '../modals/basic/ReplyNotificationComponent'

//5/9/2019
//Chi: we need to import this plugin so we can bundle it
//it lives under BundleConfig.cs for now...

export const notificationService = {
    closeAll: function () {
        $.notifyClose();
    },
    info: function (msg) {

        $.notify({
            // options
            message: msg
        }, {
                // settings
                element: 'body',
                type: "info",
                delay: 5000,
                mouse_over: 'pause',
                z_index: 9999
            });
    },
    warn: function (msg) {

        $.notify({
            // options
            message: msg
        }, {
                // settings
                element: 'body',
                type: "warning",
                delay: 5000,
                mouse_over: 'pause',
                z_index: 9999
            });
    },
    error: function (msg) {

        $.notify({
            // options
            message: msg
        }, {
                // settings
                element: 'body',
                type: "danger",
                delay: 5000,
                mouse_over: 'pause',
                z_index: 9999
            });
    },
    showNewMessageAlert: function (msg) {

        $.notify({
            // options
            message: `
                <div id='${msg.MessageId}'></div>
            `
        }, {
                // settings
                element: 'body',
                type: "info",
                delay: 5000,
                mouse_over: 'pause',
                z_index: 9999
            });
        
        ReactDOM.render(<ReplyMessageNotificationComponent msg={msg}/>, document.getElementById(msg.MessageId));
    },
    showNewNotificationAlert: function (msg) {

        $.notify({
            // options
            message: `
                <p>`+ msg.Subject + `<p><hr/>`
        }, {
                // settings
                element: 'body',
                type: "info",
                delay: 5000,
                mouse_over: 'pause',
                z_index: 9999
            });
    }
};
