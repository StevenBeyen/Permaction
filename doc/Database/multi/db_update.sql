update element set default_size = "200-400" where id = 37;
update element set default_size = "200-400" where id = 77;

update element set category_id = 13 where id = 7;
update element set category_id = 14 where id = 47;
update element set category_id = 3 where id = 36;
update element set category_id = 9 where id = 76;

delete from category where id = 15;
delete from category where id = 16;

insert into element values (6,1,'Tree',13,10,NULL,8,NULL,'1-5');
insert into element values (46,2,'Arbre',14,10,NULL,16,NULL,'1-5');
insert into element values (33,1,'Main square',3,0,1,5,NULL,'50-250');
insert into element values (73,2,'Place principale',9,0,1,13,NULL,'50-250');




insert into category values (15,1,'Water points',1);
insert into category values (16,2,'Points d’eau',1);

update category set name = 'Plants' where id = 13;
update category set name = 'Plantes' where id = 14;

update category set name = 'Trees & shrubs' where id = 4;
update category set name = 'Arbres & arbustes' where id = 10;
update category set name = 'Path' where id = 5;

update element set category_id = 4 where id = 6;
update element set category_id = 4 where id = 17;
update element set category_id = 15 where id = 26;
update element set category_id = 15 where id = 37;
update element set category_id = 4 where id = 38;

update element set category_id = 10 where id = 46;
update element set category_id = 10 where id = 57;
update element set category_id = 16 where id = 66;
update element set category_id = 16 where id = 77;
update element set category_id = 10 where id = 78;



update category set name = 'Animal shelters' where name = 'Animal shelter';
update category set name = 'Buildings' where name = 'Building';
update category set name = 'Paths' where name = 'Path';

update category set name = 'Abris pour animaux' where name = 'Abri animal';
update category set name = 'Bâtiments' where name = 'Bâtiment';
update category set name = 'Chemins' where name = 'Chemin';

update element set name = 'Pond' where id = 37;
update element set name = 'Étang' where id = 77;



update element set name = 'Sunroom' where id = 15;
update element set name = 'Road' where id = 28;
update element set name = 'Animal shelters' where id = 1;
update element set name = 'Buildings' where id = 5;
update element set name = 'Abris pour animaux' where id = 41;
update element set name = 'Bâtiments' where id = 45;
update element set name = 'Hedge' where id = 17;

update element set name = 'House' where name = 'Housing';



insert into category values (17,1,'DEMOLOCK',0);

update element set category_id = 17 where id = 6; -- used to be 4
update element set category_id = 17 where id = 31; -- used to be 3
update element set category_id = 17 where id = 46; -- used to be 10
update element set category_id = 17 where id = 71; -- used to be 9



update element set default_size = '6' where default_size = '1-5';
update element set default_size = '6' where default_size = '2-5';
update element set default_size = '6' where default_size = '5-10';



delete from binary_interaction where id = 179;
delete from binary_interaction where id = 180;

update element set default_size = '6-6' where default_size = '6';



delete from element where id = 36;
delete from element where id = 76;

delete from binary_interaction where id = 76;
delete from binary_interaction where id = 77;
delete from binary_interaction where id = 126;
delete from binary_interaction where id = 127;



update category set name = 'Batiments' where id = 9;
update element set name = 'Batiments' where id = 45;
update element set name = 'Toilettes seches' where id = 48;
update element set name = 'Foret nourriciere pour la volaille' where id = 49;
update element set name = 'Foret' where id = 52;
update element set name = 'Veranda' where id = 55;
update element set name = 'Abri herisson' where id = 56;
update element set name = 'Plantes medicinales' where id = 62;
update element set name = 'Sentier' where id = 65;
update element set name = 'Phytoepuration' where id = 66;
update element set name = 'Route' where id = 68;
update element set name = 'Remise a outils' where id = 72;
update element set name = 'Vue indesirable' where id = 74;
update element set name = 'Etang' where id = 77;



alter table category add element_id tinyint;
update category set element_id = 1 where id = 1;
update category set element_id = 5 where id = 3;
update category set element_id = 41 where id = 7;
update category set element_id = 45 where id = 9;



update element set default_size = '400-2000' where id = 37;
update element set default_size = '400-2000' where id = 77;
update element set default_size = '15-250' where id = 20;
update element set default_size = '15-250' where id = 60;

insert into binary_interaction values (72, 1, 25, 20, 10, 'The path provides access to the housing.');
insert into binary_interaction values (76, 2, 65, 60, 10, 'Le sentier fournit un accès à l’habitation.');

