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

        public Input(){
            _actionToExecute = null;
            _bindings = new Dictionary<ConsoleKey, GameAction>();
        }

        public void BindAction(ConsoleKey key, GameAction action){
            _bindings.Add(key, action);
        }

        public void BindAction(KeyValuePair<ConsoleKey, GameAction>? action){
            if (action.HasValue){
                 _bindings.Add(action.Value.Key, action.Value.Value);
            }
           
        }

        public KeyValuePair<ConsoleKey, GameAction>? UnbindAction(ConsoleKey key){
            GameAction? oldAction; 
            bool result = _bindings.Remove(key, out oldAction);
            if (oldAction == null){
                return null;
            }
            return new KeyValuePair<ConsoleKey, GameAction>(key, oldAction);
        }

        public void Listen(){
            while (true){
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