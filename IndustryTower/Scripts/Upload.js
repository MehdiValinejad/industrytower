function UploadFile(MaxFileSize, MaxFileCount, FileSizeError, MaxFileCountError, FileTypeError) {

    var ROOT = $("#ROOT").val();
    Uploader("input#profilePic",
        ROOT + "Home/ProfilePicUpload/",
        "input#picToUpload",
        /\.(gif|jpg|jpeg|png)$/i,
        function (e, data, thisButt) {
            $HF = thisButt.parents('.hasUpload').find("input#picToUpload")
            $("form.newprofile-pic, form.newCompany-pic, form.newStore-pic").show();
            if (data.dataType == "iframe ") {
                //$("#newProfilePic").html("<div id='image-cropper' ><div class='cropit-image-preview center-block'></div><img id='newUploaded' class='cropper' src='/Uploads/Temporary/" + $(data.result).text() + "?" + new Date().getTime() + "'  ></div>");
                //$HF.val($(data.result).text());
                $("#newProfilePic").html("<div id='image-cropper'><div class='cropit-image-preview center-block'></div></div>");
                $HF.val($(data.result).text());
                //$("#newProfilePic").html("<div style='height:200px; direction:ltr;'><img id='newUploaded' class='cropper' src='/Uploads/Temporary/" + $(data.result).text() + "?" + new Date().getTime() + "'  ></div>");
            }
            else { 
                //$("#newProfilePic").html("<div style='height:200px; direction:ltr;'><img id='newUploaded' class='cropper' src='/Uploads/Temporary/" + data.result + "?" + new Date().getTime() + "'  ></div>");
                $("#newProfilePic").html("<div id='image-cropper' class='text-center'><div class='cropit-image-preview-container inline-block'><div class='cropit-image-preview'></div></div></div>");
                $HF.val(data.result);
            }

            
            $(".crop-help").show();

            $imageCropper = $('#image-cropper');
            if (data.dataType == "iframe ") {
                $imageCropper.cropit({
                    imageState: {
                        src: '/Uploads/Temporary/' + $HF.val($(data.result).text())  + '?' + new Date().getTime()
                    },
                    imageBackground: true
                });
            } else {
                $imageCropper.cropit({
                    imageState: {
                        src: '/Uploads/Temporary/' + data.result + '?' + new Date().getTime()
                    },
                    imageBackground: true
                });
            }
            
            setInterval(function () {
                if ($imageCropper.length > 0) {
                    var offset = $imageCropper.cropit("offset");
                    var size = $imageCropper.cropit('previewSize');
                    var imagesize = $imageCropper.cropit('imageSize');
                    var zoom = $imageCropper.cropit("zoom")
                    var curwd, curht, curx, cury;
                    if (imagesize.height / imagesize.width > 1) {
                        curx = offset.x / zoom;
                        cury = offset.y / zoom;
                        curwd = size.width / zoom;
                        curht = size.height / zoom;
                    } else {
                        curx = offset.x;
                        cury = offset.y;
                        curwd = size.width;
                        curht = size.height;
                    }
                    $("input[id='x']").val(Math.abs(Math.ceil(curx)));
                    $("input[id='y']").val(Math.abs(Math.ceil(cury)));
                    $("input[id='w']").val(Math.ceil(curwd));
                    $("input[id='h']").val(Math.ceil(curht));
                }
            },500)
            //$('.cropper').on('load', function () {
                //$('.cropper').cropper({
                //    aspectRatio: 1,
                //    done: function (selection) {
                //        $("input[id='x']").val(selection.x1);
                //        $("input[id='y']").val(selection.y1);
                //        $("input[id='w']").val(selection.width);
                //        $("input[id='h']").val(selection.height);
                //    }
                //})
                

                


                //$162('#newUploaded').imgAreaSelect({
                //    x1: 0,
                //    y1: 0,
                //    x2: 100,
                //    y2: 100,
                //    aspectRatio: "1:1",
                //    //imageHeight: 200,
                //    maxHeight:200,
                //    handles: true,
                //    onSelectEnd: function (img, selection) {
                //        $("input[id='x']").val(selection.x1);
                //        $("input[id='y']").val(selection.y1);
                //        $("input[id='w']").val(selection.width);
                //        $("input[id='h']").val(selection.height);

                //    }
                //})
            //})
            //if (data.dataType == "iframe ") {
            //    $HF.val($(data.result).text());
            //}
            //else {
            //    $HF.val(data.result);
            //}
        });


 

    Uploader("input#postFiles",
        ROOT + "Home/postFileUpload/",
        "input[id='filesToUpload']",
        /\.(gif|jpg|jpeg|png|pdf|xls|xlsx|pps|ppt|ppsx|mda|mdb|accdb|doc|docx)$/i,
        function (e, data,thisButt) {
            var elem = thisButt.parents(".hasUpload").find("#uploadedFiles");
            if (data.dataType == "iframe ") {
                fileExtension($(data.result).text(), elem, FileTypeError);
            }
            else {
                fileExtension(data.result, elem, elem, FileTypeError);
            }
            $HF = thisButt.parents('.hasUpload').find("input#filesToUpload");
            var fileNameArray = [];
            $.each($HF.val().split(","), function (index, item) {
                if (item !== '') {
                    fileNameArray.push(item);
                }
            });
            if (data.dataType == "iframe ") {
                fileNameArray.push($(data.result).text());
            }
            else {
                fileNameArray.push(data.result);
            }

            $HF.val(fileNameArray.join(","));
            setTimeout(function () {
                $(".progress-bar").width(0);
                $(".progress-bar").attr("aria-valuenow",0);
            }, 1500);

        });

    Uploader("input#ProServFiles",
        ROOT + "Home/ProServFileUpload/",
        "input#filesToUpload",
        /\.(gif|jpg|jpeg|png|pdf|xls|xlsx|pps|ppt|ppsx|mda|mdb|accdb|doc|docx)$/i,
        function (e, data, thisButt) {
            var imgcontainer = thisButt.parents(".hasUpload").find("ul#uploadedImages");
            var doccontainer = thisButt.parents(".hasUpload").find("ul#uploadedDocuments");
            if (data.dataType == "iframe ") {
                ProServFileExtension($(data.result).text(), imgcontainer, doccontainer, FileTypeError);
            }
            else {
                ProServFileExtension(data.result, imgcontainer, doccontainer, FileTypeError);
            }
            //ProServFileExtension(data.result, imgcontainer, doccontainer);
            $HF = $("input[id='filesToUpload']");
            var fileNameArray = [];
            $.each($HF.val().split(","), function (index, item) {
                if (item !== '') {
                    fileNameArray.push(item);
                }
            });
            if (data.dataType == "iframe ") {
                fileNameArray.push($(data.result).text());
            }
            else {
                fileNameArray.push(data.result);
            }

            $HF.val(fileNameArray.join(","));
            setTimeout(function () {
                $(".progress-bar").width(0);
            }, 1500);
        });

    Uploader("input#bookFiles",
        ROOT + "Home/bookFileUpload/",
        "input[id='filesToUpload']",
        /\.(gif|jpg|jpeg|png|pdf|pps|ppt|ppsx|doc|docx)$/i,
        function (e, data, thisButt) {
            var elem = thisButt.parents(".hasUpload").find("#uploadedFiles");
            if (data.dataType == "iframe ") {
                fileExtension($(data.result).text(), elem, FileTypeError);
            }
            else {
                fileExtension(data.result, elem, elem, FileTypeError);
            }
            $HF = thisButt.parents('.hasUpload').find("input#filesToUpload");
            var fileNameArray = [];
            $.each($HF.val().split(","), function (index, item) {
                if (item !== '') {
                    fileNameArray.push(item);
                }
            });
            if (data.dataType == "iframe ") {
                fileNameArray.push($(data.result).text());
            }
            else {
                fileNameArray.push(data.result);
            }

            $HF.val(fileNameArray.join(","));
            setTimeout(function () {
                $(".progress-bar").width(0);
                $(".progress-bar").attr("aria-valuenow", 0);
            }, 1500);

        });

    //Profile/Logo// Pic
    //$("input[id='profilePic']").fileupload({
    //    dropZone: null,
    //    url: ROOT + "Home/ProfilePicUpload/" + "?" + new Date().getTime(),
    //    add: function (e, data) {
    //        var goUpload = true;
            
    //        var currentFilesCount = $(this).parents(".hasUpload").find("input[id='picToUpload']").val().split(",").length;
    //        if (currentFilesCount > MaxFileCount) {
    //            errorApear(MaxFileCountError);
    //            goUpload = false;
    //        }
    //        var uploadFile = data.files[0];
    //        if (!(/\.(gif|jpg|jpeg|png)$/i).test(uploadFile.name)) {
    //            errorApear(FileTypeError);
    //            goUpload = false;
    //        }
    //        if (uploadFile.size > MaxFileSize) {
    //            errorApear(FileSizeError);
    //            goUpload = false;
    //        }
    //        if (goUpload == true) {
    //            data.submit();
    //        }
    //    },
    //    autoUpload: true,
    //    done: function (e, data) {
    //        var elem = $(this);

    //        $HF = elem.parents('.hasUpload').find("input[id='picToUpload']")
    //        $("form.newprofile-pic, form.newCompany-pic, form.newStore-pic").show();
    //        if (data.dataType == "iframe ") {
    //            $("#newProfilePic").html("<div style='height:200px; direction:ltr;'><img id='newUploaded' class='cropper' src='/Uploads/Temporary/" + $(data.result).text() + "?" + new Date().getTime() + "'  ></div>");
    //        }
    //        else {
    //            $("#newProfilePic").html("<div style='height:200px; direction:ltr;'><img id='newUploaded' class='cropper' src='/Uploads/Temporary/" + data.result + "?" + new Date().getTime() + "'  ></div>");
    //        }
            
    //        $(".crop-help").show();
    //        $('.cropper').on('load', function () {
    //            $('.cropper').cropper({
    //                aspectRatio: 1,
    //                done: function (selection) {
    //                    $("input[id='x']").val(selection.x1);
    //                    $("input[id='y']").val(selection.y1);
    //                    $("input[id='w']").val(selection.width);
    //                    $("input[id='h']").val(selection.height);
    //                }
    //            })

    //            //$162('#newUploaded').imgAreaSelect({
    //            //    x1: 0,
    //            //    y1: 0,
    //            //    x2: 100,
    //            //    y2: 100,
    //            //    aspectRatio: "1:1",
    //            //    //imageHeight: 200,
    //            //    maxHeight:200,
    //            //    handles: true,
    //            //    onSelectEnd: function (img, selection) {
    //            //        $("input[id='x']").val(selection.x1);
    //            //        $("input[id='y']").val(selection.y1);
    //            //        $("input[id='w']").val(selection.width);
    //            //        $("input[id='h']").val(selection.height);

    //            //    }
    //            //})
    //        })
    //        if (data.dataType == "iframe ") {
    //            $HF.val($(data.result).text());
    //        }
    //        else {
    //            $HF.val(data.result);
    //        }
    //    },
    //    fail: function (e,XMLHttpRequest) {
    //        var errorText = $.parseJSON(XMLHttpRequest.jqXHR.responseText).errorMessage;

    //        if (XMLHttpRequest.status == 400) {
    //            var array = errorText.split("||");
    //            $.each(array, function (index, item) {
    //                $("[data-valmsg-for='" + item.split("%%")[0] + "']")
    //                    .html(item.split("%%")[1])
    //                    .show()
    //                    .addClass("field-validation-error");
    //            })
    //        }
    //        else {
    //            errorApear(errorText);
    //        }
    //        $(this).parent("form").find("input[type='submit']").prop("disabled", false);
    //    }
    //}).on('fileuploadprogressall', function (e, data) {
    //    loadingBox($("#newProfilePic"));
    //    var progress = parseInt(data.loaded / data.total * 100, 10);
    //    $PB = $(this).parents('.hasUpload').find('.progress-bar');
    //    $PB.css('width', progress + '%');
    //    $PB.find('.sr-only').text(progress + '%');
        
    //});

    //Post Files
    //$("input[id='postFiles']").fileupload({
    //    dropZone: null,
    //    url: ROOT + "Home/postFileUpload/",//"../Home/postFileUpload/",
    //    add: function (e, data) {
    //        var goUpload = true;
    //        var currentFilesCount = $(this).parents(".hasUpload").find("input[id='filesToUpload']").val().split(",").length;
    //        if (currentFilesCount > MaxFileCount) {
    //            errorApear(MaxFileCountError);
    //            goUpload = false;
    //        }
    //        var uploadFile = data.files[0];
    //        if (!(/\.(gif|jpg|jpeg|png|pdf|xls|xlsx|pps|ppt|ppsx|mda|mdb|accdb|doc|docx)$/i).test(uploadFile.name)) {
    //            errorApear(FileTypeError);
    //            goUpload = false;
    //        }
    //        if (uploadFile.size > MaxFileSize) {
    //            errorApear(FileSizeError);
    //            goUpload = false;
    //        }
    //        if (goUpload == true) {
    //            data.submit();
    //        }
    //    },
    //    autoUpload: true,
    //    done: function (e, data) {
    //        var elem = $(this).parents(".hasUpload").find("#uploadedFiles");
    //        if (data.dataType == "iframe ") {
    //            fileExtension($(data.result).text(), elem,FileTypeError);
    //        }
    //        else {
    //            fileExtension(data.result, elem, elem, FileTypeError);
    //        }
    //        $HF = $(this).parents('.hasUpload').find("input[id='filesToUpload']");
    //        var fileNameArray = [];
    //        $.each($HF.val().split(","), function (index, item) {
    //            if (item !== '') {
    //                fileNameArray.push(item);
    //            }
    //        });
    //        if (data.dataType == "iframe ") {
    //            fileNameArray.push($(data.result).text());
    //        }
    //        else {
    //            fileNameArray.push(data.result);
    //        }
            
    //        $HF.val(fileNameArray.join(","));
    //        setTimeout(function () {
    //            $(".progress-bar").width(0);
    //        }, 1500);

    //    },
    //    fail: function (e, XMLHttpRequest) {
    //        var errorText = $.parseJSON(XMLHttpRequest.jqXHR.responseText).errorMessage;
    //        if (XMLHttpRequest.status == 400) {
    //            var array = errorText.split("||");
    //            $.each(array, function (index, item) {
    //                $("[data-valmsg-for='" + item.split("%%")[0] + "']")
    //                    .html(item.split("%%")[1])
    //                    .show()
    //                    .addClass("field-validation-error");
    //            })
    //        }
    //        else {
    //            errorApear(errorText);
    //        }
    //        $(this).parent("form").find("input[type='submit']").prop("disabled", false);
    //    }
    //}).on('fileuploadprogressall', function (e, data) {
    //    var progress = parseInt(data.loaded / data.total * 100, 10);
    //    //$PB = $(this).parents(':eq(2)').next('.progress').find('.progress-bar');
    //    $PB = $(this).parents('.hasUpload').find('.progress-bar');
    //    $PB.css('width', progress + '%');
    //    $PB.find('.sr-only').text(progress + '%');
    //});
    

    //Product//Service// Files
    //$("input[id='ProServFiles']").fileupload({
    //    dropZone: null,
    //    url: ROOT + "Home/ProServFileUpload/",
    //    add: function (e, data) {
    //        var goUpload = true;
    //        var currentFilesCount = $(this).parents(".hasUpload").find("input[id='filesToUpload']").val().split(",").length;
    //        if (currentFilesCount > MaxFileCount) {
    //            errorApear(MaxFileCountError);
    //            goUpload = false;
    //        }
    //        var uploadFile = data.files[0];
    //        if (!(/\.(gif|jpg|jpeg|png|pdf|xls|xlsx|pps|ppt|ppsx|mda|mdb|accdb|doc|docx)$/i).test(uploadFile.name)) {
    //            errorApear(FileTypeError);
    //            goUpload = false;
    //        }
    //        if (uploadFile.size > MaxFileSize) {
    //            errorApear(FileSizeError);
    //            goUpload = false;
    //        }
    //        if (goUpload == true) {
    //            data.submit();
    //        }
    //    },
    //    autoUpload: true,
    //    done: function (e, data) {
    //        var imgcontainer = $(this).parents(".hasUpload").find("ul#uploadedImages");
    //        var doccontainer = $(this).parents(".hasUpload").find("ul#uploadedDocuments");
    //        if (data.dataType == "iframe ") {
    //            ProServFileExtension($(data.result).text(), imgcontainer, doccontainer,FileTypeError);
    //        }
    //        else {
    //            ProServFileExtension(data.result, imgcontainer, doccontainer, FileTypeError);
    //        }
    //        //ProServFileExtension(data.result, imgcontainer, doccontainer);
    //        $HF = $("input[id='filesToUpload']");
    //        var fileNameArray = [];
    //        $.each($HF.val().split(","), function (index, item) {
    //            if (item !== '') {
    //                fileNameArray.push(item);
    //            }
    //        });
    //        if (data.dataType == "iframe ") {
    //            fileNameArray.push($(data.result).text());
    //        }
    //        else {
    //            fileNameArray.push(data.result);
    //        }
            
    //        $HF.val(fileNameArray.join(","));
    //        setTimeout(function () {
    //            $(".progress-bar").width(0);
    //        }, 1500);
    //    },
    //    fail: function (e, XMLHttpRequest) {
    //        var errorText = $.parseJSON(XMLHttpRequest.jqXHR.responseText).errorMessage;

    //        if (XMLHttpRequest.status == 400) {
    //            var array = errorText.split("||");
    //            $.each(array, function (index, item) {
    //                $("[data-valmsg-for='" + item.split("%%")[0] + "']")
    //                    .html(item.split("%%")[1])
    //                    .show()
    //                    .addClass("field-validation-error");
    //            })
    //        }
    //        else {
    //            errorApear(errorText);
    //        }
    //        $(this).parent("form").find("input[type='submit']").prop("disabled", false);
    //    }
    //}).on('fileuploadprogressall', function (e, data) {
    //    var progress = parseInt(data.loaded / data.total * 100, 10);
    //    //$PB = $(this).parents(':eq(2)').next('.progress').find('.progress-bar');
    //    $PB = $(this).parents('.hasUpload').find('.progress-bar');
    //    $PB.css('width', progress + '%');
    //    $PB.find('.sr-only').text(progress + '%');
    //});

    //$("div#uploadedFiles ul").delegate("li", "mouseenter mouseleave", function (event) {
    //    if (event.type === "mouseenter") {
    //        $(this).find("div.uploaded-file-delete").show();
    //    }
    //    else { $(this).find("div.uploaded-file-delete").hide(); }
    //});

    $(document).delegate("#uploadedFiles li button", "click", function () {
        var ThisLI = $(this).parent("li");
        var ThisUL = ThisLI.parent("ul");
        var content = ThisLI.data("file");
        $HHH = ThisLI.parents('.hasUpload').find("input[id='filesToUpload']")
        if ($HHH.val() != null)
        {
            var y = $HHH.val().split(",")
            y = jQuery.grep(y, function (value) {
                return value != content;
            });
            ThisLI.remove()
            $HHH.val(y.join(","));
        }
        
        if (ThisUL.children("li").length  == 0) {
            ThisUL.find("div.subject-small").hide();
        }
    });




    function Uploader(selector, url, hiddenfield, filesregexp, doneFunc, failFunc) {
        $(selector).fileupload({
            dropZone: null,
            url: url + "?" + new Date().getTime(),
            add: function (e, data) {
                var goUpload = true;

                var currentFilesCount = $(this).parents(".hasUpload").find(hiddenfield).val().split(",").length;
                if (currentFilesCount > MaxFileCount) {
                    modalApear();
                    modalError(MaxFileCountError);
                    goUpload = false;
                }
                var uploadFile = data.files[0];
                if (!(filesregexp).test(uploadFile.name)) {
                    modalApear();
                    modalError(FileTypeError);
                    goUpload = false;
                }
                if (uploadFile.size > MaxFileSize) {
                    modalApear();
                    modalError(FileSizeError);
                    goUpload = false;
                }
                if (goUpload == true) {
                    data.submit();
                }
            },
            autoUpload: true,
            done: function (e, data) {
                if (doneFunc != null) {
                    doneFunc(e, data,$(this));
                }
            },
            fail: function (e, XMLHttpRequest) {
                if (failFunc != null) {
                    failFunc(e, XMLHttpRequest);
                }

                var errorText = $.parseJSON(XMLHttpRequest.jqXHR.responseText).errorMessage;

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
                    modalApear();
                    modalError(errorText);

                }
                $(this).parent("form").find("input[type='submit']").prop("disabled", false);
            }
        }).on('fileuploadprogressall', function (e, data) {
            loadingBox($("#newProfilePic"));
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $PB = $(this).parents('.hasUpload').find('.progress-bar');
            $PB.css('width', progress + '%');
            $PB.attr("aria-valuenow", progress);
            $PB.find('.sr-only').text(progress + '%');

        });
    }

}


