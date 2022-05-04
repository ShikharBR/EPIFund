//Chi: use this to manage all API calls / helper functions, this is very basic for now
export const epiService = {

    getParameterByName: function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';

        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    },

    //APIs
    getUserFolder: function (folderS3ObjectId = null, folderOnly = false) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/epis3/GetS3ObjectById", JSON.stringify({
            epiS3ObjectId: folderS3ObjectId
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    createFolder: function (key) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/epis3/CreateFolder", JSON.stringify({ key: key }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    getCurrentUserMessages: function (excludeReadMessages = true) {
        var dfd = jQuery.Deferred();
        var promise = $.post('/message/getCurrentUserMessages', JSON.stringify({ excludeReadMessages: excludeReadMessages }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    saveUserProfile: function (userProfile, selectedStates, selectedAssetTypes) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/api/UserInfo/SaveUserProfile", JSON.stringify({
            userProfile: userProfile,
            selectedStates: selectedStates,
            selectedAssetTypes: selectedAssetTypes
        }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    getCurrentUserInfoAndProfile: async function (includesContacts = false) {
        var dfd = jQuery.Deferred();
        var promise = $.post('/api/UserInfo/GetCurrentUserInfoAndProfile', JSON.stringify({
            includesContacts: includesContacts
        }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    getUserInfoAndProfileByUserId: function (userId) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/api/userinfo/GetUserInfoAndProfileByUserId", JSON.stringify({
            UserId: userId
        }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    getUserProfileBysUserIds: function (userIds) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/api/userinfo/GetUserProfileBysUserIds", JSON.stringify({
            userIds: userIds
        }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    markMessageAsRead: function (msg) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/message/markMessageAsRead", JSON.stringify(msg))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    getMessageById: function (id) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/message/getMessageById", JSON.stringify({ id: id }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    getUserContactsByUserID: function (userId) {
        var dfd = jQuery.Deferred();
        var promise = $.post('/api/UserContact/GetUserContactsByUserID', JSON.stringify({ ownerId: userId }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    saveContact: function (ownerUserId, savedUserId) {
        var dfd = jQuery.Deferred();
        var promise = $.post('/api/UserContact/SaveContact', JSON.stringify({
            ownerId: ownerUserId,
            savedUserId: savedUserId
        }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    removeContact: function (ownerUserId, savedUserId) {
        var dfd = jQuery.Deferred();
        var promise = $.post('/api/UserContact/RemoveContact', JSON.stringify({
            ownerId: ownerUserId,
            savedUserId: savedUserId
        }))
            .done(function (response) {
                dfd.resolve(response);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                dfd.reject(xhr.responseText);
            });

        return dfd.promise(promise);
    },
    removePermission: function (permission) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/epis3/RemovePermission", JSON.stringify({
            permission: permission
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getPermissionBySharedWithUserId: function (
        folderS3ObjectId,
        sharedWithUserId
    ) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/epis3/GetPermissionBySharedWithUserId", JSON.stringify({
            epiS3ObjectId: folderS3ObjectId,
            sharedWithUserId: sharedWithUserId

        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    givePermission: function (
        permission
    ) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/epis3/GivePermission", JSON.stringify({
            permission: permission
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    searchServiceProviders: function (name, company, selectedSPTypes, selectedStates, selectedAssetTypes) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceproviders/Search", JSON.stringify({
            name: name,
            company: company,
            selectedSPTypes: selectedSPTypes,
            selectedStates: selectedStates,
            selectedAssetTypes: selectedAssetTypes
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },

    //referral
    getUserQuickReferralLink: function () {
        var dfd = jQuery.Deferred();
        var promise = $.post("/referral/GetUserQuickReferralLink", JSON.stringify({

        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getUserReferrals: function () {
        var dfd = jQuery.Deferred();
        var promise = $.post("/referral/getUserReferrals", JSON.stringify({

        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getEmailTemplates: function () {
        var dfd = jQuery.Deferred();
        var promise = $.post("/epis3/GetEmailTemplates", JSON.stringify({

        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getReferralByCode: function (code) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/referral/GetReferralByCode", JSON.stringify({
            code: code
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    sendReferralEmails: function (emails, urlHostName, selectedEmailTemplate) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/referral/SendReferralEmails", JSON.stringify({
            emails: emails,
            urlHostName: urlHostName,
            selectedEmailTemplate: selectedEmailTemplate
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    resendInvitation: function (referral, urlHostName) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/referral/ResendInvitation", JSON.stringify({
            referral: JSON.stringify(referral),
            urlHostName: urlHostName
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },

    //preferred service providers
    addPreferredServiceProvider: function (
        preferredUserId,
        level,
        state = null, city = null, country = null,
        assetType = null) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceProviders/AddPreferredServiceProvider", JSON.stringify({
            preferredUserId: preferredUserId,
            level: level,
            state: state,
            city: city,
            country: country,
            assetType: assetType
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    addPreferredServiceProviders: function (
        preferredUserIds,
        level,
        assetTypes = [],
        locations = []) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceProviders/AddPreferredServiceProviders", JSON.stringify({
            preferredUserIds: preferredUserIds,
            level: level,
            assetTypes: assetTypes,
            locations: locations
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    removePreferredServiceProviders: function (preferredServiceProviders) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceProviders/RemovePreferredServiceProviders", JSON.stringify({
            preferredServiceProviders: preferredServiceProviders
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    removePreferredServiceProvidersByPreferredUserIdsAndLevel: function (preferredUserIds, level) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceProviders/RemovePreferredServiceProvidersByPreferredUserIdsAndLevel", JSON.stringify({
            preferredUserIds: preferredUserIds,
            level: level
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getUserPreferredServiceProviders: function (filters = null) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceProviders/GetUserPreferredServiceProviders", JSON.stringify({
            levelsFilter: null //there is alot of filters....
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getAvailableAssetTypes: function () {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceProviders/GetAvailableAssetTypes", JSON.stringify({

        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getAvailableStates: function () {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceProviders/GetAvailableStates", JSON.stringify({

        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getAvailableServiceProviderTypes: function () {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceProviders/GetAvailableServiceProviderTypes", JSON.stringify({

        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    getSuggestedPreferredSPs: function (country, state, assetType, serviceProviderType) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/serviceProviders/GetSuggestedPreferredSPs", JSON.stringify({
            country: country,
            state: state,
            assetType: assetType,
            serviceProviderType: serviceProviderType
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },


    //admin
    getManageSampleAssetData: function () {
        var dfd = jQuery.Deferred();
        var promise = $.post("/admin/GetManageSampleAssetData", JSON.stringify({

        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },
    saveManageSampleAssetData: function (list) {
        var dfd = jQuery.Deferred();
        var promise = $.post("/admin/SaveManageSampleAssetData", JSON.stringify({
            list: list
        })).done(function (response) {
            dfd.resolve(response);
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            dfd.reject(xhr.responseText);
        });

        return dfd.promise(promise);
    },

};