using Newtonsoft.Json;
using System.Security;
using System.Security.Cryptography;

namespace Password_Manager
{
    [SecurityCritical]
    public class Library : Code
    {
        private static protected Dictionary<string, string> Account_Library = new Dictionary<string, string>();

        private static protected string Software_Path = Environment.CurrentDirectory;
        private static protected string Dictionary_Path = Environment.CurrentDirectory + "/" + "saves" + "/" + "save.pwm";
        private static protected string AesKey_Save_Path = Environment.CurrentDirectory + "/" + "saves" + "/" + "key.pwn";
        private static protected string AesVector_Save_Path = Environment.CurrentDirectory + "/" + "saves" + "/" + "vector.pwm";
        private static protected Aes? myAes;

        private static protected string MainAESKey = "";
        private static protected string MainVectorKey = "";

        //Add the secret Key and vector you generated 
        private protected const string secret1 = ""; //bmTjOUAaFl4+TKGNzmSkqBeo0R
        private protected const string secret2 = ""; //8LE/Uyq+KWi3hRlDo=

        private protected const string vector1 = ""; //EwIcKW4W7Xk
        private protected const string vector2 = ""; //iCV3KTEgGPQ==

        private protected const string secretAESKey = secret1 + secret2; //bmTjOUAaFl4+TKGNzmSkqBeo0R8LE/Uyq+KWi3hRlDo=
        private protected const string secretAESVector = vector1 + vector2; //EwIcKW4W7XkiCV3KTEgGPQ==

        public static void InitializeLibrary_Public() => Initialize_Library();
        public static void DeleteDatabase_Public() => DeleteDatabase();
        public static void SaveDictionary_Public() => SaveDictionary();
        public static void WriteToLibrary_Public(string website, string accdata) => WriteToLibrary(website, accdata);
        public static void AddAccountsToLibrary_Public(string reason, string info) => AddAccountsToLibrary(reason, info);
        public static void RemoveAccountsFromLibrary_Public(string website) => RemoveFromLibrary(website);
        public static void ShowAccountsFromLibrary_Public() => ShowAccountsFromLibrary();

        private static protected void Initialize_Library()
        {
            Directory.CreateDirectory(Software_Path + "/" + "saves");
            MainAESKey = ReturnKey(); MainVectorKey = ReturnVector(); LoadDictionary();
            Thread.Sleep(1000);

            if (secretAESKey == "" | secretAESVector == "")
            {
                Console.Clear(); Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("You need to specify a secret and a vector key in Code! Otherwise this wont work");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\n\n 1. Generate a Vector and a Key\n 2. Close");
                var answer = Console.ReadLine();
                if (answer == "1") CreateNewStaticAES();
                Environment.Exit(0);
            }

            if (MainAESKey == null | MainAESKey == "" | MainAESKey == "Error" | MainVectorKey == null | MainVectorKey == "" | MainVectorKey == "Error" | !File.Exists(Dictionary_Path))
            {
                Console.WriteLine($"\n!Error-Menu!\n 1.Create New Key (Lose all Saved Data)\n 2. Close");
                var answer = Console.ReadLine();
                if (answer == "1") CreateNewAes();
                if (answer == "2") Environment.Exit(0);
                if (answer != "1" && answer != "2") Console.Clear(); Initialize_Library();
            }

            return;
        }

        private static protected void CreateNewStaticAES()
        {
            Console.Clear();
            using (myAes = Aes.Create())
            {
                myAes.GenerateKey();
                myAes.GenerateIV();
            }
            string aes = Convert.ToBase64String(myAes.Key);
            string iv = Convert.ToBase64String(myAes.IV);
            Console.WriteLine($"Generated Key:  {aes}      \nGenerated Vector:  {iv}\n\n Press any Key to Close");
            Console.ReadLine(); Environment.Exit(0);
        }

