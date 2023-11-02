$(() => {
    $("#DepartureDate").datepicker({
        dateFormat: 'dd MM yy',

        timepicker: false,
        showOtherMonths: true,
        selectOtherMonths: true,
        minDate: 0,
        maxDate: '+14'
    });

    $('#DepartureCityList').change(function () {
        var selectedValue = $(this).val();
        $('#ArrivalCityList option').removeAttr('disabled');
        $('#ArrivalCityList option[value="' + selectedValue + '"]').attr('disabled', 'disabled');
    });
    $('#ArrivalCityList').change(function () {
        var selectedValue = $(this).val();
        $('#DepartureCityList option').removeAttr('disabled');
        $('#DepartureCityList option[value="' + selectedValue + '"]').attr('disabled', 'disabled');
    });
});