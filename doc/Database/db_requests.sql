CREATE TABLE locale (
    id          tinyint PRIMARY KEY,
    locale      varchar NOT NULL,
    language    varchar NOT NULL
);

INSERT INTO locale VALUES (1, 'en', 'English');
INSERT INTO locale VALUES (2, 'fr', 'Français');



------------------------------------------------------------------------------------------------------------------------



CREATE TABLE shape (
    id          tinyint PRIMARY KEY,
    id_locale   tinyint NOT NULL,
    name        varchar NOT NULL,
    FOREIGN KEY (id_locale) REFERENCES locale(id)
);

INSERT INTO shape VALUES (1,1,'Conical');
INSERT INTO shape VALUES (2,1,'Curved');
INSERT INTO shape VALUES (3,1,'Leaf');
INSERT INTO shape VALUES (4,1,'Oval');
INSERT INTO shape VALUES (5,1,'Rectangular');
INSERT INTO shape VALUES (6,1,'Spiral');
INSERT INTO shape VALUES (7,1,'Straight');
INSERT INTO shape VALUES (8,1,'Vertical');
INSERT INTO shape VALUES (9,2,'Conique');
INSERT INTO shape VALUES (10,2,'Courbe');
INSERT INTO shape VALUES (11,2,'Feuille');
INSERT INTO shape VALUES (12,2,'Ovale');
INSERT INTO shape VALUES (13,2,'Rectangulaire');
INSERT INTO shape VALUES (14,2,'Spirale');
INSERT INTO shape VALUES (15,2,'Droite');
INSERT INTO shape VALUES (16,2,'Verticale');



------------------------------------------------------------------------------------------------------------------------



CREATE TABLE biotope (
    id                  tinyint PRIMARY KEY,
    id_locale           tinyint NOT NULL,
    value               tinyint NOT NULL,
    description         varchar NOT NULL,
    soil_composition    varchar,
    FOREIGN KEY (id_locale) REFERENCES locale(id)
);

INSERT INTO biotope VALUES (1,1,0,'None (building, path, etc.)','');
INSERT INTO biotope VALUES (2,1,1,'Annual plants','Bacterial');
INSERT INTO biotope VALUES (3,1,2,'Perennial plants','Mainly bacterial');
INSERT INTO biotope VALUES (4,1,3,'Bushes','Half-bacterial, half-fungal');
INSERT INTO biotope VALUES (5,1,4,'Shrubs and dwarf trees','Mainly fungal');
INSERT INTO biotope VALUES (6,1,5,'Trees, forest','Fungal');
INSERT INTO biotope VALUES (7,1,10,'Biodiversity enhancer (water point, animal shelter, wild corridor, etc.)','');
INSERT INTO biotope VALUES (8,2,0,'Aucun (bâtiment, chemin, …)','');
INSERT INTO biotope VALUES (9,2,1,'Plantes annuelles','Bactérien');
INSERT INTO biotope VALUES (10,2,2,'Plantes vivaces/pérennes','Majoritairement bactérien');
INSERT INTO biotope VALUES (11,2,3,'Buissons','Mi-bactérien, mi-fongique');
INSERT INTO biotope VALUES (12,2,4,'Arbustes et arbres nains','Majoritairement fongique');
INSERT INTO biotope VALUES (13,2,5,'Arbres, Forêt','Fongique');
INSERT INTO biotope VALUES (14,2,10,'Améliorateur de biodiversité (point d’eau, abri animal, couloir sauvage, …)','');



------------------------------------------------------------------------------------------------------------------------



CREATE TABLE category (
    id                  tinyint PRIMARY KEY,
    id_locale           tinyint NOT NULL,
    name                varchar NOT NULL,
    physical_category   tinyint NOT NULL,
    FOREIGN KEY (id_locale) REFERENCES locale(id)
);

INSERT INTO category VALUES (1,1,'Animal shelter',1);
INSERT INTO category VALUES (2,1,'Area',0);
INSERT INTO category VALUES (3,1,'Building',1);
INSERT INTO category VALUES (4,1,'Forest',1);
INSERT INTO category VALUES (5,1,'Road',1);
INSERT INTO category VALUES (6,1,'Sector',0);
INSERT INTO category VALUES (7,2,'Abri animal',1);
INSERT INTO category VALUES (8,2,'Zone',0);
INSERT INTO category VALUES (9,2,'Bâtiment',1);
INSERT INTO category VALUES (10,2,'Forêt',1);
INSERT INTO category VALUES (11,2,'Chemin',1);
INSERT INTO category VALUES (12,2,'Secteur',0);
INSERT INTO category VALUES (13,1,'Plants & water points',1);
INSERT INTO category VALUES (14,2,'Plantes & points d’eau',1);
INSERT INTO category VALUES (15,1,'Misc.',1);
INSERT INTO category VALUES (16,2,'Divers',1);



------------------------------------------------------------------------------------------------------------------------



CREATE TABLE interaction_level (
    id          tinyint PRIMARY KEY,
    id_locale   tinyint NOT NULL,
    value       tinyint NOT NULL,
    description varchar NOT NULL,
    FOREIGN KEY (id_locale) REFERENCES locale(id)
);

