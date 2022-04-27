var myUscService = {
  fetchAssets: function(searchCriteria) {
    // accepts an array of APNs, an address(with all the trimmings), asset name
  },
  getUserCompanies: function () {
    // accepts nothing, returns an array of operating company objects and an array of holding company objects
    return fetch('/Home/GetUserCompanies/', {
      method: 'GET',
      headers: { 'Content-Type': 'application/json; charset=utf-8' }
    }).then(response => response.json());
  },
  validateApn: function(apn) {
    // accepts an array of strings(APNs), returns an array of equal size and the status for each
  },
  claimAsset: function(obj) {
    // will evolve organically
  },
  createAsset: function(obj) {
    // will also evolve organically
  }
};
