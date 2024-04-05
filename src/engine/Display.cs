namespace SimpleGameEngine{

    class Display{
        CentralController _controller;
        int _rowCount;
        int _columnCount;
        char[,] _buffer;
        char _background;

        public Display(CentralController CentralController, int rowCount, int columnCount, char background=' '){
            _controller = CentralController;
            _rowCount = rowCount;
            _columnCount = columnCount;
            _background = background;
            _buffer = new char[_rowCount, _columnCount];
            for (int i=0;i<_rowCount;i++){
                for (int j=0;j<_columnCount;j++){
                    _buffer[i,j] = _background;
                }
            }
        }

        void RefreshSprites(){
            lock(_controller.Sprites){
                foreach(var sprite in _controller.Sprites){
                    try{
                        DrawSprite(sprite);
                    }
                    catch(IndexOutOfRangeException){
                        continue;
                    }
                }
            }     
        }

        public void Draw(int x, int y, char character){
            if (y < 0 || y >= _rowCount){
                throw new IndexOutOfRangeException("y value is out of bounds");
            }
            if (x < 0 || x >= _columnCount){
                throw new IndexOutOfRangeException("x value is out of bounds");
            }
            _buffer[y, x] = character;
        }

        public void DrawSprite(Sprite sprite){
            lock(sprite.Coords){
                foreach(var coord in sprite.Coords){
                    Draw(coord.X, coord.Y, coord.Character);
                }
            }
            
        }
        
        public void ClearBuffer(){
            for (int i=0;i<_rowCount;i++){
                for (int j=0;j<_columnCount;j++){
                    _buffer[i,j] = _background;
                }
            }
        }

        public void ShowBuffer(){
            Console.Clear();
            for (int i=0;i<_rowCount;i++){
                for (int j=0;j<_columnCount;j++){
                    Console.Write(_buffer[i,j]);
                }
                Console.Write('\n');
            }
        }

        public void Refresh(){
            Console.Clear();
            Console.CursorVisible = false;
            while (true){
                ClearBuffer();
                RefreshSprites();
                Console.SetCursorPosition(0, 0);
                for (int i=0;i<_rowCount;i++){
                    for (int j=0;j<_columnCount;j++){
                        Console.Write(_buffer[i,j]);
                    }
                    Console.Write('\n');
                }
            }
        }
    }
}
