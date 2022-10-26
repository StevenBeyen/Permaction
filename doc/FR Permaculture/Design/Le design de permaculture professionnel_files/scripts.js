/*!
 * jQuery Tiny Pub/Sub
 * https://github.com/cowboy/jquery-tiny-pubsub
 *
 * Copyright (c) 2013 "Cowboy" Ben Alman
 * Licensed under the MIT license.
 */

(function ($) {

	var o = $({});

	$.subscribe = function () {
		o.on.apply(o, arguments);
	};

	$.unsubscribe = function () {
		o.off.apply(o, arguments);
	};

	$.publish = function () {
		o.trigger.apply(o, arguments);
	};

}(jQuery));

window.ET_App = window.ET_App || {};
window.et_pb_extra_load_event_fired = false;

(function (exports, $) {

	var Elements;

	Elements = {
		cacheElements: function () {
			this.$window = $(window);

			this.$body = $('body');
			this.$container = this.$body.find('#page-container');

			this.$main_header = this.$container.find('> .header');

			this.$main_nav = this.$container.find('#main-header');
			this.main_nav_height = this.$main_nav.innerHeight();

			this.$top_nav = this.$container.find('#top-header');
			this.top_nav_height = this.$top_nav.innerHeight();

			this.$content_area = this.$container.find('#main-content');
			this.$wpadminbar = this.$body.find('#wpadminbar');
		},
		init: function () {
			this.cacheElements();

			$.subscribe('et-window.resized', $.proxy(this.hasMainNavHeightChanged, this));
			$.subscribe('et-window.resized', $.proxy(this.hasTopNavHeightChanged, this));
		},
		hasMainNavHeightChanged: function () {
			var current_nav_height = this.$main_nav.innerHeight();

			if (current_nav_height === this.main_nav_height) {
				return;
			}

			this.main_nav_height = current_nav_height;

			$.publish('et-main_nav.changed_height');
		},
		hasTopNavHeightChanged: function () {
			var current_nav_height = this.$top_nav.innerHeight();

			if (current_nav_height === this.top_nav_height) {
				return;
			}

			this.top_nav_height = current_nav_height;

			$.publish('et-top_nav.changed_height');
		}
	};

	$(function () {
		Elements.init();
	});

	$(window).resize(function () {
		$.publish('et-window.resized');
	});

	exports.Elements = Elements;

})(ET_App, jQuery);

(function (exports, $) {

	var DynamicStyles;

	DynamicStyles = {
		addRule: function (selector, rules) {
			this.maybeAddStyleElement();

			this.elements[selector] = rules;

			this.render();
		},
		addStyleElement: function () {
			$('head').append('<style id="' + this.styleID + '" />');
			this.$container = $('style#' + this.styleID);
			this.styleTagAdded = true;
		},
		init: function () {
			this.elements = {};
			this.styleTagAdded = false;
			this.styleID = 'et_custom_script_css';
		},
		maybeAddStyleElement: function () {
			if (this.styleTagAdded) {
				return;
			}

			this.addStyleElement();
		},
		render: function () {
			var output = '';

			for (var selector in this.elements) {
				output += selector + ' { ' + this.elements[selector] + ' }' + "\n";
			}

			this.$container.html(output);
		}
	};

	DynamicStyles.init();

	exports.DynamicStyles = DynamicStyles;

})(ET_App, jQuery);

(function (exports, $) {

	var elements = ET_App.Elements,
		FixedNav;

	FixedNav = {
		activateFixedPosition: function () {
			var self = this,
				main_header_fixed_height;

			if (this.maybeDeactivateOnMobile()) {
				return;
			}

			this.$nav_container.addClass(this.fixed_class);

			if (!this.isHeaderCentered()) {
				if (this.logo_initial_width_value > this.logo_fixed_width_value) {
					this.$logo.stop().animate({
						'width': this.logo_fixed_width_value
					});
				} else {
					this.$logo.css({
						'width': this.logo_fixed_width_value
					});
				}

				main_header_fixed_height = this.main_nav_fixed_height;
			} else {
				main_header_fixed_height = this.fixed_logo_height * 2 + $('#et-navigation > ul > li > a').height() + 20;
			}

			if (this.hide_nav_until_scroll) {
				return;
			}

			this.$main_nav_wrapper.height(main_header_fixed_height);

			setTimeout(function () {
				var main_nav_height = self.$main_nav.innerHeight();

				if (self.$main_nav_wrapper.height() !== main_nav_height) {
					self.$main_nav_wrapper.height(main_nav_height);
				}
			}, 500);
		},
		activateHideNav: function () {
			this.hide_nav_until_scroll = true;
		},
		applyHideNav: function () {
			var body_height, main_nav_height, viewport_height;

			if (!this.hide_nav_until_scroll) {
				return false;
			}

			body_height = elements.$body.height();
			main_nav_height = this.main_nav_height;
			viewport_height = elements.$window.height() + main_nav_height + this.viewpoint_height_modifier;

			if (body_height > viewport_height) {
				if (this.isHideNavDisabled()) {
					this.hideNavEnable();
				}

				this.hideNavMoveOffScreen();
			} else {
				this.hideNavDisable();
			}
		},
		applyWaypoints: function () {
			var self = this;

			// initialize waypoints
			this.$main_nav_wrapper.waypoint({
				offset: this.offset,
				handler: $.proxy(self.scroll_trigger, self)
			});
		},
		bindEvents: function () {
			$.subscribe('et-main_nav.changed_height', $.proxy(this.onMainNavHeightChanged, this));
		},
		cacheElements: function () {
			this.$nav_container = elements.$container;
			this.$main_nav = elements.$main_nav;
			this.$main_nav_wrapper = this.$main_nav.closest('#main-header-wrapper');
			this.main_nav_height = this.$main_nav.height();
			this.main_nav_fixed_height = this.$main_nav.data('fixed-height');

			this.$content_area = elements.$content_area;
			this.content_area_default_padding = parseInt(this.$content_area.css('padding-top'));

			this.fixed_class = 'et-fixed-header';
			this.mobile_breakpoint_width = 1024;

			this.hide_nav_class = 'et_hide_nav';
			this.hide_nav_disabled_class = 'et_hide_nav_disabled';

			this.hide_nav_until_scroll = elements.$body.hasClass(this.hide_nav_class) || elements.$body.hasClass(this.hide_nav_disabled_class);
			this.viewpoint_height_modifier = 200;

			this.$logo = this.$nav_container.find('.logo');
			this.logo_height = this.$logo.height();
			this.logo_width = this.$logo.width();
			this.fixed_logo_height = parseInt(this.$main_nav.attr('data-fixed-height')) * (parseInt(this.$logo.attr('data-fixed-height')) / 100);
			this.fixed_logo_width = (this.logo_width / this.logo_height) * this.fixed_logo_height;

			this.logo_initial_width_value = parseInt(this.logo_width);
			this.logo_fixed_width_value = parseInt(this.fixed_logo_width);
		},
		calculateOffset: function () {
			this.offset = 0;

			if (elements.$wpadminbar.length) {
				this.offset = this.offset + elements.$wpadminbar.innerHeight();
			}

			if (!elements.$top_nav.is(':visible')) {
				this.offset = -1;
			}
		},
		deactivateFixedPosition: function () {
			this.$nav_container.removeClass(this.fixed_class);

			if (this.maybeDeactivateOnMobile()) {
				return;
			}

			if (!this.isHeaderCentered()) {
				if (this.logo_fixed_width_value > this.logo_initial_width_value) {
					this.$logo.stop().animate({
						'width': this.logo_initial_width_value
					});
				} else {
					this.$logo.css({
						'width': this.logo_initial_width_value
					});
				}
			}

			if (this.hide_nav_until_scroll) {
				return;
			}

			this.$main_nav_wrapper.height(this.main_nav_height);
		},
		deactivateHideNav: function () {
			this.hide_nav_until_scroll = false;
		},
		detachHideNav: function () {
			this.deactivateHideNav();
			this.hideNavDisable();
		},
		init: function () {
			if (!$.fn.waypoint || !elements.$body.hasClass('et_fixed_nav')) {
				return;
			}

			this.cacheElements();

			this.bindEvents();

			this.calculateOffset();

			this.run();

			this.applyHideNav();

			this.$logo.attr({
				'data-fixed-width': this.fixed_logo_width,
				'data-initial-width': this.logo_width,
				'data-initial-height': this.logo_height
			});
		},
		isHeaderCentered: function () {
			return elements.$main_header.hasClass('centered');
		},
		isHideNavDisabled: function () {
			return elements.$body.hasClass(this.et_hide_nav_disabled);
		},
		maybeDeactivateOnMobile: function () {
			if (elements.$body.width() <= this.mobile_breakpoint_width) {
				this.$nav_container.removeClass(this.fixed_class);
				this.$main_nav_wrapper.css('height', 'auto');
				return true;
			}

			return false;
		},
		hideNavDisable: function () {
			elements.$body
				.removeClass(this.et_hide_nav)
				.addClass(this.et_hide_nav_disabled);

			this.$main_nav.css({
				'transform': 'translateY(0)',
				'opacity': '1'
			});
		},
		hideNavEnable: function () {
			elements.$body
				.removeClass(this.et_hide_nav_disabled)
				.addClass(this.et_hide_nav);
		},
		hideNavMoveOffScreen: function () {
			var transform = 'translateY( -' + this.main_nav_height + 'px )';
			this.$main_nav.css('transform', transform);
		},
		onMainNavHeightChanged: function () {
			// In fixed header state, main_nav height calculation is inappropriate
			if (ET_App.Elements.$container.hasClass('et-fixed-header')) {
				return;
			}
			this.main_nav_height = this.$main_nav.height();

			this.$main_nav_wrapper.height(this.main_nav_height);
		},
		reApplyHideNav: function () {
			this.activateHideNav();
			this.hideNavMoveOffScreen();
			this.applyHideNav();
		},
		run: function () {
			this.deactivateFixedPosition();

			this.applyWaypoints();
		},
		scroll_trigger: function (direction) {
			if (direction === 'down') {
				this.activateFixedPosition();
			} else {
				this.deactivateFixedPosition();
			}
		}
	};

	$(window).load(function () {
		FixedNav.init();
	});

	$(window).resize(function () {
		FixedNav.applyHideNav();
	});

	exports.FixedNav = FixedNav;

})(ET_App, jQuery);

