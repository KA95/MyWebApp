$(document).ready(function () {
    $('#answers').val("");
    $('#like').click(function () {
        var id = $('#problemId').val();
        $.ajax({
            dataType: 'json',
            type: "POST",
            url: "/Problem/Like/",
            data: 'problemId=' + id,
            success:
                function (data) {
                    $('#likeCount').html(data.l);
                    $('#dislikeCount').html(data.d);
                }
        });
    });
    $('#dislike').click(function () {
        var id = $('#problemId').val();
        $.ajax({
            dataType: 'json',
            type: "POST",
            url: "/Problem/Dislike/",
            data: 'problemId=' + id,
            success:
                function (data) {
                    $('#likeCount').html(data.l);
                    $('#dislikeCount').html(data.d);
                }
        });
    });
    $('#addComment').click(function () {
        var id = $('#problemId').val();
        var text = $('#commentTextBox').val();
        $.ajax({
            dataType: 'json',
            type: "POST",
            url: "/Problem/AddComment/",
            data: 'problemId=' + id + '&commentText=' + text,
            success:
                (function (data) {
                    $('#comments').append(data.html);
                    $('#commentTextBox').val("");
                }),
            error:
                (function (data) {
                    alert("Please log in.");
                })
        });
    });

    $(".bootstrap-tagsinput > input").attr("hidden", "");
    $(".bootstrap-tagsinput > .tag > span").remove();

});
