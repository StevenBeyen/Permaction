// JavaScript Document
jQuery(document).ready(function ($) {

    /*var hadScroll = false; // Passe à 'true' quand on déclenche un défilement vers le header (pour empêcher le bug du "scroll pendant scroll")

    var getHeaderOffset = function () { // Fonction qui renvoit les coordonnées du header, on calcule la position du header avant chaque défilement au cas où la fenêtre a été redimensionnée entre temps
        return $("#header").offset().top;
    };

    var scrollToHeader = function (headerTop) { // Fonction qui effectue le smooth scroll vers le header
        $("html, body").animate({
            "scrollTop": headerTop
        });
    };

    $(window).bind('scroll', function () { // Détection d'un scroll
        windowTop = $(window).scrollTop(); // Position de la scrollbar par rapport au haut du site
        if (!hadScroll) { // Si on est pas déjà en train de scroller
            headerTop = getHeaderOffset(); // Position du header
            if (windowTop < headerTop) { // Si on est sur la page d'accueil
                scrollToHeader(headerTop); // On scroll vers le header
                hadScroll = true;
            }
        }
        else if (hadScroll && windowTop == 0) { // Si on a déjà effectué un défilement vers le header, mais qu'on est de retour tout en haut du site
            hadScroll = false; // On réinitialise la variable pour permettre un nouveau défilement automatique
        }
    });

    $(".scroll").on("click", function () { // Au clic sur le bouton
        headerTop = getHeaderOffset();
        scrollToHeader(headerTop); // On fait défiler l'accueil jusqu'au header
        return false;
    });*/


    $(".toggled-control").on("click", function () {
        var nav = $("#header-mobile nav");
        if (nav.is(":visible")) {
            nav.animate({"right": "-50%"}, 500, function () {
                nav.toggle();
            });
            $(this).animate({"right": 0}, 500);
        } else {
            nav.toggle();
            nav.animate({"right": 0}, 500);
            $(this).animate({"right": "50%"}, 500);
        }
        return false;
    });
    $("#header-mobile nav ul.sub-menu").each(function () {
        //console.log($(this).parent("li").children("a").css("background", "red"));
        var nodeli = $(this).parent("li");
        var nodea = nodeli.children("a");
        //console.log($(node));
        nodea.css('background', '#3e9275 url("/wp-content/themes/fermes-avenir/img/arrow-bottom.png")');
        nodea.css('background-repeat', 'no-repeat');
        nodea.css('background-position', 'right');
        nodea.on("click", function (event) {
            var node = $(this);
            $(node).toggleClass("hover");
            if ($(node).hasClass("hover")) {
                $(node).css("background-image", "./wp-content/themes/fermes-avenir/img/arrow-bottom.png");
            } else {
                $(node).css("background-image", "/wp-content/themes/fermes-avenir/img/arrow-top.png");
            }
            $(this).parent("li").children("ul").toggle();
            return false;
        });
    });

    $('#annonceFormSubmit').on('click', function () {
        $('#annonceForm').submit();
    });

    /*
    $("#liste-actu .bloc").hover(function(){
      $(this).toggleClass("hover");
    });
    */

    /*$(".switch").hover(function(){
        //alert("#"+this.id+" "+"#over");
      $("#"+this.id+" "+"#over").toggleClass("over-text");
        //jQuery(this).find(".subclass")
    });*/


    // init Masonry
    var $grid = $('.grid').masonry({
        columnWidth: '.item',
        itemSelector: '.item'
    });
    // layout Masonry after each image loads
    $grid.imagesLoaded().progress( function() {
      $grid.masonry('layout');
    });


});