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

            // Game Init

            Sprite northBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "horizBorder.txt"}), 1, 0, "northBounds");
            Sprite southBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "horizBorder.txt"}), 1, 39, "southBounds");
            Sprite westBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "vertiBorder.txt"}), 0, 1, "westBounds");
            Sprite eastBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "vertiBorder.txt"}), 59, 1, "eastBounds");

            MasterEnemyController masterEnemyController = new MasterEnemyController(screen, 500, new Spawner(typeof(Pawn), 
                                                                                                            new object[] { Path.Join(new[] {"src", "spaceInvaders", "sprites", "projectile.txt"}), 30, 21, "enemyProjectile", screen, 0, true}, 
                                                                                                            typeof(EnemyProjectileController), 
                                                                                                            new object?[] {null, input, screen, 40}, 
                                                                                                            screen));
            
            Pawn ship = new Pawn(Path.Join(new[] {"src", "spaceInvaders", "sprites", "ship.txt"}), startx: 30, starty: 36, spriteId: "ship", display: screen, stackOrder: 0);
            Spawner shipProjectileSpawner = new Spawner(typeof(Pawn), 
                                                        new object[] { Path.Join(new[] {"src", "spaceInvaders", "sprites", "projectile.txt"}), 30, 21, "projectile", screen, 0, true}, 
                                                        typeof(ProjectileController), 
                                                        new object?[] {null, screen, 40}, 
                                                        screen);

            screen.AddSprite(ship);
            screen.AddSprite(northBounds);
            screen.AddSprite(southBounds);
            screen.AddSprite(westBounds);
            screen.AddSprite(eastBounds);

            input.BindAction(ConsoleKey.A, new MoveWest(ship));
            input.BindAction(ConsoleKey.D, new MoveEast(ship));
            input.BindAction(ConsoleKey.W, new MoveNorth(ship));
            input.BindAction(ConsoleKey.S, new MoveSouth(ship));
            input.BindAction(ConsoleKey.Spacebar, new ShootProjectile(ship, shipProjectileSpawner, 1));
            
            Mainloop.mainloop(screen, input, new Controller[] {masterEnemyController});

            
            
            
        }
    }
}
