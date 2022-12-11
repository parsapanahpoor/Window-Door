/*
 bezierCanvas.js v1.1.0
 Copyright (c)2014 Sergey Serafimovich
*/

(function ( $ ) {

	"use strict";
 
    $.fn.bezierCanvas = function( options ) {
    
    
    	// Detect retina
    	var retina = window.devicePixelRatio > 1;
    	var pxlRatio = (retina) ? 2 : 1;
    	
    	if (navigator.userAgent.match(/iPad/i) != null || navigator.userAgent.match(/iPhone/i) != null) { pxlRatio = 1.5 }
    
 
		var i = 1;
		var objects = [];
		var styles = [];

         // These are the defaults
        var settings = $.extend({
            maxStyles: 10,
            maxLines: 100,
            strokeWidth: 0.5,
            lineSpacing: 1,
            spacingVariation: 0,
			colorBase: {r: 100,g: 100,b: 100},
			colorVariation: {r: 100,g: 100,b: 100},
			globalSpeed: 300,
			globalAlpha: 0.5,
			delayVariation: 0.3,
			speedLinear: false,
			coordinates: [],
			loop: false,
			moveCenterX: 0,
			moveCenterY: 0,
			spacingMode: 'add'
        }, options );
        
		
		var canvas = this[0];		
		var context = canvas.getContext("2d");		
		
		// Set canvas style size
		canvas.style.height = $(window).height()+'px';
		canvas.style.width = $(window).width()+'px';
		
		// Set canvas size
		canvas.width = $(window).width()*pxlRatio;
		canvas.height = $(window).height()*pxlRatio;
		
		// Save clear context
		context.save();
		
		// Handle window resize
		$(window).on("resize", function(){
			context.restore();
			canvas.style.height = $(window).height()+'px';
			canvas.style.width = $(window).width()+'px';
			
			canvas.width = $(window).width()*pxlRatio;
			canvas.height = $(window).height()*pxlRatio;
			context.translate(canvas.width/2+settings.moveCenterX, canvas.height/2+settings.moveCenterY);
		});

		// Set center
		context.translate(canvas.width/2+settings.moveCenterX, canvas.height/2+settings.moveCenterY);
		
		// Generate styles
		for(var c=0; c<settings.maxStyles; c++) {
			styles.push({
				cR: Math.round(settings.colorBase.r+Math.random()*settings.colorVariation.g),
				cG: Math.round(settings.colorBase.g+Math.random()*settings.colorVariation.g),
				cB: Math.round(settings.colorBase.b+Math.random()*settings.colorVariation.g),
				line: settings.strokeWidth*pxlRatio,
			});
			objects[c] = [];
		}
		
		// Generate other line params
		for(var k=0; k<settings.maxLines; k++) {
			var hm = Math.random();
			var color = Math.floor(Math.random()*settings.maxStyles);
			objects[color].push({
				speed: (settings.speedLinear) ? 0.1 + k/200 : settings.delayVariation*hm+0.1,
				pos: (k*settings.lineSpacing+settings.spacingVariation*hm)*pxlRatio
			});
		}


        // Add initial coordinates
        if(settings.coordinates.length == 0) {
			var plusminus = 0.5-Math.random();
			var plusminus2 = 0.5-Math.random();
			
			var m = (plusminus > 0) ? Math.ceil(plusminus) : Math.floor(plusminus);
			var m2 = (plusminus2 > 0) ? Math.ceil(plusminus2) : Math.floor(plusminus2);
			
			settings.coordinates.push({
				x1: m*canvas.width/2,
				y1: m2*canvas.height/3,
				x2: Math.random()*canvas.width-canvas.width/2,
				y2: Math.random()*canvas.height-canvas.height/2,
				x3: Math.random()*canvas.width-canvas.width/2,
				y3: Math.random()*canvas.height-canvas.height/2,
				x4: -m*canvas.width/2,
				y4: -m2*canvas.height/3,
				kx1: 15,
				ky1: 15*Math.random(),
				kx2: 15,
				ky2: 15
			});
		}

        
        
        
        
        var methods = {
        
        	// Add pseudorandom points
			addPoints: function() {
			
				var p = settings.coordinates[settings.coordinates.length-1];
				
				var plusminus2 = 0.7-Math.random();
				
				var m = (p.x4 < 0) ? -1 : 1;
				var m2 = (p.y4 < 0) ? -1 : 1;
				var m3 = (plusminus2 > 0) ? Math.ceil(plusminus2) : Math.floor(plusminus2);
	
				settings.coordinates.push({
					x1: p.x4,
					y1: p.y4,
					x2: p.x4+(p.x4-p.x3),
					y2: p.y4+(p.y4-p.y3),
					x3: Math.random()*canvas.width - canvas.width/2,
					y3: Math.random()*canvas.height - canvas.height/2,
					x4: -m*(canvas.width/4 + Math.round(Math.random()-0.2)*canvas.height/4),
					y4: -m2*m3*(canvas.height/4+Math.round(Math.random()-0.2)*canvas.height/4),
					kx1: p.kx2,
					ky1: p.ky2,
					kx2: 15-30*Math.random(),
					ky2: 15-30*Math.random()
	
				});
			},
			
			// Calculate segment points (De Casteljau's algorithm)
			segmentPoints: function (u0, u1, t0, t1, xP1, yP1, xP2, yP2, xP3, yP3, xP4, yP4) {
				return {
					x1: u0*u0*u0*xP1 + (t0*u0*u0 + u0*t0*u0 + u0*u0*t0)*xP2 + (t0*t0*u0 + u0*t0*t0 + t0*u0*t0)*xP3 + t0*t0*t0*xP4,
					x2: u0*u0*u1*xP1 + (t0*u0*u1 + u0*t0*u1 + u0*u0*t1)*xP2 + (t0*t0*u1 + u0*t0*t1 + t0*u0*t1)*xP3 + t0*t0*t1*xP4,
					x3: u0*u1*u1*xP1 + (t0*u1*u1 + u0*t1*u1 + u0*u1*t1)*xP2 + (t0*t1*u1 + u0*t1*t1 + t0*u1*t1)*xP3 + t0*t1*t1*xP4,
					x4: u1*u1*u1*xP1 + (t1*u1*u1 + u1*t1*u1 + u1*u1*t1)*xP2 + (t1*t1*u1 + u1*t1*t1 + t1*u1*t1)*xP3 + t1*t1*t1*xP4,
					y1: u0*u0*u0*yP1 + (t0*u0*u0 + u0*t0*u0 + u0*u0*t0)*yP2 + (t0*t0*u0 + u0*t0*t0 + t0*u0*t0)*yP3 + t0*t0*t0*yP4,
					y2: u0*u0*u1*yP1 + (t0*u0*u1 + u0*t0*u1 + u0*u0*t1)*yP2 + (t0*t0*u1 + u0*t0*t1 + t0*u0*t1)*yP3 + t0*t0*t1*yP4,
					y3: u0*u1*u1*yP1 + (t0*u1*u1 + u0*t1*u1 + u0*u1*t1)*yP2 + (t0*t1*u1 + u0*t1*t1 + t0*u1*t1)*yP3 + t0*t1*t1*yP4,
					y4: u1*u1*u1*yP1 + (t1*u1*u1 + u1*t1*u1 + u1*u1*t1)*yP2 + (t1*t1*u1 + u1*t1*t1 + t1*u1*t1)*yP3 + t1*t1*t1*yP4
				}
			}
		}


		var time = new Date().getTime();

 		// Render loop
 		function draw() {
			window.requestAnimationFrame(draw, this);
			
		    var now = new Date().getTime(),
		        dt = now - (time || now);
		 
		  //   time = now;
		    

			
			context.clearRect(-canvas.width/2-settings.moveCenterX, -canvas.height/2-settings.moveCenterY,canvas.width,canvas.height);
									
			// Group by colours
			for(var c=0; c<styles.length; c++) {
			
				// Set styles
				context.beginPath();
				context.lineWidth = styles[c].line;
				context.strokeStyle = 'rgba('+styles[c].cR+','+styles[c].cG+','+styles[c].cB+', ' + settings.globalAlpha + ')';
				
				// All lines of a style
				for(var j=0; j<objects[c].length; j++) {

					var o = objects[c][j];
					
					// Global time
					var gt0 = (dt/10)/(settings.globalSpeed)-objects[c][j].speed-1;
					var gt1 = (dt/10)/(settings.globalSpeed)-objects[c][j].speed;
					
					// If looped, then max value is loop end
					var l = Math.floor(Math.max(gt0, 0));
					if(settings.loop)
						var cl = l % (settings.coordinates.length-1);
					else
						var cl = l;
						
					if(settings.coordinates.length <= l && !settings.loop) {
						methods.addPoints();
					}


					
					// Time interval
					var t0 = gt0-l;
					var t1 = gt1-l;
				
					var u0 = 1.0 - Math.min(t0, 1);
					var u1 = 1.0 - Math.min(t1, 1);
					
					if(typeof(settings.coordinates[cl]) == "undefined") break;
					
					
					// Calculate coordinates for curve segment
					if(settings.spacingMode == "multiply") {
						var p = methods.segmentPoints(u0, u1, Math.min(t0, 1), Math.min(t1, 1),
								settings.coordinates[cl].x1*o.pos,
								settings.coordinates[cl].y1*o.pos,
								settings.coordinates[cl].x2*o.pos,
								settings.coordinates[cl].y2*o.pos,
								settings.coordinates[cl].x3*o.pos,
								settings.coordinates[cl].y3*o.pos,
								settings.coordinates[cl].x4*o.pos,
								settings.coordinates[cl].y4*o.pos 
						);
					} else {
						var p = methods.segmentPoints(u0, u1, Math.min(t0, 1), Math.min(t1, 1),
								settings.coordinates[cl].x1+o.pos*settings.coordinates[cl].kx1, 
								settings.coordinates[cl].y1+o.pos*settings.coordinates[cl].ky1, 
								settings.coordinates[cl].x2+o.pos*settings.coordinates[cl].kx1, 
								settings.coordinates[cl].y2+o.pos*settings.coordinates[cl].ky1, 
								settings.coordinates[cl].x3+o.pos*settings.coordinates[cl].kx2, 
								settings.coordinates[cl].y3+o.pos*settings.coordinates[cl].ky2, 
								settings.coordinates[cl].x4+o.pos*settings.coordinates[cl].kx2, 
								settings.coordinates[cl].y4+o.pos*settings.coordinates[cl].ky2
						);
					}
					// Draw
					context.moveTo(p.x1, p.y1);
					context.bezierCurveTo(
						p.x2,
						p.y2,
						p.x3,
						p.y3,
						p.x4,
						p.y4
					);
					
					// If time interval is more than 1 draw next segment
					
					if(gt1-l >= 1) {
						
						// If looped, then max value is loop end
						l = l+1;
						if(settings.loop)
							cl = l % (settings.coordinates.length-1);
						else
							cl = l;
					
						// If there is no new coordinates and not looped let's add some
						if(settings.coordinates.length <= l && !settings.loop) {
							methods.addPoints();
						}
						
						// Time interval
						var tt0 = Math.max(gt0-l, 0);
						var tt1 = Math.max(gt1-l, 0);
						
						var uu0 = 1.0 - Math.min(tt0, 1);
						var uu1 = 1.0 - Math.min(tt1, 1);
						
						// Calculate coordinates for curve segment
						if(settings.spacingMode == "multiply") {
							var pp = methods.segmentPoints(uu0, uu1, Math.min(tt0, 1), Math.min(tt1, 1),
								settings.coordinates[cl].x1*o.pos,
								settings.coordinates[cl].y1*o.pos,
								settings.coordinates[cl].x2*o.pos,
								settings.coordinates[cl].y2*o.pos,
								settings.coordinates[cl].x3*o.pos,
								settings.coordinates[cl].y3*o.pos,
								settings.coordinates[cl].x4*o.pos,
								settings.coordinates[cl].y4*o.pos
							);
						} else {
							var pp = methods.segmentPoints(uu0, uu1, Math.min(tt0, 1), Math.min(tt1, 1),
								settings.coordinates[cl].x1+o.pos*settings.coordinates[cl].kx1, 
								settings.coordinates[cl].y1+o.pos*settings.coordinates[cl].ky1, 
								settings.coordinates[cl].x2+o.pos*settings.coordinates[cl].kx1, 
								settings.coordinates[cl].y2+o.pos*settings.coordinates[cl].ky1, 
								settings.coordinates[cl].x3+o.pos*settings.coordinates[cl].kx2, 
								settings.coordinates[cl].y3+o.pos*settings.coordinates[cl].ky2, 
								settings.coordinates[cl].x4+o.pos*settings.coordinates[cl].kx2, 
								settings.coordinates[cl].y4+o.pos*settings.coordinates[cl].ky2
							);
						}
						
						// Draw
						context.moveTo(pp.x1, pp.y1);
						context.bezierCurveTo(
							pp.x2,
							pp.y2,
							pp.x3,
							pp.y3,
							pp.x4,
							pp.y4
						);
					}

				}
				
				// Stroke
				context.stroke();
				
			}
			
			



			// i++;

			
		};
		
		draw();
		
		
		return this;
 
    };
 
}( jQuery ));



/**
 * Provides requestAnimationFrame in a cross browser way.
 * @author paulirish / http://paulirish.com/
 */
 
if ( !window.requestAnimationFrame ) {
 
	window.requestAnimationFrame = ( function() {
 
		return window.webkitRequestAnimationFrame ||
		window.mozRequestAnimationFrame ||
		window.oRequestAnimationFrame ||
		window.msRequestAnimationFrame ||
		function( /* function FrameRequestCallback */ callback, /* DOMElement Element */ element ) {
 
			window.setTimeout( callback, 1000 / 60 );
 
		};
 
	} )();
 
}