import React, { Component } from "react"
import ReactDOM from 'react-dom';
const PubSub = require('pubsub-js');
import *  as Util from '../../pages/asset-search/assetSearchUtils';
const util = Util.util;

import { LoadingComponent } from '../../loading-component'

export class CommissionCalculatorComponent extends Component {
  constructor(props) {
    super(props);
    const self = this;

    this.state = {
      isLoading: true,
      portfolioMarketValue: "$10,000,000",
      realtorCommissionRateValue: "5%",
    }
  }

  componentDidMount() {
    const that = this;
    this.refresh();

    window.saveText = "SAVE";
    window.commissionFlashDelay_ms = 3000;
    window.showingSaveText = false;
    window.value = 0;

    $(document).ready(function () {
      setListeners();
      SavingsCalc();
      setInterval(function () {
        realtorCommissionFlash();
      }, window.commissionFlashDelay_ms);
    });

    function setListeners() {
      $("#pValue").blur(function () {
        SavingsCalc();
      });

      $("#rComRate").blur(function () {
        SavingsCalc();
      });

      $("#pValue").on('change keyup paste', (e) => {
        util.onMoneyChange(e);
        document.getElementById("pValue").value = "$" + document.getElementById("pValue").value;
        that.setState({ portfolioMarketValue: document.getElementById("pValue").value });
      });

      $("#rComRate").change(function () {
        toPercent(document.getElementById("rComRate"), true);
        that.setState({ realtorCommissionRateValue: document.getElementById("rComRate").value });
      });
    }

    // Calculate savings on keypress Enter of Portfolio Market Value field and Realtor Commission Rate
    $("#pValue").keypress(function (e) {
      if (e.keyCode == 13) {
        SavingsCalc();
      }
    });
    $("#rComRate").keypress(function (e) {
      if (e.keyCode == 13) {
        SavingsCalc();
      }
    });

    // Switches the "USCRE Members Save" between showing value and "SAVE"
    function realtorCommissionFlash() {
      if (!showingSaveText) {
        $('#uSave').val(saveText);
        window.showingSaveText = true;
      } else {
        window.showingSaveText = false;
        $('#uSave').val(value);
        toMoney(document.getElementById("uSave"), true);
      }
    }

    function SavingsCalc() {
      window.enableFlashing = false;
      toPercent(document.getElementById("rComRate"), true);
      var propValue = $('#pValue').val();
      var comRate = $('#rComRate').val();
      var comValue = $('#rCommi').val();
      var saveAmt = $('#uSave').val();


      if (!checkNumber(propValue, 10000, 10000000000) ||
        !checkNumber(comRate, 0, 10000000000)) {
        comValue.value = "0";
        saveAmt.value = "0";
        return;
      }

      var val1 = getNumberValue(propValue);
      var val2 = getNumberValue(comRate);
      var val3 = ((val1 * val2) / 100)
      var val4 = val1 / 100;
      var val5 = val3 - val4;

      $('#rCommi').val(val3);
      $('#uSave').val(val5);
      $('#uFee').val(val4);

      toMoney(document.getElementById("pValue"), true);
      toMoney(document.getElementById("rCommi"), true);
      toMoney(document.getElementById("uSave"), true);
      toMoney(document.getElementById("uFee"), true);

      window.value = val5;
    }

    function toMoney(input, addDollarSign) {
      if (input.value != null && input.value.length != 0) {
        var sign = (addDollarSign) ? '$' : '';
        let num = getNumber(input);
        if (num == null) {
          return;
        }

        let str = num.toString();
        let sig = str.split('.');

        let tmp = '';
        let len = sig[0].length;
        for (var i = len, j = 1; i > 0; i--, j++) {
          var t = sig[0].substring(i, i - 1);
          tmp = t + tmp;
          if ((j % 3 == 0) && j != len) {
            tmp = ',' + tmp;
          }
        }

        if (sig.length > 1 && sig[1].length) {
          tmp += '.' + sig[1].substr(0, 1);
          var t = sig[1].substr(1, 1);
          if (t) {
            tmp += t;
          } else {
            tmp += '0';
          }
        }
        str = sign + tmp;

        input.value = str;
      }
    }

    function toPercent(input, addPercentSign) {
      if (input.value != null && input.value.length != 0) {
        var sign = (addPercentSign) ? '%' : '';
        let num = getNumber(input);

        if (num == null) {
          return;
        }

        let str = num.toString();
        let sig = str.split('.');

        let tmp = '';
        let len = sig[0].length;
        for (var i = len, j = 1; i > 0; i--, j++) {
          var t = sig[0].substring(i, i - 1);
          tmp = t + tmp;
          if ((j % 3 == 0) && j != len) {
            tmp = ',' + tmp;
          }
        }

        if (sig.length > 1 && sig[1].length) {
          tmp += '.' + sig[1].substr(0, 1);
          var t = sig[1].substr(1, 1);
          if (t) {
            tmp += t;
          } else {
            tmp += '0';
          }
        }
        str = tmp + sign;

        input.value = str;
      }
    }

    function checkNumber(fld, min, max) {
      let num = getNumberValue(fld);
      if (num == null) {
        return false;
      }
      if (num < min || max < num) {
        return false;
      }

      return true;
    }

    function getNumber(fld) {
      var str = fld.value;
      var tmp = '';

      if (fld.value.length == 0) {
        return null;
      }

      for (var i = 0; i < str.length; i++) {
        var ch = str.substring(i, i + 1);
        if (ch == '$' || ch == ',' || ch == '%' || ((ch < '0' || ch > '9') && ch != '.')) {
          continue;
        }
        tmp += ch;
      }

      if (tmp == '') {
        return null;
      }

      var num = parseFloat(tmp)

      return num;
    }

    function getNumberValue(fld) {
      var str;
      var tmp = '';

      if (fld.value == null) {
        // Sometimes when this function is called, it is already being sent the value
        str = fld;
      }
      else {
        // Other times, we need to extract the value
        str = fld.value;
      }

      if (str.length == 0) {
        return null;
      }

      for (var i = 0; i < str.length; i++) {
        var ch = str.substring(i, i + 1);
        if (ch == '$' || ch == ',' || ((ch < '0' || ch > '9') && ch != '.')) {
          continue;
        }
        tmp += ch;
      }

      if (tmp == '') {
        return null;
      }

      var num = parseFloat(tmp)

      return num;
    }

    $(function () {
      $.ajax({
        url: "/home/ValidateSiteAuth", success: function (result) {
          if (result != "True") $("#siteauth").modal();
        }
      });
    })
    let validauth = function () {
      var u = $('input[name=Username]').val();
      var p = $('input[name=Password]').val();

      $('#errmsg').val('');
      $.ajax({
        url: "/home/AuthSubmit",
        data: { 'Username': u, 'Password': p },
        success: function (result) {
          if (result == "True") {
            $("#siteauth").modal("hide");

          } else {
            $('#errmsg').html('<br/>&nbsp;Invalid UserName or Password');
            $('input[name=Username]').val('');
            $('input[name=Password]').val('');

            $('input[name=Username]').focus();
            var refreshIntervalId = setInterval(function () { $('#errmsg').fadeToggle(); }, 300);
            setTimeout(function () {
              clearInterval(refreshIntervalId);
              $('#errmsg').html('')
            }, 3000);
          }
        }
      });
    }
  }

