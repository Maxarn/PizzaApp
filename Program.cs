using System;
using System.Collections.Generic;

namespace BakaPizza
{
    // En exception som man är i stort behov av om någon försöker besudla en pizza
    public class VanhelgandeException : Exception
    {
        public VanhelgandeException(string message) : base(message) { }
    }

    // En exception som används för att förhindra för mycket av det goda. Eller äckliga.
    public class DuplikatPåläggException : Exception
    {
        public DuplikatPåläggException(string message) : base(message) { }
    }

    // Interface för generell hantering av pizzorna
    public interface IPizza
    {
        List<string> nuvarandePålägg();
        void läggTillPålägg(string pålägg);
        void taBortPålägg(string pålägg);
        string pizzansNamn();
        void ändraPizzansNamn(string nyttNamn);
        void återställPålägg();
    }

    // Abstrakt klass för hur en pizza implementerar IPizza interfacet
    public abstract class Pizza : IPizza
    {
        private static readonly List<string> obligatoriska_pålägg = new List<string>(new string[] { "Tomatsås", "Ost" });

        private List<string> pålägg;
        private String namn;

        public List<string> nuvarandePålägg()
        {
            return this.pålägg;
        }

        public void läggTillPålägg(string pålägg)
        {
            if (this.pålägg.Contains(pålägg))
            {
                throw new DuplikatPåläggException("Din pizza har redan " + pålägg + " som pålägg.");
            }
            else if (pålägg.ToLower() == "ananas")
            {
                throw new VanhelgandeException("Du kan inte ha ananas på pizzan.");
            }
            else
            {
                this.pålägg.Add(pålägg);
            }
        }

        public void taBortPålägg(string pålägg)
        {
            if (obligatoriska_pålägg.Contains(pålägg))
            {
                throw new VanhelgandeException("En pizza utan " + pålägg + " är ingen pizza alls.");
            }
            else
            {
                this.pålägg.Remove(pålägg);
            }
        }

        public string pizzansNamn()
        {
            return this.namn;
        }

        public void ändraPizzansNamn(string nyttNamn)
        {
            this.namn = nyttNamn;
        }

        public void återställPålägg()
        {
            this.pålägg = new List<string>();
            foreach (var pålägg in obligatoriska_pålägg)
            {
                this.pålägg.Add(pålägg);
            }
        }
    }

    // Förinställda pizzor för den som inte har någon fantasi
    public class Vesuvio : Pizza
    {
        public Vesuvio()
        {
            this.återställPålägg();
            this.läggTillPålägg("Skinka");
            this.ändraPizzansNamn("Vesuvio");
        }
    }

    public class Kebabpizza : Pizza
    {
        public Kebabpizza()
        {
            this.återställPålägg();
            this.läggTillPålägg("Kebabkött");
            this.läggTillPålägg("Lök");
            this.läggTillPålägg("Feferoni");
            this.läggTillPålägg("Kebabsås");
            this.ändraPizzansNamn("Kebabpizza");
        }
    }

    // Pizza för dig som vill bestämma helt själv
    public class Egenpizza : Pizza
    {
        public Egenpizza()
        {
            this.återställPålägg();
            this.ändraPizzansNamn("Noobpizza");
        }
    }

    public class Program
    {
        private static IPizza vald_pizza = null;

        // Main är mest bara introduktion osv för programmet.
        public static void Main(string[] args)
        {

            Console.WriteLine("Välkommen till Max super jätte professionella pizza program!");

            bool loop = true;
            while (loop)
            {
                Console.WriteLine("\nSkriv numret till alternativet som du vill gå igenom med för att navigera i dessa menyer.\n"
                                + "1. Gör något pizzarelaterat.\n"
                                + "0. Avslutaprogrammet.");

                switch (Console.ReadLine())
                {
                    case "1":
                        huvudPizzaMeny();
                        break;
                    case "0":
                        Console.WriteLine("\nBra val.");
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("\nInput känns inte igen. Försök igen, eller ange siffran 0 för att avsluta.");
                        break;
                }
            }
        }