        private static protected void CreateNewAes()
        {
            Console.Clear();
            File.Delete(Dictionary_Path);
            File.Delete(AesKey_Save_Path);
            File.Delete(AesVector_Save_Path);
            using (myAes = Aes.Create())
            {
                myAes.KeySize = 256;
                myAes.GenerateKey();
                myAes.GenerateIV();
                MainAESKey = Convert.ToBase64String(myAes.Key);
                MainVectorKey = Convert.ToBase64String(myAes.IV);

                string temp_dictionary = JsonConvert.SerializeObject(Account_Library);

                string encrypted_aes_key = EncryptDataWithAes(MainAESKey, secretAESKey, secretAESVector);
                string enrypted_aes_vector = EncryptDataWithAes(MainVectorKey, secretAESKey, secretAESVector);
                string encrypted_dictionary = EncryptDataWithAes(temp_dictionary, MainAESKey, MainVectorKey);


                File.WriteAllText(AesKey_Save_Path, encrypted_aes_key);
                File.WriteAllText(AesVector_Save_Path, enrypted_aes_vector);
                File.WriteAllText(Dictionary_Path, encrypted_dictionary);
            }
        }

        private static protected void ShowAccountsFromLibrary()
        {
            foreach (var item in Account_Library)
            {
                Console.WriteLine(item);
            }
            return;
        }

        private static protected void RemoveFromLibrary(string w)
        {
            if (Account_Library.Remove(w))
            {
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("Success"); Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed; Console.WriteLine("Error Key not found"); Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
            }
        }

        private static protected void AddAccountsToLibrary(string r, string info)
        {
            try
            {
                WriteToLibrary_Public(r, info);
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("Success"); Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed; Console.WriteLine(ex.Message); Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
                return;
            }
            return;
        }

        private static protected void WriteToLibrary(string w, string acd)
        {
            Account_Library.Add(w, acd);
            return;
        }

        private static protected void DeleteDatabase()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed; Console.WriteLine("This cant be undone!"); Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Youre Sure ?\n Y/N");
            var answer = Console.ReadLine();
            switch (answer)
            {
                case "Y":
                    break;
                case "N":
                    return;
            }

            try
            {
                File.Delete(Dictionary_Path);
                File.Delete(AesKey_Save_Path);
                File.Delete(AesVector_Save_Path);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Success   Closing Application");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Error   Not Being able to delete Database   {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
                return;
            }
            return;
        }

        private static protected void SaveDictionary()
        {
            try
            {
                string decrypted_dictionary = JsonConvert.SerializeObject(Account_Library);
                string encrypted_dictionary = EncryptDataWithAes(decrypted_dictionary, MainAESKey, MainVectorKey);
                File.WriteAllText(Dictionary_Path, encrypted_dictionary);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Saving Success!");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Thread.Sleep(100000);
        }

        private static protected void LoadDictionary()
        {
            try
            {
                string encrypted_dictionary = File.ReadAllText(Dictionary_Path);
                string decrypted_dictionary = DecryptDataWithAes(encrypted_dictionary, MainAESKey, MainVectorKey);
                Account_Library = JsonConvert.DeserializeObject<Dictionary<string, string>>(decrypted_dictionary);
                return;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Fatal Error while Loading save.pwm:   {ex.Message}\n");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
        }

        private static protected string ReturnVector()
        {
            try
            {
                string encrypted_vector = File.ReadAllText(AesVector_Save_Path);
                string decrypted_vector = DecryptDataWithAes(encrypted_vector, secretAESKey, secretAESVector);
                return decrypted_vector;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Fatal Error while Loading vector.pwm:   {ex.Message}\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            return "Error";
        }

        private static protected string ReturnKey()
        {
            try
            {
                var encrypted_key = File.ReadAllText(AesKey_Save_Path);
                var decrypted_key = DecryptDataWithAes(encrypted_key, secretAESKey, secretAESVector);
                return decrypted_key;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Fatal Error while Loading key.pwm:   {ex.Message}\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            return "Error";
        }

        private static protected string DecryptDataWithAes(string text, string aes, string iv)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String(aes);
                aesAlgorithm.IV = Convert.FromBase64String(iv);
                aesAlgorithm.Padding = PaddingMode.ISO10126;


                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();

                byte[] bytes = Convert.FromBase64String(text);

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        private static protected string EncryptDataWithAes(string text, string aes, string iv)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String(aes);
                aesAlgorithm.IV = Convert.FromBase64String(iv);
                aesAlgorithm.Padding = PaddingMode.ISO10126;

                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();

                byte[] encryptedData;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(text);
                        }
                        encryptedData = ms.ToArray();
                    }
                }

                return Convert.ToBase64String(encryptedData);
            }
        }
    }
}
