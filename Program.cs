#region Old
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections;

//namespace SplitTeams
//{
//    class Program
//    {
//        class Player
//        {
//            public string Name { get; set; }
//            public int Rating { get; set; }
//        }
//        class Match : List<int>
//        {
//            public int Sum { get; set; }
//            public Match() { }
//            public Match(Match from)
//                : base(from)
//            {
//                this.Sum = from.Sum;
//            }
//        }
//        class Pair
//        {
//            public Player First { get; set; }
//            public Player Second { get; set; }
//        }

//        private static int _TotalPlayers;
//        private static Random rnd = new Random(DateTime.Now.Millisecond);
//        private static float _Half;
//        private static Player[] _Players;
//        private static bool _Finished = false;
//        private static void loop(ref Match bestMatch, Match current, int depth, int start)
//        {
//            if (depth == (_TotalPlayers / 2) - 1)
//            {
//                current.Sum = 0;
//                for (int i = 0; i < current.Count; i++)
//                    current.Sum += _Players[current[i]].Rating;
//                if (Math.Abs(bestMatch.Sum - _Half) > Math.Abs(current.Sum - _Half))
//                {
//                    bestMatch = new Match(current);
//                    _Finished = bestMatch.Sum == _Half;
//                }
//            }
//            else
//            {
//                //using a copy so we don't modify current and it retains same values as when it was passed as parameter
//                Match copy = new Match(current);
//                for (copy[depth] = depth >= 1 ? copy[depth - 1] + 1 : 0; !_Finished && copy[depth] < _TotalPlayers; copy[depth]++)
//                {
//                    bool duplicate = false;
//                    int val = copy[depth];
//                    for (int i = 0; i < copy.Count && !(duplicate = (i != depth && copy[i] == val)); i++) ;
//                    if (!duplicate) 
//                        loop(ref bestMatch, copy, depth + 1, start + 1);
//                }
//            }
//        }

//        private static void Split2(Player[] team1, Player[] team2)
//        {   
//            List<Player> copy = _Players.ToList();
//            copy = copy.OrderByDescending(x => x.Rating).ToList();
//            int team1Counter = 0, team2Counter = 0;
//            while (copy.Count > 0)
//            {
//                int index1, index2;
//                if (copy.Count == 2 || Math.Abs(copy[0].Rating - copy[1].Rating) < Math.Abs(copy[1].Rating - copy[2].Rating))
//                {
//                    index1 = 0;
//                    index2 = 1;
//                }
//                else
//                {
//                    index1 = 1;
//                    index2 = 2;
//                }

