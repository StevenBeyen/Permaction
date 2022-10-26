#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from flask import current_app as app

from flask import request, abort, make_response, jsonify
from flask_login import login_required, logout_user, login_user
from datetime import datetime as dt
from .models import db, User
from . import login_manager

from parameters import *

@login_manager.user_loader
def load_user(user_id):
    """Check if user is logged-in on every page load."""
    if user_id is not None:
        return User.query.get(user_id)
    return None

@login_manager.unauthorized_handler
def unauthorized():
    """Abort since user is not logged in."""
    abort(401)

@app.route(login_route, methods = [POST_method])
def login():
    global username_tag, password_tag, id_locale_tag
    username = request.json.get(username_tag)
    password = request.json.get(password_tag)
    user = User.query.filter_by(username = username).first()
    if (user is None or not user.verify_password(password)):
        abort(403) # non existing username or wrong password
    login_user(user)
    return jsonify({id_locale_tag: user.id_locale})

@app.route(signup_route, methods = [POST_method])
def signup():
    global username_tag, password_tag, id_locale_tag, email_tag, private_tag, professional_tag, user_created_message
    id_locale = request.json.get(id_locale_tag)
    username = request.json.get(username_tag)
    password = request.json.get(password_tag)
    email = request.json.get(email_tag)
    private = request.json.get(private_tag)
    professional = request.json.get(professional_tag)
    if (id_locale is None or username is None or password is None or email is None or private is None or professional is None):
        abort(400) # missing arguments
    if (User.query.filter_by(username = username).first() is not None):
        abort(403, username_tag) # existing username
    if (User.query.filter_by(email = email).first() is not None):
        abort(403, email_tag) # existing email
    user = User(id_locale = id_locale, username = username, email = email, private = private, professional = professional, created = dt.now())
    user.hash_password(password)
    db.session.add(user)
    db.session.commit()
    login_user(user)
    return make_response(user_created_message.format(user), 201)
    
@app.route(logout_route)
@login_required
def logout():
    global logout_message
    logout_user()
    return make_response(logout_message, 200)

