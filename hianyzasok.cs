using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace hianyzasok
{
    class Program
    {
        public class Tanulo {
            public string[] Nev;
            public int Igazolt;
            public int Igazolatlan;
            public int Sum;
            public Dictionary<string, string> Hianyzasok;
        }

        static void Main(string[] args)
        {
            List<Tanulo> Tanulok = new List<Tanulo>();

            string[] txt = File.ReadAllLines("naplo.txt");

            string datum="";
            for (int i = 0; i < txt.Length; i++)
            {
                string sor = txt[i];
                if (sor.StartsWith("#")) {
                    datum = sor.Split(' ')[1] + sor.Split(' ')[2];
                } else {
                    string[] Nev = new string[2] { sor.Split(' ')[0], sor.Split(' ')[1] };
                    if (Tanulok.Any(x => x.Nev[0] == Nev[0]))
                    {
                        Tanulo t = Tanulok.Find(x => x.Nev[0] == Nev[0]);
                        string[] d = sor.Split(' ');
                        t.Igazolt += d[2].Count(x => x == 'X');
                        t.Igazolatlan += d[2].Count(x => x == 'I');
                        t.Sum = t.Igazolatlan + t.Igazolt;
                        if (!d[2].All(x => x == 'O' || x == 'N'))
                        {
                            t.Hianyzasok.Add(datum, d[2]);
                        }
                        Tanulok.RemoveAll(x => x.Nev[0] == Nev[0]);
                        Tanulok.Add(t);
                    }
                    else
                    {
                        Tanulo t = new Tanulo();
                        string[] d = sor.Split(' ');
                        t.Nev = new string[2] { d[0], d[1] };
                        t.Igazolt = d[2].Count(x => x == 'X');
                        t.Igazolatlan = d[2].Count(x => x == 'I');
                        t.Sum = t.Igazolatlan + t.Igazolt;
                        if (!d[2].All(x => x == 'O' || x == 'N'))
                        {
                            t.Hianyzasok = new Dictionary<string, string>();
                            t.Hianyzasok.Add(datum, d[2]);
                        }
                        Tanulok.Add(t);
                    }
                }
            }

            Console.WriteLine("2. feladat: A naplóban {0} bejegyzés van.", txt.Count(x=>!x.StartsWith("#")));

            Console.WriteLine("3. feladat: Az igazolt hiányzások száma {0}, az igazolatlanoké {1} óra.", Tanulok.Sum(x => x.Igazolt), Tanulok.Sum(x => x.Igazolatlan));

            Console.Write("5. feladat: Adjon meg egy dátumot \"hónap, nap\": ");
            string[] xx = Console.ReadLine().Split(',');
            Console.WriteLine("Azon a napon {0} volt.", hetnapja(int.Parse(xx[0]), int.Parse(xx[1])));

            Console.Write("6. feladat: Adjon meg egy napot és egy óra sorszámát \"kedd, 3\": ");
            string[] xxx = Console.ReadLine().Split(',');
            int hsum = 0;
            for (int i = 0; i < Tanulok.Count; i++)
            {
                foreach (KeyValuePair<string, string> h in Tanulok[i].Hianyzasok)
                {
                    int ho = int.Parse(h.Key.Substring(0, 2));
                    int nap = int.Parse(h.Key.Substring(2, 2));
                    if (hetnapja(ho,nap)==xxx[0]) {
                        if (h.Value[int.Parse(xxx[1])-1] == 'X' || h.Value[int.Parse(xxx[1])-1] == 'I') {
                            hsum++;
                        }
                    }
                }
            }
            Console.WriteLine("Ekkor összesen {0} óra hiányzás történt.", hsum);

            List<Tanulo> legtobb = Tanulok.Where(z => z.Sum == Tanulok.Max(x => x.Sum)).ToList();
            Console.Write("7. feladat: A legtöbbet hiányzó tanulok: ");
            legtobb.ForEach(x => Console.Write("{0} ", x.Nev[1]));

            Console.ReadKey();

        }
        static string hetnapja(int honap, int nap) {
            string[] napnev = {"vasarnap", "hetfo", "kedd", "szerda", "csutortok", "pentek", "szombat"};
            int[] napszam = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 335 };
            int napsorszam = (napszam[honap - 1] + nap) % 7;

            return napnev[napsorszam];
        }
    }
}
