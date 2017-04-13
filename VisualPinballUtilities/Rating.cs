using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualPinballUtilities
{
    class Rating
    {
        public class EloRating
        {
            /// 
            /// Updates the scores in the passed matchup. 
            /// 
            /// The Matchup to update
            /// Whether User 1 was the winner (false if User 2 is the winner)
            /// The desired Diff
            /// The desired KFactor
            /// 
            public static void UpdateScores(Matchup matchup, bool user1WonMatch, int diff, int kFactor)
            {
                double est1 = 1 / Convert.ToDouble(1 + Math.Pow(10, (matchup.User2Score - matchup.User1Score)) / diff);
                double est2 = 1 / Convert.ToDouble(1 + Math.Pow(10, (matchup.User1Score - matchup.User2Score)) / diff);

                int sc1 = 0;
                int sc2 = 0;

                if (user1WonMatch)
                    sc1 = 1;
                else
                    sc2 = 1;

                matchup.User1Score = Convert.ToInt32(Math.Round(matchup.User1Score + kFactor * (sc1 - est1)));
                matchup.User2Score = Convert.ToInt32(Math.Round(matchup.User2Score + kFactor * (sc2 - est2)));
            }
            /// 
            /// Updates the scores in the match, using default Diff and KFactors (400, 100)
            /// 
            /// The Matchup to update
            /// Whether User 1 was the winner (false if User 2 is the winner)
            public static void UpdateScores(Matchup matchup, bool user1WonMatch)
            {
                UpdateScores(matchup, user1WonMatch, 400, 10);
            }

            public class Matchup
            {
                public int User1Score { get; set; }
                public int User2Score { get; set; }
            }
        }
    }
}
