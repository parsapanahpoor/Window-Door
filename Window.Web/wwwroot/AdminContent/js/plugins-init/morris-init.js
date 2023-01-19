(function($) {
    "use strict"

	var dzMorris = function(){
		
		var screenWidth = $(window).width();
		
		var setChartWidth = function(){
			if(screenWidth <= 768)
			{
				var chartBlockWidth = 0;
				chartBlockWidth = (screenWidth < 300 )?screenWidth:300;
				jQuery('.morris_chart_height').css('min-width',chartBlockWidth - 31);
			}
		}
		
		var donutChart = function(){
			Morris.Donut({
				element: 'morris_donught',
				data: [{
					label: "\xa0 \xa0 دانلود \xa0 \xa0",
					value: 12,

				}, {
					label: "\xa0 \xa0 فروش داخل فروشگاه \xa0 \xa0",
					value: 30
				}, {
					label: "\xa0 \xa0 فروش پست الکترونیکی \xa0 \xa0",
					value: 20
				}],
				resize: true,
				redraw: true,
				colors: ['#3a7afe', 'rgb(255, 159, 0)', '#10ca93'],
				//responsive:true,
				
			});
		}
		
		var lineChart = function(){
			//line chart
			let line = new Morris.Line({
				element: 'morris_line',
				resize: true,
				data: [{
						y: '2011 تعداد 1',
						item1: 2666
					},
					{
						y: '2011 تعداد 2',
						item1: 2778
					},
					{
						y: '2011 تعداد 3',
						item1: 4912
					},
					{
						y: '2011 تعداد 4',
						item1: 3767
					},
					{
						y: '2012 تعداد 1',
						item1: 6810
					},
					{
						y: '2012 تعداد 2',
						item1: 5670
					},
					{
						y: '2012 تعداد 3',
						item1: 4820
					},
					{
						y: '2012 تعداد 4',
						item1: 15073
					},
					{
						y: '2013 تعداد 1',
						item1: 10687
					},
					{
						y: '2013 تعداد 2',
						item1: 8432
					}
				],
				xkey: 'y',
				ykeys: ['item1'],
				labels: ['آیتم 1'],
				gridLineColor: 'transparent',
				lineColors: ['rgb(58, 122, 254)'], //here
				lineWidth: 1,
				hideHover: 'auto',
				pointSize: 0,
				axes: false
			});	
		}
		
		var lineChart2 = function(){
			//Area chart
			Morris.Area({
				element: 'line_chart_2',
				data: [{
						period: '2001',
						smartphone: 0,
						windows: 0,
						mac: 0
					}, {
						period: '2002',
						smartphone: 90,
						windows: 60,
						mac: 25
					}, {
						period: '2003',
						smartphone: 40,
						windows: 80,
						mac: 35
					}, {
						period: '2004',
						smartphone: 30,
						windows: 47,
						mac: 17
					}, {
						period: '2005',
						smartphone: 150,
						windows: 40,
						mac: 120
					}, {
						period: '2006',
						smartphone: 25,
						windows: 80,
						mac: 40
					}, {
						period: '2007',
						smartphone: 10,
						windows: 10,
						mac: 10
					}


				],
				xkey: 'period',
				ykeys: ['smartphone', 'windows', 'mac'],
				labels: ['موبایل', 'ویندوز', 'مک'],
				pointSize: 3,
				fillOpacity: 0,
				pointStrokeColors: ['#f25767', '#10ca93', '#3a7afe'],
				behaveLikeLine: true,
				gridLineColor: 'transparent',
				lineWidth: 3,
				hideHover: 'auto',
				lineColors: ['rgb(242, 87, 103)', 'rgb(0, 171, 197)', '#3a7afe'],
				resize: true

			});
		}
		
		var barChart = function(){
			//bar chart
			Morris.Bar({
				element: 'morris_bar',
				data: [{
					y: '2006',
					a: 100,
					b: 90,
					c: 60
				}, {
					y: '2007',
					a: 75,
					b: 65,
					c: 40
				}, {
					y: '2008',
					a: 50,
					b: 40,
					c: 30
				}, {
					y: '2009',
					a: 75,
					b: 65,
					c: 40
				}, {
					y: '2010',
					a: 50,
					b: 40,
					c: 30
				}, {
					y: '2011',
					a: 75,
					b: 65,
					c: 40
				}, {
					y: '2012',
					a: 100,
					b: 90,
					c: 40
				}],
				xkey: 'y',
				ykeys: ['a', 'b', 'c'],
				labels: ['A', 'B', 'C'],
				barColors: ['#3a7afe', '#10ca93', '#ff9f00'],
				hideHover: 'auto',
				gridLineColor: 'transparent',
				resize: true,
				barSizeRatio: 0.25,
			});	
		}
		
		var barStalkChart = function(){
			//bar chart
			Morris.Bar({
				element: 'morris_bar_stalked',
				data: [{
					y: 'S',
					a: 66, 
					b: 34
				}, {
					y: 'M',
					a: 75, 
					b: 25
				}, {
					y: 'T',
					a: 50, 
					b: 50
				}, {
					y: 'W',
					a: 75, 
					b: 25
				}, {
					y: 'T',
					a: 50, 
					b: 50
				}, {
					y: 'F',
					a: 16, 
					b: 84
				}, {
					y: 'S',
					a: 70, 
					b: 30
				}, {
					y: 'S',
					a: 30, 
					b: 70
				}, {
					y: 'M',
					a: 40, 
					b: 60
				}, {
					y: 'T',
					a: 29, 
					b: 71
				}, {
					y: 'W',
					a: 44, 
					b: 56
				}, {
					y: 'T',
					a: 30, 
					b: 70
				}, {
					y: 'F',
					a: 60, 
					b: 40
				}, {
					y: 'G',
					a: 40, 
					b: 60
				}, {
					y: 'S',
					a: 46, 
					b: 54
				}],
				xkey: 'y',
				ykeys: ['a', 'b'],
				labels: ['A', 'B'],
				barColors: ['#3a7afe', "#F1F3F7"],
				hideHover: 'auto',
				gridLineColor: 'transparent',
				resize: true,
				barSizeRatio: 0.25,
				stacked: true, 
				behaveLikeLine: true,
				//redraw: true
				
				// barRadius: [6, 6, 0, 0]
			});
		
		}
		
		var areaChart = function(){
			//area chart
			Morris.Area({
				element: 'morris_area',
				data: [{
						period: '2001',
						smartphone: 0,
						windows: 0,
						mac: 0
					}, {
						period: '2002',
						smartphone: 90,
						windows: 60,
						mac: 25
					}, {
						period: '2003',
						smartphone: 40,
						windows: 80,
						mac: 35
					}, {
						period: '2004',
						smartphone: 30,
						windows: 47,
						mac: 17
					}, {
						period: '2005',
						smartphone: 150,
						windows: 40,
						mac: 120
					}, {
						period: '2006',
						smartphone: 25,
						windows: 80,
						mac: 40
					}, {
						period: '2007',
						smartphone: 10,
						windows: 10,
						mac: 10
					}


				],
				lineColors: ['#3a7afe', 'rgb(16, 202, 147)', 'rgb(255, 159, 0)'],
				xkey: 'period',
				ykeys: ['smartphone', 'windows', 'mac'],
				labels: ['موبایل', 'ویندوز', 'مک'],
				pointSize: 0,
				lineWidth: 0,
				resize: true,
				fillOpacity: 0.95,
				behaveLikeLine: true,
				gridLineColor: 'transparent',
				hideHover: 'auto'

			});
		}
		
		var areaChart2 = function(){
			//area chart
			Morris.Area({
				element: 'morris_area_2',
				data: [{
						period: '2010',
						SiteA: 0,
						SiteB: 0,

					}, {
						period: '2011',
						SiteA: 130,
						SiteB: 100,

					}, {
						period: '2012',
						SiteA: 80,
						SiteB: 60,

					}, {
						period: '2013',
						SiteA: 70,
						SiteB: 200,

					}, {
						period: '2014',
						SiteA: 180,
						SiteB: 150,

					}, {
						period: '2015',
						SiteA: 105,
						SiteB: 90,

					},
					{
						period: '2016',
						SiteA: 250,
						SiteB: 150,

					}
				],
				xkey: 'period',
				ykeys: ['SiteA', 'SiteB'],
				labels: ['سایت 1', 'سایت 2'],
				pointSize: 0,
				fillOpacity: 0.6,
				pointStrokeColors: ['#b4becb', '#00A2FF'], //here
				behaveLikeLine: true,
				gridLineColor: 'transparent',
				lineWidth: 0,
				smooth: false,
				hideHover: 'auto',
				lineColors: ['rgb(0, 171, 197)', 'rgb(0, 0, 128)'],
				resize: true

			});	
		}
		
		
		/* Function ============ */
		return {
			init:function(){
			},
			
			
			load:function(){
				setChartWidth();
				donutChart();
				lineChart();
				lineChart2();
				barChart();
				barStalkChart();
				areaChart();
				areaChart2();
			},
			
			resize:function(){
				screenWidth = $(window).width();
				setChartWidth();
				donutChart();
				lineChart();
				lineChart2();
				barChart();
				barStalkChart();
				areaChart();
				areaChart2();
			}
		}
		
	}();

	jQuery(document).ready(function(){
	});
		
	jQuery(window).on('load',function(){
		dzMorris.load();
	});

	jQuery(window).on('resize',function(){
		dzMorris.resize();
		
	});
   
})(jQuery);