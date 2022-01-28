READMME for Switch Color Blocks

COLOR BLOCKS - Blocks the phase into existance when the active color is matching the blocks.
1. Put "Taro_ColorSwitchManager" anywhere in your scene
	1a. This can be used to set the default color setting for your color blocks
	1b. Up to four colors available for now
2. For each color of tiles you want in your scene in your, to the "Grid" (Part of default student scene), add a "Taro_TilemapColorTriggerBlockStub"
3. Rename the object in the scene to whatever you like and on properties for each "Taro_TilemapColorTriggerBlockStub" select the color it will activate
4. Then for each of those "Taro_TilemapColorTriggerBlockStub" on the "Tilemap" component in the inspector change the color to whatever is desired.
5. After this is done you are ready to place blocks using whatever tile pallette you wish to use with the tilemap tool.
-Note: alternatively standalone red and blue prefabs have been provided if that is more convenient for some reason. 
-      "TaroKumagai_RedBlock" "TaroKumagai_BlueBlock" 

COLOR SWITCHES - Switches to change the active color
1. "Taro_Switch..." prefab anywhere in the scene.
2. In the inspector displays the color for which the switch will correspond to.

COLOR TRIGGERS - Objects that can interact with switches to change the active color
1. To any object attach the following component "Taro_ColorSwitchTrigger".
2. This should now allow the object to trigger switches. (REQUIRES SOME FORM OF COllIDER2D)
