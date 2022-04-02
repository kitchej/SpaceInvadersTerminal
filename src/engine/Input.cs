namespace SimpleGameEngine{

    abstract class SpriteAction{
        protected Pawn _sprite;
        public SpriteAction(Pawn sprite){
            _sprite = sprite;
        }

        abstract public void ExecuteAction();

    }

    class Input{

        ConsoleKey _exitKey;
        ConsoleKey _readKey;
        SpriteAction? _actionToExecute;
        Dictionary<ConsoleKey, SpriteAction> bindings;

        public Input(ConsoleKey exitKey){
            _exitKey = exitKey;
            _actionToExecute = null;
            bindings = new Dictionary<ConsoleKey, SpriteAction>();
        }

        public void BindAction(ConsoleKey key, SpriteAction action){
            bindings.Add(key, action);
        }

        public void UnbindAction(ConsoleKey key){
            bindings.Remove(key);
        }

        public void Listen(){
            while (true){
                _readKey = Console.ReadKey(intercept: true).Key;
                if (_readKey == _exitKey){
                    Console.CursorVisible = true;
                    break;
                }
                try{
                    bindings.TryGetValue(_readKey, out _actionToExecute);
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