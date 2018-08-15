using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace furdostat
{
    public class Guest
    {
        public int ID;
        public TimeSpan Enter;
        public TimeSpan Exit;
        public TimeSpan TimeSpent;
        public Dictionary<TimeSpan, KeyValuePair<int, int>> Movs;
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Guest> Guests = new List<Guest>();

            string[] txt = File.ReadAllLines("furdoadat.txt");
            for (int i = 0; i < txt.Length; i++)
            {
                int[] d = Array.ConvertAll(txt[i].Split(' '), x => int.Parse(x));
                if (!Guests.Any(x => x.ID == d[0]))
                {
                    Guest g = new Guest();
                    g.ID = d[0];
                    if (d[1] == 0)
                    {
                        if (d[2] == 1)
                        {
                            g.Enter = new TimeSpan(d[3], d[4], d[5]);
                        }
                        else
                        {
                            g.Exit = new TimeSpan(d[3], d[4], d[5]);
                        }
                    }
                    else
                    {
                        g.Movs = new Dictionary<TimeSpan, KeyValuePair<int, int>>();
                        g.Movs.Add(new TimeSpan(d[3], d[4], d[5]), new KeyValuePair<int, int>(d[1], d[2]));
                    }
                    Guests.Add(g);
                }
                else
                {
                    Guest g = Guests.Find(x => x.ID == d[0]);
                    if (d[1] == 0)
                    {
                        if (d[2] == 1)
                        {
                            g.Enter = new TimeSpan(d[3], d[4], d[5]);
                        }
                        else
                        {
                            g.Exit = new TimeSpan(d[3], d[4], d[5]);
                        }
                    }
                    else
                    {
                        if (g.Movs == null)
                        {
                            g.Movs = new Dictionary<TimeSpan, KeyValuePair<int, int>>();
                        }
                        g.Movs.Add(new TimeSpan(d[3], d[4], d[5]), new KeyValuePair<int, int>(d[1], d[2]));
                    }
                    Guests.RemoveAll(x => x.ID == d[0]);
                    Guests.Add(g);
                }
            }

            Console.WriteLine("2. feladat: \n Az első vendég {0}-kor lépett ki az öltözőből.\n Az utolsó vendég {1}-kor lépett ki az öltözőből.",
                Guests.OrderBy(x => x.Enter).First().Enter, Guests.OrderBy(x => x.Enter).Last().Enter);

            int xx = 0;
            for (int i = 0; i < Guests.Count; i++)
            {
                if (Guests[i].Movs.Count <= 2 && Guests[i].Movs.Values.First().Key == Guests[i].Movs.Values.Last().Key)
                {
                    xx++;
                }
            }
            Console.WriteLine("3. feladat:\n A fürdőben {0} vendég járt csak egy részlegen.", xx);

            for (int i = 0; i < Guests.Count; i++)
            {
                Guests[i].TimeSpent = Guests[i].Exit - Guests[i].Enter;
            }
            Console.WriteLine("4. feladat:\n A legtöbb időt eltöltő vendég: {0}. vendég {1}", Guests.OrderByDescending(x => x.TimeSpent).First().ID, Guests.OrderByDescending(x => x.TimeSpent).First().TimeSpent);

            Console.WriteLine("5. feladat:\n 6-9 óra között {0} vendég\n 9-16 óra között {1} vendég\n 16-20 óra között {2} vendég",
                Guests.Count(x => x.Enter > new TimeSpan(6, 0, 0) && x.Enter < new TimeSpan(8, 59, 59)),
                Guests.Count(x => x.Enter > new TimeSpan(9, 0, 0) && x.Enter < new TimeSpan(15, 59, 59)),
                Guests.Count(x => x.Enter > new TimeSpan(16, 0, 0) && x.Enter < new TimeSpan(19, 59, 59)));

            Dictionary<int, TimeSpan> szauna = new Dictionary<int, TimeSpan>();
            for (int i = 0; i < Guests.Count; i++)
            {
                TimeSpan enter = new TimeSpan();
                TimeSpan exit = new TimeSpan();
                foreach (var m in Guests[i].Movs)
                {
                    if (m.Value.Key == 2)
                    {
                        if (m.Value.Value == 0)
                        {
                            enter = m.Key;
                        }
                        else
                        {
                            exit = m.Key;
                            if (szauna.ContainsKey(Guests[i].ID))
                            {
                                szauna[Guests[i].ID] += (exit - enter);
                            }
                            else
                            {
                                szauna.Add(Guests[i].ID, exit - enter);
                            }
                        }
                    }
                }
            }

            FileStream fs = new FileStream("szauna.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            foreach (var sz in szauna)
            {
                sw.WriteLine("{0} {1}", sz.Key, sz.Value.ToString());
            }
            sw.Close();
            fs.Close();

            List<int> used = new List<int>() { 0, 0, 0, 0 };
            for (int i = 0; i < Guests.Count; i++)
            {
                List<int> beenhere = new List<int>();
                foreach (var m in Guests[i].Movs)
                {
                    if (m.Value.Value == 0)
                    {
                        if (!beenhere.Contains(m.Value.Key))
                        {
                            beenhere.Add(m.Value.Key);
                            used[m.Value.Key - 1]++;
                        }
                    }
                }
            }
            Console.WriteLine("7. feladat:\n Uszoda: {0}\n Szaunák: {1}\n Gyógyvizes medencék: {2}\n Stand: {3}", used[0], used[1], used[2], used[3]);

            Console.ReadKey();
        }
    }
}