//                team1[team1Counter++] = copy[index1];
//                team2[team2Counter++] = copy[index2];
//                copy.RemoveAt(index1);
//                copy.RemoveAt(index2 - 1);
//            }
//        }
//        private static void Split3(Player[] team1, Player[] team2)
//        {
//            for (int i = 0; i < _Players.Length/2; i++)
//                team1[i] = _Players[i];
//            for (int i = _Players.Length / 2, counter = 0; i < _Players.Length; i++, counter++)
//                team2[counter] = _Players[i];
//            int s1, s2, sum1, sum2;
//            sum1 = team1.Sum(x=>x.Rating);
//            sum2 = team2.Sum(x=>x.Rating);
//            int diff = sum1 - sum2;
//            while (diff != 0 && FindSwappables(team1, team2, diff, out s1, out s2))
//            {
//                Swap(team1, team2, s1, s2, ref sum1, ref sum2);
//                diff = sum1 - sum2;
//            }
//        }
//        private static bool FindSwappables(Player[] team1, Player[] team2, int diff, out int player1, out int player2)
//        {
//            player1 = 0;
//            player2 = 0;
//            int? maxDiff = null;
//            double halfDiff = Math.Abs(Math.Ceiling((float)diff/2));
//            for (int i = 0; i < team1.Length; i++)
//            {
//                for (int j = 0; j < team2.Length; j++)
//                {
//                    int currentDiff = team1[i].Rating - team2[j].Rating;
//                    if (currentDiff * diff > 0)//elligible swap
//                    {
//                        currentDiff = Math.Abs(currentDiff);
//                        if (currentDiff <= halfDiff && (!maxDiff.HasValue || currentDiff > maxDiff))
//                        {
//                            maxDiff = currentDiff;
//                            player1 = i;
//                            player2 = j;
//                        }
//                    }
//                }
//            }
//            return maxDiff != null;
//        }
//        private static void Swap(Player[] team1, Player[] team2, int team1Index, int team2Index, ref int sum1, ref int sum2)
//        {
//            Player temp = team1[team1Index];
//            team1[team1Index] = team2[team2Index];
//            team2[team2Index] = temp;
//            sum1 += team1[team1Index].Rating - team2[team2Index].Rating;
//            sum2 += team2[team2Index].Rating - team1[team1Index].Rating;
//        }
//        private static void Split(Player[] team1, Player[] team2)
//        {
//            Match current = new Match(), match = new Match();
//            //initialize
//            for (int i = 0; i < _TotalPlayers / 2; i++)
//            {
//                match.Add(i);
//                current.Add(i);
//            }
//            _Half = ((float)_Players.Sum(x => x.Rating)) / 2;
//            _Players = Fold(_Players);
//            loop(ref match, current, 0, 0);
//            int team1Counter = 0, team2Counter = 0;
//            for (int i = 0; i < _Players.Length; i++)
//            {
//                if (match.Contains(i))
//                    team1[team1Counter++] = _Players[i];
//                else
//                    team2[team2Counter++] = _Players[i];
//            }
//        }
//        private static Player[] Fold(Player[] players)
//        {
//            players = players.OrderBy(x => x.Rating).ToArray();
//            int foldedIndex = 0;
//            bool rightLeft = false;
//            Player[] folded = new Player[players.Length];
//            for (int i = 0; i < players.Length/2; i+=(rightLeft ? 1 : 0), rightLeft = !rightLeft)
//            {
//                folded[foldedIndex++] = rightLeft ? players[i] : players[players.Length - i - 1];
//            }
//            return folded;
//        }
//        private static Player[] GeneratePlayers()
//        {
//            Player[] result = new Player[_TotalPlayers];
//            for (int i = 0; i < result.Length; i++)
//                result[i] = new Player { Name = "Player" + i.ToString(), Rating = rnd.Next(2000) };
//            return result;
//        }
//        static void Main(string[] args)
//        {
//            //_Players = new Player[]
//            //{
//            //    new Player{Name="A", Rating=1567},
//            //    new Player{Name="B", Rating=1696},
//            //    new Player{Name="C", Rating=1697},
//            //    new Player{Name="D", Rating=1653},
//            //    new Player{Name="E", Rating=1813},
//            //    new Player{Name="F", Rating=1664},
//            //    new Player{Name="G", Rating=1664},
//            //    new Player{Name="H", Rating=1620},
//            //    new Player{Name="I", Rating=1665},
//            //    new Player{Name="J", Rating=1747}

//            //};
//            //_Players = new Player[]
//            //{   
//            //    new Player{Name="A", Rating=1},
//            //    new Player{Name="B", Rating=6},
//            //    new Player{Name="C", Rating=8},
//            //    new Player{Name="D", Rating=11},
//            //    new Player{Name="E", Rating=12},
//            //    new Player{Name="F", Rating=19}
//            //};
//            _TotalPlayers = 100;
//            if (args.Length > 0)
//                int.TryParse(args[0], out _TotalPlayers);
//            if (_TotalPlayers % 2 != 0)
//                throw new Exception("Invalid PLAYER_COUNT, must be even.");
//            DateTime start = DateTime.Now;
//            _Players = GeneratePlayers();
//            DateTime end = DateTime.Now;
//            TimeSpan diff = end.Subtract(start);
//            Console.WriteLine(string.Format("Generated {0} players in {1} milliseconds", _TotalPlayers, diff.TotalMilliseconds));
//            //SplitAndReport1(ref start, ref end, ref diff);
//            SplitAndReport2(ref start, ref end, ref diff);
//            Console.Read();
//            //Player[] x = Fold(_Players);
//            //for (int i = 0; i < _Players.Length; i++)
//            //{
//            //    Console.WriteLine(string.Format("{0}\t{1}", _Players[i].Rating, x[i].Rating));
//            //}
//            //Console.ReadKey();

