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
/******/ 		"commissioner-calculator": 0
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
/******/ 	deferredModules.push(["./components/commission-calculator/commission-calculator.js","commons"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./components/commission-calculator/commission-calculator.js":
/*!*******************************************************************!*\
  !*** ./components/commission-calculator/commission-calculator.js ***!
  \*******************************************************************/
/*! exports provided: CommissionCalculatorComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"CommissionCalculatorComponent\", function() { return CommissionCalculatorComponent; });\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ \"./node_modules/react/index.js\");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ \"./node_modules/react-dom/index.js\");\n/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var _pages_asset_search_assetSearchUtils__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../pages/asset-search/assetSearchUtils */ \"./pages/asset-search/assetSearchUtils.js\");\n/* harmony import */ var _loading_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../loading-component */ \"./loading-component.js\");\nfunction _typeof(obj) { \"@babel/helpers - typeof\"; return _typeof = \"function\" == typeof Symbol && \"symbol\" == typeof Symbol.iterator ? function (obj) { return typeof obj; } : function (obj) { return obj && \"function\" == typeof Symbol && obj.constructor === Symbol && obj !== Symbol.prototype ? \"symbol\" : typeof obj; }, _typeof(obj); }\n\nfunction asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) { try { var info = gen[key](arg); var value = info.value; } catch (error) { reject(error); return; } if (info.done) { resolve(value); } else { Promise.resolve(value).then(_next, _throw); } }\n\nfunction _asyncToGenerator(fn) { return function () { var self = this, args = arguments; return new Promise(function (resolve, reject) { var gen = fn.apply(self, args); function _next(value) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, \"next\", value); } function _throw(err) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, \"throw\", err); } _next(undefined); }); }; }\n\nfunction _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError(\"Cannot call a class as a function\"); } }\n\nfunction _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if (\"value\" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }\n\nfunction _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, \"prototype\", { writable: false }); return Constructor; }\n\nfunction _inherits(subClass, superClass) { if (typeof superClass !== \"function\" && superClass !== null) { throw new TypeError(\"Super expression must either be null or a function\"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); Object.defineProperty(subClass, \"prototype\", { writable: false }); if (superClass) _setPrototypeOf(subClass, superClass); }\n\nfunction _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }\n\nfunction _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }\n\nfunction _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === \"object\" || typeof call === \"function\")) { return call; } else if (call !== void 0) { throw new TypeError(\"Derived constructors may only return object or undefined\"); } return _assertThisInitialized(self); }\n\nfunction _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError(\"this hasn't been initialised - super() hasn't been called\"); } return self; }\n\nfunction _isNativeReflectConstruct() { if (typeof Reflect === \"undefined\" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === \"function\") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }\n\nfunction _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }\n\n\n\n\nvar PubSub = __webpack_require__(/*! pubsub-js */ \"./node_modules/pubsub-js/src/pubsub.js\");\n\n\nvar util = _pages_asset_search_assetSearchUtils__WEBPACK_IMPORTED_MODULE_2__[\"util\"];\n\nvar CommissionCalculatorComponent = /*#__PURE__*/function (_Component) {\n  _inherits(CommissionCalculatorComponent, _Component);\n\n  var _super = _createSuper(CommissionCalculatorComponent);\n\n  function CommissionCalculatorComponent(props) {\n    var _this;\n\n    _classCallCheck(this, CommissionCalculatorComponent);\n\n    _this = _super.call(this, props);\n\n    var self = _assertThisInitialized(_this);\n\n    _this.state = {\n      isLoading: true,\n      portfolioMarketValue: \"$10,000,000\",\n      realtorCommissionRateValue: \"5%\"\n    };\n    return _this;\n  }\n\n  _createClass(CommissionCalculatorComponent, [{\n    key: \"componentDidMount\",\n    value: function componentDidMount() {\n      var that = this;\n      this.refresh();\n      window.saveText = \"SAVE\";\n      window.commissionFlashDelay_ms = 3000;\n      window.showingSaveText = false;\n      window.value = 0;\n      $(document).ready(function () {\n        setListeners();\n        SavingsCalc();\n        setInterval(function () {\n          realtorCommissionFlash();\n        }, window.commissionFlashDelay_ms);\n      });\n\n      function setListeners() {\n        $(\"#pValue\").blur(function () {\n          SavingsCalc();\n        });\n        $(\"#rComRate\").blur(function () {\n          SavingsCalc();\n        });\n        $(\"#pValue\").on('change keyup paste', function (e) {\n          util.onMoneyChange(e);\n          document.getElementById(\"pValue\").value = \"$\" + document.getElementById(\"pValue\").value;\n          that.setState({\n            portfolioMarketValue: document.getElementById(\"pValue\").value\n          });\n        });\n        $(\"#rComRate\").change(function () {\n          toPercent(document.getElementById(\"rComRate\"), true);\n          that.setState({\n            realtorCommissionRateValue: document.getElementById(\"rComRate\").value\n          });\n        });\n      } // Calculate savings on keypress Enter of Portfolio Market Value field and Realtor Commission Rate\n\n\n      $(\"#pValue\").keypress(function (e) {\n        if (e.keyCode == 13) {\n          SavingsCalc();\n        }\n      });\n      $(\"#rComRate\").keypress(function (e) {\n        if (e.keyCode == 13) {\n          SavingsCalc();\n        }\n      }); // Switches the \"USCRE Members Save\" between showing value and \"SAVE\"\n\n      function realtorCommissionFlash() {\n        if (!showingSaveText) {\n          $('#uSave').val(saveText);\n          window.showingSaveText = true;\n        } else {\n          window.showingSaveText = false;\n          $('#uSave').val(value);\n          toMoney(document.getElementById(\"uSave\"), true);\n        }\n      }\n\n      function SavingsCalc() {\n        window.enableFlashing = false;\n        toPercent(document.getElementById(\"rComRate\"), true);\n        var propValue = $('#pValue').val();\n        var comRate = $('#rComRate').val();\n        var comValue = $('#rCommi').val();\n        var saveAmt = $('#uSave').val();\n\n        if (!checkNumber(propValue, 10000, 10000000000) || !checkNumber(comRate, 0, 10000000000)) {\n          comValue.value = \"0\";\n          saveAmt.value = \"0\";\n          return;\n        }\n\n        var val1 = getNumberValue(propValue);\n        var val2 = getNumberValue(comRate);\n        var val3 = val1 * val2 / 100;\n        var val4 = val1 / 100;\n        var val5 = val3 - val4;\n        $('#rCommi').val(val3);\n        $('#uSave').val(val5);\n        $('#uFee').val(val4);\n        toMoney(document.getElementById(\"pValue\"), true);\n        toMoney(document.getElementById(\"rCommi\"), true);\n        toMoney(document.getElementById(\"uSave\"), true);\n        toMoney(document.getElementById(\"uFee\"), true);\n        window.value = val5;\n      }\n\n      function toMoney(input, addDollarSign) {\n        if (input.value != null && input.value.length != 0) {\n          var sign = addDollarSign ? '$' : '';\n          var num = getNumber(input);\n\n          if (num == null) {\n            return;\n          }\n\n          var str = num.toString();\n          var sig = str.split('.');\n          var tmp = '';\n          var len = sig[0].length;\n\n          for (var i = len, j = 1; i > 0; i--, j++) {\n            var t = sig[0].substring(i, i - 1);\n            tmp = t + tmp;\n\n            if (j % 3 == 0 && j != len) {\n              tmp = ',' + tmp;\n            }\n          }\n\n          if (sig.length > 1 && sig[1].length) {\n            tmp += '.' + sig[1].substr(0, 1);\n            var t = sig[1].substr(1, 1);\n\n            if (t) {\n              tmp += t;\n            } else {\n              tmp += '0';\n            }\n          }\n\n          str = sign + tmp;\n          input.value = str;\n        }\n      }\n\n      function toPercent(input, addPercentSign) {\n        if (input.value != null && input.value.length != 0) {\n          var sign = addPercentSign ? '%' : '';\n          var num = getNumber(input);\n\n          if (num == null) {\n            return;\n          }\n\n          var str = num.toString();\n          var sig = str.split('.');\n          var tmp = '';\n          var len = sig[0].length;\n\n          for (var i = len, j = 1; i > 0; i--, j++) {\n            var t = sig[0].substring(i, i - 1);\n            tmp = t + tmp;\n\n            if (j % 3 == 0 && j != len) {\n              tmp = ',' + tmp;\n            }\n          }\n\n          if (sig.length > 1 && sig[1].length) {\n            tmp += '.' + sig[1].substr(0, 1);\n            var t = sig[1].substr(1, 1);\n\n            if (t) {\n              tmp += t;\n            } else {\n              tmp += '0';\n            }\n          }\n\n          str = tmp + sign;\n          input.value = str;\n        }\n      }\n\n      function checkNumber(fld, min, max) {\n        var num = getNumberValue(fld);\n\n        if (num == null) {\n          return false;\n        }\n\n        if (num < min || max < num) {\n          return false;\n        }\n\n        return true;\n      }\n\n      function getNumber(fld) {\n        var str = fld.value;\n        var tmp = '';\n\n        if (fld.value.length == 0) {\n          return null;\n        }\n\n        for (var i = 0; i < str.length; i++) {\n          var ch = str.substring(i, i + 1);\n\n          if (ch == '$' || ch == ',' || ch == '%' || (ch < '0' || ch > '9') && ch != '.') {\n            continue;\n          }\n\n          tmp += ch;\n        }\n\n        if (tmp == '') {\n          return null;\n        }\n\n        var num = parseFloat(tmp);\n        return num;\n      }\n\n      function getNumberValue(fld) {\n        var str;\n        var tmp = '';\n\n        if (fld.value == null) {\n          // Sometimes when this function is called, it is already being sent the value\n          str = fld;\n        } else {\n          // Other times, we need to extract the value\n          str = fld.value;\n        }\n\n        if (str.length == 0) {\n          return null;\n        }\n\n        for (var i = 0; i < str.length; i++) {\n          var ch = str.substring(i, i + 1);\n\n          if (ch == '$' || ch == ',' || (ch < '0' || ch > '9') && ch != '.') {\n            continue;\n          }\n\n          tmp += ch;\n        }\n\n        if (tmp == '') {\n          return null;\n        }\n\n        var num = parseFloat(tmp);\n        return num;\n      }\n\n      $(function () {\n        $.ajax({\n          url: \"/home/ValidateSiteAuth\",\n          success: function success(result) {\n            if (result != \"True\") $(\"#siteauth\").modal();\n          }\n        });\n      });\n\n      var validauth = function validauth() {\n        var u = $('input[name=Username]').val();\n        var p = $('input[name=Password]').val();\n        $('#errmsg').val('');\n        $.ajax({\n          url: \"/home/AuthSubmit\",\n          data: {\n            'Username': u,\n            'Password': p\n          },\n          success: function success(result) {\n            if (result == \"True\") {\n              $(\"#siteauth\").modal(\"hide\");\n            } else {\n              $('#errmsg').html('<br/>&nbsp;Invalid UserName or Password');\n              $('input[name=Username]').val('');\n              $('input[name=Password]').val('');\n              $('input[name=Username]').focus();\n              var refreshIntervalId = setInterval(function () {\n                $('#errmsg').fadeToggle();\n              }, 300);\n              setTimeout(function () {\n                clearInterval(refreshIntervalId);\n                $('#errmsg').html('');\n              }, 3000);\n            }\n          }\n        });\n      };\n    }\n  }, {\n    key: \"handlePortfolioMarketValueChange\",\n    value: function handlePortfolioMarketValueChange(event) {\n      util.onMoneyChange(event);\n      this.setState({\n        portfolioMarketValue: event.target.value\n      });\n    }\n  }, {\n    key: \"handleRealtorCommissionRateChange\",\n    value: function handleRealtorCommissionRateChange(event) {\n      this.setState({\n        realtorCommissionRateValue: event.target.value\n      });\n    }\n  }, {\n    key: \"refresh\",\n    value: function () {\n      var _refresh = _asyncToGenerator( /*#__PURE__*/regeneratorRuntime.mark(function _callee() {\n        var self;\n        return regeneratorRuntime.wrap(function _callee$(_context) {\n          while (1) {\n            switch (_context.prev = _context.next) {\n              case 0:\n                self = this;\n                self.setState({\n                  isLoading: false\n                });\n\n              case 2:\n              case \"end\":\n                return _context.stop();\n            }\n          }\n        }, _callee, this);\n      }));\n\n      function refresh() {\n        return _refresh.apply(this, arguments);\n      }\n\n      return refresh;\n    }()\n  }, {\n    key: \"render\",\n    value: function render() {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"modal fade\",\n        id: \"commissionCalculator\",\n        tabIndex: \"-1\",\n        role: \"dialog\",\n        \"aria-labelledby\": \"commissionCalculator-label\",\n        \"aria-hidden\": \"true\",\n        \"data-backdrop\": \"false\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"modal-dialog modal-lg\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"modal-content\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"modal-header\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"button\", {\n        type: \"button\",\n        className: \"close\",\n        \"data-dismiss\": \"modal\",\n        \"aria-hidden\": \"true\"\n      }, \"\\xD7\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"h4\", {\n        className: \"modal-title\"\n      }, \"Commission Saving Calculator\")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: \"modal-body\"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"div\", {\n        className: !this.state.isLoading ? 'hidden' : null\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_loading_component__WEBPACK_IMPORTED_MODULE_3__[\"LoadingComponent\"], null)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"h4\", {\n        id: \"tbAssets\",\n        style: {\n          textAlign: 'center',\n          fontWeight: 'bold'\n        }\n      }, \"Increase Liquidity and Equity in your CRE Portfolio\\u2026with one click\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"h3\", {\n        style: {\n          textAlign: 'center',\n          fontWeight: 'bold'\n        }\n      }, \"Commission Savings Calculator\"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"span\", {\n        tabIndex: \"1\",\n        onFocus: this.handleStartTabFocus\n      }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"table\", {\n        className: \"table\",\n        style: {\n          fontSize: '9px !important',\n          verticalAlign: 'middle'\n        }\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"thead\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"tr\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", {\n        style: {\n          width: '20%',\n          textAlign: 'center',\n          fontSize: '14px'\n        }\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"b\", null, \"Portfolio Market Value\")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", {\n        style: {\n          width: '20%',\n          textAlign: 'center',\n          fontSize: '14px'\n        }\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"b\", null, \"Realtor Commission Rate\")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", {\n        style: {\n          width: '20%',\n          textAlign: 'center',\n          fontSize: '14px'\n        }\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"b\", null, \"Realtor Commission\")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", {\n        style: {\n          width: '20%',\n          textAlign: 'center',\n          fontSize: '14px'\n        }\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"b\", null, \"USCRE Fee\")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", {\n        style: {\n          width: '20%',\n          textAlign: 'center',\n          fontSize: '14px'\n        }\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"b\", null, \"USCRE Members Save\")))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"tbody\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"tr\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"input\", {\n        className: \"autofocus1\",\n        style: {\n          width: '100%',\n          height: '50px',\n          border: 'solid 2px #2f0a29',\n          borderRadius: '8px',\n          fontSize: '18px',\n          color: '#191919',\n          textAlign: 'center',\n          position: 'relative',\n          display: 'inline-block',\n          padding: '31px 5px'\n        },\n        name: \"pValue\",\n        id: \"pValue\",\n        type: \"text\",\n        value: this.state.portfolioMarketValue,\n        onBlur: this.SavingsCalc,\n        tabIndex: \"2\",\n        onChange: this.handlePortfolioMarketValueChange.bind(this),\n        onFocus: this.handleFocus\n      })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"input\", {\n        style: {\n          width: '100%',\n          height: '50px',\n          border: 'solid 2px #2f0a29',\n          borderRadius: '8px',\n          fontSize: '18px',\n          color: '#191919',\n          textAlign: 'center',\n          position: 'relative',\n          display: 'inline-block',\n          padding: '31px 5px'\n        },\n        name: \"rComRate\",\n        id: \"rComRate\",\n        type: \"text\",\n        value: this.state.realtorCommissionRateValue,\n        onBlur: this.SavingsCalc,\n        tabIndex: \"3\",\n        onChange: this.handleRealtorCommissionRateChange.bind(this)\n      })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"input\", {\n        style: {\n          width: '100% ',\n          height: '50px',\n          color: '#fff',\n          backgroundColor: '#820a0a',\n          borderColor: '#820a0a',\n          textAlign: 'center',\n          border: 'solid 2px ',\n          borderRadius: '8px',\n          fontSize: '18px',\n          padding: '31px 5px'\n        },\n        name: \"rCommi\",\n        id: \"rCommi\",\n        type: \"text\",\n        readOnly: \"readOnly\",\n        tabIndex: \"4\"\n      })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"input\", {\n        style: {\n          width: '100% ',\n          height: '50px',\n          color: '#fff',\n          backgroundColor: '#3e7cbe',\n          borderColor: '#3e7cbe',\n          textAlign: 'center',\n          border: 'solid 2px ',\n          borderRadius: '8px',\n          fontSize: '18px',\n          padding: '31px 5px'\n        },\n        name: \"uFee\",\n        id: \"uFee\",\n        type: \"text\",\n        readOnly: \"readOnly\",\n        tabIndex: \"5\"\n      })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"td\", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"input\", {\n        className: \"table animated pulse infinite autofocus2\",\n        style: {\n          width: '100%',\n          height: '50px',\n          color: '#fff',\n          backgroundColor: 'limegreen',\n          borderColor: '#094a1e',\n          textAlign: 'center',\n          border: 'solid 2px',\n          borderRadius: '8px',\n          fontSize: '18px',\n          padding: '31px 5px'\n        },\n        name: \"uSave\",\n        id: \"uSave\",\n        type: \"text\",\n        readOnly: \"readOnly\",\n        tabIndex: \"6\"\n      }))))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(\"em\", {\n        style: {\n          fontSize: 'smaller'\n        }\n      }, \"Enter your Portfolio value & realtor commission rate and see how much you will save with the USCRE Data Portal.  It\\u2019s a no-brainer!\"))))));\n    }\n  }]);\n\n  return CommissionCalculatorComponent;\n}(react__WEBPACK_IMPORTED_MODULE_0__[\"Component\"]);\nvar element = document.getElementById('commissioner-calculator');\nif (element != null) react_dom__WEBPACK_IMPORTED_MODULE_1___default.a.render( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(CommissionCalculatorComponent, null), element);\n\n//# sourceURL=webpack:///./components/commission-calculator/commission-calculator.js?");

/***/ })

/******/ });