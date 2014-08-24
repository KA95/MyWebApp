﻿(function () {

    var bar = $('.progress-bar');
    var percent = $('.percent');
    var status = $('#status');

    $('#imageform').ajaxForm({
        beforeSend: function () {
            status.empty();
            var percentVal = '0%';
            bar.width(percentVal);
            percent.html(percentVal);
        },
        uploadProgress: function (event, position, total, percentComplete) {
            var percentVal = percentComplete + '%';
            bar.width(percentVal);
            percent.html(percentVal);
        },
        success: function () {
            var percentVal = '100%';
            bar.width(percentVal);
            percent.html(percentVal);
        },
        complete: function (xhr) {
            var result = xhr.responseText;
            result = result.substr(1, result.length - 2);
            status.html(result);
        }
    });

})();