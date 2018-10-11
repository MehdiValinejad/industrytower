
$(document).ready(function () {
    $initmodal = $("#initmodal");
    $newmodal = $("#newmodal");
})



function modalApear() {
    //$(".modal-content").html("<div class='modal-header'><button type='button' class='close' data-dismiss='modal'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><h4 class='modal-title'>"+$("title").text()+"</h4></div><div class='modal-body'><div id='loading'></div></div><div class='modal-footer'></div>");
    
    $('#Modal').modal();
    $("#initmodal .modal-body").html("<div id='loading'></div>");
    $(".modal-content").removeClass("errormodal");
    $('#Modal').on('hide.bs.modal', function (e) {
        $initmodal.show();
        $newmodal.hide();
    })
}


function modalData(data) {
    $initmodal.hide();
    $newmodal.show(); 
    $("#newmodal").html(data);
}


function modalError(data) {
    $newmodal.hide();
    $initmodal.show();
    $("#initmodal .modal-body").html(data);
    $("#initmodal").addClass("errormodal");
    
}

function modalMessage(data) {
    $("#newmodal").hide();
    $("#initmodal").show();
    $("#initmodal .modal-body").html(data);
}



//function modalApear(data) {
//    $("#modalContainer").css("background-color", "#fff");
//    $("#modalContainer").css("font-weight", "normal");
//    $("#innerModal").css("padding", "0").html(data);
//    $("#modalBG").fadeIn("slow", function () {
//        $("#outherModal").fadeIn("slow", function () {
//            $("#outherModal, #closeModal").click(function (e) {
//                if ($(e.target).is('td')) {
//                    modalClose();
//                }
//            });
//            $("#closeModal").click(function () {
//                modalClose();
//            });
//        });
//    });
//}

//function messageApear(data) {
//    $("#modalContainer").css("background-color", "#dde7ed");
//    $("#modalContainer").css("font-weight", "bold");
//    $("#innerModal").css("padding", "20").html(data);
//    $("#modalBG").fadeIn("slow", function () {
//        $("#outherModal").fadeIn("slow", function () {
//            $("#outherModal, #closeModal").click(function (e) {
//                if ($(e.target).is('td')) {
//                    modalClose();
//                }
//            });
//            $("#closeModal").click(function () {
//                modalClose();
//            });
//        });
//    });
//}

//function errorApear(data) {
//    $("#modalContainer").css("background-color", "#ffe1b7");
//    $("#modalContainer").css("font-weight", "bold");
//    $("#innerModal").css("padding", "10px").html(data);
//    $("#modalBG").fadeIn("slow", function () {
//        $("#outherModal").fadeIn("slow", function () {
//            $("#outherModal").click(function (e) {
//                if ($(e.target).is('td')) {
//                    modalClose();
//                }
//            });
//            $("#closeModal").click(function () {
//                modalClose();
//            });
//        });
//    });
//}
//function modalClose() {
//    $("#modalBG, #outherModal").fadeOut("slow", function () {
//        $("#innerModal").empty();
//    });
//}