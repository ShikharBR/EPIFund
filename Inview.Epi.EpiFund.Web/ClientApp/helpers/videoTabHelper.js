$(function () {
    //this will return the video iframe if the url is ok
    function checkVideoFormat(url, returnType, autoplay, size) {

        //we just want the video id
        var youtubeRegex = /youtube.com\/watch\?v=([a-zA-Z0-9-_]+)/;
        var vimeoRegex = /vimeo.com\/([a-zA-Z0-9-_]+)/;

        var isYouTube = url.indexOf("https://www.youtube.com/") > -1;
        var isVimeo = url.indexOf("https://vimeo.com/") > -1;

        var found = [];
        if (isYouTube) {
            found = url.match(youtubeRegex);
        } else if (isVimeo) {
            found = url.match(vimeoRegex);
        } else {
            return null; //not found then return null
        }

        var auto = autoplay === true ? 1 : 0;

        if (returnType == "thumbnail") {
            if (isYouTube) {
                //https://img.youtube.com/vi/mRqDWkIhMBM/
                //https://img.youtube.com/vi/mRqDWkIhMBM/0.jpg/
                return '<div rel=' + url + ' class="viewVideo"><img style="width:100%; min-height:50px;" src="https://img.youtube.com/vi/' + found[1] + '/0.jpg"/></div>';
            } else if (isVimeo) {

                //for vimeo, you'll need to call vimeo api
                $.getJSON('https://vimeo.com/api/oembed.json?url=https%3A//vimeo.com/' + found[1], {
                    format: "json",
                    width: "640"
                },
                    function (data) {
                        $(".vimeo-image-" + found[1]).html('<img style="width:100%; min-height:50px;" src="' + data.thumbnail_url + '"/>');
                    });

                return '<div class="vimeo-image-' + found[1] + ' viewVideo" rel=' + url + ' class="viewVideo"></div>'
            }
        } else if (returnType == "video") {

            if (isYouTube) {
                return '<iframe class="previewVideoIframe" allowfullscreen frameborder="0" scrolling="no" marginheight="0" marginwidth="0" width="100%" height="300" type="text/html" src="https://www.youtube.com/embed/' + found[1] + '?autoplay=' + auto + '&fs=1&iv_load_policy=3&showinfo=0&rel=0&cc_load_policy=0&start=0&end=0"></iframe>';
            } else if (isVimeo) {
                return '<iframe class="previewVideoIframe" src="https://player.vimeo.com/video/' + found[1] + '?autoplay=' + auto + '" width="100%" height="300" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>';
            }
        }
    }

    //show featuredVideo
    var foundFeatureVideo = $('.featuredVideo')[0];
    if (foundFeatureVideo != null) {
        var featuredUrl = $(foundFeatureVideo).attr('rel');

        // Check to see if featuredUrl is null to prevent errors from arising within checkVideoFormat
        if (featuredUrl != null) {
            var featuredIframe = checkVideoFormat(featuredUrl, "video");
            if (featuredIframe != null) {
                $(".featuredVideo").html(featuredIframe);
            }

            var vidIndex = 0;
            $.each($(".videoDiv"), function () {
                var url = $(this).attr("rel");
                var img = checkVideoFormat(url, "thumbnail");
                $(this).html(img);
            });

            //view video
            $("#videos").on("click", ".viewVideo", function () {
                var url = $(this).attr("rel");
                var iframe = checkVideoFormat(url, "video");
                $(".featuredVideo").html(iframe);
            });
        }
    }


});