/******/ (function(modules) { // webpackBootstrap
/******/ 	// install a JSONP callback for chunk loading
/******/ 	function webpackJsonpCallback(data) {
/******/ 		var chunkIds = data[0];
/******/ 		var moreModules = data[1];
/******/ 		var executeModules = data[2];
/******/
/******/ 		// add "moreModules" to the modules object,
/******/ 		// then flag all "chunkIds" as loaded and fire callback
/******/ 		var moduleId, chunkId, i = 0, resolves = [];
/******/ 		for(;i < chunkIds.length; i++) {
/******/ 			chunkId = chunkIds[i];
/******/ 			if(Object.prototype.hasOwnProperty.call(installedChunks, chunkId) && installedChunks[chunkId]) {
/******/ 				resolves.push(installedChunks[chunkId][0]);
/******/ 			}
/******/ 			installedChunks[chunkId] = 0;
/******/ 		}
/******/ 		for(moduleId in moreModules) {
/******/ 			if(Object.prototype.hasOwnProperty.call(moreModules, moduleId)) {
/******/ 				modules[moduleId] = moreModules[moduleId];
/******/ 			}
/******/ 		}
/******/ 		if(parentJsonpFunction) parentJsonpFunction(data);
/******/
/******/ 		while(resolves.length) {
/******/ 			resolves.shift()();
/******/ 		}
/******/
/******/ 		// add entry modules from loaded chunk to deferred list
/******/ 		deferredModules.push.apply(deferredModules, executeModules || []);
/******/
/******/ 		// run deferred modules when all chunks ready
/******/ 		return checkDeferredModules();
/******/ 	};
/******/ 	function checkDeferredModules() {
/******/ 		var result;
/******/ 		for(var i = 0; i < deferredModules.length; i++) {
/******/ 			var deferredModule = deferredModules[i];
/******/ 			var fulfilled = true;
/******/ 			for(var j = 1; j < deferredModule.length; j++) {
/******/ 				var depId = deferredModule[j];
/******/ 				if(installedChunks[depId] !== 0) fulfilled = false;
/******/ 			}
/******/ 			if(fulfilled) {
/******/ 				deferredModules.splice(i--, 1);
/******/ 				result = __webpack_require__(__webpack_require__.s = deferredModule[0]);
/******/ 			}
/******/ 		}
/******/
/******/ 		return result;
/******/ 	}
/******/
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// object to store loaded and loading chunks
/******/ 	// undefined = chunk not loaded, null = chunk preloaded/prefetched
/******/ 	// Promise = chunk loading, 0 = chunk loaded
/******/ 	var installedChunks = {
/******/ 		"footer": 0
/******/ 	};
/******/
/******/ 	var deferredModules = [];
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	var jsonpArray = window["webpackJsonp"] = window["webpackJsonp"] || [];
/******/ 	var oldJsonpFunction = jsonpArray.push.bind(jsonpArray);
/******/ 	jsonpArray.push = webpackJsonpCallback;
/******/ 	jsonpArray = jsonpArray.slice();
/******/ 	for(var i = 0; i < jsonpArray.length; i++) webpackJsonpCallback(jsonpArray[i]);
/******/ 	var parentJsonpFunction = oldJsonpFunction;
/******/
/******/
/******/ 	// add entry module to deferred list
/******/ 	deferredModules.push(["./pages/nav/footer.js","commons"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./pages/nav/footer.js":
/*!*****************************!*\
  !*** ./pages/nav/footer.js ***!
  \*****************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ \"./node_modules/react/index.js\");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ \"./node_modules/react-dom/index.js\");\n/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var APP_pages_nav_partners__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! APP/pages/nav/partners */ \"./pages/nav/partners.js\");\nfunction _typeof(obj) { \"@babel/helpers - typeof\"; return _typeof = \"function\" == typeof Symbol && \"symbol\" == typeof Symbol.iterator ? function (obj) { return typeof obj; } : function (obj) { return obj && \"function\" == typeof Symbol && obj.constructor === Symbol && obj !== Symbol.prototype ? \"symbol\" : typeof obj; }, _typeof(obj); }\n\nfunction _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError(\"Cannot call a class as a function\"); } }\n\nfunction _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if (\"value\" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }\n\nfunction _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, \"prototype\", { writable: false }); return Constructor; }\n\nfunction _inherits(subClass, superClass) { if (typeof superClass !== \"function\" && superClass !== null) { throw new TypeError(\"Super expression must either be null or a function\"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); Object.defineProperty(subClass, \"prototype\", { writable: false }); if (superClass) _setPrototypeOf(subClass, superClass); }\n\nfunction _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }\n\nfunction _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }\n\nfunction _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === \"object\" || typeof call === \"function\")) { return call; } else if (call !== void 0) { throw new TypeError(\"Derived constructors may only return object or undefined\"); } return _assertThisInitialized(self); }\n\nfunction _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError(\"this hasn't been initialised - super() hasn't been called\"); } return self; }\n\nfunction _isNativeReflectConstruct() { if (typeof Reflect === \"undefined\" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === \"function\") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }\n\nfunction _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }\n\n\n\n\n\nvar FooterMain = /*#__PURE__*/function (_Component) {\n  _inherits(FooterMain, _Component);\n\n  var _super = _createSuper(FooterMain);\n\n  function FooterMain(props) {\n    var _this;\n\n    _classCallCheck(this, FooterMain);\n\n    _this = _super.call(this, props);\n    _this.state = {};\n    return _this;\n  }\n\n  _createClass(FooterMain, [{\n    key: \"render\",\n    value: function render() {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"footertopwrap\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"smallfooterwrap logoiconsfooter\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"img\", {\n        alt: \"\",\n        src: \"https://s3-us-west-1.amazonaws.com/dev-uscreonline-content/uscrelogo_white.png\",\n        style: {\n          cursor: \"pointer\",\n          maxWidth: \"250px\"\n        },\n        onClick: function onClick() {\n          location.href = \"/Home/\";\n        }\n      }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"footer_tinyicons\",\n        style: {\n          paddingTop: \"0px\",\n          marginTop: \"-15px\",\n          marginLeft: \"17px\"\n        }\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"#\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"img\", {\n        alt: \"\",\n        src: \"https://i.imgur.com/ohgEBlH.png\",\n        style: {\n          maxWidth: \"34px\"\n        }\n      })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"#\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"img\", {\n        alt: \"\",\n        src: \"https://i.imgur.com/8H4kW67.png\",\n        className: \"tinyiconpad\",\n        style: {\n          maxWidth: \"34px\"\n        }\n      })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"#\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"img\", {\n        alt: \"\",\n        src: \"https://i.imgur.com/axnHUJm.png\",\n        className: \"tinyiconpad\",\n        style: {\n          maxWidth: \"34px\"\n        }\n      })))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"bigfooterwrap\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Carousel, null)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"smallfooterwrap flexdisplay\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"smallfooter_halfwrap\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"h3\", null, \"Resources\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"/Home/\"\n      }, \"Home\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"/Home/MyUSCPage\"\n      }, \"My USCRE\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"/Home/ContactUs\"\n      }, \"Contact Us\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"/Home/Affiliations\"\n      }, \"Partners\")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"smallfooter_halfwrap\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"h3\", null, \"Company\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"/Home/AboutUs\"\n      }, \"About Us\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"/Home/EmploymentOpportunities\"\n      }, \"Opportunities\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"a\", {\n        href: \"/Home/PrivacyPolicy\"\n      }, \"Privacy Policy\")))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"footerbottomwrap\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"h2\", null, \"\\xA9 2019 USCRE ONLINE INC - ALL RIGHTS RESERVED\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"alignrighttext\"\n      }, \"USCRE is Wholly Owned by USC Global Holdings, Inc.\", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"br\", null), \"USCRE is Designed and Maintained by Decision Management Technologies, LLC\")));\n    }\n  }]);\n\n  return FooterMain;\n}(react__WEBPACK_IMPORTED_MODULE_0__[\"Component\"]); // const imgUrls = [\n// ];\n// for (var i = 1; i < 59; i++) {\n//     imgUrls.push(\"http://d34jlwp7y45ovp.cloudfront.net/footericons/\" + i + \".png\");\n// }\n// console.log(imgUrls);\n\n\nvar Carousel = /*#__PURE__*/function (_React$Component) {\n  _inherits(Carousel, _React$Component);\n\n  var _super2 = _createSuper(Carousel);\n\n  function Carousel(props) {\n    var _this2;\n\n    _classCallCheck(this, Carousel);\n\n    _this2 = _super2.call(this, props);\n    _this2.state = {\n      currentImageIndex: 0,\n      intervalId: null\n    };\n    _this2.nextSlide = _this2.nextSlide.bind(_assertThisInitialized(_this2));\n    _this2.previousSlide = _this2.previousSlide.bind(_assertThisInitialized(_this2));\n    _this2.nextBig = _this2.nextBig.bind(_assertThisInitialized(_this2));\n    _this2.mouseOver = _this2.mouseOver.bind(_assertThisInitialized(_this2));\n    _this2.mouseLeave = _this2.mouseLeave.bind(_assertThisInitialized(_this2));\n    return _this2;\n  }\n\n  _createClass(Carousel, [{\n    key: \"previousSlide\",\n    value: function previousSlide() {\n      var lastIndex = APP_pages_nav_partners__WEBPACK_IMPORTED_MODULE_2__[\"default\"].length - 1;\n      var currentImageIndex = this.state.currentImageIndex;\n      var shouldResetIndex = currentImageIndex === 0;\n      var index = shouldResetIndex ? lastIndex : currentImageIndex - 1;\n      this.setState({\n        currentImageIndex: index\n      });\n    }\n  }, {\n    key: \"nextSlide\",\n    value: function nextSlide() {\n      var lastIndex = APP_pages_nav_partners__WEBPACK_IMPORTED_MODULE_2__[\"default\"].length - 1;\n      var currentImageIndex = this.state.currentImageIndex;\n      var shouldResetIndex = currentImageIndex === lastIndex;\n      var index = shouldResetIndex ? 0 : currentImageIndex + 1;\n      this.setState({\n        currentImageIndex: index\n      });\n    }\n  }, {\n    key: \"nextBig\",\n    value: function nextBig() {\n      var lastIndex = APP_pages_nav_partners__WEBPACK_IMPORTED_MODULE_2__[\"default\"].length - 1;\n      var currentImageIndex = this.state.currentImageIndex;\n      var shouldResetIndex = currentImageIndex === lastIndex;\n      var indexthree = currentImageIndex + 1;\n      var index;\n\n      if (!APP_pages_nav_partners__WEBPACK_IMPORTED_MODULE_2__[\"default\"][indexthree]) {\n        index = 0;\n      } else {\n        index = shouldResetIndex ? 0 : currentImageIndex + 1;\n      }\n\n      this.setState({\n        currentImageIndex: index\n      });\n    }\n  }, {\n    key: \"componentDidMount\",\n    value: function componentDidMount() {\n      var interval = setInterval(this.nextBig, 1500);\n      this.setState({\n        intervalId: interval\n      });\n    }\n  }, {\n    key: \"mouseOver\",\n    value: function mouseOver() {\n      clearInterval(this.state.intervalId);\n    }\n  }, {\n    key: \"mouseLeave\",\n    value: function mouseLeave() {\n      var interval = setInterval(this.nextBig, 1500);\n      this.setState({\n        currentImageIndex: this.state.currentImageIndex + 1,\n        intervalId: interval\n      });\n    }\n  }, {\n    key: \"render\",\n    value: function render() {\n      var imgarr = [];\n\n      for (var i = 0; i < 4; i++) {\n        if (APP_pages_nav_partners__WEBPACK_IMPORTED_MODULE_2__[\"default\"][this.state.currentImageIndex + i]) {\n          imgarr.push(APP_pages_nav_partners__WEBPACK_IMPORTED_MODULE_2__[\"default\"][this.state.currentImageIndex + i]);\n        }\n      }\n\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"carousel\",\n        onMouseEnter: this.mouseOver,\n        onMouseLeave: this.mouseLeave\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"h5\", null, \"USCRE is proud to be a member of these trusted industry organizations\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Arrow, {\n        direction: \"left\",\n        clickFunction: this.previousSlide,\n        glyph: \"\\u25C0\"\n      }), imgarr.map(function (item) {\n        if (item.imglink && item.sitelink) {\n          return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(ImageSlide, {\n            key: item.key,\n            url: item.imglink,\n            link: item.sitelink\n          });\n        }\n      }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Arrow, {\n        direction: \"right\",\n        clickFunction: this.nextSlide,\n        glyph: \"\\u25B6\"\n      })));\n    }\n  }]);\n\n  return Carousel;\n}(react__WEBPACK_IMPORTED_MODULE_0___default.a.Component);\n\nvar Arrow = function Arrow(_ref) {\n  var direction = _ref.direction,\n      clickFunction = _ref.clickFunction,\n      glyph = _ref.glyph;\n  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n    className: \"slide-arrow \".concat(direction),\n    onClick: clickFunction\n  }, glyph);\n};\n\nvar ImageSlide = /*#__PURE__*/function (_React$Component2) {\n  _inherits(ImageSlide, _React$Component2);\n\n  var _super3 = _createSuper(ImageSlide);\n\n  function ImageSlide(props) {\n    var _this3;\n\n    _classCallCheck(this, ImageSlide);\n\n    _this3 = _super3.call(this, props);\n    _this3.state = {};\n    _this3.goPartner = _this3.goPartner.bind(_assertThisInitialized(_this3));\n    return _this3;\n  }\n\n  _createClass(ImageSlide, [{\n    key: \"goPartner\",\n    value: function goPartner(url) {\n      window.open(url, '_blank');\n    }\n  }, {\n    key: \"render\",\n    value: function render() {\n      var _this4 = this;\n\n      var styles = {\n        backgroundImage: \"url(\".concat(this.props.url, \")\"),\n        backgroundSize: 'contain',\n        backgroundRepeat: 'no-repeat',\n        backgroundPosition: 'center',\n        marginLeft: \"4px\",\n        marginRight: \"4px\",\n        height: '70%'\n      };\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"image-slide animated slideInRight faster\",\n        style: styles,\n        onClick: function onClick() {\n          return _this4.goPartner(_this4.props.link);\n        },\n        key: Math.random()\n      });\n    }\n  }]);\n\n  return ImageSlide;\n}(react__WEBPACK_IMPORTED_MODULE_0___default.a.Component);\n\nreact_dom__WEBPACK_IMPORTED_MODULE_1___default.a.render( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(FooterMain, null), document.getElementById('footermain'));\n\n//# sourceURL=webpack:///./pages/nav/footer.js?");

/***/ })

/******/ });