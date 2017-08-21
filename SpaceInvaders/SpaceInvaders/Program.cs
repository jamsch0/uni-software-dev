using System;

namespace SpaceInvaders {

	static class Program {

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args) {
            SpaceInvaders game;
            
            // Allow program to take optional window width/height commandline arguments
            try {
                game = new SpaceInvaders(int.Parse(args[0]), int.Parse(args[1]));
            } catch (Exception ex) {
                game = new SpaceInvaders();
            }

            game.Run();
		}
	}
}
