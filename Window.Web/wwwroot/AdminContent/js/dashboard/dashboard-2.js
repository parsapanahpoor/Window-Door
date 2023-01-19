(function($) {
    "use strict";
	
	//#chart_widget_4
	if(jQuery('#chart_widget_4').length > 0 ){
    const chart_widget_4 = document.getElementById("chart_widget_4").getContext('2d');
    
    // chart_widget_4.height = 100;

    let barChartData = {
        defaultFontFamily: 'inherit',
        labels: ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'],
        datasets: [{
            label: 'هزینه',
            backgroundColor: '#fff',
            hoverBackgroundColor: '#eee', 
            data: [
                '20',
                '14',
                '18',
                '25',
                '27',
                '22',
                '12', 
                '24', 
                '20', 
                '14', 
                '18', 
                '16'
            ]
        }, {
            label: 'درآمد',
            backgroundColor: 'rgba(255,255,255,0.1)',
            hoverBackgroundColor: 'rgba(255,255,255,0.15)', 
            data: [
                '12',
                '18',
                '14',
                '7',
                '5',
                '10',
                '20', 
                '8', 
                '12', 
                '18', 
                '14', 
                '16'
            ]
        }]

    };

    new Chart(chart_widget_4, {
        type: 'bar',
        data: barChartData,
        options: {
            legend: {
                display: false
            }, 
            title: {
                display: false
            },
            tooltips: {
                mode: 'index',
                intersect: false
            },
            responsive: true,
            maintainAspectRatio: false, 
            scales: {
                xAxes: [{
                    display: false, 
                    stacked: true,
                    barPercentage: 0.2, 
                    ticks: {
                        display: false
                    }, 
                    gridLines: {
                        display: false, 
                        drawBorder: false
                    }
                }],
                yAxes: [{
                    display: false, 
                    stacked: true, 
                    gridLines: {
                        display: false, 
                        drawBorder: false
                    }, 
                    ticks: {
                        display: false
                    }
                }]
            }
        }
    });
	}

    /*======== 16. ANALYTICS - ACTIVITY CHART ========*/
    var activity = document.getElementById("activity");
    if (activity !== null) {
        var activityData = [{
                first: [35, 18, 15, 35, 40, 20, 30, 25, 22, 20, 45, 35]
            },
            {
                first: [50, 35, 10, 45, 40, 50, 60, 80, 10, 50, 34, 35]
            },
            {
                first: [20, 35, 60, 45, 40, 70, 30, 80, 65, 70, 60, 20]
            },
            {
                first: [25, 88, 25, 50, 70, 70, 60, 70, 50, 60, 50, 70]
            }
        ];
        activity.height = 300;
		
        var config = {
            type: "bar",
            data: {
                labels: [
                    "01",
                    "02",
                    "03",
                    "04",
                    "05",
                    "06",
                    "07",
                    "08",
                    "09",
                    "10",
                    "11",
                    "12"
                ],
                datasets: [
					{
						label: "اولین مجموعه داده من",
						data:  [35, 18, 15, 35, 40, 20, 30, 25, 22, 20, 45, 35],
						borderColor: 'rgba(26, 51, 213, 1)',
						borderWidth: "0",
						backgroundColor: 'rgba(58, 122, 254, 1)'
						
					}
				]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
				
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [{
                        gridLines: {
                            color: "rgba(89, 59, 219,0.1)",
                            drawBorder: true
                        },
                        ticks: {
                            fontColor: "#999",
                        },
                    }],
                    xAxes: [{
                        gridLines: {
                            display: false,
                            zeroLineColor: "transparent"
                        },
                        ticks: {
                            stepSize: 5,
                            fontColor: "#999",
                            fontFamily: "Nunito, sans-serif"
                        }
                    }]
                },
                tooltips: {
                    mode: "index",
                    intersect: false,
                    titleFontColor: "#888",
                    bodyFontColor: "#555",
                    titleFontSize: 12,
                    bodyFontSize: 15,
                    backgroundColor: "rgba(256,256,256,0.95)",
                    displayColors: true,
                    xPadding: 10,
                    yPadding: 7,
                    borderColor: "rgba(220, 220, 220, 0.9)",
                    borderWidth: 2,
                    caretSize: 6,
                    caretPadding: 10
                }
            }
        };

        var ctx = document.getElementById("activity").getContext("2d");
        var myLine = new Chart(ctx, config);

        var items = document.querySelectorAll("#user-activity .nav-tabs .nav-item");
        items.forEach(function(item, index) {
            item.addEventListener("click", function() {
                config.data.datasets[0].data = activityData[index].first;
                myLine.update();
            });
        });
    }

    
    var data = {
        labels: ["0", "1", "2", "3", "4", "5", "6", "0", "1", "2", "3", "4", "5", "6"],
        datasets: [{
            label: "اولین مجموعه داده من",
            backgroundColor: "rgba(0,131,143,1)",
            strokeColor: "rgba(0,131,143,1)",
            pointColor: "rgba(0,0,0,0)",
            pointStrokeColor: "rgba(0,0,0,0)",
            pointHighlightFill: "rgba(0,131,143,1)",
            pointHighlightStroke: "rgba(0,131,143,1)",
            data:  [35, 18, 15, 35, 40, 20, 30, 25, 22, 20, 45, 35]
        }]
    };

    if(jQuery('#activeUser').length > 0 ){
		var data = {
			labels: ["0", "1", "2", "3", "4", "5", "6", "0", "1", "2", "3", "4", "5", "6"],
			datasets: [{
				label: "اولین مجموعه داده من",
				backgroundColor: "rgba(58,223,174,1)",
				strokeColor: "rgba(58,223,174,1)",
				pointColor: "rgba(0,0,0,0)",
				pointStrokeColor: "rgba(58,223,174,1)",
				pointHighlightFill: "rgba(58,223,174,1)",
				pointHighlightStroke: "rgba(58,223,174,1)",
				data: [65, 59, 80, 81, 56, 55, 40, 65, 59, 80, 81, 56, 55, 40]
			}]
		};

		var ctx = document.getElementById("activeUser").getContext("2d");
		var chart = new Chart(ctx, {
			type: "bar",
			data: data,
			options: {
				responsive: !0,
				maintainAspectRatio: false,
				legend: {
					display: !1
				},
				tooltips: {
					enabled: false
				},
				scales: {
					xAxes: [{
						display: !1,
						gridLines: {
							display: !1
						},
						barPercentage: 1,
						categoryPercentage: 0.5
					}],
					yAxes: [{
						display: !1,
						ticks: {
							padding: 10,
							stepSize: 20,
							max: 100,
							min: 0
						},
						gridLines: {
							display: !0,
							drawBorder: !1,
							lineWidth: 1,
							zeroLineColor: "#48f3c0"
						}
					}]
				}
			}
		});
		
		setInterval(function() {
			chart.config.data.datasets[0].data.push(
				Math.floor(10 + Math.random() * 80)
			);
			chart.config.data.datasets[0].data.shift();
			chart.update();
		}, 2000);
		
	}

   if(jQuery('#areaChart_2').length > 0 ){
		const areaChart_2 = document.getElementById("areaChart_2").getContext('2d');
		//generate gradient
		const areaChart_2gradientStroke = areaChart_2.createLinearGradient(0, 1, 0, 500);
		areaChart_2gradientStroke.addColorStop(0, "rgba(16, 202, 147, 0.1)");
		areaChart_2gradientStroke.addColorStop(1, "rgba(16, 202, 147, 0)");
		
		areaChart_2.height = 50;

		new Chart(areaChart_2, {
			type: 'line',
			data: {
				defaultFontFamily: 'inherit',
				labels: ["فرودین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهرl"],
				datasets: [
					{
						label: "اولین مجموعه داده من",
						data: [60, 80, 60, 40, 70, 55, 95],
						borderColor: "#10ca93",
						borderWidth: "3",
						backgroundColor: areaChart_2gradientStroke
					}
				]
			},
			options: {
				
				tooltips: {
					enabled: false,
				},
				elements: {
						point:{
							radius: 0
						}
				},
				legend: false, 
				scales: {
					yAxes: [{
						
						ticks: {
							beginAtZero: true, 
							max: 100, 
							min: 0, 
							stepSize: 10, 
							padding: 2
						},
						display:false,
					}],
					xAxes: [{ 
						ticks: {
							padding: 2
						},
						display:false,
					}]
				}
			}
		});
	}    
	
let draw = Chart.controllers.line.__super__.draw; //draw shadow	
	
//dual line chart
if(jQuery('#lineChart_3Kk').length > 0 ){
	
	if($(window).width() < 991)
	{
	 jQuery('#lineChart_3Kk').removeAttr('height');
	}
	
    const lineChart_3Kk = document.getElementById("lineChart_3Kk").getContext('2d');
    //generate gradient
    const lineChart_3gradientStroke1 = lineChart_3Kk.createLinearGradient(500, 0, 100, 0);
    lineChart_3gradientStroke1.addColorStop(0, "rgba(26, 51, 213, 1)");
    lineChart_3gradientStroke1.addColorStop(1, "rgba(26, 51, 213, 0.5)");

    const lineChart_3gradientStroke2 = lineChart_3Kk.createLinearGradient(500, 0, 100, 0);
    lineChart_3gradientStroke2.addColorStop(0, "rgba(56, 164, 248, 1)");
    lineChart_3gradientStroke2.addColorStop(1, "#ce1d76");

    Chart.controllers.line = Chart.controllers.line.extend({
        draw: function () {
            draw.apply(this, arguments);
            let nk = this.chart.chart.ctx;
            let _stroke = nk.stroke;
            nk.stroke = function () {
                nk.save();
                nk.shadowColor = 'rgba(0, 0, 0, 0)';
                nk.shadowBlur = 10;
                nk.shadowOffsetX = 0;
                nk.shadowOffsetY = 10;
                _stroke.apply(this, arguments)
                nk.restore();
            }
        }
    });
        
    lineChart_3Kk.height = 250;

    new Chart(lineChart_3Kk, {
        type: 'line',
        data: {
            defaultFontFamily: 'inherit',
            labels: ["فرودین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهرl"],
            datasets: [
                {
                    label: "اولین مجموعه داده من",
                    data: [90, 80, 90, 70, 90, 75, 100],
                    borderColor: '#72a0ff',
                    borderWidth: "1",
                    backgroundColor: 'rgba(58,122,254,0.8)', 
                    pointBackgroundColor: 'rgba(255,255,255, 1)'
                }, {
                    label: "اولین مجموعه داده من",
                    data: [70, 60, 70, 50, 70, 55, 80],
                    borderColor: '#acc7ff',
                    borderWidth: "1",
					backgroundColor: 'rgba(58,122,254,1)',
                    pointBackgroundColor: 'rgba(58,122,254, 1)'
                }
            ]
        },
        options: {
			responsive:true,
			legend: false, 
			elements: {
					point:{
						radius: 0
					}
			},
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true, 
                        max: 100, 
                        min: 0, 
                        stepSize: 20, 
                        padding: 10
                    },
					display:false,
                }],
                xAxes: [{ 
                    ticks: {
                        padding: 5
                    },
					display:false,
                }]
            }
        }
    });
    

}
//#chart_widget_2
if(jQuery('#chart_widget_2').length > 0 ){
	
    const chart_widget_2 = document.getElementById("chart_widget_2").getContext('2d');
    //generate gradient
    const chart_widget_2gradientStroke = chart_widget_2.createLinearGradient(0, 0, 0, 250);
    chart_widget_2gradientStroke.addColorStop(0, "#a0bfff");
    chart_widget_2gradientStroke.addColorStop(1, "#a0bfff");

    // chart_widget_2.attr('height', '100');

    new Chart(chart_widget_2, {
        type: 'bar',
        data: {
            defaultFontFamily: 'inherit',
            labels: ["فرودین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"],
            datasets: [
                {
                    label: "اولین مجموعه داده من",
                    data: [65, 59, 80, 81, 56, 55, 40, 88, 45, 95, 54, 76],
                    borderColor: chart_widget_2gradientStroke,
                    borderWidth: "0",
                    backgroundColor: chart_widget_2gradientStroke, 
                    hoverBackgroundColor: chart_widget_2gradientStroke
                }
            ]
        },
        options: {
            legend: false,
            responsive: true, 
            maintainAspectRatio: false,  
            scales: {
                yAxes: [{
                    display: false, 
                    ticks: {
                        beginAtZero: true, 
                        display: false, 
                        max: 100, 
                        min: 0, 
                        stepSize: 10
                    }, 
                    gridLines: {
                        display: false, 
                        drawBorder: false
                    }
                }],
                xAxes: [{
                    display: false, 
                    barPercentage: 0.3, 
                    gridLines: {
                        display: false, 
                        drawBorder: false
                    }, 
                    ticks: {
                        display: false
                    }
                }]
            }
        }
    });

}
		
		
//#chart_widget_5
if(jQuery('#chart_widget_5').length > 0 ){
    new Chartist.Line("#chart_widget_5", {
        labels: ["1", "2", "3", "4", "5", "6", "7", "8"],
        series: [
            [4, 5, 3.5, 4, 5, 5.5, 5.8, 4.6]
        ]
    }, 
	{
        low: 0,	
        showArea: 1,
        showPoint: !0,
        showLine: !0,
        fullWidth: !0,
        lineSmooth: !1,
        chartPadding: {
            top: 2,
            right: 0,
            bottom: 0,
            left: 0
        },
        axisX: {
            showLabel: !1,
            showGrid: !1,
            offset: 0
        },
        axisY: {
            showLabel: !1,
            showGrid: !1,
            offset: 0
        }
    });
}
	
    $(".peity-line").peity("line", {
        fill: ["rgba(32, 222, 166, 1)"], 
        stroke: 'rgb(70, 255, 200)', 
        width: "400",
        height: "100"
    });
	
	if(jQuery('#ShareProfit2').length > 0 ){
		//doughut chart
		const ShareProfit2 = document.getElementById("ShareProfit2").getContext('2d');
		// ShareProfit2.height = 100;
		new Chart(ShareProfit2, {
			type: 'doughnut',
			data: {
				defaultFontFamily: 'inherit',
				datasets: [{
					data: [45, 25, 20],
					borderWidth: 3, 
					borderColor: "rgba(255, 255, 255, 1)",
					backgroundColor: [
						"rgba(58, 122, 254, 1)",
						"rgba(255, 159, 0, 1)",
						"rgba(41, 200, 112, 1)"
					],
					hoverBackgroundColor: [
						"rgba(58, 122, 254, 0.9)",
						"rgba(255, 159, 0, .9)",
						"rgba(41, 200, 112, .9)"
					]

				}],
				
			},
			options: {
				weight: 1,	
				 cutoutPercentage: 65,
				responsive: true,
				maintainAspectRatio: false
			}
		});
	}
	  

	jQuery('.dz-chat-user-box .dz-chat-user').on('click',function(){
		jQuery('.dz-chat-user-box').addClass('d-none');
		jQuery('.dz-chat-history-box').removeClass('d-none');
	}); 
	
	jQuery('.dz-chat-history-back').on('click',function(){
		jQuery('.dz-chat-user-box').removeClass('d-none');
		jQuery('.dz-chat-history-box').addClass('d-none');
	}); 
	
	var direction =  getUrlParams('dir');
	if(direction != 'rtl')
	{direction = 'ltr'; }

	var dezSettingsOptions = {
		typography: "roboto",
		version: "dark",
		layout: "Vertical",
		headerBg: "color_1",
		navheaderBg: "color_1",
		sidebarBg: "color_1",
		sidebarStyle: "mini",
		sidebarPosition: "fixed",
		headerPosition: "fixed",
		containerLayout: "full",
		direction: direction
	};
	new dezSettings(dezSettingsOptions); 
	
	jQuery(window).on('resize',function(){
		new dezSettings(dezSettingsOptions); 
	});

	
})(jQuery);

