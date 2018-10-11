$(document).ready(function () {

    //**********************FriendsCount**********************\\
    AjaxGetCall("#friends[data-ajax='true']",
        function (data) {
            modalData(data)
        }, function () {
            modalApear();
        });


    //**********************Delete ProfilePic**********************\\
    ModalAjaxCall("a.deleteprofilepic-link",
        null,
        function (data) {
            location.reload();
        }, 
        null)

    //**********************Change ProfilePic**********************\\
    ModalAjaxCall("a.changeprofilepic-link",
        null,
        function (data) {
            if (data.Success) {
                location.reload();
            }
        },
        null)

    //**********************FriendRequest**********************\\
    AjaxGetCall("#friendCount a",
        function (data) {
            modalApear(data)
        })


    //**********************Edit Info**********************\\
    AjaxCall("#userinfoEdit form",
       function (data) {
           location.replace(data.URL)
       },
       null)


    //**********************Edit advanced Info**********************\\
    AjaxCall("#userinfoAdvEdit form",
       function (data) {
           location.replace(data.URL)
       },
       null)

    
    var typ = $("#InfoContainer").data("type");
    $(".uinfo-tabs li[data-tab='" + typ + "']").addClass("active");

    AjaxGetCall("a.next-words-users",
        function (data) {
            $thisLink.replaceWith(data);
        },
        function () {
            loadingBox($thisLink);
        });

    AutoScroll("a.next-words-users", 1000);
    
});