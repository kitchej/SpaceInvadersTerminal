using SimpleGameEngine;

namespace spaceInvaders{
    
    static class Program{

        static void Main(){
            Display screen = new Display(40, 60);
            Input input = new Input(exitKey: ConsoleKey.Q);

            Sprite northBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "horizBorder.txt"}), 1, 0, "northBounds");
            Sprite southBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "horizBorder.txt"}), 1, 39, "southBounds");
            Sprite westBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "vertiBorder.txt"}), 0, 1, "westBounds");
            Sprite eastBounds = new Sprite(Path.Join(new[] {"src", "spaceInvaders", "sprites", "vertiBorder.txt"}), 59, 1, "eastBounds");


            List<Pawn> ships = new List<Pawn>();
            List<EnemyController> controllers = new List<EnemyController>();
            
            int startx = 2;
            int starty = 1;
            for (int i=0;i<3;i++){
                for(int j=0;j<6;j++){
                    ships.Add(new Pawn(Path.Join(new[] {"src", "spaceInvaders", "sprites", "alien1.txt"}), startx: startx, starty: starty, spriteId: "enemyShip", display: screen));
                    startx += 5;
                }
                startx = 2;
                starty += 5;
            }
            
            foreach (var s in ships){
                screen.AddSprite(s);
                controllers.Add(new EnemyController(s, screen, 500, new Spawner(typeof(Pawn), new object[] { Path.Join(new[] {"src", "spaceInvaders", "sprites", "projectile.txt"}), 30, 21, "enemyProjectile", screen, 0, true}, 
                                          typeof(EnemyProjectileController), new object?[] {null, screen, 50}, 
                                          screen)));
            }


            
            Pawn ship = new Pawn(Path.Join(new[] {"src", "spaceInvaders", "sprites", "ship.txt"}), startx: 30, starty: 36, spriteId: "ship", display: screen, stackOrder: 0);
            Spawner shipProjectileSpawner = new Spawner(typeof(Pawn), new object[] { Path.Join(new[] {"src", "spaceInvaders", "sprites", "projectile.txt"}), 30, 21, "projectile", screen, 0, true}, 
                                          typeof(ProjectileController), new object?[] {null, screen, 50}, 
                                          screen);

            // Pawn enemyShip = new Pawn(@"src\spaceInvaders\sprites\alien1.txt", startx: 30, starty: 12, spriteId: "enemyShip", display: screen);
            
            // Spawner enemyProjectileSpawner = new Spawner(typeof(Pawn), new object[] { @"src\spaceInvaders\sprites\projectile.txt", 30, 21, "enemyProjectile", screen, 0, true}, 
            //                               typeof(EnemyProjectileController), new object?[] {null, screen, 50}, 
            //                               screen);
            // EnemyController enemyAi = new EnemyController(enemyShip, screen, 500, enemyProjectileSpawner);
            
            screen.AddSprite(ship);
            screen.AddSprite(northBounds);
            screen.AddSprite(southBounds);
            screen.AddSprite(westBounds);
            screen.AddSprite(eastBounds);
            // screen.AddSprite(enemyShip);
            input.BindAction(ConsoleKey.A, new MoveWest(ship));
            input.BindAction(ConsoleKey.D, new MoveEast(ship));
            input.BindAction(ConsoleKey.W, new MoveNorth(ship));
            input.BindAction(ConsoleKey.S, new MoveSouth(ship));
            input.BindAction(ConsoleKey.Spacebar, new ShootProjectile(ship, shipProjectileSpawner));
            
            Mainloop.mainloop(screen, input, controllers.ToArray());
        }
    }
}
