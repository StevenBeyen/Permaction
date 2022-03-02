#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from threading import Thread
from random import uniform, randint, random

from .element import RectangleElement, LinearElement, RoadPathElement, ZoneElement

from parameters import *

class Individual(Thread):

    """Genetic algorithm base class: placement of elements on given terrain.
    Evolution of individual based on given rules of selection, mutation and crossover."""

    def __init__(self, elements, terrain, fitness_dict, ternary_fitness_dict, fixed_element_indexes):
        Thread.__init__(self)
        self.elements = [element.copy() for element in elements]
        self.fixed_element_indexes = fixed_element_indexes
        self.terrain = terrain
        self.terrain_length = len(self.terrain)
        self.terrain_width = len(self.terrain[0])
        self.road_connected_elements = []
        self.path_connected_elements = []
        self.fitness_dict = fitness_dict
        self.ternary_fitness_dict = ternary_fitness_dict
        self.fitness_malus = 0
        self.fitness = 0
        self.interaction_data = []
        self.horizontal_init = True
    
    def copy(self):
        copy = Individual(self.elements, self.terrain, self.fitness_dict, self.ternary_fitness_dict, self.fixed_element_indexes)
        return copy
    
    def init_elements(self):
        global min_square_deviation_ratio
        global max_square_deviation_ratio
        global random_shape_precision
        for i in range(len(self.elements)):
            if (i not in self.fixed_element_indexes):
                e = self.elements[i]
                if (isinstance(e, RectangleElement)):
                    # Initialize position on map
                    e.set_position([randint(0, self.terrain_length - e.length), randint(0, self.terrain_width - e.width)])
                    e.update()
                elif (isinstance(e, LinearElement)):
                    # Several things to instanciate: direction (horizontal/vertical), position and length
                    # Let's start with the direction:
                    e.horizontal = self.horizontal_init
                    # Swapping horizontal and vertical linear elements, alternates and reduces search space.
                    self.horizontal_init = not self.horizontal_init
                    # Now let's put some random length
                    if (e.horizontal):
                        if (e.id in road_path_ids):
                            e.set_length(road_path_length_multiple * randint(1, round(self.terrain_length*max_linear_element_ratio/road_path_length_multiple)))
                        else:
                            e.set_length(linear_element_length_multiple * randint(1, round(self.terrain_length*max_linear_element_ratio/linear_element_length_multiple)))
                    else: # Vertical
                        if (e.id in road_path_ids):
                            e.set_length(road_path_length_multiple * randint(1, round(self.terrain_width*max_linear_element_ratio/road_path_length_multiple)))
                        else:
                            e.set_length(linear_element_length_multiple * randint(1, round(self.terrain_width*max_linear_element_ratio/linear_element_length_multiple)))
                    # Finally, let's put it at a random postion where it doesn't exit the terrain boundaries
                    if (e.horizontal):
                        e.set_position([randint(0, self.terrain_length - e.length), randint(0, self.terrain_width - e.width)])
                    else: # Vertical
                        e.set_position([randint(0, self.terrain_length - e.width), randint(0, self.terrain_width - e.length)])
                    e.update()
                else:
                    raise ValueError
    
    def update_fitness_malus(self):
        global biotope_enhancer_value, epsilon
        self.fitness_malus = 0
        for i in range(len(self.elements) - 1):
            malus = self.elements[i].out_of_terrain_level(self.terrain, self.terrain_length, self.terrain_width)
            if (malus != 0):
                self.fitness_malus += biotope_enhancer_value + epsilon * malus
            for j in range(i + 1, len(self.elements)):
                malus = self.elements[i].intersection_level(self.elements[j])
                if (malus != 0):
                    self.fitness_malus += biotope_enhancer_value + epsilon * malus
        # Don't forget last element!
        malus = self.elements[-1].out_of_terrain_level(self.terrain, self.terrain_length, self.terrain_width)
        if (malus != 0):
            self.fitness_malus += biotope_enhancer_value + epsilon * malus
    
    def update_fitness(self):
        # Computing fitness based on neighbouring and database binary interactions
        self.fitness = 0
        self.interaction_data = []
        for i in range(len(self.elements) - 1):
            for j in range(i + 1, len(self.elements)):
                # Fitness dict
                e1 = self.elements[i]
                e2 = self.elements[j]
                try:
                    fitness = self.fitness_dict[(e1.id, e2.id)]
                    added_fitness = e1.fitness_level(e2, fitness)
                    self.fitness += added_fitness
                    interaction = {'e1': e1.id, 'e2': e2.id, 'fitness': added_fitness}
                    if (not isinstance(e1, ZoneElement) and not isinstance(e2, ZoneElement)):
                        interaction['distance'] = e1.distance(e2)
                    self.interaction_data.append(interaction)
                except KeyError:
                    pass
                # Ternary fitness dict
                key1 = (e1.id, e2.id)
                key2 = (e2.id, e1.id)
                added_fitness = 0
                if key1 in self.ternary_fitness_dict:
                    added_fitness = e1.ternary_fitness_level(e2, self.ternary_fitness_dict[key1], self.terrain)
                elif key2 in self.ternary_fitness_dict:
                    added_fitness = e2.ternary_fitness_level(e1, self.ternary_fitness_dict[key2], self.terrain)
                self.fitness += added_fitness
        # Substracting the malus, computed with intersections and out of terrain boundaries
        self.fitness -= self.fitness_malus
    
    def fight(self, individual):
        global ga_tournament_rate
        if (random() > ga_tournament_rate): # Lowest fitness wins
            return (self if self.fitness < individual.fitness else individual)
        else: # Highest fitness wins
            return (self if self.fitness >= individual.fitness else individual)
    
    def breed(self, individual):
        ind1_elements = []
        ind2_elements = []
        for i in range(len(self.elements)):
            [e1, e2] = self.elements[i].breed(individual.elements[i])
            if (random() >= 0.5):
                ind1_elements += [e1]
                ind2_elements += [e2]
            else:
                ind1_elements += [e2]
                ind2_elements += [e1]
        ind1 = Individual(ind1_elements, self.terrain, self.fitness_dict, self.ternary_fitness_dict, self.fixed_element_indexes)
        ind2 = Individual(ind2_elements, self.terrain, self.fitness_dict, self.ternary_fitness_dict, self.fixed_element_indexes)
        return [ind1, ind2]
    
    def apply_mutation(self, mutation_rate):
        for i in range(len(self.elements)):
            self.elements[i].apply_mutation(mutation_rate, self.terrain_length, self.terrain_width)
    
    def update_road_path_sublists(self):
        for element in self.elements:
            if (element.road_connected_element):
                self.road_connected_elements.append(element)
            if (element.path_connected_element):
                self.path_connected_elements.append(element)
    
    def get_northernmost_element(self, elements):
        northernmost_element = elements[0]
        for element in elements[1:]:
            if (element.get_corners()[-1][1] > northernmost_element.get_corners()[-1][1]):
                northernmost_element = element
        return northernmost_element
    
    def get_easternmost_element(self, elements):
        easternmost_element = elements[0]
        for element in elements[1:]:
            if (element.get_corners()[0][0] > easternmost_element.get_corners()[0][0]):
                easternmost_element = element
        return easternmost_element
    
    def get_westernmost_element(self, elements):
        westernmost_element = elements[0]
        for element in elements[1:]:
            if (element.get_corners()[-1][0] < westernmost_element.get_corners()[-1][0]):
                westernmost_element = element
        return westernmost_element
    
    def get_southernmost_element(self, elements):
        southernmost_element = elements[0]
        for element in elements[1:]:
            if (element.get_corners()[0][1] > southernmost_element.get_corners()[0][1]):
                southernmost_element = element
        return southernmost_element
    
    def init_thread(self):
        Thread.__init__(self)
    
    def run(self):
        self.update_fitness_malus()
        self.update_fitness()
    
    def is_equal(self, individual):
        for i in range(len(self.elements)):
            if (self.elements[i] != individual.elements[i]):
                return False
        return True
    
    def road_postprocessing(self, road_element):
        pass
    
    def path_postprocessing(self, path_element):
        global path_max_manhattan_distance
        path_elements = []
        # No need for paths if there are not at least two elements to connect!
        if (len(self.path_connected_elements) < 2):
            pass
        else:
            # First, let's connect the westernmost element with the one closest to it.
            e1 = self.get_westernmost_element(self.path_connected_elements)
            self.path_connected_elements.remove(e1)
            self.path_connected_elements.insert(0, e1)
            e2 = self.path_connected_elements[1]
            min_distance = e1.distance(e2)
            for i in range(2, len(self.path_connected_elements)):
                distance = e2.distance(self.path_connected_elements[i])
                if (distance < min_distance):
                    e2 = self.path_connected_elements[i]
                    min_distance = distance
            self.path_connected_elements.remove(e2)
            self.path_connected_elements.insert(1, e2)
            # Now that we have our two elements, let's connect them!
            path_sections = []
            first_path_section = path_element.copy()
            second_path_section = None
            if (e1.horizontal_intersection(e2, path_max_manhattan_distance)):
                first_path_section.connect(e1, e2, horizontal = True)
            elif (e1.vertical_intersection(e2, path_max_manhattan_distance)):
                first_path_section.connect(e1, e2, horizontal = False)
            else: # No possible straight line so we need two path sections
                swapped = first_path_section.connect(e1, e2, horizontal = True)
                second_path_section = path_element.copy()
                if (swapped):
                    second_path_section.connect(e1, first_path_section, horizontal = False)
                else:
                    second_path_section.connect(e2, first_path_section, horizontal = False)
            if (first_path_section.length != 0):
                path_sections.append(first_path_section)
            if (second_path_section is not None and second_path_section.length != 0):
                path_sections.append(second_path_section)
            # And now we check for intersection with other elements, and we add path sections to go around them
            for element in self.elements:
                for path in path_sections:
                    if not (isinstance(element, ZoneElement)):
                        if (path.intersection_level(element) > 0):
                            # First step: splitting
                            path_split = path.split(element)
                            if (path.length == 0):
                                path_sections.remove(path)
                            if (path_split.length != 0):
                                path_sections.append(path_split)
                            # Second step: going around element
                            connect_around_subpaths = path.connect_around(path_split, element)
                            # And finally we can add these path sections to the list
                            for subpath in connect_around_subpaths:
                                if (subpath.length != 0):
                                    path_sections.append(subpath)
            # Add all path sections to elements
            self.elements += path_sections
            # Whooops, this should be looped with all elements!
            # And finally, check for intersections with roads, and simply reduce or remove path sections that intersect with roads (roads > paths)
        
    def to_json(self):
        reply = []
        for i in range(len(self.elements)):
            if (i not in self.fixed_element_indexes):
                reply.append(self.elements[i].to_json())
        return reply

