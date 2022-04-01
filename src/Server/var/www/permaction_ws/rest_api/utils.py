#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from flask import abort
from flask_login import current_user
from datetime import datetime as dt
from .models import UserPlacementRequest
from flaskthreads import AppContextThread
from queue import Queue

from ai import AI
from parameters import *


user_placement_ai_responses = None
def user_placement_ai(request_id, id_locale):
    global user_placement_ai_responses
    try:
        ai = AI(request_id, id_locale)
        ai.run()
        user_placement_ai_responses.put(ai.make_response())
    except NotEnoughSpaceException:
        user_placement_ai_responses.put(None)


def validate_placement_request(placement_request_data):
    global terrain_data_tag, elements_data_tag, error_evaluating_message, error_converting_message, error_verifying_message
    try:
        id_locale = placement_request_data[id_locale_tag]
        terrain_data = placement_request_data[terrain_data_tag]
        elements_data = placement_request_data[elements_data_tag]
    except:
        abort(400, error_evaluating_message) # bad syntax for terrain or elements data
    # Now we create the placement request in the database, and make a preprocessing to convert the data to a format more suitable to its handling
    #user_placement_request = UserPlacementRequest(user_id = current_user.id, terrain_data = '', elements_data = '', requested = dt.now())
    # TODO remove temporary solution when login will work correctly...
    user_placement_request = UserPlacementRequest(user_id=1, terrain_data = '', elements_data = '', requested = dt.now())
    try:
        terrain_data = user_placement_request.terrain_preprocessing(terrain_data)
        elements_data = user_placement_request.elements_preprocessing(elements_data)
    except:
        abort(400, error_converting_message) # bad syntax for terrain or elements data
    # Finally, we check the data consistency
    if (user_placement_request.check_terrain_data(terrain_data) and user_placement_request.check_elements_data(id_locale, elements_data, terrain_data)):
        user_placement_request.terrain_data = str(terrain_data)
        user_placement_request.elements_data = str(elements_data)
    else:
        abort(400, error_verifying_message) # bad structure for terrain or elements data
    return user_placement_request


def start_placement_request(user_placement_request, id_locale):
    global ai_nb_placement_requests, ai_fitness_tag, user_placement_ai_responses, ai_optimum_tag, not_enough_space_message
    """user_placement_ais = []
    user_placement_ai_responses = Queue()
    for i in range(ai_nb_placement_requests):
        #user_placement_ais += [AppContextThread(target=user_placement_ai, args=(user_placement_request.id,current_user.id_locale,))]
    for ai in user_placement_ais:
        ai.start()
    # Join on all threads and return best result
    best_result = None
    for ai in user_placement_ais:
        result = user_placement_ai_responses.get()
        if (result is None):
            abort(409, not_enough_space_message) # Not enough space to fit all elements inside the terrain. 
        elif (result[ai_optimum_tag]):
            best_result = result
            break
        elif ((best_result is None) or (result[ai_fitness_tag] > best_result[ai_fitness_tag])):
            best_result = result
    return best_result"""
    ai = AI(user_placement_request.id, id_locale)
    ai.run()
    return ai.make_response()

