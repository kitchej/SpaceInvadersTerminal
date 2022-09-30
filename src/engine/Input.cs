namespace SimpleGameEngine{

    abstract class GameAction{
        abstract public void ExecuteAction();
    }


    abstract class SpriteAction: GameAction{
        public Pawn Sprite {get; set;}
        public SpriteAction(Pawn sprite){
            Sprite = sprite;
        }

    }


    class Input{
        ConsoleKey _readKey;
        GameAction? _actionToExecute;
        Dictionary<ConsoleKey, GameAction> _bindings;
        public bool Kill{get; set;}

        public Input(){
            _actionToExecute = null;
            _bindings = new Dictionary<ConsoleKey, GameAction>();
            Kill = false;
        }

        public void BindAction(ConsoleKey key, GameAction action){
            _bindings.Add(key, action);
        }

        public void BindAction(ConsoleKey key, SpriteAction action){
            _bindings.Add(key, action);
        }

        public void UnbindAction(ConsoleKey key){
            _bindings.Remove(key);
        }

        public void Listen(){
            while (true){
                if (Kill){
                    Console.CursorVisible = true;
                    break;
                }
                _readKey = Console.ReadKey(intercept: true).Key;
                try{
                    _bindings.TryGetValue(_readKey, out _actionToExecute);
                    if (_actionToExecute == null){
                        continue;
                    }
                    else{
                        _actionToExecute.ExecuteAction();
                    }
                }
                catch(ArgumentNullException){
                    continue;
                }
                
            }
        }

    }
}