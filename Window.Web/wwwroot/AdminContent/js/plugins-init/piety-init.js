
(function($) {
    "use strict"


/****************
Piety chart
*****************/

	
	function getGraphBlockSize(selector)
	{
		var screenWidth = $(window).width();
		var graphBlockSize = '100%';
		
		if(screenWidth <= 768)
			{
				screenWidth = (screenWidth < 300 )?screenWidth:300;
				
				var blockWidth  = jQuery(selector).parent().innerWidth() - jQuery(selector).parent().width();
		
				blockWidth = Math.abs(blockWidth);
				
				var graphBlockSize = screenWidth - blockWidth - 10;	
			}
	
		
		
		return graphBlockSize;
		
	}
	
	
    $(".bar-line").peity("bar", {
        width: "100",
        height: "100"
    });

    
    $("span.pie").peity("pie", {
        fill: ['#3a7afe', 'rgba(58, 122, 254, .3)'], 
        width: "100",
        height: "100"
    });    
    
    
    $("span.donut").peity("donut", {
        width: "100",
        height: "100"
    });
    
    
    
    $(".peity-line").peity("line", {
        fill: ["rgba(58, 122, 254, .5)"], 
        stroke: '#3a7afe', 
        width: "100%",
        height: "100"
    });
    
    $(".peity-line-2").peity("line", {
        fill: "#fa707e", 
        stroke: "#f77f8b", 
        //width: "100%",
        width: getGraphBlockSize('.peity-line-2'),
		strokeWidth: "3",
        height: "150"
    });
	
	$(".peity-line-3").peity("line", {
        fill: "#673bb7", 
        stroke: "#ab84f3", 
        width: "100%",
		strokeWidth: "3",
        height: "150"
    });
    
    $(".bar").peity("bar", {
        fill: ["#3a7afe", "#10ca93", "#ff9f00"],  
        width: "100%",
        height: "100",
	});
    
	$(".bar1").peity("bar", {
        fill: ["#3a7afe", "#10ca93", "#ff9f00"],    
        //width: "100%",
        width: getGraphBlockSize('.bar1'),
        height: "140"
    });
    
    $(".bar-colours-1").peity("bar", {
        fill: ["#3a7afe", "#10ca93", "#ff9f00"],  
        width: "100",
        height: "100"
    });
    
    
    
    $(".bar-colours-2").peity("bar", {
        fill: function(t, e, i) {
            return "rgb(58, " + parseInt(e / i.length * 122) + ", 254)"
        },
        width: "100",
        height: "100"
    });
    
    $(".bar-colours-3").peity("bar", {
        fill: function(t, e, i) {
            return "rgb(16, " + parseInt(e / i.length * 202) + ", 147)"
        },
        width: "100",
        height: "100"
    });
    
    $(".pie-colours-1").peity("pie", {
        fill: ["cyan", "magenta", "yellow", "black"],
        width: "100",
        height: "100"
    });
    
    $(".pie-colours-2").peity("pie", {
        fill: ["#3a7afe", "#10ca93", "#f25767", "#ff9f00", "#00afef"],
        width: "100",
        height: "100"
    });
    
    
    
    $(".data-attr").peity("donut");



    var t = $(".updating-chart").peity("line", {
        fill: ['rgba(58, 122, 254, .5)'],
        stroke: 'rgb(58, 122, 254)', 
        width: "100%",
        height: 100
    });
    setInterval(function() {
        var e = Math.round(10 * Math.random()),
            i = t.text().split(",");
        i.shift(), i.push(e), t.text(i.join(",")).change()
    }, 1e3)

    

})(jQuery);