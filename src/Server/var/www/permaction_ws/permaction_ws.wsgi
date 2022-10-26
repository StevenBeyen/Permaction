#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

import sys
sys.path.insert(0, '/var/www/permaction_ws')

from rest_api import create_app

application = create_app()

