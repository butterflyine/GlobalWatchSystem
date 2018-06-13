$(function () {
    var transportItem = $('#tranportItem');
    var pre_val;
    var tipMsg = transportItem.data('tip-val');
    $('#transportPlan').focus(function () {
        pre_val = $(this).val();
    }).change(function () {
        var str = $(this).val();

        if (str != pre_val && pre_val != "") {
            var sucess = confirm(tipMsg)
            if (!sucess) {
                $(this).val(pre_val);
                return false;
            }
            else {
                $(this).val(str);
                return true;
            }
        }
    });
});