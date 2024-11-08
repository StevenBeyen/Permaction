# [Permaction](https://www.permaction.com/)

*(Version française ci-dessous.)*

## *English version*

### 1. General description

[Permaction](https://www.permaction.com/) is a website project to help designing lands using general permaculture principles, meant for both isolated people and communities in the countryside. 

The main goal of this project is to provide an easy-to-use graphical interface as well as graphical, 'cartoonish' results for easy visualisation. Users would only have to delimit their lands on a world map and select the elements they want to place (house, vegetable garden, henhouse, tool shed, composting toilet, ...), as well as fixed (existing or desired) elements.
The service then proposes an optimised placement of requested elements regarding to their characteristics and positive and negative interactions.

The output is shown as a 2.5D area, which then allows users to roam through the lands and see the result for themselves. Selecting an element also opens a visual effect that shows the elements it interacts with.

It is crucial for the project to be intuitive and provide quality graphical results as well as, of course, quality information regarding the permaculture interaction principles.

### 2. Technical realisation

The project is composed of two main parts: a web service backend and a visual interface.

The server is composed of a Python REST API web service regulating access (using Flask), providing data from the database (using SQLAlchemy) and computing element placement requests using a custom-written genetic algorithm.

In order to provide quality visual results, Unity has been chosen to create the interface. Unity projects can also be exported as websites or mobile applications, which means there's no need to develop multiple front-ends.

### 3. Demo version

The demo version has been released! This version provides a prebuilt piece of land and limits selection to a maximum of ten elements, the main goal being to show future capabilities of the project as well as the appealing visual results.

### 4. Next development steps

In order to enhance user experience, here are the next development steps for the project :
- adding a background light for selected elements ;
- adding a name/description when mouse hovers an icon (computer version) ;
- adding an information pop-up when arriving on 3D results page ;
- enhancing 3D interaction descriptions display ;
- adding a progression bar when generating result.

### 5. Open suggestions

Here is a list of the suggestions that have been made and that are yet to be discussed or thought before being included to the project:
- generation of roads and paths ;
- possibility to move 3D elements with dynamic update of fitness score ;
- selection of interaction priorities between elements, in an advanced user mode ;
- possibility to parameter permaculture sectors (hot/cold winds, fire risk, undesirable view, ...) during land selection on map ;
- adding temporal maintenance notion in order to follow land evolution and attention points.

### 6. Scaling up to a community

Thoughts and experience are making me consider structural modifications to the project, making it useful to a larger scale :
- taking into account individual and collective parcels ;
- selection of elements to include on each parcel and in collective spaces (as a 2D selection table).

Scaling up this project for a community or an (eco)town  is an interesting step towards greater autonomy for a larger group of people, which means greater resilience.

### 7. Definitive version

The public version of the project would then add the remaining features in order to provide the full user experience as described in the introduction:
- user account management ;
- selection of the land on a map as well as its digitalisation ;
- placement of fixed elements on the land ;
- extraction of detailed plans (topography, distances, ...) ;
- allowing selection of multiple instances of the same element ;
- relevant suggestions made on the demo version.

*Any contribution or suggestion is very welcome and will be appreciated!*

## *Version française*

### 1. Description générale

[Permaction](https://www.permaction.com/) est un projet de site web d'aide au design de terrains en permaculture, destiné aux personnes isolées et au communautés vivant à la campagne.

L'objectif principal de ce projet est de fournir une interface graphique facile d'utilisation, ainsi qu'un résultat graphique, type 'cartoon', pour une visualisation aisée du résultat. Les utilisateurs auraient seulement à délimiter leur terrain sur une carte du monde et à sélectionner les éléments qu'ils désirent placer (habitation, potager, poulailler, remise à outils, toilette sèche, ...), ainsi que des éléments fixes (existants ou souhaités).
Le service proposerait ensuite un placement optimisé de ces éléments en fonction de leurs caractéristiques et leurs interactions positives et négatives.

Le résultat est montré via une interface 2.5D, qui permet aux utilisateurs de se promener sur le terrain et d'en découvrir l'agencement eux-mêmes. Sélectionner un élément déclenche également un effet visuel qui montre les éléments avec lesquels il est en interaction.

Il est crucial que le projet soit intuitif d'utilisation et fournisse des résultats de bonne qualité graphique, ainsi que des informations de qualité sur les principes d'interaction en permaculture.

### 2. Réalisation technique

Le projet est composé de deux parties principales: un service web et une interface graphique.

Le serveur est composé d'une API REST écrite en Python qui régule l'accès (avec Flask), fournit les données de la base de données (avec SQLAlchemy) et calcule les requêtes d'agencement via un algorithme génétique personnalisé.

Afin de fournir des résultats graphiques de qualité, l'interface a été créée avec Unity. Les projets Unity peuvent également être exportés comme sites web ou applications mobiles, ce qui permet de concentrer les efforts sur la réalisation d'une seule interface utilisateur.

### 3. Version prototype

La version prototype du projet a été publiée ! Cette version fournit un terrain fictif et limite la sélection à dix éléments, l'objectif principal étant de montrer le potentiel du projet ainsi que l'interface graphique attractive.

### 4. Prochaines étapes de développement

Afin de rendre l'expérience utilisateur plus agréable, voici les prochaines priorités de développement pour le projet :
- ajout d'un halo lumineux autour des éléments sélectionnés ;
- affichage d'un nom/descriptif au passage de la souris sur une icône (version ordinateur) ;
- affichage d'un panneau d'informations lors du passage en vue résultat 3D ;
- rendre les descriptions d'interactions plus lisibles ;
- ajout d'une barre de progression du calcul.

### 5. Suggestions ouvertes

Voici une liste des suggestions qui ont actuellement été faites et qui doivent encore être discutées avant d'être incluses au projet:
- la génération des routes et chemins ;
- la possibilité de déplacer les éléments 3D avec mise à jour dynamique du score de placement ;
- la sélection de priorités d'interaction entre les éléments, dans un mode d'utilisation avancé ;
- la possibilité de paramétrer les différents secteurs (vents chauds/froids, risque d'incendie, vue indésirable, ...) lors de la sélection du terrain ;
- l'ajout d'une notion de maintenance temporelle afin de suivre l'évolution du terrain et les points d'attention.

### 6. Mise à l'échelle d'une communauté

Des réflexions et des expériences supplémentaires me poussent à considérer des modifications structurelles du projet, afin de le rendre utile à l'échelle d'un terrain de plus grande dimension :
- prise en compte de parcelles individuelles et collectives ;
- sélection des éléments à intégrer sur chaque parcelle et dans les espaces communs (sous forme de tableau de sélection en 2D).

Rendre ce projet utile à l'échelle d'une communauté ou d'un (éco)village est une étape intéressante vers une autonomie croissante pour un plus grand nombre, et donc une plus grande résilience.

### 7. Version publique

La version publique du projet ajoutera les fonctionnalités restantes afin de fournir l'expérience utilisateur complète telle que décrite dans l'introduction:
- la gestion des comptes utilisateur ;
- la sélection du terrain sur une carte ainsi que sa digitalisation ;
- le placement des éléments fixes sur le terrain ;
- l'extraction de plans/schémas détaillés (coupes de terrain, distances, ...) ;
- la possibilité de sélectionner plusieurs instances du même élément.

*Toute contribution ou suggestion est la bienvenue et sera appréciée !*
