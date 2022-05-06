export const accountingService = {

    //APIs
    getRevenueTransactions: async function () {
        var dfd = jQuery.Deferred();
        var promise = $.post("/accounting/getRevenueTransactions", JSON.stringify({
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    saveRevenueTransaction: async function (transaction) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/accounting/saveRevenueTransaction", JSON.stringify({
            transaction: transaction
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    }
};