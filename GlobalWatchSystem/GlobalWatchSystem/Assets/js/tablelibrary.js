$(function() {

    var deviceRows = $("tr[data-device-id]");
    var deviceIds = _.map(deviceRows, function(row) {
        return $(row).data("device-id");
    });

    var showData = function(entries) {
        $.each(entries, function(index, data) {
            updateDevice(data);
        });
    };


    var updateDevice = function(deviceData) {
        var row = _.find(deviceRows, function(deviceRow) {
             return $(deviceRow).data("device-id") === deviceData.Id;
        });

        var update = function(data, selector) {
            if (!data)return;
            var cells = $(row).find("td." + selector);

            $(cells).each(function (index) {
                var cellData = index < data.length ? data[index] : { Value: "-", InAlarm: false };
                $(this).find(".content").text(cellData.Value);
                if (cellData.InAlarm) {
                    $(this).addClass("text-alarm");
                } else {
                    $(this).removeClass("text-alarm");
                }
            });
        };
        update(deviceData.Data["Temperature"], "temperature");
        update(deviceData.Data["Humidity"], "humidity");
        update(deviceData.Data["Power"], "power");

        var online = $(row).find("span.on");
        var offline = $(row).find("span.off");
        if (deviceData.OnLine) {
            $(online).show();
            $(offline).hide();
        } else {
            $(online).hide();
            $(offline).show();
        }
    };

    var updateTable = function() {
        $.ajax({
            url: "/api/device/status/" + deviceIds.join("-"),
            accepts: "application/json",
            success: function(deviceData) {
                showData(deviceData);
            }
        });
    };

    updateTable();
    setInterval(updateTable, 1 * 30 * 1000);
});