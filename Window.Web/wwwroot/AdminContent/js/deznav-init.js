(function($) {
	
	var direction =  getUrlParams('dir');
	if(direction != 'rtl')
	{direction = 'ltr'; }

	var dezSettingsOptions = {
		typography: "roboto",
		version: "light",
		layout: "Vertical",
		headerBg: "color_1",
		navheaderBg: "color_3",
		sidebarBg: "color_3",
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