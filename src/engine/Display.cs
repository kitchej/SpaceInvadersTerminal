namespace SimpleGameEngine{
    class Display{
        int _rowCount;
        int _columnCount;
        char[,] screen;
        char _background;
        List<Sprite> _sprites;
        bool ExitFlag {get; set;}

        public Display(int rowCount, int columnCount, char background=' '){
            _rowCount = rowCount;
            _columnCount = columnCount;
            _background = background;
            screen = new char[_rowCount, _columnCount];
            _sprites = new List<Sprite>();
            for (int i=0;i<_rowCount;i++){
                for (int j=0;j<_columnCount;j++){
                    screen[i,j] = _background;
                }
            }
        }

        void RefreshSprites(){
            foreach(var sprite in _sprites){
                DrawSprite(sprite);
            }
        }

        public void Draw(int x, int y, char character){
            if (y < 0 || y >= _rowCount){
                throw new IndexOutOfRangeException("y value is out of bounds");
            }
            if (x < 0 || x >= _columnCount){
                throw new IndexOutOfRangeException("x value is out of bounds");
            }
            screen[y, x] = character;
        }

        public void DrawSprite(Sprite sprite){
            foreach(var coord in sprite.Coords){
                Draw(coord.X, coord.Y, coord.Character);
            }
        }
        
        public void ClearDisplay(){
            for (int i=0;i<_rowCount;i++){
                for (int j=0;j<_columnCount;j++){
                    screen[i,j] = _background;
                }
            }
        }
        
        public void AddSprite(Sprite sprite){
            _sprites.Add(sprite);
        }

        public void DeleteSprite(Sprite sprite){
            _sprites.Remove(sprite);
        }

        public void Show(){
            Console.Clear();
            for (int i=0;i<_rowCount;i++){
                for (int j=0;j<_columnCount;j++){
                    Console.Write(screen[i,j]);
                }
                Console.Write('\n');
            }
        }

        public void Refresh(){
            Console.Clear();
            Console.CursorVisible = false;
            while (true){
                ClearDisplay();
                RefreshSprites();
                Console.SetCursorPosition(0, 0);
                for (int i=0;i<_rowCount;i++){
                    for (int j=0;j<_columnCount;j++){
                        Console.Write(screen[i,j]);
                    }
                    Console.Write('\n');
                }
            }
        }

    }
}
