jQuery(function($){'use strict';$(window).on('scroll',function(){if($(this).scrollTop()>150){$('.main-nav').addClass('menu-shrink');}else{$('.main-nav').removeClass('menu-shrink');}});jQuery('.mean-menu').meanmenu({meanScreenWidth:'991'});$('.modal a').not('.dropdown-toggle').on('click',function(){$('.modal').modal('hide');});$('select').niceSelect();jQuery('#rev_slider_1').show().revolution({sliderLayout:'auto',responsiveLevels:[1240,1024,778,480],gridwidth:[1240,1024,778,480],gridheight:[700,700,1000,1300],visibilityLevels:[1240,1024,1024,480],navigation:{arrows:{enable:true,style:'gyges',hide_onleave:false,left:{container:'layergrid',h_align:'center',v_align:'bottom',h_offset:-30,v_offset:155,},right:{container:'layergrid',h_align:'center',v_align:'bottom',h_offset:30,v_offset:155,}}}});$('.banner-slider').owlCarousel({items:1,loop:true,margin:15,nav:true,dots:false,smartSpeed:1000,autoplay:true,autoplayTimeout:4000,autoplayHoverPause:true,navText:["<i class='bx bx-left-arrow-alt'></i>","<i class='bx bx-right-arrow-alt'></i>"],});$('#Container').mixItUp();$('.testimonials-slider').owlCarousel({loop:true,center:true,margin:15,nav:true,dots:false,smartSpeed:1000,autoplay:true,autoplayTimeout:4000,autoplayHoverPause:true,navText:["<i class='bx bx-left-arrow-alt'></i>","<i class='bx bx-right-arrow-alt'></i>"],responsive:{0:{items:1,},768:{items:2,},992:{items:3,}}});$('.testimonials-slider-two').owlCarousel({items:1,loop:true,margin:15,nav:true,dots:false,smartSpeed:1000,autoplay:true,autoplayTimeout:4000,autoplayHoverPause:true,navText:["<i class='bx bx-left-arrow-alt'></i>","<i class='bx bx-right-arrow-alt'></i>"],});$('.minus').on('click',function(){var $input=$(this).parent().find('input');var count=parseInt($input.val())-1;count=count<1?1:count;$input.val(count);$input.change();return false;});$('.plus').on('click',function(){var $input=$(this).parent().find('input');$input.val(parseInt($input.val())+1);$input.change();return false;});jQuery(window).on('load',function(){jQuery('.loader').fadeOut(500);});$(window).on('scroll',function(){var scrolled=$(window).scrollTop();if(scrolled>100)$('.go-top').addClass('active');if(scrolled<100)$('.go-top').removeClass('active');});$('.go-top').on('click',function(){$('html, body').animate({scrollTop:'0'},500);});$('.sorting-slider').owlCarousel({loop:true,margin:15,nav:true,dots:false,smartSpeed:1000,autoplay:true,autoplayTimeout:4000,autoplayHoverPause:true,navText:["<i class='bx bx-left-arrow-alt'></i>","<i class='bx bx-right-arrow-alt'></i>"],responsive:{0:{items:2,},768:{items:4,},992:{items:6,}}});let getDaysId=document.getElementById('days');if(getDaysId!==null){const second=1000;const minute=second*60;const hour=minute*60;const day=hour*24;let countDown=new Date('February 25, 2021 00:00:00').getTime();setInterval(function(){let now=new Date().getTime();let distance=countDown-now;document.getElementById('days').innerText=Math.floor(distance/(day)),document.getElementById('hours').innerText=Math.floor((distance%(day))/(hour)),document.getElementById('minutes').innerText=Math.floor((distance%(hour))/(minute)),document.getElementById('seconds').innerText=Math.floor((distance%(minute))/second);},second);};$('.js-modal-btn').modalVideo();$('.video-slider').owlCarousel({loop:true,margin:15,nav:true,dots:false,smartSpeed:1000,autoplay:true,autoplayTimeout:4000,autoplayHoverPause:true,navText:["<i class='bx bx-left-arrow-alt'></i>","<i class='bx bx-right-arrow-alt'></i>"],responsive:{0:{items:1,},768:{items:2,},992:{items:2,}}});$('.accordion > li:eq(0) .faq-head').addClass('active').next().slideDown();$('.accordion .faq-head').on('click',function(j){var dropDown=$(this).closest('li').find('.faq-content');$(this).closest('.accordion').find('.faq-content').not(dropDown).slideUp(300);if($(this).hasClass('active')){$(this).removeClass('active');}else{$(this).closest('.accordion').find('.faq-head.active').removeClass('active');$(this).addClass('active');}
dropDown.stop(false,true).slideToggle(300);j.preventDefault();});setTimeout(function(){$('#modal-subscribe').modal('show')},1500)
$('.newsletter-form').validator().on('submit',function(event){if(event.isDefaultPrevented()){formErrorSub();submitMSGSub(false,'Please enter your email correctly.');}else{event.preventDefault();}});function callbackFunction(resp){if(resp.result==='success'){formSuccessSub();}
else{formErrorSub();}}
function formSuccessSub(){$('.newsletter-form')[0].reset();submitMSGSub(true,'Thank you for subscribing!');setTimeout(function(){$('#validator-newsletter').addClass('hide');},4000)}
function formErrorSub(){$('.newsletter-form').addClass('animated shake');setTimeout(function(){$('.newsletter-form').removeClass('animated shake');},1000)}
function submitMSGSub(valid,msg){if(valid){var msgClasses='validation-success';}else{var msgClasses='validation-danger';}
$('#validator-newsletter').removeClass().addClass(msgClasses).text(msg);}
    $('.newsletter-form').ajaxChimp({ url: 'https://envytheme.us20.list-manage.com/subscribe/post?u=60e1ffe2e8a68ce1204cd39a5&amp;id=42d6d188d9', callback: callbackFunction });
}(jQuery));


