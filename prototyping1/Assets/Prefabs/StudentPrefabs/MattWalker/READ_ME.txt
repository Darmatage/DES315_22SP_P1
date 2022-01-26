To use Destructible Objects:

Place a "DestructibleObject" prefab in the scene.
You can replace the sprite, so the object looks like whatever you want it to be!  See "Object Sprite" variable below

Public Variables:
---------------------------------
1. "Contained Object" - This GameObject will be dropped (instantiated) when the DestructibleObject is destroyed.
	                    It can be any gameobject prefeb (item pickup, enemy, etc)
2. "Prompt Object UI" - don't change this
3. "Explosion Prefab" - don't change this, unless you want a different explosion animation
4. "Object Sprite"    - This sprite used for this object (default is a wooden crate)



To use Health Pickups:

The prefab "PickupHealth" will restore 25 hp to the player when they touch it heart.  You can use this prefab in the
"Contained Object" variable desribed above so that some objects will drop a heart when you destroy them.
