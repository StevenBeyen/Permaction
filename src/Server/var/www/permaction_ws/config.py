#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

class Config:
    """Flask configuration."""

    # General
    ENV = "production"
    
    # Security
    SESSION_COOKIE_SECURE = True
    REMEMBER_COOKIE_SECURE = True
    SECRET_KEY = "6b90d7857e2f988933f9ad374ef40367252792b86c0dd751"

    # Database
    SQLALCHEMY_DATABASE_URI = "sqlite:///permaction.db"
    SQLALCHEMY_TRACK_MODIFICATIONS = False

