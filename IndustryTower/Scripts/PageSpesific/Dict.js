$(document).ready(function () {
    AjaxCall("#dictCreateEdit form",
       function (data) {
           window.location.replace(data.Url)
       },
       null,
       function () {
           //overridesDefault
       });




    AjaxCall("#wordCreate form",
       function (data) {
           window.location.replace(data.Url)
       },
       null,
       function () {
           //overridesDefault
       });

    AjaxCall("#wordEdit form",
       function (data) {
           window.location.replace(data.Url)
       },
       null,
       function () {
           //overridesDefault
       });

    AjaxCall("#wordDescEdit form",
       function (data) {
           window.location.replace(data.Url)
       },
       null,
       function () {
           //overridesDefault
       });

    
    AjaxGetCall(".WordByLet",
        function (data) {
            $("#Meanning").html(data);
        }, function () {
            loadingBox($("#Meanning"));
        });

    AjaxGetCall("#Letters a",
        function (data) {
            $("#Words").html(data);
        }, function () {
            $("#Words").empty();
            loadingBox($("#Words"));
        });


    



    var ROOT = $("#ROOT").val();
    var minlength = 2;
    $('.hasWordShearch').keyup(function () {
        $currentWords = $(this).parent().find(".currentwords");
        $currentWords.html("<div id='loading'><div>");
        if (!$currentWords.is("visible")) $currentWords.slideDown();
        var that = this;
        var searchText = $(this).val();
        if (searchText.length >= minlength && !$(this).is(":disabled")) {
            $.ajax({
                url: ROOT + 'Word/WordSearch/',
                data: { s: searchText },
                type: "GET",
                success: function (data) {
                    if (searchText == $(that).val()) {
                        $(that).parent().find(".currentwords").html(data);
                    }
                }
            });
        }
    });

    $(window).keydown(function (event) {
        if ((event.keyCode == 13)) {
            event.preventDefault();
            return false;
        }
    });

    $(document).delegate(".form-control", "keyup", function () {
        var value = $(this).val();
        var data = $(this).data("prv");
        $("[data-prvw='" + data + "']").html(value);
    });


    $('input:radio[name="lang"]').change(
    function () {
        if ($(this).is(':checked') && $(this).val() == 'fa') {
            // append goes here
            $("#wordChoose").addClass("dir");
            $("#word").attr("placeholder", $("#word").data("phfa"));
            $("#wordesc").addClass("dir").attr("placeholder", $("#wordDesc").data("phfa"));
            $("#wordDesc").attr("placeholder", $("#wordDesc").data("phfa"));

        }
        else if ($(this).is(':checked') && $(this).val() == 'en') {
            $("#wordChoose").removeClass("dir").attr("placeholder", $("#word").data("phen"));
            $("#word").attr("placeholder", $("#word").data("phen"));
            $("#wordesc").removeClass("dir").attr("placeholder", $("#wordDesc").data("phen"));
            $("#wordDesc").attr("placeholder", $("#wordDesc").data("phen"));
        }

    });


    $(document).delegate("#wordChoose .search-mini-item", "click", function () {
        $("#WId").val($(this).data("id"));
        $("#word").val($(this).text().trim()).prop("disabled", true).trigger("keyup");
        $("input#lang").prop("disabled", true);
        $("#wordesc").show();
        $("#wordChoose .currentwords").empty();
        $("#wordChoose .enable-input").show();
        $(".final-submit").show();
        $(".currentwords").hide();
    });

    //$(document).delegate("#faChoose .search-mini-item", "click", function () {
    //    $("#faWId").val($(this).data("id"));
    //    $("#faWord").val($(this).text().trim()).prop("disabled", true).trigger("keyup");
    //    $("#fadesc").show();
    //    $(".final-submit").show();
    //    $("#faChoose .currentwords").empty();
    //    $("#faChoose .enable-input").show();
    //});

    $(document).delegate(".enable-input", "click", function () {
        $(this).prev().prop("disabled", false);
        $("input#lang").prop("disabled", false);
        $(this).prev().prev().val("");
        $(this).hide();
    });


    $(document).delegate("#wordChoose .new-word-insert", "click", function () {
        $("#wordesc").show();
        $(this).parents(".currentwords").empty();
        $(".final-submit").show();
        $(".currentwords").hide();
    });

    //$(document).delegate("#faChoose .new-word-insert", "click", function () {
    //    $(this).parents(".currentwords").empty();
    //    $("#fadesc").show();
    //    $(".final-submit").show();
    //});

        
});