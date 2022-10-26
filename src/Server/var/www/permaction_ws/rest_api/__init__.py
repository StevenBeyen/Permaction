#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from flask import Flask, abort
from flask_sqlalchemy import SQLAlchemy
from flask_login import LoginManager
from flask_paranoid import Paranoid
from flask_cors import CORS

db = SQLAlchemy()
login_manager = LoginManager()


def create_app():
    """Construct the core application."""
    app = Flask(__name__, instance_relative_config=False)
    CORS(app, supports_credentials=True)
    app.config.from_object('config.Config')
    db.init_app(app)
    login_manager.init_app(app)
    paranoid = Paranoid(app)
    @paranoid.on_invalid_session
    def invalid_session():
        abort(401)

    with app.app_context():
        from . import routes
        from . import auth

        # Create tables for our models
        db.create_all()

        return app