INSERT INTO interaction_level VALUES (1,1,-10,'Interaction between those elements is to be avoided at all costs.');
INSERT INTO interaction_level VALUES (2,1,-9,'Very bad interaction between two elements.');
INSERT INTO interaction_level VALUES (3,1,-8,'Very bad interaction between two elements.');
INSERT INTO interaction_level VALUES (4,1,-7,'Very bad interaction between two elements.');
INSERT INTO interaction_level VALUES (5,1,-6,'Bad interaction between two elements.');
INSERT INTO interaction_level VALUES (6,1,-5,'Bad interaction between two elements.');
INSERT INTO interaction_level VALUES (7,1,-4,'Bad interaction between two elements.');
INSERT INTO interaction_level VALUES (8,1,-3,'Mildly bad interaction between two elements.');
INSERT INTO interaction_level VALUES (9,1,-2,'Mildly bad interaction between two elements.');
INSERT INTO interaction_level VALUES (10,1,-1,'Mildly bad interaction between two elements.');
INSERT INTO interaction_level VALUES (11,1,0,'No known interaction between two elements.');
INSERT INTO interaction_level VALUES (12,1,1,'Mildly positive interaction between two elements.');
INSERT INTO interaction_level VALUES (13,1,2,'Mildly positive interaction between two elements.');
INSERT INTO interaction_level VALUES (14,1,3,'Mildly positive interaction between two elements.');
INSERT INTO interaction_level VALUES (15,1,4,'Positive interaction between two elements.');
INSERT INTO interaction_level VALUES (16,1,5,'Positive interaction between two elements.');
INSERT INTO interaction_level VALUES (17,1,6,'Positive interaction between two elements.');
INSERT INTO interaction_level VALUES (18,1,7,'Strong positive interaction between two elements.');
INSERT INTO interaction_level VALUES (19,1,8,'Strong positive interaction between two elements.');
INSERT INTO interaction_level VALUES (20,1,9,'Strong positive interaction between two elements.');
INSERT INTO interaction_level VALUES (21,1,10,'Mandatory interaction between two elements.');
INSERT INTO interaction_level VALUES (22,2,-10,'L’interaction de ces éléments doit être absolument évitée.');
INSERT INTO interaction_level VALUES (23,2,-9,'Très mauvaise interaction entre deux éléments.');
INSERT INTO interaction_level VALUES (24,2,-8,'Très mauvaise interaction entre deux éléments.');
INSERT INTO interaction_level VALUES (25,2,-7,'Très mauvaise interaction entre deux éléments.');
INSERT INTO interaction_level VALUES (26,2,-6,'Mauvaise interaction entre deux éléments.');
INSERT INTO interaction_level VALUES (27,2,-5,'Mauvaise interaction entre deux éléments.');
INSERT INTO interaction_level VALUES (28,2,-4,'Mauvaise interaction entre deux éléments.');
INSERT INTO interaction_level VALUES (29,2,-3,'Interaction légèrement mauvaise entre deux éléments.');
INSERT INTO interaction_level VALUES (30,2,-2,'Interaction légèrement mauvaise entre deux éléments.');
INSERT INTO interaction_level VALUES (31,2,-1,'Interaction légèrement mauvaise entre deux éléments.');
INSERT INTO interaction_level VALUES (32,2,0,'Pas d’interaction connue entre les éléments.');
INSERT INTO interaction_level VALUES (33,2,1,'Interaction légèrement positive entre deux éléments.');
INSERT INTO interaction_level VALUES (34,2,2,'Interaction légèrement positive entre deux éléments.');
INSERT INTO interaction_level VALUES (35,2,3,'Interaction légèrement positive entre deux éléments.');
INSERT INTO interaction_level VALUES (36,2,4,'Interaction positive entre deux éléments.');
INSERT INTO interaction_level VALUES (37,2,5,'Interaction positive entre deux éléments.');
INSERT INTO interaction_level VALUES (38,2,6,'Interaction positive entre deux éléments.');
INSERT INTO interaction_level VALUES (39,2,7,'Interaction très positive entre deux éléments.');
INSERT INTO interaction_level VALUES (40,2,8,'Interaction très positive entre deux éléments.');
INSERT INTO interaction_level VALUES (41,2,9,'Interaction très positive entre deux éléments.');
INSERT INTO interaction_level VALUES (42,2,10,'Interaction obligatoire entre deux éléments.');



------------------------------------------------------------------------------------------------------------------------



CREATE TABLE interaction_type (
    id          tinyint PRIMARY KEY,
    name        varchar NOT NULL,
    description varchar NOT NULL
);

INSERT INTO interaction_type VALUES (1,'higher','Runtime interpretation for ternary interaction : element1 placed higher than element2.');
INSERT INTO interaction_type VALUES (2,'south','Runtime interpretation for ternary interaction : element1 placed south of element2.');



------------------------------------------------------------------------------------------------------------------------



CREATE TABLE element (
    id                  tinyint PRIMARY KEY,
    id_locale           tinyint NOT NULL,
    name                varchar NOT NULL,
    category_id         tinyint,
    biotope_values      tinyint,
    default_amount      tinyint,
    default_shape_id    tinyint,
    default_width       varchar,
    default_size        varchar,
    FOREIGN KEY (id_locale) REFERENCES locale(id),
    FOREIGN KEY (category_id) REFERENCES category(id),
    FOREIGN KEY (default_shape_id) REFERENCES shape(id)
);

