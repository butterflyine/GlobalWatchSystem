var map;

function createMap() {
    map = new BMap.Map("mapContainer");
    var point = new BMap.Point(101.397428, 38.90923);
    map.centerAndZoom(point, 10);
    map.enableScrollWheelZoom();
}


function addMarker(lng, lat, info, isCenter) {

    var pt = new BMap.Point(lng, lat);
    var pic = isCenter ? "/Assets/image/flag.png" : "http://webapi.amap.com/images/marker_sprite.png";
    var myIcon = new BMap.Icon(pic, new BMap.Size(28, 37));
    var marker2 = new BMap.Marker(pt, { icon: myIcon });  // 创建标注
    map.addOverlay(marker2);              // 将标注添加到地图中
    marker2.setTitle(info);
    if (isCenter) {
        map.setCenter(marker2.getPosition());
    }
}

function displayMarker() {
    mapTag = $("#mapContainer");
    var isFirst = true;
    var devData = mapTag.data("dev-gpsdata");
    var devTip = mapTag.data("dev-startpoint");
    var linArr = new Array();
    var FirstArr = new Array();

    $.each(devData, function (key, item) {
        linArr[item.name] = new Array();
        FirstArr[item.name] = true;
    });

    $.each(devData, function (key, item) {
        linArr[item.name].push(new BMap.Point(item.longitude, item.latitude));
        addMarker(item.longitude, item.latitude, FirstArr[item.name] ? devTip + "-" + item.name : "", FirstArr[item.name]);
        if (FirstArr[item.name]) {
            FirstArr[item.name] = false;
        }
    });

    
    

    for (var key in linArr) {
        var polyline = new BMap.Polyline(linArr[key],
            { strokeColor: "blue", strokeWeight: 5, strokeOpacity: 1 });   //创建折线
        map.addOverlay(polyline);
    }
}

$(function () {
    createMap();
    displayMarker();
});