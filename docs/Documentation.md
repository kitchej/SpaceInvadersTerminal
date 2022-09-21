
- [**SIMPLE GAME ENGINE - C# EDITION**](#simple-game-engine---c-edition)
- [**General Notes**](#general-notes)
  - [# **--- Display.cs ---**](#-----displaycs----)
  - [**```class``` Display**](#class-display)
    - [**Properties**](#properties)
    - [**Constructors**](#constructors)
    - [**Methods**](#methods)
- [**--- Input.cs ---**](#----inputcs----)
  - [**``abstract`` ``class`` GameAction**](#abstract-class-gameaction)
    - [**Methods**](#methods-1)
  - [**``abstract`` ``class`` SpriteAction: GameAction**](#abstract-class-spriteaction-gameaction)
    - [**Constructors**](#constructors-1)
    - [**Methods**](#methods-2)
  - [**```class``` Input**](#class-input)
    - [**Constructors**](#constructors-2)
    - [**Methods**](#methods-3)
- [**-- Logger.cs ---**](#---loggercs----)
  - [**```class``` Logger**](#class-logger)
    - [**Constructors**](#constructors-3)
    - [**Methods**](#methods-4)
- [**--- Mainloop.cs ---**](#----mainloopcs----)
  - [**```static``` ```class``` Mainloop**](#static-class-mainloop)
    - [**Methods**](#methods-5)
- [**--- Spawner.cs ---**](#----spawnercs----)
  - [**```class``` Spawner**](#class-spawner)
    - [**Constructors**](#constructors-4)
    - [**Methods**](#methods-6)
- [**--- Sprite.cs ---**](#----spritecs----)
  - [**```struct``` Pixel**](#struct-pixel)
    - [**Properties**](#properties-1)
    - [**Constructors**](#constructors-5)
  - [**```struct``` CollisionsInfo**](#struct-collisionsinfo)
    - [**Properties**](#properties-2)
  - [**```class``` Sprite**](#class-sprite)
    - [**Properties**](#properties-3)
    - [**Constructors**](#constructors-6)
    - [**Methods**](#methods-7)
  - [**```class``` Pawn: Sprite**](#class-pawn-sprite)
    - [**Properties**](#properties-4)
    - [**Constructors**](#constructors-7)
    - [**Methods**](#methods-8)
- [**--- SpriteController.cs ---**](#----spritecontrollercs----)
  - [**```abstract``` ```class``` Controller**](#abstract-class-controller)
    - [**Constructors**](#constructors-8)
    - [**Methods**](#methods-9)
  - [**```abstract``` ```class``` SpriteController: Controller**](#abstract-class-spritecontroller-controller)
    - [**Constructors**](#constructors-9)
    - [**Methods**](#methods-10)

# **SIMPLE GAME ENGINE - C# EDITION**
 
This is a port/redesign of my original text-based game engine written in Python. It allows for simple, text-based games to be made in the terminal. Emphasis on simple; there is a lot it cannot do right now, but that shouldn't deter you from using your imagination. I did my best to ensure a modular design and current documentation so that anyone else (but most likely future me) could make improvements.

# **General Notes**
 
This game engine uses a coordinate system such that the origin (0, 0) is in the *top, left-hand* corner.
 
Sprites can be created using the ```Sprite``` class and its descendants. The constructor accepts a file path to a text file containing ascii art. Any whitespace around the art will be ignored.
 
# **--- Display.cs ---**
---
## **```class``` Display**
 
Represents the main display of the game. There should only ever be one Display object per game. Sprites will be drawn to the screen automatically, provided ```Refresh()``` has been called. Sprites can be added and removed from the game through the ```AddSprite()``` and the ```RemoveSprite()``` methods. Though individual characters and Sprite objects can be drawn to the screen with ```Draw()```, ```DrawSprite()```, and displayed with ```Show()```, these changes will not show up in during the main game loop. These are methods used internally by ```Refresh()``` but made public due to their usefulness for visualizing sprite positions on the screen for debuging.
 
### **Properties**
 
* **```List<Sprite>``` Sprites**: A list of sprites to be drawn to the screen. This property is read-only. In order to add or remove sprites from this list, you must use AddSprite() or RemoveSprite() respectively.
 
### **Constructors**
 
 * **Display(```int``` rowCount, ```int``` columnCount, ```char``` background=' ')**
 
    **rowCount**: The number of rows the display will have
 
    **columnCount**: The number of columns the display will have
 
    **background**: The background character of the display. The default is a space.
 
 
### **Methods**
 
* **```void``` AddSprite(```Sprite``` sprite)**
 
  Adds a sprite to the ```Sprites``` list of the display. In order for any sprite to show up on screen, it must be added with this method.
 
 
* **```void``` DeleteSprite(```Sprite``` sprite)**
 
    Removes a sprite from the ```Sprites``` list.
 
 
* **```void``` Draw(```int``` x, ```int``` y, ```char``` character)**
 
    Draws a single character to the internal display buffer. Throws IndexOutOfRangeException if the character is drawn outside the screen bounds.
 
 
* **```void``` DrawSprite(```Sprite``` sprite)**
 
    Draws a single ```Sprite``` object to the screen. This does not add the sprite to the ```Display.Sprites``` list.
 
* **```void``` Show()**
 
    Shows whatever is currently in the internal display buffer.
 
* **```void``` Refresh()**
 
    Begins the refresh cycle. The internal display buffer is cleared once every cycle and all sprites in the Sprites list are redrawn. At the moment, if called outside of ```Mainloop.mainloop```, it will loop forever until the program is stopped with Ctr+C.
 
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
* **Input(```ConsoleKey``` exitKey)**
 
    **exitKey**: The key that will trigger an exit from the game.
 
### **Methods**
* **```void``` BindAction(```ConsoleKey``` key, ```SpriteAction``` action)**
 
    Binds a ```SpriteAction``` class to the specified key.
 
* **```void``` UnbindAction(```ConsoleKey``` key)**

    Unbinds a ```SpriteAction``` class from the specified key.
 
* **```void``` Listen()**

    Listens for keyboard input and executes bound actions until ```exitKey``` is pressed.
 
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
 
# **--- Mainloop.cs ---**
 
## **```static``` ```class``` Mainloop**
 
### **Methods**
* **```static``` ```void``` mainloop(```Display``` displayObj, ```Input``` inputObj, ```SpriteController[]?``` spriteControllers = ```null```)**
 
    Initializes the main game loop. The game loop will end when the key defined by ```Input.exitKey``` is pressed.
 
---

# **--- Spawner.cs ---**
 
## **```class``` Spawner**
 
Responsible for spawning in sprites.
 
### **Constructors**
 
* **Spawner(```Type``` spriteCls, ```object[]``` spriteAttr, ```Type``` spriteControllerCls, ```object?[]``` controllerAttr, ```Display``` display)**
 
    **spriteCls**: The type of the ```Sprite``` class you want to spawn in (usually a ```Pawn```) using ```System.Reflection.typeof()```.
 
    **spriteAttr**: An ```object``` array containing the exact number and order of arguments to pass to the spriteCls constructor (including default parameters).
 
    **spriteControllerCls**: The type of the ```spriteController``` class you want to control your spawned sprite using ```System.Reflection.typeof()```.
 
    **controllerAttr**: An ```object``` array containing the exact number and order of arguments to pass to the spriteControllerCls constructor (including default parameters). Pass ```null``` in place of the ```sprite``` argument since no sprite object exists yet.
 
    **display**: A reference to a ```Display``` object.
 
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
 
* **Sprite(```string``` file, ```int``` startx, ```int``` starty, ```string``` spriteId, ```Display``` display, ```int``` stackOrder=0,  ```bool``` collisions=```true```)**
 
    **file**: A text file containing ascii art to convert into a sprite.
 
    **startx**: The initial starting x-position of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.
 
    **starty**: The initial starting y-position of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.
 
    **display**: A reference to a ```Display``` object.
 
    **spriteid**: A unique identifier for the sprite.
 
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
An basic controller template for controlling entities other than a single sprite (such as a group of sprites that you want to move as a single entity for example). If you only want to control a single sprite, you should use the ```SpriteController``` class instead.
 
### **Constructors**
 
* **Controller(```Display``` display, ```int``` speed)**

    **display**: A ```Display``` object.
 
    **speed**: An integer representing how many milliseconds to delay the behavior cycle. Controls the speed at which the controlled sprite operates.
 
### **Methods**
* **```abstract``` ```void``` Behavior()**
 
    This method determines the actions an object will perform.
 
* **```virtual``` ```bool``` CheckDespawnConditions()**

    Method for defining when an object should despawn.
 
* **```abstract``` ```void``` Initialize()**
 
    Put code for initializing the object's behavior cycle here.


 
## **```abstract``` ```class``` SpriteController: Controller**
Independently controls a sprite using code contained in ```Behavior()``` and despawns the sprite when the conditions specified in ```CheckDespawnConditions()``` are met.
 
### **Constructors**
 
* **SpriteController(```Pawn?``` sprite, ```Display``` display, ```int``` speed)**
 
    **sprite**: A ```Pawn``` object to be controlled.
 
    **display**: A ```Display``` object.
 
    **speed**: An integer representing how many milliseconds to delay the behavior cycle. Controls the speed at which the controlled sprite operates.
 
### **Methods**
* **```abstract``` ```void``` Behavior()**
 
    This method determines the actions a sprite will take during its behavior cycle.
 
* **```virtual``` ```bool``` CheckDespawnConditions()**

    This method tells the sprite controller when to despawn the sprite during its behavior cycle. By default it simply returns ```false```.
 
* **```void``` Initialize()**
 
    Initializes the behavior cycle. The speed of this cycle is determined by the ```speed``` attribute. The cycle first calls ```Behavior()```, then it calls ```CheckDespawnConditions()```. If ```CheckDespawnConditions()``` returns ```true```, the sprite will be despawned and the cycle will end.