INSERT INTO element VALUES (1,1,'Animal shelter',1,0,NULL,NULL,NULL,'1-200');
INSERT INTO element VALUES (2,1,'Aromatic plants',13,'1,2',1,6,NULL,'1-50');
INSERT INTO element VALUES (3,1,'Barn',3,0,1,5,NULL,'20-200');
INSERT INTO element VALUES (4,1,'Beehive',1,10,1,5,NULL,'1-10');
INSERT INTO element VALUES (5,1,'Building',3,0,1,5,NULL,'10-500');
INSERT INTO element VALUES (6,1,'Climbers',13,'1,2',2,8,0,'var.');
INSERT INTO element VALUES (7,1,'Composter',15,10,3,5,NULL,'1-50');
INSERT INTO element VALUES (8,1,'Composting toilet',3,0,1,5,NULL,'1-10');
INSERT INTO element VALUES (9,1,'Feed forest for the poultry',4,'3,4',1,2,NULL,'1000-10000');
INSERT INTO element VALUES (10,1,'Fire',6,0,1,NULL,NULL,'var.');
INSERT INTO element VALUES (11,1,'Flower meadow',13,'1,2',1,2,NULL,'10-500');
INSERT INTO element VALUES (12,1,'Forest',4,'3,4,5',2,2,NULL,'1000-10000');
INSERT INTO element VALUES (13,1,'Garden',13,'1,2',1,2,NULL,'100-1000');
INSERT INTO element VALUES (14,1,'Greenhouse',3,'1,2',1,5,NULL,'5-500');
INSERT INTO element VALUES (15,1,'Greenhouse conservatory',3,0,1,5,NULL,'5-50');
INSERT INTO element VALUES (16,1,'Hedgehog shelter',1,10,1,5,NULL,'2-5');
INSERT INTO element VALUES (17,1,'Hedgerow',13,'2,3,4',NULL,2,'3-6','var.');
INSERT INTO element VALUES (18,1,'Heights',2,NULL,NULL,NULL,NULL,'var.');
INSERT INTO element VALUES (19,1,'Henhouse',1,10,1,5,NULL,'5-50');
INSERT INTO element VALUES (20,1,'Housing',3,0,1,NULL,NULL,'50-500');
INSERT INTO element VALUES (21,1,'Low',2,NULL,NULL,NULL,NULL,'var.');
INSERT INTO element VALUES (22,1,'Medicinal plants',13,'1,2',1,2,NULL,'5-100');
INSERT INTO element VALUES (23,1,'Mid-height',2,NULL,NULL,NULL,NULL,'var.');
INSERT INTO element VALUES (24,1,'Orchard',4,'3,4',1,2,NULL,'1000-10000');
INSERT INTO element VALUES (25,1,'Path',5,0,1,2,'1','var.');
INSERT INTO element VALUES (26,1,'Phytopurification',13,10,1,5,NULL,'10-500');
INSERT INTO element VALUES (27,1,'Pine trees',4,'4,5',1,2,NULL,'1000-10000');
INSERT INTO element VALUES (28,1,'Road suitable for vehicles',5,0,1,2,3,'var.');
INSERT INTO element VALUES (29,1,'Shadow',2,NULL,NULL,NULL,NULL,'var.');
INSERT INTO element VALUES (30,1,'Sun',6,NULL,1,NULL,NULL,'var.');
INSERT INTO element VALUES (31,1,'Terrace',3,0,NULL,5,NULL,'5-50');
INSERT INTO element VALUES (32,1,'Tool shed',3,0,1,5,NULL,'5-50');
INSERT INTO element VALUES (33,1,'Trellis',15,10,NULL,8,0,'var.');
INSERT INTO element VALUES (34,1,'Unwanted view',6,NULL,1,NULL,NULL,'var.');
INSERT INTO element VALUES (35,1,'Vegetable garden',13,'1,2',1,2,NULL,'50-1000');
INSERT INTO element VALUES (36,1,'Wall',15,0,NULL,2,1,'var.');
INSERT INTO element VALUES (37,1,'Water point',13,10,3,2,NULL,'5-200');
INSERT INTO element VALUES (38,1,'Wild corridor',13,10,1,1,'var.','100-1000');
INSERT INTO element VALUES (39,1,'Wind',6,NULL,1,NULL,NULL,'var.');
INSERT INTO element VALUES (40,1,'Workshop',3,0,1,5,NULL,'10-100');
INSERT INTO element VALUES (41,2,'Abri animal',7,0,NULL,NULL,NULL,'1-200');
INSERT INTO element VALUES (42,2,'Plantes aromatiques',13,'1,2',1,14,NULL,'1-50');
INSERT INTO element VALUES (43,2,'Grange',9,0,1,13,NULL,'20-200');
INSERT INTO element VALUES (44,2,'Ruche',7,10,1,13,NULL,'1-10');
INSERT INTO element VALUES (45,2,'Bâtiment',9,0,1,13,NULL,'10-500');
INSERT INTO element VALUES (46,2,'Plantes grimpantes',14,'1,2',2,16,0,'var.');
INSERT INTO element VALUES (47,2,'Composteur',15,10,3,13,NULL,'1-50');
INSERT INTO element VALUES (48,2,'Toilettes sèches',9,0,1,13,NULL,'1-10');
INSERT INTO element VALUES (49,2,'Forêt nourricière pour la volaille',10,'3,4',1,10,NULL,'1000-10000');
INSERT INTO element VALUES (50,2,'Feu',12,0,1,NULL,NULL,'var.');
INSERT INTO element VALUES (51,2,'Prairie fleurie',14,'1,2',1,10,NULL,'10-500');
INSERT INTO element VALUES (52,2,'Forêt',10,'3,4,5',2,10,NULL,'1000-10000');
INSERT INTO element VALUES (53,2,'Jardin',14,'1,2',1,10,NULL,'100-1000');
INSERT INTO element VALUES (54,2,'Serre',9,'1,2',1,13,NULL,'5-500');
INSERT INTO element VALUES (55,2,'Véranda',9,0,1,13,NULL,'5-50');
INSERT INTO element VALUES (56,2,'Abri hérisson',7,10,1,13,NULL,'2-5');
INSERT INTO element VALUES (57,2,'Haie',14,'2,3,4',NULL,10,'3-6','var.');
INSERT INTO element VALUES (58,2,'Hauteur',8,NULL,NULL,NULL,NULL,'var.');
INSERT INTO element VALUES (59,2,'Poulailler',7,10,1,13,NULL,'5-50');
INSERT INTO element VALUES (60,2,'Habitation',9,0,1,NULL,NULL,'50-500');
INSERT INTO element VALUES (61,2,'Bas',8,NULL,NULL,NULL,NULL,'var.');
INSERT INTO element VALUES (62,2,'Plantes médicinales',14,'1,2',1,10,NULL,'5-100');
INSERT INTO element VALUES (63,2,'Mi-hauteur',8,NULL,NULL,NULL,NULL,'var.');
INSERT INTO element VALUES (64,2,'Verger',10,'3,4',1,10,NULL,'1000-10000');
INSERT INTO element VALUES (65,2,'Chemin pédestre',11,0,1,10,'1','var.');
INSERT INTO element VALUES (66,2,'Phytoépuration',14,10,1,13,NULL,'10-500');
INSERT INTO element VALUES (67,2,'Pins',10,'4,5',1,10,NULL,'1000-10000');
INSERT INTO element VALUES (68,2,'Chemin carrossable',11,0,1,10,3,'var.');
INSERT INTO element VALUES (69,2,'Ombre',8,NULL,NULL,NULL,NULL,'var.');
INSERT INTO element VALUES (70,2,'Soleil',12,NULL,1,NULL,NULL,'var.');
INSERT INTO element VALUES (71,2,'Terrasse',9,0,NULL,13,NULL,'5-50');
INSERT INTO element VALUES (72,2,'Remise à outils',9,0,1,13,NULL,'5-50');
INSERT INTO element VALUES (73,2,'Treillage',16,10,NULL,16,0,'var.');
INSERT INTO element VALUES (74,2,'Vue indésirable',12,NULL,1,NULL,NULL,'var.');
INSERT INTO element VALUES (75,2,'Potager',14,'1,2',1,10,NULL,'50-1000');
INSERT INTO element VALUES (76,2,'Mur',16,0,NULL,10,1,'var.');
INSERT INTO element VALUES (77,2,'Point d’eau',14,10,3,10,NULL,'5-200');
INSERT INTO element VALUES (78,2,'Couloir sauvage',14,10,1,9,'var.','100-1000');
INSERT INTO element VALUES (79,2,'Vent',12,NULL,1,NULL,NULL,'var.');
INSERT INTO element VALUES (80,2,'Atelier',9,0,1,13,NULL,'10-100');



