$(document).ready(function () {

    //********************** Edit Company**********************\\
    AjaxCall("#companyEdit form",
       function (data) {
           location.replace(data.URL)
       },
       null)

    //**********************Change Logo**********************\\
    ModalAjaxCall("a.deleteCoLogo-link",
        null,
        function (data) {
            location.reload(); 
        },
        null)

    //**********************Delete Logo**********************\\
    ModalAjaxCall("a.changeCoLogo-link",
        null,
        function (data) {
            if (data.Success) {
                location.reload();
            }
        },
        null)

    //**********************Change Logo**********************\\
    $("body").delegate("a.changeLogo-link", 'click', function () {
        $editLink = $(this);
        $Modal = $("#innerModal");
        $.get($editLink.attr("href"),
            null,
            function (data) {
                $Modal.html(data);
                modalApear();
                $("#innerModal form.newCompany-pic").submit(function () {
                    loadingBox($("#modalConfirm"));
                    $thisForm = $(this);
                    $loadingBox = $thisForm.find("#loading");
                    $.validator.unobtrusive.parse($thisForm);
                    if ($thisForm.valid()) {
                        $.ajax({
                            url: this.action,
                            type: this.method,
                            data: $thisForm.serialize(),
                            success: function (data) {
                                if (data.Success) {
                                    location.reload();
                                }
                                else {
                                    $Modal.html(data.Message);
                                    errorApear();
                                }
                                //else $thisForm.parent().after(data.Result);
                            }
                        });
                    }
                    $loadingBox.remove();
                    return false;
                })
            })
    })

    //**********************Delete Logo**********************\\
    $("body").delegate("a.deleteLogo-link", 'click', function () {
        $editLink = $(this);
        $Modal = $("#innerModal");
        $.get($editLink.attr("href"),
            null,
            function (data) {
                $Modal.html(data);
                modalApear();
                $("#innerModal form.deletLogo-pic").submit(function () {
                    loadingBox($("#modalConfirm"));
                    $thisForm = $(this);
                    $loadingBox = $thisForm.find("#loading");
                    $thisForm = $(this);
                    $.validator.unobtrusive.parse($thisForm);
                    if ($thisForm.valid()) {
                        $.ajax({
                            url: this.action,
                            type: this.method,
                            data: $thisForm.serialize(),
                            success: function (data) {
                                if (data.Success) {
                                    location.reload();
                                }
                                else {
                                    $Modal.html(data.Message);
                                    errorApear();
                                }
                                //else $thisForm.parent().after(data.Result);
                            }
                        });
                    }
                    $loadingBox.remove();
                    return false;
                })
            })
    })

});