function DPicker(regional, datepattern) {
    $dP = $(".hasDatePick");
    
        var checkin = $dP.datepicker({
            format: 'dd/mm/yyyy'
        }).on('changeDate', function (ev) {
            if (ev.date.valueOf() > checkout.date.valueOf()) {
                var newDate = new Date(ev.date)
                newDate.setDate(newDate.getDate() + 1);
                checkout.setValue(newDate);
            }
            checkin.hide();
            $(".hasDatePickUntil")[0].focus();
        }).data('datepicker');
        var checkout = $(".hasDatePickUntil").datepicker({
            format: 'dd/mm/yyyy',
            onRender: function (date) {
                return date.valueOf() <= checkin.date.valueOf() ? 'disabled' : '';
            }
        }).on('changeDate', function (ev) {
            checkout.hide();
        }).data('datepicker');
  
}
