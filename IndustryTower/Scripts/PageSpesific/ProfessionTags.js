function ProfessionsTag(maxTags, errorMaxTags) {
    var ROOT = $("#ROOT").val();
    $('#ProfessionSearchBox').keyup(function () {
        if (!$(".professtionTags-container").is("visible")) $(".professtionTags-container").slideDown();
        var searchText = $(this).val();
        $.ajax({
            url: ROOT + 'Profession/ListPartial/',
            data: { q: searchText },
            type: "GET",
            success: function (data) {
                $('.professtionTags-container').html(data);
            }
        })
    });



    $(document).delegate("#profession", "click", function () {
        var initialvalue = $("input[name='professionTags']").val();
        //var initialvluearray = initialvalue.split(",").filter(function (v) { return v !== '' });
        var initialvluearray = [];
        $.each(initialvalue.split(","), function (index, item) {
            if (item !== '') {
                initialvluearray.push(item);
            }
        })
        if (initialvluearray.length <= maxTags) {
            var thisID = $(this).data("id");
            initialvluearray.push(thisID);
            $("input[name='professionTags']").val(initialvluearray.join(","))
            $("<div id='selectedProf' class='pink-tag inline-block selectedtag-marg' data-id='" + thisID + "'>" + $(this).find("div.panel-heading").text() + "<button type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button></div>").appendTo("#selectedProfs .panel .panel-body")
        } 
        else {
            modalApear();
            modalError(errorMaxTags)
        }
    });


    var wrap = $("#selectedProfs")
    wrap.delegate('#selectedProf button', 'click', function () {
        $this = $(this).parents("#selectedProf");
        $Input = $("input[name='professionTags']");
        var thisID = $this.data("id");
        var initialvalue = $Input.val();
        var initialvluearray = initialvalue.split(",").filter(function (v) { return v !== '' });
        initialvluearray = jQuery.grep(initialvluearray, function (value) {
            return value != thisID;
        })
        $this.remove()
        $Input.val(initialvluearray.join(","));
    });
}