
$(document).ready(function () {

    AjaxCall("#groupCreateEdit form",
       function (data) {
           window.location.replace(data.Url)
       },
       null,
       function () {
           //overridesDefault
       });

    AjaxCall("#GroupUserEdit form",
       function (data) {
           window.location.replace(data.Url)
       },
       null,
       function () {
           //overridesDefault
       });
   
    AjaxCall("#groupSessionCreatEdit form",
          function (data) {
              window.location.replace(data.Url)
          },
          null,
          function () {
              //overridesDefault
          });

    //**************SessionTabs***********\\\
    AjaxGetCall(".sesstion-type-choose a",
        function (data) {
            $("div#SessionContainer.active #loading").remove();
            $("div#SessionContainer.active").slideUp().removeClass("active");
            $("div#SessionContainer[data-sessiontype='" + $sessionType + "']").addClass("active").html(data).slideDown();
        },
        function () {
            $sessionType = $thisLink.data("sessiontype");
            var CurrentresultType = $("div#SessionContainer.active").data("sessiontype")
            if ($sessionType == CurrentresultType) { Continue = false; return false; }
            if (!($.trim($("div#SessionContainer[data-sessiontype='" + $sessionType + "']").html())==''))
            {
                $("a.sessionType-active-header").parent().removeClass("active");
                $("a.sessionType-active-header").removeClass("sessionType-active-header");
                $thisLink.parent().addClass("active");
                $thisLink.addClass("sessionType-active-header");
                $("div#SessionContainer.active").slideUp().removeClass("active");
                $("div#SessionContainer[data-sessiontype='" + $sessionType + "']").addClass("active").slideDown();
                Continue = false; return false;
            }
            loadingBox($("div#SessionContainer.active"));
            $("a.sessionType-active-header").parent().removeClass("active");
            $("a.sessionType-active-header").removeClass("sessionType-active-header");
            
            $thisLink.parent().addClass("active");
            $thisLink.addClass("sessionType-active-header");

            $.ajax({
                url: $thisLink.attr("href"),
                type: "GET",
                cache: false,
                success: function (data) {
                    $("div#SessionContainer[data-sessiontype='" + $sessionType + "']").html(data);
                }
            });
            
        });

    //**********************Next Sessions**********************\\
    AjaxGetCall("a.next-sessions-link",
        function (data) {
            var thisParent = $thisLink.parent();
            $thisLink.replaceWith(data);
            //$('html, body').animate({
            //    scrollTop: $pos
            //}, 2000)
            PartialContentLoader(thisParent);
            thisParent.find(".expandable").expander();
            thisParent.find('input, textarea').placeholder();
        },
        function () {
            //$pos = $thisLink.offset().top;
            loadingBox($thisLink);
        });

    AutoScroll("a.next-sessions-link", 1000);


    //**********************Delete Sessions**********************\\
    ModalAjaxCall("a.delete-GroupSession",
        null,
        function (data) {
            window.location.replace(data.Url);
        },
        null)

    //**********************MembersCount**********************\\
    AjaxGetCall("#groupMembers[data-ajax='true']",
        function (data) {
            modalData(data)
        }, function () {
            modalApear();
        });
    //**********************AdminsCount**********************\\
    AjaxGetCall("#groupAdmins",
        function (data) {
            modalData(data)
        }, function () {
            modalApear();
        });
    //**********************Membership**********************\\
    AjaxCall("#GroupMembership form",
       function (data) {
           $('div#GroupMembership').find("input[type='submit']").val(data.Result);
           $ThisLoadingElement.remove();
           messageApear(data.Message);
       },
       null,
       function () {
           loadingBox($thisForm.parents("div#GroupMembership"));
           $ThisLoadingElement = $thisForm.parent("#GroupMembership").find('div#loading');//overridesDefault
       })

    //**********************Next Offers**********************\\
    AjaxGetCall("a.next-offers-link",
        function (data) {
            var thisParent = $thisLink.parent('#nextOffers').parent();
            $thisLink.parent('#nextOffers').replaceWith(data);
            //$('html, body').animate({
            //    scrollTop: $pos
            //}, 2000)
            PartialContentLoader(thisParent);
            thisParent.find(".expandable").expander();
            thisParent.find('input, textarea').placeholder();
        },
        function () {
            //$pos = $thisLink.offset().top;
            loadingBox($thisLink);
        })

    AutoScroll("a.next-offers-link", 1000);

    //**********************Offer Accept**********************\\
    $(document).delegate("button#offacc", "click", function () {
        $(this).parents("#offerAcception form").submit();
    })
    AjaxCall("#offerAcception form",
       function (data) {
           $thisForm.parents('div#offerAcception').find("button i").text(data.Result);
           if (data.Accepted) {
               $thisForm.parents('div#offerAcception').addClass("accepted-offer");
               $thisForm.find("button").removeClass("btn-default").addClass("btn-success");
           }
           else {
               $thisForm.parents('div#offerAcception').removeClass("accepted-offer");
               $thisForm.find("button").removeClass("btn-success").addClass("btn-default");
           } 
           $ThisLoadingElement.remove();
           messageApear(data.Message);
       },
       null,
       function () {
           loadingBox($thisForm.parents("div#offerAcception"));
           $ThisLoadingElement = $thisForm.parent("#offerAcception").find('div#loading');//overridesDefault
       })


    //**********************Edit Offer**********************\\
    AjaxGetCall("a.edit-GroupSessionOffer",
       function (data) {
           var pos = $thisLink.parents('li').offset().top - 75;
           $thisLink.parents('li').find('#offerDesc').html(data);
           $('html, body').animate({
               scrollTop: pos
           }, 1000)
           $textarea = $("#editOfferForm form textarea");
           $textarea.height($textarea[0].scrollHeight + 10);
       });
    AjaxCall("#editOfferForm form",
       function (data) {
           $thisForm.parents("div#editOfferForm").find("div#loading").remove();
           $thisForm.parents('div#offerDesc').html(data.Result);
           messageApear(data.Message);
       },
       function () {
           loadingBox($thisForm.parents('div#editOfferForm'));
       });
    
    //**********************Delete Offer**********************\\
    ModalAjaxCall("a.delete-GroupSessionOffer",
        null,
        function (data) {
            if (data.Success)
            {
                $thisLink.parents('li').remove();
                modalMessage(data.Message);
            }
            
        },
        null)

    //*********************New Offer**********************\\
    AjaxCall("form.new-offer-form",
           function (data) {
               $thisForm.find("textarea").val("");
               if ($('ul#SessionOffers > li').length > 0) {
                   $('ul#SessionOffers > li:first').before(data.Result);

               }
               else {
                   $('ul#SessionOffers').append(data.Result);
               }
               modalApear();
               modalMessage(data.Message)
               PartialContentLoader($("ul#SessionOffers:first"));
               $submitbut.button("reset");
           },
           null,
           function () {
               //loadingBox($thisForm.parent("div#newOfferForm"));

               //$ThisLoadingElement = $thisForm.parent("#newOfferForm").find('div#loading');
           })

});

