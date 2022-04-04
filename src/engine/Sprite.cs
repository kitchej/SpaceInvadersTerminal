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
        public bool CollisionOccured;
        public Sprite? Entity;
    }

    class Sprite{

        protected List<Pixel> _coords;
        protected string _spriteId;
        protected bool _hasCollisons;
        protected int _stackOrder;

        public List<Pixel> Coords {get {return _coords;}}
        public string SpriteId {get{return _spriteId;}}
        public bool HasCollisons {get {return _hasCollisons;} set{_hasCollisons = value;}} // This may need to be locked if edited from other threads
        public int StackOrder {get {return _stackOrder;} set{_stackOrder = value;}} // This may need to be locked if edited from other threads

        public Sprite(string file, int startx, int starty, string spriteId, int stackOrder=0, bool collisions=true){
            int x;
            _spriteId = spriteId;
            _stackOrder = stackOrder;
            _coords = new List<Pixel>();
            _hasCollisons = collisions;
            int whiteSpace;
            string row;
            foreach(var line in File.ReadLines(file)){
                x = startx;
                // Removing the surrounding whitespace from a line causes the characters to be out of position
                // To compensate for this, we count the whitespace before the first character, then add that to the x pos of the first chararacter
                whiteSpace= CountWhiteSpace(line);
                row = line.Trim();
                x += whiteSpace;
                foreach(var c in row){
                    _coords.Add(new Pixel(x, starty, c));
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

        public void MoveTo(int x, int y){
            int xTransform;
            int yTransform;

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

            for (int i=0; i<_coords.Count; i++){
                Pixel c = _coords[i];
                c.X += xTransform;
                c.Y += yTransform;
                _coords[i] = c;
            }
        }
    }

    class Pawn: Sprite{
        Display _display;
        CollisionInfo _collisionsInfo;
        public Pawn(string file, int startx, int starty, string spriteId, Display display, int stackOrder=0, bool collisions=true): 
        base(file, startx, starty, spriteId, stackOrder){
            _display = display;
        }

        CollisionInfo CheckCollisions(){
            CollisionInfo output = new CollisionInfo();
            lock (_display.Sprites){
                foreach(var sprite in _display.Sprites){
                if (sprite == this){
                    continue;
                }
                if (!sprite.HasCollisons){
                    continue;
                }
                lock(sprite.Coords){
                    foreach (var otherCoord in sprite.Coords){
                        foreach(var coord in _coords){
                            
                            if (otherCoord.X == coord.X && otherCoord.Y == coord.Y){
                                output.CollisionOccured = true;
                                output.Entity = sprite;
                                return output;
                            }
                        }
                    }
                }
                
            }
            
            }
            output.CollisionOccured = false;
            output.Entity = null;
            return output;
        }

        public CollisionInfo MoveNorth(int distnace){
            lock(_coords){
                for (int i=0; i<_coords.Count; i++){
                Pixel c = _coords[i];
                c.Y -= distnace;
                _coords[i] = c;
                }
            }
            
            _collisionsInfo = CheckCollisions();
            if (_collisionsInfo.CollisionOccured){
                lock(_coords){
                    for (int i=0; i<_coords.Count; i++){
                        Pixel c = _coords[i];
                        c.Y += distnace;
                        _coords[i] = c;
                    }
                }
                
            }
            return _collisionsInfo;
        }
        public CollisionInfo MoveSouth(int distnace){
            lock(_coords){
                for (int i=0; i<_coords.Count; i++){
                    Pixel c = _coords[i];
                    c.Y += distnace;
                    _coords[i] = c;
                }
            }
            
             _collisionsInfo = CheckCollisions();
            if (_collisionsInfo.CollisionOccured){
                lock(_coords){
                    for (int i=0; i<_coords.Count; i++){
                        Pixel c = _coords[i];
                        c.Y -= distnace;
                        _coords[i] = c;
                    }
                }
            }
            return _collisionsInfo;
        }
        public CollisionInfo MoveWest(int distnace){
            lock(_coords){
                for (int i=0; i<_coords.Count; i++){
                    Pixel c = _coords[i];
                    c.X -= distnace;
                    _coords[i] = c;
                }
            }
             _collisionsInfo = CheckCollisions();
            if (_collisionsInfo.CollisionOccured){
                lock (_coords){
                    for (int i=0; i<_coords.Count; i++){
                    Pixel c = _coords[i];
                    c.X += distnace;
                    _coords[i] = c;
                    }
                }
            }
            return _collisionsInfo;
        }
        public CollisionInfo MoveEast(int distnace){
            lock (_coords){
                for (int i=0; i<_coords.Count; i++){
                    Pixel c = _coords[i];
                    c.X += distnace;
                    _coords[i] = c;
                }
            }
             _collisionsInfo = CheckCollisions();
            if (_collisionsInfo.CollisionOccured){
               lock(_coords){
                   for (int i=0; i<_coords.Count; i++){
                        Pixel c = _coords[i];
                        c.X -= distnace;
                        _coords[i] = c;
                    }
               }
            }
            return _collisionsInfo;
        }
    }
}