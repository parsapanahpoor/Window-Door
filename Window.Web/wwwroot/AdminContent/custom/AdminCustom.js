$('#scrollTrigger').on('click',
    function () {
        $([document.documentElement, document.body]).animate({
            scrollTop: $("#scrollTargetLatestUsers").offset().top
        }, 1000);
    });

$('#scrollTrigger').css('cursor', 'pointer');
