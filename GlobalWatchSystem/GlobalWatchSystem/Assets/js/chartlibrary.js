// chartlibrary.js Charts library


Date.prototype.Format = function(fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};
$(function() {

    var devIMEI = $('#devIMEI').text();
    devIMEI = $.trim(devIMEI);
    var chDesc = $('#devIMEI').data('channel-desc');

    var lastQueryTime = new Date();
    var firstQuery = true;

    var dataPoints = [];
    var line = Morris.Line({
        element: 'temp-line-chart',
        data: [],
        xkey: 'd',
        ykeys: ['temp0', 'hum0', 'temp1', 'hum1'],
        labels: [chDesc + '0 ℃', chDesc + '0 %RH', chDesc + '1 ℃', chDesc + '1 %RH'],
        lineColors: ['#6E3D94', '#EC0936', '#5AB01F', '#2E6F9D'],
        smooth: false,
        resize: true
    });

    var updateChart = function() {
        if (firstQuery) {
            lastQueryTime = new Date();
            lastQueryTime.setHours(lastQueryTime.getHours() - 1);
            firstQuery = false;
        }
        $.getJSON("../../api/Curve/" + devIMEI, {
            date: lastQueryTime.Format("yyyy-MM-dd HH:mm:ss")
        }).done(function(data) {
            $.each(data, function(key, item) {

                if (dataPoints.length > 100) {
                    dataPoints = dataPoints.splice(1);
                } else {
                    dataPoints.push({ d: item.recvTime, temp0: item.tempValueCH0, hum0: item.humValueCH0, temp1: item.tempValueCH1, hum1: item.humValueCH1 });
                }
            });
            line.setData(dataPoints);
            if (dataPoints.length > 0) {
                lastQueryTime = new Date(Date.parse(dataPoints[dataPoints.length - 1].d.replace(/-/g, "/")));
            }
        }).fail(function(jqXHR, textStatus, err) {
            console.log(textStatus);
        });
    };

    setInterval(updateChart, 60 * 1000);
    updateChart();
});

(function() {
    var deviceId = $.trim($('#deviceId').text());

    var showDeviceData = function(latestData) {
        var update = function(valueEntries, selector) {
            if (!valueEntries) {
                return;
            }
            var $items = $(".device-realtime-data" + " ." + selector.toLowerCase());

            $($items).each(function(index, element) {
                var data = index < valueEntries.length ? valueEntries[index] : { Value: "-", InAlarm: false };
                $(element).find(".big-number").text(data.Value);
                if (data.InAlarm) {
                    $(element).addClass("text-alarm").removeClass("text-normal");
                } else {
                    $(element).removeClass("text-alarm").addClass("text-normal");
                }
            });
        };

        update(latestData.Data["Temperature"], "temperature");
        update(latestData.Data["Humidity"], "humidity");
        update(latestData.Data["Power"], "power");

        $(".device-realtime-data .last-update").text(latestData.LastUpdate);
    };

    var updateDeviceData = function() {
        $.ajax({
            url: "/api/device/status/" + deviceId,
            accepts: "application/json",
            success: function(deviceData) {
                showDeviceData(deviceData[0]);
            }
        });
    };

    updateDeviceData();
    setInterval(updateDeviceData, 60 * 1000);
})();