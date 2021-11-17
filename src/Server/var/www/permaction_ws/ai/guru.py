#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from rest_api.models import BinaryInteraction, TernaryInteraction, Element, Category
from sqlalchemy.orm.exc import NoResultFound#, MultipleResultsFound Should not be necessary at the moment
from random import choice, randint, random

from .individual import Individual
from parameters import *

ai_search_stop = False

class Guru:
    
    """Genetic algorithm helper class to track the generations of individuals and manage stop condition."""
    
    def __init__(self, id_locale, elements, terrain):
        global ai_search_stop
        ai_search_stop = False
        self.id_locale = id_locale
        self.elements = elements
        self.terrain = terrain
        self.fitness_dict = {}
        self.ternary_fitness_dict = {}
        self.road_sections = []
        self.multiple_roads = False
        self.path_sections = []
        self.multiple_paths = False
        self.max_fitness = 0
        self.fixed_element_indexes = []
        self.terrain_element_ids = []
        self.population = []
        self.best_individual = None
        self.best_fitness = -1
        self.stuck_counter = 0
        self.stop_condition = False

    def run(self):
        self.init_fixed_element_indexes()
        self.fitness_preprocessing()
        self.add_road_path_sections()
        self.init_terrain_element_ids()
        self.create_population()
        self.genetic_algorithm_routine()
        print("Best individual fitness:", self.best_fitness)

    def fitness_preprocessing(self):
        # Getting all binary interactions into an easy-to-use dictionary: (e1_id, e2_id) => fitness
        self.init_fitness_dict()
        self.init_ternary_fitness_dict()
        # Computing maximum possible fitness 
        self.compute_max_fitness()
        print("Max fitness:", self.max_fitness)
        
    def init_fixed_element_indexes(self):
        for i in range(len(self.elements)):
            if (self.elements[i].is_fixed()):
                self.fixed_element_indexes += [i]
    
    def init_fitness_dict(self):
        # Step 1: binary interactions
        binary_interactions = BinaryInteraction.query.filter_by(id_locale=self.id_locale).all()
        for interaction in binary_interactions:
            key1 = (interaction.element1_id, interaction.element2_id)
            if key1 in self.fitness_dict:
                self.fitness_dict[key1] += interaction.interaction_level
            else:
                self.fitness_dict[key1] = interaction.interaction_level
            key2 = (interaction.element2_id, interaction.element1_id)
            if key2 in self.fitness_dict:
                self.fitness_dict[key2] += interaction.interaction_level
            else:
                self.fitness_dict[key2] = interaction.interaction_level
        # Step 2: biotope intersections
        for i in range(len(self.elements) - 1):
            for j in range(i + 1, len(self.elements)):
                e1 = self.elements[i]
                e2 = self.elements[j]
                biotope_intersection = e1.biotope_intersection(e2)
                key1 = (e1.id, e2.id)
                if key1 in self.fitness_dict:
                    self.fitness_dict[key1] += biotope_intersection
                else:
                    self.fitness_dict[key1] = biotope_intersection
                key2 = (e2.id, e1.id)
                if key2 in self.fitness_dict:
                    self.fitness_dict[key2] += biotope_intersection
                else:
                    self.fitness_dict[key2] = biotope_intersection
        # Step 3: category interactions to element interactions
        for i in range(len(self.elements) - 1):
            for j in range(i + 1, len(self.elements)):
                e1 = self.elements[i]
                e2 = self.elements[j]
                element1_category_id = Element.query.filter_by(id=e1.id).one().category_id
                element1_category_element_id = Category.query.filter_by(id=element1_category_id).one().element_id
                element2_category_id = Element.query.filter_by(id=e2.id).one().category_id
                element2_category_element_id = Category.query.filter_by(id=element2_category_id).one().element_id
                added_value = 0
                try:
                    added_value += self.fitness_dict[(element1_category_element_id, e2.id)]
                except KeyError:
                    pass
                try:
                    added_value += self.fitness_dict[(element2_category_element_id, e1.id)]
                except KeyError:
                    pass
                try:
                    added_value += self.fitness_dict[(element1_category_element_id, element2_category_element_id)]
                except KeyError:
                    pass
                if (added_value != 0):
                    key1 = (e1.id, e2.id)
                    if key1 in self.fitness_dict:
                        self.fitness_dict[key1] += added_value
                    else:
                        self.fitness_dict[key1] = added_value
                    key2 = (e2.id, e1.id)
                    if key2 in self.fitness_dict:
                        self.fitness_dict[key2] += added_value
                    else:
                        self.fitness_dict[key2] = added_value
        # Step 4: connect road and path parts with themselves
        for road_path_id in road_path_ids:
            self.fitness_dict[(road_path_id, road_path_id)] = biotope_enhancer_value
    
    def init_ternary_fitness_dict(self):
        ternary_interactions = TernaryInteraction.query.filter_by(id_locale=self.id_locale).all()
        for interaction in ternary_interactions:
            key = (interaction.element1_id, interaction.element2_id)
            if key in self.ternary_fitness_dict:
                self.ternary_fitness_dict[key].append(interaction.interaction_type_id)
            else:
                self.ternary_fitness_dict[key] = [interaction.interaction_type_id]

    def compute_max_fitness(self):
        for i in range(len(self.elements) - 1):
            for j in range(i + 1, len(self.elements)):
                e1 = self.elements[i]
                e2 = self.elements[j]
                try:
                    fitness = self.fitness_dict[(e1.id, e2.id)]
                    if (fitness > 0):
                        self.max_fitness += fitness
                        if (e1.id in road_ids):
                            if (not self.road_sections):
                                self.road_sections = [e1]
                            else:
                                self.road_sections += [e1.copy()]
                                self.multiple_roads = True
                                self.max_fitness += fitness
                        elif (e2.id in road_ids):
                            if (not self.road_sections):
                                self.road_sections = [e2]
                            else:
                                self.road_sections += [e2.copy()]
                                self.multiple_roads = True
                                self.max_fitness += fitness
                        if (e1.id in path_ids):
                            if (not self.path_sections):
                                self.path_sections = [e1]
                            else:
                                self.path_sections += [e1.copy()]
                                self.multiple_paths = True
                                self.max_fitness += fitness
                        elif (e2.id in path_ids):
                            if (not self.path_sections):
                                self.path_sections = [e2]
                            else:
                                self.path_sections += [e2.copy()]
                                self.multiple_paths = True
                                self.max_fitness += fitness
                except KeyError:
                    pass
    
    def add_road_path_sections(self):
        # Ignoring first element since it's already in list of elements, only adding duplicates
        for road in self.road_sections[1:]:
            self.elements.append(road)
        for path in self.path_sections[1:]:
            self.elements.append(path)

    def init_terrain_element_ids(self):
        global unallowed_height
        for line in self.terrain:
            self.terrain_element_ids += [[None if i != unallowed_height else i for i in line]]
        for i in self.fixed_element_indexes:
            for coordinate in self.elements[i].get_used_coordinates():
                self.terrain_element_ids[coordinate[0]][coordinate[1]] = i

    def create_population(self):
        global ga_population_size
        for i in range(ga_population_size):
            individual = Individual(self.elements, self.terrain, self.fitness_dict, self.ternary_fitness_dict, self.fixed_element_indexes, self.terrain_element_ids, self.multiple_roads, self.multiple_paths)
            individual.init_elements()
            self.population += [individual]
    
    def update_best_fitness(self, individual):
        self.best_individual = individual
        self.best_fitness = individual.fitness
        self.stuck_counter = 0
    
    def update_stop_condition(self):
        global ga_stop_nb_gen_no_improvement, ai_search_stop
        if (self.best_fitness >= self.max_fitness):
            ai_search_stop = True
        self.stop_condition = (ai_search_stop or (self.stuck_counter == ga_stop_nb_gen_no_improvement))
    
    def create_next_generation(self):
        global ga_elitism, ga_population_size
        next_generation = []
        parents = [self.best_individual] if (ga_elitism) else []
        # len/2 parents selected by tournament, they will make half of the new population
        for i in range(int(ga_population_size/2)):
            new_individual = choice(self.population).fight(choice(self.population)).copy()
            parents += [new_individual]
            next_generation += [new_individual]
        # len/4 couples selected at random that will cross their genes to create two new individuals
        for i in range(int(ga_population_size/4)):
            parent1 = choice(parents)
            parent2 = choice(parents)
            next_generation += parent1.breed(parent2)
        # Applying mutations to the new population
        for individual in next_generation:
            individual.apply_mutation()
        # randomly replacing one individual of next generation by best individual if elitism is active
        if (ga_elitism):
            self.best_individual.init_thread()
            next_generation.append(self.best_individual)
        # Finally, the next generation becomes the current one
        self.population = next_generation
    
    def genetic_algorithm_routine(self):
        while (not self.stop_condition):
            self.stuck_counter += 1
            if (self.stuck_counter == 1):
                print(self.best_fitness)
            # TODO Check speed difference, but it seems that too many threads are actually slowing things down instead of speeding up...
            #for individual in self.population:
            #    individual.start()
            for individual in self.population:
                individual.run()
            #    individual.join()
                if (individual.fitness > self.best_fitness):
                    self.update_best_fitness(individual)
            self.update_stop_condition()
            self.create_next_generation()
    
    def make_response(self):
        global ai_fitness_tag, ai_result_tag, ai_optimum_tag, ai_yes_tag, ai_no_tag
        response = {ai_result_tag: self.best_individual.to_json(), ai_fitness_tag: self.best_fitness, ai_optimum_tag: 1 if self.best_fitness == self.max_fitness else 0}
        return response

