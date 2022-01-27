To use Destructible Objects:

Place a "MattWalker_DestructibleObject" prefab in the scene.
You can replace the sprite, so the object looks like whatever you want it to be!  See the "Object Sprite" variable below

Public Variables:
---------------------------------
1. "Contained Object" - This GameObject will be dropped (instantiated) when the DestructibleObject is destroyed.
	                    It can be any gameobject prefeb (item pickup, enemy, etc)
4. "Object Sprite"    - This sprite used for this object (default is a wooden crate).  Feel free to change this!



To use Health Pickups:

The prefab "MattWalker_PickupHealth" will restore 25 hp to the player when they touch it heart.  You can use this prefab in the
"Contained Object" variable desribed above so that some objects will drop a heart when you destroy them.



To use Coins and the Score System:

1. Place the prefab named "MattWalker_PlayerScoreUI" in the hierarchy beneath "PauseMenu" in the GameHandlerCanvas.  
2. The prefab "MattWalker_PickupCoin" will increase the score upon pickup.  You can use this prefab in the
"Contained Object" variable desribed above so that some objects will drop a coin when you destroy them.
