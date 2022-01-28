Hello. Welcome to Garry Chen's Fog of War. Following steps will show how to integrate the feature to your project:


Basic:

. Add GarryChen_FogOfWarCanvas to GameHandlerCanvas

. Make sure Canvas size is big enough to cover your level

. Add GarryChen_FogOfWarMainCamera and GarryChen_FogOfWarSecondaryCamera to the scene.

. Replace Player with GarryChen_FogOfWarPlayer.

. Player vision range can be change by modifing the scale of "Vision" child.

Tourch:

. A permanent light source. Work the same as player vision.

Lighted Area:

. Area covered will be permanert lighted.

High Ground Fog:

. Player vision will only works when stand on the high ground area.

Custom Vision:

. You can add "Vision" to any gameobject of your choice and it works the same as player vision.

Helper script:

. Provide a global function to change a gameobject's vision. You can 



