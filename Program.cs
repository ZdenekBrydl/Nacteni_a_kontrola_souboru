using System;
using System.IO;

namespace cvicny_ukol_puxdesign
{
    class Program
    {
        public class Global
        {
            //casto pouzivana data:
            public static string zadany_adresar;
            public static string pwd = Directory.GetCurrentDirectory();
            //data na porovnani:
            public static string[,] soubor_pameti = new string[1000, 100];
            public static int[] pocet_souboru_adresare_z_pameti = new int[1000];
            public static string[] adresar_pameti = new string[1000];
            public static string[,] soubor_adresare = new string[1000, 100];
            public static int[] pocet_souboru_adresare_z_adresare = new int[1000];
            public static string[] adresar_adresare = new string[1000];
            public static int celkovy_pocet_adresaru_adresare = 0;//tuto promenou potrebuji kvuli rekurzivnimu nacitani dat z adresare
            //data na ulozeni do pameti:
            public static string nova_pamet = "";
            //data na vypis: seznamy novych,zmenenych a odstranenych
            public static string nove_soubory = "";
            public static string nove_adresare = "";
            public static string odstranene_soubory = "";
            public static string odstranene_adresare = "";
            public static string zmenene_soubory = "";
            //Boolean pro praci s vice pruchody:
            public static bool prvni_pruchod = true;
        }
        static void Main(string[] args)
        {
            //Zjisteni adresare se kterym se bude pracovat pri opakovani:
            bool vymazat = true;
            if (!Global.prvni_pruchod)
            {
                Console.WriteLine("\n---------------------------------------------------------------------------------------------------");
                Console.WriteLine("Pro opakovani se stejnym adresarem stisknete Enter nebo libovolnou jinou klavesu pro zadani jineho:");
                if (Console.ReadKey(true).Key == ConsoleKey.Enter) vymazat = false;
            }
            //Procisteni dat pri opakovani:
            if(!Global.prvni_pruchod)
            {
                if (vymazat)
                {
                    Global.zadany_adresar = "";
                    Global.prvni_pruchod = true;
                }
                Global.soubor_pameti = new string[1000, 100];
                Global.pocet_souboru_adresare_z_pameti = new int[1000];
                Global.adresar_pameti = new string[1000];
                Global.soubor_adresare = new string[1000, 100];
                Global.pocet_souboru_adresare_z_adresare = new int[1000];
                Global.adresar_adresare = new string[1000];
                Global.celkovy_pocet_adresaru_adresare = 0;
                Global.nova_pamet = "";
                Global.nove_soubory = "";
                Global.nove_adresare = "";
                Global.odstranene_soubory = "";
                Global.odstranene_adresare = "";
                Global.zmenene_soubory = "";
            }
            //Zjisteni adresare ze zadani:
            if(Global.prvni_pruchod)
            {
                Console.WriteLine("Zadejte adresu prohledavaneho adresare:");
                Global.zadany_adresar = Console.ReadLine();
            }
            while (!Directory.Exists(Global.zadany_adresar))
            {
                Console.WriteLine("Vami zadany adresar neexistuje, zadejte plnou adresu:");
                Global.zadany_adresar = Console.ReadLine();
            }
            //Zajisteni existence textoveho souboru plniciho funkci pameti pro zadany adresar v adresari pamet:
            if (!Directory.Exists($"{Global.pwd}\\pamet")) Directory.CreateDirectory($"{Global.pwd}\\pamet");
            if (!File.Exists($"{Global.pwd}\\pamet\\{Global.zadany_adresar.Replace(":\\", "-").Replace("\\", "_")}_pamet.txt"))
            {
                FileStream tmp = new FileStream($"{Global.pwd}\\pamet\\{Global.zadany_adresar.Replace(":\\", "-").Replace("\\", "_")}_pamet.txt", FileMode.OpenOrCreate);
                tmp.Close();
            }
            StringReader SRradky_pameti = new StringReader(File.ReadAllText($"{Global.pwd}\\pamet\\{Global.zadany_adresar.Replace(":\\", "-").Replace("\\", "_")}_pamet.txt"));
            //Prvni zapis do pameti:
            if (SRradky_pameti.Peek() == -1)
            {
                SRradky_pameti.Close();
                using (FileStream fileStream_1psani = new FileStream($"{Global.pwd}\\pamet\\{Global.zadany_adresar.Replace(":\\", "-").Replace("\\", "_")}_pamet.txt", FileMode.Open))
                {
                    using (StreamWriter pamet_1psani = new StreamWriter(fileStream_1psani))
                        Prvni_zapis_do_pameti(Global.zadany_adresar, pamet_1psani);
                }
                Console.WriteLine("Pamet byla vytvorena");
            }
            //Reseni zadani:
            else
            {
                SRradky_pameti.Close();
                //Nacteni dat na porovnani:
                Nacteni_pameti();
                Nacteni_adresare(Global.zadany_adresar);
                //Porovnani:
                Reseni_ukolu();
                //Vypis do pameti:
                File.WriteAllText($"{Global.pwd}\\pamet\\{Global.zadany_adresar.Replace(":\\", "-").Replace("\\", "_")}_pamet.txt", Global.nova_pamet);
                //Vypis novych souboru:
                Console.WriteLine("\n");
                int pocet_novych_souboru = Global.nove_soubory.Split('\n').GetLength(0)-1;
                if (pocet_novych_souboru > 0)
                {
                    string koncovka1 = "e";
                    string koncovka2 = "y";
                    if (pocet_novych_souboru == 1)
                    {
                        koncovka1 = "y";
                        koncovka2 = "";
                    }
                    else if (pocet_novych_souboru >= 5)
                    {
                        koncovka1 = "ych";
                        koncovka2 = "u";
                    }
                    Console.WriteLine($"\nV adresari {Global.zadany_adresar} se nachazi {pocet_novych_souboru} nov{koncovka1} soubor{koncovka2}:");
                    foreach (string novy_soubor in Global.nove_soubory.Split('\n'))
                    {
                        if (novy_soubor == "") break;
                        Console.WriteLine("    {0}", novy_soubor);
                    }
                }
                else Console.WriteLine("\nV adresari se nenachazi zadne nove soubory.");
                //Vypis zmenenych souboru:
                int pocet_zmenenych_souboru = Global.zmenene_soubory.Split('\n').GetLength(0)-1;
                if (pocet_zmenenych_souboru > 0)
                {
                    string koncovka1 = "y";
                    string koncovka2 = "e";
                    string koncovka3 = "y";
                    if (pocet_zmenenych_souboru == 1)
                    {
                        koncovka1 = "";
                        koncovka2 = "y";
                        koncovka3 = "";
                    }
                    else if (pocet_zmenenych_souboru >= 5)
                    {
                        koncovka1 = "o";
                        koncovka2 = "ych";
                        koncovka3 = "u";
                    }
                    Console.WriteLine($"\nV adresari {Global.zadany_adresar} byl{koncovka1} {pocet_zmenenych_souboru} zmenen{koncovka2} soubor{koncovka3}:");
                    foreach (string zmeneny_soubor in Global.zmenene_soubory.Split('\n'))
                    {
                        if (zmeneny_soubor == "") break;
                        Console.WriteLine("    {0}", zmeneny_soubor);
                    }
                }
                else Console.WriteLine("\nV adresari se nenachazi zadne zmenene soubory.");
                //Vypis odstranenych adresaru:
                int pocet_odstranenych_adresaru = Global.odstranene_adresare.Split('\n').GetLength(0)-1;
                if (pocet_odstranenych_adresaru > 0)
                {
                    string koncovka1 = "y";
                    string koncovka2 = "y";
                    string koncovka3 = "e";
                    if (pocet_odstranenych_adresaru == 1)
                    {
                        koncovka1 = "";
                        koncovka2 = "";
                        koncovka3 = "";
                    }
                    else if (pocet_odstranenych_adresaru >= 5)
                    {
                        koncovka1 = "o";
                        koncovka2 = "o";
                        koncovka3 = "u";
                    }
                    Console.WriteLine($"\nZ adresare {Global.zadany_adresar} byl{koncovka1} odstaranen{koncovka2} {pocet_odstranenych_adresaru} podadresar{koncovka3}:");
                    foreach (string odstraneny_adresar in Global.odstranene_adresare.Split('\n'))
                    {
                        if (odstraneny_adresar == "") break;
                        Console.WriteLine("    {0}", odstraneny_adresar);
                    }
                }
                else Console.WriteLine("\nV adresari se nenachazi zadne odstranene adresare.");
                //Vypis odstranenych souboru:
                int pocet_odstranenych_souboru = Global.odstranene_soubory.Split('\n').GetLength(0)-1;
                if (pocet_odstranenych_souboru > 0)
                {
                    string koncovka1 = "y";
                    string koncovka2 = "y";
                    string koncovka3 = "y";
                    if (pocet_odstranenych_souboru == 1)
                    {
                        koncovka1 = "";
                        koncovka2 = "";
                        koncovka3 = "";
                    }
                    else if (pocet_odstranenych_souboru >= 5)
                    {
                        koncovka1 = "o";
                        koncovka2 = "o";
                        koncovka3 = "u";
                    }
                    Console.WriteLine($"\nZ adresare {Global.zadany_adresar} byl{koncovka1} odstranen{koncovka2} {pocet_odstranenych_souboru} soubor{koncovka3}:");
                    foreach (string odstarneny_soubor in Global.odstranene_soubory.Split('\n'))
                    {
                        if (odstarneny_soubor == "") break;
                        Console.WriteLine("    {0}", odstarneny_soubor);
                    }
                }
                else Console.WriteLine("\nV adresari se nenachazi zadne odstranene soubory.");
                Console.WriteLine("\n");
            }
            //Vypis pameti:
            Console.WriteLine("\nChcete-li vypsat pamet stisknete Enter, pokud ne libovolnou jinou klavesu:");
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                string radky_pameti = File.ReadAllText($"{Global.pwd}\\pamet\\{Global.zadany_adresar.Replace(":\\", "-").Replace("\\", "_")}_pamet.txt");
                Console.WriteLine($"\nVypis pameti:\n{radky_pameti}");
            }
            Global.prvni_pruchod = false;
            Console.WriteLine("\nStisknete Escape pro unkonceni programu nebo jakoukoli jinou klavesu pro opakovani:");
            if (Console.ReadKey(true).Key != ConsoleKey.Escape) Main(args);
        }
        static void Nacteni_pameti()
        {
            StringReader SRradky_pameti = new StringReader(File.ReadAllText($"{Global.pwd}\\pamet\\{Global.zadany_adresar.Replace(":\\", "-").Replace("\\", "_")}_pamet.txt"));
            string radek_pameti = SRradky_pameti.ReadLine();
            int celkovy_pocet_adresaru_pameti = 0;
            while (radek_pameti.StartsWith(char.Parse(Global.zadany_adresar.Substring(0, 1))))
            {
                Global.adresar_pameti[celkovy_pocet_adresaru_pameti] = radek_pameti;
                radek_pameti = SRradky_pameti.ReadLine();
                if (radek_pameti != null && radek_pameti != "")
                {
                    Global.pocet_souboru_adresare_z_pameti[celkovy_pocet_adresaru_pameti] = 0;
                    while (!radek_pameti.StartsWith(char.Parse(Global.zadany_adresar.Substring(0, 1))))
                    {
                        Global.soubor_pameti[celkovy_pocet_adresaru_pameti, Global.pocet_souboru_adresare_z_pameti[celkovy_pocet_adresaru_pameti]] = radek_pameti;
                        radek_pameti = SRradky_pameti.ReadLine();
                        Global.pocet_souboru_adresare_z_pameti[celkovy_pocet_adresaru_pameti]++;
                        if (radek_pameti == null || radek_pameti == "") break;
                    }
                }
                celkovy_pocet_adresaru_pameti++;
                if (radek_pameti == null || radek_pameti == "") break;
            }
            SRradky_pameti.Close();
        }
        static void Nacteni_adresare(string adresar, int celkovy_pocet_adresaru_adresare = 0)
        {
            Global.adresar_adresare[Global.celkovy_pocet_adresaru_adresare] = adresar + " - " + Directory.GetCreationTime(adresar);
            string[] soubory = Directory.GetFiles(adresar);
            foreach (string soubor in soubory)
            {
                DateTime cas = File.GetLastWriteTime(soubor);
                string datum = cas.ToString();
                if (datum.Length < 19) datum = datum.Insert(11, "0");
                Global.soubor_adresare[Global.celkovy_pocet_adresaru_adresare, Global.pocet_souboru_adresare_z_adresare[Global.celkovy_pocet_adresaru_adresare]] = ($"01 - {datum} - {soubor.Remove(0, Global.zadany_adresar.Length + 1)}");
                Global.pocet_souboru_adresare_z_adresare[Global.celkovy_pocet_adresaru_adresare]++;
            }
            Global.celkovy_pocet_adresaru_adresare++;
            string[] podadresare = Directory.GetDirectories(adresar);
            foreach (string podadresar in podadresare) Nacteni_adresare(podadresar);
        }
        static void Reseni_ukolu()
        {
            //Data pro pozastavovani postupu pri cteni pro pripad zmen:
            int pocet_pridanych_adresaru = 0;
            int pocet_odebranych_adresaru = 0;
            for (int y = 0; y < Global.adresar_pameti.GetLength(0); y++)
            {
                int tmp_pocet_pridanych_adresaru = 0;
                int tmp_pocet_odebranych_adresaru = 0;
                if (Global.adresar_pameti[y - pocet_pridanych_adresaru] == null && Global.adresar_adresare[y - pocet_odebranych_adresaru] == null) break;
                {
                    if (Global.adresar_pameti[y - pocet_pridanych_adresaru] == null)
                    {
                        //adresar byl pridan
                        Global.nova_pamet += Global.adresar_adresare[y - pocet_odebranych_adresaru] + "\n";
                        Global.nove_adresare += Global.adresar_adresare[y - pocet_odebranych_adresaru][0..^21] + "\n";
                        //Ulozeni pridanych souboru pridaneho adresare:
                        for (int n = 0; 0 < Global.soubor_adresare.GetLength(1); n++)
                        {
                            if (Global.soubor_adresare[y - pocet_odebranych_adresaru, n] == null) break;
                            Global.nova_pamet += Global.soubor_adresare[y - pocet_odebranych_adresaru, n] + "\n";
                            Global.nove_soubory += Global.soubor_adresare[y - pocet_odebranych_adresaru, n].Remove(0, 27) + "\n";
                        }
                        tmp_pocet_pridanych_adresaru = 1;
                    }
                    if (Global.adresar_adresare[y - pocet_odebranych_adresaru] == null)
                    {
                        //adresar byl odebran
                        Global.odstranene_adresare += Global.adresar_pameti[y - pocet_pridanych_adresaru][0..^21] + "\n";
                        //Ulozeni odebranych souboru odebraneho adresare:
                        for (int n = 0; 0 < Global.soubor_pameti.GetLength(1); n++)
                        {
                            if (Global.soubor_pameti[y - pocet_pridanych_adresaru, n] == null) break;
                            Global.odstranene_soubory += Global.soubor_pameti[y - pocet_pridanych_adresaru, n].Remove(0, 27) + "\n";
                        }
                        tmp_pocet_odebranych_adresaru = 1;
                    }
                    if (Global.adresar_pameti[y - pocet_pridanych_adresaru] != null && Global.adresar_adresare[y - pocet_odebranych_adresaru] != null)
                    {
                        if (Global.adresar_pameti[y - pocet_pridanych_adresaru] != Global.adresar_adresare[y - pocet_odebranych_adresaru])
                        {
                            if (!string.Concat(Global.adresar_pameti).Contains(Global.adresar_adresare[y - pocet_odebranych_adresaru]))
                            {
                                //adresar byl pridan
                                Global.nova_pamet += Global.adresar_adresare[y - pocet_odebranych_adresaru] + "\n";
                                Global.nove_adresare += Global.adresar_adresare[y - pocet_odebranych_adresaru][0..^21] + "\n";
                                //Ulozeni pridanych souboru pridaneho adresare:
                                for (int n = 0; 0 < Global.soubor_adresare.GetLength(1); n++)
                                {
                                    if (Global.soubor_adresare[y - pocet_odebranych_adresaru, n] == null) break;
                                    Global.nova_pamet += Global.soubor_adresare[y - pocet_odebranych_adresaru, n] + "\n";
                                    Global.nove_soubory += Global.soubor_adresare[y - pocet_odebranych_adresaru, n].Remove(0, 27) + "\n";
                                }
                                tmp_pocet_pridanych_adresaru = 1;
                            }
                            if (!string.Concat(Global.adresar_adresare).Contains(Global.adresar_pameti[y - pocet_pridanych_adresaru]))
                            {
                                //adresar byl odebran
                                Global.odstranene_adresare += Global.adresar_pameti[y - pocet_pridanych_adresaru][0..^21] + "\n";
                                //Ulozeni odebranych souboru odebraneho adresare:
                                for (int n = 0; 0 < Global.soubor_pameti.GetLength(1); n++)
                                {
                                    if (Global.soubor_pameti[y - pocet_pridanych_adresaru, n] == null) break;
                                    Global.odstranene_soubory += Global.soubor_pameti[y - pocet_pridanych_adresaru, n].Remove(0, 27) + "\n";
                                }
                                tmp_pocet_odebranych_adresaru = 1;
                            }
                        }
                    }
                }
                if (tmp_pocet_pridanych_adresaru == 0 && tmp_pocet_odebranych_adresaru == 0)
                {
                    Global.nova_pamet += Global.adresar_adresare[y - pocet_odebranych_adresaru] + "\n";
                    //Data pro pozastavovani postupu pri cteni pro pripad zmen:
                    int pocet_pridanych_souboru = 0;
                    int pocet_odebranych_souboru = 0;
                    for (int x = 0; x < Global.soubor_pameti.GetLength(1); x++)
                    {
                        int tmp_pocet_pridanych_souboru = 0;
                        int tmp_pocet_odebranych_souboru = 0;
                        if (Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru] == null && Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru] == null) break;
                        else 
                        {
                            if (Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru] == null)
                            {
                                //soubor byl pridan
                                Global.nova_pamet += Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru] + "\n";
                                Global.nove_soubory += Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru].Remove(0, 27) + "\n";
                                tmp_pocet_pridanych_souboru = 1;
                            }
                            if (Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru] == null)
                            {
                                //soubor byl odebran
                                Global.odstranene_soubory += Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru].Remove(0, 27) + "\n";
                                tmp_pocet_odebranych_souboru = 1;
                            }
                            bool bez_zmeny = true;
                            if (Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru] != null && Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru] != null)
                            {
                                if (Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru].Remove(0, 2) != Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru].Remove(0, 2))
                                {
                                    if (Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru].Remove(0, 27) == Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru].Remove(0, 27))
                                    {
                                        //soubor byl zmenen
                                        int verze_souboru = int.Parse(Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru].Substring(0, 2));
                                        string nula = "0";
                                        if (verze_souboru >= 9) nula = nula.Remove(0);
                                        string posledni_zmena = Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru].Substring(5, 19);
                                        Global.zmenene_soubory += $"{Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru].Remove(0, 27)} - verze {nula}{verze_souboru + 1}\n";
                                        Global.nova_pamet += $"{nula}{verze_souboru + 1} - {posledni_zmena} - {Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru].Remove(0, 27)}\n";
                                        bez_zmeny = false;
                                    }
                                    else
                                    {
                                        //Spojeni souboru pro zjisteni toho, kde soubor chybi:
                                        string soubory_adresare_z_pameti = "";
                                        for (int n = 0; n < Global.pocet_souboru_adresare_z_pameti[y - pocet_pridanych_adresaru]; n++)
                                        {
                                            if (Global.soubor_pameti[y - pocet_pridanych_adresaru, n] == null) break;
                                            soubory_adresare_z_pameti += Global.soubor_pameti[y - pocet_pridanych_adresaru, n];
                                        }
                                        string soubory_adresare_z_adresare = "";
                                        for (int n = 0; n < Global.pocet_souboru_adresare_z_adresare[y - pocet_odebranych_adresaru]; n++)
                                        {
                                            if (Global.soubor_adresare[y - pocet_odebranych_adresaru, n] == null) break;
                                            soubory_adresare_z_adresare += Global.soubor_adresare[y - pocet_odebranych_adresaru, n];
                                        }
                                        if (!soubory_adresare_z_pameti.Contains(Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru]))
                                        {
                                            //soubor byl pridan
                                            Global.nova_pamet += Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru] + "\n";
                                            Global.nove_soubory += Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru].Remove(0, 27) + "\n";
                                            tmp_pocet_pridanych_souboru = 1;
                                        }
                                        if (!soubory_adresare_z_adresare.Contains(Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru]))
                                        {
                                            // soubor byl odebran
                                            Global.odstranene_soubory += Global.soubor_pameti[y - pocet_pridanych_adresaru, x - pocet_pridanych_souboru].Remove(0, 27) + "\n";
                                            tmp_pocet_odebranych_souboru = 1;
                                        }
                                    }
                                }
                            }
                            if (tmp_pocet_pridanych_souboru == 0 && tmp_pocet_odebranych_souboru == 0 && bez_zmeny) Global.nova_pamet += Global.soubor_adresare[y - pocet_odebranych_adresaru, x - pocet_odebranych_souboru] + "\n";
                            else if (tmp_pocet_pridanych_souboru == 0 || tmp_pocet_odebranych_souboru == 0)
                            {
                                pocet_pridanych_souboru += tmp_pocet_pridanych_souboru;
                                pocet_odebranych_souboru += tmp_pocet_odebranych_souboru;
                            }
                        }
                    }
                }
                else if (tmp_pocet_pridanych_adresaru == 0 || tmp_pocet_odebranych_adresaru == 0)
                {
                    pocet_pridanych_adresaru += tmp_pocet_pridanych_adresaru;
                    pocet_odebranych_adresaru += tmp_pocet_odebranych_adresaru;
                }
            }
        }
        static void Prvni_zapis_do_pameti(string adresar, StreamWriter pamet)
        {
            pamet.WriteLine($"{adresar} - {Directory.GetCreationTime(adresar)}");
            string[] soubory = Directory.GetFiles(adresar);
            foreach (string soubor in soubory)
            {
                DateTime cas = File.GetLastWriteTime(soubor);
                string datum = cas.ToString();
                if (datum.Length < 19) datum = datum.Insert(11, "0");
                pamet.WriteLine($"01 - {datum} - {soubor.Remove(0, Global.zadany_adresar.Length + 1)}");
            }
            string[] podadresare = Directory.GetDirectories(adresar);
            foreach (string podadresar in podadresare) Prvni_zapis_do_pameti(podadresar, pamet);
        }
    }
}