(function ($) {
	if (EXTRA.is_ab_testing_active && 'yes' === EXTRA.is_cache_plugin_active) {
		// Update window.et_pb_extra_load_event_fired if .load event fired
		$(window).load(function () {
			window.et_pb_extra_load_event_fired = true;
		});

		// load all scripts once the et_pb_ab_subject_ready event fired
		$('body').on('et_pb_ab_subject_ready', function () {
			et_pb_init_scripts();
		});
	} else {
		// load script immediately if no split testing enabled on page
		et_pb_init_scripts();
	}

	function et_pb_init_scripts() {
		jQuery.fn.reverse = [].reverse;

		var $et_window = $(window),
			$et_container = $('#main-content > .container'),
			et_container_width = $et_container.width(),
			et_single_col_breakpoint = 690,
			et_is_mobile_device = navigator.userAgent.match(/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/),
			et_is_opera_mini = navigator.userAgent.match(/Opera Mini|Opera Mobi/),
			et_is_ipad = navigator.userAgent.match(/iPad/),
			$et_builder_video_section = $('.et_builder_section_video_bg'),
			$et_builder_parallax = $('.et_builder_parallax_bg');

		/* Adding body class for targetting Opera Mini/Mobi specific issue */
		if (et_is_opera_mini) {
			document.body.className += " et_opera_mini";
		}

		// Returns a function, that, as long as it continues to be invoked, will not
		// be triggered. The function will be called after it stops being called for
		// N milliseconds. If `immediate` is passed, trigger the function on the
		// leading edge, instead of the trailing.
		function debounce(func, wait, immediate) {
			var timeout;
			return function () {
				var context = this,
					args = arguments;
				var later = function () {
					timeout = null;
					if (!immediate) {
						func.apply(context, args);
					}
				};
				var callNow = immediate && !timeout;
				clearTimeout(timeout);
				timeout = setTimeout(later, wait);
				if (callNow) {
					func.apply(context, args);
				}
			};
		}

		function form_placeholders_init($form) {
			$form.find('input:text, textarea').each(function (index, domEle) {
				var $et_current_input = jQuery(domEle),
					$et_comment_label = $et_current_input.siblings('label'),
					et_comment_label_value = $et_current_input.siblings('label').text();
				if ($et_comment_label.length) {
					$et_comment_label.hide();
					if ($et_current_input.siblings('span.required')) {
						et_comment_label_value += $et_current_input.siblings('span.required').text();
						$et_current_input.siblings('span.required').hide();
					}
					$et_current_input.val(et_comment_label_value);
				}
			}).bind('focus', function () {
				var et_label_text = jQuery(this).siblings('label').text();
				if (jQuery(this).siblings('span.required').length) {
					et_label_text += jQuery(this).siblings('span.required').text();
				}
				if (jQuery(this).val() === et_label_text) {
					jQuery(this).val("");
				}
			}).bind('blur', function () {
				var et_label_text = jQuery(this).siblings('label').text();
				if (jQuery(this).siblings('span.required').length) {
					et_label_text += jQuery(this).siblings('span.required').text();
				}
				if (jQuery(this).val() === "") {
					jQuery(this).val(et_label_text);
				}
			});
		}

		// remove placeholder text before form submission
		function remove_placeholder_text($form) {
			$form.find('input:text, textarea').each(function (index, domEle) {
				var $et_current_input = jQuery(domEle),
					$et_label = $et_current_input.siblings('label');

				if ($et_label.length && $et_label.is(':hidden')) {
					if ($et_label.text() === $et_current_input.val()) {
						$et_current_input.val('');
					}
				}
			});
		}

		function et_duplicate_menu(menu, append_to, menu_id, menu_class) {
			var $cloned_nav;
			menu.clone().attr('id', menu_id).removeClass().attr('class', menu_class).appendTo(append_to);
			$cloned_nav = append_to.find('> ul');
			$cloned_nav.find('.menu_slide').remove();
			$cloned_nav.find('li:first').addClass('et_first_mobile_item');

			append_to.find('a').click(function (event) {
				event.stopPropagation();
			});
		}

		function resize_section_video_bg($video) {
			$element = typeof $video !== 'undefined' ? $video.closest('.et_builder_section_video_bg') : $('.et_builder_section_video_bg');

			$element.each(function () {
				var $this_el = $(this),
					ratio = (typeof $this_el.attr('data-ratio') !== 'undefined') ? $this_el.attr('data-ratio') : $this_el.find('video').attr('width') / $this_el.find('video').attr('height'),
					$video_elements = $this_el.find('.mejs-video, video, object').css('margin', 0),
					$container = $this_el.closest('.et_builder_section').length ? $this_el.closest('.et_builder_section') : $this_el.closest('.et_builder_slides'),
					body_width = $container.width(),
					container_height = $container.innerHeight(),
					width,
					height;

				if (typeof $this_el.attr('data-ratio') === 'undefined') {
					$this_el.attr('data-ratio', ratio);
				}

				if (body_width / container_height < ratio) {
					width = container_height * ratio;
					height = container_height;
				} else {
					width = body_width;
					height = body_width / ratio;
				}

				$video_elements.width(width).height(height);
			});
		}

		function center_video($video) {
			$element = typeof $video !== 'undefined' ? $video : $('.et_builder_section_video_bg .mejs-video');

			$element.each(function () {
				var $video_width = $(this).width() / 2;
				var $video_width_negative = 0 - $video_width;
				$(this).css("margin-left", $video_width_negative);

				if (typeof $video !== 'undefined') {
					if ($video.closest('.et_builder_slider').length && !$video.closest('.et_builder_first_video').length) {
						return false;
					}

					setTimeout(function () {
						$(this).closest('.et_builder_preload').removeClass('et_builder_preload');
					}, 500);
				}
			});
		}

		function et_apply_parallax() {
			var $this = $(this),
				element_top = $this.offset().top,
				window_top = $et_window.scrollTop(),
				y_pos = (((window_top + $et_window.height()) - element_top) * 0.3),
				main_position;

			main_position = 'translate3d(0, ' + y_pos + 'px, 0px)';

			$this.find('.et_builder_parallax_bg').css({
				'-webkit-transform': main_position,
				'-moz-transform': main_position,
				'-ms-transform': main_position,
				'transform': main_position
			});
		}

		function et_parallax_set_height() {
			var $this = $(this),
				bg_height;

			bg_height = ($et_window.height() * 0.3 + $this.innerHeight());

			$this.find('.et_builder_parallax_bg').css({
				'height': bg_height
			});
		}

		function et_calclate_admin_bar_height() {
			var admin_bar = $('#wpadminbar').length,
				admin_bar_height = 0,
				admin_bar_position;

			if (admin_bar) {
				admin_bar_position = $('#wpadminbar').length ? $('#wpadminbar').css('position') : '';
				admin_bar_height = $('#wpadminbar').length ? $('#wpadminbar').height() : 0;
				if ('fixed' !== admin_bar_position) {
					admin_bar_height = 0;
				}
			}

			return admin_bar_height;
		}

		function et_pb_on_load_scripts() {
			setTimeout(function () {
				$('.et_builder_preload').removeClass('et_builder_preload');
			}, 500);

			if ($et_builder_parallax.length && !et_is_mobile_device) {
				$et_builder_parallax.each(function () {
					if ($(this).hasClass('et_builder_parallax_css')) {
						return;
					}

					var $this_parent = $(this).parent();

					$.proxy(et_parallax_set_height, $this_parent)();

					$.proxy(et_apply_parallax, $this_parent)();

					$et_window.on('scroll', $.proxy(et_apply_parallax, $this_parent));

					$et_window.on('resize', $.proxy(et_parallax_set_height, $this_parent));
					$et_window.on('resize', $.proxy(et_apply_parallax, $this_parent));
				});
			}

			if ($.fn.waypoint) {
				$('.et-waypoint, .et_builder_waypoint').waypoint({
					offset: '50%',
					handler: function () {
						$(this).addClass('et-animated et_builder_animated');
					}
				});
			} /* end if ( $.fn.waypoint ) */
		}

		if (window.et_pb_extra_load_event_fired) {
			et_pb_on_load_scripts();
		} else {
			$(window).load(function () {
				et_pb_on_load_scripts();
			});
		}

		$(document).ready(function () {
			var $comment_form = $('#commentform'),
				$rating_stars = $('#rating-stars'),
				$tabbed_post_module = $('.tabbed-post-module'),
				$post_module = $('.post-module'),
				$et_slider = $('.et-slider'),
				$featured_posts_slider_module = $('.featured-posts-slider-module'),
				$posts_carousel_module = $('.posts-carousel-module'),
				$filterable_portfolio = $('.et_filterable_portfolio'),
				$timeline = $('#timeline'),
				$contact_form = $('.extra-contact-form'),
				$post_format_map = $('.post-format-map'),
				$paginated = $('.paginated'),
				$internal_links = $('a[href^="#"]:not([href="#"])'),
				$back_to_top = $('#back_to_top'),
				$social_share_icons = $('.ed-social-share-icons'),
				$woocommerce_details_accordion = $('.extra-woocommerce-details-accordion'),
				$search_icon = $('#et-search-icon'),
				$et_trending_container = $('#et-trending-container'),
				$widget_et_recent_videos = $('.widget_et_recent_videos'),
				$et_top_cart_total = $('.et-top-cart-total');

			if ( $et_top_cart_total.length && $('.shop_table.cart').length ) {
				$( document.body ).on( 'updated_wc_div', function(){
					var new_total = 0;
					var new_text;
					var new_title;
					$('.shop_table.cart').find('.product-quantity input').each(function(){
						new_total = new_total + parseInt( $(this).val() );
					});

					if ( new_total === 1 ) {
						new_text  = EXTRA.item_count;
						new_title = EXTRA.item_in_cart_count;
					} else {
						new_text  = EXTRA.items_count;
						new_title = EXTRA.items_in_cart_count;
					}

					new_title = new_title.replace('%d', new_total);
					new_text = new_text.replace('%d', new_total);

					$et_top_cart_total.find('.et-cart span').text(new_text);
					$et_top_cart_total.find('.et-cart').attr('title', new_title);
				});
			}

			if ($internal_links.length) {
				$internal_links.click(function (e) {
					if (window.location.pathname.replace(/^\//, '') === this.pathname.replace(/^\//, '') && location.hostname === this.hostname) {
						var target_hash = this.hash,
							$target = $(target_hash);

						$target = $target.length ? $target : $('[name=' + this.hash.slice(1) + ']');

						if ($target.length) {

							var window_width = $(window).width(),
								menu_offset = 0,
								scroll_position = $target.offset().top,
								admin_bar_height = $('#wpadminbar').length ? $('#wpadminbar').height() : 0,
								window_top = $(window).scrollTop() + admin_bar_height,
								window_bottom = window_top + $(window).height();

							e.preventDefault();

							if (scroll_position > window_top && scroll_position < window_bottom) {
								return;
							}

							if (admin_bar_height > 0 && window_width > 600) {
								menu_offset += admin_bar_height;
							}

							scroll_position = scroll_position - menu_offset;

							$('html, body').animate({
								scrollTop: scroll_position
							}, {
								duration: 500,
								complete: function () {
									setTimeout(function () {
										var yScroll = document.body.scrollTop;
										window.location.hash = target_hash;
										document.body.scrollTop = yScroll;
									}, 200);
								}
							});
						}
					}
				});
			}

			if ($back_to_top.length) {
				$(window).scroll(function () {
					if ($(this).scrollTop() > 400) {
						$back_to_top.show().removeClass('hidden').addClass('visible');
					} else {
						$back_to_top.removeClass('visible').addClass('hidden');
					}
				});

				$back_to_top.click(function () {
					$('html, body').animate({
						scrollTop: 0
					}, 800);
				});
			}

			if ($social_share_icons.length) {
				$social_share_icons.on('click', '.social-share-link', function (e) {

					e.preventDefault();

					var $this = $(this),
						network_name = $this.data('network-name'),
						share_link = $this.attr('href'),
						new_window_height = 450,
						new_window_width = 550,
						new_window_left = ($(window).width() / 2) - (new_window_width / 2),
						new_window_top = ($(window).height() / 2) - (new_window_height / 2),
						new_window;

					if ('basic_email' === network_name) {
						var email_body,
							email_subject,
							email_mailto,
							new_line = "%0A%0a",
							share_title = encodeURIComponent($this.data('share-title')),
							share_url = encodeURIComponent($this.data('share-url'));

						email_subject = "Subject=" + share_title;
						email_body = "&body=" + new_line + share_url + new_line;
						email_mailto = "mailto:?" + email_subject + email_body;

						window.location.href = email_mailto;
					} else if ('basic_print' === network_name) {
						window.print();
					} else {
						new_window = window.open(share_link, '', 'scrollbars=1, height=' + new_window_height + ', width=' + new_window_width + ', left=' + new_window_left + ', top=' + new_window_top);

						if (window.focus) {
							new_window.focus();
						}
					}
				});
			} // end if ( $social_share_icons.length )

			if (et_is_mobile_device) {
				$('.et_builder_section_video_bg').each(function () {
					var $this_el = $(this);

					$this_el.css('visibility', 'hidden').closest('.et_builder_preload').removeClass('et_builder_preload');
				});

				$('body').addClass('et_mobile_device');

				if (!et_is_ipad) {
					$('body').addClass('et_mobile_device_not_ipad');
				}
			}

			if ($('ul.et_disable_top_tier').length) {
				$('ul.et_disable_top_tier > li > ul').prev('a').attr('href', '#');

				$('ul.et_disable_top_tier > li > ul').prev('a[href="#"]').click(function (e) {
					e.preventDefault();
				});
			}

			et_duplicate_menu($('#et-navigation ul.nav'), $('#et-mobile-navigation nav'), 'et-extra-mobile-menu', 'et_extra_mobile_menu');

			if ($('#top-header #et-info').length) {
				$('#et-navigation #et-extra-mobile-menu').before($('#top-header #et-info').clone());
			}

			if ($('#et-secondary-menu').length > 0) {
				$('#et-navigation #et-extra-mobile-menu').append($('#et-secondary-menu').clone().html());
			}

			$('.show-menu').on('click', function (e) {
				e.preventDefault();

				$(this).children('.show-menu-button').toggleClass('toggled');

				$('#et-mobile-navigation nav').stop().animate({
					height: "toggle",
					opacity: "toggle"
				}, 300);
			});

			$('.et-top-search-primary-menu-item .search-icon').on('click', function () {
				$(this).parent().children('.et-top-search').stop().fadeToggle();
			});

			$('#et-extra-mobile-menu .menu-item-has-children a').on('click', function (e) {
				$(this).parent('li').children('ul').stop().animate({
					height: "toggle",
					opacity: "toggle"
				}, 300);
				$(this).toggleClass('selected', 300);
			});

			var last_mobile_menu_click;
			$('#et-extra-mobile-menu .menu-item-has-children > a').on('click', function (e) {
				var mobile_menu_clicks_distance = 500,
					current_time = new Date().getTime(),
					preventDefault = true;

				// Users click the menu multiple times in short interval is a signal that the default expand/collapse
				// behaviour is not what they want and they intend to go to the link; thus let the link works instead
				if (last_mobile_menu_click + mobile_menu_clicks_distance > current_time) {
					preventDefault = false;
				}

				last_mobile_menu_click = current_time;

				if (preventDefault) {
					e.preventDefault();
				}
			});

			$('#et-trending-button').on('click', function (e) {
				e.preventDefault();
				$(this).toggleClass('toggled');

				$('#et-secondary-menu').stop().animate({
					height: "toggle",
					opacity: "toggle"
				}, 300);
				$(this).toggleClass('selected', 400);
			});

			form_placeholders_init($comment_form);

			if ($et_builder_video_section.length) {
				$et_builder_video_section.find('video').mediaelementplayer({
					pauseOtherPlayers: false,
					success: function (mediaElement, domObject) {
						mediaElement.addEventListener('canplay', function () {
							resize_section_video_bg($(domObject));
							center_video($(domObject));
						});
					}
				});
			}

			function trendingLoop() {
				var et_trending_next_post_eq;

				et_trending_next_post_eq = et_trending_active_post_eq + 1;

				if (et_trending_post_count === et_trending_next_post_eq) {
					et_trending_next_post_eq = 0;
				}

				$et_trending_posts.eq(et_trending_active_post_eq).fadeOut(et_trending_fade_speed, function () {
					$et_trending_posts.eq(et_trending_next_post_eq).fadeIn(et_trending_fade_speed);
					et_trending_active_post_eq = et_trending_next_post_eq;
				});

			}

			if ($et_trending_container.length) {
				var $et_trending_posts = $et_trending_container.children('.et-trending-post'),
					et_trending_post_count = $et_trending_posts.length,
					et_trending_post_duration = 10000,
					et_trending_fade_speed = 300,
					et_trending_active_post_eq = 0;

				$et_trending_posts.not('.et-trending-post:first-child').hide();

				setInterval(function () {
					trendingLoop();
				}, et_trending_post_duration);
			}

			if ($woocommerce_details_accordion.length) {
				$woocommerce_details_accordion.accordion({
					header: '> .group > .header',
					heightStyle: "content",
					collapsible: true,
					active: ($woocommerce_details_accordion.data('desc-tab-active') ? 0 : false),
					activate: function() {
						// WooCommerce page uses builder
						if ($('body').hasClass('et_pb_pagebuilder_layout')) {
							// Some module animation reinit only occur if actual window container is changed
							// Setup flag to force width container change callback even if the window width remains
							window.et_force_width_container_change = true;
							$(window).trigger('resize');
						}
					}
				});
			}

			if ($search_icon.length) {
				$search_icon.click(function () {
					var $this = $(this),
						$form = $this.siblings('.et-top-search'),
						$field = $form.find('.et-search-field');

					$form.toggleClass('search-form-visible');

					if ($form.hasClass('search-form-visible')) {
						$field.select();
					}
				});
			}

			function re_init_new_content($new_content) {
				init_videos();

				if ($new_content.find('.et-slider').length) {
					$new_sliders = $new_content.find('.et-slider');
					$new_sliders.each(function () {
						et_slider_setup($(this));
					});
					et_slider_fix();
				}

				if ($new_content.find('.wp-audio-shortcode, .wp-video-shortcode').length) {
					var settings = {};

					if (typeof _wpmejsSettings !== 'undefined') {
						settings.pluginPath = _wpmejsSettings.pluginPath;
					}

					settings.success = function (mejs) {
						var autoplay = mejs.attributes.autoplay && 'false' !== mejs.attributes.autoplay;
						if ('flash' === mejs.pluginType && autoplay) {
							mejs.addEventListener('canplay', function () {
								mejs.play();
							}, false);
						}
					};
					$new_content.find('.wp-audio-shortcode, .wp-video-shortcode').mediaelementplayer(settings);
				}

				if ($new_content.find('.post-format-map').length) {
					$new_maps = $new_content.find('.post-format-map');
					$new_maps.each(function () {
						et_simple_marker_map($(this));
					});
				}

				// Re-apply Salvattore grid to the new content.
				if ('undefined' !== $new_content.attr('data-columns')) {
					salvattore.registerGrid($new_content[0]);
					salvattore.recreateColumns($new_content[0]);
				}

			}

			function paginated_transition_page($this_paginated, to_page) {
				var $this = $this_paginated,
					$paginated_pages = $this.find('.paginated_page'),
					window_width = $(window).width();

				$this.data('current_page', to_page);

				paginated_update_pagination($this);

				$paginated_pages.removeClass('active').hide();
				$paginated_pages.filter('.paginated_page_' + to_page).addClass('active').fadeIn('fast', function () {

					if ($('body').hasClass('et_fixed_nav') && 980 < window_width) {
						$menu_offset = $('#top-header').outerHeight() + $('#main-header').outerHeight() - 1;
					} else {
						$menu_offset = -1;
					}

					var scroll_to_top_offset = ($this_paginated.offset().top - $menu_offset - et_calclate_admin_bar_height() - 20);

					if ($('html').scrollTop()) {
						$('html').animate({
							scrollTop: scroll_to_top_offset
						}, {
							duration: 500,
						});

						return;
					}

					$('body').animate({
						scrollTop: scroll_to_top_offset
					}, {
						duration: 500,
					});
				});
			}

			function paginated_update_pagination($this_paginated) {
				var current_page = $this_paginated.data('current_page'),
					$pagination = $this_paginated.find('.pagination'),
					total_pages = $pagination.find('li').not('.arrow, .ellipsis').length,
					show_front = false,
					show_back = false;

				$pagination.find('li').removeClass('active');
				$pagination.find('li').not('.arrow, .ellipsis').each(function (i) {

					var $this = $(this);
					i = i + 1;

					if (i === current_page) {
						$this.addClass('active');
					}

					if (total_pages > 5) {
						if (i === 1 || i === total_pages) {
							$this.show();
						} else {
							if (i < 4 && current_page < 4) {
								$this.show();
							} else if (i > (total_pages - 3) && current_page > (total_pages - 3)) {
								$this.show();
							} else if ((i >= current_page - 1) && (i <= current_page + 1)) {
								$this.show();
							} else {
								$this.hide();
							}
						}

					}

				});

				if (total_pages > 5) {
					if (current_page < 4) {
						show_back = true;
						show_front = false;
					} else if (current_page <= (total_pages - 3) && current_page >= 4) {
						show_back = true;
						show_front = true;
					} else if (current_page > (total_pages - 5)) {
						show_front = true;
						show_back = false;
					} else {
						show_front = false;
						show_back = false;
					}

					if (show_front) {
						$pagination.find('.ellipsis.front').show();
					} else {
						$pagination.find('.ellipsis.front').hide();
					}

					if (show_back) {
						$pagination.find('.ellipsis.back').show();
					} else {
						$pagination.find('.ellipsis.back').hide();
					}
				}

				if (current_page > 1) {
					$pagination.find('li.prev').show();
				} else {
					$pagination.find('li.prev').hide();
				}

				if (current_page < total_pages) {
					$pagination.find('li.next').show();
				} else {
					$pagination.find('li.next').hide();
				}
			}

			if ($paginated.length) {

				$paginated.each(function () {
					var $this = $(this),
						$pagination = $this.find('.pagination');

					paginated_update_pagination($this);

					$pagination.on('click', 'a', function (e) {
						var $this = $(this),
							$this_paginated = $this.closest('.paginated'),
							$loader = $this_paginated.find('.loader'),
							current_page = $this_paginated.data('current_page'),
							to_page,
							page_is_loaded,
							posts_per_page = $this_paginated.data('posts_per_page'),
							order = $this_paginated.data('order'),
							orderby = $this_paginated.data('orderby'),
							categories = $this_paginated.data('category_id'),
							show_featured_image = $this_paginated.data('show_featured_image'),
							blog_feed_module_type = $this_paginated.data('blog_feed_module_type'),
							et_column_type = $this_paginated.data('et_column_type'),
							show_author = $this_paginated.data('show_author'),
							show_categories = $this_paginated.data('show_categories'),
							show_date = $this_paginated.data('show_date'),
							show_rating = $this_paginated.data('show_rating'),
							show_more = $this_paginated.data('show_more'),
							show_comments = $this_paginated.data('show_comments'),
							date_format = $this_paginated.data('date_format'),
							content_length = $this_paginated.data('content_length'),
							hover_overlay_icon = $this_paginated.data('hover_overlay_icon'),
							use_tax_query = $this_paginated.data('use_tax_query'),
							tax_query = ('undefined' === typeof (EXTRA_TAX_QUERY) || 1 !== parseInt(use_tax_query)) ? [] : EXTRA_TAX_QUERY;

						e.preventDefault();

						if ($this.hasClass('ellipsis')) {
							return;
						} else if ($this.hasClass('arrow')) {
							if ($this.hasClass('prev')) {
								to_page = current_page - 1;
							} else {
								to_page = current_page + 1;
							}
						} else {
							to_page = $this.data('page');
						}

						page_is_loaded = $this_paginated.find('.paginated_page_' + to_page).length;

						if (page_is_loaded) {
							paginated_transition_page($this_paginated, to_page);
						} else {

							var $to_page_link = $this;
							if ($this.hasClass('arrow')) {
								$to_page_link = $this_paginated.find('.pagination-page-' + to_page);
							}

							$to_page_link_li = $to_page_link.parent();

							$loader.appendTo($to_page_link_li);
							$to_page_link.hide();

							if ($this_paginated.data('getting_more_content')) {
								return;
							}

							$this_paginated.data('getting_more_content', true);

							$.ajax({
								type: "POST",
								url: EXTRA.ajaxurl,
								data: {
									action: 'extra_blog_feed_get_content',
									et_load_builder_modules: '1',
									blog_feed_nonce: EXTRA.blog_feed_nonce,
									to_page: to_page,
									posts_per_page: posts_per_page,
									order: order,
									orderby: orderby,
									categories: categories,
									show_featured_image: show_featured_image,
									blog_feed_module_type: blog_feed_module_type,
									et_column_type: et_column_type,
									show_author: show_author,
									show_categories: show_categories,
									show_date: show_date,
									show_rating: show_rating,
									show_more: show_more,
									show_comments: show_comments,
									date_format: date_format,
									content_length: content_length,
									hover_overlay_icon: hover_overlay_icon,
									use_tax_query: use_tax_query,
									tax_query: tax_query
								},
								success: function (data) {

									if (data) {
										var $new_page = $(data);

										$new_page.appendTo($this_paginated.find('.paginated_content'));

										paginated_transition_page($this_paginated, to_page);

										setTimeout(function () {
											re_init_new_content($new_page);
										}, 500);
									}

									setTimeout(function () {
										$this_paginated.data('getting_more_content', false);
									}, 250);

									// reset loader image and re-show pagination page number
									$loader.appendTo($this_paginated);
									$to_page_link.show();
								}
							});
						}
					});

				}); // end $paginated.each
			} // end if ( $paginated.length )

			function et_simple_marker_map($map_div) {
				var map,
					marker,
					position;

				position = new google.maps.LatLng(parseFloat($map_div.data('lat')), parseFloat($map_div.data('lng')));

				map = new google.maps.Map($map_div[0], {
					zoom: parseInt($map_div.data('zoom')),
					center: position,
					mapTypeId: google.maps.MapTypeId.ROADMAP
				});

				marker = new google.maps.Marker({
					position: position,
					map: map
				});
			}

			if ($post_format_map.length) {
				google.maps.event.addDomListener(window, 'load', function () {
					$post_format_map.each(function () {

						et_simple_marker_map($(this));
					});
				});
			}

			if ($contact_form.length) {
				var $contact_form_loader = $contact_form.find('.loader'),
					$contact_form_message = $contact_form.find('.message'),
					$contact_form_map = $('.contact-map');

				if ($contact_form_map.length) {
					et_simple_marker_map($contact_form_map);
				}

				var update_contact_form_error_messages = debounce(function (errorMap) {
					$contact_form_message.html('');

					for (var error in errorMap) {
						error = errorMap[error];
						$contact_form_message.append('<p class="error" data-element="' + $(error.element).attr('id') + '">' + error.message + '</p>');
					}

					$contact_form_message.slideDown('fast');
				}, 500);

				$contact_form.validate({
					rules: {
						contact_name: {
							required: true,
						},
						contact_email: {
							required: true,
							email: true
						}
					},
					messages: {
						contact_name: EXTRA.contact_error_name_required,
						contact_email: {
							required: EXTRA.contact_error_email_required,
							email: EXTRA.contact_error_email_invalid
						}
					},
					showErrors: function (errorsObj, errorMap) {
						if (errorMap.length) {
							update_contact_form_error_messages(errorMap);
						} else {
							$contact_form_message.slideUp('fast', function () {
								$contact_form_message.html('');
							});
						}
					},
					submitHandler: function () {
						$contact_form_loader.slideDown('fast');

						$contact_form_message.slideUp('fast', function () {
							$contact_form_message.html('');
						});

						$.ajax({
							type: "POST",
							url: EXTRA.ajaxurl,
							data: {
								action: 'extra_contact_form_submit',
								nonce_extra_contact_form: $('#nonce_extra_contact_form').val(),
								contact_name: $('#contact_name').val(),
								contact_email: $('#contact_email').val(),
								contact_subject: $('#contact_subject').val(),
								contact_message: $('#contact_message').val()
							},
							dataType: 'json',
							success: function (data) {
								$contact_form_loader.slideUp('fast');

								if (typeof data.message === 'undefined') {
									data = {
										message: EXTRA.error,
										type: 'error'
									};
								}

								if (data.message.type === 'success') {
									$('#contact_name').val('');
									$('#contact_email').val('');
									$('#contact_subject').val('');
									$('#contact_message').val('');
								}

								$contact_form_message.append('<p class="' + data.type + '">' + data.message + '</p>');
								$contact_form_message.slideDown('fast', function () {
									$('body').animate({
										scrollTop: ($contact_form_message.offset().top - 50)
									}, {
										duration: 200,
										complete: function () {
											setTimeout(function () {
												$contact_form_message.effect("highlight", {
													duration: 1000
												});
											}, 200);
										}
									});

								});
							}
						});
					}
				});
			} /* end if ( $contact_form.length ) */

			function timeline_scroll() {
				admin_bar_height = et_calclate_admin_bar_height();

				var $this = $timeline,
					element_top = $this.offset().top,
					element_bottom = element_top + $this.height(),
					window_top = $(window).scrollTop() + admin_bar_height,
					window_bottom = window_top + $(window).height(),
					$timeline_modules = $timeline.find('.timeline-module'),
					total_timeline_modules = $timeline_modules.length,
					$last_module = $timeline_modules.last(),
					last_module_top = $last_module.offset().top,
					sticky_offset,
					$current_module,
					$next_module = null,
					$prev_module = null,
					fixed_nav_height = window.ET_App.Elements.$body.hasClass('et_fixed_nav') && 'fixed' === window.ET_App.Elements.$main_nav.css('position') && '0' !== window.ET_App.Elements.$main_nav.css('opacity') ? window.ET_App.Elements.$main_nav.innerHeight() : 0,
					menu_offset = window.ET_App.Elements.$body.hasClass('et_fixed_nav') ? fixed_nav_height + 15 : $sticky_header.height() / 2,
					last_item_top = $last_module.find('.posts-list > li').last().offset().top - sticky_header_height,
					menu_move_postion = element_bottom - $timeline_menu.height() - menu_offset;

				if (window_top > element_top - menu_offset) {
					$timeline_menu.css({
						position: 'fixed',
						marginTop: (admin_bar_height + menu_offset) + 'px',
					});

					if (window_top > menu_move_postion) {
						sticky_offset = (menu_move_postion - window_top) + 'px';
						$timeline_menu.css('top', sticky_offset);
					} else {
						$timeline_menu.css('top', 0);
					}
				} else {
					$timeline_menu.css({
						position: 'relative',
						marginTop: 0,
						top: 0
					});
				}

				if (window_top > last_item_top) {
					sticky_offset = (last_item_top - window_top) + 'px';
					$sticky_header.css('top', sticky_offset);
				} else if (window_top > element_top) {
					// setup current module eq
					var find_current_module_eq = debounce(function () {
						$timeline_modules.each(function (index) {
							if ($(this).offset().top > window_top) {
								current_module_eq = index - 1;
								return false;
							}
						});
					}, 200);

					find_current_module_eq();

					if (window_bottom >= (last_module_top - 200)) {
						timeline_load_content();
					}

					$current_module = $timeline_modules.eq(current_module_eq);

					var current_year = $current_module.data('year'),
						year_already_open = !$timeline_menu.find('li.year.year-' + current_year).is(':visible');

					// sets the has for the very first entered module
					if (0 === current_module_eq && !timeline_hash_initialized) {
						timeline_hash_initialized = true;
						timeline_set_location_hash();
					}

					if (!year_already_open) {
						timeline_menu_years_setup();
					}

					// check if there is next module
					if ((current_module_eq + 1) < total_timeline_modules) {
						$next_module = $timeline_modules.eq(current_module_eq + 1);
					}

					// check if there is a previous module
					if (current_module_eq > 0) {
						$prev_module = $timeline_modules.eq(current_module_eq - 1);
					}

					// setup active nav menu item
					var setup_active_menu_item = debounce(function () {
						var current_module_id = $current_module.attr('id'),
							$active_menu_item = $('a[href="#' + current_module_id + '"]');

						if (!$active_menu_item.closest('li').hasClass('active')) {
							$active_menu_item.closest('li').siblings().removeClass('active');
							$active_menu_item.closest('li').addClass('active');

							timeline_set_location_hash();
						}
					}, 500);

					setup_active_menu_item();

					// setup correct sticky header text
					$sticky_header_month.text($current_module.find('.module-title-month').text());
					$sticky_header_year.text($current_module.find('.module-title-year').text());

					if ($current_module.hasClass('collapsed')) {
						$sticky_header.addClass('collapsed');
					} else {
						$sticky_header.removeClass('collapsed');
					}

					// reset sticky header standard styles
					$sticky_header.show().css({
						position: 'fixed',
						marginTop: admin_bar_height + fixed_nav_height + 'px',
						top: 0,
						width: $this.width(),
						zIndex: 100
					});

					var current_module_top = $current_module.offset().top;

					if (null !== $prev_module) {

						if (current_module_top > window_top) {
							sticky_offset = (current_module_top - sticky_header_height - window_top) + 'px';
							$sticky_header.css('top', sticky_offset);

							if ($prev_module.hasClass('collapsed')) {
								$sticky_header.addClass('collapsed');
							} else {
								$sticky_header.removeClass('collapsed');
							}

							$sticky_header_month.text($prev_module.find('.module-title-month').text());
							$sticky_header_year.text($prev_module.find('.module-title-year').text());
						}

						if ((current_module_top - sticky_header_height) > window_top) {
							$sticky_header.css('top', 0);
							current_module_eq = current_module_eq - 1;
						}
					}

					if (null !== $next_module) {
						var next_module_top = $next_module.offset().top;

						if ((next_module_top - sticky_header_height) < window_top) {
							sticky_offset = (next_module_top - sticky_header_height - window_top) + 'px';
							$sticky_header.css('top', sticky_offset);
						}

						if (next_module_top < window_top) {

							if ($next_module.hasClass('collapsed')) {
								$sticky_header.addClass('collapsed');
							} else {
								$sticky_header.removeClass('collapsed');
							}

							$sticky_header_month.text($next_module.find('.module-title-month').text());
							$sticky_header_year.text($next_module.find('.module-title-year').text());

							$sticky_header.css('top', 0);
							current_module_eq = current_module_eq + 1;
						}
					}
				} else {
					$sticky_header.hide();

					if (!$timeline_menu.find('li.active').length) {
						$timeline_menu.find('li.month').first().addClass('active');
					}
				}
			} /* end timeline_scroll() */

			var timeline_set_location_hash = debounce(function () {
				var $timeline_modules = $timeline.find('.timeline-module'),
					$current_module = $timeline_modules.eq(current_module_eq),
					current_module_id = $current_module.attr('id'),
					$active_menu_item = $('a[href="#' + current_module_id + '"]'),
					hash = 'timeline' + et_hash_module_param_seperator + $active_menu_item.attr('href').substring(1);

				et_set_hash(hash);
			}, 500);

			function timeline_menu_years_setup() {
				var $timeline_modules = $timeline.find('.timeline-module'),
					_current_module_eq = current_module_eq || 0,
					$current_module = $timeline_modules.eq(_current_module_eq),
					current_year = $current_module.data('year');

				$timeline_menu.find('li.month').not('.year-' + current_year).slideUp('fast');
				$timeline_menu.find('li.year').not('.year-' + current_year).slideDown('fast');
				$timeline_menu.find('li.month.year-' + current_year).slideDown('fast');
				$timeline_menu.find('li.year.year-' + current_year).slideUp('fast');
			}

			function timeline_menu_scroll($menu_item, $list_target) {
				var $loader = $timeline_menu.find('.content-loader'),
					window_top = $(window).scrollTop() + admin_bar_height,
					direction_buffer = window_top > target_top ? -3 : 3,
					target_top = $list_target.offset().top - admin_bar_height + direction_buffer;

				if ($loader.length) {
					$loader.fadeOut('fast', function () {
						$loader.remove();
					});
				}

				$('html, body').animate({
						scrollTop: (target_top)
					},
					timeline_menu_scroll_duration,
					'swing',
					function () {
						setTimeout(function () {
							$menu_item.closest('li').siblings().removeClass('active');

							if ($menu_item.closest('li').hasClass('year')) {
								var year = $menu_item.data('year'),
									$first_menu_item_of_year = $timeline_menu.find('li.month.year-' + year).first();

								$first_menu_item_of_year.addClass('active');
							} else {
								$menu_item.closest('li').addClass('active');
							}

							timeline_set_location_hash();
							$(window).trigger('scroll');
						}, 500);
					});
			}

			function timeline_is_completed(last_month, last_year) {
				var $timeline_menu_last = $('#timeline-menu li.month:last a'),
					limit_month = $timeline_menu_last.attr('data-month'),
					limit_year = $timeline_menu_last.attr('data-year'),
					last_timestamp = new Date(last_month + '-' + last_year).getTime() / 1000,
					limit_timestamp = new Date(limit_month + '-' + limit_year).getTime() / 1000;

				return last_timestamp < limit_timestamp;
			}

			function timeline_load_content(opts, callback) {
				if (timeline_getting_more_content || timeline_content_complete) {
					return;
				}

				var $last_module = $('#timeline').find('.timeline-module').last(),
					last_month = timeline_last_month || $last_module.data('month'),
					last_year = timeline_last_year || $last_module.data('year');

				$timeline_loader.slideDown('fast');
				timeline_getting_more_content = true;

				// default callback jQuery.noop
				callback = typeof callback === 'function' ? callback : jQuery.noop;

				default_data = {
					action: 'extra_timeline_get_content',
					timeline_nonce: EXTRA.timeline_nonce,
					last_month: last_month,
					last_year: last_year
				};

				data = $.extend(true, default_data, opts);

				$.ajax({
					type: "POST",
					url: EXTRA.ajaxurl,
					data: data,
					success: function (data) {
						$timeline_loader.slideUp('fast');

						if (data) {
							$timeline_modules = $(data);

							if (et_container_width <= et_single_col_breakpoint) {
								$timeline_modules.each(function () {
									$(this).find('.posts-list').hide();
									$(this).addClass('collapsed');
								});
							}

							$timeline_modules.insertAfter($last_module);

							// Reset timeline_last_* variables
							timeline_last_month = false;
							timeline_last_year = false;
						} else {
							// Mark content completed if returned data is empty and last loaded content has passed the limit month
							if (data === '' && timeline_is_completed(last_month, last_year)) {
								timeline_content_complete = true;

								// Reset timeline_last_* variables
								timeline_last_month = false;
								timeline_last_year = false;
							} else {
								var timeline_last_month_timestamp = new Date(last_month + '-' + last_year).getTime(),
									six_months_in_milisecond = 60 * 60 * 24 * 30 * 6 * 1000,
									timeline_six_month_ago = new Date(timeline_last_month_timestamp - six_months_in_milisecond),
									month_names = [
										'january',
										'february',
										'march',
										'april',
										'may',
										'june',
										'july',
										'august',
										'september',
										'october',
										'november',
										'december'
									],
									month_six_month_ago = month_names[timeline_six_month_ago.getMonth()],
									year_six_month_ago = timeline_six_month_ago.getFullYear();

								// Set timeline_last_* variables which will be used instead of DOM-based last year / month
								timeline_last_month = month_six_month_ago;
								timeline_last_year = year_six_month_ago;

								// immediately load next set of content
								timeline_load_content();
							}
						}

						setTimeout(function () {
							timeline_getting_more_content = false;
						}, 250);

						callback();
					}
				});
			}

			if ($timeline.length) {
				var current_module_eq = null,
					admin_bar_height = 0,
					$sticky_header = $('#timeline-sticky-header'),
					$timeline_loader = $timeline.find('.loader'),
					$sticky_header_month = $sticky_header.find('.module-title-month'),
					$sticky_header_year = $sticky_header.find('.module-title-year'),
					$first_timeline_module = $('.timeline-module:visible').first(),
					first_module_border_top_width_px = $first_timeline_module.css('border-top-width'),
					first_module_border_top_width = first_module_border_top_width_px.substr(0, first_module_border_top_width_px.length - 2),
					first_module_head_height = $first_timeline_module.find('.module-head').outerHeight(),
					sticky_header_height = parseInt(first_module_border_top_width) + parseInt(first_module_head_height),
					timeline_getting_more_content = false,
					timeline_content_complete = false,
					$timeline_menu = $('#timeline-menu'),
					timeline_menu_scroll_duration = 1000,
					timeline_hash_initialized = false,
					timeline_last_month = false,
					timeline_last_year = false;

				$(window).on('scroll', timeline_scroll).trigger('scroll');

				$timeline.on('et_hashchange', function (event) {
					if (timeline_hash_initialized) {
						return;
					}

					var params = event.params,
						month_year = params[0],
						$menu_item = $timeline_menu.find('a[href="#' + month_year + '"]'),
						$target = $('#' + month_year);

					timeline_hash_initialized = true;

					if ($target.length) {
						timeline_menu_scroll($menu_item, $target);
					} else {
						var month_year_array = month_year.split('_'),
							month = month_year_array[0],
							year = month_year_array[1];

						options = {
							'through_month': month,
							'through_year': year
						};

						timeline_load_content(options, function () {
							setTimeout(function () {
								$target = $('#' + month_year);
								timeline_menu_scroll($menu_item, $target);
							}, 300);
						});
					}
				});

				// collapse if mobile
				if (et_container_width <= et_single_col_breakpoint) {
					$timeline.find('.timeline-module').each(function () {
						$(this).find('.posts-list').slideUp('fast');
						$(this).addClass('collapsed');
					});
				}

				$sticky_header.addClass('container-width-change-notify').on('containerWidthChanged', function () {
					$sticky_header.css({
						width: $timeline.width()
					});
				});

				$sticky_header.on('click', function () {
					var $current_module = $timeline.find('.timeline-module').eq(current_module_eq),
						$module_head = $current_module.find('.module-head');

					$module_head.click();
				});

				$timeline.on('click', '.module-head', function () {
					var $this = $(this),
						$module = $this.closest('.timeline-module'),
						$posts_list = $module.find('.posts-list');

					if ($module.hasClass('collapsed')) {
						$posts_list.slideDown('fast');
						$module.removeClass('collapsed');
					} else {
						$posts_list.slideUp('fast');
						$module.addClass('collapsed');
					}
				});

				$timeline_menu.on('click', 'a', function (e) {
					var $this = $(this),
						year = $this.data('year'),
						$target = $($this.attr('href'));

					e.preventDefault();

					if (!$target.length) {
						$target = $timeline.find('.timeline-module.year-' + year).first();
					}

					if ($target.length) {
						timeline_menu_scroll($this, $target);
					} else {
						$loading_img = $timeline_loader.find('img').clone();
						$loading_img.addClass('content-loader');
						$this.append($loading_img);
						options = {
							'through_year': year
						};
						timeline_load_content(options, function () {
							$target = $timeline.find('.timeline-module.year-' + year).first();
							timeline_menu_scroll($this, $target);
						});
					}
				});

				// setup first time
				timeline_menu_years_setup();

			} /* end if ( $timeline.length ) */

			$comment_form.submit(function () {
				remove_placeholder_text($comment_form);
			});

			if ($rating_stars.length) {
				$rating_stars.raty({
					half: true,
					width: false,
					space: false,
					size: 5,
					path: EXTRA.images_uri,
					starOn: 'star-full.svg',
					starOff: 'star-full.svg',
					starHalf: 'star-half-full.svg',
					click: function (rating) {
						$.ajax({
							type: "POST",
							url: EXTRA.ajaxurl,
							dataType: "json",
							data: {
								action: 'extra_new_rating',
								extra_rating_nonce: EXTRA.rating_nonce,
								extra_post_id: $('#post_id').val(),
								extra_rating: rating
							},
							success: function () {
								$('#rate-title').text(EXTRA.your_rating);
								$('#rating-stars').raty('readOnly', true);
								$('#rating-stars').attr({
									'title': ''
								});
								$('#rating-stars').find('img').each(function () {
									$(this).attr({
										'title': '',
										'alt': ''
									});
								});
							}
						});

					}
				});
			} /* end if ( $rating_stars.length ) */

			if ($post_module.length) {
				$post_module.find('.title-thumb-hover').each(function () {
					var $this = $(this),
						$thumb = $this.find('.post-thumbnail > img'),
						$title = $this.find('.post-content > h3 > a'),
						title_hover_color = $title.data('hover-color');

					$title.hover(function () {
						$thumb.addClass('hover');
						$title.css({
							'color': title_hover_color
						});
					}, function () {
						$thumb.removeClass('hover');
						$title.css('color', '');
					});
				});
			}

			function tabbed_post_module_tab_change($tab) {
				var $module = $tab.parents('.tabbed-post-module'),
					tab_id = $tab.data('tab-id'),
					$tab_content = $module.find('.tab-content-' + tab_id),
					term_color = $tab.data('term-color');

				$tab.css({
					color: term_color
				});

				$module.css({
					borderTopColor: term_color
				});

				$tab_content.siblings().hide();

				$tab_content.stop().css({
					'display': 'flex'
				}).hide().fadeIn(300, 'swing');

				$tab.addClass('active').siblings().removeClass('active').css({
					color: ''
				});
			}

			if ($tabbed_post_module.length) {
				$tabbed_post_module.each(function () {

					// Set first active colors
					var $this = $(this),
						$tabs_container = $this.find('.tabs'),
						$tabs = $tabs_container.find('li'),
						tabs_count = $tabs.length,
						term_color = $tabs.first().data('term-color'),
						$arrow = $tabs_container.find('.arrow');

					$tabs.first().addClass('active').css({
						color: term_color
					});

					$this.css({
						borderTopColor: term_color
					});

					$tabs.hover(
						function () {
							$(this).css({
								color: $(this).data('term-color')
							});
						},
						function () {
							if (!$(this).hasClass('active')) {
								$(this).css({
									color: ''
								});
							}
						}
					);

					$tabs.click(function (e) {
						var $tab = $(this),
							$ripple_div = $('<div class="ripple" />'),
							ripple_size = 60,
							ripple_offset = $tab.offset(),
							ripple_y = e.pageY - ripple_offset.top,
							ripple_x = e.pageX - ripple_offset.left;

						if (!$(this).hasClass('active')) {
							tabbed_post_module_tab_change($tab);

							$ripple_div.css({
								top: ripple_y - (ripple_size / 2),
								left: ripple_x - (ripple_size / 2)
							}).appendTo($tab);
						}

						// Implement slider fix, in case the first post has gallery format
						et_slider_fix();

						window.setTimeout(function () {
							$ripple_div.remove();
						}, 900);
					});

					$arrow.on('click', function (e) {
						var $this = $(this),
							$active_tab = $tabs.filter('.active'),
							active_tab_index = $active_tab.index(),
							to_tab_index,
							$to_tab;

						if ($this.hasClass('prev')) {
							if ((active_tab_index) <= 0) {
								to_tab_index = tabs_count - 1;
							} else {
								to_tab_index = active_tab_index - 1;
							}
						} else {
							if ((active_tab_index + 1) >= tabs_count) {
								to_tab_index = 0;
							} else {
								to_tab_index = active_tab_index + 1;
							}
						}

						$to_tab = $tabs.eq(to_tab_index);

						tabbed_post_module_tab_change($to_tab);

						// Implement slider fix, in case the first post has gallery format
						et_slider_fix();
					});

				});
			} /* end if ( $tabbed_post_module.length ) */

			function et_slider_setup($the_slider) {
				$the_slider.each(function () {
					var slideshow = $(this).data('autoplay');

					$(this).et_pb_simple_slider({
						slide: '.carousel-item', // slide class
						arrows: '.et-pb-slider-arrows', // arrows container class
						prev_arrow: '.et-pb-arrow-prev', // left arrow class
						next_arrow: '.et-pb-arrow-next', // right arrow class
						control_active_class: 'et-pb-active-control', // active control class name
						fade_speed: 500,
						use_arrows: true,
						use_controls: false,
						slideshow: typeof slideshow !== 'undefined',
						slideshow_speed: $.isNumeric(slideshow) ? slideshow * 1000 : 7000,
						show_progress_bar: false,
						tabs_animation: false,
						use_carousel: false
					});
				});
			}

			function et_slider_fix() {
				$et_slider.each(function () {
					var $slider = $(this);

					$slider.each(function () {
						var $the_slider = $(this),
							$the_slide = $the_slider.find('.carousel-item'),
							slide_max_height = 0;

						// Remove transition class to prevent unwanted transition when element reappearing
						$the_slider.removeClass('et_slide_transition_to_previous et_slide_transition_to_next');

						$the_slider.imagesLoaded(function () {
							$the_slider.removeAttr('style');

							$the_slide.each(function () {
								var slide_height = $(this).height();

								if (slide_max_height === 0 || slide_max_height > slide_height) {
									slide_max_height = slide_height;
								}
							});

							$the_slider.css({
								'maxHeight': slide_max_height
							});
						});
					});
				});
			}

			if ($et_slider.length) {
				$et_slider.each(function () {
					et_slider_setup($(this));
				});

				et_slider_fix();

				window.addEventListener('resize', et_slider_fix);
			}

			function et_featured_posts_slider_fix() {
				$featured_posts_slider_module.each(function () {
					var $featured_posts_slider = $(this),
						slider_width = $featured_posts_slider.width(),
						slider_height = (slider_width / 15.3) * 9;

					$featured_posts_slider.imagesLoaded(function () {
						$featured_posts_slider.each(function () {
							var $the_slider = $(this),
								$the_slide = $the_slider.find('.carousel-item'),
								$post_content_box = $the_slider.find('.post-content-box'),
								$nav_arrow = $the_slider.find('.et-pb-slider-arrows a'),
								is_relative_caption = $post_content_box.css('position') === 'relative' ? true : false,
								max_caption_height = 0,
								the_slider_height,
								nav_arrow_position_top;

							$the_slide.css({
								'height': ''
							});
							$nav_arrow.removeAttr('style');
							$post_content_box.removeAttr('style');

							if (is_relative_caption) {
								$the_slide.each(function () {
									var post_content_box_height = $(this).find('.post-content-box').height();
									if (post_content_box_height > max_caption_height) {
										max_caption_height = post_content_box_height;
									}
								});

								$post_content_box.css({
									'minHeight': max_caption_height,
									'marginTop': slider_height
								});

								the_slider_height = $the_slider.height();
								nav_arrow_position_top = ((the_slider_height - max_caption_height) / 2) - 20;
								$nav_arrow.css({
									'top': nav_arrow_position_top,
									'marginTop': 0
								});
							} else {
								$the_slide.height(slider_height);
							}
						});
					});
				});
			}

			if ($featured_posts_slider_module.length) {
				$featured_posts_slider_module.each(function () {
					var slideshow = $(this).data('autoplay');

					$(this).et_pb_simple_slider({
						slide: '.et_pb_slide', // slide class
						arrows: '.et-pb-slider-arrows', // arrows container class
						prev_arrow: '.et-pb-arrow-prev', // left arrow class
						next_arrow: '.et-pb-arrow-next', // right arrow class
						controls: '.et-pb-controllers a', // control selector
						carousel_controls: '.et_pb_carousel_item', // carousel control selector
						control_active_class: 'et-pb-active-control', // active control class name
						fade_speed: 500,
						use_arrows: true,
						use_controls: true,
						controls_class: 'et-pb-controllers',
						slideshow: typeof slideshow !== 'undefined',
						slideshow_speed: $.isNumeric(slideshow) ? slideshow * 1000 : 7000,
						show_progress_bar: false,
						tabs_animation: false,
						use_carousel: false
					});
				});

				et_featured_posts_slider_fix();

				window.addEventListener('resize', et_featured_posts_slider_fix);
			} /* end if ( $featured_posts_slider_module.length ) */

			function et_posts_carousel_init() {
				$posts_carousel_module.each(function () {
					var $the_carousel = $(this);
					var slideshow = $the_carousel.data('autoplay');
					var $carousel_items = $the_carousel.find('.carousel-items');
					var carousel_width = $carousel_items.width();
					var $carousel_group;
					var $carousel_item = $carousel_items.find('.carousel-item');
					var carousel_item_widths_array = $carousel_item.map(function() {
						return $(this).width();
					}).get();
					var carousel_item_width = Math.max.apply(null, carousel_item_widths_array);
					var carousel_item_thumb_widths_array = $carousel_item.find('.post-thumbnail').map(function() {
						return $(this).width();
					}).get();
					var carousel_item_thumb_width = Math.max.apply(null, carousel_item_thumb_widths_array);
					var carousel_item_thumb_height = (carousel_item_thumb_width / 15) * 9;
					var carousel_column = Math.round(carousel_width / carousel_item_width);
					var index = 0;
					var min_height = 0;
					var $current_carousel_group = $the_carousel.find('.carousel-group');
					var skip_group_height = false;
					var current_carousel_column;

					// Setup max-height for post thumbnail
					$carousel_item.find('.post-thumbnail').css({
						'maxHeight': carousel_item_thumb_height
					});

					// Remove existing column
					if ($current_carousel_group.length) {
						current_carousel_column = $carousel_items.find('.carousel-group:first .carousel-item').size();

						if (current_carousel_column === carousel_column) {
							fix_carousel_arrow_position();
							return;
						}

						$current_carousel_group.each(function () {
							$(this).find('.carousel-item').each(function () {
								$(this).appendTo($carousel_items);
							});
						}).remove();

						$the_carousel.find('.et-pb-slider-arrows').remove();
					}

					// If the column isn't valid, assume its the largest possible columns
					// Also, skip min height correction since it'll be invalid
					if (carousel_column > 4 || carousel_column < 1) {
						carousel_column = 4;
						skip_group_height = true;
					}

					// Group items
					$carousel_items.find('.carousel-item').each(function () {
						var $the_carousel_item = $(this);

						// Reset index
						if (carousel_column === index) {
							index = 0;
						}

						// Append new group
						if (index === 0) {
							$('<div />', {
								class: 'carousel-group et_pb_slide'
							}).appendTo($carousel_items);
						}

						// Append current item to last group
						$the_carousel_item.appendTo($carousel_items.find('.carousel-group:last'));

						index++;
					});

					$carousel_group = $the_carousel.find('.carousel-group');

					// Calculate group index
					if (skip_group_height === false) {
						$carousel_group.each(function () {
							var $the_carousel_group = $(this),
								carousel_group_height = $the_carousel_group.height();

							if (carousel_group_height > min_height) {
								min_height = carousel_group_height;
								$carousel_group.css({
									'min-height': min_height
								});
							}
						});
					}

					if ($carousel_items.data('et_pb_simple_slider')) {
						$carousel_items.data('et_pb_simple_slider').et_slider_destroy();
					}

					$carousel_items.et_pb_simple_slider({
						slide: '.et_pb_slide', // slide class
						arrows: '.et-pb-slider-arrows', // arrows container class
						prev_arrow: '.et-pb-arrow-prev', // left arrow class
						next_arrow: '.et-pb-arrow-next', // right arrow class
						controls: '.et-pb-controllers a', // control selector
						carousel_controls: '.et_pb_carousel_item', // carousel control selector
						control_active_class: 'et-pb-active-control', // active control class name
						fade_speed: 500,
						use_arrows: true,
						use_controls: false,
						slideshow: typeof slideshow !== 'undefined',
						slideshow_speed: $.isNumeric(slideshow) ? slideshow * 1000 : 7000,
						show_progress_bar: false,
						tabs_animation: false,
						use_carousel: false
					});

					// Remove loading state
					if ($the_carousel.hasClass('loading')) {
						$the_carousel.removeClass('loading');
						$carousel_item.removeClass('carousel-item-hide-on-load carousel-item-hide-on-load-medium carousel-item-hide-on-load-small');
					}

					// Never let carousel-group has empty slot
					$the_carousel.on('simple_slider_before_move_to', function (trigger, param) {
						var $carousel = $(this),
							$next_slide = $(param.next_slide),
							active_slide_item_count = $carousel.find('.et-pb-active-slide .carousel-item').size(),
							next_slide_item_count = $next_slide.find('.carousel-item').size(),
							remaining_item_count = active_slide_item_count - next_slide_item_count;

						if (remaining_item_count > 0) {
							var is_next = param.direction === 'next',
								carousel_item_count = $the_carousel.find('.carousel-group').size(),
								next_slide_index = $next_slide.index() + 1,
								correction_slide_index,
								$correction_slide,
								$correction_slide_item;

							if (is_next) {
								if ((next_slide_index + 1) > carousel_item_count) {
									correction_slide_index = 1;
								} else {
									correction_slide_index = next_slide_index + 1;
								}
							} else {
								if ((next_slide_index - 1) < 1) {
									correction_slide_index = carousel_item_count;
								} else {
									correction_slide_index = next_slide_index - 1;
								}
							}

							$correction_slide = $carousel.find('.carousel-group:nth-child(' + correction_slide_index + ')');

							$the_carousel.addClass('et-pb-slide-is-transitioning');

							if (is_next) {
								$correction_slide_item = $correction_slide.find('.carousel-item:lt(' + (remaining_item_count) + ')');

								$next_slide.append($correction_slide_item.clone());
							} else {
								$correction_slide_item = $correction_slide.find('.carousel-item:gt(' + (active_slide_item_count - remaining_item_count - 1) + ')');

								$next_slide.prepend($correction_slide_item.clone());
							}

							$the_carousel.one('simple_slider_after_move_to', function () {
								$correction_slide_item.remove();
								$the_carousel.removeClass('et-pb-slide-is-transitioning');
							});
						}
					});

					function fix_carousel_arrow_position() {
						// Fix arrow position
						var $updated_carousel_group = $the_carousel.find('.carousel-group'),
							$updated_carousel_group_thumb = $updated_carousel_group.find('.post-thumbnail'),
							updated_carousel_group_thumb_height = $updated_carousel_group_thumb.height(),
							carousel_arrow_constant = parseInt($updated_carousel_group_thumb.css('marginLeft')) === 0 ? 5 : 25,
							carousel_arrow_top = (updated_carousel_group_thumb_height / 2) + carousel_arrow_constant;

						$the_carousel.find('.et-pb-slider-arrows a').css({
							'top': carousel_arrow_top
						});
					}

					$the_carousel.imagesLoaded(function () {
						fix_carousel_arrow_position();
					});
				});
			}

			if ($posts_carousel_module.length) {
				et_posts_carousel_init();

				var resizeTimer;
				window.addEventListener('resize', function() {
					clearTimeout(resizeTimer);
					resizeTimer = setTimeout(function() {
						et_posts_carousel_init();
					}, 100);
				});
			} /* end if ( $posts_carousel_module.length ) */

			if ($filterable_portfolio.length) {

				$filterable_portfolio.each(function () {

					var $portfolio_list = $(this).find('.filterable_portfolio_list'),
						$portfolio_filter = $(this).find('.filterable_portfolio_filter');

					$portfolio_filter.find('li:nth-child(2) a').addClass('current');

					$portfolio_filter.find('li:first-child').on('click', function () {
						$portfolio_filter.toggleClass('opened');
					});

					$portfolio_filter.on('click', 'a', function (e) {
						$portfolio_filter.find('a').removeClass('current');
						$(this).addClass('current');
						e.preventDefault();

						var filter_cat = $(this).attr("rel");

						if (typeof filter_cat !== 'undefined' && filter_cat.length > 0) {
							$filter_items = $portfolio_list.find('.project_category_' + filter_cat);
							$hidden_items = $portfolio_list.find('.project').not($filter_items);

							$filter_items.show();
							$hidden_items.hide();
						} else {
							$portfolio_list.find('.project').show();
						}

					});
				});
			} /* end if ( $filterable_portfolio.length ) */

			function et_adjust_navigation_offset() {
				// Resetting all sub menu inline styling
				$('.nav > li > ul').css({
					'left': '',
					'right': ''
				});

				// Anticipating very short first level menu item with sub-menu
				$('.nav').each(function () {
					var $nav = $(this),
						$first_level_menu_item = $nav.children('ul > li'),
						window_width = $(window).width();

					if ($first_level_menu_item.length) {
						$first_level_menu_item.each(function () {
							var $menu = $(this),
								$submenu = $menu.children('ul'),
								submenu_width = $submenu.width(),
								submenu_offset = $submenu.offset(),
								is_mega_menu = $menu.hasClass('mega-menu');

							if (!is_mega_menu && $submenu.length && window_width < (submenu_offset.left + submenu_width)) {
								$submenu.css({
									'left': 'auto',
									'right': -20
								});
							}
						});
					}
				});
			} /* end et_adjust_navigation_offset() */
			et_adjust_navigation_offset();
			window.addEventListener('resize', et_adjust_navigation_offset);

			if ($widget_et_recent_videos.length) {
				$widget_et_recent_videos.each(function () {
					var $recent_videos_widget = $(this),
						$widget_video_wrapper = $recent_videos_widget.find('.widget_video_wrapper'),
						widget_video_width = $widget_video_wrapper.width(),
						widget_video_rational_min_height = (widget_video_width / 16) * 9, // embedded iframes comes with height properties. Due to stretched 100% width, given height properties are often not ideal
						$new_widget_video_div = $('<div />', {
							class: 'widget_video fadeIn'
						}),
						$new_video = $recent_videos_widget.find('.widget-video-item:first').clone().html().trim(),
						$empty_video = $recent_videos_widget.find('.widget-video-item-empty').clone().html().trim(),
						$new_widget_video = $new_video !== '' ? $new_widget_video_div.html($new_video) : $new_widget_video_div.html($empty_video),
						is_embedded_video;

					// Prepend widget video content
					$widget_video_wrapper.prepend($new_widget_video);

					// Check wheter current video embeds video or not
					is_embedded_video = $widget_video_wrapper.find('iframe').length;

					// Adjust embedded video height, preventing cropped video
					if (is_embedded_video) {
						$widget_video_wrapper.find('.widget_video.fadeIn').find('iframe').css({
							'width': widget_video_width,
							'height': widget_video_rational_min_height
						});
					}

					// Adjust no video found height
					if ($new_video === '') {
						$widget_video_wrapper.find('.no-video-title').css({
							'padding': (widget_video_rational_min_height / 2) + 'px 0'
						});
					}

					// Set first video item as active
					$recent_videos_widget.find('li:first a').addClass('active');

					// Clicking other video title
					$recent_videos_widget.on('click', '.title', function (event) {
						event.preventDefault();

						var $title = $(this),
							video_id = $title.attr('data-video-id'),
							widget_video_width = $widget_video_wrapper.width(), // redeclare this, anticipating window's width has been changed since page load
							widget_video_rational_min_height = (widget_video_width / 16) * 9,
							$new_widget_video_div = $('<div />', {
								class: 'widget_video fadeIn'
							}),
							$new_video = $recent_videos_widget.find('.widget-video-item-' + video_id).clone().html().trim(),
							$empty_video = $recent_videos_widget.find('.widget-video-item-empty').clone().html().trim(),
							$new_widget_video = $new_video !== '' ? $new_widget_video_div.html($new_video) : $new_widget_video_div.html($empty_video);

						// If the clicked item is active, bail further action to prevent unwanted glitch
						if ($title.hasClass('active')) {
							return;
						}

						// Remove active state from any title
						$recent_videos_widget.find('.widget_list a').removeClass('active');

						// Mark clicked title as active
						$title.addClass('active');

						// Fade existing video
						$widget_video_wrapper.find('.widget_video').removeClass('fadeIn').addClass('fadeOut');

						// Check wheter current video is emebedded video or not
						is_embedded_video = $new_widget_video.find('iframe').length;

						// Adjust embedded video height, preventing cropped video
						if (is_embedded_video) {
							$new_widget_video.find('iframe').css({
								'width': widget_video_width,
								'height': widget_video_rational_min_height
							});
						}

						// Prepend new video
						$widget_video_wrapper.prepend($new_widget_video);

						// Adjust no video found height
						if ($new_video === '') {
							$widget_video_wrapper.find('.no-video-title').css({
								'padding': (widget_video_rational_min_height / 2) + 'px 0'
							});
						}

						// Wait a reasonable amount of time to get correct calculation...
						setTimeout(function () {
							// Remove previous video
							$widget_video_wrapper.find('.fadeOut').remove();

							// Update widget height
							$widget_video_wrapper.animate({
								'height': $widget_video_wrapper.find('.widget_video.fadeIn').children().outerHeight()
							});
						}, 500);
					});

					// Click to play
					$recent_videos_widget.on('click', '.video-overlay', function (event) {
						event.preventDefault();

						et_play_overlayed_video($(this), '.widget_video');
					});
				});

				// Resizing mechanism
				var video_widget_resizer = debounce(function () {
					$widget_et_recent_videos.each(function () {
						var $recent_videos_widget = $(this),
							$widget_video_wrapper = $recent_videos_widget.find('.widget_video_wrapper'),
							$active_video = $widget_video_wrapper.find('.widget_video.fadeIn'),
							active_video_width = $active_video.width(),
							$active_video_content = $active_video.children(),
							$active_video_no_title = $active_video.find('.no-video-title'),
							is_no_video = $active_video_no_title.length ? true : false,
							is_embedded_video = $active_video.find('iframe').length,
							widget_video_rational_min_height = (active_video_width / 16) * 9,
							widget_video_height = is_embedded_video ? widget_video_rational_min_height : $active_video_content.outerHeight();

						// Adjust no video text's padding
						if (is_no_video) {
							$active_video_no_title.css({
								'padding': (widget_video_rational_min_height / 2) + 'px 0'
							});

							// Widget video height might be changed after the title is adjusted
							widget_video_height = $active_video.outerHeight();
						}

						// Basic resize for all: adjust the video wrapper's fixed height
						$widget_video_wrapper.css({
							'height': widget_video_height
						});

						// Fixing embedded video which cannot fix its dimension automatically
						if (is_embedded_video) {
							$active_video.find('iframe').css({
								'width': active_video_width,
								'height': widget_video_height
							});
						}
					});
				});

				window.addEventListener('resize', video_widget_resizer);
			} /* end if ($widget_et_recent_videos.length) */

			/* Add video overlay clicking mechanism */
			$('#content-area').on('click', '.hentry .video-overlay', function (event) {
				event.preventDefault();

				et_play_overlayed_video($(this), '.video-format');
			});

		}); /* end $(document).ready */

		function et_play_overlayed_video($overlay, wrapper_class) {
			var $wrapper = $overlay.parent(wrapper_class),
				$video_iframe = $wrapper.find('iframe'),
				is_embedded = $video_iframe.length ? true : false,
				video_iframe_src,
				video_iframe_src_splitted,
				video_iframe_src_autoplay;

			if (is_embedded) {
				// Add autoplay parameter to automatically play embedded content when overlay is clicked
				video_iframe_src = $video_iframe.attr('src');
				video_iframe_src_splitted = video_iframe_src.split("?");

				if (video_iframe_src.indexOf('autoplay=') !== -1) {
					return;
				}

				if (typeof video_iframe_src_splitted[1] !== 'undefined') {
					video_iframe_src_autoplay = video_iframe_src_splitted[0] + "?autoplay=1&amp;" + video_iframe_src_splitted[1];
				} else {
					video_iframe_src_autoplay = video_iframe_src_splitted[0] + "?autoplay=1";
				}

				$video_iframe.attr({
					'src': video_iframe_src_autoplay
				});
			} else {
				$wrapper.find('video').get(0).play();
			}

			$overlay.fadeTo(500, 0, function () {
				$overlay.hide();
			});
		}

		function et_mobile_body_class() {
			if (et_container_width <= et_single_col_breakpoint) {
				$('body').addClass('et_single_col');
			} else {
				$('body').removeClass('et_single_col');
			}
		}
		et_mobile_body_class();

		function et_fix_video_wmode(video_wrapper) {
			$(video_wrapper).each(function () {
				if ($(this).find('iframe').length) {
					var $this_el = $(this).find('iframe'),
						src_attr = $this_el.attr('src'),
						wmode_character = src_attr.indexOf('?') === -1 ? '?' : '&amp;',
						this_src = src_attr + wmode_character + 'wmode=opaque';

					$this_el.attr('src', this_src);
				}
			});
		}

		function init_videos() {
			if ($.fn.fitVids) {
				$('#main-content').fitVids({
					customSelector: "iframe[src^='http://www.hulu.com'], iframe[src^='http://www.dailymotion.com'], iframe[src^='http://www.funnyordie.com'], iframe[src^='https://embed-ssl.ted.com'], iframe[src^='http://embed.revision3.com'], iframe[src^='https://flickr.com'], iframe[src^='http://blip.tv'], iframe[src^='http://www.collegehumor.com'], iframe[src^='https://cloudup.com']"
				});
			}

			et_fix_video_wmode('.fluid-width-video-wrapper');
		}

		$(window).ready(function () {
			init_videos();
		});

		$(window).resize(function () {

			resize_section_video_bg();
			center_video();

			var current_container_width = $et_container.width();

			var containerWidthChanged = et_container_width !== current_container_width;

			if (containerWidthChanged) {
				var et_container_width_change_direction = et_container_width > current_container_width ? 'down' : 'up';

				et_mobile_body_class();

				et_container_width = current_container_width;
				$('.container-width-change-notify').trigger('containerWidthChanged', {
					direction: et_container_width_change_direction
				});
			}
		}); /* end $(window).resize */
	}
})(jQuery);
