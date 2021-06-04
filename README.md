# Permaction

## General description

Permaction is a website project to help designing lands using general permaculture principles, meant for both isolated people and communities in the countryside. 

The main goal of this project is to provide an easy-to-use graphical interface as well as graphical, 'cartoonish' results for easy visualisation. Users would only have to delimit their lands on a world map and select the elements they want to place (house, vegetable garden, henhouse, tool shed, composting toilet, ...), as well as fixed (existing or desired) elements.
The service then proposes an optimised placement of requested elements regarding to their positive and negative interactions.

The output is shown as a 2.5D area, which then allows users to roam through the lands and see the result for themselves. Selecting an element also opens a visual effect that shows the elements it interacts with.

It is crucial for the project to be intuitive and provide quality graphical results as well as, of course, quality information regarding the permaculture interaction principles.

## Technical realisation

The project is composed of two main parts: a web service backend and a visual interface.

The server is composed of a Python REST API web service regulating access (using Flask), providing data from the database (using SQLAlchemy) and computing element placement requests using a custom-written genetic algorithm.

In order to provide quality visual results, Unity has been chosen to create the interface. Unity projects can also be exported as websites or mobile applications, which means there's no need to develop multiple front-ends.

## Current goals and demo version

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

## Open suggestions

Here is a list of the suggestions that have been made and that are yet to be discussed or thought before being included to the project:
- adding possibility to move 3D elements with dynamic update of fitness score.

## Definitive version

The public version of the project would then add the remaining features in order to provide the full user experience as described in the introduction :
- selection of the land on a map as well as its digitalisation ;
- placement of fixed elements on the land ;
- allowing selection of multiple instances of the same element ;
- relevant suggestions made on the demo version.

