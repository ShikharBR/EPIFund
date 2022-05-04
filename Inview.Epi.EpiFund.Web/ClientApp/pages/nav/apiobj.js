        const api = {
            getICAPendingCounts: () => {
                fetch('/Home/GetICAPendingCounts')
                    .then(res => res.json())
                    .then(result => {
                        pendingCounts = result;
                    });
            },
            searchAssets: (payload, cb) => {
                fetch('/Home/SearchAssets', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8', },
                    body: JSON.stringify(payload),
                })
                    .then(res => res.json())
                    .then(result => {
                        console.log('<searchAssets> response', result);
                        cb(null, result);
                    });
            },
            getUserCompanies: cb => {
                fetch('/Home/GetUserCompanies', {
                    method: 'POST',
                })
                    .then(res => res.json())
                    .then(result => {
                        console.log('<getUserCompanies> response', result);
                        cb(null, result);
                    });
            },
            doesUserCompanyExist: (payload, cb) => {
                // I failed to find existing duplicate logic for OC/HC, so
                // just check company name for now, add more/less later
                fetch('/Home/DoesUserCompanyExist', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8', },
                    body: JSON.stringify(payload),
                })
                    .then(res => res.json())
                    .then(result => {
                        console.log('<doesUserCompanyExist> response', result);
                        cb(null, result);
                    });
            },
            createUserCompany: (payload, cb) => {
                fetch('/Home/CreateUserCompany', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8', },
                    body: JSON.stringify(payload),
                })
                    .then(res => res.json())
                    .then(result => {
                        console.log('<createUserCompany> response', result);
                        cb(null, result);
                    });
            },
            updateUserCompany: (payload, cb) => {
                fetch('/Home/UpdateUserCompany', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8', },
                    body: JSON.stringify(payload),
                })
                    .then(res => res.json())
                    .then(result => {
                        console.log('<updateUserCompany> response', result);
                        cb(null, result);
                    });
            },
            createAsset: (payload, cb) => {

                fetch('/Home/CreateAsset', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8', },
                    body: JSON.stringify(payload),
                })
                    .then(res => res.json())
                    .then(result => {
                        console.log('<createAsset> response', result);
                        cb(null, result);
                    });
            },
            claimAsset: (payload, cb) => {
                fetch('/Home/ClaimAsset', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8', },
                    body: JSON.stringify(payload),
                })
                    .then(res => res.json())
                    .then(result => {
                        console.log('<createAsset> response', result);
                        cb(null, result);
                    });
            },
            uploadFilesForNewlyCreatedAsset: (payload, cb) => {
                fetch('/Home/UploadCreateAssetDocsAndCreateAssetVersion', {
                    method: 'POST',
                    body: payload,
                })
                    .then(res => res.json())
                    .then(result => {
                        console.log('<uploadFilesForNewlyCreatedAsset> response', result);
                        cb(null, result);
                    });
            },
            uploadFilesForClaimAsset: (payload, cb) => {
                fetch('/Home/UploadClaimAssetDocs', {
                    method: 'POST',
                    body: payload,
                })
                    .then(res => res.json())
                    .then(result => {
                        console.log('<uploadFilesForClaimAsset> response', result);
                        cb(null, result);
                    });
            },
            uploadFile: (files, type) => {

                if (files.length === 1) {
                    if (files[0].name.split(".").pop().toLowerCase() === 'pdf') {
                        console.log(files[0].size);
                        if (files[0].size <= 25000000) {
                            //doc0 = files[0];
                            var success;
                            switch (type) {
                                case 'Title Insurance Policy':
                                    api.tempGlobal.docTitleInsurancePolicy = files[0];
                                    success = true;
                                    break;
                                case 'Vesting Deed':
                                    api.tempGlobal.docVestingDeed = files[0];
                                    success = true;
                                    break;
                                case 'State Documentation':
                                    api.tempGlobal.docStateDocumentation = files[0];
                                    success = true;
                                    break;
                                case 'Other':
                                    api.tempGlobal.docOther = files[0];
                                    success = true;
                                    break;
                                case 'Offering Memorandum':
                                    api.tempGlobal.docOm = files[0];
                                    success = true;
                                    break;
                                default:
                                    alert('we currently do not support document type ' + event.target.getAttribute('data-type'))
                                    break;
                            }
                            if (success) {
                                console.log(`Successfully uploaded ${event.target.getAttribute('data-type')} document`);
                            }

                        } else {
                            alert('File cannot be larger than 25mb');
                        }
                    } else {
                        alert('invalid file format!');
                    }
                } else {
                    alert('Only select one file por favor.')
                }
            },
            validateThatOnePopupWithCompanyInfoAndStuff: (state, globalAssetObj, cb) => {
                // Shouldve tried harder but couldnt figure out why get summary didnt work,
                // so saving even more data to global
                let errors = [];
                let success = false;
                if (!state.operating || !state.operating.length) {
                    errors.push('Please select an operating company');
                }
                if (!state.holding || !state.holding.length) {
                    errors.push('Please select a holding company');
                }
                if (!state.aquisition || !state.aquisition.length) {
                    errors.push('Aquisition date required')
                } else {
                    // TODO: add moment or https://github.com/moment/luxon to project for date testing
                    const re = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
                    if (re.test(state.aquisition)) {
                        errors.push('Please specify a valid aquisition date');
                    }
                }
                if (!state.terms || !state.terms.length) {
                    errors.push('Seller terms required(TODO: fix auto selected but not)')
                }

                if (!state.purchaseprice || !state.purchaseprice.length) {
                    errors.push('Purchase Price required')
                }
                if (!errors.length) {
                    success = true;
                    globalAssetObj.aquisition = new Date(state.aquisition);
                    globalAssetObj.purchaseprice = parseInt(state.purchaseprice);
                    globalAssetObj.sellertermId = state.terms;
                    api.tempGlobal.sellerTerms.forEach(term => {
                        if (term.val === state.terms) { globalAssetObj.sellertermId = term.id; }
                    });
                    if (state.operating && state.operating.length) {
                        state.operating.forEach(c => {
                            if (c['Id']) { globalAssetObj.operatingId = c['Id']; }
                        });
                    }
                    if (state.holding && state.holding.length) {
                        state.holding.forEach(c => {
                            if (c['Id']) { globalAssetObj.holdingId = c['Id']; }
                        });
                    }
                    console.log('asset:', globalAssetObj);

                } else {
                    success = false;
                    alert(errors.join('\n'))
                }
                cb(success);
            },
            tempGlobal: {
                operating: [],
                holding: [],
                assets: [], // for multi asset claiming????
                claimedAsset: null,
                newAsset: null,
                docTitleInsurancePolicy: null,
                docVestingDeed: null,
                docStateDocumentation: null,
                docOther: null,
                docOm: null,
                sellerTerms: [
                    { id: 1, val: 'All Cash – no PMF' },
                    { id: 2, val: 'Cash & PMF' },
                    { id: 3, val: 'Cash & Assumption of existing Debt Package' },
                    { id: 4, val: 'Cash & Seller Carryback with Assumption of Existing Debt Package' },
                    { id: 5, val: 'Cash & Seller Carryback (Property was F&C of any Debt Package)' },
                    { id: 6, val: 'Submit Proposal' },
                    { id: 7, val: 'Cash & Property for Property 1031 Exchange' },
                    { id: 8, val: 'Property for Property 1031 Exchange – No Cash Transfer' },
                    { id: 9, val: 'Other' },
                ],
                operatingMap: {
                    'Id': 'Id',
                    'CompanyName': 'Operating Company',
                    'FirstName': 'Operating Company E-mail',
                    'LastName': 'Officer First Name',
                    'Email': 'Officer Last Name',
                    'AddressLine1': 'Address',
                    'AddressLine2': 'Address 2',
                    'City': 'City',
                    'State': 'State',
                    'Zip': 'Zip Code',
                    'Country': 'Country',
                    'CellNumber': 'Cell Number',
                    'WorkNumber': 'Work Number',
                    'FaxNumber': 'Fax Number',
                },
                holdingMap: {
                    'Id': 'Id',
                    'CompanyName': 'Contract Owner',
                    'FirstName': 'Officer First Name',
                    'LastName': 'Officer Last Name',
                    'Email': 'E-Mail',
                    'AddressLine1': 'Address',
                    'AddressLine2': 'Address 2',
                    'City': 'City',
                    'State': 'State',
                    'Zip': 'Zip Code',
                    'Country': 'Country',
                    'CellNumber': 'Cell Number',
                    'WorkNumber': 'Work Number',
                    'FaxNumber': 'Fax Number',
                }
            }
        };