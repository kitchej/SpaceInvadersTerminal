using SimpleGameEngine;

namespace spaceInvaders{
    
    static class Program{

        static void Main(){
            Display screen = new Display(40, 60);
            Input input = new Input(exitKey: ConsoleKey.Q);

            // Intro Screen

            screen.DrawSprite(new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "introGraphic.txt"}), 5, 5, "Header"));

            screen.Show();

            Console.ReadKey();

            Init.InitGame(screen, input);
            
            
        }
    }
}
