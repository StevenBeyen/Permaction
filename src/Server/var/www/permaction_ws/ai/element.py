#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from math import sqrt, inf
from random import random, randint, uniform

from parameters import *


class AbstractElement:
    
    """Abstract Element class for AI optimisation algorithm. Is inherited by the actual Element classes defined by their shape.""" 
    
    def __init__(self):
        raise NotImplementedError
    
    def get_edges(self):
        raise NotImplementedError
        
    def get_used_coordinates(self):
        raise NotImplementedError
    
    def is_fixed(self):
        raise NotImplementedError
    
    def apply_mutation(self):
        raise NotImplementedError
    
    def get_minmax_coordinates(self):
    
        """ Return min & max X & Y values : (min_x, min_y, max_x, max_y).
        """
        
        min_x = float('inf')
        max_x = float('-inf')
        min_y = float('inf')
        max_y = float('-inf')
        for c in self.get_edges():
            if (c[0] < min_x):
                min_x = c[0]
            if (c[0] > max_x):
                max_x = c[0]
            if (c[1] < min_y):
                min_y = c[1]
            if (c[1] > max_y):
                max_y = c[1]
        return (min_x, min_y, max_x, max_y)
    
    def is_neighbour(self, element):
        """ Neighbouring based on Manhattan distance: if distance between two coordinates is less than max Manhattan distance,
        we consider the two elements to be neighbours.
        Example Manhattan distance: dist (0,1), (1,1) = 1 ; dist (0,0), (1,1) = 2."""
        global max_manhattan_distance
        for coord1 in self.get_edges():
            for coord2 in element.get_edges():
                if ((abs(coord1[0] - coord2[0]) + abs(coord1[1] - coord2[1])) <= max_manhattan_distance):
                    return True
        return False
    
    def biotope_intersection(self, element):
        """ Computation of the added fitness value based on biotope intersection of two elements.
            If elements have the same ID, we return 0 to avoid clustering all multiple elements together
            (assuming if multiple instances are wanted, they should not be next to each other)."""
        global biotope_intersection_added_value
        if (self.id == element.id):
            added_value = 0
        elif (self.biotope_killer or element.biotope_killer):
            added_value = 0
        # Biotope enhancers should be combined with something else than other enhancers
        elif (self.biotope_enhancer and element.biotope_enhancer):
            added_value = 0
        else:
            added_value = biotope_intersection_added_value * len(set(self.biotope_values).intersection(element.biotope_values))
            if (self.biotope_enhancer or element.biotope_enhancer):
                added_value += biotope_intersection_added_value
        return added_value
    
    def fitness_level(self, element, fitness):
        """Generic method for elements interaction."""
        return fitness if self.is_neighbour(element) else 0
    
    def is_equal(self, element):
        return (self.id == element.id and self.coordinates == element.coordinates)
        
    def to_json(self):
        global ai_x_tag, ai_y_tag, ai_id_tag, ai_coordinates_tag
        coordinates = []
        for coordinate in self.get_used_coordinates():
            coordinates.append({ai_x_tag: coordinate[0], ai_y_tag: coordinate[1]})
        reply = {ai_id_tag: self.id, ai_coordinates_tag: coordinates}
        return reply
    
    def similarity(self, element):
        if (self.id != element.id):
            return 0
        else:
            size_biggest_element = max(len(self.get_used_coordinates()), len(element.get_used_coordinates()))
            return len(set(element.get_used_coordinates()).intersection(self.get_used_coordinates())) / size_biggest_element
    
    def ternary_fitness_level(self, element, interaction_type_ids, terrain):
        """ Method for ternary interaction fitness computation.
            Interpretation of this method is : self, interaction, element (e.g. is the pond south of the house?)."""
            
        global ternary_interactions_mapping, higher_interaction_type, south_interaction_type, ternary_interaction_added_value
        
        for interaction_type_id in interaction_type_ids:
            interaction = ternary_interactions_mapping[interaction_type_id]
            
            # Case 1: we check if self is higher than the other element
            if (interaction == higher_interaction_type):
                try:
                    # First we take the max height of element:
                    max_element_height = -inf
                    for c in element.get_used_coordinates():
                        height = terrain[c[0]][c[1]]
                        if (height > max_element_height):
                            max_element_height = height
                    # Now we count the percentage of self's coordinates that are higher than other element's highest point
                    higher_counter = 0.0
                    for c in self.get_used_coordinates():
                        if (terrain[c[0]][c[1]] > max_element_height):
                            higher_counter += 1
                    return ternary_interaction_added_value * (higher_counter / len(self.get_used_coordinates()))
                # To discourage elements going out of the terrain boundaries, we return 0
                except IndexError:
                    return 0
            
            # Case 2: we check if self is south of the other element
            elif(interaction == south_interaction_type):
                # First we take the south, east and west limits of the other element
                element_west_limit = inf
                element_east_limit = -inf
                element_south_limit = -inf
                for c in element.get_edges():
                    if (c[0] < element_west_limit):
                        element_west_limit = c[0]
                    if (c[0] > element_east_limit):
                        element_east_limit = c[0]
                    if (c[1] > element_south_limit):
                        element_south_limit = c[1]
                # Now we check for the north, east and west limits of self
                self_west_limit = inf
                self_east_limit = -inf
                self_north_limit = inf
                for c in self.get_edges():
                    if (c[0] < self_west_limit):
                        self_west_limit = c[0]
                    if (c[0] > self_east_limit):
                        self_east_limit = c[0]
                    if (c[1] < self_north_limit):
                        self_north_limit = c[1]
                # If self's most north coordinate is not south of element's most south coordinate, we can already return 0
                if (self_north_limit <= element_south_limit):
                    return 0
                # Else we check what percentage of self is directly south of element, and return the value
                else:
                    # Case 1: self is southwest or southeast of element
                    if (self_east_limit < element_west_limit or self_west_limit > element_east_limit):
                        return 0
                    # Case 2: if self is wider than element and overflows on both sides, or if self is smaller and fully contained in element's east and west limits, then we can return the max value
                    if ((self_west_limit <= element_west_limit and self_east_limit >= element_east_limit) or (self_west_limit >= element_west_limit and self_east_limit <= element_east_limit)):
                        return ternary_interaction_added_value
                    # Here it's the general case: self's limit overflows on one side of element's limits, but not both. We can compute the percentage of coordinates that are directly south by computing the intersection between the two elements' limits
                    else:
                        self_width_range = range(self_west_limit, self_east_limit)
                        element_width_range = range(element_west_limit, element_east_limit)
                        return ternary_interaction_added_value * len(set(self_width_range).intersection(element_width_range)) / len(self_width_range)


