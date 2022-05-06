var uploadList = [];
var tableBody = $(".uploadedLOIDocuments table tbody");

var GuidEmpty = "00000000-0000-0000-0000-000000000000";
var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png", ".pdf", ".doc", ".docx"];
function addLOIUploadInfo(uploadObj) {
    var data = "<p id='objID' rel='" + uploadObj.id + "'>" + uploadObj.file.name + "</p>";
    $(tableBody).append($(`
            <tr class="manage-table-item"><td class="file-icon"><div class="file"><i class="far fa-file"></i></div></td>
            <td class="name">` + data + `</td>
        <td class="action"><button title="Delete" data-toggle="tooltip" data-placement="right" class="pull-right btn btn-sm btn-danger margin-right10px removeThisUploadObj">Cancel</button></td>
        </tr>`));
}

function GenerateGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function ValidateExtension(fileName) {
    if (fileName.length > 0) {
        var blnValid = false;
        for (var j = 0; j < _validFileExtensions.length; j++) {
            var sCurExtension = _validFileExtensions[j];
            if (fileName.substr(fileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                blnValid = true;
                break;
            }
        }

        if (!blnValid) {
            return false;
        }
    }

    return blnValid;
}

$(".uploadedLOIDocuments").on("click", ".removeThisUploadObj", function (e) {
    e.preventDefault();

    //remove the obj in the list too
    var objID = $(this).parents(".manage-table-item").find("#objID").attr("rel");
    for (var i = uploadList.length - 1; i >= 0; i--) {
        var thisObj = uploadList[i];
        if (thisObj.id == objID)
            uploadList.splice(i, 1);
    }

    $(this).parents(".manage-table-item").remove();
});

//4/21/2019 Chi
//call this before you submit the form
//the reason I have to do this is because current LOI doesn't exist in database until it is submitted
function uploadLOIDocuments() {
    for (var i = 0; i < uploadList.length; i++) {
        var obj = uploadList[i];
        obj.data.submit();
    }
}

$(document).ready(function () {

    if (GenerateGuid == null)
        return; //<-- check app.js if this is null

    if (loiID == null || loiID == undefined)
        return; //<-- if you don't pass in a LOI ID, nothining should be initialized

    $('#fileupload').fileupload({
        url: '/epis3/UploadLOIFiles',
        dropZone: $(".main-dropzone"),
        singleFileUploads: true, // <-- single upload is fine
        autoUpload: false,
        formData: {
            loiID: loiID,
            lOIDocumentType: "ProofOfFunds" //<-- TODO: Chi: might need to do more work here to allow users upload different type of documents
        },
        add: function (e, data) {

            var file = data.files[0];
            if (ValidateExtension(file.name) == false) {
                $.notify({
                    // options
                    message: "File type is not allowed."
                }, {
                        // settings
                        element: 'body',
                        type: "danger",
                        delay: 5000,
                        mouse_over: 'pause'
                    });
                return;
            }

            if (file.size > 1e+7) {
                $.notify({
                    // options
                    message: "File size is over 10MB."
                }, {
                        // settings
                        element: 'body',
                        type: "danger",
                        delay: 5000,
                        mouse_over: 'pause'
                    });
                return;
            }

            var uploadObj = {
                id: GenerateGuid(),
                file: data.files[0],
                data: data
            };
            uploadList.push(uploadObj);

            addLOIUploadInfo(uploadObj);
        },
        stop: function (e, data) {
            //after all files are done
            if (submitLOIForm != null)
                submitLOIForm(); //<-- call the submit 
        },
        done: function (e, data) {
            //after each file is done   
        },
    });
});

$(document).bind('drop dragover', function (e) {
    e.preventDefault();
});

$(document).bind('dragover', function (e) {
    var dropZones = $('.main-dropzone'),
        timeout = window.dropZoneTimeout;
    if (timeout) {
        clearTimeout(timeout);
    } else {
        dropZones.addClass('in');
    }
    var hoveredDropZone = $(e.target).closest(dropZones);
    dropZones.not(hoveredDropZone).removeClass('hover');
    hoveredDropZone.addClass('hover');
    window.dropZoneTimeout = setTimeout(function () {
        window.dropZoneTimeout = null;
        dropZones.removeClass('in hover');
    }, 100);
});