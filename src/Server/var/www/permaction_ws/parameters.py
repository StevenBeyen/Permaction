#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals



"""Parameters for the REST API (Flask routes)."""
# Routes
root_route = '/'
login_route = '/login'
signup_route = '/signup'
logout_route = '/logout'
physical_elements_route = '/physical_elements'
binary_interactions_route = '/binary_interactions'
placement_request_route = '/placement_request'
# Route methods
POST_method = 'POST'
# Main website URI
permaction_uri = 'https://permaction.com'
# JSON user creation/login tags
username_tag = 'username'
password_tag = 'password'
id_locale_tag = 'id_locale'
email_tag = 'email'
private_tag = 'private'
professional_tag = 'professional'
# JSON physical elements tags
physical_elements_tag = 'physical_elements'
id_tag = 'id'
name_tag = 'name'
category_tag = 'category'
terrain_flattening_tag = 'terrain_flattening'
# JSON binary interactions tags
binary_interactions_tag = 'binary_interactions'
# JSON placement request tags
terrain_data_tag = 'terrain_data'
elements_data_tag = 'elements_data'
terrain_line_data_tag = 'terrain_line_data'
index_tag = 'index'
counter_tag = 'counter'
# JSON placement reply tags
ai_fitness_tag = 'fitness'
ai_result_tag = 'result'
ai_optimum_tag = 'optimum'
ai_x_tag = 'x'
ai_y_tag = 'y'
ai_id_tag = 'id'
ai_coordinates_tag = 'coordinates'
# Routes reply messages
user_created_message = '{} successfully created!'
logout_message = 'Successfully logged out!'
error_evaluating_message = 'Error evaluating.'
error_converting_message = 'Error converting.'
error_verifying_message = 'Error verifying.'
not_enough_space_message = 'Not enough space.'



"""Parameters for the Database model."""
# Locale table
locale_table = 'locale'
locale_table_id = 'locale.id'
# Category table
category_table = 'category'
category_table_id = 'category.id'
# Element table
element_table = 'element'
element_table_id = 'element.id'
# User table
user_table = 'user'
user_table_id = 'user.id'
sha256 = 'sha256'
utf8 = 'utf-8'
# User placement request table
user_placement_request_table = 'user_placement_request'
# Binary interaction table
binary_interaction_table = 'binary_interaction'
# Interaction type table
interaction_type_table = 'interaction_type'
interaction_type_table_id = 'interaction_type.id'
# Ternary interaction table
ternary_interaction_table = 'ternary_interaction'



"""Parameters for the data preprocessing."""
# DB variable width/size name
var_width_size = 'var.'
# Locale IDs
locale_en = 1
locale_fr = 2
# Terrain heights tags
low_tag = 'low'
midheight_tag = 'mid-height'
heights_tag = 'heights'
# EN terrain heights IDs
en_low_id = 21
en_midheight_id = 23
en_heights_id = 18
# FR terrain heights IDs
fr_low_id = 61
fr_midheight_id = 63
fr_heights_id = 58
# Heights mapping for generic use
heights_mapping = {}
heights_mapping[locale_en] = {low_tag: en_low_id, midheight_tag: en_midheight_id, heights_tag: en_heights_id}
heights_mapping[locale_fr] = {low_tag: fr_low_id, midheight_tag: fr_midheight_id, heights_tag: fr_heights_id}
# Ternary interaction types ID mapping
higher_interaction_type = 'higher'
south_interaction_type = 'south'
ternary_interactions_mapping = {1: higher_interaction_type, 2: south_interaction_type}
# Road & path tags
road_tag = 'road'
path_tag = 'path'
# EN road & path IDs
en_road_id = 28
en_path_id = 25
# FR road & path IDs
fr_road_id = 68
fr_path_id = 65
# Roads mapping for generic use
road_path_ids = [en_road_id, en_path_id, fr_road_id, fr_path_id]
road_ids = [en_road_id, fr_road_id]
path_ids = [en_path_id, fr_path_id]
roads_mapping = {}
roads_mapping[locale_en] = {road_tag: en_road_id, path_tag: en_path_id}
roads_mapping[locale_fr] = {road_tag: fr_road_id, path_tag: fr_path_id}