update binary_interaction set description = 'La route sert de coupe-feu.' where id = 98;
update binary_interaction set description = 'La route fournit un accès à la grange.' where id = 99;
update binary_interaction set description = 'La route fournit un accès à l’habitation.' where id = 100;
update binary_interaction set description = 'Le sentier fournit un accès à l’abri animal.' where id = 101;
update binary_interaction set description = 'Le sentier fournit un accès à l’atelier.' where id = 102;
update binary_interaction set description = 'Le sentier fournit un accès au point d’eau.' where id = 103;
update binary_interaction set description = 'Le sentier fournit un accès au potager.' where id = 104;



update binary_interaction set description = 'Le sentier fournit un accès aux toilettes sèches.' where id = 105;
update binary_interaction set description = 'Le sentier fournit un accès au verger.' where id = 106;



alter table category add terrain_flattening tinyint not null default 0;
update category set terrain_flattening = 1 where id = 1;
update category set terrain_flattening = 1 where id = 3;
update category set terrain_flattening = 1 where id = 7;
update category set terrain_flattening = 1 where id = 9;
update category set terrain_flattening = 1 where id = 15;
update category set terrain_flattening = 1 where id = 16;



alter table category add show_interactions tinyint not null default 1;
update category set show_interactions = 0 where id = 5;
update category set show_interactions = 0 where id = 11;
update category set show_interactions = 0 where id = 17;



update element set default_size = '500-2500' where default_size = '500-5000';



update element set default_size = '100-500' where default_size = '250-1000';



update element set default_size = '250-500' where id = 33;
update element set default_size = '250-500' where id = 73;



update element set default_size = '500-2500' where id = 9;
update element set default_size = '500-2500' where id = 49;



update element set default_size = '100-250' where default_size = '50-100';


update element set default_size = '500-1000' where id = 38;
update element set default_size = '500-1000' where id = 78;



update binary_interaction set description = 'Hedgehogs are nocturnal animals that are very fond of slugs, which can be found in the vegetable garden.' where id = 34;
update binary_interaction set description = 'The wild corridor and the forest share their biodiversity.' where id = 87;
update binary_interaction set description = 'The wild corridor provides biodiversity close to the house, in addition to providing a nice view.' where id = 88;
delete from binary_interaction where id = 12;
update binary_interaction set description = 'The barn can be used to store animal resources away from the rain.' where id = 6;
update binary_interaction set description = 'A mid-height housing provides a nice view without dominating the terrain.' where id = 50;
update binary_interaction set interaction_level = 0 where id = 8;
delete from binary_interaction where id = 64;
delete from binary_interaction where id = 79;
delete from binary_interaction where id = 81;
update binary_interaction set description = 'The frogs, toads and salamanders that settle in the water point feed among other things on slugs from the vegetable garden.' where id = 84;
delete from binary_interaction where id = 24;
update binary_interaction set interaction_level = 0 where id = 44;
update binary_interaction set interaction_level = 0 where id = 45;
update binary_interaction set interaction_level = 0 where id = 46;
update binary_interaction set description = 'The henhouse requires regular care for food and water.' where id = 45;
update binary_interaction set description = 'The henhouse manure needs to be taken care of every two or three weeks maximum.' where id = 46;
update binary_interaction set description = 'Chickens protect the bees from the Asian hornet, which attacks them at the exit of the beehive.' where id = 42;
delete from binary_interaction where id = 21;
update binary_interaction set description = 'The flower meadow provides a refuge for many beneficial insects in the vegetable garden.' where id = 22;
update binary_interaction set description = 'The flower meadow attracts pollinating insects, which will also pollinate the vegetable garden and increase fruit production.' where id = 23;
update binary_interaction set description = 'The flower meadow attracts pollinating insects, which will also pollinate the orchard and increase fruit production.' where id = 20;
update binary_interaction set interaction_level = 0 where id = 164;
delete from binary_interaction where id = 14;
update binary_interaction set description = 'The orchard and the forest share their biodiversity.' where id = 52;
delete from binary_interaction where id = 53;
update binary_interaction set description = 'The sunroom provides heat to the adjoining building.' where id = 32;
update binary_interaction set description = 'The sunroom requires sun.' where id = 33;
update binary_interaction set interaction_level = 0 where id = 61;
update binary_interaction set interaction_level = 0 where id = 62;
delete from binary_interaction where id = 85;


