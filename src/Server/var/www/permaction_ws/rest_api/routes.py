#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from flask import request, redirect, jsonify
from flask_login import login_required, current_user
from flask import current_app as app
from .models import db, Element, Category, BinaryInteraction

from parameters import *
from .utils import *


@app.route(root_route)
@login_required
def root():
    global permaction_uri
    return redirect(permaction_uri)


@app.route(physical_elements_route)
@login_required
def get_physical_elements():
    global physical_elements_tag
    physical_categories = Category.query.filter_by(id_locale = current_user.id_locale, physical_category = True).all()
    physical_category_ids = [category.id for category in physical_categories]
    physical_category_names = [category.name for category in physical_categories]
    physical_elements = Element.query.filter_by(id_locale = current_user.id_locale).all()
    physical_elements_dict = {physical_elements_tag: []}
    for element in physical_elements:
        if (element.name not in physical_category_names and element.category_id in physical_category_ids):
            category = physical_categories[physical_category_ids.index(element.category_id)]
            physical_elements_dict[physical_elements_tag] += [element.to_dict(category)]
    return jsonify(physical_elements_dict)


@app.route(binary_interactions_route)
@login_required
def binary_interactions():
	binary_interactions = BinaryInteraction.query.filter_by(id_locale = current_user.id_locale).all()
	return jsonify({binary_interactions_tag: [binary_interaction.serialize for binary_interaction in binary_interactions]})


@app.route(placement_request_route, methods = [POST_method])
@login_required
def placement_request():
    # First of all, we have to check for the request consistency
    user_placement_request = validate_placement_request(request.get_json())
    # The request is valid, let's add it to the database
    db.session.add(user_placement_request)
    db.session.commit()
    # TODO Add check for payment/tokens
    # Start AI and pick best result to give back to the user
    best_result = start_placement_request(user_placement_request)
    # TODO Save result to database
    # Return best found result
    return jsonify(best_result)

