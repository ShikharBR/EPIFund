import React, { Component } from "react";
import ReactDOM from 'react-dom';
import { ToastContainer, Slide } from 'react-toastify';
import { Provider } from 'react-redux'
import store from '../../ /../redux/epiStore'
import './AssetSearch2.css';
import 'react-toastify/dist/ReactToastify.css';
import AssetSearch from "./AssetSearch";


class AssetSearchApp extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <Provider store={store}>
                <AssetSearch />
                <ToastContainer transition={Slide} autoClose={3000} draggable pauseOnFocusLoss={false} />
            </Provider>
        );
    }
}

ReactDOM.render(<AssetSearchApp />, document.getElementById('SearchMount'));