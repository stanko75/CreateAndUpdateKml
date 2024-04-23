(function (ns) {
    /*globals google, $*/
    "use strict";

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
        loadJsonConfig(function (config) {
            window.setInterval(function () {
                $.getJSON(config.LiveImageMarkersJsonUrl,
                    function (data) {
                        data.forEach(function (liveImageMarkersNodes) {

                            var search = { lat: liveImageMarkersNodes.lat, lng: liveImageMarkersNodes.lng };
                            if (!markerExists(search)) {

                                var gpsLatLng =
                                    new google.maps.LatLng(liveImageMarkersNodes.lat, liveImageMarkersNodes.lng);

                                var marker = new google.maps.Marker({
                                    position: gpsLatLng,
                                    map: ns.map,
                                    icon: liveImageMarkersNodes.fileName
                                });

                                ns.map.setCenter(gpsLatLng);

                                markers.push(marker);
                            }
                        });
                    }).done(function () {
                        console.log("success");
                    }).fail(function (xhr, status, error) {
                        //alert("An AJAX error occured: " + xhr.statusCode().status + "\nError: " + error);
                        console.log("An AJAX error occured: " + xhr.statusCode().status + "\nError: " + error);
                    }).always(function () {
                        console.log("finished");
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
            }).done(function () {
                console.log("success");
            }).fail(function (xhr, status, error) {
                //alert("An AJAX error occured: " + xhr.statusCode().status + "\nError: " + error);
                console.log("An AJAX error occured: " + xhr.statusCode().status + "\nError: " + error);
            }).always(function () {
                console.log("finished");
            });
    }

})(window.milosev);

window.onload = function () {
    window.milosev.displayAndPushLiveImageMarker(window.milosev.markers);
};