------------------------------------------------------------------------------------------------------------------------



CREATE TABLE ternary_interaction (
    id                  tinyint PRIMARY KEY,
    id_locale           tinyint NOT NULL,
    element1_id         tinyint NOT NULL,
    interaction_type_id tinyint NOT NULL,
    element2_id         tinyint NOT NULL,
    description         varchar NOT NULL,
    FOREIGN KEY (id_locale) REFERENCES locale(id),
    FOREIGN KEY (element1_id) REFERENCES element(id),
    FOREIGN KEY (interaction_type_id) REFERENCES interaction_type(id),
    FOREIGN KEY (element2_id) REFERENCES element(id)
);

INSERT INTO ternary_interaction VALUES (1,1,20,1,26,'The wastewater from the home flows to the phytopurification basins.');
INSERT INTO ternary_interaction VALUES (2,1,26,1,37,'The water cleaned by phytopurification flows to the water point.');
INSERT INTO ternary_interaction VALUES (3,1,37,2,35,'A water point located to the south of the vegetable garden reflects the sunlight towards the latter.');
INSERT INTO ternary_interaction VALUES (4,2,60,1,66,'Les eaux usées de l’habitation s’écoulent vers les bassins de phytoépuration.');
INSERT INTO ternary_interaction VALUES (5,2,66,1,77,'Les eaux nettoyées par la phytoépuration s’écoulent vers le point d’eau.');
INSERT INTO ternary_interaction VALUES (6,2,77,2,75,'Un point d’eau situé au sud du potager permet de refléter les rayons du soleil vers ce dernier.');



------------------------------------------------------------------------------------------------------------------------



CREATE TABLE binary_interaction (
    id                  tinyint PRIMARY KEY,
    id_locale           tinyint NOT NULL,
    element1_id         tinyint NOT NULL,
    element2_id         tinyint NOT NULL,
    interaction_level   tinyint NOT NULL,
    description         varchar NOT NULL,
    FOREIGN KEY (id_locale) REFERENCES locale(id),
    FOREIGN KEY (element1_id) REFERENCES element(id),
    FOREIGN KEY (element2_id) REFERENCES element(id),
    FOREIGN KEY (interaction_level) REFERENCES interaction_level(value)
);

