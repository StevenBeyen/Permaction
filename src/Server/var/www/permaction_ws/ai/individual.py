#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from threading import Thread
from random import uniform, randint, random

from .element import RectangleElement, LinearElement, RoadPathElement

from parameters import *

class Individual(Thread):

    """Genetic algorithm base class: placement of elements on given terrain.
    Evolution of individual based on given rules of selection, mutation and crossover."""

    def __init__(self, elements, terrain, fitness_dict, ternary_fitness_dict, fixed_element_indexes, init_terrain_element_ids, multiple_roads, multiple_paths):
        Thread.__init__(self)
        self.elements = [element.copy() for element in elements]
        self.fixed_element_indexes = fixed_element_indexes
        self.terrain = terrain
        self.terrain_length = len(self.terrain)
        self.terrain_width = len(self.terrain[0])
        self.init_terrain_element_ids = init_terrain_element_ids
        self.fitness_dict = fitness_dict
        self.ternary_fitness_dict = ternary_fitness_dict
        self.multiple_roads = multiple_roads
        self.multiple_paths = multiple_paths
        self.terrain_element_ids = None
        self.nb_terrain_overlaps = 0
        self.nb_disconnected_roads_paths = 0
        self.fitness = 0
    
    def copy(self):
        copy = Individual(self.elements, self.terrain, self.fitness_dict, self.ternary_fitness_dict, self.fixed_element_indexes, self.init_terrain_element_ids, self.multiple_roads, self.multiple_paths)
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
                    e.set_position((randint(0, self.terrain_length - e.length), randint(0, self.terrain_width - e.width)))
                    e.update()
                elif (isinstance(e, LinearElement)):
                    # Several things to instanciate: direction (horizontal/vertical), position and length
                    # Let's start with the direction:
                    if (random() > 0.5):
                        e.horizontal = True
                    else:
                        e.horizontal = False
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
                        e.set_position((randint(0, self.terrain_length - e.length), randint(0, self.terrain_width - e.width)))
                    else: # Vertical
                        e.set_position((randint(0, self.terrain_length - e.width), randint(0, self.terrain_width - e.length)))
                    e.update()
                else:
                    raise ValueError
    
    def update_terrain_element_ids(self):
        self.terrain_element_ids = [list(line) for line in self.init_terrain_element_ids]
        self.nb_terrain_overlaps = 0
        for i in range(len(self.elements)):
            if (i not in self.fixed_element_indexes):
                e = self.elements[i]
                for coordinate in e.get_used_coordinates():
                    try:
                        if (self.terrain_element_ids[coordinate[0]][coordinate[1]] is None):
                            self.terrain_element_ids[coordinate[0]][coordinate[1]] = e.id
                        else: # Overlap between two elements
                            self.nb_terrain_overlaps += 1
                    except IndexError: # Or out of range !
                        self.nb_terrain_overlaps += 1
    
    def update_fitness(self):
        # Computing fitness based on neighbouring and database binary interactions
        self.fitness = 0
        for i in range(len(self.elements) - 1):
            for j in range(i + 1, len(self.elements)):
                # Fitness dict
                e1 = self.elements[i]
                e2 = self.elements[j]
                try:
                    fitness = self.fitness_dict[(e1.id, e2.id)]
                    self.fitness += e1.fitness_level(e2, fitness)
                except KeyError:
                    pass
                # Ternary fitness dict
                key1 = (e1.id, e2.id)
                key2 = (e2.id, e1.id)
                if key1 in self.ternary_fitness_dict:
                    self.fitness += e1.ternary_fitness_level(e2, self.ternary_fitness_dict[key1], self.terrain)
                elif key2 in self.ternary_fitness_dict:
                    self.fitness += e2.ternary_fitness_level(e1, self.ternary_fitness_dict[key2], self.terrain)
        # Disconnected roads and paths counter
        for e in self.elements:
            if (isinstance(e, RoadPathElement) and e.disconnected_road_path()):
                if ((self.multiple_roads and e.id in road_ids) or (self.multiple_paths and e.id in path_ids)):
                    self.nb_disconnected_roads_paths += 1
        # Dividing the fitness by the number of element overlaps and disconnected roads and paths +1 to prevent them.
        self.fitness = self.fitness / (self.nb_terrain_overlaps + self.nb_disconnected_roads_paths + 1)
    
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
            # New individual 1
            if (random() > 0.5):
                ind1_elements += [self.elements[i]]
            else:
                ind1_elements += [individual.elements[i]]
            # New individual 2
            if (random() > 0.5):
                ind2_elements += [individual.elements[i]]
            else:
                ind2_elements += [self.elements[i]]
        ind1 = Individual(ind1_elements, self.terrain, self.fitness_dict, self.ternary_fitness_dict, self.fixed_element_indexes, self.init_terrain_element_ids, self.multiple_roads, self.multiple_paths)
        ind2 = Individual(ind2_elements, self.terrain, self.fitness_dict, self.ternary_fitness_dict, self.fixed_element_indexes, self.init_terrain_element_ids, self.multiple_roads, self.multiple_paths)
        return [ind1, ind2]
    
    def apply_mutation(self):
        for i in range(len(self.elements)):
            if (i not in self.fixed_element_indexes):
                self.elements[i].apply_mutation(self.terrain_length, self.terrain_width)
    
    def similarity(self, individual):
        similarity = 0
        counter = 0
        for i in range(len(self.elements)):
            if (i not in self.fixed_element_indexes):
                 similarity += self.elements[i].similarity(individual.elements[i])
                 counter += 1
        return (similarity / counter)
    
    def init_thread(self):
        Thread.__init__(self)
    
    def run(self):
        self.update_terrain_element_ids()
        self.update_fitness()
    
    def is_equal(self, individual):
        for i in range(len(self.elements)):
            if (self.elements[i] != individual.elements[i]):
                return False
        return True
    
    def __repr__(self):
        return(str(self.terrain_element_ids))
    
    def to_json(self):
        reply = []
        for i in range(len(self.elements)):
            if (i not in self.fixed_element_indexes):
                reply.append(self.elements[i].to_json())
        return reply

