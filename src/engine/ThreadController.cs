using System.Threading;

namespace SimpleGameEngine{

    class ThreadController{
        bool _stopped;
        Thread _thread;
        Controller _controllerClass;

        Logger log;

        public ThreadController(Thread thread, Controller controllerClass){
            _stopped = false;
            _thread = thread;
            _controllerClass = controllerClass;
            log = new Logger("ThreadingDebug.log");
        }

        public void Pause(){
            log.Log($"_stopped = {_stopped}");
            if(_stopped){
                //Unpause
               _thread.Interrupt();
               _stopped = false;
               _controllerClass.Stop = false;
            }
            else{
                //Pause
                _controllerClass.Stop = true;
                _stopped = true;
            }
            
        }

        public void UnPause(){
            
        }
    }
}