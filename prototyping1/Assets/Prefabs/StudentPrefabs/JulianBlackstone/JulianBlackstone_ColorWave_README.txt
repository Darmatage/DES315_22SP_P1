Welcome to the Color Wave README File!
  
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
