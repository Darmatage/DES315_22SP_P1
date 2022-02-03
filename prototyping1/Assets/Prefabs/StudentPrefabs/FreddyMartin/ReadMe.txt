To add the mirror enemy to your level:

- Drag and place the Mirror prefab into your level wherever you want it
- Add the "FreddyMartinPlayerScript" to the player and set CooldownPrefab on it to "FreddyMartinCooldownBar"

That's it, but there's some properties on the mirror you can mess with to change how it works:

- Mirror Enemy Prefab: Don't mess with this, it's the prefab used when spawning the enemy
- Max Health: The starting health of the mirror in the case that someone wants to damage and destroy the mirror, this is the health of the mirror, not the enemy that spawns
- Radius: The radius in unity units of the area around the mirror in which the mirror enemy will spawn
- Shard Damage: The damage that the mirror shards do
- Time Between Swaps: The number of seconds after which the player and the mirror enemy will swap
- Shards Dropped Between Swaps: The number of mirror shards dropped between swaps
- Shard Fade Time: The time in seconds it takes for the mirror shards to dissapear after spawning

On the player you can change how often the player can force a swap.