class FixedElement(AbstractElement):
    
    """Fixed element class based on its coordinates."""
    
    def __init__(self, id, biotope_values, coordinates):
        global square_size, biotope_killer_value, biotope_enhancer_value
        squared_square_size = square_size ** 2
        
        self.id = id
        self.biotope_values = biotope_values
        if (biotope_killer_value in self.biotope_values):
            self.biotope_killer = True
            self.biotope_enhancer = False
        elif (biotope_enhancer_value in self.biotope_values):
            self.biotope_killer = False
            self.biotope_enhancer = True
        else:
            self.biotope_killer = False
            self.biotope_enhancer = False
        self.coordinates = coordinates
        self.size = len(coordinates) * squared_square_size
    
    def fitness_level(self, element, fitness):
        if (isinstance(element, ZoneElement)):
            return element.fitness_level(self, fitness)
        else:
            return super().fitness_level(element, fitness)
    
    def copy(self):
        # No need to copy this type of element since it's always the same
        return self
    
    def is_fixed(self):
        return True
    
    def get_edges(self):
        # TODO Update this with something more clever
        return self.coordinates
    
    def get_used_coordinates(self):
        return self.coordinates
        
    def get_size(self):
        return self.size
    
    def __repr__(self):
        print(self.coordinates)


class ZoneElement(FixedElement):
    
    """Zone element class for non-physical fixed elements like height zones.
    Behaves in the same way as a fixed element, but it should be able to overlap with other elements. Therefore the 
    get_used_coordinates() method returns an empty list."""
    
    def fitness_level(self, element, fitness):
        # Two Zone elements cannot interact together
        if (isinstance(element, ZoneElement)):
            return 0
        else: # Default : we check if the physical element's coordinates are inside the zone's coordinates
            return (len(set(element.get_used_coordinates()).intersection(self.get_zone_coordinates())) / len(element.get_used_coordinates())) * fitness
    
    def get_zone_coordinates(self):
        return self.coordinates
    
    def get_edges(self):
        return []
    
    def get_used_coordinates(self):
        return []


