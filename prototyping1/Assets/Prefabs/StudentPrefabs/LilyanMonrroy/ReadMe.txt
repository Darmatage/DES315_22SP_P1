Prefabs:
TilemapQuicksand: The feature that sinks the player if they press any key while in the tile, and moves the player if no key is being pressed.
Sinking Pop Up: UI text for quicksand tile.
Moving Pop UP: UI text for quicksand tile.
MovingDust: Particle speed lines for the player when in quicksand.
SinkingMask: UI effect that slowly hides the player's body when moving in the quicksand.

How to use:
1. Place quicksand prefab into the hierarchy under the "Grid" game object folder.
2. Place the SinkingPopUp and MovingPopUp game object under the "GameHandlerCanvas/Canvas."
3. Place the MovingDust and SinkingMask prefabs into the Player object. i.e Make them children game objects in the Player.

4. Select the Player, and select both of it children game obejcts named Player_art and PlayerShadow_art.
   4.a In the inspector, it should show the SpriteRenderer component,  in that component click on the variable "Mask Interaction"
   4.b Change "Mask Interaction" variable to Visible Outside Mask. This allows the player to be hidden in the quicksand tiles to create a sinking effect.

3. Select the quicksand prefab in the hierarchy, in the scene view of the actual game of unity select the pop up "Open Tile Palette" icon.
   3.a If it does not pop this icon up, go to "Window/2D/Tile Pallete.
4. While having the quicksand prefab in the hierarchy, you can now place the quicksand tiles using the Tile Pallete any way you want in your level.

Note: Do not rename any of these prefabs, or it will cause null references in the quicksand script.
