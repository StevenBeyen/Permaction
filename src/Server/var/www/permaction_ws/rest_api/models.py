#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from os import urandom
from hashlib import pbkdf2_hmac
from flask_login import UserMixin, current_user

from . import db
from parameters import *

class Locale(db.Model):
    """Model for database locale (language option)."""
    
    global locale_table
    
    __tablename__ = locale_table
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    locale = db.Column(db.Text, nullable=False)
    language = db.Column(db.Text, nullable=False)
    
    def __repr__(self):
        return '<Locale {}>'.format(self.language)

class Category(db.Model):
    """Model for database elements' categories."""
    
    global category_table, locale_table_id
    
    __tablename__ = category_table
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    id_locale = db.Column(db.Integer, db.ForeignKey(locale_table_id), nullable=False)
    name = db.Column(db.Text, nullable=False)
    physical_category = db.Column(db.Boolean, nullable=False)
    element_id = db.Column(db.Integer)
    terrain_flattening = db.Column(db.Integer, nullable=False)
    
    def __repr__(self):
        return '<Category {}>'.format(self.name)

class Element(db.Model):
    """Model for database elements."""
    
    global element_table, locale_table_id, category_table_id, id_tag, name_tag, category_tag
    
    __tablename__ = element_table
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    id_locale = db.Column(db.Integer, db.ForeignKey(locale_table_id), nullable=False)
    name = db.Column(db.Text, nullable=False)
    category_id = db.Column(db.Integer, db.ForeignKey(category_table_id))
    biotope_values = db.Column(db.Text)
    default_amount = db.Column(db.Integer)
    default_shape_id = db.Column(db.Integer)
    default_width = db.Column(db.Text)
    default_size = db.Column(db.Text)
    
    def to_dict(self, category = None):
        reply = {id_tag: self.id, name_tag: self.name}
        if (category is not None):
            reply[category_tag] = category.name
            reply[terrain_flattening_tag] = category.terrain_flattening
        return reply
    
    def __repr__(self):
        return '<Element {}>'.format(self.name)

class User(UserMixin, db.Model):
    """Model for user accounts."""   
    
    global user_table, locale_table_id, sha256, utf8
    
    __tablename__ = user_table
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    id_locale = db.Column(db.Integer, db.ForeignKey(locale_table_id), nullable=False)
    username = db.Column(db.String(32), index = True, unique = True, nullable=False)
    password_hash = db.Column(db.String(256), nullable=False)
    email = db.Column(db.String(128), unique = True, nullable=False)
    private = db.Column(db.Boolean, nullable=False)
    professional = db.Column(db.Boolean, nullable=False)
    created = db.Column(db.DateTime, nullable=False)
    salt = db.Column(db.String(32), nullable=False)
    
    def hash_password(self, password):
        self.salt = urandom(32)
        self.password_hash = pbkdf2_hmac(sha256, password.encode(utf8), self.salt, 100000)

    def verify_password(self, password):
        return (self.password_hash == pbkdf2_hmac(sha256, password.encode(utf8), self.salt, 100000))
    
    def __repr__(self):
        return '<User {}>'.format(self.username)

