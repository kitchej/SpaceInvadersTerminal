namespace SimpleGameEngine{

    struct Pixel{
        public int X;
        public int Y;
        public char Character;

        public Pixel(int x, int y, char character){
            X = x;
            Y = y;
            Character = character;
        }
    }

    struct CollisionInfo{
        public bool CollisionOccurred;
        public Sprite? Entity;

        public CollisionInfo(bool collisionOccurred, Sprite? entity){
            CollisionOccurred = collisionOccurred;
            Entity = entity;
        }
    }

    class Sprite{

        public CollisionInfo CollisionInfo {get; set;}
        public List<Pixel> Coords {get;}
        public string SpriteId {get;}
        public bool HasCollisions {get; set;} // This will need to be locked if edited from other threads
        public int StackOrder {get; set;} // This will need to be locked if edited from other threads
        public CollisionInfo LastCollided {get; set;}
        public bool IsDespawned {get;set;}

        public Sprite(string file, int startx, int starty, string spriteId, int stackOrder=0, bool collisions=true){
            
            SpriteId = spriteId;
            StackOrder = stackOrder;
            Coords = new List<Pixel>();
            HasCollisions = collisions;
            CollisionInfo = new CollisionInfo(false, null);
            LastCollided = new CollisionInfo(false, null);
            IsDespawned = false;
            int x;
            int whiteSpace;
            string row;
            foreach(var line in File.ReadLines(file)){
                x = startx;
                // Removing the surrounding whitespace from a line causes the characters to be out of position
                // To compensate for this, we count the whitespace before the first character, then add that to the x pos of the first character
                whiteSpace= CountWhiteSpace(line);
                row = line.Trim();
                x += whiteSpace;
                foreach(var c in row){
                    Coords.Add(new Pixel(x, starty, c));
                    x ++;
                }
                starty ++;
            }
        }


        int CountWhiteSpace(string input){
            int whiteSpaceCount = 0;
            for(int i = 0;i<input.Length;i++){
                if (input[i] == ' '){
                    whiteSpaceCount ++;
                }
                else{
                    break;
                }
            } 
            return whiteSpaceCount;
        }

        public static int CompareByStackOrder(Sprite sprite1, Sprite sprite2){
            return sprite1.StackOrder.CompareTo(sprite2.StackOrder);
        }

        public void MoveTo(int x, int y){
            int xTransform;
            int yTransform;
            lock(Coords){
                if (x < Coords[0].X){
                xTransform = (Coords[0].X - x) * - 1;
                }
                else if(x == Coords[0].X){
                    xTransform = 0;
                }
                else{
                    xTransform = x - Coords[0].X;
                }

                if (y < Coords[0].X){
                    yTransform = (Coords[0].Y - y) * - 1;
                }
                else if(y == Coords[0].Y){
                    yTransform = 0;
                }
                else{
                    yTransform = y - Coords[0].Y;
                }

                for (int i=0; i<Coords.Count; i++){
                    Pixel c = Coords[i];
                    c.X += xTransform;
                    c.Y += yTransform;
                    Coords[i] = c;
                }
            }      
        }
    }

    class Pawn: Sprite{
        CentralController _controller;

        public Pawn(string file, int startx, int starty, string spriteId, CentralController centralController, int stackOrder=0, bool collisions=true): 
        base(file, startx, starty, spriteId, stackOrder, collisions){
            _controller = centralController;
        }

        CollisionInfo CheckCollisions(){
            CollisionInfo output = new CollisionInfo();
            if (!HasCollisions){
                output.CollisionOccurred = false;
                output.Entity = null;
                return output;
            }
            lock (_controller.Sprites){
                foreach(var sprite in _controller.Sprites){
                    if (sprite == this){
                        continue;
                    }
                    if (!sprite.HasCollisions){
                        continue;
                    }
                    lock(sprite.Coords){
                        foreach (var otherCoord in sprite.Coords){
                            foreach(var coord in Coords){
                                if (otherCoord.X == coord.X && otherCoord.Y == coord.Y){
                                    output.CollisionOccurred = true;
                                    output.Entity = sprite;
                                    return output;
                                }
                            }
                        }
                    }
                }
            }
            output.CollisionOccurred = false;
            output.Entity = null;
            return output;
        }

        public CollisionInfo MoveNorth(int distance){
            lock(Coords){
                for (int i=0; i<Coords.Count; i++){
                    Pixel c = Coords[i];
                    c.Y -= distance;
                    Coords[i] = c;
                }
            }
            
            CollisionInfo = CheckCollisions();
            if (CollisionInfo.CollisionOccurred){
                lock(Coords){
                    for (int i=0; i<Coords.Count; i++){
                        Pixel c = Coords[i];
                        c.Y += distance;
                        Coords[i] = c;
                    }
                }
                CollisionInfo.Entity.LastCollided = new CollisionInfo(true, this);
                
            }
            return CollisionInfo;
        }
        public CollisionInfo MoveSouth(int distance){
            lock(Coords){
                for (int i=0; i<Coords.Count; i++){
                    Pixel c = Coords[i];
                    c.Y += distance;
                    Coords[i] = c;
                }
            }
            
             CollisionInfo = CheckCollisions();
            if (CollisionInfo.CollisionOccurred){
                lock(Coords){
                    for (int i=0; i<Coords.Count; i++){
                        Pixel c = Coords[i];
                        c.Y -= distance;
                        Coords[i] = c;
                    }
                }
                CollisionInfo.Entity.LastCollided = new CollisionInfo(true, this);
            }
            return CollisionInfo;
        }
        public CollisionInfo MoveWest(int distance){
            lock(Coords){
                for (int i=0; i<Coords.Count; i++){
                    Pixel c = Coords[i];
                    c.X -= distance;
                    Coords[i] = c;
                }
            }
             CollisionInfo = CheckCollisions();
            if (CollisionInfo.CollisionOccurred){
                lock (Coords){
                    for (int i=0; i<Coords.Count; i++){
                    Pixel c = Coords[i];
                    c.X += distance;
                    Coords[i] = c;
                    }
                }
                CollisionInfo.Entity.LastCollided = new CollisionInfo(true, this);
            }
            return CollisionInfo;
        }
        public CollisionInfo MoveEast(int distance){
            lock (Coords){
                for (int i=0; i<Coords.Count; i++){
                    Pixel c = Coords[i];
                    c.X += distance;
                    Coords[i] = c;
                }
            }
             CollisionInfo = CheckCollisions();
            if (CollisionInfo.CollisionOccurred){
               lock(Coords){
                   for (int i=0; i<Coords.Count; i++){
                        Pixel c = Coords[i];
                        c.X -= distance;
                        Coords[i] = c;
                    }
               }
               CollisionInfo.Entity.LastCollided = new CollisionInfo(true, this);
            }
            return CollisionInfo;
        }
    }
}