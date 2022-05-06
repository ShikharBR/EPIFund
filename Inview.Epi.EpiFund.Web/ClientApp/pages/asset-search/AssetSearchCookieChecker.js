export function getLocationByIp(cb) {
    $.ajax('http://ip-api.com/json')
        .then(
            function success(response) {
                console.log('User\'s Location Data is ', response);
                console.log('User\'s Country', response.country);
            },

            function fail(data, status) {
                console.log('Request failed.  Returned status of',
                    status);
            }
        );
}
export function setCookieFavoriteAsset(assetData) {
    var cookieName = "favoriteAssetData";
    document.cookie = cookieName + '=' + assetData + ';';
}
export function setCookieAssetSearch(locationData) {
    var cookieName = "asData";
    document.cookie = cookieName + '=' + locationData + ';';
}
export function getCookieAssetSearch(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
export function deleteCookieAssetSearch(cname) {
    document.cookie = cname + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