class UserPlacementRequest(db.Model):
    """Model for user placement requests (main functionality)."""
    
    global user_placement_request_table, user_table_id
    
    __tablename__ = user_placement_request_table
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    user_id = db.Column(db.Integer, db.ForeignKey(user_table_id), nullable=False)
    terrain_data = db.Column(db.Text, nullable=False)
    elements_data = db.Column(db.Text, nullable=False)
    requested = db.Column(db.DateTime, nullable=False)
    
    def terrain_preprocessing(self, terrain_data):
        """Terrain data preprocessing: converting from format of received JSON [{index, line_data[str,]}] to float[][]."""
        
        global terrain_line_data_tag, index_tag
        
        formatted_data = [None] * len(terrain_data)
        for line in terrain_data:
            line_data = []
            for value in line[terrain_line_data_tag]:
                line_data.append(float(value))
            formatted_data[line[index_tag]] = line_data
        return formatted_data
    
    def elements_preprocessing(self, elements_data):
        """Elements data preprocessing: converting from format of received JSON [{id, counter},] to [{id: [counter]}]."""
        
        global id_tag, counter_tag
        
        # TODO Add processing of fixed_elements.
        formatted_data = {}
        for element_data in elements_data:
            formatted_data[element_data[id_tag]] = [element_data[counter_tag]]
        return formatted_data
    
    def check_terrain_data(self, terrain_data):
        """Checking if terrain data is a rectangle."""
        
        try:
            length = len(terrain_data[0])
            for i in range(len(terrain_data)):
                if (len(terrain_data[i]) != length):
                    return False # not a rectangle
        except:
            return False # inconsistent data type
        return True
    
    def check_elements_data(self, elements_data, terrain_data):
        """Checking constistency of data structure: {element_id: [amount, [(fixed, coordinates)], [other, fixed, coordinates]]}.
        The fixed coordinates are a list of tuples representing the coordinates of the element (all squares that are used).
        Fixed coordinates are optional."""
        allowed_element_ids = [element.id for element in Element.query.filter_by(id_locale = current_user.id_locale).all()]
        global unallowed_height
        try:
            for key in elements_data:
                if (type(key) != int or key not in allowed_element_ids):
                    return False # not an integer or not in allowed element ids
                values = elements_data[key]
                amount = values[0]
                if (int(str(amount)) != amount or amount < 1):
                    return False # amount is not a positive integer
                if (len(values) > (amount + 1)):
                    return False # too many fixed elements
                for i in range(1, len(values)):
                    for coordinates in values[i]:
                        if (len(coordinates) != 2):
                            return False # one of the coordinates is not a tuple
                        if (type(coordinates[0]) != int or type(coordinates[1]) != int):
                            return False # one of the coordinates is not an integer value
                        if (terrain_data[coordinates[0]][coordinates[1]] == unallowed_height):
                            return False # out of terrain boundaries
        except:
            return False # inconsistent data type
        return True

class BinaryInteraction(db.Model):
    """Model for database binary interactions between elements."""

    global binary_interaction_table, locale_table_id, element_table_id

    __tablename__ = binary_interaction_table
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    id_locale = db.Column(db.Integer, db.ForeignKey(locale_table_id), nullable=False)
    element1_id = db.Column(db.Integer, db.ForeignKey(element_table_id), nullable=False)
    element2_id = db.Column(db.Integer, db.ForeignKey(element_table_id), nullable=False)
    interaction_level = db.Column(db.Integer, nullable=False)
    description = db.Column(db.Text, nullable=False)
    
    @property
    def serialize(self):
        """Return object data in easily serializable format."""
        return {
            'id': self.id,
            'id_locale': self.id_locale,
            'element1_id': self.element1_id,
            'element2_id': self.element2_id,
            'interaction_level': self.interaction_level,
            'description': self.description
        }
    
    def __repr__(self):
        return '<Binary interaction {}>'.format(self.id)

class InteractionType(db.Model):
    """Model for database ternary interaction types."""
    
    global interaction_type_table
    
    __tablename__ = interaction_type_table
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    name = db.Column(db.String, nullable=False)
    description = db.Column(db.Text, nullable=False)

class TernaryInteraction(db.Model):
    """Model for database ternary interaction between two elements."""
    
    global ternary_interaction_table, locale_table_id, element_table_id, interaction_type_table_id
    
    __tablename__ = ternary_interaction_table
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    id_locale = db.Column(db.Integer, db.ForeignKey(locale_table_id), nullable=False)
    element1_id = db.Column(db.Integer, db.ForeignKey(element_table_id), nullable=False)
    interaction_type_id = db.Column(db.Integer, db.ForeignKey(interaction_type_table_id), nullable=False)
    element2_id = db.Column(db.Integer, db.ForeignKey(element_table_id), nullable=False)
    description = db.Column(db.Text, nullable=False)

