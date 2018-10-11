function AjaxStateDropDown(URL, SelectListText) {

    $('#countries').change(function () {
        if ($(this).val() != 0) {
            getSecond($(this).val());
        }
    });
    function getSecond(stateId) {
        if (stateId != 0) {
            $.ajax({ 
                url: URL,
                type: 'POST',
                async: false,
                dataType: "json",
                data: { 'countryId': stateId },
                success: function (data) {
                    var items = '<option value="">'+ SelectListText + '</option>';
                    $.each(data, function (i, state) {
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                    });
                    $('#stateID').prop('disabled', false).html(items);
                    $('#stateID').attr("data-val-number", $('#stateID').data("val-required"))
                }
            });
        }
    }
}