//        }

//        private static void SplitAndReport1(ref DateTime start, ref DateTime end, ref TimeSpan diff)
//        {
//            Player[] team1 = new Player[_TotalPlayers / 2];
//            Player[] team2 = new Player[_TotalPlayers / 2];
//            start = DateTime.Now;
//            Split(team1, team2);
//            end = DateTime.Now;
//            diff = end.Subtract(start);
//            Console.WriteLine(string.Format("Split {0} players into 2 {1} player teams in {2} milliseconds.", _TotalPlayers, _TotalPlayers / 2, diff.TotalMilliseconds));
//            float sum1 = team1.Sum(x => x.Rating);
//            float sum2 = team2.Sum(x => x.Rating);
//            float perc1 = (sum1 * 100) / (sum1 + sum2);
//            float perc2 = (sum2 * 100) / (sum1 + sum2);
//            Console.WriteLine(string.Format("Team 1: ({0:f2}%)", perc1));
//            foreach (Player p in team1)
//                Console.WriteLine(string.Format("Name: {0}, Rating: {1}", p.Name, p.Rating));
//            Console.WriteLine(string.Format("Team 2: ({0:f2}%)", perc2));
//            foreach (Player p in team2)
//                Console.WriteLine(string.Format("Name: {0}, Rating: {1}", p.Name, p.Rating));
//        }

//        private static void SplitAndReport2(ref DateTime start, ref DateTime end, ref TimeSpan diff)
//        {
//            Player[] team1 = new Player[_TotalPlayers / 2];
//            Player[] team2 = new Player[_TotalPlayers / 2];
//            start = DateTime.Now;
//            Split3(team1, team2);
//            end = DateTime.Now;
//            diff = end.Subtract(start);
//            Console.WriteLine(string.Format("Split {0} players into 2 {1} player teams in {2} milliseconds.", _TotalPlayers, _TotalPlayers / 2, diff.TotalMilliseconds));
//            float sum1 = team1.Sum(x => x.Rating);
//            float sum2 = team2.Sum(x => x.Rating);
//            float perc1 = (sum1 * 100) / (sum1 + sum2);
//            float perc2 = (sum2 * 100) / (sum1 + sum2);
//            Console.WriteLine(string.Format("Team 1: ({0:f2}%)", perc1));
//            foreach (Player p in team1)
//                Console.WriteLine(string.Format("Name: {0}, Rating: {1}", p.Name, p.Rating));
//            Console.WriteLine(string.Format("Team 2: ({0:f2}%)", perc2));
//            foreach (Player p in team2)
//                Console.WriteLine(string.Format("Name: {0}, Rating: {1}", p.Name, p.Rating));
//        }
//    }
//}

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using PseudoRandom;

namespace SplitTeams
{
    class Program
    {
        class Player
        {
            public string Name { get; set; }
            public long Rating { get; set; }
        }

        private static int _TotalPlayers;
        private static Random rnd = new Random(DateTime.Now.Millisecond);
        private static MersenneTwister mersenneTwister = new MersenneTwister(new ulong[] { 0x123, 0x234, 0x345, 0x456 });
        private static Player[] _Players;

        private static bool FindSwappables(Player[] team1, Player[] team2, long diff, out int player1, out int player2)
        {
            player1 = 0;
            player2 = 0;
            long? minDiff = null;//trying to obtain the minimum distance between the swap effect and the current difference
            long halfDiff = Convert.ToInt64(Math.Abs(Math.Ceiling((double)diff / 2)));
            long swapEffect = 0; //effect of swapping items between the 2 teams
            //shortcut evaluation
            if (diff > 0 && team1[team1.Length - 1].Rating - team2[0].Rating <= diff)
            {
                player1 = team1.Length - 1;
                player2 = 0;
                return true;
            }
            if (diff < 0 && team2[team2.Length - 1].Rating - team1[0].Rating <= diff)
            {
                player1 = 0;
                player2 = team2.Length - 1;
                return true;
            }

            for (int i = 0; i < team1.Length && swapEffect != diff; i++)
            {
                long rating = team1[i].Rating;
                int? foundIndex = null;
                if (diff > 0) //S1 > S2
                    foundIndex = FindPlayerToSwapWith(team2, 0, team2.Length - 1, rating - (halfDiff > rating ? 0 : halfDiff), Math.Max(rating - halfDiff * 2, 0), rating);
                else
                    foundIndex = FindPlayerToSwapWith(team2, 0, team2.Length - 1, rating + halfDiff, rating, rating + halfDiff * 2);

                if (foundIndex.HasValue)
                {
                    swapEffect = team1[i].Rating - team2[foundIndex.Value].Rating;
                    swapEffect = Math.Abs(swapEffect);
                    if (swapEffect != 0 && (!minDiff.HasValue || Math.Abs(diff - swapEffect) < minDiff))
                    {
                        minDiff = diff - swapEffect;
                        player1 = i;
                        player2 = foundIndex.Value;
                    }
                }
            }
            return minDiff != null;
        }

