/**
 * jQuery.hoverPlay
 * @version   1.0
 * @author    biohzrdmx <github.com/biohzrdmx>
 * @requires  jQuery 1.8+
 * @license   MIT
 * @copyright Copyright © 2018 biohzrdmx. All rights reserved.
 */
;(function($) {
	$.hoverPlay = {};
	$.fn.hoverPlay = function(options) {
		if (!this.length) { return this; }
		var opts = $.extend(true, {}, $.hoverPlay.defaults, options);
		this.each(function() {
			var el = $(this),
				video = el.get(0);
			if (typeof video['play'] === 'function') {
				video.controls = false;
				video.loop = true;
				el.on('mouseover', function() {
					var timeout = el.data('hoverPlayTimeout');
					if (!timeout) {
						timeout = setTimeout(function() {
							el.data('hoverPlayTimeout', null);
							opts.callbacks.play(el, video);
							el.trigger('hoverPlay');
						}, opts.playDelay);
						el.data('hoverPlayTimeout', timeout);
					}
				}).on('mouseout', function() {
					var timeout = el.data('hoverPlayTimeout');
					if (timeout) {
						clearTimeout(timeout);
						el.data('hoverPlayTimeout', null);
					}
					setTimeout(function() {
						opts.callbacks.pause(el, video);
						el.trigger('hoverPause');
					},  opts.pauseDelay);
				});
			}
		});
		return this;
	};
	$.hoverPlay.defaults = {
		playDelay: 350,
		pauseDelay: 0,
		callbacks: {
			play: function(el,  video) {
				video.play();
			},
			pause: function(el,  video) {
				video.pause();
			}
		}
	};
	jQuery(document).ready(function($) {
		$('[data-play=hover]').hoverPlay();
	});
})(jQuery);