
Feature:
Grabbing/Throwing Enemies.
The player can grab and throw enemies.
Thrown enemies will destroy other enemies or "BraedanBreakableWall" objects.


Controls:
Left mouse click to grab enemies.
Right mouse click to throw enemies.
Enemy is thrown in current movement direction, or direction player is facing if not moving.

Prefabs-
-BraedanBreakableWall
Use: Place as obstacles in level, destroyed when hit by thrown enemy

-BraedanControlsText
Use: Place as child object of Canvas to display controls

-BraedanGrabPrefab
Use: Place as child object of player to allow grabbing/throwing, can change speed of thrown enemies here

-BraedanProjectile
Use: Handles collisions for the thrown enemies, automatically used by BraedanGrabPrefab

Other Notes:
-Projectiles also destroy themselves when hitting the "TilemapWalls" object
-Player grabs objects from the "enemy" layer and thrown enemies destroy objects on "enemy" layer
-The tag "BraedanBreakable" can be used to allow projectiles to destroy other objects
-BraedanGrabPrefab and BraedanProjectile have the tag "monsterShooter" to allow enemy projectiles to pass through them.
-I will also likely be adding a "BraedanEnemySpawner" prefab, that will spawn enemies at it's location, useful if specific walls need to be broken to complete the level  