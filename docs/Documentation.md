# **SIMPLE GAME ENGINE - C# EDITION**

This is a port/redesign of my origianl text-based game engine written in Python. It allows for simple, text-based games to be made in the terminal. Empahsis on simple; there is a lot it cannot do right now, but that shouldn't deter you from using your imagination. I did my best to ensure a modular design and current documentation so that anyone else (or future me) could make improvements.

- [**SIMPLE GAME ENGINE - C# EDITION**](#simple-game-engine---c-edition)
  - [**General Notes**](#general-notes)
  - [## **--- Display.cs ---**](#-----displaycs----)
    - [**```class``` Display**](#class-display)
      - [**Properties**](#properties)
      - [**Constructors**](#constructors)
      - [**Methods**](#methods)
  - [**--- Input.cs ---**](#----inputcs----)
    - [**``abstract`` ``class`` SpriteAction**](#abstract-class-spriteaction)
      - [**Constructors**](#constructors-1)
      - [**Methods**](#methods-1)
    - [**```class``` Input**](#class-input)
      - [**Consturctors**](#consturctors)
      - [**Methods**](#methods-2)
  - [**--- Mainloop.cs ---**](#----mainloopcs----)
    - [**```static``` ```class``` Mainloop**](#static-class-mainloop)
      - [**Methods**](#methods-3)
  - [**--- Spawner.cs ---**](#----spawnercs----)
    - [**```class``` Spawner**](#class-spawner)
      - [**Consturctors**](#consturctors-1)
      - [**Methods**](#methods-4)
  - [**--- Sprite.cs ---**](#----spritecs----)
    - [**```struct``` Pixel**](#struct-pixel)
      - [**Properties**](#properties-1)
      - [**Consturctors**](#consturctors-2)
    - [**```struct``` CollisonsInfo**](#struct-collisonsinfo)
      - [**Properties**](#properties-2)
    - [**```class``` Sprite**](#class-sprite)
      - [**Properties**](#properties-3)
      - [**Constructors**](#constructors-2)
      - [**Methods**](#methods-5)
    - [**```class``` Pawn: Sprite**](#class-pawn-sprite)
      - [**Properties**](#properties-4)
      - [**Constructors**](#constructors-3)
      - [**Methods**](#methods-6)
  - [**--- SpriteContorller.cs ---**](#----spritecontorllercs----)
    - [**```abstract``` ```class``` SpriteController**](#abstract-class-spritecontroller)
      - [**Constructors**](#constructors-4)
      - [**Methods**](#methods-7)
## **General Notes**

This game engine uses a coordinate system such that the orgin (0, 0) is in the *top, lefthand* corner. 

Sprites can be created using the ```Sprite``` class and it's descendants. The constructor accepts a filepath to a text file contatining ascii art. Any whitespace around the art will be ignored.

Spawning utilizes reflection, as such it is nesscary to include ```using System.Refelection;``` to any file using the ```Spawner``` class as certain paramters need to be passed with the ```typeof()``` method.

## **--- Display.cs ---**
---
### **```class``` Display**

Represents the main display of the game. There should only ever be one Display object per game. Sprites will be drawn to the screen automiaclly, provided ```Refresh()``` has been called. Sprites can be added and removed from the game through the ```AddSprite()``` and the ```RemoveSprite()``` methods. Though individual characters and Sprite objects can be drawn to the screen with ```Draw()```, ```DrawSprite()```, and displayed with ```Show()```, these changes will not show up in during the main game loop. These are methods used internally, but made public due to their usefulness for visualizing sprite positions on the screen outside of the main gameloop.

#### **Properties**

* **```List<Sprite>``` Sprites**: A list of sprites to be drawn to the screen. This property is read-only. In order to add or remove sprites from this list, you must use AddSprite() or RemoveSprite() respectively.

#### **Constructors**

 * **Display(```int``` rowCount, ```int``` columnCount, ```char``` background=' ')**

    **rowCount**: The number of rows the display will have

    **columnCount**: The number of columns the display will have

    **background**: The background character of the display. The defualt is a space.


#### **Methods**

* **```void``` AddSprite(```Sprite``` sprite)**

  Adds a sprite to the ```Sprites``` list of the display. In order for any sprite to show up on screen, it must be added with this method.


* **```void``` DeleteSprite(```Sprite``` sprite)**

    Removes a sprite from the ```Sprites``` list.


* **```void``` Draw(```int``` x, ```int``` y, ```char``` character)**

    Draws a single character to the internal display buffer. Throws IndexOutOfRangeException if the character is drawn outside the screen bounds.


* **```void``` DrawSprite(```Sprite``` sprite)**

    Draws a single ```Sprite``` object to the screen. This does not add the sprite to the ```Display.Sprites``` list.

* **```void``` Show()**

    Shows whaterver is currently in the internal display buffer.

* **```void``` Refresh()**

    Begins the refresh cycle. The internal display buffer is cleared once every cycle and all sprites in the Sprites list are redrawn. At the momement, if called outside of ```Mainloop.mainloop```, it will loop forever until the program is stopped with Ctr+C. 

---
## **--- Input.cs ---**

### **``abstract`` ``class`` SpriteAction**
Allows for the binding of keyborad input to an action to be preformed by a ```Sprite``` object. 


#### **Constructors**

**SpriteAction(```Pawn``` sprite)**

* **sprite**: The sprite to munipulate.

#### **Methods**

* **```abstract``` ```void``` ExecuteAction()**

    Overide this method with the actions to be preformed by the sprite when a key is pressed. 

---

### **```class``` Input**

Listens for user input from the keyboard and executes actions defined by a ```SpriteAction``` class until the ```exitKey``` is pressed.

#### **Consturctors**
* **Input(```ConsoleKey``` exitKey)**

    **exitKey**: The key that will trigger an exit from the game.

#### **Methods**
* **```void``` BindAction(```ConsoleKey``` key, ```SpriteAction``` action)**
  
    Binds a ```SpriteAction``` class to the specfied key.

* **```void``` UnbindAction(ConsoleKey key)**
* 
    Unbinds a ```SpriteAction``` class from the specfied key.

* **```void``` Listen()**
* 
    Listens for keboard input and executes bound actions until ```exitKey``` is pressed.

---

## **--- Mainloop.cs ---**

### **```static``` ```class``` Mainloop**

#### **Methods**
* **```static``` ```void``` mainloop(```Display``` displayObj, ```Input``` inputObj, ```SpriteController[]?``` spriteContollers = ```null```)**

    Initializes the main game loop. The game loop will end when the key defined by ```Input.exitKey``` is pressed.

---
## **--- Spawner.cs ---**

### **```class``` Spawner**

Responsible for spawning in sprites.

#### **Consturctors**

* **Spawner(```Type``` spriteCls, ```object[]``` spriteAttr, ```Type``` spriteControllerCls, ```object?[]``` controllerAttr, ```Display``` display)**
  
    **spriteCls**: The type of the ```Sprite``` class you want to spawn in (usually a ```Pawn```) using ```System.Reflection.typeof()```.

    **spriteAttr**: An ```object``` array containing the exact number and order of arguments to pass to the spriteCls constructor (including defualt parameters).

    **spriteControllerCls**: The type of the ```spriteContoller``` class you want to control your spawned sprite using ```System.Reflection.typeof()```.

    **controllerAttr**: An ```object``` array containing the exact number and order of arguments to pass to the spriteControllerCls constructor (including defualt parameters). Pass ```null``` in place of the ```sprite``` argument since no sprite object exists yet.

    **display**: A refrence to a ```Display``` object.

#### **Methods**
* **```void``` SpwawnSprite(```int``` startx, ```int``` starty)**

    Spawns the sprite at the coordiantes indicated by startx and starty and initaties it's behavior.

---

## **--- Sprite.cs ---**

### **```struct``` Pixel**
Represents a single character and it's coordiantes within a ```Sprite``` object.

#### **Properties**
* ```int``` **X**: Gets and sets the x coordiante of the pixel.
* ```int``` **Y**: Gets and sets the x coordiante of the pixel.
* ```char``` **Character**: Gets and sets the character dislayed by the pixel.

#### **Consturctors**

* **Pixel(```int``` x, ```int``` y, ```char``` character**

    **x**: The x-coordinate of the pixel.

    **y**: The y-coordinate of the pixel.

    **character**: The character to display.

### **```struct``` CollisonsInfo**
A struct containing information about a collision between two sprites.

#### **Properties**

* **```bool``` CollisionOccured**: A flag indicating if a collison occured. Set to ```true``` if a collison occured, ```false``` if not.

* **```Sprite``` Entity**: Contains a refrence to the entity that triggred the collision, if a collison occured. Set to ```null``` if no collison occured.

### **```class``` Sprite**
Represents a basic game object. Sprites consist of a list of ```Pixel``` structs that determine the shape and location of the sprite. 
#### **Properties**

* **```List<Pixel>``` Coords**: Gets and sets the list of coordiantes that define the sprite.

*  **```string``` SpriteId**: Gets a string identifying the sprite. This property is read-only.
  
*  **```bool``` HasCollisions**: A flag that determines if the sprite has collisons or not. Set to ```true``` for collisons, ```false``` for no collisions.

*  **```int``` StackOrder**: Determines the stack order of the sprite. Sprites with lower values will appear under sprites with higher values.

#### **Constructors**

* **Sprite(```string``` file, ```int``` startx, ```int``` starty, ```string``` spriteId, ```int``` stackOrder=0, ```bool``` collisions=```true```)**

    **file**: A text file contatining ascii art to convert into a sprite.

    **startx**: The intial starting x-postion of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.

    **starty**: The intial starting y-postion of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.

    **spriteid**: A unique identifier for the sprite

    **collisions**: Pass ```true``` for collisons, ```false``` for no collisions.

#### **Methods**

* **```static``` ```int``` CompareByStackOrder(```Sprite``` sprite1, ```Sprite``` sprite2)**
  
    Returns an interger representing the comparison between ```sprite1.SpriteID``` and ```sprite2.SpriteId```.

* **```void``` MoveTo(```int``` x, ```int``` y)**
  
    Moves the sprite to the position indicated by the x and y arguments. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.

### **```class``` Pawn: Sprite**

#### **Properties**

* **```List<Pixel>``` Coords**: Gets and sets the list of coordiantes that define the sprite.
  
*  **```string``` SpriteId**: Gets a string identifying the sprite. This property is read-only.

*  **```bool``` HasCollisions**: A flag that determines if the sprite has collisons or not. Set to ```true``` for collisons, ```false``` for no collisions.
  
*  **```int``` StackOrder**: Determines the stack order of the sprite. Sprites with lower values will appear under sprites with higher values.

#### **Constructors**

* **Sprite(```string``` file, ```int``` startx, ```int``` starty, ```string``` spriteId, ```Display``` display, ```int``` stackOrder=0,  ```bool``` collisions=```true```)**

    **file**: A text file contatining ascii art to convert into a sprite.

    **startx**: The intial starting x-postion of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.

    **starty**: The intial starting y-postion of the sprite. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.

    **display**: A refrence to a ```Display``` object.

    **spriteid**: A unique identifier for the sprite.

    **collisions**: Pass ```true``` for collisons, ```false``` for no collisions.

#### **Methods**

* **```static``` ```int``` CompareByStackOrder(```Sprite``` sprite1, ```Sprite``` sprite2)**
  
    Returns an interger representing the comparison between ```sprite1.SpriteID``` and ```sprite2.SpriteId```.

* **```void``` MoveTo(```int``` x, ```int``` y)**
  
    Moves the sprite to the position indicated by the x and y arguments. NOTE: The sprite will not be center aligned, rather the *top-left* pixel of the sprite will snap to this position.

* **```CollisionInfo``` MoveNorth(```int``` distnace)**

    Moves the sprite up on the screen and then prefroms a collisions check. The result of the check is then returned.

* **```CollisionInfo``` MoveSouth(```int``` distnace)**

    Moves the sprite down on the screen and then prefroms a collisions check. The result of the check is then returned.

* **```CollisionInfo``` MoveWest(```int``` distnace)**

    Moves the sprite right on the screen and then prefroms a collisions check. The result of the check is then returned.

* **```CollisionInfo``` MoveEast(```int``` distnace)**

    Moves the sprite up left the screen and then prefroms a collisions check. The result of the check is then returned.

---

## **--- SpriteContorller.cs ---**

### **```abstract``` ```class``` SpriteController**

#### **Constructors**

* **SpriteController(```Pawn?``` sprite, ```Display``` display, ```int``` speed)**

    **sprite**: A ```Pawn``` object to be controlled.

    **display**: A ```Display``` object.

    **speed**: An integer representing how many miliseconds to delay the behavior cycle. Controls the speed at which the controlled sprite operates.

#### **Methods**
* **```abstract``` ```void``` Behavior()**

    This method determines the actions a sprite will take during it's behavior cycle.

* **```virtual``` ```bool``` CheckDespawnConditions()**
    This method tells the sprite controller when to despawn the sprite during it's behavior cycle. By defualt it simply returns ```false```.

* **```void``` Initialize()**
    Initializes the behavior cycle. The speed of this cycle is determined by the ```speed``` attribute. The cycle first calls ```Behavior()```, then it calls ```CheckDespawnConditions()```. If ```CheckDespawnConditions()``` returns ```true```, the sprite will be despawned and the cycle will end.

    