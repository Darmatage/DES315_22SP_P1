Prefabs:
TilemapQuicksand: The feature that sinks the player if they press any key while in the tile, and moves the player if no key is being pressed.
Sinking Pop Up: UI text for quicksand tile.
Moving Pop UP: UI text for quicksand tile.

How to use:
1. Place quicksand prefab into the hierarchy under the "Grid" game object folder.
2. Place the SinkingPopUp and MovingPopUp game object under the "GameHandlerCanvas/Canvas."
3. Select the quicksand prefab in the hierarchy and it should in the scene view of the actual game of unity, pop up a "Open Tile Palette" icon.
   3.a If it does not pop this icon up, go to "Window/2D/Tile Pallete.

4. While having the quicksand prefab in the hierarchy, you can now place the quicksand tiles using the Tile Pallete any way you want in your level.

Note: Do not rename any of these prefabs, or it will cause null references in the quicksand script.
