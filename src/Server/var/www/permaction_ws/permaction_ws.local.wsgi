#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals
from werkzeug.middleware.profiler import ProfilerMiddleware

from rest_api import create_app

app = create_app()

if (__name__ == '__main__'):
    app.env = 'development'
    app.config['PROFILE'] = True
    app.wsgi_app = ProfilerMiddleware(app.wsgi_app, restrictions=[30])
    app.run(debug=True, port='13731', ssl_context='adhoc')