        private static int FindInsertIndex(Player[] list, int start, int end, long value)
        {
            int diff = end - start;
            if (value <= list[start].Rating) return start;// V-----S-----------E
            if (value >= list[end].Rating) return end;//       S-----------E-----V
            if (diff == 1)                                //       S-----V-----E
                return end;

            if (list[start + diff / 2].Rating > value)
                end = start + diff / 2;
            else
                start = start + diff / 2;
            return FindInsertIndex(list, start, end, value);
        }
        /*private static int? FindPlayerToSwapWith(Player[] list, int start, int end, long value, long? lowerBound, long? upperBound)
        {
            int diff = end - start;
            bool acceptHigher = lowerBound.HasValue;
            bool acceptLower = upperBound.HasValue;
            if (value == list[start].Rating || (acceptLower && upperBound > list[end].Rating)) return start;
            if (value == list[end].Rating || (acceptHigher && lowerBound < list[start].Rating)) return end;
            if (diff == 1)
            {
                if (acceptLower) return start;
                if (acceptHigher) return end;
            }
            if (end <= start || value < list[start].Rating || value > list[end].Rating) return null;
            if (list[start + diff / 2].Rating > value)
                end = start + diff / 2;
            else
                start = start + diff / 2;
            return FindPlayerToSwapWith(list, start, end, value, lowerBound, upperBound);
        }*/

        private static int? FindPlayerToSwapWith(Player[] list, int start, int end, long value, long lowerBound, long upperBound)
        {
            if (upperBound < list[start].Rating || lowerBound > list[end].Rating)
                return null;
            if (value <= list[start].Rating) return start;
            if (value >= list[end].Rating) return end;
            int diff = end - start;
            //termination condition
            if (diff == 1)//choose between the only 2 players in this range
                return MakeChoice(list, start, end, value, lowerBound, upperBound);
            if (list[start + diff / 2].Rating > value)
                end = start + diff / 2;
            else
                start = start + diff / 2;
            return FindPlayerToSwapWith(list, start, end, value, lowerBound, upperBound);
        }

        private static int? MakeChoice(Player[] list, int start, int end, long value, long lowerBound, long upperBound)
        {
            Player ps = list[start], pe = list[end];
            bool startValid = IsValid(ps, lowerBound, upperBound), endValid = IsValid(pe, lowerBound, upperBound);
            if (!startValid)
            {
                if (endValid)
                    return end;
                return null;
            }
            if (!endValid)
                return start;
            bool endIsCloser = Math.Abs(pe.Rating - value) < Math.Abs(ps.Rating - value);
            if (endIsCloser)
                return end;
            return start;
        }
        private static bool IsValid(Player p, long lowerBound, long upperBound)
        {
            return p.Rating >= lowerBound && p.Rating <= upperBound;
        }
        private static void Cut(out Player[] team1, out Player[] team2)
        {
            team1 = new Player[_TotalPlayers / 2];
            team2 = new Player[_TotalPlayers / 2];
            for (int i = 0; i < _Players.Length / 2; i++)
                team1[i] = _Players[i];
            for (int i = _Players.Length / 2, counter = 0; i < _Players.Length; i++, counter++)
                team2[counter] = _Players[i];
            team1 = team1.OrderBy(x => x.Rating).ToArray();
            team2 = team2.OrderBy(x => x.Rating).ToArray();
        }

