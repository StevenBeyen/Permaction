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