class LinearElement(AbstractElement):
    
    """Linear element class based on width. Length is variable."""
    
    def __init__(self, id, biotope_values, width):
        self.id = id
        self.biotope_values = biotope_values
        if (biotope_killer_value in self.biotope_values):
            self.biotope_killer = True
            self.biotope_enhancer = False
        elif (biotope_enhancer_value in self.biotope_values):
            self.biotope_killer = False
            self.biotope_enhancer = True
        else:
            self.biotope_killer = False
            self.biotope_enhancer = False
        self.width = width
        self.position = None
        self.length = None
        self.edges = None
        self.coordinates = None
        self.horizontal = None
        
    def copy(self):
        copy = LinearElement(self.id, self.biotope_values, self.width)
        copy.position = self.position
        copy.length = self.length
        copy.edges = None if self.edges is None else list(self.edges)
        copy.coordinates = None if self.coordinates is None else list(self.coordinates)
        copy.horizontal = self.horizontal
        return copy
    
    def fitness_level(self, element, fitness):
        if (isinstance(element, ZoneElement)):
            return element.fitness_level(self, fitness)
        else:
            return super().fitness_level(element, fitness)
        
    def is_fixed(self):
        return False
    
    def set_position(self, position):
        self.position = position
    
    def set_length(self, length):
        self.length = length
    
    def set_horizontal(self, horizontal):
        self.horizontal = horizontal
    
    def get_edges(self):
        return self.edges
        
    def get_used_coordinates(self):
        return self.coordinates
    
    def update(self):
        self.update_edges()
        self.update_coordinates()
    
    def update_edges(self):
        edges = []
        if (self.horizontal):
            width_edges = [0, self.width-1] if self.width != 1 else [0]
            for i in range(self.length):
                edges += [(self.position[0] + i, self.position[1] + j) for j in width_edges]
        else: # Vertical: swapping i and j
            width_edges = [0, self.width-1] if self.width != 1 else [0]
            for i in range(self.length):
                edges += [(self.position[0] + j, self.position[1] + i) for j in width_edges]
        self.edges = edges
    
    def update_coordinates(self):
        used_coordinates = []
        if (self.horizontal):
            for i in range(self.width):
                used_coordinates += [(self.position[0] + j, self.position[1] + i) for j in range(self.length)]
        else: # Vertical: swapping i and j
            for i in range(self.width):
                used_coordinates += [(self.position[0] + i, self.position[1] + j) for j in range(self.length)] 
        self.coordinates = used_coordinates
    
    def apply_mutation(self, max_length, max_width):
        
        """Produces the equivalent of a bit mutation with a probability of ga_mutation_rate.
        Mutation is applied on direction (horizontal/vertical), position and length."""
        
        global ga_mutation_rate
        
        # direction
        if (random() <= ga_mutation_rate):
            self.horizontal = not self.horizontal
        
        # position
        if (self.horizontal):
            length_pos = self.position[0]
            if (random() <= ga_mutation_rate):
                try:
                    length_pos = randint(0, max_length - self.length)
                except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                    pass
            # position: width (y)
            width_pos = self.position[1]
            if (random() <= ga_mutation_rate):
                try:
                    width_pos = randint(0, max_width - self.width)
                except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                    pass
        else: # Vertical
            length_pos = self.position[0]
            if (random() <= ga_mutation_rate):
                try:
                    length_pos = randint(0, max_length - self.width)
                except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                    pass
            # position: width (y)
            width_pos = self.position[1]
            if (random() <= ga_mutation_rate):
                try:
                    width_pos = randint(0, max_width - self.length)
                except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                    pass
        self.position = (length_pos, width_pos)
        
        # Multiplying max length and width by max_linear_element_ratio for new length computation
        max_length *= max_linear_element_ratio
        max_width *= max_linear_element_ratio
        
        # length
        self.length_mutation(max_length, max_width)
        
        # Update at the end to make sure all parameters are correct
        self.update()
    
    def length_mutation(self, max_length, max_width):
        if (random() <= ga_mutation_rate):
            if (self.horizontal):
                try:
                    length = linear_element_length_multiple * randint(1, round((max_length - self.position[0])/linear_element_length_multiple))
                except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                    pass
            else: # Vertical
                try:
                    length = linear_element_length_multiple * randint(1, round((max_width - self.position[1])/linear_element_length_multiple))
                except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                    pass


