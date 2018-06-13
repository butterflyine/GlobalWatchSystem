var map;
var markerArray = new Array();
var inforWndArray = new Array();
var polylineArray = new Array();

function createMap() {
    map = new BMap.Map("mapContainer");
    var point = new BMap.Point(101.397428, 38.90923);
    map.centerAndZoom(point, 5);
    map.enableScrollWheelZoom();
}


function addInfoWindow(mk, devInfo) {
    mapTag = $("#mapContainer");
    var devInfoText = mapTag.data("dev-infors");
    var devName = mapTag.data("dev-name");
    var devBelongTo = mapTag.data("dev-belongto");
    var devCurve = mapTag.data("dev-curve");
    var devHistCurve = mapTag.data("dev-historycurve");
    var info = [];
    info.push("<div style=\"padding:0px 0px 0px 4px;\"><b>" + devInfoText + "</b>");
    info.push("<b>" + devName + ":</b>" + devInfo.Name);
    info.push("<b>IMEI:</b>" + devInfo.IMEI);
    info.push("<b>" + devBelongTo + ":</b>" + devInfo.AreaName);
    info.push("<a href = '/device/curvemode/" + devInfo.Id + "'>" + devCurve + "</a>  " + "<a href = '/device/historymode/" + devInfo.Id + "'>" + devHistCurve + "</a>" + "</div>");

    var inforWindow = new BMap.InfoWindow(info.join("<br>"));
    inforWndArray.push(inforWindow);
    mk.addEventListener("click", function () {
        this.openInfoWindow(inforWindow);
    });
}

function addMarker(lng, lat, info, isCenter) {

    var pt = new BMap.Point(lng, lat);
    var myIcon = new BMap.Icon("http://webapi.amap.com/images/marker_sprite.png", new BMap.Size(32, 34));
    var marker2 = new BMap.Marker(pt, { icon: myIcon });  // 创建标注
    map.addOverlay(marker2);              // 将标注添加到地图中
    //marker.setTitle(info);
    markerArray.push(marker2, info);
    if (isCenter) {
        map.setCenter(marker2.getPosition());
    }
    addInfoWindow(marker2, info);
}

function findMarkerByTitle(title) {
    var i;
    for (i = 0; i < markerArray.length; ++i) {
        if (markerArray[i].getTitle() == title) {
            return markerArray[i];
        }
    }
}

function showMarker(index, visible) {
    if (visible) {
        markerArray[index].show();
    }
    else {
        markerArray[index].hide();
    }
}
function showMarkerByTitle(title, visible) {
    var mk = findMarkerByTitle(title);
    if (mk != null) {
        if (visible) {
            mk.show();
        }
        else {
            mk.hide();
        }
    }
}
function displayMarker(areaId) {
    var isFirst = true;
    $.getJSON("../../api/Map/", { areaId: areaId }).done(function (data) {
        $.each(data, function (key, item) {
            addMarker(item.DtuGPS.Longitude, item.DtuGPS.Latitude, item.Device, isFirst);
            if (isFirst) {
                isFirst = false;
            }
        });
    }).fail(function (jqXHR, textStatus, err) {
        alert(err);
    });
}

$(function () {
    createMap();
    var areaId = $('#areaId').text();
    displayMarker(areaId);
});