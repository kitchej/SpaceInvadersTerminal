using System.Threading;

namespace SimpleGameEngine{
    static class Mainloop{
        static bool EXIT = false;
        public static void mainloop(Display displayObj, Input inputObj){
            var inputThread = new Thread(inputObj.Listen);
            var displayThread = new Thread(displayObj.Refresh);
            displayThread.IsBackground = true;
            inputThread.Start();
            displayThread.Start();
        }
    }
}