        private static void Swap(Player[] team1, Player[] team2, int team1Index, int team2Index, ref long sum1, ref long sum2)
        {
            Player p1 = team1[team1Index];
            Player p2 = team2[team2Index];
            int insertIndex;

            insertIndex = FindInsertIndex(team2, 0, team2.Length - 1, p1.Rating);

            insertIndex = CorrectInsertIndex(team2, team2Index, p1, insertIndex);

            ShiftPlayers(team2, team2Index, insertIndex);
            team2[insertIndex] = p1;

            insertIndex = FindInsertIndex(team1, 0, team1.Length - 1, p2.Rating);
            insertIndex = CorrectInsertIndex(team1, team1Index, p2, insertIndex);

            ShiftPlayers(team1, team1Index, insertIndex);
            team1[insertIndex] = p2;

            sum1 += p2.Rating - p1.Rating;
            sum2 += p1.Rating - p2.Rating;

        }

        private static int CorrectInsertIndex(Player[] team2, int team2Index, Player p1, int insertIndex)
        {
            if(insertIndex == team2Index) return insertIndex;
            if (insertIndex < team2Index)
            {
                if (insertIndex < team2.Length - 1 && team2[insertIndex].Rating < p1.Rating)
                    insertIndex++;
            }
            else
            {
                if (insertIndex > 0 && team2[insertIndex].Rating > p1.Rating)
                    insertIndex--;
            }
            return insertIndex;
        }

        private static void ShiftPlayers(Player[] list, int removeFrom, int insertAt)
        {
            if (insertAt <= removeFrom)
                for (int i = removeFrom; i > insertAt; i--)
                    list[i] = list[i - 1];
            else
                for (int i = removeFrom; i < insertAt; i++)
                    list[i] = list[i + 1];
        }

        private static void Split(out Player[] team1, out Player[] team2)
        {
            //_Players = Fold(_Players);
            Cut(out team1, out team2);
            int s1, s2; long sum1, sum2, startDiff;
            sum1 = team1.Sum(x => x.Rating);
            sum2 = team2.Sum(x => x.Rating);

            long diff = sum1 - sum2;
            startDiff = diff;
            double percentageComplete = 0, previousPercentage = 0;
            Console.WriteLine();
            Console.Write("\r0 %");
            while (diff != 0 && FindSwappables(team1, team2, diff, out s1, out s2))
            {
                Swap(team1, team2, s1, s2, ref sum1, ref sum2);
                diff = sum1 - sum2;

                percentageComplete = (1 - (double)diff / (double)startDiff) * 100;
                if (percentageComplete - previousPercentage >= 1)
                {
                    previousPercentage = percentageComplete;
                    Console.Write(string.Format("\r{0:f2}%", percentageComplete));
                }
            }
            Console.WriteLine("\r100 %   ");
        }

        #region Util

        private static void GeneratePlayers()
        {
            mt19937.sgenrand((ulong)rnd.Next());

            DateTime start = DateTime.Now;
            _Players = new Player[_TotalPlayers];
            int shift = 0, dots = 0, onePercent = _Players.Length / 100;
            bool flip = false;
            for (int i = 0; i < _Players.Length; i++)
            {
                if (i % 7 == 0)
                    shift = rnd.Next(32, 63);
                if (i % onePercent == 0)
                {
                    dots = (dots + 1) % 10;
                    flip = dots == 0;
                    Console.Write('\r' + new string('.', flip ? dots : 10 - dots) + new string(' ', flip ? 10 - dots : dots));
                }
                _Players[i] = new Player { Name = "Player" + i.ToString(), Rating = (long)(mt19937.genrand() >> shift) };//(long)mersenneTwister.genrand_uint32() };
            }
            DateTime end = DateTime.Now;
            TimeSpan diff = end.Subtract(start);
            Console.WriteLine(string.Format("Generated {0} players in {1} milliseconds", _TotalPlayers, diff.TotalMilliseconds));
        }

