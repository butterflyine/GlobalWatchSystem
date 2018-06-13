// historychartlibrary.js Charts library

$(function () {
    $(".form_datetime").datetimepicker({
        format: 'yyyy-mm-dd hh:ii',
        language: ($.cookie("_culture").toLowerCase() == 'zh-cn') ? 'zh-CN' : "",
        todayBtn: true
    });
    var devIMEI = $('#devIMEI').text();
    var chDesc = $('#devIMEI').data('channel-desc');
    $('#btnView').click(function () {
        getData();
    });

    devIMEI = $.trim(devIMEI);

    var dataPoints = [];
    line = Morris.Line({
        element: 'temp-line-chart',
        data: [],
        xkey: 'd',
        ykeys: ['temp0', 'hum0', 'temp1', 'hum1'],
        labels: [chDesc + '0 ℃', chDesc + '0 %RH', chDesc + '1 ℃', chDesc + '1 %RH'],
        lineColors: ['#6E3D94', '#EC0936', '#5AB01F', '#2E6F9D'],
        smooth: false,
        resize: true
    });
    function getData() {
        dataPoints = [];
        line.setData(dataPoints);
        var start = $('#startTimePicker').val();
        var end = $('#endTimePicker').val();
        $.getJSON("../../api/Curve/" + devIMEI, {
            startDate: start,
            endDate: end
        }).done(function (data) {
            $.each(data, function (key, item) {
                dataPoints.push({ d: item.recvTime, temp0: item.tempValueCH0, hum0: item.humValueCH0, temp1: item.tempValueCH1, hum1: item.humValueCH1 });
            });
            line.setData(dataPoints);
        }).fail(function (jqXHR, textStatus, err) {
            alert(err);
        });
    };


    $("#btnExport").click(function () {
        var devIMEI = $("#devIMEI").text();
        var start = Date.parse($("#startTimePicker").val()) / 1000;
        var end = Date.parse($("#endTimePicker").val()) / 1000;

        window.open("/devices/" + devIMEI + "/export/" + start + "/" + end+"/", "_blank");
        window.focus();
    });
});