  handlePortfolioMarketValueChange(event) {
    util.onMoneyChange(event);
    this.setState({ portfolioMarketValue: event.target.value })
  }

  handleRealtorCommissionRateChange(event) {
    this.setState({ realtorCommissionRateValue: event.target.value })
  }

  async refresh() {
    const self = this;
    self.setState({ isLoading: false });

  }

  render() {
    return (
      <div className="modal fade" id="commissionCalculator" tabIndex="-1" role="dialog" aria-labelledby="commissionCalculator-label" aria-hidden="true" data-backdrop="false">
        <div className="modal-dialog modal-lg">
          <div className="modal-content">
            <div className="modal-header">
              <button type="button" className="close" data-dismiss="modal" aria-hidden="true">&times;</button>
              <h4 className="modal-title">Commission Saving Calculator</h4>
            </div>
            <div className="modal-body">
              <React.Fragment>
                <div className={!this.state.isLoading ? 'hidden' : null}>
                  <LoadingComponent />
                </div>

                <h4 id="tbAssets" style={{ textAlign: 'center', fontWeight: 'bold' }}>Increase Liquidity and Equity in your CRE Portfolio…with one click</h4>
                <h3 style={{ textAlign: 'center', fontWeight: 'bold' }}>Commission Savings Calculator</h3>
                <span tabIndex="1" onFocus={this.handleStartTabFocus}></span>
                <table className="table" style={{ fontSize: '9px !important', verticalAlign: 'middle' }}>
                  <thead>
                    <tr>
                      <td style={{ width: '20%', textAlign: 'center', fontSize: '14px' }}>
                        <b>Portfolio Market Value</b>
                      </td>
                      <td style={{ width: '20%', textAlign: 'center', fontSize: '14px' }}>
                        <b>Realtor Commission Rate</b>
                      </td>
                      <td style={{ width: '20%', textAlign: 'center', fontSize: '14px' }}>
                        <b>Realtor Commission</b>
                      </td>
                      <td style={{ width: '20%', textAlign: 'center', fontSize: '14px' }}>
                        <b>USCRE Fee</b>
                      </td>
                      <td style={{ width: '20%', textAlign: 'center', fontSize: '14px' }}>
                        <b>USCRE Members Save</b>
                      </td>
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td>
                        <input className="autofocus1" style={{ width: '100%', height: '50px', border: 'solid 2px #2f0a29', borderRadius: '8px', fontSize: '18px', color: '#191919', textAlign: 'center', position: 'relative', display: 'inline-block', padding: '31px 5px' }} name="pValue" id="pValue" type="text" value={this.state.portfolioMarketValue} onBlur={this.SavingsCalc} tabIndex="2" onChange={this.handlePortfolioMarketValueChange.bind(this)} onFocus={this.handleFocus} />
                      </td>
                      <td>
                        <input style={{ width: '100%', height: '50px', border: 'solid 2px #2f0a29', borderRadius: '8px', fontSize: '18px', color: '#191919', textAlign: 'center', position: 'relative', display: 'inline-block', padding: '31px 5px' }} name="rComRate" id="rComRate" type="text" value={this.state.realtorCommissionRateValue} onBlur={this.SavingsCalc} tabIndex="3" onChange={this.handleRealtorCommissionRateChange.bind(this)} />
                      </td>
                      <td>
                        <input style={{ width: '100% ', height: '50px', color: '#fff', backgroundColor: '#820a0a', borderColor: '#820a0a', textAlign: 'center', border: 'solid 2px ', borderRadius: '8px', fontSize: '18px', padding: '31px 5px' }} name="rCommi" id="rCommi" type="text" readOnly="readOnly" tabIndex="4" />
                      </td>
                      <td>
                        <input style={{ width: '100% ', height: '50px', color: '#fff', backgroundColor: '#3e7cbe', borderColor: '#3e7cbe', textAlign: 'center', border: 'solid 2px ', borderRadius: '8px', fontSize: '18px', padding: '31px 5px' }} name="uFee" id="uFee" type="text" readOnly="readOnly" tabIndex="5" />
                      </td>
                      <td>
                        <input className="table animated pulse infinite autofocus2" style={{ width: '100%', height: '50px', color: '#fff', backgroundColor: 'limegreen', borderColor: '#094a1e', textAlign: 'center', border: 'solid 2px', borderRadius: '8px', fontSize: '18px', padding: '31px 5px' }} name="uSave" id="uSave" type="text" readOnly="readOnly" tabIndex="6" />
                      </td>
                    </tr>
                  </tbody>
                </table>
                <em style={{ fontSize: 'smaller' }}>Enter your Portfolio value & realtor commission rate and see how much you will save with the USCRE Data Portal.  It’s a no-brainer!</em>
              </React.Fragment>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

const element = document.getElementById('commissioner-calculator');
if (element != null)
  ReactDOM.render(<CommissionCalculatorComponent />, element)