class RoadPathElement(LinearElement):
    
    """Road & path elements class, adding specific logic for those particular cases."""
    
    def __init__(self, id, biotope_values, width):
        super().__init__(id, biotope_values, width)
        self.connected_roads_paths_counter = 0 # Number of roads/paths this road/path is connected to. Resets at every generation with the mutation.
        self.physical_element_connection = False
    
    def copy(self):
        copy = RoadPathElement(self.id, self.biotope_values, self.width)
        copy.position = self.position
        copy.length = self.length
        copy.edges = None if self.edges is None else list(self.edges)
        copy.coordinates = None if self.coordinates is None else list(self.coordinates)
        copy.horizontal = self.horizontal
        copy.connected_roads_paths_counter = self.connected_roads_paths_counter
        copy.physical_element_connection = self.physical_element_connection
        return copy
        
    def fitness_level(self, element, fitness):
        if (isinstance(element, ZoneElement) or isinstance(element, RectangleElement)): # Warning: all physical elements should be called like this
            return element.fitness_level(self, fitness)
        else:
            return super().fitness_level(element, fitness)
    
    def is_neighbour(self, element):
        if (self.id != element.id):
            return super().is_neighbour(element)
        #elif (self.horizontal != element.horizontal): # Prettier to alternate horizontal and vertical road/path sections
        else:
            (self_min_x, self_min_y, self_max_x, self_max_y) = self.get_minmax_coordinates()
            (element_min_x, element_min_y, element_max_x, element_max_y) = element.get_minmax_coordinates()
            distance = min(abs(self_min_x - element_max_x), abs(self_max_x - element_min_x))
            distance += min(abs(self_max_y - element_min_y), abs(self_min_y - element_max_y))
            if (distance <= road_path_max_manhattan_distance):
                # It's a match!
                self.connected_roads_paths_counter += 1
                element.connected_roads_paths_counter += 1
                return True
        return False
    
    def disconnected_road_path(self):
        return (self.connected_roads_paths_counter == 0)
                        
    def apply_mutation(self, max_length, max_width):
        super().apply_mutation(max_length, max_width)
        # Little trick to reset connected roads/paths counter and physical element access between two generations.
        self.connected_roads_paths_counter = 0
        self.physical_element_connection = False
            

    def length_mutation(self, max_length, max_width):
        if (random() <= ga_mutation_rate):
            if (self.horizontal):
                try:
                    length = road_path_length_multiple * randint(1, round((max_length - self.position[0])/road_path_length_multiple))
                except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                    pass
            else: # Vertical
                try:
                    length = road_path_length_multiple * randint(1, round((max_width - self.position[1])/road_path_length_multiple))
                except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                    pass

