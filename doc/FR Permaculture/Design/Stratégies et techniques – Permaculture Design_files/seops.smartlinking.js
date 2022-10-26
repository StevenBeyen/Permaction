jQuery(document).ready(function(){
	
	jQuery('.seops_smart_linking_cloak').bind('contextmenu', function(e){
		return false;
	});
	
    jQuery('.seops_smart_linking_cloak').click(function(e) {
		e.preventDefault();
		window.location.href = jQuery(this).attr('data-url');
	});
	
});