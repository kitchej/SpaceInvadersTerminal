namespace SimpleGameEngine{

    internal struct ControllerInfo{
        public Controller Controller{get; set;}

        public Thread Thread{get; set;}

        public ControllerInfo(Controller controller, Thread controllerThread){
            Controller = controller;
            Thread = controllerThread;
        }    
    }

    class CentralController{
        Display? _display;
        Input? _input;
        Dictionary<string, ControllerInfo> _controllers;
        Thread? _displayThread;
        Thread? _inputThread;
        public List<Sprite> Sprites {get;}

        Logger logger;

        public CentralController(){
            _controllers = new Dictionary<string, ControllerInfo>();
            Sprites = new List<Sprite>();
            logger = new Logger("CentralController.log");
        }

        public void AddSprite(Sprite sprite){
            lock (Sprites){
                Sprites.Add(sprite); 
                Sprites.Sort(Sprite.CompareByStackOrder);
            }
        }

        public void DeleteSprite(Sprite sprite){
            lock (Sprites){
                Sprites.Remove(sprite);
                Sprites.Sort(Sprite.CompareByStackOrder);
            }
        }

        public void DeleteAllSprites(){
            lock(Sprites){
                Sprites.Clear();
            }
        }

        public bool AddController(Controller controller, Thread thread){
            lock (_controllers){
                try{
                    _controllers.Add(controller.Id, new ControllerInfo(controller, thread));
                    return true;
                }
                catch(ArgumentException){
                    return false;
                }
            }
        }

        public bool RemoveController(string controllerId){
            lock (_controllers){
                return _controllers.Remove(controllerId);
            }
        }

        public bool PauseController(string controllerId){
            lock(_controllers){
                try{
                    ControllerInfo controller = _controllers[controllerId];
                    controller.Controller.Stop = true;
                    return true;
                }
                catch(KeyNotFoundException){
                    return false;
                }
            }
        }

        public bool ResumeController(string controllerId){
            lock(_controllers){
                try{
                    ControllerInfo controller = _controllers[controllerId];
                    controller.Controller.Stop = false;
                    controller.Thread.Interrupt();
                    return true;
                }
                catch(KeyNotFoundException){
                    return false;
                }
            }
        }

        public void PauseAll(){
            lock(_controllers){
                foreach(var controller in _controllers){
                    if (controller.Value.Controller.Stop == true){
                        continue;
                    }
                    controller.Value.Controller.Stop = true;
                }
            }
        }

        public void ResumeAll(){
            lock(_controllers){
                foreach(var controller in _controllers){
                    if (controller.Value.Controller.Stop == false){
                        continue;
                    }
                    controller.Value.Controller.Stop = false;
                    controller.Value.Thread.Interrupt();
                }
                
            }
        }

        public void EndGame(){
            Environment.Exit(0);
        }

        public void Mainloop(Display displayObj, Input inputObj, Controller[]? spriteControllers = null){
            _display = displayObj;
            _input = inputObj;

            _inputThread = new Thread(inputObj.Listen);
            _displayThread = new Thread(displayObj.Refresh);
            _displayThread.IsBackground = true;

            Thread controllerThread;
            List<Thread> controllerThreads = new List<Thread>();
            if (spriteControllers != null){
                foreach(var controller in spriteControllers){
                    controllerThread = new Thread(controller.Initialize);
                    controllerThread.IsBackground = true;
                    controllerThreads.Add(controllerThread);
                    bool stat = AddController(controller, controllerThread);
                }
            }
            
            _inputThread.Start();
            _displayThread.Start();

            if (controllerThreads.Count != 0){
                foreach(var thread in controllerThreads){
                    thread.Start();
                }
            }
            
        }
    }
}