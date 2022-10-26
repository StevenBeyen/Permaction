#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from .data_processing import Preprocessing
from .guru import Guru

class AI:
    
    """Main processing thread that will start the AI for the given placement request."""

    def __init__(self, request_id, id_locale):
        self.request_id = request_id
        self.id_locale = id_locale
        self.guru = None
    
    def run(self):
        # Preprocessing
        preprocessing = Preprocessing(self.request_id, self.id_locale)
        preprocessing.run()
        elements = preprocessing.get_elements()
        terrain = preprocessing.get_terrain()
        # Start genetic algorithm
        self.guru = Guru(self.id_locale, elements, terrain)
        self.guru.run()
    
    def make_response(self):
        return self.guru.make_response()