class RectangleElement(AbstractElement):

    """Rectangle element class based on given size."""

    def __init__(self, id, biotope_values, size):
        self.id = id
        self.biotope_values = biotope_values
        if (biotope_killer_value in self.biotope_values):
            self.biotope_killer = True
            self.biotope_enhancer = False
        elif (biotope_enhancer_value in self.biotope_values):
            self.biotope_killer = False
            self.biotope_enhancer = True
        else:
            self.biotope_killer = False
            self.biotope_enhancer = False
        self.size = size
        self.position = None
        self.length = phi_ratio_values[size][0]
        self.width = phi_ratio_values[size][1]
        self.edges = None
        self.coordinates = None
        self.road_access = False
        self.path_access = False
    
    def copy(self):
        copy = RectangleElement(self.id, self.biotope_values, self.size)
        copy.position = self.position
        copy.length = self.length
        copy.width = self.width
        copy.edges = None if self.edges is None else list(self.edges)
        copy.coordinates = None if self.coordinates is None else list(self.coordinates)
        copy.road_access = self.road_access
        copy.path_access = self.path_access
        return copy
    
    def fitness_level(self, element, fitness):
        # Method override to avoid multiple road and path accesses
        if (isinstance(element, ZoneElement)):
            return element.fitness_level(self, fitness)
        elif (not isinstance(element, RoadPathElement)):
            return super().fitness_level(element, fitness)
        elif (element.id in road_ids):
            if (self.road_access or element.physical_element_connection):
                return 0
            else:
                if (self.is_neighbour(element)):
                    self.road_access = True
                    element.physical_element_connection = True
                    return fitness
                return 0
        elif (element.id in path_ids):
            if (self.path_access or element.physical_element_connection):
                return 0
            else:
                if (self.is_neighbour(element)):
                    self.path_access = True
                    element.physical_element_connection = True
                    return fitness
                return 0
        else: # Not a road or a path, so we raise a value error
            raise ValueError
    
    def is_fixed(self):
        return False
    
    def swap_width_length(self):
    	width = self.width
    	self.width = self.length
    	self.length = width
    
    def set_position(self, position):
        self.position = position
       
    def get_position(self):
        return self.position
   
    def get_length(self):
        return self.length
    
    def get_width(self):
        return self.width
    
    def get_edges(self):
        return self.edges
        
    def get_used_coordinates(self):
        return self.coordinates
    
    def update(self):
        self.update_edges()
        self.update_coordinates()
    
    def update_edges(self):
        edges = []
        length_edges = [0, self.length-1] if self.length != 1 else [0]
        for i in range(self.width):
            edges += [(self.position[0] + j, self.position[1] + i) for j in length_edges]
        self.edges = edges
    
    def update_coordinates(self):
        used_coordinates = []
        for i in range(self.width):
            used_coordinates += [(self.position[0] + j, self.position[1] + i) for j in range(self.length)]
        self.coordinates = used_coordinates
    
    def apply_mutation(self, max_length, max_width):
        
        """Produces the equivalent of a bit mutation with a probability of ga_mutation_rate.
        Mutation is applied on shape and position."""
        
        global ga_mutation_rate
        
        # shape
        if (random() <= ga_mutation_rate):
        	self.swap_width_length()
        	self.update()
        
        # position: length (x)
        length_pos = self.position[0]
        if (random() <= ga_mutation_rate):
            try:
                length_pos = randint(0, max_length - self.length)
            except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                pass
        # position: width (y)
        width_pos = self.position[1]
        if (random() <= ga_mutation_rate):
            try:
                width_pos = randint(0, max_width - self.width)
            except ValueError: # empty range means no other possible options, so we better leave it the way it is.
                pass
        self.position = (length_pos, width_pos)
        
        # Finally, let's reset road and path access variables
        self.road_access = False
        self.path_access = False
        
        # Update at the end to make sure all parameters are correct
        self.update()

