/* ----------------- Start Document ----------------- */
(function($){
"use strict";

$(document).ready(function(){

	/*--------------------------------------------------*/
	/*  Mobile Menu - mmenu.js
	/*--------------------------------------------------*/
	$(function() {
		function mmenuInit() {
			var wi = $(window).width();
			if(wi <= '992') {

				$('#footer').removeClass("sticky-footer");

				$(".mmenu-init" ).remove();
				$("#navigation").clone().addClass("mmenu-init").insertBefore("#navigation").removeAttr('id').removeClass('style-1 style-2').find('ul').removeAttr('id');
				$(".mmenu-init").find(".container").removeClass("container");

				$(".mmenu-init").mmenu({
				 	"counters": true
				}, {
				 // configuration
				 offCanvas: {
				    pageNodetype: "#wrapper"
				 }
				});

				var mmenuAPI = $(".mmenu-init").data( "mmenu" );
				var $icon = $(".hamburger");

				$(".mmenu-trigger").click(function() {
					mmenuAPI.open();
				});

				mmenuAPI.bind( "open:finish", function() {
				   setTimeout(function() {
				      $icon.addClass( "is-active" );
				   });
				});
				mmenuAPI.bind( "close:finish", function() {
				   setTimeout(function() {
				      $icon.removeClass( "is-active" );
				   });
				});


			}
			$(".mm-next").addClass("mm-fullsubopen");
		}
		mmenuInit();
		$(window).resize(function() { mmenuInit(); });
	});

    /*  User Menu */
    $('.user-menu').on('click', function(){
		$(this).toggleClass('active');
	});


	/*----------------------------------------------------*/
	/*  Sticky Header
	/*----------------------------------------------------*/
	$( "#header" ).not( "#header-container.header-style-2 #header" ).clone(true).addClass('cloned unsticky').insertAfter( "#header" );
	$( "#navigation.style-2" ).clone(true).addClass('cloned unsticky').insertAfter( "#navigation.style-2" );

	// Logo for header style 2
	$( "#logo .sticky-logo" ).clone(true).prependTo("#navigation.style-2.cloned ul#responsive");


	// sticky header script
	var headerOffset = $("#header-container").height() * 2; // height on which the sticky header will shows

	$(window).scroll(function(){
		if($(window).scrollTop() >= headerOffset){
			$("#header.cloned").addClass('sticky').removeClass("unsticky");
			$("#navigation.style-2.cloned").addClass('sticky').removeClass("unsticky");
		} else {
			$("#header.cloned").addClass('unsticky').removeClass("sticky");
			$("#navigation.style-2.cloned").addClass('unsticky').removeClass("sticky");
		}
	});


	/*----------------------------------------------------*/
	/* Top Bar Dropdown Menu
	/*----------------------------------------------------*/

	$('.top-bar-dropdown').on('click', function(event){
		$('.top-bar-dropdown').not(this).removeClass('active');
		if ($(event.target).parent().parent().attr('class') == 'options' ) {
			hideDD();
		} else {
			if($(this).hasClass('active') &&  $(event.target).is( "span" )) {
				hideDD();
			} else {
				$(this).toggleClass('active');
			}
		}
		event.stopPropagation();
	});

	$(document).on('click', function(e){ hideDD(); });

	function hideDD(){
		$('.top-bar-dropdown').removeClass('active');
	}


	/*----------------------------------------------------*/
	/* Advanced Search Button
	/*----------------------------------------------------*/
	$('#map-container .adv-search-btn').on('click', function(e){

		if ( $(this).is(".active") ) {

			$(this).removeClass("active");
			$(".main-search-container").removeClass("active");
			setTimeout( function() {
				$("#map-container.homepage-map").removeClass("overflow")
			}, 0);

		} else {

			$(this).addClass("active");
			$(".main-search-container").addClass("active");
			setTimeout( function() {
				$("#map-container.homepage-map").addClass("overflow")
			}, 400);

		}

		e.preventDefault();
	});



	/*----------------------------------------------------*/
	/*  Inline CSS replacement for backgrounds etc.
	/*----------------------------------------------------*/
	function inlineCSS() {

		// Common Inline CSS
		$(".some-classes, section.fullwidth, .img-box-background, .flip-banner, .property-slider .item, .fullwidth-property-slider .item, .fullwidth-home-slider .item, .address-container").each(function() {
			var attrImageBG = $(this).attr('data-background-image');
			var attrColorBG = $(this).attr('data-background-color');

	        if(attrImageBG !== undefined) {
	            $(this).css('background-image', 'url('+attrImageBG+')');
	        }

	        if(attrColorBG !== undefined) {
	            $(this).css('background', ''+attrColorBG+'');
	        }
		});

	}

	// Init
	inlineCSS();

	function parallaxBG() {

		$('.parallax').prepend('<div class="parallax-overlay"></div>');

		$( ".parallax").each(function() {
			var attrImage = $(this).attr('data-background');
			var attrColor = $(this).attr('data-color');
			var attrOpacity = $(this).attr('data-color-opacity');

	        if(attrImage !== undefined) {
	            $(this).css('background-image', 'url('+attrImage+')');
	        }

	        if(attrColor !== undefined) {
	            $(this).find(".parallax-overlay").css('background-color', ''+attrColor+'');
	        }

	        if(attrOpacity !== undefined) {
	            $(this).find(".parallax-overlay").css('opacity', ''+attrOpacity+'');
	        }

		});
	}

	parallaxBG();


	// Slide to anchor
	$('#titlebar .listing-address').on('click', function(e){
	    e.preventDefault();

	    $('html, body').animate({
	        scrollTop: $( $.attr(this, 'href') ).offset().top-40
	    }, 600);
	});


	/*----------------------------------------------------*/
	/*  Tooltips
	/*----------------------------------------------------*/

	$(".tooltip.top").tipTip({
	  defaultPosition: "top"
	});

	$(".tooltip.bottom").tipTip({
	  defaultPosition: "bottom"
	});

	$(".tooltip.left").tipTip({
	  defaultPosition: "left"
	});

	$(".tooltip.right").tipTip({
	  defaultPosition: "right"
	});



	/*----------------------------------------------------*/
	/*  Mortgage Calculator
	/*----------------------------------------------------*/

	// Gets property price
	var propertyPricing = parseFloat($('.property-price').text().replace(/[^0-9\.]+/g,""));
	if (propertyPricing > 0) {
		$('.pick-price').on('click', function(){
			$('#amount').val(parseInt(propertyPricing));
		});
	}

	// replacing comma with dot
	$(document).on('change', function() {
		$("#interest").val($("#interest").val().replace(/,/g, '.'));
	});

	// Calculator
	function mortgageCalc() {

		var amount = parseFloat($("#amount").val().replace(/[^0-9\.]+/g,"")),
			months =parseFloat($("#years").val().replace(/[^0-9\.]+/g,"")*12),
			down = parseFloat($("#downpayment").val().replace(/[^0-9\.]+/g,"")),
			annInterest = parseFloat($("#interest").val().replace(/[^0-9\.]+/g,"")),
			monInt = annInterest / 1200,
			calculation = ((monInt + monInt / (Math.pow(1 + monInt, months) - 1)) * (amount - (down || 0))).toFixed(2);

			if (calculation > 0 ){
				$(".calc-output-container").css({'opacity' : '1', 'max-height' : '200px' });
				$(".calc-output").hide().html(calculation + ' ' + $('.mortgageCalc').attr("data-calc-currency")).fadeIn(300);
			}
	}

	// Calculate
	$('.calc-button').on('click', function(){
		mortgageCalc();
	});



	/*----------------------------------------------------*/
	/*  Parallax
	/*----------------------------------------------------*/

	/* detect touch */
	if("ontouchstart" in window){
	    document.documentElement.className = document.documentElement.className + " touch";
	}
	if(!$("html").hasClass("touch")){
	    /* background fix */
	    $(".parallax").css("background-attachment", "fixed");
	}

	/* fix vertical when not overflow
	call fullscreenFix() if .fullscreen content changes */
	function fullscreenFix(){
	    var h = $('body').height();
	    // set .fullscreen height
	    $(".content-b").each(function(i){
	        if($(this).innerHeight() > h){ $(this).closest(".fullscreen").addClass("overflow");
	        }
	    });
	}
	$(window).resize(fullscreenFix);
	fullscreenFix();

	/* resize background images */
	function backgroundResize(){
	    var windowH = $(window).height();
	    $(".parallax").each(function(i){
	        var path = $(this);
	        // variables
	        var contW = path.width();
	        var contH = path.height();
	        var imgW = path.attr("data-img-width");
	        var imgH = path.attr("data-img-height");
	        var ratio = imgW / imgH;
	        // overflowing difference
	        var diff = 100;
	        diff = diff ? diff : 0;
	        // remaining height to have fullscreen image only on parallax
	        var remainingH = 0;
	        if(path.hasClass("parallax") && !$("html").hasClass("touch")){
	            //var maxH = contH > windowH ? contH : windowH;
	            remainingH = windowH - contH;
	        }
	        // set img values depending on cont
	        imgH = contH + remainingH + diff;
	        imgW = imgH * ratio;
	        // fix when too large
	        if(contW > imgW){
	            imgW = contW;
	            imgH = imgW / ratio;
	        }
	        //
	        path.data("resized-imgW", imgW);
	        path.data("resized-imgH", imgH);
	        path.css("background-size", imgW + "px " + imgH + "px");
	    });
	}


	$(window).resize(backgroundResize);
	$(window).focus(backgroundResize);
	backgroundResize();

	/* set parallax background-position */
	function parallaxPosition(e){
	    var heightWindow = $(window).height();
	    var topWindow = $(window).scrollTop();
	    var bottomWindow = topWindow + heightWindow;
	    var currentWindow = (topWindow + bottomWindow) / 2;
	    $(".parallax").each(function(i){
	        var path = $(this);
	        var height = path.height();
	        var top = path.offset().top;
	        var bottom = top + height;
	        // only when in range
	        if(bottomWindow > top && topWindow < bottom){
	            //var imgW = path.data("resized-imgW");
	            var imgH = path.data("resized-imgH");
	            // min when image touch top of window
	            var min = 0;
	            // max when image touch bottom of window
	            var max = - imgH + heightWindow;
	            // overflow changes parallax
	            var overflowH = height < heightWindow ? imgH - height : imgH - heightWindow; // fix height on overflow
	            top = top - overflowH;
	            bottom = bottom + overflowH;


	            // value with linear interpolation
	            // var value = min + (max - min) * (currentWindow - top) / (bottom - top);
	            var value = 0;
				if ( $('.parallax').is(".titlebar") ) {
				    value = min + (max - min) * (currentWindow - top) / (bottom - top) *2;
				} else {
					value = min + (max - min) * (currentWindow - top) / (bottom - top);
				}

	            // set background-position
	            var orizontalPosition = path.attr("data-oriz-pos");
	            orizontalPosition = orizontalPosition ? orizontalPosition : "50%";
	            $(this).css("background-position", orizontalPosition + " " + value + "px");
	        }
	    });
	}
	if(!$("html").hasClass("touch")){
	    $(window).resize(parallaxPosition);
	    //$(window).focus(parallaxPosition);
	    $(window).scroll(parallaxPosition);
	    parallaxPosition();
	}

	// Jumping background fix for IE
	if(navigator.userAgent.match(/Trident\/7\./)) { // if IE
	    $('body').on("mousewheel", function () {
	        event.preventDefault();

	        var wheelDelta = event.wheelDelta;
	        var currentScrollPosition = window.pageYOffset;
	        window.scrollTo(0, currentScrollPosition - wheelDelta);
	    });
	}


	/*----------------------------------------------------*/
	/*  Search Type Buttons
	/*----------------------------------------------------*/
	function searchTypeButtons() {

		// Radio attr reset
		$('.search-type label.active input[type="radio"]').prop('checked',true);

		// Positioning indicator arrow
		var buttonWidth = $('.search-type label.active').width();
		var arrowDist = $('.search-type label.active').position().left;
		$('.search-type-arrow').css('left', arrowDist + (buttonWidth/2) );

		$('.search-type label').on('change', function() {
		    $('.search-type input[type="radio"]').parent('label').removeClass('active');
		    $('.search-type input[type="radio"]:checked').parent('label').addClass('active');

			// Positioning indicator arrow
			var buttonWidth = $('.search-type label.active').width();
			var arrowDist = $('.search-type label.active').position().left;

			$('.search-type-arrow').css({
				'left': arrowDist + (buttonWidth/2),
				'transition':'left 0.4s cubic-bezier(.87,-.41,.19,1.44)'
			});
		});

	}

	// Init
	if ($(".main-search-form").length){
		searchTypeButtons();
		$(window).on('load resize', function() { searchTypeButtons(); });
	}


    /*----------------------------------------------------*/
    /*  Chosen Plugin
    /*----------------------------------------------------*/

    var config = {
      '.chosen-select'           : {disable_search_threshold: 10, width:"100%"},
      '.chosen-select-deselect'  : {allow_single_deselect:true, width:"100%"},
      '.chosen-select-no-single' : {disable_search_threshold:100, width:"100%"},
      '.chosen-select-no-single.no-search' : {disable_search_threshold:10, width:"100%"},
      '.chosen-select-no-results': {no_results_text:'Oops, nothing found!'},
      '.chosen-select-width'     : {width:"95%"}
    };

    for (var selector in config) {
	   	if (config.hasOwnProperty(selector)) {
	      $(selector).chosen(config[selector]);
	  	}
    }


    /*  Custom Input With Select
    /*----------------------------------------------------*/
	$('.select-input').each(function(){

		var thisContainer = $(this);
	    var $this = $(this).children('select'), numberOfOptions = $this.children('option').length;

	    $this.addClass('select-hidden');
	    $this.wrap('<div class="select"></div>');
	    $this.after('<div class="select-styled"></div>');
	    var $styledSelect = $this.next('div.select-styled');
	    $styledSelect.text($this.children('option').eq(0).text());

	    var $list = $('<ul />', {
	        'class': 'select-options'
	    }).insertAfter($styledSelect);

	    for (var i = 0; i < numberOfOptions; i++) {
	        $('<li />', {
	            text: $this.children('option').eq(i).text(),
	            rel: $this.children('option').eq(i).val()
	        }).appendTo($list);
	    }

	    var $listItems = $list.children('li');

	 	$list.wrapInner('<div class="select-list-container"></div>');


	    $(this).children('input').on('click', function(e){
	    	$('.select-options').hide();
	        e.stopPropagation();
	        $styledSelect.toggleClass('active').next('ul.select-options').toggle();
	     });

	    $(this).children('input').keypress(function() {
	        $styledSelect.removeClass('active');
	        $list.hide();
	    });


	    $listItems.on('click', function(e){
	        e.stopPropagation();
	        // $styledSelect.text($(this).text()).removeClass('active');
	        $(thisContainer).children('input').val( $(this).text() ).removeClass('active');
	        $this.val($(this).attr('rel'));
	        $list.hide();
	        //console.log($this.val());
	    });

	    $(document).on('click', function(e){
	        $styledSelect.removeClass('active');
	        $list.hide();
	    });


	    // Unit character
	    var fieldUnit = $(this).children('input').attr('data-unit');
	    $(this).children('input').before('<i class="data-unit">'+ fieldUnit + '</i>');


	});



    /*----------------------------------------------------*/
    /*  Searh Form More Options
    /*----------------------------------------------------*/
    $('.more-search-options-trigger').on('click', function(e){
    	e.preventDefault();
		$('.more-search-options, .more-search-options-trigger').toggleClass('active');
		$('.more-search-options.relative').animate({height: 'toggle', opacity: 'toggle'}, 300);
	});


	/*----------------------------------------------------*/
	/*  Compare Menu
	/*----------------------------------------------------*/
    $('.csm-trigger').on('click', function(){
		$('.compare-slide-menu').toggleClass('active');
	});

    $('.csm-mobile-trigger').on('click', function(){
		$('.compare-slide-menu').removeClass('active');
	});

    // Tooltips
	$(".compare-button.with-tip, .like-icon.with-tip, .widget-button.with-tip").each(function() {
		$(this).on('click', function(e){
	    	e.preventDefault();
		});
		var tipContent = $(this).attr('data-tip-content');
		$(this).append('<div class="tip-content">'+ tipContent + '</div>');
	});

	// Demo Purpose Trigger
	$('.compare-button, .compare-widget-button').on('click', function(){
		$('.compare-slide-menu').addClass('active');
	});

	$(".remove-from-compare").on('click', function(e){
    	e.preventDefault();
	});


    /*----------------------------------------------------*/
    /*  Like Icon Trigger
    /*----------------------------------------------------*/
    $('.like-icon, .widget-button').on('click', function(e){
    	e.preventDefault();
		$(this).toggleClass('liked');
		$(this).children('.like-icon').toggleClass('liked');
	});


    /*----------------------------------------------------*/
    /*  Show More Button
    /*----------------------------------------------------*/
    $('.show-more-button').on('click', function(e){
    	e.preventDefault();
		$('.show-more').toggleClass('visible');
	});


	/*----------------------------------------------------*/
	/*  Back to Top
	/*----------------------------------------------------*/
	  var pxShow = 600; // height on which the button will show
	  var fadeInTime = 300; // how slow / fast you want the button to show
	  var fadeOutTime = 300; // how slow / fast you want the button to hide
	  var scrollSpeed = 500; // how slow / fast you want the button to scroll to top.

	  $(window).scroll(function(){
		 if($(window).scrollTop() >= pxShow){
			$("#backtotop").fadeIn(fadeInTime);
		 } else {
			$("#backtotop").fadeOut(fadeOutTime);
		 }
	  });

	  $('#backtotop a').on('click', function(){
		 $('html, body').animate({scrollTop:0}, scrollSpeed);
		 return false;
	  });


    /*----------------------------------------------------*/
    /*  Owl Carousel
    /*----------------------------------------------------*/

	$('.carousel').owlCarousel({
		autoPlay: false,
		navigation: true,
		slideSpeed: 600,
		items : 3,
		itemsDesktop : [1239,3],
		itemsTablet : [991,2],
		itemsMobile : [767,1]
	});


	$('.logo-carousel').owlCarousel({
		autoPlay: false,
		navigation: true,
		slideSpeed: 600,
		items : 5,
		itemsDesktop : [1239,4],
		itemsTablet : [991,3],
		itemsMobile : [767,1]
	});


	$('.listing-carousel').owlCarousel({
		autoPlay: false,
		navigation: true,
		slideSpeed: 800,
		items : 1,
		itemsDesktop : [1239,1],
		itemsTablet : [991,1],
		itemsMobile : [767,1]
	});

    $('.owl-next, .owl-prev').on("click", function (e) {
        e.preventDefault();
     });



    /*----------------------------------------------------*/
    /*  Slick Carousel
    /*----------------------------------------------------*/
	 $('.property-slider').slick({
		slidesToShow: 1,
		slidesToScroll: 1,
		arrows: true,
		fade: true,
		asNavFor: '.property-slider-nav',
		centerMode: true,
		slide: ".item",
		 adaptiveHeight: true
	});

	$('.property-slider-nav').slick({
		slidesToShow: 6,
		slidesToScroll: 1,
		asNavFor: '.property-slider',
		dots: false,
		arrows: false,
		centerMode: false,
		focusOnSelect: true,
		responsive: [
			{
			  breakpoint: 993,
			  settings: {
			   		slidesToShow: 4,
			  }
			},
			{
			  breakpoint: 767,
			  settings: {
			   		slidesToShow: 3,
			  }
			}
		]
	});


	 $('.fullwidth-property-slider').slick({
		centerMode: true,
		centerPadding: '20%',
		slidesToShow: 1,
		responsive: [
			{
			  breakpoint: 1367,
			  settings: {
			    centerPadding: '15%'
			  }
			},
			{
			  breakpoint: 993,
			  settings: {
			    centerPadding: '0'
			  }
			}
		]
	});


	 $('.fullwidth-home-slider').slick({
		centerMode: true,
		centerPadding: '0',
		slidesToShow: 1,
		autoplay: true,
		autoplaySpeed: 1500,
		responsive: [
			{
			  breakpoint: 1367,
			  settings: {
			    centerPadding: '0'
			  }
			},
			{
			  breakpoint: 993,
			  settings: {
			    centerPadding: '0'
			  }
			}
		]
	});



    /*----------------------------------------------------*/
    /*  Magnific Popup
    /*----------------------------------------------------*/
	$('body').magnificPopup({
		 type: 'image',
		 delegate: 'a.mfp-gallery',

		 fixedContentPos: true,
		 fixedBgPos: true,

		 overflowY: 'auto',

		 closeBtnInside: false,
		 preloader: true,

		 removalDelay: 0,
		 mainClass: 'mfp-fade',

		 gallery:{enabled:true}
	});


	$('.popup-with-zoom-anim').magnificPopup({
		 type: 'inline',

		 fixedContentPos: false,
		 fixedBgPos: true,

		 overflowY: 'auto',

		 closeBtnInside: true,
		 preloader: false,

		 midClick: true,
		 removalDelay: 300,
		 mainClass: 'my-mfp-zoom-in'
	});


	$('.mfp-image').magnificPopup({
		 type: 'image',
		 closeOnContentClick: true,
		 mainClass: 'mfp-fade',
		 image: {
			  verticalFit: true
		 }
	});

	$('.popup-youtube, .popup-vimeo, .popup-gmaps').magnificPopup({
		 disableOn: 700,
		 type: 'iframe',
		 mainClass: 'mfp-fade',
		 removalDelay: 160,
		 preloader: false,

		 fixedContentPos: false
	});


    /*----------------------------------------------------*/
    /*  Sticky Footer (footer-reveal.js)
    /*----------------------------------------------------*/

	// disable if IE
	if(navigator.userAgent.match(/Trident\/7\./)) { // if IE
	    $('#footer').removeClass("sticky-footer");
	}

	$('#footer.sticky-footer').footerReveal();


    /*----------------------------------------------------*/
    /*  Image Box
    /*----------------------------------------------------*/
	$('.img-box').each(function(){

		// add a photo container
		$(this).append('<div class="img-box-background"></div>');

		// set up a background image for each tile based on data-background-image attribute
		$(this).children('.img-box-background').css({'background-image': 'url('+ $(this).attr('data-background-image') +')'});

		// background animation on mousemove
		// $(this).on('mousemove', function(e){
		//   $(this).children('.img-box-background').css({'transform-origin': ((e.pageX - $(this).offset().left) / $(this).width()) * 100 + '% ' + ((e.pageY - $(this).offset().top) / $(this).height()) * 100 +'%'});
		// })
	});


    /*----------------------------------------------------*/
    /*  Listing Layout Switcher
    /*----------------------------------------------------*/
	function gridLayoutSwitcher() {

		var listingsContainer = $('.listings-container');

		// switcher buttons / anchors
		if ( $(listingsContainer).is(".list-layout") ) {
			owlReload();
			$('.layout-switcher a.grid, .layout-switcher a.grid-three').removeClass("active");
			$('.layout-switcher a.list').addClass("active");
		}

		if ( $(listingsContainer).is(".grid-layout") ) {
			owlReload();
			$('.layout-switcher a.grid').addClass("active");
			$('.layout-switcher a.grid-three, .layout-switcher a.list').removeClass("active");
			gridClear(2);
		}

		if ( $(listingsContainer).is(".grid-layout-three") ) {
			owlReload();
			$('.layout-switcher a.grid, .layout-switcher a.list').removeClass("active");
			$('.layout-switcher a.grid-three').addClass("active");
			gridClear(3);
		}


		// grid cleaning
		function gridClear(gridColumns) {
			$(listingsContainer).find(".clearfix").remove();
			$(".listings-container > .listing-item:nth-child("+gridColumns+"n)").after("<div class='clearfix'></div>");
		}


		// objects that need to resized
		var resizeObjects =  $('.listings-container .listing-img-container img, .listings-container .listing-img-container');

		// if list layout is active
		function listLayout() {
			if ( $('.layout-switcher a').is(".list.active") ) {

				$(listingsContainer).each(function(){
					$(this).removeClass("grid-layout grid-layout-three");
					$(this).addClass("list-layout");
				});

				$('.listing-item').each(function(){
					var listingContent = $(this).find('.listing-content').height();
					$(this).find(resizeObjects).css('height', ''+listingContent+'');
				});
			}
		} listLayout();

		$(window).on('load resize', function() {
			listLayout();
		});


		// if grid layout is active
		$('.layout-switcher a.grid').on('click', function(e) { gridClear(2); });

		function gridLayout() {
			if ( $('.layout-switcher a').is(".grid.active") ) {

				$(listingsContainer).each(function(){
					$(this).removeClass("list-layout grid-layout-three");
					$(this).addClass("grid-layout");
				});

				$('.listing-item').each(function(){
					$(this).find(resizeObjects).css('height', 'auto');
				});

			}
		} gridLayout();


		// if grid layout is active
		$('.layout-switcher a.grid-three').on('click', function(e) { gridClear(3); });

		function gridThreeLayout() {
			if ( $('.layout-switcher a').is(".grid-three.active") ) {

				$(listingsContainer).each(function(){
					$(this).removeClass("list-layout grid-layout");
					$(this).addClass("grid-layout-three");
				});

				$('.listing-item').each(function(){
					$(this).find(resizeObjects).css('height', 'auto');
				});

			}
		} gridThreeLayout();


		// Mobile fixes
		$(window).on('resize', function() {
			$(resizeObjects).css('height', '0');
			listLayout();
			gridLayout();
			gridThreeLayout();
		});

		$(window).on('load resize', function() {
			var winWidth = $(window).width();

			if(winWidth < 992) {
				owlReload();

				// reset to two columns grid
				gridClear(2);
			}

			if(winWidth > 992) {
				if ( $(listingsContainer).is(".grid-layout-three") ) {
					gridClear(3);
				}
				if ( $(listingsContainer).is(".grid-layout") ) {
					gridClear(2);
				}
			}

			if(winWidth < 768) {
				if ( $(listingsContainer).is(".list-layout") ) {
					$('.listing-item').each(function(){
						$(this).find(resizeObjects).css('height', 'auto');
					});
				}
			}

			if(winWidth < 1366) {
				if ( $(".fs-listings").is(".list-layout") ) {
					$('.listing-item').each(function(){
						$(this).find(resizeObjects).css('height', 'auto');
					});
				}
			}
		});


		// owlCarousel reload
		function owlReload() {
			$('.listing-carousel').each(function(){
				$(this).data('owlCarousel').reload();
			});
		}


	    // switcher buttons
		$('.layout-switcher a').on('click', function(e) {
		    e.preventDefault();

		    var switcherButton = $(this);
		    switcherButton.addClass("active").siblings().removeClass('active');

		    // reset images height
			$(resizeObjects).css('height', '0');

		    // carousel reload
			owlReload();

		    // if grid layout is active
			gridLayout();

		    // if three columns grid layout is active
			gridThreeLayout();

			// if list layout is active
			listLayout();

		});

	} gridLayoutSwitcher();



	/*----------------------------------------------------*/
	/*  Range Sliders
	/*----------------------------------------------------*/

	// Area Range
	$("#area-range").each(function() {

		var dataMin = $(this).attr('data-min');
		var dataMax = $(this).attr('data-max');
		var dataUnit = $(this).attr('data-unit');

		$(this).append( "<input type='text' class='first-slider-value'disabled/><input type='text' class='second-slider-value' disabled/>" );

		$(this).slider({

		  range: true,
		  min: dataMin,
		  max: dataMax,
		  step: 10,
		  values: [ dataMin, dataMax ],

		  slide: function( event, ui ) {
			 event = event;
			 $(this).children( ".first-slider-value" ).val( ui.values[ 0 ]  + " " + dataUnit );
			 $(this).children( ".second-slider-value" ).val( ui.values[ 1 ] + " " +  dataUnit );
		  }
		});
		 $(this).children( ".first-slider-value" ).val( $( this ).slider( "values", 0 ) + " " + dataUnit );
		 $(this).children( ".second-slider-value" ).val( $( this ).slider( "values", 1 ) + " " + dataUnit );

	});


	// Price Range
	$("#price-range").each(function() {

		var dataMin = $(this).attr('data-min');
		var dataMax = $(this).attr('data-max');
		var dataUnit = $(this).attr('data-unit');

		$(this).append( "<input type='text' class='first-slider-value' disabled/><input type='text' class='second-slider-value' disabled/>" );


		$(this).slider({

		  range: true,
		  min: dataMin,
		  max: dataMax,
		  values: [ dataMin, dataMax ],

		  slide: function( event, ui ) {
			 event = event;
			 $(this).children( ".first-slider-value" ).val( ui.values[ 0 ].toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + dataUnit);
			 $(this).children( ".second-slider-value" ).val( ui.values[ 1 ].toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + dataUnit);
		  }
		});
		 $(this).children( ".first-slider-value" ).val( $( this ).slider( "values", 0 ).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + dataUnit);
		 $(this).children( ".second-slider-value" ).val(   $( this ).slider( "values", 1 ).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + dataUnit);


	});



	/*----------------------------------------------------*/
	/*  Masonry
	/*----------------------------------------------------*/

	// Agent Profile Alignment
    $(window).on('load resize', function() {

		$('.agents-grid-container').masonry({
			itemSelector: '.grid-item', // use a separate class for itemSelector, other than .col-
			columnWidth: '.grid-item',
			percentPosition: true
		});

		var agentAvatarHeight = $(".agent-avatar img").height();
		var agentContentHeight = $(".agent-content").innerHeight();

		if ( agentAvatarHeight < agentContentHeight ) {
			$('.agent-page').addClass('long-content');
		} else  {
			$('.agent-page').removeClass('long-content');
		}
    });



    /*----------------------------------------------------*/
    /*  Submit Property
    /*----------------------------------------------------*/

    // Tooltip
	$(".tip").each(function() {
		var tipContent = $(this).attr('data-tip-content');
		$(this).append('<div class="tip-content">'+ tipContent + '</div>');
	});



	/*----------------------------------------------------*/
	/*  Tabs
	/*----------------------------------------------------*/

	var $tabsNav    = $('.tabs-nav'),
	$tabsNavLis = $tabsNav.children('li');

	$tabsNav.each(function() {
		 var $this = $(this);

		 $this.next().children('.tab-content').stop(true,true).hide()
		 .first().show();

		 $this.children('li').first().addClass('active').stop(true,true).show();
	});

	$tabsNavLis.on('click', function(e) {
		 var $this = $(this);

		 $this.siblings().removeClass('active').end()
		 .addClass('active');

		 $this.parent().next().children('.tab-content').stop(true,true).hide()
		 .siblings( $this.find('a').attr('href') ).fadeIn();

		 e.preventDefault();
	});
	var hash = window.location.hash;
	var anchor = $('.tabs-nav a[href="' + hash + '"]');
	if (anchor.length === 0) {
		 $(".tabs-nav li:first").addClass("active").show(); //Activate first tab
		 $(".tab-content:first").show(); //Show first tab content
	} else {
		 console.log(anchor);
		 anchor.parent('li').click();
	}


	/*----------------------------------------------------*/
	/*  Accordions
	/*----------------------------------------------------*/
	var $accor = $('.accordion');

	 $accor.each(function() {
		 $(this).toggleClass('ui-accordion ui-widget ui-helper-reset');
		 $(this).find('h3').addClass('ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons ui-corner-all');
		 $(this).find('div').addClass('ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom');
		 $(this).find("div").hide();
	});

	var $trigger = $accor.find('h3');

	$trigger.on('click', function(e) {
		 var location = $(this).parent();

		 if( $(this).next().is(':hidden') ) {
			  var $triggerloc = $('h3',location);
			  $triggerloc.removeClass('ui-accordion-header-active ui-state-active ui-corner-top').next().slideUp(300);
			  $triggerloc.find('span').removeClass('ui-accordion-icon-active');
			  $(this).find('span').addClass('ui-accordion-icon-active');
			  $(this).addClass('ui-accordion-header-active ui-state-active ui-corner-top').next().slideDown(300);
		 }
		 else if( $(this).is(':visible') ) {
			  var $triggerloc = $('h3',location);
			  $triggerloc.removeClass('ui-accordion-header-active ui-state-active ui-corner-top').next().slideUp(300);
			  $triggerloc.find('span').removeClass('ui-accordion-icon-active');
		 }
		  e.preventDefault();
	});



	/*----------------------------------------------------*/
	/*	Toggle
	/*----------------------------------------------------*/

	$(".toggle-container").hide();

	$('.trigger, .trigger.opened').on('click', function(a){
		$(this).toggleClass('active');
		a.preventDefault();
	});

	$(".trigger").on('click', function(){
		$(this).next(".toggle-container").slideToggle(300);
	});

	$(".trigger.opened").addClass("active").next(".toggle-container").show();


	/*----------------------------------------------------*/
	/*  Notifications
	/*----------------------------------------------------*/

	$("a.close").removeAttr("href").on('click', function(){
		$(this).parent().fadeOut(200);
	});


	/*----------------------------------------------------*/
	/*  Contact Form
	/*----------------------------------------------------*/

    var shake = "No";

    $('#message').hide();

    // Add validation parts
    $('#contact input[type=text], #contact input[type=number], #contact input[type=email], #contact input[type=url], #contact input[type=tel], #contact select, #contact textarea').each(function(){

    });

	 // Validate as you type
	   $('#name, #comments, #subject').focusout(function() {
	       if (!$(this).val()) {
	           $(this).addClass('error').parent().find('mark').removeClass('valid').addClass('error');
	       }
	       else {
	           $(this).removeClass('error').parent().find('mark').removeClass('error').addClass('valid');
	       }
	         $('#submit')
	           .prop('disabled',false)
	           .removeClass('disabled');
	   });
	   $('#email').focusout(function() {
	       if (!$(this).val() || !isEmail($(this).val())) {
	           $(this).addClass('error').parent().find('mark').removeClass('valid').addClass('error');
	       } else {
	           $(this).removeClass('error').parent().find('mark').removeClass('error').addClass('valid');
	       }
	   });

	   $('#email').focusin(function() {
	        $('#submit')
	           .prop('disabled',false)
	           .removeClass('disabled');
	   });

    $('#submit').click(function() {
        $("#contact-message").slideUp(200,function() {
            $('#contact-message').hide();

            // Kick in Validation
            $('#name, #subject, #phone, #comments, #website, #email').triggerHandler("focusout");

            if ($('#contact mark.error').size()>0) {
                if(shake == "Yes") {
                    $('#contact').effect('shake', { times:2 }, 75, function(){
                        $('#contact input.error:first, #contact textarea.error:first').focus();
                    });
                } else $('#contact input.error:first, #contact textarea.error:first').focus();

                return false;
            }

        });
    });

    $('#contactform').submit(function(){

        if ($('#contact mark.error').size()>0) {
            if(shake == "Yes") {
            $('#contact').effect('shake', { times:2 }, 75);
            }
            return false;
        }

        var action = $(this).attr('action');

        $('#contact #submit').after('<img src="images/loader.gif" class="loader" />')

        $('#submit')
            .prop('disabled',true)
            .addClass('disabled');

        $.post(action, $('#contactform').serialize(),
            function(data){
                $('#contact-message').html( data );
                $('#contact-message').slideDown();
                $('#contactform img.loader').fadeOut('slow',function(){$(this).remove()});
                // $('#contactform #submit').removeAttr('disabled');
                if(data.match('success') != null) $('#contactform').slideUp('slow');

            }
        );

        return false;

    });

    function isEmail(emailAddress) {

        var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);

        return pattern.test(emailAddress);
    }

    function isNumeric(input) {
        return (input - 0) == input && input.length > 0;
    }


	/*----------------------------------------------------*/
	/* Panel Dropdown
	/*----------------------------------------------------*/
    function close_panel_dropdown() {
		$('.panel-dropdown').removeClass("active");
		$('.fs-inner-container.content').removeClass("faded-out");
    }

    $('.panel-dropdown a').on('click', function(e) {

		if ( $(this).parent().is(".active") ) {
            close_panel_dropdown();
        } else {
            close_panel_dropdown();
            $(this).parent().addClass('active');
			$('.fs-inner-container.content').addClass("faded-out");
        }

        e.preventDefault();
    });

    // Apply / Close buttons
    $('.panel-buttons button').on('click', function(e) {
	    $('.panel-dropdown').removeClass('active');
		$('.fs-inner-container.content').removeClass("faded-out");
    });

    // Closes dropdown on click outside the conatainer
	var mouse_is_inside = false;

	$('.panel-dropdown').hover(function(){
	    mouse_is_inside=true;
	}, function(){
	    mouse_is_inside=false;
	});

	$("body").mouseup(function(){
	    if(! mouse_is_inside) close_panel_dropdown();
	});


	// Adjusting Panel Dropdown Width
	$(window).on('load resize', function() {
	   var panelTrigger = $('.booking-widget .panel-dropdown a');
	   $('.booking-widget .panel-dropdown .panel-dropdown-content').css({
	      'width' : panelTrigger.outerWidth()
	   });
	});

	/*----------------------------------------------------*/
	/*  Quantity Buttons with Total Value Counter
	/*----------------------------------------------------*/
	if ($(".qtyButtons").length > 0){

		function qtySum(){
		    var arr = document.getElementsByName('qtyInput');
		    var tot=0;
		    for(var i=0;i<arr.length;i++){
		        if(parseInt(arr[i].value))
		            tot += parseInt(arr[i].value);
		    }

		    var cardQty = document.querySelector(".qtyTotal");
		    cardQty.innerHTML = tot;
		} 
		qtySum();

		$(function() {

		   $(".qtyButtons input").after('<div class="qtyInc"></div>');
		   $(".qtyButtons input").before('<div class="qtyDec"></div>');

		   $(".qtyDec, .qtyInc").on("click", function() {

		      var $button = $(this);
		      var oldValue = $button.parent().find("input").val();

		      if ($button.hasClass('qtyInc')) {
		         var newVal = parseFloat(oldValue) + 1;
		      } else {
		         // don't allow decrementing below zero
		         if (oldValue > 0) {
		            var newVal = parseFloat(oldValue) - 1;
		         } else {
		            newVal = 0;
		         }
		      }

		      $button.parent().find("input").val(newVal);
		      qtySum();
		      $(".qtyTotal").addClass("rotate-x");

		   });

		   // Total Value Counter Animation
		   function removeAnimation() { $(".qtyTotal").removeClass("rotate-x"); }

		   const counter = document.querySelector(".qtyTotal");
		   counter.addEventListener("animationend", removeAnimation);

		});

	}

// ------------------ End Document ------------------ //
});

})(this.jQuery);



(function($) {

  $.fn.footerReveal = function(options) {

	$('#footer.sticky-footer').before('<div class="footer-shadow"></div>');

    var $this = $(this),
        $prev = $this.prev(),
        $win = $(window),

        defaults = $.extend ({
          shadow : true,
          shadowOpacity: 0.12,
          zIndex : -10
        }, options ),

        settings = $.extend(true, {}, defaults, options);

		$this.before('<div class="footer-reveal-offset"></div>');

    if ($this.outerHeight() <= $win.outerHeight()) {
      $this.css({
        'z-index' : defaults.zIndex,
        position : 'fixed',
        bottom : 0
      });

      $win.on('load resize', function() {
        $this.css({
          'width' : $prev.outerWidth()
        });
        $prev.css({
          'margin-bottom' : $this.outerHeight()
        });
      });
    }

    return this;

  };

}) (this.jQuery);
