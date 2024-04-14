(function (ns) {
    /*globals google, map, $*/
    "use strict";
    var map;

    function markerExists(markerPosition) {
        for (var i = 0; i < ns.markers.length; i++) {
            if (ns.markers[i].getPosition().lat() === markerPosition.lat &&
                ns.markers[i].getPosition().lng() === markerPosition.lng) {
                return true;
            }
        }
        return false;
    }

    function displayAndPushLiveImageMarker(markers) {
        loadJsonConfig(function(config) {
            window.setInterval(function() {
                $.getJSON(config.liveImageMarkersJsonUrl,
                        function(data) {
                            data.forEach(function(liveImageMarkersNodes) {

                                var search = { lat: liveImageMarkersNodes.lat, lng: liveImageMarkersNodes.lng };
                                if (!markerExists(search)) {

                                    var gpsLatLng =
                                        new google.maps.LatLng(liveImageMarkersNodes.lat, liveImageMarkersNodes.lng);

                                    var marker = new google.maps.Marker({
                                        position: gpsLatLng,
                                        map: map,
                                        icon: liveImageMarkersNodes.fileName
                                    });

                                    markers.push(marker);
                                }

                            });
                        });
                },
                5000);
        });
    };

    ns.displayAndPushLiveImageMarker = displayAndPushLiveImageMarker;
    ns.markers = [];
    function loadJsonConfig(callback) {
        $.getJSON(ns.configJson,
            function (data) {
                callback(data);
            });
    }

})(window.milosev);

window.onload = function () {
    window.milosev.displayAndPushLiveImageMarker(window.milosev.markers);
};