        private static void SplitAndReporttoConsole()
        {
            Player[] team1, team2;
            DateTime start = DateTime.Now;
            Split(out team1, out team2);
            DateTime end = DateTime.Now;
            TimeSpan diff = end.Subtract(start);
            Console.WriteLine(string.Format("Split {0} players into 2 {1} player teams in {2} milliseconds.", _TotalPlayers, _TotalPlayers / 2, diff.TotalMilliseconds));
            float sum1 = team1.Sum(x => x.Rating);
            float sum2 = team2.Sum(x => x.Rating);
            float perc1 = (sum1 * 100) / (sum1 + sum2);
            float perc2 = (sum2 * 100) / (sum1 + sum2);
            Console.WriteLine(string.Format("Team 1: ({0:f2}%)", perc1));
            foreach (Player p in team1)
                Console.WriteLine(string.Format("Name: {0}, Rating: {1}", p.Name, p.Rating));
            Console.WriteLine(string.Format("Team 2: ({0:f2}%)", perc2));
            foreach (Player p in team2)
                Console.WriteLine(string.Format("Name: {0}, Rating: {1}", p.Name, p.Rating));
        }

        private static void SplitAndReportToFile()
        {
            FileInfo fi = new FileInfo("C:\\output.txt");
            FileStream fs = null;
            if (!fi.Exists)
                fs = new FileStream("C:\\output.txt", FileMode.Create);
            else
                fs = File.Open("C:\\output.txt", FileMode.Truncate);
            StreamWriter sw = new StreamWriter(fs);

            Player[] team1, team2;
            DateTime start = DateTime.Now;
            Split(out team1, out team2);
            DateTime end = DateTime.Now;
            TimeSpan diff = end.Subtract(start);
            Console.WriteLine(string.Format("Split {0} players into 2 {1} player teams in {2} milliseconds.", _TotalPlayers, _TotalPlayers / 2, diff.TotalMilliseconds));
            float sum1 = team1.Sum(x => x.Rating);
            float sum2 = team2.Sum(x => x.Rating);
            float perc1 = (sum1 * 100) / (sum1 + sum2);
            float perc2 = (sum2 * 100) / (sum1 + sum2);
            sw.WriteLine(string.Format("Team 1: ({0:f2}%)", perc1));
            foreach (Player p in team1)
                sw.WriteLine(string.Format("Name: {0}, Rating: {1}", p.Name, p.Rating));
            sw.WriteLine(string.Format("Team 2: ({0:f2}%)", perc2));
            foreach (Player p in team2)
                sw.WriteLine(string.Format("Name: {0}, Rating: {1}", p.Name, p.Rating));
            sw.Close();
            fs.Close();
        }

        private static void InsureTotalPlayers(string[] args)
        {
            if (args.Length > 0)
                int.TryParse(args[0], out _TotalPlayers);
            if (_TotalPlayers % 2 != 0)
                throw new Exception("Invalid PLAYER_COUNT, must be even.");
        }

        #endregion

        static void Main(string[] args)
        {
            //    _Players = new Player[]
            //    {
            //    new Player{Name="A", Rating=500},
            //    new Player{Name="A", Rating=1},
            //    new Player{Name="A", Rating=233},
            //    new Player{Name="A", Rating=10},
            //    new Player{Name="A", Rating=71},
            //    new Player{Name="A", Rating=112},
            //    new Player{Name="A", Rating=9},
            //    new Player{Name="A", Rating=46}
            //};
            _Players = new Player[]
            {
                new Player{Name="A", Rating=0  },
                new Player{Name="B", Rating=0  },
                new Player{Name="C", Rating=0  },
                new Player{Name="D", Rating=0  },
                new Player{Name="F", Rating=7  },
                new Player{Name="G", Rating=2  },
                new Player{Name="H", Rating=4  },
                new Player{Name="I", Rating=9  },
                new Player{Name="J", Rating=11  },
                new Player{Name="K", Rating=44  },
            };
            _TotalPlayers = 888888;

            InsureTotalPlayers(args);
            //_Players = new Player[_TotalPlayers];
            //for (int i = 0; i < _TotalPlayers; i++)
            //    _Players[i] = new Player { Name = i.ToString(), Rating = i };
            GeneratePlayers();
            SplitAndReportToFile();
            Console.Write("Press any key to exit.");
            Console.Read();
        }
    }
}
