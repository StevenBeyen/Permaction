#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from math import sqrt, inf
from random import random, randint, gauss, choice, randrange

from parameters import *


class AbstractElement:
    
    """Abstract Element class for AI optimisation algorithm. Is inherited by the actual Element classes defined by their shape.""" 
    
    def __init__(self):
        raise NotImplementedError
    
    def get_corners(self):
        raise NotImplementedError
    
    def breed(self, element):
        raise NotImplementedError
    
    def is_fixed(self):
        raise NotImplementedError
    
    def apply_mutation(self):
        raise NotImplementedError
    
    def get_minmax_coordinates(self):
        return (self.get_corners()[0] + self.get_corners()[1])
    
    def get_used_coordinates(self):
        corners = self.get_corners()
        coordinates = []
        for i in range (corners[0][0], corners[-1][0] + 1):
            for j in range (corners[0][1], corners[-1][1] + 1):
                coordinates.append([i,j])
        return coordinates
    
    def distance(self, element):
        distance = 0
        (self_min_x, self_min_y, self_max_x, self_max_y) = self.get_minmax_coordinates()
        (element_min_x, element_min_y, element_max_x, element_max_y) = element.get_minmax_coordinates()
        
        if (self_max_x < element_min_x): # self left of element, so we need to add this distance to manhattan distance
            distance += element_min_x - self_max_x
        elif (element_max_x < self_min_x): # self right of element, same thing but the other way around
            distance += self_min_x - element_max_x
        if (self_max_y < element_min_y): # self 'under' element (on y axis)
            distance += element_min_y - self_max_y
        elif (element_max_y < self_min_y): # self 'above' element
            distance += self_min_y - element_max_y
        return distance
    
    def horizontal_intersection(self, element, added_distance):
        (self_min_x, self_min_y, self_max_x, self_max_y) = self.get_minmax_coordinates()
        (element_min_x, element_min_y, element_max_x, element_max_y) = element.get_minmax_coordinates()
        self_min_y - added_distance
        return not (self_max_y + added_distance < element_min_y - added_distance or element_max_y + added_distance < self_min_y - added_distance)
    
    def vertical_intersection(self, element, added_distance):
        (self_min_x, self_min_y, self_max_x, self_max_y) = self.get_minmax_coordinates()
        (element_min_x, element_min_y, element_max_x, element_max_y) = element.get_minmax_coordinates()
        return not (self_max_x + added_distance < element_min_x - added_distance or element_max_x + added_distance < self_min_x - added_distance)
    
    def neighbour_value(self, element, fitness_value):
    
        """ Neighbouring based on Manhattan distance: if distance between two coordinates is less than max Manhattan distance,
        we consider the two elements to be neighbours.
        Example Manhattan distance: dist (0,1), (1,1) = 1 ; dist (0,0), (1,1) = 2."""
        
        global max_manhattan_distance
        distance = 0
        (self_min_x, self_min_y, self_max_x, self_max_y) = self.get_minmax_coordinates()
        (element_min_x, element_min_y, element_max_x, element_max_y) = element.get_minmax_coordinates()
        
        if (self_max_x < element_min_x): # self left of element, so we need to add this distance to manhattan distance
            distance += element_min_x - self_max_x
        elif (element_max_x < self_min_x): # self right of element, same thing but the other way around
            distance += self_min_x - element_max_x
        if (self_max_y < element_min_y): # self 'under' element (on y axis)
            distance += element_min_y - self_max_y
        elif (element_max_y < self_min_y): # self 'above' element
            distance += self_min_y - element_max_y
            
        if (distance == max_manhattan_distance):
            return fitness_value
        elif (distance < max_manhattan_distance): # Compensating distance for elements too close to each other
            distance += (max_manhattan_distance - distance) * 3
        return (fitness_value / (distance - max_manhattan_distance + 1))
    
    def intersection_level(self, element):
    
        """Computing the intersection percentage between two elements."""
        
        if (isinstance(self, ZoneElement) or isinstance(element, ZoneElement)):
            return 0
        min_x = max(self.get_corners()[0][0], element.get_corners()[0][0])
        min_y = max(self.get_corners()[0][1], element.get_corners()[0][1])
        max_x = min(self.get_corners()[-1][0], element.get_corners()[-1][0])
        max_y = min(self.get_corners()[-1][1], element.get_corners()[-1][1])
        if (max_x >= min_x and max_y >= min_y):
            return ((max_x - min_x + 1) * (max_y - min_y + 1)) / min(self.size, element.size)
        else:
            return 0
    
    def out_of_terrain_level(self, terrain, length, width):
    
        """Computing the percentage element has out of terrain boundaries"""
        
        global unallowed_height, border_protection
        if (isinstance(self, ZoneElement) or self.size == 0):
            return 0
        length -= border_protection
        width -= border_protection
        oot_counter = 0
        (self_min_x, self_min_y, self_max_x, self_max_y) = self.get_minmax_coordinates()
        # WARNING: this section has been commented out, but it's actually the right was to do it since terrain may have
        #   unallowed placement spots. Used script after commented one is a simplified version for demo purposes
        """for i in range(self_min_x, self_max_x + 1):
            for j in range(self_min_y, self_max_y + 1):
                try:
                    if (i < 0 or j < 0 or terrain[i][j] == unallowed_height):
                        oot_counter += 1
                except IndexError: # Means we exited terrain boundaries, so simply incrementing counter
                    oot_counter += 1
        return oot_counter"""
        self_length = self_max_x - self_min_x + 1
        self_width = self_max_y - self_min_y + 1
        if (self_min_x < border_protection):
            oot_counter += border_protection - self_min_x * self_width
        if (self_max_x >= length):
            oot_counter += (self_max_x - length + 1) * self_width
        if (self_min_y < border_protection):
            oot_counter += border_protection - self_min_y * self_length
        if (self_max_y >= width):
            oot_counter += (self_max_y - width + 1) * self_length
        return oot_counter / self.size
    
    def fitness_level(self, element, fitness):
    
        """Generic method for elements interaction."""
        
        neighbour_value = self.neighbour_value(element, fitness)
        if (neighbour_value == fitness):
            self.update_cluster(element)
        return neighbour_value
    
    def update_cluster(self, element):
        self.cluster = self.cluster.union(element.cluster)
        for element_cluster in element.cluster:
            element_cluster.cluster = self.cluster
        element.cluster = self.cluster
    
    def is_equal(self, element):
        return (self.id == element.id and self.corners == element.corners)
        
    def to_json(self):
        global ai_x_tag, ai_y_tag, ai_id_tag, ai_coordinates_tag
        coordinates = []
        for coordinate in self.get_corners():
            coordinates.append({ai_x_tag: coordinate[0], ai_y_tag: coordinate[1]})
        reply = {ai_id_tag: self.id, ai_coordinates_tag: coordinates}
        return reply
    
    def ternary_fitness_level(self, element, interaction_type_ids, terrain):
    
        """ Method for ternary interaction fitness computation.
            Interpretation of this method is : self, interaction, element (e.g. is the pond south of the greenhouse?)."""
            
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
                    return ternary_interaction_added_value * (higher_counter / self.size)
                # To discourage elements going out of the terrain boundaries, we return 0
                except IndexError:
                    return 0
            
            # Case 2: we check if self is south of the other element
            elif(interaction == south_interaction_type):
                # First we take the south, east and west limits of the other element
                (element_min_x, element_min_y, element_max_x, element_max_y) = element.get_minmax_coordinates()
                element_west_limit = element_min_x
                element_east_limit = element_max_x
                element_south_limit = element_max_y
                # Now we check for the north, east and west limits of self
                (self_min_x, self_min_y, self_max_x, self_max_y) = self.get_minmax_coordinates()
                self_west_limit = self_min_x
                self_east_limit = self_max_x
                self_north_limit = self_min_y
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

    def jump_over(self, element):
        (self_min_x, self_min_y, self_max_x, self_max_y) = self.get_minmax_coordinates()
        (element_min_x, element_min_y, element_max_x, element_max_y) = element.get_minmax_coordinates()
        x_distance = min(abs(self_max_x - element_min_x), abs(self_min_x - element_max_x))
        y_distance = min(abs(self_max_y - element_min_y), abs(self_min_y - element_max_y))
        if (self_max_x <= element_min_x): # self is left of element
            x_position = element_max_x + x_distance
        else: # self is right of element
            x_position = element_min_x - x_distance - self.length
        if (self_max_y <= element_min_y): # self is under element
            y_position = element_max_y + y_distance
        else: # self is above element
            y_position = element_min_x - y_distance - self.width
        self.position = [x_position, y_position]

