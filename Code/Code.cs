using System;

namespace Password_Manager
{
    public class Code
    {
        public static void Main(string[] args)
        {
            Library.InitializeLibrary_Public();

            Console.Clear();
            Console.WriteLine($"!Menu!\n\n\n 1. Add Accounts\n 2. Delete Accounts\n 3. Show Accounts\n 4. Delete Database\n 5. Calculator\n 6.Exit");
            var answer = Console.ReadLine();
            if (answer == null) Main(args);

            switch (answer)
            {
                case "1":
                    Add_Accounts(); break;
                case "2":
                    Delete_Accounts(); break;
                case "3":
                    Show_Accounts(); break;
                case "4":
                    Library.DeleteDatabase_Public(); break;
                case "5":
                    Use_Calculator(0,0,0,'X'); break;
                case "6":
                    Environment.Exit(0); break;
            }
            Main(args);
        }

        static void Add_Accounts()
        {
            Console.Clear();
            Console.WriteLine("State Website:");
            var reason = Console.ReadLine();
            Console.WriteLine("State Accoutname and Password:");
            var logininfo = Console.ReadLine();
            if (reason != null) Library.AddAccountsToLibrary_Public(reason, logininfo); else Add_Accounts();
            Library.SaveDictionary_Public();
            return;
        }

        static void Delete_Accounts()
        {
            Console.Clear();
            Console.WriteLine("State Website-Name:");
            var websitename = Console.ReadLine();
            if (websitename != null) Library.RemoveAccountsFromLibrary_Public(websitename); else Delete_Accounts();
            Library.SaveDictionary_Public();
            return;
        }

        static void Show_Accounts()
        {
            Console.Clear();
            Library.ShowAccountsFromLibrary_Public();
            Console.WriteLine("\nEnd"); Console.ReadLine(); Console.Clear();
            return;
        }

        static void Use_Calculator(double result, double display1, double display2, char op)
        {
            Calculator.Calculate(0,0,out result, out display1, out display2, out op);
            Console.Clear(); Console.WriteLine($"{display1}   {op}    {display2}   =  {result}"); Console.ReadLine();
            return;
        }
    }
}
