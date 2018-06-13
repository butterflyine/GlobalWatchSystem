var map;
var markerArray = new Array();
var inforWndArray = new Array();
var polylineArray = new Array();

function createMap() {
    map = new AMap.Map("mapContainer", {
        resizeEnable: true,
        //二维地图显示视口
        view: new AMap.View2D({
            center: new AMap.LngLat(101.397428, 38.90923),//地图中心点
            zoom: 5 //地图显示的缩放级别
        })
    });
}

function addLine() {
    var linArr = new Array();
    $.getJSON("api/Map").done(function (data) {
        $.each(data, function (key, item) {
            linArr.push(new AMap.LngLat(item.longitude, item.latitude));
        });
        var polyline = new AMap.Polyline({
            path: linArr,
            strokeColor: "#4455dd",
            strokeOpacity: 1,
            strokeWeight: 5,
            strokeStyle: "solid",
            strokeDasharray:[10,5]
        });
        polyline.setMap(map);
        polylineArray.push(polyline);
    }).fail(function (jqXHR, textStatus, err) {
        alert(err);
    });
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

    var inforWindow = new AMap.InfoWindow({
        offset: new AMap.Pixel(0, -23),
        content: info.join("<br>")
    });
    inforWndArray.push(inforWindow);
    AMap.event.addListener(mk, "click", function (e) {
        inforWindow.open(map, mk.getPosition());
    });
}

function addMarker(lng, lat, info,isCenter) {
   
    var marker = new AMap.Marker({
        icon: "http://webapi.amap.com/images/marker_sprite.png",
        position: new AMap.LngLat(lng, lat)
    });
    marker.setMap(map);  //在地图上添加点
    //marker.setTitle(info);
    markerArray.push(marker,info);
    if (isCenter) {
        map.setCenter(marker.getPosition());
    }
    addInfoWindow(marker, info);
}

function delMarker() {
    $.each(markerArray, function (key, item) {
        item.setMap(null);
    });
    markerArray = [];
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
    if (mk != null)
    {
        if (visible)
        {
            mk.show();
        }
        else
        {
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