        // Generell hantering av pizza.
        public static void huvudPizzaMeny()
        {
            bool loop = true;
            while (loop)
            { // Mardrömsloop öppnas
                if (vald_pizza == null)
                {
                    Console.WriteLine("\nDu har inte valt någon pizza.\n"
                                    + "1. Välj en pizza.\n"
                                    + "0. Backa till tidigare meny.");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            pizzaVy();
                            break;
                        case "0":
                            loop = false;
                            break;
                        default:
                            Console.WriteLine("\nInput känns inte igen. Försök igen.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nDu har en {0}. Vad vill du göra?\n"
                                    + "1. Välj en ny pizza.\n"
                                    + "2. Hantera pålägg.\n"
                                    + "3. Konsumera pizzan.\n"
                                    + "4. Kasta bort pizzan.\n"
                                    + "5. Döp om pizzan.\n"
                                    + "0. Backa till tidigare meny.",
                                    vald_pizza.pizzansNamn());

                    switch (Console.ReadLine())
                    {
                        case "1":
                            pizzaVy();
                            break;
                        case "2":
                            påläggsVy();
                            break;
                        case "3":
                            vald_pizza = null;
                            Console.WriteLine("\nDu konsumerade pizzan.");
                            break;
                        case "4":
                            vald_pizza = null;
                            Console.WriteLine("\nDu kastade bort en pizza, wow.");
                            break;
                        case "5":
                            Console.WriteLine("\nAnge pizzans nya namn:");
                            vald_pizza.ändraPizzansNamn(Console.ReadLine());
                            break;
                        case "0":
                            loop = false;
                            break;
                        default:
                            Console.WriteLine("\nInput känns inte igen. Försök igen.");
                            break;
                    }
                }
            } // Mardrömsloop sluts
        }

        // Meny för val av pizza
        public static void pizzaVy()
        {
            bool loop = true;
            while (loop)
            { // Mardrömsloop öppnas
                Console.WriteLine("\nVälj en pizza från mitt massiva utbud!\n"
                                + "1. Kebabpizza.\n"
                                + "2. Vesuvio.\n"
                                + "3. Tom pizza. Bäst för dig att du lägger lite pålägg på den först.\n"
                                + "0. Backa till tidigare meny.");

                switch (Console.ReadLine())
                {
                    case "1":
                        vald_pizza = new Kebabpizza();
                        loop = false;
                        break;
                    case "2":
                        vald_pizza = new Vesuvio();
                        loop = false;
                        break;
                    case "3":
                        vald_pizza = new Egenpizza();
                        loop = false;
                        break;
                    case "0":
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("\nInput känns inte igen. Försök igen.");
                        break;
                }
            } // Mardrömsloop sluts
        }

        public static void påläggsVy()
        {
            bool loop = true;
            while (loop)
            { // Mardrömsloop öppnas
                string pålägg_sträng = "";
                List<string> nuvarande_pålägg = vald_pizza.nuvarandePålägg();

                for (int i = 0; i < nuvarande_pålägg.Count - 2; i++)
                {
                    pålägg_sträng += nuvarande_pålägg[i] + ", ";
                }

                pålägg_sträng += nuvarande_pålägg[nuvarande_pålägg.Count - 2] + " och " + nuvarande_pålägg[nuvarande_pålägg.Count - 1] + ".";

                Console.WriteLine("\nDin pizza har följande pålägg:\n"
                                + "{0}\n"
                                + "1. Lägg till ett pålägg.\n"
                                + "2. Ta bort ett pålägg.\n"
                                + "0. Backa till tidigare meny.",
                                pålägg_sträng);

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("\nAnge namnet på pålägget som du vill lägga till:");
                        try
                        {
                            vald_pizza.läggTillPålägg(Console.ReadLine());
                        }
                        catch (Exception e) when (e is DuplikatPåläggException || e is VanhelgandeException)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    case "2":
                        påläggsBorttagningsByrån();
                        break;
                    case "0":
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("\nInput känns inte igen. Försök igen.");
                        break;
                }
            } // Mardrömsloop sluts
        }

        // Denna funktionen används för att ta bort nuvarande pålägg.
        public static void påläggsBorttagningsByrån()
        {
            bool loop = true;
            while (loop)
            { // Mardrömsloop öppnas
                string pålägg_sträng = "";
                List<string> nuvarande_pålägg = vald_pizza.nuvarandePålägg();

                for (int i = 0; i < nuvarande_pålägg.Count; i++)
                {
                    pålägg_sträng += (i + 1) + ". " + nuvarande_pålägg[i] + "\n";
                }

                Console.WriteLine("\nVilket pålägg vill du amputera?\n"
                                + "{0}"
                                + "0. Backa till tidigare meny.",
                                pålägg_sträng);

                int parsed_value;
                int.TryParse(Console.ReadLine(), out parsed_value);
                if (-1 < (parsed_value - 1) && (parsed_value - 1) < nuvarande_pålägg.Count)
                {
                    try
                    {
                        vald_pizza.taBortPålägg(nuvarande_pålägg[parsed_value - 1]);
                    }
                    catch (VanhelgandeException e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else if (parsed_value == 0)
                {
                    loop = false;
                }
                else
                {
                    Console.WriteLine("\nInput känns inte igen. Försök igen.");
                }
            } // Mardrömsloop sluts
        }
    }
}
