export const viewAssetService = {
    getParameterByName: function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';

        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    },
    getAsset: function (assetId) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/dataportal/GetAsset", JSON.stringify({
            assetId: assetId,
            isSampleAssetView: false
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getSampleAsset: function (selectedAssetType) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/dataportal/GetSamepleAsset", JSON.stringify({
            selectedAssetType: selectedAssetType,
            isSampleAssetView: true
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getAvailableSampleAssetTypes: function () {
        var dfd = jQuery.Deferred();
        var promise = $.post("/dataportal/GetAvailableSampleAssetTypes", JSON.stringify({

        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getViewAssetPageHeader: function (assetId, isSampleAssetView) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/dataportal/GetViewAssetPageHeader", JSON.stringify({
            assetId: assetId,
            isSampleAssetView: isSampleAssetView
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getViewAssetPageImagesAndVideos: function (assetId, isSampleAssetView) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/dataportal/GetViewAssetPageImagesAndVideos", JSON.stringify({
            assetId: assetId,
            isSampleAssetView: isSampleAssetView
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getViewAssetPageDetail: function (assetId, isSampleAssetView) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/dataportal/GetViewAssetPageDetail", JSON.stringify({
            assetId: assetId,
            isSampleAssetView: isSampleAssetView
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getViewAssetPageCalculators: function (assetId, isSampleAssetView) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/dataportal/GetViewAssetPageCalculators", JSON.stringify({
            assetId: assetId,
            isSampleAssetView: isSampleAssetView
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getViewAssetPageAdditionalInformation: function (assetId, isSampleAssetView) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/dataportal/GetViewAssetPageAdditionalInformation", JSON.stringify({
            assetId: assetId,
            isSampleAssetView: isSampleAssetView
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
};
