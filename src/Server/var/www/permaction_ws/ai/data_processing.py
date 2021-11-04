#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from math import inf

from parameters import *
from rest_api.models import UserPlacementRequest, Element
from .element import *

class Preprocessing:
    
    """Data preprocessing: checks feasability and computes ideal sizes for requested elements to place."""

    def __init__(self, request_id, id_locale):
        user_placement_request = UserPlacementRequest.query.filter_by(id = request_id).first()
        self.id_locale = id_locale
        self.terrain_data = eval(user_placement_request.terrain_data)
        self.terrain_low = inf
        self.terrain_high = -inf
        self.elements_data = eval(user_placement_request.elements_data)
        self.elements = []
        self.average_element_ratio = 0.0
        
    def run(self):
        # First preprocessing: check if there is enough space to fit all elements,
        # and compute the percentage of maximum size each of them should use.
        self.compute_average_elements_ratio()
        # Then create all the elements that will be used by the AI.
        self.create_height_zones()
        self.create_elements()
        # TODO Add elements with default width and no default size
        # TODO Add zones and sectors
    
    def compute_average_elements_ratio(self):
        # First we'll check how much space we have on our terrain
        terrain_square_meters = 0.0
        global square_size, unallowed_height, var_width_size, max_filled_terrain_ratio
        squared_square_size = square_size ** 2
        for i in range(len(self.terrain_data)):
            for j in range(len(self.terrain_data[i])):
                terrain_ij = self.terrain_data[i][j]
                if (terrain_ij != unallowed_height):
                    terrain_square_meters += squared_square_size
                    # Since we are here, let's remember the terrain's highest and lowest values
                    if (terrain_ij < self.terrain_low):
                        self.terrain_low = terrain_ij
                    if (terrain_ij > self.terrain_high):
                        self.terrain_high = terrain_ij
        # Now let's check if the minimal size of all elements can fit inside the terrain.
        min_elements_size = 0
        max_elements_size = 0
        for element_id in self.elements_data:
            element_data = self.elements_data[element_id]
            default_size = Element.query.filter_by(id = element_id).first().default_size
            min_size = 0
            max_size = 0
            if (default_size != var_width_size):
                default_size_split = default_size.split('-')
                min_size = int(default_size_split[0])
                max_size = int(default_size_split[1])
            num_fixed_elements = len(element_data) - 1
            num_var_elements = element_data[0] - num_fixed_elements
            min_elements_size += min_size * num_var_elements
            max_elements_size += max_size * num_var_elements
            fixed_elements_size = 0
            for coordinates_list in element_data[1:]:
                fixed_elements_size += len(coordinates_list) * squared_square_size
            min_elements_size += fixed_elements_size
            max_elements_size += fixed_elements_size
        # If it cannot, let's throw an exception.
        if (min_elements_size > terrain_square_meters):
            raise NotEnoughSpaceException
        # If it can, let's check if the max size of all the elements also fits.
        # If it does, let's simply take the max size (since there is enough space).
        if (max_elements_size <= terrain_square_meters):
            self.average_element_ratio = 1.0
        else: # Otherwise, let's compute a ratio of all the elements' sizes so that they can all fit inside the terrain.
            self.average_element_ratio = min((terrain_square_meters/max_elements_size*max_filled_terrain_ratio), max_filled_terrain_ratio)
    
    def create_element(self, id, biotope_values, size = None, width = None, coordinates = None):
        if (size is not None):
            return RectangleElement(id, biotope_values, size)
        elif (width is not None):
            return LinearElement(id, biotope_values, width)
        elif (coordinates is not None):
            pass # TODO Add fixed elements creation
    
    def create_elements(self):
        fixed_width_elements = []
        for element_id in self.elements_data:
            element_data = self.elements_data[element_id]
            db_element = Element.query.filter_by(id = element_id).first()
            try:
                biotope_values = eval(db_element.biotope_values)
            except TypeError:
                biotope_values = [int(db_element.biotope_values)]
            finally:
                if (type(biotope_values) == int):
                    biotope_values = [biotope_values]
            # Variable elements
            for i in range(element_data[0] - (len(element_data) - 1)):
                # Case 1: elements with default size
                try:
                    max_size = int(db_element.default_size.split('-')[1])
                    element_size = int(max_size * self.average_element_ratio)
                    # Finding closest size for golden ratio
                    if (element_size < phi_min_value):
                    	element_size = phi_min_value
                    while (element_size not in phi_ratio_values):
                        element_size -= 1
                	# Creating the actual element
                    self.elements.append(self.create_element(element_id, biotope_values, size=element_size))
                except IndexError:
                    fixed_width_elements.append([element_id, db_element, biotope_values])
            # Fixed elements
            for i in range(len(element_data[1:])):
                element_coordinates = element_data[i+1]
                self.elements.append(self.create_element(element_id, biotope_values, coordinates=element_coordinates))
        # Case 2: elements without default size. Those elements should have a fixed width
        for element in fixed_width_elements: # [ID, DB_ELEMENT, BIOTOPE_VALUES]
            element_id = element[0]
            element_width = int(element[1].default_width)
            biotope_values = element[2]
            self.elements.append(self.create_element(element_id, biotope_values, width = element_width))
        
    def create_height_zones(self):
        global heights_mapping, low_tag, midheight_tag, heights_tag
        terrain_low_coordinates = []
        terrain_midheight_coordinates = []
        terrain_heights_coordinates = []
        # Computing limits for low, mid-height and heights
        terrain_low_limit = (self.terrain_high - self.terrain_low) / 3.0
        terrain_midheight_limit = terrain_low_limit * 2
        # Assigning terrain coordinates to different categories
        for i in range(len(self.terrain_data)):
            for j in range(len(self.terrain_data[i])):
                terrain_ij = self.terrain_data[i][j]
                if (terrain_ij <= terrain_low_limit):
                    terrain_low_coordinates.append((i,j))
                elif (terrain_ij <= terrain_midheight_limit):
                    terrain_midheight_coordinates.append((i,j))
                else: # Heights
                    terrain_heights_coordinates.append((i,j))
        # Creation of the three height zones (if they are not empty)
        if (terrain_low_coordinates):
            id = heights_mapping[self.id_locale][low_tag]
            biotope_values = [int(Element.query.filter_by(id = id).first().biotope_values)]
            self.elements.append(ZoneElement(id, biotope_values, terrain_low_coordinates))
        if (terrain_midheight_coordinates):
            id = heights_mapping[self.id_locale][midheight_tag]
            biotope_values = [int(Element.query.filter_by(id = id).first().biotope_values)]
            self.elements.append(ZoneElement(id, biotope_values, terrain_midheight_coordinates))
        if (terrain_heights_coordinates):
            id = heights_mapping[self.id_locale][heights_tag]
            biotope_values = [int(Element.query.filter_by(id = id).first().biotope_values)]
            self.elements.append(ZoneElement(id, biotope_values, terrain_heights_coordinates))
    
    def get_elements(self):
        return self.elements
    
    def get_terrain(self):
        return self.terrain_data

