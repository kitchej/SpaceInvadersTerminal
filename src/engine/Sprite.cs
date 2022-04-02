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
    class Sprite{
        public List<Pixel> Coords {get;}
        int _stackOrder;
        public string SpriteId {get; set;}

        public Sprite(string file, int startx, int starty, int stackOrder, string spriteId){
            int x;
            SpriteId = spriteId;
            _stackOrder = stackOrder;
            Coords = new List<Pixel>();
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
    }

    class Pawn: Sprite{
        public Pawn(string file, int startx, int starty, int stackOrder, string spriteId): 
        base(file, startx, starty, stackOrder, spriteId){}

        public void MoveNorth(int distnace){
            for (int i=0; i<Coords.Count; i++){
                Pixel c = Coords[i];
                c.Y -= distnace;
                Coords[i] = c;
            }
        }
        public void MoveSouth(int distnace){
            for (int i=0; i<Coords.Count; i++){
                Pixel c = Coords[i];
                c.Y += distnace;
                Coords[i] = c;
            }
        }
        public void MoveWest(int distnace){
            for (int i=0; i<Coords.Count; i++){
                Pixel c = Coords[i];
                c.X -= distnace;
                Coords[i] = c;
            }
        }
        public void MoveEast(int distnace){
            for (int i=0; i<Coords.Count; i++){
                Pixel c = Coords[i];
                c.X += distnace;
                Coords[i] = c;
            }
        }
    }
}