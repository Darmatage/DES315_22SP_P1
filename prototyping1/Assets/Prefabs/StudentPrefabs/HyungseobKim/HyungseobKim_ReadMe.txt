When a player character reaches "HyungseobKim_Trigger", it will move all registered walls.

You need to assign the trigger to walls to bind walls and triggers.
There is a variable for trigger under wall prefabs.
Drag the trigger object to the variable "Trigger" under HyungseobKim_Wall_Horizontal or HyungseobKim_Wall_Vertical.

Horizontal walls move horizontally, and Verticals are as well.

Amount variable means the amount to move when it is called.
A positive number means right for horizontal walls and up for vertical walls. The distance of one tile is 1.

You can schedule multiple movements for one wall.
Just add components as much as you want and assign triggers for each of them.

There are various prefabs for different sizes, but you may need to make your own for your level.