"""Parameters for the AI."""
# AI number of parallel runs for every user request
ai_nb_placement_requests = 1
# Reserved height to rule out parts of given terrain
unallowed_height = -999.0
# Square size received from terrain coordinates (in meters)
square_size = 1
# Interaction added value to fitness for biotope intersections 
biotope_intersection_added_value = 9
# Ternary interaction added value to fitness
ternary_interaction_added_value = 9
# Biotope value that cannot be combined with any other
biotope_killer_value = 0
# Biotope value that can be combined with any other (except 0)
biotope_enhancer_value = 10
# Max filled terrain ratio, this is an approximation so in reality we will be somewhere between +-5%
max_filled_terrain_ratio = 0.8
# golden ratio values for rectangle elements, with deviation from 1.618... less than 2 percent (except 6 and 15)
phi_ratio_values = {6: (2, 3), 15: (3, 5), 40: (5, 8), 104: (8, 13), 160: (10, 16), 198: (11, 18), 273: (13, 21), 322: (14, 23), 360: (15, 24), 416: (16, 26), 459: (17, 27), 476: (17, 28), 522: (18, 29), 589: (19, 31), 640: (20, 32), 660: (20, 33), 714: (21, 34), 770: (22, 35), 792: (22, 36), 851: (23, 37), 936: (24, 39), 1000: (25, 40), 1025: (25, 41), 1092: (26, 42), 1161: (27, 43), 1188: (27, 44), 1260: (28, 45), 1288: (28, 46), 1334: (29, 46), 1363: (29, 47), 1440: (30, 48), 1470: (30, 49), 1550: (31, 50), 1581: (31, 51), 1632: (32, 51), 1664: (32, 52), 1749: (33, 53), 1782: (33, 54), 1836: (34, 54), 1870: (34, 55), 1904: (34, 56), 1960: (35, 56), 1995: (35, 57), 2088: (36, 58), 2124: (36, 59), 2183: (37, 59), 2220: (37, 60), 2257: (37, 61), 2318: (38, 61), 2356: (38, 62), 2418: (39, 62), 2457: (39, 63), 2496: (39, 64), 2560: (40, 64), 2600: (40, 65), 2640: (40, 66), 2706: (41, 66), 2747: (41, 67), 2814: (42, 67), 2856: (42, 68), 2898: (42, 69), 2967: (43, 69), 3010: (43, 70), 3080: (44, 70), 3124: (44, 71), 3168: (44, 72), 3240: (45, 72), 3285: (45, 73), 3330: (45, 74), 3358: (46, 73), 3404: (46, 74), 3450: (46, 75), 3525: (47, 75), 3572: (47, 76), 3619: (47, 77), 3696: (48, 77), 3744: (48, 78), 3792: (48, 79), 3822: (49, 78), 3871: (49, 79), 3920: (49, 80), 4000: (50, 80), 4050: (50, 81), 4100: (50, 82), 4131: (51, 81), 4182: (51, 82), 4233: (51, 83), 4284: (51, 84), 4316: (52, 83), 4368: (52, 84), 4420: (52, 85), 4505: (53, 85), 4558: (53, 86), 4611: (53, 87), 4644: (54, 86), 4698: (54, 87), 4752: (54, 88), 4806: (54, 89), 4840: (55, 88), 4895: (55, 89), 4950: (55, 90), 4984: (56, 89)}
# Minimal element size
phi_min_value = 6
# Genetic algorithm population size
ga_population_size = 100
# Genetic algorithm stop condition: max. number of generations without improvement
ga_stop_nb_gen_no_improvement = 100
# Genetic algorithm tournament rate: probability of best individual to win the fight
ga_tournament_rate = 0.95
# Genetic algorithm mutation rate: probability of mutation on shape and position
ga_mutation_rate = 0.05
# Genetic algorithm elitism
ga_elitism = True
# Maximum manhattan distance for neighbour computation, if you set this to 1 all elements have to touch each other
# in order for their interaction value to be added. Recommended value is at least 2.
max_manhattan_distance = 3
# Exception for not enough space on the terrain to fit all requested elements
class NotEnoughSpaceException(Exception):
    pass
# Max length of linear elements
max_linear_element_ratio = 0.33
# Road and path length generation multiple
road_path_length_multiple = 4
# Maximum distance for road and path neighbour computation.
road_path_max_manhattan_distance = 1

