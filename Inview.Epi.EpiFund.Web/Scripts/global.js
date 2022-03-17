function getPageNumber(url) {
    var array = url.split('page=');
    if (array.length === 2) {
        return array[1];
    }
    return null;
}
function clearSearchValues() {
    $('#Page').val(null);
    $('#SortOrder').val(null);
}
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
function SubmitFormPagination(input, id) {
    // this post isnt behaving as expected so im just adding this code to any page that needs it
    var href = $(input).attr('href');
    if (typeof href !== typeof undefined && href !== false) {
        var page = parseInt(getPageNumber(href));
        $('#Page').val(page);
        $('#' + id).submit();
        return false;
    }
}
