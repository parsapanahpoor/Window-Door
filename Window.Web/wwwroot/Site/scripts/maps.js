 (function($){
    "use strict";


    // Marker
    // ----------------------------------------------- //
    var markerIcon = {
        path: 'M19.9,0c-0.2,0-1.6,0-1.8,0C8.8,0.6,1.4,8.2,1.4,17.8c0,1.4,0.2,3.1,0.5,4.2c-0.1-0.1,0.5,1.9,0.8,2.6c0.4,1,0.7,2.1,1.2,3 c2,3.6,6.2,9.7,14.6,18.5c0.2,0.2,0.4,0.5,0.6,0.7c0,0,0,0,0,0c0,0,0,0,0,0c0.2-0.2,0.4-0.5,0.6-0.7c8.4-8.7,12.5-14.8,14.6-18.5 c0.5-0.9,0.9-2,1.3-3c0.3-0.7,0.9-2.6,0.8-2.5c0.3-1.1,0.5-2.7,0.5-4.1C36.7,8.4,29.3,0.6,19.9,0z M2.2,22.9 C2.2,22.9,2.2,22.9,2.2,22.9C2.2,22.9,2.2,22.9,2.2,22.9C2.2,22.9,3,25.2,2.2,22.9z M19.1,26.8c-5.2,0-9.4-4.2-9.4-9.4 s4.2-9.4,9.4-9.4c5.2,0,9.4,4.2,9.4,9.4S24.3,26.8,19.1,26.8z M36,22.9C35.2,25.2,36,22.9,36,22.9C36,22.9,36,22.9,36,22.9 C36,22.9,36,22.9,36,22.9z M13.8,17.3a5.3,5.3 0 1,0 10.6,0a5.3,5.3 0 1,0 -10.6,0',
        strokeOpacity: 0,
        strokeWeight: 1,
        fillColor: '#274abb',
        fillOpacity: 1,
        rotation: 0,
        scale: 1,
        anchor: new google.maps.Point(19,50)
    }

    /* Half Map Adjustments */
    $(window).on('load resize', function() {

      var topbarHeight = $("#top-bar").height();
      var headerHeight = $("#header").innerHeight() + topbarHeight;

      $(".fs-container").css('height', '' + $(window).height() - headerHeight +'px');
    });


    // Main Main
    // ----------------------------------------------- //
    function mainMap() {

      function locationData(locationURL,locationPrice,locationPriceDetails,locationImg,locationTitle,locationAddress) {
          return('<a href="'+ locationURL +'" class="listing-img-container"><div class="infoBox-close"><i class="fa fa-times"></i></div><div class="listing-img-content"><span class="listing-price">'+ locationPrice +'<i>' + locationPriceDetails +'</i></span></div><img src="'+locationImg+'" alt=""></a><div class="listing-content"><div class="listing-title"><h4><a href="#">'+locationTitle+'</a></h4><p>'+locationAddress+'</p></div></div>')
      }

      var locations = [
        [ locationData('single-property-page-1.html','$275,000','$520 / sq ft','images/listing-01.jpg','Eagle Apartmets',"9364 School St. Lynchburg, NY "), 40.7427837, -73.11445617675781, 1, markerIcon ],
        [ locationData('single-property-page-1.html','$900','monthly','images/listing-02.jpg','Serene Uptown',"6 Bishop Ave. Perkasie, PA"), 40.70437865245596, -73.98674011230469, 2, markerIcon ],
        [ locationData('single-property-page-1.html','$425,000','$770 / sq ft','images/listing-04.jpg','Selway Apartments',"33 William St. Northbrook, IL "), 40.94401669296697, -74.16938781738281, 3, markerIcon ],
        [ locationData('single-property-page-1.html','$535,000','$640 / sq ft','images/listing-05.jpg','Oak Tree Villas',"71 Lower River Dr. Bronx, NY "), 40.77055783505125, -74.26002502441406, 4, markerIcon ],

        [ locationData('single-property-page-1.html','$1700','monthly','images/listing-03.jpg','Meridian Villas',"778 Country St. Panama City, FL"), 41.79424986338271, -87.7093505859375, 5, markerIcon ],
        [ locationData('single-property-page-1.html','$500','monthly','images/listing-06.jpg','Old Town Manchester',"7843 Durham Avenue, MD"), 41.76967281691741, -87.9510498046875, 6, markerIcon ],

        [ locationData('single-property-page-1.html','$275,000','$520 / sq ft','images/listing-01.jpg','Eagle Apartmets',"9364 School St. Lynchburg, NY "), 36.13610021320376, -115.1312255859375, 7, markerIcon ],
        [ locationData('single-property-page-1.html','$900','monthly','images/listing-02.jpg','Serene Uptown',"6 Bishop Ave. Perkasie, PA"), 36.10637081203522, -115.22872924804688, 8, markerIcon ],

        [ locationData('single-property-page-1.html','$1700','monthly','images/listing-03.jpg','Meridian Villas',"778 Country St. Panama City, FL"), 32.86020942314693, -97.09442138671875, 9, markerIcon ],
        [ locationData('single-property-page-1.html','$425,000','$770 / sq ft','images/listing-04.jpg','Selway Apartments',"33 William St. Northbrook, IL "), 32.80943825730526, -96.88018798828125, 10, markerIcon ],
        [ locationData('single-property-page-1.html','$535,000','$640 / sq ft','images/listing-05.jpg','Oak Tree Villas',"71 Lower River Dr. Bronx, NY "), 32.684695132205626, -96.89666748046875, 11, markerIcon ],
      ];


      var mapZoomAttr = $('#map').attr('data-map-zoom');
      var mapScrollAttr = $('#map').attr('data-map-scroll');

      if (typeof mapZoomAttr !== typeof undefined && mapZoomAttr !== false) {
          var zoomLevel = parseInt(mapZoomAttr);
      } else {
          var zoomLevel = 5;
      }

      if (typeof mapScrollAttr !== typeof undefined && mapScrollAttr !== false) {
         var scrollEnabled = parseInt(mapScrollAttr);
      } else {
        var scrollEnabled = false;
      }

      var map = new google.maps.Map(document.getElementById('map'), {
        zoom: zoomLevel,
        scrollwheel: scrollEnabled,
        center: new google.maps.LatLng(38, -96),
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        zoomControl: false,
        mapTypeControl: false,
        scaleControl: false,
        panControl: false,
        navigationControl: false,
        streetViewControl: false,
        gestureHandling: 'cooperative',

        // Google Map Style
        styles: [{"featureType":"poi","elementType":"labels.text.fill","stylers":[{"color":"#747474"},{"lightness":"23"}]},{"featureType":"poi.attraction","elementType":"geometry.fill","stylers":[{"color":"#f38eb0"}]},{"featureType":"poi.government","elementType":"geometry.fill","stylers":[{"color":"#ced7db"}]},{"featureType":"poi.medical","elementType":"geometry.fill","stylers":[{"color":"#ffa5a8"}]},{"featureType":"poi.park","elementType":"geometry.fill","stylers":[{"color":"#c7e5c8"}]},{"featureType":"poi.place_of_worship","elementType":"geometry.fill","stylers":[{"color":"#d6cbc7"}]},{"featureType":"poi.school","elementType":"geometry.fill","stylers":[{"color":"#c4c9e8"}]},{"featureType":"poi.sports_complex","elementType":"geometry.fill","stylers":[{"color":"#b1eaf1"}]},{"featureType":"road","elementType":"geometry","stylers":[{"lightness":"100"}]},{"featureType":"road","elementType":"labels","stylers":[{"visibility":"off"},{"lightness":"100"}]},{"featureType":"road.highway","elementType":"geometry.fill","stylers":[{"color":"#ffd4a5"}]},{"featureType":"road.arterial","elementType":"geometry.fill","stylers":[{"color":"#ffe9d2"}]},{"featureType":"road.local","elementType":"all","stylers":[{"visibility":"simplified"}]},{"featureType":"road.local","elementType":"geometry.fill","stylers":[{"weight":"3.00"}]},{"featureType":"road.local","elementType":"geometry.stroke","stylers":[{"weight":"0.30"}]},{"featureType":"road.local","elementType":"labels.text","stylers":[{"visibility":"on"}]},{"featureType":"road.local","elementType":"labels.text.fill","stylers":[{"color":"#747474"},{"lightness":"36"}]},{"featureType":"road.local","elementType":"labels.text.stroke","stylers":[{"color":"#e9e5dc"},{"lightness":"30"}]},{"featureType":"transit.line","elementType":"geometry","stylers":[{"visibility":"on"},{"lightness":"100"}]},{"featureType":"water","elementType":"all","stylers":[{"color":"#d2e7f7"}]}]

      });


      var boxText = document.createElement("div");
      boxText.className = 'map-box'

      var currentInfobox;

      var boxOptions = {
              content: boxText,
              disableAutoPan: true,
              alignBottom : true,
              maxWidth: 0,
              pixelOffset: new google.maps.Size(-60, -55),
              zIndex: null,
              boxStyle: {
                width: "260px"
              },
              closeBoxMargin: "0",
              closeBoxURL: "",
              infoBoxClearance: new google.maps.Size(1, 1),
              isHidden: false,
              pane: "floatPane",
              enableEventPropagation: false,
      };


      var markerCluster, marker, i;
      var allMarkers = [];

      var clusterStyles = [
      {
        textColor: 'white',
        url: '',
        height: 50,
        width: 50
      }
      ];



        // Custom zoom buttons
        var zoomControlDiv = document.createElement('div');
        var zoomControl = new ZoomControl(zoomControlDiv, map);

        function ZoomControl(controlDiv, map) {

          zoomControlDiv.index = 1;
          map.controls[google.maps.ControlPosition.RIGHT_CENTER].push(zoomControlDiv);
          // Creating divs & styles for custom zoom control
          controlDiv.style.padding = '5px';

          // Set CSS for the control wrapper
          var controlWrapper = document.createElement('div');
          controlDiv.appendChild(controlWrapper);

          // Set CSS for the zoomIn
          var zoomInButton = document.createElement('div');
          zoomInButton.className = "custom-zoom-in";
          controlWrapper.appendChild(zoomInButton);

          // Set CSS for the zoomOut
          var zoomOutButton = document.createElement('div');
          zoomOutButton.className = "custom-zoom-out";
          controlWrapper.appendChild(zoomOutButton);

          // Setup the click event listener - zoomIn
          google.maps.event.addDomListener(zoomInButton, 'click', function() {
            map.setZoom(map.getZoom() + 1);
          });

          // Setup the click event listener - zoomOut
          google.maps.event.addDomListener(zoomOutButton, 'click', function() {
            map.setZoom(map.getZoom() - 1);
          });

      }


      for (i = 0; i < locations.length; i++) {
        marker = new google.maps.Marker({
          position: new google.maps.LatLng(locations[i][1], locations[i][2]),

          icon: locations[i][4],
          id : i
        });
        allMarkers.push(marker);

        var ib = new InfoBox();

        google.maps.event.addListener(marker, 'click', (function(marker, i) {


          return function() {
             ib.setOptions(boxOptions);
             boxText.innerHTML = locations[i][0];
             ib.close();
             ib.open(map, marker);
             
             currentInfobox = marker.id;
             var latLng = new google.maps.LatLng(locations[i][1], locations[i][2]);
             map.panTo(latLng);
             map.panBy(0,-180);


            google.maps.event.addListener(ib,'domready',function(){
              $('.infoBox-close').click(function(e) {
            e.preventDefault();
                  ib.close();
              });
            });

          }
        })(marker, i));

      } //eof for

      var options = {
          imagePath: 'images/',
          styles : clusterStyles,
          minClusterSize : 2

      };

      markerCluster = new MarkerClusterer(map, allMarkers, options);

      google.maps.event.addDomListener(window, "resize", function() {
          var center = map.getCenter();
          google.maps.event.trigger(map, "resize");
          map.setCenter(center);
      });


      // Scroll enabling button
      var scrollEnabling = $('#scrollEnabling');

      $(scrollEnabling).click(function(e){
          e.preventDefault();
          $(this).toggleClass("enabled");

          if ( $(this).is(".enabled") ) {
             map.setOptions({'scrollwheel': true});
          } else {
             map.setOptions({'scrollwheel': false});
          }
      })


      // Geo location button
      $("#geoLocation").click(function(e){
          e.preventDefault();
          geolocate();
      });

      function geolocate() {

          if (navigator.geolocation) {
              navigator.geolocation.getCurrentPosition(function (position) {
                  var pos = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                  map.setCenter(pos);
                  map.setZoom(12);
              });
          }
      }


      // Next / Prev buttons
        $('#nextpoint').click(function(e){
            e.preventDefault();

             map.setZoom(15);

            var index = currentInfobox;
            if (index+1 < allMarkers.length ) {
                google.maps.event.trigger(allMarkers[index+1],'click');

            } else {
                google.maps.event.trigger(allMarkers[0],'click');
            }
        });


        $('#prevpoint').click(function(e){
            e.preventDefault();

             map.setZoom(15);

            if ( typeof(currentInfobox) == "undefined" ) {
                 google.maps.event.trigger(allMarkers[allMarkers.length-1],'click');
            } else {
                 var index = currentInfobox;
                 if(index-1 < 0) {
                    //if index is less than zero than open last marker from array
                   google.maps.event.trigger(allMarkers[allMarkers.length-1],'click');
                 } else {
                    google.maps.event.trigger(allMarkers[index-1],'click');
                 }
            }
      });

    }


    // Map Init
    var map =  document.getElementById('map');
    if (typeof(map) != 'undefined' && map != null) {
      google.maps.event.addDomListener(window, 'load',  mainMap);
    }


    // ---------------- Main Map / End ---------------- //



    // Single Property Map
    // ----------------------------------------------- //

    function singlePropertyMap() {

      var myLatLng = {lng: $( '#propertyMap' ).data('longitude'),lat: $( '#propertyMap' ).data('latitude'), };

      var single_map = new google.maps.Map(document.getElementById('propertyMap'), {
        zoom: 13,
        center: myLatLng,
        scrollwheel: false,
        zoomControl: false,
        mapTypeControl: false,
        scaleControl: false,
        panControl: false,
        navigationControl: false,
        streetViewControl: false,
        styles:  [{"featureType":"poi","elementType":"labels.text.fill","stylers":[{"color":"#747474"},{"lightness":"23"}]},{"featureType":"poi.attraction","elementType":"geometry.fill","stylers":[{"color":"#f38eb0"}]},{"featureType":"poi.government","elementType":"geometry.fill","stylers":[{"color":"#ced7db"}]},{"featureType":"poi.medical","elementType":"geometry.fill","stylers":[{"color":"#ffa5a8"}]},{"featureType":"poi.park","elementType":"geometry.fill","stylers":[{"color":"#c7e5c8"}]},{"featureType":"poi.place_of_worship","elementType":"geometry.fill","stylers":[{"color":"#d6cbc7"}]},{"featureType":"poi.school","elementType":"geometry.fill","stylers":[{"color":"#c4c9e8"}]},{"featureType":"poi.sports_complex","elementType":"geometry.fill","stylers":[{"color":"#b1eaf1"}]},{"featureType":"road","elementType":"geometry","stylers":[{"lightness":"100"}]},{"featureType":"road","elementType":"labels","stylers":[{"visibility":"off"},{"lightness":"100"}]},{"featureType":"road.highway","elementType":"geometry.fill","stylers":[{"color":"#ffd4a5"}]},{"featureType":"road.arterial","elementType":"geometry.fill","stylers":[{"color":"#ffe9d2"}]},{"featureType":"road.local","elementType":"all","stylers":[{"visibility":"simplified"}]},{"featureType":"road.local","elementType":"geometry.fill","stylers":[{"weight":"3.00"}]},{"featureType":"road.local","elementType":"geometry.stroke","stylers":[{"weight":"0.30"}]},{"featureType":"road.local","elementType":"labels.text","stylers":[{"visibility":"on"}]},{"featureType":"road.local","elementType":"labels.text.fill","stylers":[{"color":"#747474"},{"lightness":"36"}]},{"featureType":"road.local","elementType":"labels.text.stroke","stylers":[{"color":"#e9e5dc"},{"lightness":"30"}]},{"featureType":"transit.line","elementType":"geometry","stylers":[{"visibility":"on"},{"lightness":"100"}]},{"featureType":"water","elementType":"all","stylers":[{"color":"#d2e7f7"}]}]

      });

      var marker = new google.maps.Marker({
          position: myLatLng,
          map: single_map,
          icon: markerIcon
        });


      // Custom zoom buttons
      var zoomControlDiv = document.createElement('div');
      var zoomControl = new ZoomControl(zoomControlDiv, single_map);

      function ZoomControl(controlDiv, single_map) {

      zoomControlDiv.index = 1;
      single_map.controls[google.maps.ControlPosition.RIGHT_CENTER].push(zoomControlDiv);
      // Creating divs & styles for custom zoom control
      controlDiv.style.padding = '5px';

      // Set CSS for the control wrapper
      var controlWrapper = document.createElement('div');
      controlDiv.appendChild(controlWrapper);

      // Set CSS for the zoomIn
      var zoomInButton = document.createElement('div');
      zoomInButton.className = "custom-zoom-in";
      controlWrapper.appendChild(zoomInButton);

      // Set CSS for the zoomOut
      var zoomOutButton = document.createElement('div');
      zoomOutButton.className = "custom-zoom-out";
      controlWrapper.appendChild(zoomOutButton);

      // Setup the click event listener - zoomIn
      google.maps.event.addDomListener(zoomInButton, 'click', function() {
        single_map.setZoom(single_map.getZoom() + 1);
      });

      // Setup the click event listener - zoomOut
      google.maps.event.addDomListener(zoomOutButton, 'click', function() {
        single_map.setZoom(single_map.getZoom() - 1);
      });

      }

      $('#streetView').click(function(e){
         e.preventDefault();
         single_map.getStreetView().setOptions({visible:true,position:myLatLng});
         $(this).css('display', 'none')
      });

    }


    // Single Property Map Init
    var single_map =  document.getElementById('propertyMap');
    if (typeof(single_map) != 'undefined' && single_map != null) {
      google.maps.event.addDomListener(window, 'load',  singlePropertyMap);
    }


    // -------------- Single Property Map / End -------------- //


})(this.jQuery);
