const webpack = require('webpack');
const path = require('path');

module.exports = {
    entry: {
        vendors: [
            'babel-polyfill', //<<-- for async functions
        ],

        //nav
        "navmain": "./pages/nav/navmain.js",
        "topnav": "./pages/nav/topnav.js",
        "footer": "./pages/nav/footer.js",
        "affiliations": "./pages/nav/affiliations.js",

        //map search
        "assetSearch": "./pages/asset-search/assetSearch.js",
        "main": "./pages/AssetSearch2/main.js",

        //view asset
        "view-asset": "./pages/data-portal/view-asset/view-asset.js",
        "view-sample-asset": "./pages/data-portal/view-asset/view-sample-asset.js",

        //general components (e.g. contacts, user profile, manage upload, etc.)
        "user-basic-components": "./user-basic-components.js",

        //basic components
        'basic-components': "./basic-components.js",

        'commissioner-calculator' : './components/commission-calculator/commission-calculator.js',

        //signalR - real time communication
        "intra-site-communication-component": "./intra-site-communication-component.js",

        //service provider components
        "service-provider-components": "./pages/service-provider/service-provider-components.js",

        //admin components
        "admin-components": "./pages/admin/admin-components.js"
    },
    output: {
        filename: '[name].js',
        path: __dirname + '/build/'
    },
    resolve: {
      alias: {
        APP: path.resolve(__dirname, ''),
      }
    },
    optimization: {
        splitChunks: {
            minSize: 30000,
            maxSize: 0,
            minChunks: 1,
            name: true,
            cacheGroups: {
                commons: {
                    name: 'commons',
                    chunks: 'initial',
                    minChunks: 2
                }
            }
        }
    },
    plugins: [
        //new webpack.ProvidePlugin({
        //    $: "jquery",
        //    jQuery: "jquery"
        //})
    ],
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            },
            {
                test: /\.css$/,
                loader: ['style-loader', 'css-loader']
            },
            {
                test: /\.(png|jpg|woff|woff2|eot|ttf|svg)$/,
                loader: 'url-loader?limit=100000'
            }
        ]
    }
};
