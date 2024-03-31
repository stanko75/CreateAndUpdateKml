(function (ns) {
    /*globals google, map, $*/
    "use strict";
    var map,
        google,
        lookup = [],
        $;

    window.setInterval(function() {
        $.getJSON("../json/liveImageMarkers.json",
            function(data) {
                data.forEach(function(liveImageMarkersNodes) {

                    var search = [liveImageMarkersNodes.lat, liveImageMarkersNodes.lng];
                    if (!isLocationFree(search)) {
                        lookup.push(search);
                    }

                    var gpsLatLng = new google.maps.LatLng(liveImageMarkersNodes.lat, liveImageMarkersNodes.lng);

                    var marker = new google.maps.Marker({
                        position: gpsLatLng,
                        map: map,
                        icon: liveImageMarkersNodes.fileName
                    });
                });
            });
    }, 5000);

    function isLocationFree(search) {
        for (var i = 0, l = lookup.length; i < l; i++) {
            if (lookup[i][0] === search[0] && lookup[i][1] === search[1]) {
                return false;
            }
        }
        return true;
    }

    isLocationFree(search);

})(window.milosev);