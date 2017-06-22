var measurements = window.model.measurements;
var coordinates = window.model.coordinates;

function initMap() {

    var map = new google.maps.Map(document.getElementById('map'));
    var bounds = new google.maps.LatLngBounds();
    var coordInfoWindow = new google.maps.InfoWindow();

    var ranges = {
        aqIndex: {
            1: "darkgreen",
            2: "green",
            3: "yellow",
            4: "orange",
            5: "red",
            6: "maroon"
        },

        SO2: {
            50: "darkgreen",
            100: "green",
            200: "yellow",
            350: "orange",
            500: "red",
            501: "maroon"
        },

        PM10: {
            20: "darkgreen",
            60: "green",
            100: "yellow",
            140: "orange",
            200: "red",
            201: "maroon"
        },

        CO: {
            2: "darkgreen",
            6: "green",
            10: "yellow",
            14: "orange",
            20: "red",
            21: "maroon"
        },

        "PM2.5": {
            12: "darkgreen",
            36: "green",
            60: "yellow",
            84: "orange",
            120: "red",
            121: "maroon"
        },

        O3: {
            30: "darkgreen",
            70: "green",
            120: "yellow",
            160: "orange",
            240: "red",
            241: "maroon"
        },

        NO2: {
            40: "darkgreen",
            100: "green",
            150: "yellow",
            200: "orange",
            400: "red",
            401: "maroon"
        },

        C6H6: {
            5: "darkgreen",
            10: "green",
            15: "yellow",
            20: "orange",
            50: "red",
            51: "maroon"
        }
    };
    //icon obrazki 15x15px
    var chooseIcon = function (value, element) {
        var range = ranges[element];

        var keys = Object.keys(range);

        for (var j = 0; j < keys.length; j++) {
            if (value < parseInt(keys[j])) {
                return range[keys[j]];
            }
        }
        return range[keys[-1]];
    }

    var markers = [];
    window.markers = markers;
    for (var i = 0; i < measurements.length; i++) {
        var measurement = measurements[i];
        var stationPosition = coordinates[measurement.stationId];
        if (stationPosition == null) {
            console.log("Brak koordynat dla stacji: ", measurement.stationId);
            continue;
        }
        var position = new google.maps.LatLng(stationPosition.latitude, stationPosition.longitude);
        bounds.extend(position);
        var marker = new google.maps.Marker({
            position: position,
            map: map,
            icon: "/images/" + chooseIcon(measurement.aqIndex, "aqIndex")+ ".png",
            title: measurement.stationName
        });

        markers.push(marker);

        // Allow each marker to have an info window    
        var showContent = (function (marker, measurement) {
            return function() {
                coordInfoWindow.setContent(
                    createInfoWindowContent(marker, measurement));
                coordInfoWindow.open(map, marker);
            }
        })(marker, measurement, stationPosition);

        google.maps.event.addListener(marker, 
            'click',
            showContent
        );

        // Automatically center the map fitting all markers on the screen
        map.fitBounds(bounds);
    }
    $('#legend').html('<img src="/images/legendaqIndex.png">');

    // Override our map zoom level once our fitBounds function runs (Make sure it only runs once)
    var boundsListener = google.maps.event.addListener((map), 'bounds_changed', function () {
        this.setZoom(6);
        google.maps.event.removeListener(boundsListener);
    });

    $("#list-form-pollutions input").click(function() {
        var element = this.value;

        markers.forEach(function (marker, index) {
            var valueOfElement;
            if (element !== "aqIndex") {
                valueOfElement = measurements[index].values[element];
            } else {
                valueOfElement = measurements[index].aqIndex;
            }

            if (valueOfElement != null) {
                marker.icon = "/images/" + chooseIcon(valueOfElement, element) + ".png",
                marker.setMap(null);
                marker.setMap(map);
            } else {
                marker.setMap(null);
            }
        });
        $('#legend').html('<img src="/images/legend' + element + '.png">');

    });
}

function createInfoWindowContent(marker, measurement) {
    return [
        { key: 'Stacja: ', value: measurement.stationName },
        { key: 'Wsakźnik powietrza: ', value: measurement.aqIndex },
        { key: 'C6H6: ', value: measurement.values.C6H6 },
        { key: 'PM10: ', value: measurement.values.PM10 },
        { key: 'SO2: ', value: measurement.values.SO2 },
        { key: 'NO2: ', value: measurement.values.NO2 },
        { key: 'CO: ', value: measurement.values.CO },
        { key: 'PM2.5: ', value: measurement.values.PM25 },
        { key: 'O3: ', value: measurement.values.O3 }
    ].filter(m => m.value != null)
        .map(m => m.key + m.value)
        .join('<br>');
}










