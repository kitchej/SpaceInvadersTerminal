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

        ConsoleKey _exitKey;
        ConsoleKey _readKey;
        GameAction? _actionToExecute;
        Dictionary<ConsoleKey, GameAction> _bindings;
        bool killGame;

        public Input(ConsoleKey exitKey){
            _exitKey = exitKey;
            _actionToExecute = null;
            _bindings = new Dictionary<ConsoleKey, GameAction>();
            killGame = false;
        }

        public void KillGame(){
            killGame = true;
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
                if (killGame){
                    Console.CursorVisible = true;
                    break;
                }
                _readKey = Console.ReadKey(intercept: true).Key;
                if (_readKey == _exitKey){
                    Console.CursorVisible = true;
                    break;
                }
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