var map;
function createMap() {
    map = new AMap.Map("mapContainer", {
        resizeEnable: true,
        //二维地图显示视口
        view: new AMap.View2D({
            center: new AMap.LngLat(101.397428, 38.90923),//地图中心点
            zoom: 10 //地图显示的缩放级别
        })
    });
}

function addMarker(lng, lat, info, isCenter) {
    var pic = isCenter ? "/Assets/image/flag.png" : "http://webapi.amap.com/images/marker_sprite.png";
    var marker = new AMap.Marker({
        icon: new AMap.Icon({
            //图标大小
            size: new AMap.Size(28, 37),
            //大图地址
            image: pic
        }),
        position: new AMap.LngLat(lng, lat)
    });
    marker.setMap(map);  //在地图上添加点
    marker.setTitle(info);
    if (isCenter) {
        map.setCenter(marker.getPosition());
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
        linArr[item.name].push(new AMap.LngLat(item.longitude, item.latitude));
        addMarker(item.longitude, item.latitude, FirstArr[item.name] ? devTip + "-" + item.name : "", FirstArr[item.name]);
        if (FirstArr[item.name]) {
            FirstArr[item.name] = false;
        }
    });

    for (var key in linArr) {
        var polyline = new AMap.Polyline({
            path: linArr[key],
            strokeColor: "#4455dd",
            strokeOpacity: 1,
            strokeWeight: 5,
            strokeStyle: "solid",
            strokeDasharray: [10, 5]
        });
        polyline.setMap(map);
    }
}

$(function () {
    createMap();
    displayMarker();
});