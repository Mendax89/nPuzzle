using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;


namespace Puzzle.Services
{
    /// <summary>
    /// Best score representation
    /// </summary>
    public class BestScore 
    {
        private List<PlayerScore> playerScore =new List<PlayerScore>();
        private PlayerScoreTimeComparer timeComparer = new PlayerScoreTimeComparer();
        /// <summary>
        /// Add new player time
        /// </summary>
        /// <param name="name">User name</param>
        /// <param name="time">Time</param>
        /// <param name="movesCount">Moves count</param>
        public void addPlayerTime(String name, int time, int movesCount)
        {
            playerScore.Add(new PlayerScore(name,time,movesCount));
            playerScore.Sort(timeComparer);
        }


        /// <summary>
        /// Read data from DB into list of score
        /// </summary>
        private void readFromDB()
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }

        /// <summary>
        /// Write new player score to DB
        /// </summary>
        /// <param name="score">Player score</param>
        private void writeToDB(PlayerScore score)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }

        /// <summary>
        /// String representation of the best scores
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Meno                |" + "   Time|" + "   Moves|");
            sb.AppendLine("================================================");
            for (int i = 0; i < playerScore.Count; i++)
			{
                sb.AppendLine(playerScore[i].Name + "        |" + "   " + playerScore[i].Time + "|   " + playerScore[i].MovesCount + "|");
			}
            return sb.ToString();
        }


        public class PlayerScoreTimeComparer : IComparer<PlayerScore>
        {
            public int Compare(PlayerScore obj1, PlayerScore obj2)
            {
                if ((obj1.Name == obj2.Name) && (obj1.MovesCount == obj2.MovesCount) && (obj1.Time == obj2.Time))
                {
                    return 0;
                }
                else if ((obj1.Time < obj2.Time))
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
                //return obj1.Time - obj2.Time;
            }        }


        /// <summary>
        /// Player score type
        /// </summary>
        public class PlayerScore
        {
            //User name
            public string Name { get; set; }
            //Number of seconds
            public int Time { get; set; }
            //Number of movements
            public int MovesCount { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name">Player name</param>
            /// <param name="time">Time</param>
            /// <param name="movesCount">Moves count</param>
            public PlayerScore(string name, int time, int movesCount)
            {
                Name = name;
                Time = time;
                MovesCount = movesCount;
            }
        }

    }
}