$(document).ready(function () {

    var sync1 = $("#sync1");
    var sync2 = $("#sync2");
    var slidesPerPage = 4; //globaly define number of elements per page
    var syncedSecondary = true;

    sync1.owlCarousel({
        items: 1,
        slideSpeed: 2000,
        nav: true,
        autoplay: false,
        dots: true,
        loop: true,
        responsiveRefreshRate: 200,
        navText: ['<svg width="100%" height="100%" viewBox="0 0 11 20"><path style="fill:none;stroke-width: 1px;stroke: #000;" d="M9.554,1.001l-8.607,8.607l8.607,8.606"/></svg>', '<svg width="100%" height="100%" viewBox="0 0 11 20" version="1.1"><path style="fill:none;stroke-width: 1px;stroke: #000;" d="M1.054,18.214l8.606,-8.606l-8.606,-8.607"/></svg>'],
    }).on('changed.owl.carousel', syncPosition);

    sync2
        .on('initialized.owl.carousel', function () {
            sync2.find(".owl-item").eq(0).addClass("current");
        })
        .owlCarousel({
            items: slidesPerPage,
            dots: true,
            nav: true,
            smartSpeed: 200,
            slideSpeed: 500,
            slideBy: slidesPerPage, //alternatively you can slide by 1, this way the active slide will stick to the first item in the second carousel
            responsiveRefreshRate: 100
        }).on('changed.owl.carousel', syncPosition2);

    function syncPosition(el) {
        //if you set loop to false, you have to restore this next line
        //var current = el.item.index;

        //if you disable loop you have to comment this block
        var count = el.item.count - 1;
        var current = Math.round(el.item.index - (el.item.count / 2) - .5);

        if (current < 0) {
            current = count;
        }
        if (current > count) {
            current = 0;
        }

        //end block

        sync2
            .find(".owl-item")
            .removeClass("current")
            .eq(current)
            .addClass("current");
        var onscreen = sync2.find('.owl-item.active').length - 1;
        var start = sync2.find('.owl-item.active').first().index();
        var end = sync2.find('.owl-item.active').last().index();

        if (current > end) {
            sync2.data('owl.carousel').to(current, 100, true);
        }
        if (current < start) {
            sync2.data('owl.carousel').to(current - onscreen, 100, true);
        }
    }

    function syncPosition2(el) {
        if (syncedSecondary) {
            var number = el.item.index;
            sync1.data('owl.carousel').to(number, 100, true);
        }
    }

    sync2.on("click", ".owl-item", function (e) {
        e.preventDefault();
        var number = $(this).index();
        sync1.data('owl.carousel').to(number, 300, true);
    });
});