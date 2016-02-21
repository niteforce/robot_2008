using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Robot_Dani
{
    class RobotProgram
    {
        public string Program;

        public string Ujprogram;

        public bool Egyszerusitheto;

        public int KNpos;

        public int EDpos;

        public int Energia;

        public RobotProgram(string program)
        {
            this.Program = program;
            this.Egyszerusitheto = program.Contains("ED") || program.Contains("DE") || program.Contains("KN") ||
                                   program.Contains("NK");
            this.Energia = energiaszamito(program);
            this.Ujprogram = atalakito(program);
        }

        int energiaszamito(string s)
        {
            int energia = 0;

            char[] irany = new char[s.Length];

            energia += 2; // Indulás
            energia++; // 1. egység.

            for (int i = 0; i < s.Length - 1; i++)
            {
                irany[i] = s[i];

                if (irany[i] != s[i + 1]) // Irányt vált
                {
                    energia += 2;
                    energia++;
                }
                else
                {
                    // Nem vált irányt
                    energia++;
                }
            }

            return energia;
        }

        string atalakito(string s)
        {
            int lepes = 1;
            string atalakitott = "";

            char[] irany = new char[s.Length];


            for (int i = 0; i < s.Length - 1; i++)
            {
                irany[i] = s[i];

                if (irany[i] != s[i + 1]) // Irányt vált
                {
                    atalakitott += (lepes != 1) ? lepes.ToString() + irany[i] : irany[i].ToString();
                    lepes = 1;
                }
                else
                {
                    lepes++;
                }
            }
            atalakitott += s.Last();
            return atalakitott;
        }
    }

    class Program
    {
        static List<RobotProgram> lista = new List<RobotProgram>();

        static void Main(string[] args)
        {
            Feladat1();
            Feladat2();
            Feladat3();
            Feladat4();
            Feladat5();

            Console.ReadKey();
        }

        static void Feladat1()
        {
            StreamReader reader = new StreamReader("program.txt");
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                lista.Add(new RobotProgram(reader.ReadLine()));
            }
            reader.Close();
        }

        static void Feladat2()
        {
            Console.WriteLine("2. Feladat");
            Console.WriteLine("Add meg egy utasitassor sorszamat!");

            int sorszam = int.Parse(Console.ReadLine()) - 1;

            double jelenlegiTav = 0;

            List<double> lepesDoubles = new List<double>();

            foreach (char c in lista[sorszam].Program)
            {
                if (c == 'E' || c == 'D')
                    if (c == 'E') lista[sorszam].EDpos++;
                    else
                    {
                        lista[sorszam].EDpos--;
                    }
                else
                {
                    if (c == 'K') lista[sorszam].KNpos++;
                    else
                    {
                        lista[sorszam].KNpos--;
                    }
                }

                jelenlegiTav = (double) Math.Sqrt(Math.Pow(lista[sorszam].EDpos, 2) + Math.Pow(lista[sorszam].KNpos, 2));
                lepesDoubles.Add(jelenlegiTav);
            }

            #region Kiíratás

            Console.WriteLine("A(z) " + (sorszam + 1) + ". utasitassor " +
                              ((lista[sorszam].Egyszerusitheto) ? "egyszerusitheto." : "nem egyszerusitheto."));

            Console.WriteLine("A(z)  " + (sorszam + 1) + ". utasitassor vegrehajtasa soran a robot a "
                              + (lepesDoubles.IndexOf(lepesDoubles.Max()) + 1) +
                              ". lepeskor elerte a maximalis tavolsagot: " + lepesDoubles.Max().ToString("F3") +
                              " egyseg.");

            Console.WriteLine("Hogy visszaterjen a kiindulopontba, az ED tengelyen "
                              + Math.Abs(lista[sorszam].EDpos) + " lepest, a KN tengelyen "
                              + Math.Abs(lista[sorszam].KNpos) + " lepest kell tennie.");

            #endregion
        }

        static void Feladat3()
        {
            Console.WriteLine();
            Console.WriteLine("3. Feladat");
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Energia <= 100)
                {
                    Console.WriteLine(i + ". utasitassor: " + lista[i].Energia + " energiaegyseg.");
                }
            }
        }

        static void Feladat4()
        {
            StreamWriter writer = new StreamWriter("ujprog.txt");

            foreach (RobotProgram program in lista)
            {
                writer.WriteLine(program.Ujprogram);
            }

            writer.Close();
        }

        static void Feladat5()
        {
            Console.WriteLine();
            Console.WriteLine("5. Feladat");
            Console.Write("Adj meg egy utasitast az uj formaban!: ");
            string beolvasott = Console.ReadLine();
            Console.WriteLine("A visszaalakitott utasitas:");
            Console.WriteLine(visszaalakito(beolvasott));
        }


        static string visszaalakito(string s)
        {
            StringBuilder visszaalakitott = new StringBuilder();
            StringBuilder szam = new StringBuilder();


            foreach (char c in s)

                if (char.IsLetter(c))
                {
                    if (szam.Length > 0)
                    {
                        visszaalakitott.Append(c, int.Parse(szam.ToString()));
                    }
                    else
                    {
                        visszaalakitott.Append(c);
                    }

                    szam.Length = 0;
                }

                else szam.Append(c);


            return visszaalakitott.ToString();
        }
    }
}