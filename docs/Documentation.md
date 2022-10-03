
- [**SIMPLE GAME ENGINE - C# EDITION**](#simple-game-engine---c-edition)
- [**General Notes**](#general-notes)
- [**--- CentralController.cs ---**](#----centralcontrollercs----)
  - [**```class``` CentralController**](#class-centralcontroller)
    - [**Properties**](#properties)
    - [**Constructors**](#constructors)
    - [**Methods**](#methods)
- [**--- Display.cs ---**](#----displaycs----)
  - [**```class``` Display**](#class-display)
    - [**Constructors**](#constructors-1)
    - [**Methods**](#methods-1)
- [**--- Input.cs ---**](#----inputcs----)
  - [**``abstract`` ``class`` GameAction**](#abstract-class-gameaction)
    - [**Methods**](#methods-2)
  - [**``abstract`` ``class`` SpriteAction: GameAction**](#abstract-class-spriteaction-gameaction)
    - [**Constructors**](#constructors-2)
    - [**Methods**](#methods-3)
  - [**```class``` Input**](#class-input)
    - [**Constructors**](#constructors-3)
    - [**Methods**](#methods-4)
- [**-- Logger.cs ---**](#---loggercs----)
  - [**```class``` Logger**](#class-logger)
    - [**Constructors**](#constructors-4)
    - [**Methods**](#methods-5)
- [**--- Spawner.cs ---**](#----spawnercs----)
  - [**```class``` Spawner**](#class-spawner)
    - [**Constructors**](#constructors-5)
    - [**Methods**](#methods-6)
- [**--- Sprite.cs ---**](#----spritecs----)
  - [**```struct``` Pixel**](#struct-pixel)
    - [**Properties**](#properties-1)
    - [**Constructors**](#constructors-6)
  - [**```struct``` CollisionsInfo**](#struct-collisionsinfo)
    - [**Properties**](#properties-2)
  - [**```class``` Sprite**](#class-sprite)
    - [**Properties**](#properties-3)
    - [**Constructors**](#constructors-7)
    - [**Methods**](#methods-7)
  - [**```class``` Pawn: Sprite**](#class-pawn-sprite)
    - [**Properties**](#properties-4)
    - [**Constructors**](#constructors-8)
    - [**Methods**](#methods-8)
- [**--- SpriteController.cs ---**](#----spritecontrollercs----)
  - [**```abstract``` ```class``` Controller**](#abstract-class-controller)
    - [**Constructors**](#constructors-9)
    - [**Methods**](#methods-9)
  - [**```abstract``` ```class``` SpriteController: Controller**](#abstract-class-spritecontroller-controller)
    - [**Constructors**](#constructors-10)
    - [**Methods**](#methods-10)

# **SIMPLE GAME ENGINE - C# EDITION**
 
This is a port/redesign of my original text-based game engine written in Python. It allows for simple, text-based games to be made in the terminal. Emphasis on simple; there is a lot it cannot do right now, but that shouldn't deter you from using your imagination. I did my best to ensure a modular design and current documentation so that anyone else (but most likely future me) could make improvements.

# **General Notes**
 
This game engine uses a coordinate system such that the origin (0, 0) is in the *top, left-hand* corner.
 
Sprites can be created using the ```Sprite``` class and its descendants. The constructor accepts a file path to a text file containing ascii art. Any whitespace around the art will be ignored.

---
# **--- CentralController.cs ---**

## **```class``` CentralController**

**NOTE: Other classes in this engine rely on the existence of an object of this class. Therefore, an object of this class should always be created first, and there should only ever be one instance of this class per game.**
 
A central controller for the game. Contains methods for starting the mainloop, allowing for pausing/resuming threads, and de-spawning/spawning sprites. While most of the time you will want to use the ```Spawner``` class for spawning and despawing sprites, there are methods here that allow you to manually spawn and de-spawn sprites if needed. Note, however, if you want to manually spawn/despwawn sprites to be controlled by a ```Controller``` or ```SpriteController```class, you will have to also need manually create a controller object and put it on a thread. You will then need to pass both a reference to the thread and the controller object you created using ```AddController()``` and remove it when you're done with ```RemoveController()``` to allow this class to work properly.
 
### **Properties**
 
* **```List<Sprite>``` Sprites**: A list of sprites that are currently being drawn to the screen. This property is read-only. In order to add or remove sprites from this list, you must use ```AddSprite()``` or ```DeleteSprite()``` respectively.
 
### **Constructors**
 
 * **CentralController()**
 
 
### **Methods**
 
* **```void``` AddSprite(```Sprite``` sprite)**
 
  Adds a sprite to the ```Sprites``` list. In order for any sprite to show up on screen, it must be added with this method.
 
 
* **```void``` DeleteSprite(```Sprite``` sprite)**
 
    Removes a sprite from the ```Sprites``` list. This has the effect of "de-spwaning" the sprite. NOTE: This method just stops the sprite from being drawn top the screen and it's controller will still be running.
 
 
* **```void``` DeleteAllSprites()**
 
    Clears the ```Sprites``` list, deleting all sprites from the display.
 
 
* **```bool``` AddController(```Controller``` controller, ```Thread``` thread)**
 
    Makes ```CentralController``` aware of a Controller object created manually. Returns ```false``` if the controller is already in the dictionary. The ```Spawner``` class already takes care of this task for you when spawning a sprite.
 
* **```bool``` RemoveController(```string``` controllerId)**
 
    Removes a controller from  ```CentralController```'s internal dictionary. Returns ```false``` if the controller is not in the dictionary. A ```SpriteController``` class automatically preforms this task when it's despawn conditions are met.
 
* **```bool``` PauseController(```string``` controllerId)**
 
    Pauses the controller specified by ```controllerId```. Returns ```false``` of the controller is not found.

* **```bool``` ResumeController(```string``` controllerId)**
 
    Resumes the controller specified by ```controllerId```. Returns ```false``` of the controller is not found.

* **```void``` PauseAll()**
 
    Pauses all controllers currently running.

* **```void``` ResumeAll()**
 
    Resumes all controllers that are stopped

* **```void``` EndGame()**
 
    Ends the game.

* **```void``` mainloop(```Display``` displayObj, ```Input``` inputObj, ```SpriteController[]?``` spriteControllers = ```null```)**
 
    Initializes the main game loop.


---
# **--- Display.cs ---**

## **```class``` Display**
 
Represents the main display of the game. There should only ever be one Display object per game. Sprites will be drawn to the screen automatically, provided ```Refresh()``` has been called. Though individual characters and Sprite objects can be drawn to the screen with ```Draw()```, ```DrawSprite()```, and displayed with ```Show()```, these changes will not show up in during the main game loop. These are methods used internally by ```Refresh()``` but made public due to their usefulness for visualizing sprite positions on the screen for debuging.
 
### **Constructors**
 
 * **Display(```CentralController``` CentralController, ```int``` rowCount, ```int``` columnCount, ```char``` background=' ')**

    **centralController**: A reference to the game's ```CentralController``` object.
 
    **rowCount**: The number of rows the display will have
 
    **columnCount**: The number of columns the display will have
 
    **background**: The background character of the display. The default is a space.
 
 
### **Methods**

* **```void``` Draw(```int``` x, ```int``` y, ```char``` character)**
 
    Draws a single character to the internal display buffer. Throws IndexOutOfRangeException if the character is drawn outside the screen bounds.
 
 
* **```void``` DrawSprite(```Sprite``` sprite)**
 
    Draws a single ```Sprite``` object to the screen. This does not add the sprite to the ```CentralController.Sprites``` list.

* **```void``` ClearDisplay()**

    Clears the internal display buffer.
 
* **```void``` Show()**
 
    Shows whatever is currently in the internal display buffer.
 
* **```void``` Refresh()**
 
    Initializes the display. Do not call this method on its own as you will not be able to control the game. ```CentralController.mainloop``` will do this for you when you call it.
 
---

# **--- Input.cs ---**

## **``abstract`` ``class`` GameAction**
Allows for the binding of keyboard input to an action to be performed.

### **Methods**
 
* **```abstract``` ```void``` ExecuteAction()**
 
    Override this method with the actions to be performed when a key is pressed.

---

## **``abstract`` ``class`` SpriteAction: GameAction**
Allows for the binding of keyboard input to an action to be performed by a ```Sprite``` object.
 
 
### **Constructors**
 
**SpriteAction(```Pawn``` sprite)**
 
* **sprite**: The sprite to manipulate.
 
### **Methods**
 
* **```abstract``` ```void``` ExecuteAction()**
 
    Override this method with the actions to be performed by the sprite when a key is pressed.
 
---
 
## **```class``` Input**
 
Listens for user input from the keyboard and executes actions defined by a ```SpriteAction``` class until the ```exitKey``` is pressed.
 
### **Constructors**
* **Input()**
 
### **Methods**
* **```void``` BindAction(```ConsoleKey``` key, ```GameAction``` action)**
 
    Binds a ```GameAction``` or a ```SpriteAction```  class to the specified key.

* **```void``` BindAction(```KeyValuePair<ConsoleKey, GameAction>``` action)**
 
    Overload that allows for a ```KeyValuePair<ConsoleKey, GameAction>``` to passed instead.
 
* **```KeyValuePair<ConsoleKey, GameAction>``` UnbindAction(```ConsoleKey``` key)**

    Unbinds a ```GameAction``` or a ```SpriteAction``` class from the specified key and returns the binding.
 
* **```void``` Listen()**

    Listens for keyboard input and executes bound actions until ```exitKey``` is pressed. Do not call this method on its own or your game may not work. ```CentralController.mainloop``` will do this for you when you call it.
 
---
# **-- Logger.cs ---**

## **```class``` Logger**

A simple convince class for basic logging. 

### **Constructors**
* **Logger(```string``` logFile)**
  
    **logFile**: Filepath to log messages to. If the file does not exist, a new file is created when ```Log()``` is first called.

### **Methods**
* **```async``` ```void``` Log(```string``` message)**
   
   Appends a new message to the file specified by logFile.

---
 
# **--- Spawner.cs ---**
 
## **```class``` Spawner**
 
Responsible for spawning in sprites.
 
### **Constructors**
 
* **Spawner(```Type``` spriteCls, ```object[]``` spriteAttr, ```Type``` spriteControllerCls, ```object?[]``` controllerAttr, ```Display``` display, ```CentralController``` centralController)**
 
    **spriteCls**: The type of the ```Sprite``` class you want to spawn in (usually a ```Pawn```) using ```System.Reflection.typeof()```.
 
    **spriteAttr**: An ```object``` array containing the exact number and order of arguments to pass to the spriteCls constructor (including default parameters).
 
    **spriteControllerCls**: The type of the ```spriteController``` class you want to control your spawned sprite using ```System.Reflection.typeof()```.
 
    **controllerAttr**: An ```object``` array containing the exact number and order of arguments to pass to the spriteControllerCls constructor (including default parameters). Pass ```null``` in place of the ```sprite``` argument since no sprite object exists yet.
 
    **display**: A reference to a ```Display``` object.

    **centralController**: A reference to the game's ```CentralController``` object.
 
### **Methods**
* **```System.Thread``` SpawnSprite(```int``` startx, ```int``` starty)**
 
    Spawns the sprite at the coordinates indicated by startx and starty and initiates its behavior. Returns a reference to the thread the sprite's controller is running on.
 
---
 
# **--- Sprite.cs ---**
 
## **```struct``` Pixel**
Represents a single character and its coordinates within a ```Sprite``` object.
 
### **Properties**
* ```int``` **X**: Gets and sets the x coordinate of the pixel.
* ```int``` **Y**: Gets and sets the x coordinate of the pixel.
* ```char``` **Character**: Gets and sets the character displayed by the pixel.
 
### **Constructors**
 
* **Pixel(```int``` x, ```int``` y, ```char``` character**
 
    **x**: The x-coordinate of the pixel.
 
    **y**: The y-coordinate of the pixel.
 
    **character**: The character to display.

---

## **```struct``` CollisionsInfo**
A struct containing information about a collision between two sprites.
 
### **Properties**
 
* **```bool``` CollisionOccurred**: A flag indicating if a collision occurred. Set to ```true``` if a collision occurred, ```false``` if not.
 
* **```Sprite``` Entity**: Contains a reference to the entity that triggered the collision, if a collision occurred. Set to ```null``` if no collision occurred.
  
---
 
## **```class``` Sprite**
Represents a basic game object. Sprites consist of a list of ```Pixel``` structs that determine the shape and location of the sprite.

### **Properties**
 
* **```List<Pixel>``` Coords**: Gets and sets the list of coordinates that define the sprite.
 
*  **```string``` SpriteId**: Gets a string identifying the sprite. This property is read-only.
 
*  **```bool``` HasCollisions**: A flag that determines if the sprite has collisions or not. Set to ```true``` for collisions, ```false``` for no collisions.
 
*  **```int``` StackOrder**: Determines the stack order of the sprite. Sprites with lower values will appear under sprites with higher values.

*  **```CollisionsInfo``` LastCollided**: Contains information about what last hit the sprite.
  
*  **```bool``` IsDespawned**: Optional property that can be used to indicate if the sprite has been despawned.
 
### **Constructors**
 
* **Sprite(```string``` file, ```int``` startx, ```int``` starty, ```string``` spriteId, ```int``` stackOrder=0, ```bool``` collisions=```true```)**
 
    **file**: A text file containing ascii art to convert into a sprite.
 
    **startx**: The initial starting x-position of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.
 
    **starty**: The initial starting y-position of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.
 
    **spriteid**: A unique identifier for the sprite
 
    **collisions**: Pass ```true``` for collisions, ```false``` for no collisions.
 
### **Methods**
 
* **```static``` ```int``` CompareByStackOrder(```Sprite``` sprite1, ```Sprite``` sprite2)**
 
    Returns an integer representing the comparison between ```sprite1.SpriteID``` and ```sprite2.SpriteId```.
 
* **```void``` MoveTo(```int``` x, ```int``` y)**
 
    Moves the sprite to the position indicated by the x and y arguments. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.

---

## **```class``` Pawn: Sprite**
Represents a sprite with the ability to move.
 
### **Properties**
 
* **```List<Pixel>``` Coords**: Gets and sets the list of coordinates that define the sprite.
 
*  **```string``` SpriteId**: Gets a string identifying the sprite. This property is read-only.
 
*  **```bool``` HasCollisions**: A flag that determines if the sprite has collisions or not. Set to ```true``` for collisions, ```false``` for no collisions.
 
*  **```int``` StackOrder**: Determines the stack order of the sprite. Sprites with lower values will appear under sprites with higher values.

*  **```CollisionsInfo``` LastCollided**: Contains information about what last hit the sprite. The ```MoveNorth()```, ```MoveSouth()```, ```MoveEast()```, ```MoveWest()``` methods automatically sets this property for the sprite that was hit. This can be used to aid in despawning. For example, now ```SpriteController.CheckDespawnConditions()``` can check this property to determine if a despawn needs to occur.

*  **```bool``` IsDespawned**: Optional property that can be used to indicate if the sprite has been despawned.
 
### **Constructors**
 
* **Sprite(```string``` file, ```int``` startx, ```int``` starty, ```string``` spriteId, ```CentralController``` centralController, ```int``` stackOrder=0,  ```bool``` collisions=```true```)**
 
    **file**: A text file containing ascii art to convert into a sprite.
 
    **startx**: The initial starting x-position of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.
 
    **starty**: The initial starting y-position of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.
 
    **spriteid**: A unique identifier for the sprite.

    **centralController**: A reference to the game's ```CentralController``` object.

    **stackOrder**: Controls in what order sprites will be drawn. Sprites with a higher stack order will appear on top of sprites with lower stack order.
 
    **collisions**: Pass ```true``` for collisions, ```false``` for no collisions.
 
### **Methods**
 
* **```static``` ```int``` CompareByStackOrder(```Sprite``` sprite1, ```Sprite``` sprite2)**
 
    Returns an integer representing the comparison between ```sprite1.SpriteID``` and ```sprite2.SpriteId```.
 
* **```void``` MoveTo(```int``` x, ```int``` y)**
 
    Moves the sprite to the position indicated by the x and y arguments. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.
 
* **```CollisionInfo``` MoveNorth(```int``` distance)**
 
    Moves the sprite up on the screen and then performs a collision check. The result of the check is then returned.
 
* **```CollisionInfo``` MoveSouth(```int``` distance)**
 
    Moves the sprite down on the screen and then performs a collision check. The result of the check is then returned.
 
* **```CollisionInfo``` MoveWest(```int``` distance)**
 
    Moves the sprite right on the screen and then performs a collision check. The result of the check is then returned.
 
* **```CollisionInfo``` MoveEast(```int``` distance)**
 
    Moves the sprite up left the screen and then performs a collision check. The result of the check is then returned.
 
---
 
# **--- SpriteController.cs ---**

## **```abstract``` ```class``` Controller**
An basic controller template for controlling unique entities or if you need the sprite's behavior cycle to work a little differently. In most cases though ```SpriteController``` class should be used.
 
### **Constructors**
 
* **Controller(```CentralController``` centralController, ```int``` speed)**

    **centralController**: A reference to the game's ```CentralController``` object.
 
    **speed**: An integer representing how many milliseconds to delay the behavior cycle. Controls the speed at which the controlled sprite operates.
 
### **Methods**
* **```abstract``` ```void``` Behavior()**
 
    This method determines the actions an object will perform.
 
* **```virtual``` ```bool``` CheckDespawnConditions()**

    Method for defining when an object should despawn.
 
* **```abstract``` ```void``` Initialize()**
 
    Put code for initializing the object's behavior cycle here. While it is ultimately up to you how this method runs I strongly recommend you use this template as a starting point:

```
bool despawn;
while (true){
    if (Stop){
        try{
            Thread.Sleep(Timeout.Infinite);
        }
        catch(ThreadInterruptedException){
            continue;
        }
    }
    try{
        Thread.Sleep(_speed);
    }
    catch(ThreadInterruptedException){
        /* 
        This exception needs to be caught here or the game will 
        crash if CentralController.ResumeAll() or ResumeController() is spammed. 
        */
    }  
    
    Behavior();
    despawn = CheckDespawnConditions();
    if (despawn){
        // Put code for handing a despawn here
        return;
    }
}
```

    
NOTE: If you want this controller to be controllable from ```CentralController``` you will need to include this code snippet at the start fo your while loop:

``` 
if (Stop){
    try{
        Thread.Sleep(Timeout.Infinite);
    }
    catch(ThreadInterruptedException){
        continue;
    }
}
try{
    Thread.Sleep(_speed);
}
catch(ThreadInterruptedException){
    /* 
    This exception needs to be caught here or the game will 
    crash if CentralController.ResumeAll() or ResumeController() is spammed. 
    */
}
```
 
## **```abstract``` ```class``` SpriteController: Controller**
Independently controls a sprite.
 
### **Constructors**
 
* **SpriteController(```Pawn?``` sprite, ```CentralController``` centralController, ```int``` speed)**
 
    **sprite**: A ```Pawn``` object to be controlled.
 
    **centralController**: A reference to the game's ```CentralController``` object.
 
    **speed**: An integer representing how many milliseconds to delay the behavior cycle. Controls the speed at which the controlled sprite operates.
 
### **Methods**
* **```abstract``` ```void``` Behavior()**
 
    This method determines the actions a sprite will take during its behavior cycle.
 
* **```virtual``` ```bool``` CheckDespawnConditions()**

    This method tells the sprite controller when to despawn the sprite during its behavior cycle. By default it simply returns ```false```.
 
* **```void``` Initialize()**
 
    Initializes the behavior cycle. The speed of this cycle is determined by the ```speed``` attribute.
