(function() {
    // Ask for confirm before perform delete opration
    $("input[data-delete-alert]").click(function(e) {
        e.preventDefault();

        var form = $(this).parent("form");
        var confirmText = $(this).data("confirm") || "Yes";
        var cancelText = $(this).data("cancel") || "No";
        var message = $(this).data("delete-alert");

        bootbox.dialog({
            message: message,
            buttons: {
                no: { label: cancelText, className: "btn-default btn" },
                yes: { label: confirmText, className: "btn-danger btn", callback: function() { form.submit(); } }
            }
        });
    });
})();

(function () {
    var findPos = function (data) {
        var tbody = $("#devTable");
        if (tbody == null) {
            return;
        }
        var dcModeDes = tbody.data("dc-description");
        var acModeDes = tbody.data("ac-description");
        if (dcModeDes == undefined || acModeDes == undefined) {
            return;
        }
        tbody.find("tr").each(function () {
            var tds = $(this).find("td");
            var deviceId = tds.eq(2).data("device-id");
            var hasAlarm = false;
            for (var i = 0; i < data.length; i++) {
                if (data[i].DeviceId == deviceId) {
                    $(this).css("background-color", "red");
                    hasAlarm = true;
                    break;
                }else {
                    $(this).css("background-color", '');
                }
            }
            if (!hasAlarm) {
                $(this).css("background-color", '');
            }
        });
    }
    // Load alarms, and notify
    var loadAlarms = function () {
        var notificationSound = new Howl({ urls: ["/Assets/sound/alert.mp3"] });

        var showAlarms = function (alarms) {
            var template = _.template(
                "<li class='alarm'>" +
                    "<a href='/device/curvemode/<%=deviceId%>'><%=deviceName%> <span class='label label-danger'><%=alarmType%></span></a>" +
                "</li>");

            var $alarmDropDown = $("#alarms .alert-dropdown");
            $alarmDropDown.find(".alarm").remove();

            _.each(alarms, function (alarm) {
                $alarmDropDown.prepend(
                    template({
                        deviceName: alarm.DeviceName,
                        alarmType: $alarmDropDown.data(alarm.AlarmType.toLowerCase()),
                        deviceId: alarm.DeviceId,
                    })
                );
            });
        };

        var notifyAlarms = function(alarms) {
            var $alarmNav = $("#alarms");

            if (_.size(alarms) > 0) {
                $alarmNav.find(".dropdown-toggle").addClass("in-alarm");
                notificationSound.play();

            } else {
                $alarmNav.find(".dropdown-toggle").removeClass("in-alarm");
                notificationSound.pause();
            }
        }

        $.ajax({
            url: "/api/alarms/area/" + $.cookie("_currentArea"),
            accepts: "application/json",
            success: function(alarms) {
                showAlarms(alarms);
                notifyAlarms(alarms);
                findPos(alarms);
            }
        });
    };

    loadAlarms();
    setInterval(loadAlarms, 5 * 60 * 1000);
})();