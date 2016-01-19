var minZoomLevel = 8;
var centerMap = new google.maps.LatLng(42.657153, 23.356044);

var map = new google.maps.Map(document.getElementById('map'), {
    zoom: minZoomLevel,
    center: centerMap,
    mapTypeId: google.maps.MapTypeId.ROAD
});

map.addListener('click', function (e) {
    placeMarkerAndPanTo(e.latLng, map);
});

var lastPinnedMarker;
function placeMarkerAndPanTo(latLng, map) {
    if (lastPinnedMarker) {
        lastPinnedMarker.setMap(null);
    }
    lastPinnedMarker = new google.maps.Marker({
        position: latLng,
        map: map
    });
    map.panTo(latLng);
}