class FixedElement(AbstractElement):
    
    """Fixed element class based on its coordinates."""
    
    def __init__(self, id, coordinates):
        self.id = id
        self.coordinates = coordinates
        self.corners = self.set_corners(coordinates)
        self.road_connected_element = False
        self.path_connected_element = False
    
    def set_corners(self, coordinates):
        min_x = float('inf')
        max_x = float('-inf')
        min_y = float('inf')
        max_y = float('-inf')
        for c in coordinates:
            if (c[0] < min_x):
                min_x = c[0]
            if (c[0] > max_x):
                max_x = c[0]
            if (c[1] < min_y):
                min_y = c[1]
            if (c[1] > max_y):
                max_y = c[1]
        return [[min_x, min_y], [max_x, max_y]]
    
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
    
    def get_corners(self):
        return self.corners
    
    def breed(self, element):
        # Not much to do with fixed elements...
        return [self, element]

    def apply_mutation(self, mutation_rate, max_length, max_width):
        # Not mutating fixed elements
        pass
        
    def get_size(self):
        return self.size



class ZoneElement(FixedElement):
    
    """Zone element class for non-physical fixed elements like height zones.
    Behaves in the same way as a fixed element, but it should be able to overlap with other elements. Therefore the 
    get_used_coordinates() method returns an empty list."""
    
    def fitness_level(self, element, fitness):
        # Two Zone elements cannot interact together
        if (isinstance(element, ZoneElement)):
            return 0
        else: # Default : we check if the physical element's coordinates are inside the zone's coordinates
            element_coordinates = []
            corners = element.get_corners()
            for i in range (corners[0][0], corners[-1][0] + 1):
                for j in range (corners[0][1], corners[-1][1] + 1):
                    element_coordinates.append((i,j))
            return (len(set(element_coordinates).intersection(self.get_zone_coordinates())) / len(element_coordinates)) * fitness
    
    def get_zone_coordinates(self):
        return self.coordinates



