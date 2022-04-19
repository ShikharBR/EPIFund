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
/******/ 		"topnav": 0
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
/******/ 	deferredModules.push(["./pages/nav/topnav.js","commons"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./pages/nav/topnav.js":
/*!*****************************!*\
  !*** ./pages/nav/topnav.js ***!
  \*****************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ \"./node_modules/react/index.js\");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ \"./node_modules/react-dom/index.js\");\n/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);\nfunction _typeof(obj) { \"@babel/helpers - typeof\"; return _typeof = \"function\" == typeof Symbol && \"symbol\" == typeof Symbol.iterator ? function (obj) { return typeof obj; } : function (obj) { return obj && \"function\" == typeof Symbol && obj.constructor === Symbol && obj !== Symbol.prototype ? \"symbol\" : typeof obj; }, _typeof(obj); }\n\nfunction _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError(\"Cannot call a class as a function\"); } }\n\nfunction _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if (\"value\" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }\n\nfunction _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, \"prototype\", { writable: false }); return Constructor; }\n\nfunction _inherits(subClass, superClass) { if (typeof superClass !== \"function\" && superClass !== null) { throw new TypeError(\"Super expression must either be null or a function\"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); Object.defineProperty(subClass, \"prototype\", { writable: false }); if (superClass) _setPrototypeOf(subClass, superClass); }\n\nfunction _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }\n\nfunction _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }\n\nfunction _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === \"object\" || typeof call === \"function\")) { return call; } else if (call !== void 0) { throw new TypeError(\"Derived constructors may only return object or undefined\"); } return _assertThisInitialized(self); }\n\nfunction _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError(\"this hasn't been initialised - super() hasn't been called\"); } return self; }\n\nfunction _isNativeReflectConstruct() { if (typeof Reflect === \"undefined\" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === \"function\") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }\n\nfunction _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }\n\n\n // Top Navigation\n\nvar TopNav = /*#__PURE__*/function (_React$Component) {\n  _inherits(TopNav, _React$Component);\n\n  var _super = _createSuper(TopNav);\n\n  function TopNav(props) {\n    var _this;\n\n    _classCallCheck(this, TopNav);\n\n    _this = _super.call(this, props);\n    _this.state = {\n      user: null\n    };\n    return _this;\n  }\n\n  _createClass(TopNav, [{\n    key: \"componentDidMount\",\n    value: function componentDidMount() {\n      var _this2 = this;\n\n      fetch('/api/userInfo/GetCurrentUserInfoAndProfile', {\n        method: 'post',\n        body: JSON.stringify()\n      }).then(function (response) {\n        return response.json();\n      }).then(function (data) {\n        if (!data.Message) {\n          _this2.setState({\n            user: data\n          });\n        } else {\n          _this2.setState({\n            user: null\n          });\n        }\n      });\n    }\n  }, {\n    key: \"render\",\n    value: function render() {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"topnavwrap\"\n      }, this.state.user ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(TopNavLoggedIn, {\n        user: this.state.user\n      }) : /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(TopNavLoggedOut, null));\n    }\n  }]);\n\n  return TopNav;\n}(react__WEBPACK_IMPORTED_MODULE_0___default.a.Component);\n\nvar TopNavLoggedIn = /*#__PURE__*/function (_React$Component2) {\n  _inherits(TopNavLoggedIn, _React$Component2);\n\n  var _super2 = _createSuper(TopNavLoggedIn);\n\n  function TopNavLoggedIn(props) {\n    var _this3;\n\n    _classCallCheck(this, TopNavLoggedIn);\n\n    _this3 = _super2.call(this, props);\n    _this3.state = {\n      showPop: false,\n      showPop2: false\n    };\n    _this3.togglePop = _this3.togglePop.bind(_assertThisInitialized(_this3));\n    _this3.togglePop2 = _this3.togglePop2.bind(_assertThisInitialized(_this3));\n    _this3.clickPop = _this3.clickPop.bind(_assertThisInitialized(_this3));\n    _this3.clickItem = _this3.clickItem.bind(_assertThisInitialized(_this3));\n    _this3.handleClickOutside = _this3.handleClickOutside.bind(_assertThisInitialized(_this3));\n    return _this3;\n  }\n\n  _createClass(TopNavLoggedIn, [{\n    key: \"togglePop\",\n    value: function togglePop() {\n      this.setState({\n        showPop: !this.state.showPop\n      });\n    }\n  }, {\n    key: \"togglePop2\",\n    value: function togglePop2() {\n      this.setState({\n        showPop2: !this.state.showPop2\n      });\n    }\n  }, {\n    key: \"componentWillMount\",\n    value: function componentWillMount() {\n      document.addEventListener('click', this.clickPop, false);\n    }\n  }, {\n    key: \"componentWillUnmount\",\n    value: function componentWillUnmount() {\n      document.removeEventListener('click', this.clickPop, false);\n    }\n  }, {\n    key: \"handleClickOutside\",\n    value: function handleClickOutside() {\n      this.setState({\n        showPop: false,\n        showPop2: false\n      });\n    }\n  }, {\n    key: \"clickPop\",\n    value: function clickPop(e, data) {\n      if (data) {\n        document.location.href = data.link;\n      }\n\n      if (this.pop1.contains(e.target)) {\n        this.togglePop();\n        return;\n      }\n\n      if (this.pop2.contains(e.target)) {\n        this.togglePop2();\n        return;\n      }\n\n      this.handleClickOutside();\n    }\n  }, {\n    key: \"clickItem\",\n    value: function clickItem(data) {\n      document.location.href = data.link;\n    }\n  }, {\n    key: \"render\",\n    value: function render() {\n      var _this4 = this;\n\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"userinfotopwrap\",\n        ref: function ref(node) {\n          return _this4.pop1 = node;\n        }\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        style: {\n          height: \"20px\",\n          fontSize: \"54px\",\n          lineHeight: \"38px\"\n        }\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"img\", {\n        alt: \"\",\n        src: \"https://s3-us-west-1.amazonaws.com/dev-uscreonline-content/arrowdown.png\",\n        style: {\n          marginTop: \"5px\"\n        }\n      })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", null, this.props.user != null && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"img\", {\n        alt: \"\",\n        src: this.props.user.ProfileImageUrl\n      })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", null, \"Welcome\", this.props.user != null && \", \" + this.props.user.User.FullName), this.state.showPop && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopdown\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopitem\",\n        onMouseDown: function onMouseDown(e) {\n          return _this4.clickPop(e, {\n            link: '/Home/EditProfile'\n          });\n        }\n      }, \"Edit Profile\"), this.props.user != null && this.props.user.User.UserType === 14 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopitem\",\n        onMouseDown: function onMouseDown(e) {\n          return _this4.clickPop(e, {\n            link: '/General/UserProfile'\n          });\n        }\n      }, \"Edit SP Profile\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopitem\",\n        onMouseDown: function onMouseDown(e) {\n          return _this4.clickPop(e, {\n            link: '/Home/ChangePassword'\n          });\n        }\n      }, \"Account Settings\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopitem\",\n        onMouseDown: function onMouseDown(e) {\n          return _this4.clickPop(e, {\n            link: '/Home/RegistrantRecords'\n          });\n        }\n      }, \"Registrant Records\"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"topnavbuttonwrap\",\n        ref: this.setWrapperRef\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"img\", {\n        alt: \"\",\n        src: \"https://i.imgur.com/bVEpvrv.png\",\n        ref: function ref(node) {\n          return _this4.pop2 = node;\n        }\n      }), this.state.showPop2 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopdown\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopitem\",\n        onMouseDown: function onMouseDown(e) {\n          return _this4.clickPop(e, {\n            link: '/General/CommunicationCenter'\n          });\n        }\n      }, \"Communication Center\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopitem\",\n        onMouseDown: function onMouseDown(e) {\n          return _this4.clickPop(e, {\n            link: '/General/UserContacts'\n          });\n        }\n      }, \"My Contacts\")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"topnavbutton\",\n        onClick: function onClick() {\n          return _this4.clickItem({\n            link: '/Home/Logout'\n          });\n        }\n      }, \"Logout\")));\n    }\n  }]);\n\n  return TopNavLoggedIn;\n}(react__WEBPACK_IMPORTED_MODULE_0___default.a.Component);\n\nvar TopNavLoggedOut = /*#__PURE__*/function (_React$Component3) {\n  _inherits(TopNavLoggedOut, _React$Component3);\n\n  var _super3 = _createSuper(TopNavLoggedOut);\n\n  function TopNavLoggedOut(props) {\n    var _this5;\n\n    _classCallCheck(this, TopNavLoggedOut);\n\n    _this5 = _super3.call(this, props);\n    _this5.state = {\n      user: {},\n      showPop: false\n    };\n    _this5.openPage = _this5.openPage.bind(_assertThisInitialized(_this5));\n    _this5.togglePop = _this5.togglePop.bind(_assertThisInitialized(_this5));\n    _this5.clickItem = _this5.clickItem.bind(_assertThisInitialized(_this5));\n    return _this5;\n  }\n\n  _createClass(TopNavLoggedOut, [{\n    key: \"openPage\",\n    value: function openPage(link) {\n      document.location.href = link;\n    }\n  }, {\n    key: \"togglePop\",\n    value: function togglePop() {\n      this.setState({\n        showPop: !this.state.showPop\n      });\n    }\n  }, {\n    key: \"clickItem\",\n    value: function clickItem(data) {\n      if (data.link) {\n        document.location.href = data.link;\n      }\n    }\n  }, {\n    key: \"render\",\n    value: function render() {\n      var _this6 = this;\n\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", null, \"Please Login or Register\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"topnavbutton\",\n        onClick: function onClick() {\n          return _this6.openPage('/Home/Login');\n        }\n      }, \"Login\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"topnavbutton\",\n        onMouseEnter: this.togglePop,\n        onMouseLeave: this.togglePop\n      }, \"Register\", this.state.showPop && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopdown\",\n        onMouseDown: this.clickItem\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopitem\",\n        onClick: function onClick() {\n          return _this6.clickItem({\n            link: '/Home/RegistrationIntro'\n          });\n        }\n      }, \"Principal Investor\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopitem\",\n        onClick: function onClick() {\n          return _this6.clickItem({\n            link: '/Home/JointVentureMarketing'\n          });\n        }\n      }, \"Service Provider\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"minipopitem\",\n        onClick: function onClick() {\n          return _this6.clickItem({\n            link: '/Home/EmploymentOpportunities'\n          });\n        }\n      }, \"Independent Contractor\")))));\n    }\n  }]);\n\n  return TopNavLoggedOut;\n}(react__WEBPACK_IMPORTED_MODULE_0___default.a.Component);\n\nreact_dom__WEBPACK_IMPORTED_MODULE_1___default.a.render( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(TopNav, null), document.getElementById('topnav'));\n\n//# sourceURL=webpack:///./pages/nav/topnav.js?");

/***/ })

/******/ });