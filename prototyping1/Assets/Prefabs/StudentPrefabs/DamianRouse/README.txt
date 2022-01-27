Mouse Pickup mechanic:

-Prefab:
--CommandBlock
---MousePickUpScript
----"LayersWithPickUpObject" to have the layers of objects you want to grab.

-Prefab:
--MonsterSlime
---CanGrabDefault
----seeBelow

-Prefab:
--PickupSkull
---CanGrabFillTile
----seeBelow

-Scripts:
--CanGrabDefault
***Let you pickup and put down objects with the mouse
---needs a collider component

---GridlessPlacement bool requirements:
-----nothing

---GridPlacement bool requirements:
----"SeperateSprite"(if using lock and sprite is a child)
----"PlacementMap" to the tilemap you want picked up objects to snap to
----"LockColor"(if using lock)
----"LockWhenAnchored" whether placing the object down once will make it unmovable

--CanGrabFillTile
***Lets you pickup and put down objects with mouse
***AND delete the tile of placementmap and place a tile in the background tilemap beneath it
---Same as default but:
----"FillerTile" Tile that will fill the replaced map
----"MapToFill" the background tilemap that'll visible when front tile is deleted

--DamianRouse_AI_Hijack
***Disables all scripts on anything when not close enough to the player
---"Player" set to the player
---"Distance" how far the player must be fore the scripts turn off
---"All" if you want all scripts disable
---"Specifics" if you want specific scripts disabled, it's a list

NOTES:
Do not use the ranged skull with the pickup mechanic