class LinearElement(AbstractElement):
    
    """Linear element class based on width. Length is variable."""
    
    def __init__(self, id, width):
        self.id = id
        self.width = width
        self.size = None
        self.position = None
        self.length = None
        self.corners = None
        self.horizontal = None
        self.cluster = set()
        self.cluster_corners = None
        self.road_connected_element = False
        self.path_connected_element = False
        
    def copy(self):
        copy = LinearElement(self.id, self.width)
        copy.position = self.position
        copy.length = self.length
        copy.corners = None if self.corners is None else list(self.corners)
        copy.horizontal = self.horizontal
        copy.cluster = set(self.cluster)
        copy.cluster_corners = self.cluster_corners if self.cluster_corners is None else list(self.cluster_corners)
        copy.road_connected_element = self.road_connected_element
        copy.path_connected_element = self.path_connected_element
        return copy
    
    def fitness_level(self, element, fitness):
        if (isinstance(element, ZoneElement)):
            return element.fitness_level(self, fitness)
        else:
            return super().fitness_level(element, fitness)
        
    def is_fixed(self):
        return False
    
    def get_position(self):
        return self.position
    
    def set_position(self, position):
        self.position = position
    
    def set_length(self, length):
        self.length = length
    
    def set_horizontal(self, horizontal):
        self.horizontal = horizontal
    
    def get_corners(self):
        return self.corners
    
    def breed(self, element):
        self_copy = self.copy()
        element_copy = element.copy()
        return [self_copy, element_copy]
    
    def update(self):
        self.update_size()
        self.update_corners()
        self.update_cluster_corners()
    
    def update_size(self):
        if (self.length != 0):
            self.size = self.length * self.width
        else:
            self.size = 1
    
    def update_corners(self):
        if (self.length != 0):
            if (self.horizontal):
                hor_extr = self.position[0] + self.length - 1
                vert_extr = self.position[1] + self.width - 1
            else: # Vertical
                hor_extr = self.position[0] + self.width - 1
                vert_extr = self.position[1] + self.length - 1
        else:
            hor_extr = self.position[0]
            vert_extr = self.position[1]
        self.corners = [self.position, [hor_extr, vert_extr]]
    
    def update_cluster_corners(self):
        self.cluster_corners = list(self.corners)
        for element in self.cluster:
            (min_x, min_y) = self.cluster_corners[0]
            (max_x, max_y) = self.cluster_corners[-1]
            if (min_x < self.cluster_corners[0][0]):
                self.cluster_corners[0][0] = min_x
            if (min_y < self.cluster_corners[0][1]):
                self.cluster_corners[0][1] = min_y
            if (max_x > self.cluster_corners[-1][0]):
                self.cluster_corners[-1][0] = max_x
            if (max_y > self.cluster_corners[-1][1]):
                self.cluster_corners[-1][1] = max_y
    
    def fitness_level(self, element, fitness):
        if (isinstance(element, ZoneElement) or isinstance(element, RectangleElement)): # Warning: all physical elements should be called like this
            return element.fitness_level(self, fitness)
        else:
            fitness_level = super().fitness_level(element, fitness)
            if (fitness_level == fitness):
                self.update_cluster(element)
            return fitness_level
    
    def apply_mutation(self, mutation_rate, max_length, max_width):
        
        """Produces the equivalent of a bit mutation with a probability of ga_mutation_rate.
        Mutation is applied on direction (horizontal/vertical), position and length."""
        
        global ga_mutation_sigma, ga_cluster_rate
        
        # Length & width extraction
        length_pos = self.position[0]
        width_pos = self.position[1]
        
        # direction
        if (random() <= mutation_rate):
            self.horizontal = not self.horizontal
        
        # position
        if (random() <= mutation_rate):
            try:
                if (self.corners[0][0] == self.cluster_corners[0][0]): # Element most left of cluster, so we can start at 0
                    min_x = 0
                else: # Otherwise add the difference
                    min_x = self.corners[0][0] - self.cluster_corners[0][0]
                if (self.corners[-1][0] == self.cluster_corners[-1][0]): # Element most right of cluster, so we can go til max_length - self.length
                    max_x = max_length - self.length
                else: # Otherwise substract the difference
                    max_x = max_length - self.length - (self.cluster_corners[-1][0] - self.corners[-1][0])
                length_pos = randint(min_x, max_x)
                #length_pos = int(round(gauss(length_pos, ga_mutation_sigma)))
                if (random() <= ga_cluster_rate):
                    length_pos_translation = length_pos - self.position[0]
                    for element in self.cluster:
                        element.position[0] += length_pos_translation
            except TypeError:
                print(self.corners)
                print(self.cluster_corners)
                raise TypeError

        if (random() <= mutation_rate):
            if (self.corners[0][1] == self.cluster_corners[0][1]): # Lowest element of cluster, so we can start at 0
                min_y = 0
            else: # Otherwise add the difference
                min_y = self.corners[0][1] - self.cluster_corners[0][1]
            if (self.corners[-1][1] == self.cluster_corners[-1][1]): # Highest element of cluster, so we can go til max_width - self.width
                max_y = max_width - self.width
            else: # Otherwise substract the difference
                max_y = max_width - self.width - (self.cluster_corners[-1][1] - self.corners[-1][1])
            width_pos = randint(min_y, max_y)
            #width_pos = int(round(gauss(width_pos, ga_mutation_sigma)))
            if (random() <= ga_cluster_rate):
                width_pos_translation = width_pos - self.position[1]
                for element in self.cluster:
                    element.position[1] += width_pos_translation
        
        # exotic "jump-over" mutation
        if ((len(self.cluster) != 0) and (random() <= mutation_rate)):
            element = choice(self.cluster)
            self.jump_over(element)
        
        self.position = [length_pos, width_pos]
        
        # length
        if (random() <= mutation_rate):
            self.length_mutation(max_length, max_width)
        
        # Update at the end to make sure all parameters are correct
        self.update()
        for element in self.cluster:
            element.update()
        
        # And let's reset the cluster as well
        self.cluster = set()
    
    def length_mutation(self, max_length, max_width):
        global max_linear_element_ratio, linear_element_length_multiple
        if (self.horizontal):
            max_random_length = min(max_length - self.position[0], max_length * max_linear_element_ratio)
        else: # Vertical
            max_random_length = min(max_width - self.position[1], max_width * max_linear_element_ratio)
        try:
            self.length = randrange(linear_element_length_multiple, max_random_length, step = linear_element_length_multiple)
        except ValueError: # No other possible values, so we leave it with its current length
            pass
    
    """def length_mutation(self, max_length, max_width):
        global ga_mutation_sigma, max_linear_element_ratio, linear_element_length_multiple
        # Multiplying max length and width by max_linear_element_ratio for new length computation
        max_length *= max_linear_element_ratio
        max_width *= max_linear_element_ratio
        if (self.horizontal):
            self.length = int(round(gauss(self.length, ga_mutation_sigma * linear_element_length_multiple)))
            if (self.length > max_length):
                self.length = int(max_length)
            while (self.length % linear_element_length_multiple != 0):
                self.length -= 1
            if (self.length < linear_element_length_multiple):
                self.length = linear_element_length_multiple
        else: # Vertical
            self.length = int(round(gauss(self.length, ga_mutation_sigma * linear_element_length_multiple)))
            if (self.length > max_width):
                self.length = int(max_width)
            while (self.length % linear_element_length_multiple != 0):
                self.length -= 1
            if (self.length < linear_element_length_multiple):
                self.length = linear_element_length_multiple"""


