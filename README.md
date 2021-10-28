# Permaction

*(Version française ci-dessous.)*

## *English version*

### General description

Permaction is a website project to help designing lands using general permaculture principles, meant for both isolated people and communities in the countryside. 

The main goal of this project is to provide an easy-to-use graphical interface as well as graphical, 'cartoonish' results for easy visualisation. Users would only have to delimit their lands on a world map and select the elements they want to place (house, vegetable garden, henhouse, tool shed, composting toilet, ...), as well as fixed (existing or desired) elements.
The service then proposes an optimised placement of requested elements regarding to their characteristics and positive and negative interactions.

The output is shown as a 2.5D area, which then allows users to roam through the lands and see the result for themselves. Selecting an element also opens a visual effect that shows the elements it interacts with.

It is crucial for the project to be intuitive and provide quality graphical results as well as, of course, quality information regarding the permaculture interaction principles.

### Technical realisation

The project is composed of two main parts: a web service backend and a visual interface.

The server is composed of a Python REST API web service regulating access (using Flask), providing data from the database (using SQLAlchemy) and computing element placement requests using a custom-written genetic algorithm.

In order to provide quality visual results, Unity has been chosen to create the interface. Unity projects can also be exported as websites or mobile applications, which means there's no need to develop multiple front-ends.

### Current goals and demo version

The demo version is currently under development. This version provides a prebuilt piece of land and limits selection to a maximum of ten elements, the main goal being to show future capabilities of the project as well as the appealing visual results.

The remaining work to finish the demo includes:
- creating all 3D prefabs for the end visual result ;
- managing generation of roads and paths ;
- transferring AI service to a computing cloud or a server with better computational capabilities ;
- finishing visual link between selected element and the ones it interacts with ;
- adding sidebar on end result visualisation ;
- adding random visual improvements to better overall quality of visual effect ;
- adding user account management ;
- adding multilingual management (currently french and english, other languages are of course welcome) ;
- asking for feedback on user experience using a small form.

Any contribution or suggestion is very welcome and will be appreciated!

### Open suggestions

Here is a list of the suggestions that have been made and that are yet to be discussed or thought before being included to the project:
- possibility to move 3D elements with dynamic update of fitness score ;
- extraction of detailed plans (topography, distances, ...) ;
- selection of interaction priorities between elements, in an advanced user mode ;
- possibility to parameter permaculture sectors (hot/cold winds, fire risk, undesirable view, ...) during land selection on map ;
- adding temporal maintenance notion in order to follow land evolution and attention points.

### Definitive version

The public version of the project would then add the remaining features in order to provide the full user experience as described in the introduction:
- selection of the land on a map as well as its digitalisation ;
- placement of fixed elements on the land ;
- allowing selection of multiple instances of the same element ;
- relevant suggestions made on the demo version.

## *Version française*

### Description générale

Permaction est un projet de site web d'aide au design de terrains en permaculture, destiné aux personnes isolées et au communautés vivant à la campagne.

L'objectif principal de ce projet est de fournir une interface graphique facile d'utilisation, ainsi qu'un résultat graphique, type 'cartoon', pour une visualisation aisée du résultat. Les utilisateurs auraient seulement à délimiter leur terrain sur une carte du monde et à sélectionner les éléments qu'ils désirent placer (habitation, potager, poulailler, remise à outils, toilette sèche, ...), ainsi que des éléments fixes (existants ou souhaités).
Le service proposerait ensuite un placement optimisé de ces éléments en fonction de leurs caractéristiques et leurs interactions positives et négatives.

Le résultat est montré via une interface 2.5D, qui permet aux utilisateurs de se promener sur le terrain et d'en découvrir l'agencement eux-mêmes. Sélectionner un élément déclenche également un effet visuel qui montre les éléments avec lesquels il est en interaction.

Il est crucial que le projet soit intuitif d'utilisation et fournisse des résultats de bonne qualité graphique, ainsi que des informations de qualité sur les principes d'interaction en permaculture.

### Réalisation technique

Le projet est composé de deux parties principales: un service web et une interface graphique.

Le serveur est composé d'une API REST écrite en Python qui régule l'accès (avec Flask), fournit les données de la base de données (avec SQLAlchemy) et calcule les requêtes d'agencement via un algorithme génétique personnalisé.

Afin de fournir des résultats graphiques de qualité, l'interface a été créée avec Unity. Les projets Unity peuvent également être exportés comme sites web ou applications mobiles, ce qui permet de concentrer les efforts sur la réalisation d'une seule interface utilisateur.

### Objectifs actuels et prototype

La version de démonstration (prototype) est actuellement en cours de développement. Cette version fournit un terrain préconstruit et limite la sélection à dix éléments, l'objectif principal étant de montrer le potentiel du projet ainsi que l'interface graphique attractive.

Le travail restant afin de terminer le prototype inclut:
- la création de tous les préfabriqués 3D pour le résultat final ;
- la génération des routes et chemins ;
- le transfert du service d'IA vers un service de calcul dans le cloud ou vers un serveur avec de meilleures capacités de calcul ;
- l'achèvement du lien visuel entre un élément sélectionné et ceux avec lesquels il interagit ;
- l'ajout d'une barre latérale sur l'affichage 3D final ;
- l'ajout de diverses améliorations visuelles afin d'obtenir un résultat de meilleure qualité graphique ;
- l'ajout de la gestion des comptes utilisateur ;
- l'ajout de la gestion multilingue (actuellement français et anglais, d'autres langues sont évidemment les bienvenues) ;
- l'ajout d'un retour utilisateur via un petit formulaire.

Toute contribution ou suggestion est la bienvenue et sera appréciée !

### Suggestions ouvertes

Voici une liste des suggestions qui ont actuellement été faites et qui doivent encore être discutées avant d'être incluses au projet:
- la possibilité de déplacer les éléments 3D avec mise à jour dynamique du score de placement ;
- l'extraction de plans/schémas détaillés (coupes de terrain, distances, ...) ;
- la sélection de priorités d'interaction entre les éléments, dans un mode d'utilisation avancé ;
- la possibilité de paramétrer les différents secteurs (vents chauds/froids, risque d'incendie, vue indésirable, ...) lors de la sélection du terrain ;
- l'ajout d'une notion de maintenance temporelle afin de suivre l'évolution du terrain et les points d'attention.

### Version définitive

La version publique du projet ajouterait les fonctionnalités restantes afin de fournir l'expérience utilisateur complète telle que décrite dans l'introduction:
- la sélection du terrain sur une carte ainsi que sa digitalisation ;
- le placement des éléments fixes sur le terrain ;
- la possibilité de sélectionner plusieurs instances du même élément ;
- les suggestions pertinentes faites sur la version prototype.
