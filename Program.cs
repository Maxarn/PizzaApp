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
        private static readonly List<string> obligatoriska_pålägg = new List<string>(new string[] { "Tomato sauce", "Cheese" });

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
                throw new DuplikatPåläggException("Your pizza already have " + pålägg + " as a topping.");
            }
            else if (pålägg.ToLower() == "pineapple")
            {
                throw new VanhelgandeException("You can't have pineapple on a pizza.");
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
                throw new VanhelgandeException("A pizza without " + pålägg + " ain't no pizza at all.");
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
            this.läggTillPålägg("Ham");
            this.ändraPizzansNamn("Vesuvio");
        }
    }

    public class Kebabpizza : Pizza
    {
        public Kebabpizza()
        {
            this.återställPålägg();
            this.läggTillPålägg("Kebab meat");
            this.läggTillPålägg("Onions");
            this.läggTillPålägg("Feferoni");
            this.läggTillPålägg("Kebab sauce");
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

            Console.WriteLine("Welcome to Max super very professional pizza program!");

            bool loop = true;
            while (loop)
            {
                Console.WriteLine("\nTo navigate though these menus, you'll have to type the number of the desired option.\n"
                                + "1. Do something related with pizzas.\n"
                                + "0. Exit the program.");

                switch (Console.ReadLine())
                {
                    case "1":
                        huvudPizzaMeny();
                        break;
                    case "0":
                        Console.WriteLine("\nGood choice.");
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("\nThe provided input couldn't be processed.");
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
                    Console.WriteLine("\nYou don't have a pizza.\n"
                                    + "1. Choose a pizza.\n"
                                    + "0. Return to the previous menu.");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            pizzaVy();
                            break;
                        case "0":
                            loop = false;
                            break;
                        default:
                            Console.WriteLine("\nThe provided input couldn't be processed.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nYou got a {0}. What do you wish to do?\n"
                                    + "1. Choose a new pizza.\n"
                                    + "2. Manage toppings.\n"
                                    + "3. Consume the pizza.\n"
                                    + "4. Throw away the pizza.\n"
                                    + "5. Rename the pizza.\n"
                                    + "0. Return to the previous menu.",
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
                            Console.WriteLine("\nYou Consumed the pizza.");
                            break;
                        case "4":
                            vald_pizza = null;
                            Console.WriteLine("\nYou threw away the pizza, wow.");
                            break;
                        case "5":
                            Console.WriteLine("\nProvide the new name for the pizza:");
                            vald_pizza.ändraPizzansNamn(Console.ReadLine());
                            break;
                        case "0":
                            loop = false;
                            break;
                        default:
                            Console.WriteLine("\nThe provided input couldn't be processed.");
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
                Console.WriteLine("\nFeel free to pick anything from this massive range of options!\n"
                                + "1. Kebabpizza.\n"
                                + "2. Vesuvio.\n"
                                + "3. An empty pizza. You better add some toppings to it.\n"
                                + "0. Return to the previous menu.");

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
                        Console.WriteLine("\nThe provided input couldn't be processed.");
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

                pålägg_sträng += nuvarande_pålägg[nuvarande_pålägg.Count - 2] + " and " + nuvarande_pålägg[nuvarande_pålägg.Count - 1] + ".";

                Console.WriteLine("\nYour pizza has the following toppings:\n{0}\n"
                                + "1. Add another topping.\n"
                                + "2. Remove a topping.\n"
                                + "0. Return to the previous menu.",
                                pålägg_sträng);

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("\nProvide the name for the topping you wish to add:");
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
                        Console.WriteLine("\nThe provided input couldn't be processed.");
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

                Console.WriteLine("\nWhich topping do you wish to amputate?\n"
                                + "{0}"
                                + "0. Return to the previous menu.",
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
                    Console.WriteLine("\nThe provided input couldn't be processed.");
                }
            } // Mardrömsloop sluts
        }
    }
}
