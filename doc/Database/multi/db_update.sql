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

