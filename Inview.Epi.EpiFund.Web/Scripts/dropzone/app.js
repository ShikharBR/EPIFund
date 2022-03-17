Dropzone.autoDiscover = false;
$(function () {
    var myDropzone = new Dropzone("div#dropzone",
        {
            maxFilesize: 10,
            acceptedFiles: 'image/jpeg,image/png,image/gif',
            url: "/Admin/UploadImages",
            dragenter: function (e) {
                
            },
            init: function () {
                var self = this;
                // config
                self.options.addRemoveLinks = true;
                self.options.dictRemoveFile = "Delete";

                // load already saved files

                var images = $("#JsonPictures").val();
                if (typeof images !== typeof undefined && images !== false && images !== '') {
                    var files = JSON.parse($("#JsonPictures").val()).Images;
                    for (var i = 0; i < files.length; i++) {
                        var mockFile = {
                            name: files[i].FileName,
                            size: 1000,
                            type: files[i].ContentType,
                            order: files[i].Order,
                            isFlyer: files[i].IsFlyerImage,
                            assetId: files[i].AssetId,
                            isMain: files[i].IsMainImage,
                            origName: files[i].OriginalFileName,
                            tempSize: files[i].Size,
                            sOrder: files[i].StaticOrder,
                            id: files[i].AssetImageId
                        }

                        self.options.addedfile.call(self, mockFile);
                        if (!files[i].AssetImageId || typeof files[i].AssetImageId == typeof undefined || files[i].AssetImageId == '00000000-0000-0000-0000-000000000000')
                            self.options.thumbnail.call(self, mockFile, "/Admin/GetTempImageThumbnailFromFile?filename=" + files[i].FileName + "&assetId=" + files[i].AssetId + "&userId=" + files[i].UserId + "&dateString=" + files[i].DateString);
                        else
                            self.options.thumbnail.call(self, mockFile, "/Admin/GetThumbnailFromFile?filename=" + files[i].FileName + "&assetId=" + files[i].AssetId);
                    }
                }

                //$.get('Admin/upload', function (data) {
                //    var files = JSON.parse(data).files;
                //    for (var i = 0; i < files.length; i++) {

                //        var mockFile = {
                //            name: files[i].name,
                //            size: files[i].size,
                //            type: 'image/jpeg'
                //        };

                //        self.options.addedfile.call(self, mockFile);
                //        self.options.thumbnail.call(self, mockFile, files[i].url);

                //    };
                //});

                // bind events

                //New file added
                self.on("addedfile", function (file) {
                    console.log('new file added ', file);
                });

                // Send file starts
                self.on("sending", function (file, xhr, formData) {
                    formData.append("assetId", $("#AssetId").val());
                    formData.append("userId", $("#UserId").val());
                    formData.append("dateString", $("#DateForTempImages").val());
                    console.log('upload started', file);
                    $('.meter').show();
                });

                // File upload Progress
                self.on("totaluploadprogress", function (progress) {
                    console.log("progress ", progress);
                    $('.roller').width(progress + '%');
                });

                self.on("queuecomplete", function (progress) {
                    $('.meter').delay(999).slideUp(999);
                });

                // On removing file
                self.on("removedfile", function (file) {
                    console.log(file);
                    RemoveFile(file);
                    
                    /*$.ajax({
                        url: '/Admin/Delete?fileName=' + file.name,
                        type: 'DELETE',
                        success: function (result) {
                            console.log(result);
                        }
                    });*/
                });
            }
        });
    $('#dropzone').sortable({
        stop: function (event, ui) {
            var inputs = $('.currentposition');
            var labels = $('.currentpositionlabel');
            $(inputs).each(function (idx) {
                $(this).val(idx);
            });
            $(labels).each(function (idx) {
                $(this).text(idx);
            });
        }
    }).disableSelection();
    /*
    $('#dropzone').sortable({
        stop: function (event, ui) {
            if ($(ui.item).find('.mainflyer').is(':checked')) {
                $('#imgsPreviewer').sortable("cancel");
                alert("Cannot change order of main flyer image. This image will always show first. Uncheck the Is Main Flyer image checkbox to change order of image.");
            } else {
                var inputs = $('.currentposition');
                var labels = $('.currentpositionlabel');
                var nbElems = inputs.length;
                $(inputs).each(function (idx) {
                    $(this).val(idx);
                });
                $(labels).each(function (idx) {
                    $(this).text(idx);
                });
            }
        }
    }).disableSelection();
    */
});

//Dropzone.options.myDropzone = {
//    init: function () {
//        var self = this;
//        // config
//        self.options.addRemoveLinks = true;
//        self.options.dictRemoveFile = "Delete";

//        // load already saved files

//        var files = JSON.parse($("#JsonPictures").val()).Images;
//        if (typeof files !== typeof undefined && files !== false) {
//            for (var i = 0; i < files.length; i++) {
//                var mockFile = {
//                    name: files[i].FileName,
//                    size: 1000,
//                    type: files[i].ContentType
//                }

//                self.options.addedfile.call(self, mockFile);
//                self.options.thumbnail.call(self, mockFile, "/Admin/GetThumbnailFromFile?filename=" + files[i].FileName + "&assetId=" + files[i].AssetId);
//            }
//        }

//        //$.get('Admin/upload', function (data) {
//        //    var files = JSON.parse(data).files;
//        //    for (var i = 0; i < files.length; i++) {

//        //        var mockFile = {
//        //            name: files[i].name,
//        //            size: files[i].size,
//        //            type: 'image/jpeg'
//        //        };

//        //        self.options.addedfile.call(self, mockFile);
//        //        self.options.thumbnail.call(self, mockFile, files[i].url);

//        //    };
//        //});

//        // bind events

//        //New file added
//        self.on("addedfile", function (file) {
//            console.log('new file added ', file);
//        });

//        // Send file starts
//        self.on("sending", function (file, xhr, formData) {
//            console.log('upload started', file);
//            $('.meter').show();
//        });

//        // File upload Progress
//        self.on("totaluploadprogress", function (progress) {
//            console.log("progress ", progress);
//            $('.roller').width(progress + '%');
//        });

//        self.on("queuecomplete", function (progress) {
//            $('.meter').delay(999).slideUp(999);
//        });

//        // On removing file
//        self.on("removedfile", function (file) {
//            console.log(file);
//            $.ajax({
//                url: '/Admin/Delete?fileName=' + file.name,
//                type: 'DELETE',
//                success: function (result) {
//                    console.log(result);
//                }
//            });
//        });

//    }
//};