using System;
using Puzzle.Core;
using System.Threading;
using Puzzle.CUI;
using Puzzle.Services;

namespace Puzzle
{
    /// <summary>
    /// Starts game
    /// </summary>
    public class PuzzleGame
    {
        private IUserInterface userInterface = null;
        public static PuzzleGame puzzleGame;
        public BestScore BestScore { get; private set; }
        public int MovesCount { get; set; }
        private long ticks;
        public int PlayingSeconds
        {
            get
            {
                return (int)((DateTime.Now.Ticks - ticks) / TimeSpan.TicksPerSecond); ;
            }
        }
                
        public PuzzleGame()
        {
            Field field = new Field(4, 4);
            userInterface = new ConsoleUI(field);
            userInterface.StartNewGame();
            puzzleGame = this;
            BestScore = new BestScore();
        }

        public static object GetInstance()
        {
            return puzzleGame;
        }

        public void StartGameInit()
        {
            MovesCount = 0;
            ticks = DateTime.Now.Ticks;
        }

        static void Main(string[] args)
        {
            
            new PuzzleGame();
        }
    }
}