class RoadPathElement(LinearElement):
    
    """Road & path elements class, adding specific logic for those particular cases."""
    
    def __init__(self, id, width):
        super().__init__(id, width)
    
    def copy(self):
        copy = RoadPathElement(self.id, self.width)
        copy.position = self.position
        copy.length = self.length
        copy.horizontal = self.horizontal
        copy.corners = self.corners
        return copy
    
    def connect(self, source_element, destination_element, horizontal):
        global road_path_length_multiple, road_max_manhattan_distance, path_max_manhattan_distance, road_ids, path_ids, road_width, path_width
        swapped = False
        if (self.id in road_ids):
            added_distance = road_max_manhattan_distance
            removed_distance = road_width
        else: # Path
            added_distance = path_max_manhattan_distance
            removed_distance = path_max_manhattan_distance
        if (horizontal): # West -> east
            # Swap if necessary
            if (destination_element.get_corners()[-1][0] < source_element.get_corners()[-1][0]):
                (source_element, destination_element) = (destination_element, source_element)
                swapped = True
            x_pos = source_element.get_corners()[0][0]
            # Same starting height, so we can just put a straight line until the end of destination element
            if (source_element.get_corners()[0][1] == destination_element.get_corners()[0][1]):
                y_pos = source_element.get_corners()[0][1] - removed_distance
                length = destination_element.get_corners()[-1][0] - x_pos #+ 1
            # Same end height, same rule applies
            elif (source_element.get_corners()[-1][1] == destination_element.get_corners()[-1][1]):
                y_pos = source_element.get_corners()[-1][1] + added_distance
                length = destination_element.get_corners()[-1][0] - x_pos #+ 1
            else: # General case, we stop at the edge of the destination element and another road/path section will complete the connection
                y_pos = source_element.get_corners()[-1][1] + added_distance
                length = destination_element.get_corners()[0][0] - x_pos - removed_distance - 1
        else: # North -> south
            # Swap if necessary
            if (destination_element.get_corners()[-1][1] < source_element.get_corners()[-1][1]):
                (source_element, destination_element) = (destination_element, source_element)
                swapped = True
            y_pos = source_element.get_corners()[0][1]
            # Same west limit, so we can just put a straight line until the end of destination element
            if (source_element.get_corners()[0][1] == destination_element.get_corners()[0][1]):
                x_pos = source_element.get_corners()[0][0] - removed_distance
                length = destination_element.get_corners()[-1][1] - y_pos #+ 1
            # Same east limit, same rule applies
            elif (source_element.get_corners()[-1][1] == destination_element.get_corners()[-1][1]):
                x_pos = source_element.get_corners()[-1][0] + added_distance
                length = destination_element.get_corners()[-1][1] - y_pos #+ 1
            else: # General case, we stop at the edge of the destination element and another road/path section will complete the connection
                x_pos = source_element.get_corners()[-1][0] + added_distance
                length = destination_element.get_corners()[0][1] - y_pos - removed_distance - 1
        # Rework length to fit multiple
        length -= length % road_path_length_multiple
        if (length < 0):
            length = 0
        # Update road/path element
        self.horizontal = horizontal
        self.position = [x_pos, y_pos]
        self.length = length
        self.update()
        return swapped
    
    def split(self, element):
        # Splitting self over given element (first step of going around element)
        global road_path_length_multiple, road_max_manhattan_distance, path_max_manhattan_distance, road_ids, path_ids, road_width, path_width
        if (self.id in road_ids):
            added_distance = road_max_manhattan_distance
            removed_distance = road_width
        else: # Path
            added_distance = path_max_manhattan_distance
            removed_distance = path_max_manhattan_distance
        copy = self.copy()
        if (self.horizontal):
            new_self_length = element.get_corners()[0][0] - self.position[0] - removed_distance
            new_copy_length = self.length - (element.get_corners()[-1][0] - self.position[0]) - removed_distance
            new_copy_x_pos = element.get_corners()[-1][0] + added_distance
            new_copy_y_pos = copy.position[1]
        else:
            new_self_length = element.get_corners()[0][1] - self.position[1] - removed_distance
            new_copy_length = self.length - (element.get_corners()[-1][1] - self.position[1]) - removed_distance
            new_copy_x_pos = copy.position[0]
            new_copy_y_pos = element.get_corners()[-1][1] + added_distance
        # Rework lengths to fit multiple
        new_self_length -= new_self_length % road_path_length_multiple
        if (new_self_length < 0):
            new_self_length = 0
        new_copy_length -= new_copy_length % road_path_length_multiple
        if (new_copy_length < 0):
            new_copy_length = 0
        # Updating road/path elements with new positions and lengths
        self.length = new_self_length
        copy.length = new_copy_length
        copy.position = [new_copy_x_pos, new_copy_y_pos]
        self.update()
        copy.update()
        return copy
    
    def connect_around(self, path, element):
        # Method to connect self and path by going around the given element (i.e. creating three subpaths)
        global road_path_length_multiple, road_max_manhattan_distance, path_max_manhattan_distance, road_ids, path_ids, road_width, path_width
        if (self.id in road_ids):
            added_distance = road_max_manhattan_distance
            removed_distance = road_width
            width = road_width
        else: # Path
            added_distance = path_max_manhattan_distance
            removed_distance = path_max_manhattan_distance
            width = path_width
        subpath1 = self.copy()
        subpath1.horizontal = not subpath1.horizontal
        subpath2 = self.copy()
        subpath3 = self.copy()
        subpath3.horizontal = not subpath3.horizontal
        subpaths = [subpath1, subpath2, subpath3]
        if (self.horizontal):
            if (random() < 0.5): # We go 'above' the element (towards smaller y values)
                subpath2.position = [element.get_corners()[0][0] - removed_distance, element.get_corners()[0][1] - removed_distance]
                subpath2.length = element.get_corners()[-1][0] + removed_distance - subpath2.position[0]
                subpath1.position = [element.get_corners()[0][0] - removed_distance, subpath2.position[1] + width]
                subpath1.length = self.position[1] - subpath1.position[1]
                subpath3.position = [element.get_corners()[-1][0] + added_distance, subpath2.position[1] + width]
                subpath3.length = self.position[1] - subpath3.position[1]
            else: # We go 'below' the element (towards higher y values)
                subpath2.position = [element.get_corners()[-1][0] - removed_distance, element.get_corners()[-1][1] + added_distance]
                subpath2.length = element.get_corners()[-1][0] + removed_distance - subpath2.position[0]
                subpath1.position = [element.get_corners()[0][0] - removed_distance, self.position[1]]
                subpath1.length = subpath1.position[1] - self.position[1]
                subpath3.position = [element.get_corners()[-1][0] + added_distance, self.position[1]]
                subpath3.length = subpath3.position[1] - self.position[1]
        else: # Vertical
            if (random() < 0.5): # We go left of element (towards smaller x values)
                subpath2.position = [element.get_corners()[0][0] - removed_distance, element.get_corners()[0][1] - removed_distance]
                subpath2.length = element.get_corners()[-1][1] + removed_distance - subpath2.position[1]
                subpath1.position = [subpath2.position[0] + width, element.get_corners()[0][1] - removed_distance]
                subpath1.length = self.position[0] - subpath1.position[0]
                subpath3.position = [subpath2.position[0] + width, element.get_corners()[-1][1] + added_distance]
                subpath3.length = self.position[0] - subpath3.position[0]
            else: # We go right of element (towards higher x values)
                subpath2.position = [element.get_corners()[-1][0] + added_distance, element.get_corners()[-1][1] - removed_distance]
                subpath2.length = element.get_corners()[-1][1] + removed_distance - subpath2.position[1]
                subpath1.position = [self.position[0], element.get_corners()[0][1] - removed_distance]
                subpath1.length = subpath1.position[0] - self.position[0]
                subpath3.position = [self.position[0], element.get_corners()[-1][1] + added_distance]
                subpath3.length = subpath3.position[0] - self.position[0]
        # Rework lengths to fit multiple
        for subpath in subpaths:
            subpath.length -= subpath.length % road_path_length_multiple
            if (subpath.length < 0):
                subpath.length = 0
            subpath.update()
        return subpaths