INSERT INTO binary_interaction VALUES (1,1,1,7,9,'Animals supply carbon dioxide and methane to the compost (in a closed system).');
INSERT INTO binary_interaction VALUES (2,1,1,14,9,'The animal shelter provides heat to the greenhouse.');
INSERT INTO binary_interaction VALUES (3,1,1,14,9,'Animals supply carbon dioxide and methane to the greenhouse.');
INSERT INTO binary_interaction VALUES (4,1,1,14,9,'The animal shelter provides manure to the greenhouse.');
INSERT INTO binary_interaction VALUES (5,1,2,20,9,'Aromatic plants are used on a daily basis for cooking.');
INSERT INTO binary_interaction VALUES (6,1,3,1,9,'The barn can be used to store food and manure away from the rain.');
INSERT INTO binary_interaction VALUES (7,1,4,11,9,'Bees gather flowers from the meadow.');
INSERT INTO binary_interaction VALUES (8,1,4,20,2,'During the summer, bees take an hour a week to care for them and harvest their honey.');
INSERT INTO binary_interaction VALUES (9,1,4,35,5,'The bees pollinate the plants in the vegetable garden.');
INSERT INTO binary_interaction VALUES (10,1,5,29,9,'Buildings provide shadow.');
INSERT INTO binary_interaction VALUES (11,1,5,39,9,'Buildings block the wind.');
INSERT INTO binary_interaction VALUES (12,1,6,20,9,'Climbers provide insulation to the housing.');
INSERT INTO binary_interaction VALUES (13,1,8,11,9,'The composting toilet can provide compost to the flowers.');
INSERT INTO binary_interaction VALUES (14,1,8,11,9,'Flowers are nice to look at when you are sitting on the toilet.');
INSERT INTO binary_interaction VALUES (15,1,8,12,9,'The composting toilet provides compost to the forest.');
INSERT INTO binary_interaction VALUES (16,1,8,17,9,'The composting toilet provides compost to the hedgerows.');
INSERT INTO binary_interaction VALUES (17,1,9,19,9,'This forest provides food and shelter to the poultry.');
INSERT INTO binary_interaction VALUES (18,1,9,29,9,'Trees provide shadow.');
INSERT INTO binary_interaction VALUES (19,1,11,20,5,'Flowers provide a nice view from home.');
INSERT INTO binary_interaction VALUES (20,1,11,24,9,'Attracts pollinating insects, which will also pollinate the orchard and increase fruit production.');
INSERT INTO binary_interaction VALUES (21,1,11,25,9,'The flower meadow provides a visual and olfactory spectacle.');
INSERT INTO binary_interaction VALUES (22,1,11,35,5,'Refuge for many beneficial insects in the garden.');
INSERT INTO binary_interaction VALUES (23,1,11,35,5,'Attracts pollinating insects, which will also pollinate the vegetable garden and increase fruit production.');
INSERT INTO binary_interaction VALUES (24,1,12,6,9,'The trees provide a structure for the climbers.');
INSERT INTO binary_interaction VALUES (25,1,12,18,9,'An elevated forest helps reduce runoff and erosion caused by rain.');
INSERT INTO binary_interaction VALUES (26,1,12,29,9,'Trees provide shadow.');
INSERT INTO binary_interaction VALUES (27,1,12,34,9,'The forest hides the unwanted view.');
INSERT INTO binary_interaction VALUES (28,1,12,39,5,'Trees reduce strength of the wind.');
INSERT INTO binary_interaction VALUES (29,1,13,20,9,'The garden is a place of relaxation close to the home.');
INSERT INTO binary_interaction VALUES (30,1,14,1,9,'The greenhouse provides heat to the animal shelter.');
INSERT INTO binary_interaction VALUES (31,1,14,30,9,'The greenhouse requires sun.');
INSERT INTO binary_interaction VALUES (32,1,15,20,9,'The greenhouse conservatory provides heat to the adjoining building.');
INSERT INTO binary_interaction VALUES (33,1,15,30,9,'The greenhouse conservatory requires sun.');
INSERT INTO binary_interaction VALUES (34,1,16,35,9,'Hedgehogs are nocturnal animals that are very fond of slugs.');
INSERT INTO binary_interaction VALUES (35,1,17,12,9,'The hedgerow shelters and feeds the wild fauna of the forest.');
INSERT INTO binary_interaction VALUES (36,1,17,20,3,'A hedgerow can provide berries: blackcurrants, raspberries, currants, blackberries, etc.');
INSERT INTO binary_interaction VALUES (37,1,17,24,9,'The hedgerow shelters and feeds wildlife and creates a favorable transition to the orchard.');
INSERT INTO binary_interaction VALUES (38,1,17,29,7,'The hedgerow provides shadow.');
INSERT INTO binary_interaction VALUES (39,1,17,35,3,'The hedgerow helps regulate temperature.');
INSERT INTO binary_interaction VALUES (40,1,17,35,3,'The hedgerow promotes biodiversity.');
INSERT INTO binary_interaction VALUES (41,1,17,39,7,'The hedgerow reduces wind strength.');
INSERT INTO binary_interaction VALUES (42,1,19,4,9,'The chickens protect the bees from the Asian hornet, which attacks the bees at the exit of the beehive.');
INSERT INTO binary_interaction VALUES (43,1,19,10,-9,'Charred chicken smells worse than roast chicken.');
INSERT INTO binary_interaction VALUES (44,1,19,20,5,'The henhouse provides eggs.');
INSERT INTO binary_interaction VALUES (45,1,19,20,2,'The henhouse requires daily care for food and water.');
INSERT INTO binary_interaction VALUES (46,1,19,20,1,'The henhouse manure needs to be taken care of every two or three weeks.');
INSERT INTO binary_interaction VALUES (47,1,19,35,9,'The chickens fertilize the land and make it ready for cultivation.');
INSERT INTO binary_interaction VALUES (48,1,19,35,9,'The chickens kill the pests of the vegetable garden. To prevent them from destroying the plants, they must be provided with routes between crops or released at the end of the season.');
INSERT INTO binary_interaction VALUES (49,1,19,39,-5,'The henhouse should be protected from the wind.');
INSERT INTO binary_interaction VALUES (50,1,20,23,9,'A mid-height housing allows a balance between the shelter provided by the forest in height and the view on the rest of the land.');
INSERT INTO binary_interaction VALUES (51,1,22,20,9,'Medicinal plants are used among other things as essential remedies and tonics.');
INSERT INTO binary_interaction VALUES (52,1,24,12,9,'The orchard and the forest are close biotopes that have much to gain by being close to each other.');
INSERT INTO binary_interaction VALUES (53,1,24,20,5,'The orchard provides fruit to the land owners.');
INSERT INTO binary_interaction VALUES (54,1,24,29,9,'The orchard provides shadow.');
INSERT INTO binary_interaction VALUES (55,1,25,1,10,'The path provides access to the animal shelter.');
INSERT INTO binary_interaction VALUES (56,1,25,8,10,'The path provides access to the composting toilet.');
INSERT INTO binary_interaction VALUES (57,1,25,24,10,'The path provides access to the orchard.');
INSERT INTO binary_interaction VALUES (58,1,25,35,10,'The path provides access to the vegetable garden.');
INSERT INTO binary_interaction VALUES (59,1,25,37,10,'The path provides access to the water point.');
INSERT INTO binary_interaction VALUES (60,1,25,40,10,'The path provides access to the workshop.');
INSERT INTO binary_interaction VALUES (61,1,26,20,9,'The phytopurification system cleans gray water from the housing.');
INSERT INTO binary_interaction VALUES (62,1,26,37,9,'The purified water feeds the water point.');
INSERT INTO binary_interaction VALUES (63,1,27,10,-9,'Pine trees burn very easily.');
INSERT INTO binary_interaction VALUES (64,1,27,20,1,'Pine trees provide edible pine nuts.');
INSERT INTO binary_interaction VALUES (65,1,27,29,9,'Pine trees provide shadow.');
INSERT INTO binary_interaction VALUES (66,1,27,39,9,'Pine trees provide a strong protection against wind.');
INSERT INTO binary_interaction VALUES (67,1,28,3,10,'The road provides motorised access to the barn.');
INSERT INTO binary_interaction VALUES (68,1,28,10,9,'The road is used as a firebreak.');
INSERT INTO binary_interaction VALUES (69,1,28,20,10,'The road provides motorised access to the housing.');
INSERT INTO binary_interaction VALUES (70,1,32,24,9,'Storing tools near the orchard limits their need for transportation.');
INSERT INTO binary_interaction VALUES (71,1,32,35,9,'Storing tools near the vegetable garden limits their need for transportation.');
INSERT INTO binary_interaction VALUES (72,1,33,6,9,'The trellis provides a structure for the climbers.');
INSERT INTO binary_interaction VALUES (73,1,35,7,9,'The vegetable garden provides organic matter to compost.');
INSERT INTO binary_interaction VALUES (74,1,35,30,9,'Plants need sun to grow.');
INSERT INTO binary_interaction VALUES (75,1,35,39,-9,'Sheltering the vegetable garden from the wind limits evaporation in the summer and increases its temperature in the winter.');
INSERT INTO binary_interaction VALUES (76,1,36,29,5,'Walls provide shadow.');
INSERT INTO binary_interaction VALUES (77,1,36,39,5,'Walls block the wind.');
INSERT INTO binary_interaction VALUES (78,1,37,10,9,'The water points serve as protection against possible fires, and provide water for extinguishing them.');
INSERT INTO binary_interaction VALUES (79,1,37,18,9,'An elevated water point makes it possible to store the water that flows from the heights, and to easily use it using gravity.');
INSERT INTO binary_interaction VALUES (80,1,37,21,9,'A Water point at the bottom of the land collects all the water flowing on it.');
INSERT INTO binary_interaction VALUES (81,1,37,23,9,'A Water point located at mid-height collects a large amount of water and can be used by gravity.');
INSERT INTO binary_interaction VALUES (82,1,37,30,9,'Water points absorb heat and reflect light.');
INSERT INTO binary_interaction VALUES (83,1,37,32,9,'The water point can be used to wash tools.');
INSERT INTO binary_interaction VALUES (84,1,37,35,9,'The frogs, toads and salamanders that settle in the water point feed among other things on slugs.');
INSERT INTO binary_interaction VALUES (85,1,37,35,9,'A water point reflects the sunlight, which provides light and warmth to the plants.');
INSERT INTO binary_interaction VALUES (86,1,37,35,9,'A water point brings biodiversity close to the crops.');
INSERT INTO binary_interaction VALUES (87,1,38,12,9,'The wild corridor links the forest and the housing.');
INSERT INTO binary_interaction VALUES (88,1,38,20,9,'Creating an unmaintained corridor from the forest to the home provides biodiversity across the land, in addition to providing a great view.');
INSERT INTO binary_interaction VALUES (89,1,40,8,9,'The workshop provides sawdust to the composting toilet.');
INSERT INTO binary_interaction VALUES (90,2,56,75,9,'Les hérissons sont des animaux nocturnes très friands des limaces.');
INSERT INTO binary_interaction VALUES (91,2,41,47,9,'Les animaux fournissent du dioxyde de carbone et du méthane au compost (en système clos).');
INSERT INTO binary_interaction VALUES (92,2,41,54,9,'L’abri animal fournit de la chaleur à la serre.');
INSERT INTO binary_interaction VALUES (93,2,41,54,9,'Les animaux fournissent du dioxyde de carbone et du méthane à la serre.');
INSERT INTO binary_interaction VALUES (94,2,41,54,9,'L’abri animal fournit du fumier à la serre.');
INSERT INTO binary_interaction VALUES (95,2,80,48,9,'L’atelier fournit de la sciure pour les toilettes sèches.');
INSERT INTO binary_interaction VALUES (96,2,45,69,9,'Les bâtiments fournissent de l’ombre.');
INSERT INTO binary_interaction VALUES (97,2,45,79,9,'Les bâtiments protègent du vent.');
INSERT INTO binary_interaction VALUES (98,2,68,50,9,'Le chemin sert de coupe-feu.');
INSERT INTO binary_interaction VALUES (99,2,68,43,10,'Le chemin fournit un accès carrossable à la grange.');
INSERT INTO binary_interaction VALUES (100,2,68,60,10,'Le chemin fournit un accès à l’habitation.');
INSERT INTO binary_interaction VALUES (101,2,65,41,10,'Le chemin fournit un accès à l’abri animal.');
INSERT INTO binary_interaction VALUES (102,2,65,80,10,'Le chemin fournit un accès à l’atelier.');
INSERT INTO binary_interaction VALUES (103,2,65,77,10,'Le chemin fournit un accès au point d’eau.');
INSERT INTO binary_interaction VALUES (104,2,65,75,10,'Le chemin fournit un accès au potager.');
INSERT INTO binary_interaction VALUES (105,2,65,48,10,'Le chemin fournit un accès aux toilettes sèches.');
INSERT INTO binary_interaction VALUES (106,2,65,64,10,'Le chemin fournit un accès au verger.');
INSERT INTO binary_interaction VALUES (107,2,78,52,9,'Le couloir sauvage relie la forêt et l’habitation.');
INSERT INTO binary_interaction VALUES (108,2,78,60,9,'Créer un couloir non entretenu de la zone sauvage jusqu’à l’habitation permet de fournir de la biodiversité sur tout le terrain, en plus de fournir une belle vue.');
INSERT INTO binary_interaction VALUES (109,2,52,58,9,'Une forêt située en hauteur permet de réduire l’écoulement et l’érosion provoqués par la pluie.');
INSERT INTO binary_interaction VALUES (110,2,52,69,9,'Les arbres fournissent de l’ombre.');
INSERT INTO binary_interaction VALUES (111,2,52,46,9,'Les arbres fournissent une structure pour les plantes grimpantes.');
INSERT INTO binary_interaction VALUES (112,2,52,79,5,'Les arbres protègent du vent.');
INSERT INTO binary_interaction VALUES (113,2,52,74,9,'La forêt cache des éléments indésirables à l’oeil.');
INSERT INTO binary_interaction VALUES (114,2,49,69,9,'Les arbres fournissent de l’ombre.');
INSERT INTO binary_interaction VALUES (115,2,49,59,9,'Cette forêt sert de réserve de nourriture pour la volaille.');
INSERT INTO binary_interaction VALUES (116,2,43,41,9,'La grange permet de stocker le fumier animal et la nourriture au sec.');
INSERT INTO binary_interaction VALUES (117,2,60,63,9,'Une habitation à mi-hauteur permet un équilibre entre l’abri fourni par la forêt en hauteur et la vue sur le reste du terrain.');
INSERT INTO binary_interaction VALUES (118,2,57,52,9,'La haie abrite et nourrit la faune sauvage de la forêt.');
INSERT INTO binary_interaction VALUES (119,2,57,60,3,'Une haie peut fournir de petits fruits : cassis, framboises, groseilles, mûres, …');
INSERT INTO binary_interaction VALUES (120,2,57,69,7,'La haie fournit de l’ombre.');
INSERT INTO binary_interaction VALUES (121,2,57,75,3,'La haie aide à réguler les températures.');
INSERT INTO binary_interaction VALUES (122,2,57,75,3,'La haie favorise la biodiversité.');
INSERT INTO binary_interaction VALUES (123,2,57,79,7,'La haie protège du vent.');
INSERT INTO binary_interaction VALUES (124,2,57,64,9,'La haie abrite et nourrit la faune sauvage et crée une transition favorable vers le verger.');
INSERT INTO binary_interaction VALUES (125,2,53,60,9,'Le jardin est un lieu de détente proche de l’habitation.');
INSERT INTO binary_interaction VALUES (126,2,76,69,5,'Le mur fournit de l’ombre.');
INSERT INTO binary_interaction VALUES (127,2,76,79,5,'Le mur protège du vent.');
INSERT INTO binary_interaction VALUES (128,2,66,60,9,'Le système de phytoépuration nettoie les eaux grises de l’habitation.');
INSERT INTO binary_interaction VALUES (129,2,66,77,9,'L’eau épurée alimente le point d’eau.');
INSERT INTO binary_interaction VALUES (130,2,67,50,-9,'Les pins brûlent très facilement.');
INSERT INTO binary_interaction VALUES (131,2,67,60,1,'Les pins fournissent des pignons comestibles.');
INSERT INTO binary_interaction VALUES (132,2,67,69,9,'Les pins fournissent de l’ombre.');
INSERT INTO binary_interaction VALUES (133,2,67,79,9,'Les pins forment des brise-vent résistants.');
INSERT INTO binary_interaction VALUES (134,2,42,60,9,'Les plantes aromatiques sont utilisées de façon quotidienne pour la cuisine.');
INSERT INTO binary_interaction VALUES (135,2,46,60,9,'Les plantes grimpantes fournissent de l’isolation à l’habitation.');
INSERT INTO binary_interaction VALUES (136,2,62,60,9,'Les plantes médicinales servent entre autres de remèdes de première nécessité et de toniques.');
INSERT INTO binary_interaction VALUES (137,2,77,61,9,'Un point d’eau en bas du terrain permet de collecter toutes les eaux s’écoulant sur celui-ci.');
INSERT INTO binary_interaction VALUES (138,2,77,50,9,'Les points d’eau servent de rempart face à d’éventuels incendies, et fournissent de l’eau pour l’extinction du feu.');
INSERT INTO binary_interaction VALUES (139,2,77,58,9,'Un point d’eau en amont permet de stocker les eaux qui s’écoulent depuis les hauteurs, et d’utiliser facilement celle-ci en usant de la gravité.');
INSERT INTO binary_interaction VALUES (140,2,77,63,9,'Un point d’eau situé à mi-hauteur permet de collecter une importante quantité d’eau et une utilisation par gravité.');
INSERT INTO binary_interaction VALUES (141,2,77,75,9,'Les grenouilles, crapauds et salamandres qui s’établissent dans le point d’eau se nourrissent entre autres de limaces.');
INSERT INTO binary_interaction VALUES (142,2,77,75,9,'Un point d’eau réfléchit les rayons du soleil, ce qui fournit lumière et chaleur aux plantes.');
INSERT INTO binary_interaction VALUES (143,2,77,75,9,'Un point d’eau apporte de la biodiversité à proximité des cultures.');
INSERT INTO binary_interaction VALUES (144,2,77,72,9,'Le point d’eau peut être utilisé pour y laver les outils.');
INSERT INTO binary_interaction VALUES (145,2,77,70,9,'Les points d’eau absorbent la chaleur et reflètent la lumière.');
INSERT INTO binary_interaction VALUES (146,2,75,47,9,'Le potager fournit de la matière organique à composter.');
INSERT INTO binary_interaction VALUES (147,2,75,70,9,'Les plantes ont besoin de soleil pour pousser.');
INSERT INTO binary_interaction VALUES (148,2,75,79,-9,'Abriter le potager du vent limite l’évaporation en été, et augmente sa température en hiver.');
INSERT INTO binary_interaction VALUES (149,2,59,50,-9,'Le poulet carbonisé sent moins bon que le poulet rôti.');
INSERT INTO binary_interaction VALUES (150,2,59,60,5,'Le poulailler fournit des œufs de façon plus ou moins quotidienne.');
INSERT INTO binary_interaction VALUES (151,2,59,60,2,'Le poulailler demande une visite hebdomadaire pour traiter la nourriture des animaux.');
INSERT INTO binary_interaction VALUES (152,2,59,60,1,'Le fumier du poulailler doit être traité toutes les deux à trois semaines.');
INSERT INTO binary_interaction VALUES (153,2,59,75,9,'Les poules fertilisent le terrain et le rendent prêt à être cultivé.');
INSERT INTO binary_interaction VALUES (154,2,59,75,9,'Les poules éliminent les ravageurs du potager. Pour éviter qu’elles ne détruisent les plantes, il faut leur aménager des parcours entre les cultures ou les lâcher en fin de saison.');
INSERT INTO binary_interaction VALUES (155,2,59,44,9,'Les poules protègent les abeilles du frelon asiatique, qui guette les abeilles en vol stationnaire à la sortie de la ruche.');
INSERT INTO binary_interaction VALUES (156,2,59,79,-5,'Le poulailler devrait être protégé du vent.');
INSERT INTO binary_interaction VALUES (157,2,51,65,9,'La prairie fleurie fournit un spectacle à la fois visuel et olfactif au passant.');
INSERT INTO binary_interaction VALUES (158,2,51,60,5,'Les fleurs offrent une belle vue depuis l’habitation.');
INSERT INTO binary_interaction VALUES (159,2,51,75,5,'Refuge pour de nombreux insectes bénéfiques au jardin.');
INSERT INTO binary_interaction VALUES (160,2,51,75,5,'Attire les insecte pollinisateurs, qui vont également polliniser le potager et augmenter la production de fruits.');
INSERT INTO binary_interaction VALUES (161,2,51,64,9,'Attire les insecte pollinisateurs, qui vont également polliniser le verger et augmenter la production de fruits.');
INSERT INTO binary_interaction VALUES (162,2,72,75,9,'Stocker les outils près du potager limite leur besoin de transport.');
INSERT INTO binary_interaction VALUES (163,2,72,64,9,'Stocker les outils près du verger limite leur besoin de transport.');
INSERT INTO binary_interaction VALUES (164,2,44,60,2,'En été, les abeilles demandent une heure par semaine pour s’occuper d’elles et récolter leur miel.');
INSERT INTO binary_interaction VALUES (165,2,44,75,5,'Les abeilles pollinisent les plantes présentes dans le potager.');
INSERT INTO binary_interaction VALUES (166,2,44,51,9,'Les abeilles butinent les fleurs de cette prairie.');
INSERT INTO binary_interaction VALUES (167,2,54,41,9,'La serre fournit de la chaleur à l’abri animal.');
INSERT INTO binary_interaction VALUES (168,2,54,70,9,'La serre a besoin de soleil pour être efficace.');
INSERT INTO binary_interaction VALUES (169,2,55,60,9,'La serre-véranda fournit de la chaleur au bâtiment auquel elle est adossée.');
INSERT INTO binary_interaction VALUES (170,2,55,70,9,'La serre-véranda a besoin de soleil.');
INSERT INTO binary_interaction VALUES (171,2,48,52,9,'Les toilettes sèches fournissent du compost aux arbres.');
INSERT INTO binary_interaction VALUES (172,2,48,57,9,'Les toilettes sèches fournissent du compost à la haie.');
INSERT INTO binary_interaction VALUES (173,2,48,51,9,'Les toilettes sèches fournissent du compost aux fleurs de la prairie.');
INSERT INTO binary_interaction VALUES (174,2,48,51,9,'Les fleurs offrent une belle vue depuis les toilettes sèches.');
INSERT INTO binary_interaction VALUES (175,2,73,46,9,'Le treillage fournit une structure pour les plantes grimpantes.');
INSERT INTO binary_interaction VALUES (176,2,64,52,9,'Le verger et la forêt sont des biotopes proches qui ont beaucoup à gagner à être proches l’un de l’autre.');
INSERT INTO binary_interaction VALUES (177,2,64,60,5,'Le verger fournit des fruits aux habitants du domaine.');
INSERT INTO binary_interaction VALUES (178,2,64,69,9,'Le verger fournit de l’ombre.');
INSERT INTO binary_interaction VALUES (179,1,31,20,9,'The terrace is usually close to the housing.');
INSERT INTO binary_interaction VALUES (180,2,71,60,9,'La terrasse se situe généralement près de l’habitation.');



------------------------------------------------------------------------------------------------------------------------



CREATE TABLE user (
    id              int PRIMARY KEY,
    id_locale       tinyint NOT NULL,
    username        varchar NOT NULL,
    password_hash   varchar NOT NULL,
    email           varchar NOT NULL,
    private         tinyint NOT NULL,
    professional    tinyint NOT NULL,
    FOREIGN KEY (id_locale) REFERENCES locale(id)
);



------------------------------------------------------------------------------------------------------------------------