update binary_interaction set description = 'Les hérissons sont des animaux nocturnes très friands des limaces, que l’on trouve au potager.' where id = 90;
update binary_interaction set description = 'Le couloir sauvage et la forêt s’échangent leur biodiversité.' where id = 107;
update binary_interaction set description = 'Le couloir sauvage fournit de la biodiversité jusqu’à l’habitation, en plus de fournir une belle vue.' where id = 108;
delete from binary_interaction where id = 111;
update binary_interaction set description = 'La grange permet de stocker les denrées animales au sec.' where id = 116;
update binary_interaction set description = 'Une habitation à mi-hauteur fournit une belle vue sur le terrain sans être prédominante.' where id = 117;
delete from binary_interaction where id = 131;
delete from binary_interaction where id = 135;
delete from binary_interaction where id = 139;
delete from binary_interaction where id = 140;
update binary_interaction set description = 'Les grenouilles, crapauds et salamandres qui s’établissent dans le point d’eau se nourrissent entre autres des limaces du potager.' where id = 141;
delete from binary_interaction where id = 142;
update binary_interaction set interaction_level = 0 where id = 150;
update binary_interaction set interaction_level = 0 where id = 151;
update binary_interaction set interaction_level = 0 where id = 152;
update binary_interaction set description = 'Le poulailler demande une visite régulière pour traiter la nourriture des animaux.' where id = 151;
update binary_interaction set description = 'Le fumier du poulailler doit être traité toutes les deux à trois semaines maximum.' where id = 152;
update binary_interaction set description = 'Les poules protègent les abeilles du frelon asiatique, qui les guette en vol stationnaire à la sortie de la ruche.' where id = 155;
delete from binary_interaction where id = 157;
update binary_interaction set description = 'La prairie fleurie fournit un refuge pour de nombreux insectes bénéfiques au jardin potager.' where id = 159;
update binary_interaction set description = 'La prairie fleurie attire les insecte pollinisateurs, qui vont également polliniser le potager et augmenter la production de fruits.' where id = 160;
update binary_interaction set description = 'La prairie fleurie attire les insecte pollinisateurs, qui vont également polliniser le verger et augmenter la production de fruits.' where id = 161;
update binary_interaction set interaction_level = 0 where id = 164;
update binary_interaction set description = 'Les abeilles butinent gaiement dans la prairie fleurie.' where id = 166;
delete from binary_interaction where id = 174;
update binary_interaction set description = 'Le verger et la forêt s’échangent leur biodiversité.' where id = 176;
delete from binary_interaction where id = 177;
update binary_interaction set interaction_level = 0 where id = 128;
update binary_interaction set interaction_level = 0 where id = 129;



update category set physical_category = 1 where id = 5;
update category set physical_category = 1 where id = 11;
delete from binary_interaction where id = 158;
delete from binary_interaction where id = 19;
update binary_interaction set description = 'Le couloir sauvage fournit de la biodiversité jusqu’à l’habitation.' where id = 108;
update binary_interaction set description = 'The wild corridor provides biodiversity close to the house.' where id = 88;
delete from element where id = 15;
delete from element where id = 55;
delete from binary_interaction where id = 32;
delete from binary_interaction where id = 33;
delete from binary_interaction where id = 169;
delete from binary_interaction where id = 170;

delete from element where id = 15;
delete from element where id = 55;
insert into element values (15,1,'Terrain access',2,NULL,NULL,NULL,NULL,'var.');
insert into element values (55,2,'Accès au terrain',8,NULL,NULL,NULL,NULL,'var.');

insert into binary_interaction values (12,1,15,28,10,'The road provides motorised access to the terrain.');
insert into binary_interaction values (14,2,55,68,10,'La route fournit un accès au terrain.');

insert into binary_interaction values (19,1,35,38,9,'The wild corridor provides biodiversity to the vegetable garden.');
insert into binary_interaction values (21,2,75,78,9,'Le couloir sauvage fournit de la biodiversité au potager.');
update binary_interaction set interaction_level = 3 where id = 44;
update binary_interaction set description = 'The henhouse provides eggs and requires regular care for food and water.' where id = 44;
update binary_interaction set interaction_level = 3 where id = 150;
update binary_interaction set description = 'Le poulailler fournit des oeufs et demande une visite régulière pour traiter l’eau et la nourriture.' where id = 150;
insert into binary_interaction values (32,1,14,25,10,'The path provides access to the greenhouse.');
insert into binary_interaction values (33,2,54,65,10,'Le sentier fournit un accès à la serre.');

insert into binary_interaction values (53,1,14,35,9,'The greenhouse can be used to grow plants for the vegetable garden.');
insert into binary_interaction values (64,2,54,75,9,'La serre peut être utilisée pour faire pousser des plantes pour le jardin potager.');
update binary_interaction set element2_id = 75 where id = 64;

update binary_interaction set interaction_level = 9 where id = 64;



update category set physical_category = 0 where id = 5;
update category set physical_category = 0 where id = 11;

