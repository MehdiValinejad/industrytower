$(document).ready(function () {
    $(document).on("click", "a[data-ajax=true]", function (evt) {
        evt.preventDefault();
    });

    $(document).on("click", "a[href='#']", function (evt) {
        evt.preventDefault();
    });

}); 


function AjaxGetCall(link, getFunc, beforeGet) {
    $("body").delegate(link, "click", function () {
        $thisLink = $(this);
        Continue = true;
        if (beforeGet != null) {
            beforeGet();
        }
        if (!Continue) return false;
        $.ajax({
            url: $thisLink.attr("href"),
            type: "GET",
            cache: false,
            success: function (data) {
                if (getFunc != null) {
                    getFunc(data);
                }
            }
        });
    });
}


function AjaxCall(form, postSuccessFunc, postErrorFunc, beforValidation) {
    $("body").delegate(form, 'submit', function () {
        $thisForm = $(this);
        $submitbut = $thisForm.find("input[type='submit']");
        $submitbut.button("loading");
        //$thisForm.find("input[type='submit']").prop("disabled", true);
        //loadingBox($thisForm.find("div.submitContainer"));
        //$ThisLoadingElement = $thisForm.find('div#loading');
        if (beforValidation != null) {
            beforValidation();
        }
        $.validator.unobtrusive.parse($thisForm);  //added
        if ($thisForm.valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $thisForm.serialize(),
                success: function (data) {
                    if (postSuccessFunc != null) {
                        postSuccessFunc(data);
                    }
                    //submitbut.button("reset");
                    //$thisForm.find("input[type='submit']").prop("disabled", false);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    var errorText = $.parseJSON(XMLHttpRequest.responseText).errorMessage;

                    if (XMLHttpRequest.status == 400) {
                        var array = errorText.split("||");
                        $.each(array, function (index, item) {
                            $("[data-valmsg-for='" + item.split("%%")[0] + "']")
                                .html(item.split("%%")[1])
                                .show()
                                .addClass("field-validation-error");
                        })
                    }
                    else {
                        modalApear()
                        modalError(errorText);
                    }
                    //$thisForm.find("input[type='submit']").prop("disabled", false);
                    //$ThisLoadingElement.remove();
                    $submitbut.button("reset");
                    if (postErrorFunc != null) {
                        postErrorFunc(XMLHttpRequest);
                    }
                }
            });
        } else {
            //$thisForm.find("input[type='submit']").prop("disabled", false);
            //$ThisLoadingElement.remove();
            $submitbut.button("reset");
        }
        return false;
    })
}

function ModalAjaxCall(link, getFunc, postSuccessFunc, postErrorFunc) {
    $("body").delegate(link, "click", function () {
        modalApear();
        $thisLink = $(this);
        $.ajax({
            url: $thisLink.attr("href"),
            type: "GET",
            cache: false,
            success: function (data) {
                modalData(data);
                //$Modal = $("#innerModal");
                if (getFunc != null) {
                    getFunc();
                }
                modalFormSubmit();
            }
        });

    });
    function modalFormSubmit() {
        $("#newmodal form").submit(function () {
            $thisForm = $(this);
            $submitbut = $thisForm.find("input[type='submit']");
            $submitbut.button("loading");
            //$thisForm.find("input[type='submit']").prop("disabled", true);
            //loadingBox($thisForm.find("div#modalConfirm"));
            //$ThisLoadingElement = $thisForm.find('div#loading');
            $.validator.unobtrusive.parse($thisForm);
            if ($thisForm.valid()) {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $thisForm.serialize(),
                    success: function (data) {
                        if (data.FormResend != null) {
                            $Modal.html(data.FormResend);
                            if (getFunc != null) {
                                getFunc();
                            }
                            modalFormSubmit();
                            
                            //$ThisLoadingElement.remove();
                            //$thisForm.find("input[type='submit']").prop("disabled", false);
                        }
                        if (data.Result != null || data.Message != null || data.Success != null) {
                            if (postSuccessFunc != null) {
                                postSuccessFunc(data);
                            }
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                       var errorText = $.parseJSON(XMLHttpRequest.responseText).errorMessage;
                       $(".field-validation-error").removeClass("field-validation-error").addClass("field-validation-valid")
                        if (XMLHttpRequest.status == 400) {
                            var array = errorText.split("||");
                            $.each(array, function (index, item) {
                                $("[data-valmsg-for='" + item.split("%%")[0] + "']")
                                    .html(item.split("%%")[1])
                                    .show()
                                    .addClass("field-validation-error");
                            })
                        }
                        else {
                            modalApear()
                            modalError(errorText);
                        }
                        //$thisForm.find("input[type='submit']").prop("disabled", false);
                        //$ThisLoadingElement.remove();
                        $submitbut.button("reset");
                        if (postErrorFunc != null) {
                            postErrorFunc(XMLHttpRequest);
                        }
                    }
                });
            } else {
                //$thisForm.find("input[type='submit']").prop("disabled", false);
                //$ThisLoadingElement.remove();
                $submitbut.button("reset");
            }
            return false;
        })
    }

}