class RectangleElement(AbstractElement):

    """Rectangle element class based on given size."""

    def __init__(self, id, size):
        self.id = id
        self.size = size
        self.position = None
        self.length = phi_ratio_values[size][0]
        self.width = phi_ratio_values[size][1]
        self.corners = None
        self.road_access = False
        self.path_access = False
        self.cluster = set()
        self.cluster_corners = None
        self.road_connected_element = False
        self.path_connected_element = False
    
    def copy(self):
        copy = RectangleElement(self.id, self.size)
        copy.position = self.position
        copy.length = self.length
        copy.width = self.width
        copy.corners = None if self.corners is None else list(self.corners)
        copy.road_access = self.road_access
        copy.path_access = self.path_access
        copy.cluster = set(self.cluster)
        copy.cluster_corners = self.cluster_corners if self.cluster_corners is None else list(self.cluster_corners)
        copy.road_connected_element = self.road_connected_element
        copy.path_connected_element = self.path_connected_element
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
                neighbour_value = self.neighbour_value(element, fitness)
                if (neighbour_value == fitness):
                    self.road_access = True
                    element.physical_element_connection = True
                    self.update_cluster(element)
                return neighbour_value
        elif (element.id in path_ids):
            if (self.path_access or element.physical_element_connection):
                return 0
            else:
                neighbour_value = self.neighbour_value(element, fitness)
                if (neighbour_value == fitness):
                    self.path_access = True
                    element.physical_element_connection = True
                    self.update_cluster(element)
                return neighbour_value
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
    
    def get_corners(self):
        return self.corners
    
    def breed(self, element):
        self_copy = self.copy()
        element_copy = element.copy()
        return [self_copy, element_copy]
    
    def update(self):
        self.update_corners()
        self.update_cluster_corners()
    
    def update_corners(self):
        hor_extr = self.position[0] + self.length - 1
        vert_extr = self.position[1] + self.width - 1
        self.corners = [self.position, [hor_extr, vert_extr]]
    
    def update_cluster_corners(self):
        self.cluster_corners = list(self.corners)
        for element in self.cluster:
            (min_x, min_y) = self.cluster_corners[0]
            (max_x, max_y) = self.cluster_corners[-1]
            if (min_x < self.cluster_corners[0][0]):
                self.cluster_corners[0][0] = min_x
            if (min_y < self.cluster_corners[0][1]):
                self.cluster_corners[0][1] = min_y
            if (max_x > self.cluster_corners[-1][0]):
                self.cluster_corners[-1][0] = max_x
            if (max_y > self.cluster_corners[-1][1]):
                self.cluster_corners[-1][1] = max_y
    
    def apply_mutation(self, mutation_rate, max_length, max_width):
        
        """Produces mutations with a probability of ga_mutation_rate.
        Mutation is applied on shape and position."""
        
        global ga_mutation_sigma, ga_cluster_rate
        
        length_pos = self.position[0]
        width_pos = self.position[1]
        
        # position
        if (random() <= mutation_rate):
            if (self.corners[0][0] == self.cluster_corners[0][0]): # Element most left of cluster, so we can start at 0
                min_x = 0
            else: # Otherwise add the difference
                min_x = self.corners[0][0] - self.cluster_corners[0][0]
            if (self.corners[-1][0] == self.cluster_corners[-1][0]): # Element most right of cluster, so we can go til max_length - self.length
                max_x = max_length - self.length
            else: # Otherwise substract the difference
                max_x = max_length - self.length - (self.cluster_corners[-1][0] - self.corners[-1][0])
            length_pos = randint(min_x, max_x)
            #length_pos = int(round(gauss(length_pos, ga_mutation_sigma)))
            length_pos_translation = length_pos - self.position[0]
            for element in self.cluster:
                if (random() <= ga_cluster_rate):
                    element.position[0] += length_pos_translation

        if (random() <= mutation_rate):
            if (self.corners[0][1] == self.cluster_corners[0][1]): # Lowest element of cluster, so we can start at 0
                min_y = 0
            else: # Otherwise add the difference
                min_y = self.corners[0][1] - self.cluster_corners[0][1]
            if (self.corners[-1][1] == self.cluster_corners[-1][1]): # Highest element of cluster, so we can go til max_width - self.width
                max_y = max_width - self.width
            else: # Otherwise substract the difference
                max_y = max_width - self.width - (self.cluster_corners[-1][1] - self.corners[-1][1])
            width_pos = randint(min_y, max_y)
            #width_pos = int(round(gauss(width_pos, ga_mutation_sigma)))
            width_pos_translation = width_pos - self.position[1]
            for element in self.cluster:
                if (random() <= ga_cluster_rate):
                    element.position[1] += width_pos_translation
        
        # shape
        if (random() <= mutation_rate):
            self.swap_width_length()
        
        # exotic "jump-over" mutation
        if ((len(self.cluster) != 0) and (random() <= mutation_rate)):
            element = choice(self.cluster)
            self.jump_over(element)

        self.position = [length_pos, width_pos]
        
        # Finally, let's reset road and path access variables (necessary between two generations)
        self.road_access = False
        self.path_access = False
        
        # Update at the end to make sure all parameters are correct
        self.update()
        for element in self.cluster:
            element.update()
        
        # And let's reset the cluster as well
        self.cluster = set()

