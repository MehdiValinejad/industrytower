$(document).ready(function () {

    //**********************Create Edit Product**********************\\
    AjaxCall("#productCreateEdit form",
       function (data) {
           location.replace(data.URL)
       },
       null)


    //**********************Delete Product**********************\\
    AjaxCall("#productDelete form",
       function (data) { 
           location.replace(data.URL)
       },
       null)


    //**********************Search Navigation Product**********************\\
    //$(document).delegate(".search-product-pic", "mouseenter", function () {
    //    $this = $(this);
    //    $this.parents(".search-grid-product").find(".curren-product-icon").removeClass("curren-product-icon");
    //    $this.addClass("curren-product-icon");
    //    $thisCont = $(this).parents(".search-grid-product").next(".search-grid-desc");
    //    $thisCont.fadeOut("fast", function () {
    //        var content = $this.next(".search-product-desc").html();
    //        $thisCont.html(content).fadeIn();
    //        $thisCont.fadeIn();
    //    })
    //});

    
    


    //$("#productCreateEdit form").submit(function (e) {
    //    var Form = this;
    //    e.preventDefault();
    //    var arrayToPass = [];
    //    var Cat_array = $("ul#SelectedCategories li").map(function () {
    //        arrayToPass.push($(this).data("value"))
    //        //return $(this).data("value");
    //    });
    //    var final = arrayToPass.join(",");
    //    $("#Cats").val(final).ready(function () {
    //        Form.submit();
    //    });

    //});
})