Welcome to the Color Wave README File!
  
This system allows the player to touch color totems across
a level in order to reveal or hide objects for short duration.

--- Notes under usage ---
Keep in contact with me about the featuer if you use it!
My email is j.blackstone@digipen.edu, and you can always
contact me on teams!
    
Currently, the system uses prefab color totems and walls.
This could change in the future, but during functionality
changes I am keeping it limited to prefabs. The goal this
upcoming week is to make it so that the system is entirely
applicable to ANY OBJECT.

You place down a totem, change it's color, change it's
COLOR TAG, and then make sure any walls match the COLOR TAG
specifically. This is important. The current tag colors are
Red, Blue, and Green. They will output their respective wave
on player collision.

If you want to hide your object on color wave hit instead
of reveal it, then hit the respective check marks in the
details section of the colorsystem script that is attached
to the object!


DISABLE VOLUMES:
The disable volume is used for the sake of disabling a block
set if the player is in an area where them not disabling would
softlock the game. You can drag the instances of the walls or
objects you want to activate on player collision into the volume's
reference array in the details panel.

-- Side Note --
If you really want to use the currnet system for making any object 
colored, you must add a CircleCollider2D with the trigger checkbox 
marked, and add the "colorsystem" script to the desired object as well.

If you want the object to have proper collisons even when not 
visible, SEPARATELY have a BoxCollider2D while I am working out
the kinks in applying this to any object without requiring as
much complexity in implementation.

Thanks for reading through this!

 - Julian Blackstone (j.blackstone@digipen.edu)
