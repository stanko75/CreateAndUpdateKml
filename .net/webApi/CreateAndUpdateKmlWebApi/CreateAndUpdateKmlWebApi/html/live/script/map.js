/*global google, $*/
(function (ns) {
    "use strict";
    var map,
        gpsLatLng,
        marker;

    function initMap() {
        var kmlUrl,
            currentLocation;
        loadJsonConfig(function (config) {
            try {
                kmlUrl = config.kmlUrl;
                currentLocation = config.currentLocation;

                map = new google.maps.Map(document.getElementById('map-canvas'),
                    {
                        center: { lat: 50.7541783, lng: 7.0881042 },
                        scrollWheel: true,
                        zoom: 10
                    });

                var kmlLayer = new google.maps.KmlLayer({
                    url: kmlUrl + "?dummy=" + (new Date()).getTime(),
                    map: map,
                    preserveViewport: true

                });

                google.maps.event.addListener(kmlLayer,
                    "metadata_changed",
                    function () {
                        console.log("metadata_changed");
                        map.setCenter(gpsLatLng);
                    });

                google.maps.event.addListener(kmlLayer,
                    'status_changed',
                    function () {
                        if (kmlLayer.getStatus() == google.maps.KmlLayerStatus.OK) {
                            map.setCenter(gpsLatLng);
                        } else {
                            // Failure
                        }
                    });

                $.getJSON(currentLocation,
                    function (data) {
                        gpsLatLng = new google.maps.LatLng(data.lat, data.lng);

                        marker = new google.maps.Marker({
                            position: gpsLatLng,
                            map: map
                        });

                        map.setCenter(gpsLatLng);
                    }).done(function () {
                        console.log("second success");
                    }).fail(function (xhr, status, error) {
                        alert("An AJAX error occured: " + xhr.statusCode().status + "\nError: " + error);
                    }).always(function () {
                        console.log("finished");
                    });

                window.setInterval(function () {
                    google.maps.event.trigger(map, 'resize');
                    kmlLayer = new google.maps.KmlLayer({
                        url: kmlUrl + "?dummy=" + (new Date()).getTime(),
                        map: map,
                        preserveViewport: true
                    });

                    $.getJSON(currentLocation,
                        function (data) {
                            gpsLatLng = new google.maps.LatLng(data.lat, data.lng);

                            marker.setPosition(gpsLatLng);

                            map.setCenter(gpsLatLng);
                        }).done(function () {
                            console.log("second success");
                        }).fail(function (xhr, status, error) {
                            alert("An AJAX error occured: " + xhr.statusCode().status + "\nError: " + error);
                        }).always(function () {
                            console.log("finished");
                        });

                    /*
                    map.setCenter(map.getCenter());
                    map.setZoom(map.getZoom());
                    */
                },
                    5000);

                ns.map = map;
            } catch (e) {
                console.log(e);
                setTimeout(function () {
                    if (typeof google !== 'object') {
                        location.reload();
                    }
                },
                    1000);
            }
        });
    }

    function loadJsonConfig(callback) {
        $.getJSON("../../../config.json",
            function (data) {
                callback(data);
            });
    }

    ns.initMap = initMap;

})(window.milosev);