function uploadedInsert(elem, filename, fileAddress, ext) {
    var elc;
    if (ext != null) {
        elc = "<a href='" + fileAddress + "'><div class='thumbnail icn-60 " + ext + "'></div></a>";
    } else {
        elc = "<img src='" + fileAddress + "' class='thumbnail full-height' >";
    }
    elem.append("<li class='margin-top-bot-md margin-lf-r8-md ver-top' data-file='" + filename + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button>" + elc + "</li>");
}

function labelHandler(elem) {
    if (elem.has("li")) {
        elem.find("div.subject-small").show();
    }
}


function ProServFileExtension(fileName, imageContainer, DocContainer, FileTypeError) {

    var extension = fileName.split('.').pop();
    var fileAddress = "/Uploads/Temporary/" + fileName
    switch (extension.toLowerCase()) {
        case 'jpg':
        case 'gif':
        case 'png':
        case 'jpeg':
            uploadedInsert(imageContainer, fileName, fileAddress);
            //imageContainer.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><img src='" + fileAddress + "'  class='thumbnail full-height' ></li>");
            labelHandler(imageContainer);
            break;
        case 'pdf':
            uploadedInsert(DocContainer, fileName, fileAddress, "icon-pdf");
            //DocContainer.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_pdf thumbnail' ></a></li>");
            labelHandler(DocContainer);
            
            break;
        case 'doc':
        case 'docx':
            uploadedInsert(DocContainer, fileName, fileAddress, "icon-word");
            //DocContainer.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_doc thumbnail' ></a></li>");
            labelHandler(DocContainer);
            break;
        case 'xls':
        case 'xlsx':
            uploadedInsert(DocContainer, fileName, fileAddress, "icon-excel");
            //DocContainer.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_xls thumbnail' ></a></li>");
            labelHandler(DocContainer);
            break;
        case 'pps':
        case 'ppt':
        case 'ppsx':
            uploadedInsert(DocContainer, fileName, fileAddress, "icon-powerpoint");
            //DocContainer.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_pps thumbnail' ></a></li>");
            labelHandler(DocContainer);
            break;
        case 'mda':
        case 'mdb':
        case 'accdb':
            uploadedInsert(DocContainer, fileName, fileAddress, "icon-database");
            //DocContainer.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_DB thumbnail' ></a></li>");
            labelHandler(DocContainer);
            break;
        default:
            modalApear();
            modalError(FileTypeError);
    }
    
}

function fileExtension(fileName, elem, FileTypeError) {
    var extension = fileName.split('.').pop();
    var fileAddress = "/Uploads/Temporary/" + fileName;
    switch (extension.toLowerCase()) {
        case 'jpg':
        case 'gif':
        case 'png':
        case 'jpeg':
            var fileAddress = "/Uploads/Temporary/" + fileName
            uploadedInsert(elem, fileName, fileAddress);
            //elem.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><img src='" + fileAddress + "'  class='thumbnail full-height' ></li>");
            break;
        case 'pdf':
            uploadedInsert(elem, fileName, fileAddress, "icon-pdf");
            //elem.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_pdf thumbnail full-height' ></a></li>");
            break;
        case 'doc':
        case 'docx':
            uploadedInsert(elem, fileName, fileAddress, "icon-word");
            //elem.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_doc thumbnail full-height' ></a></li>");
            break;
        case 'xls':
        case 'xlsx':
            uploadedInsert(elem, fileName, fileAddress, "icon-excel");
            //elem.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_xls thumbnail full-height' ></a></li>");
            break;
        case 'pps':
        case 'ppt':
        case 'ppsx':
            uploadedInsert(elem, fileName, fileAddress, "icon-powerpoint");
            //elem.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_pps thumbnail full-height' ></a></li>");
            break;
        case 'mda':
        case 'mdb':
        case 'accdb':
            uploadedInsert(elem, fileName, fileAddress, "icon-database");
            //elem.append("<li data-file='" + fileName + "'><button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><a href='" + fileAddress + "'><img class='F_DB thumbnail full-height' ></a></li>");
            break;
        default:
            modalApear();
            modalError(FileTypeError);
    }
}


//Upload Shortcut Button
//$(document).ready(function () {
    

//    $("body").delegate("div#uploadShortcut", 'click', function () {
//        $(this).parents('div#newPost').find("div#uploadedFiles").slideDown();
//        $(this).parents('div#postEditorContainer').find("div#uploadedFiles").slideDown();
//    })
//});