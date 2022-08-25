using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace pendu
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.Title = "Jeux du pendu";
            while (true)
            {
                
                Request();
                
                Console.WriteLine("Jeux du pendu");
                Console.WriteLine("Veuillez rentrer seulement une lettre, il n'y a pas d'accent");
                Console.WriteLine("Choisissez la dificulté");
                Console.WriteLine("Veuillez taper 0,1,2 ou 3");
                Console.WriteLine("0: dfficulté facile -> 11 tentative ");
                Console.WriteLine("1: dfficulté moyenne -> 9 tentative ");
                Console.WriteLine("2: dfficulté difficile -> 7 tentative ");
                Console.WriteLine("3: dfficulté très difficile -> 5 tentative ");
                
                while(true)
                {
                    string difficult = Console.ReadLine();
                    if (difficult == "0")
                    {
                        Utility.Difficulté = 11;
                        break;
                    }
                    else if (difficult == "1")
                    {
                        Utility.Difficulté = 9;
                        break;
                    }
                    else if (difficult == "2")
                    {
                        Utility.Difficulté = 7;
                        break;
                    }
                    else if (difficult == "3")
                    {
                        Utility.Difficulté = 5;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Merci de rentrer un chiffre valide");
                        continue;
                    }
                }
                
                Console.Clear();
                Console.WriteLine("Vous avez le droit à " + Utility.Difficulté +  " mauvaise réponses");
                Lettres();
                Acceuil(); 
                List<string> LettrePropose = new List<string>();
                int NbLettrePropose = 0;
                Utility.Essai = 0;
                while (Utility.Essai != Utility.Difficulté)
                {
                    
                    Utility.Restant = false;
                    Utility.Succès = false;
                    Console.WriteLine("");
                    Console.WriteLine("");
                    string c = Console.ReadLine();
                    Console.Clear();
                    LettrePropose.Add(c);
                    LettrePropose.Sort();
                    NbLettrePropose++;

                    int a = 0;
                    Console.WriteLine("");
                    while (a != Utility.NbLt)
                    {
                        if (c == Utility.LettersModif[a])
                        {
                            Utility.Lettretrouvé[a] = Utility.Letters[a];
                        }
                        else if(c != Utility.LettersModif[a])
                        {
                            Utility.Raté++;
                        }
                        
                        Console.Write(Utility.Lettretrouvé[a]);
                        Console.Write(" ");
                        a++;
                    }
                    Console.WriteLine("");
                    if (Utility.Raté == Utility.NbLt)
                    {
                        Utility.Essai++;
                        Utility.Raté = 0;
                        int essairestant = Utility.Difficulté - Utility.Essai;
                        Console.WriteLine("");
                        Console.WriteLine("Il vous reste encore " + essairestant + " essais");
                    }
                    else
                    {
                        Utility.Raté = 0;
                    }
                    a = 0;
                    Console.WriteLine("");
                    string check = "_";
                    while (a != Utility.NbLt)
                    {
                        if (check == Utility.Lettretrouvé[a])
                        {
                            Utility.Restant = true;
                        }
                        a++;
                    }
                    if (Utility.Restant == false)
                    {
                        Utility.Succès = true;
                        break;
                    }
                    
                    int w = 0;
                    Console.WriteLine("Voici les lettres que vous avez déjà proposé : ");
                    while (w != NbLettrePropose)
                    {
                        Console.Write(LettrePropose[w]);
                        w++;
                    }

                }

                Console.WriteLine("");
                if (Utility.Succès == false)
                {
                    Console.Clear();
                    Console.WriteLine("Trop de tentative");
                    Console.WriteLine("Voilà le mot qu'il fallait trouver : ");
                    int i = 0;
                    while (i != Utility.NbLt)
                    {
                        Console.Write(Utility.Letters[i]);
                        Console.Write(" ");
                        i++;
                    }
                    Console.WriteLine("");
                }
                if (Utility.Succès == true)
                {
                    Console.Clear();
                    int i = 0;
                    while (i != Utility.NbLt)
                    {
                        Console.Write(Utility.Letters[i]);
                        Console.Write(" ");
                        i++;
                    }
                    Console.WriteLine("");
                    Console.WriteLine("Bien joué vous avez trouvé le mot");
                }
                Console.WriteLine("Voulez vous réessayer avec un autre mot ?");
                Console.WriteLine("Veuillez taper 1 ou 2");
                Utility.Succès = false;

                Console.WriteLine("1: oui, 2:non");
                while (true)
                {
                    Utility.option = Console.ReadLine();
                    if (Utility.option == "1")
                    {

                        Utility.LettersModif.Clear();
                        Utility.Essai = 0;
                        Utility.NbLt = 0;
                        Console.Clear();
                        break;

                    }
                    else if (Utility.option == "2")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Merci de rentrer un chiffre valide");
                        continue;

                    }
                }
                if (Utility.option == "1")
                {

                    Utility.LettersModif.Clear();
                    Utility.Essai = 0;
                    Utility.NbLt = 0;
                    Console.Clear();
                    continue;

                }
                else if(Utility.option == "2")
                {
                    break;
                }


            }
            
        }
        static void Request()
        {
            WebRequest req = WebRequest.Create("https://www.motsqui.com/mots-aleatoires.php?Submit=Nouveau+mot");
            req.Method = "GET";
            using var webResponse = req.GetResponse();
            using var webStream = webResponse.GetResponseStream();
            using var reader = new StreamReader(webStream);
            var response = reader.ReadToEnd();
            string[] separatingStrings = { "<a href=\"http://www.le-dictionnaire.com/definition.php?mot=", "", "\" target=" };
            string[] words = response.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

            int i = 0;
            foreach (var word in words)
            {
                if (i == 3)
                {
                    Utility.Mot = word;
                    
                }

                i++;   
            }
        }
        static void Lettres()
        {

            List<string> LettersModif = new List<string>();
            List<string> Letters = new List<string>();
            LettersModif.Clear();
            try
            {
                
                foreach (char letter in Utility.Mot)
                {
                    Letters.Add("" + letter + "");
                    if (letter == 'ê')
                    {
                        LettersModif.Add("e");
                    }
                    else if (letter == 'É')
                    {
                        LettersModif.Add("e");
                    }
                    else if (letter == 'é')
                    {
                        LettersModif.Add("e");
                    }
                    else if (letter == 'è')
                    {
                        LettersModif.Add("e");
                    }
                    else if (letter == 'ô')
                    {
                        LettersModif.Add("o");
                    }
                    else if (letter == 'â')
                    {
                        LettersModif.Add("a");
                    }
                    else
                    {
                        LettersModif.Add("" + letter + "");
                    }                    
                    Utility.NbLt++;

                }
                Utility.LettersModif = LettersModif;
                Utility.Letters = Letters;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        static void Acceuil()
        {
            List<string> acceuil = new List<string>();
            int b = 0;
            while (Utility.NbLt != b)
            {
                acceuil.Add("_");
                Console.Write(acceuil[b]);
                Console.Write(" ");
                b++;
            }
            Utility.Lettretrouvé = acceuil;
            Console.WriteLine("");
            Console.WriteLine("Le mot a " + Utility.NbLt + " Lettres");
        }

    }
    class Utility
    {
        public static string Mot { get; set; }
        public static int NbLt { get; set; }
        public static List<string> LettersModif { get; set; }
        public static List<string> Letters { get; set; }
        public static int Raté { get; set; }
        public static int Essai { get; set; }
        public static List<string> Lettretrouvé { get; set; }
        public static bool Succès { get; set; }
        public static bool Restant { get; set; }
        public static int Difficulté { get; set; }
        public static string option { get; set; }
    }
}
