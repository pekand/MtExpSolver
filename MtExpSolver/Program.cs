using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;

namespace MtExpSolver
{
    internal static class Program
    {
        public static string AppName = "MtExpSolver";
        public static string roamingPath = "";

        public static FormMtExpSolver? formMtExpSolver = null;

        public static Mutex? mutex = null;


        public static string CalcHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes); // Converts bytes to a hex string.
            }
        }

        public static bool createMutex(string path)
        {

            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Dispose();
            }

            string mutexName = "Global\\" + Program.AppName + "-" + CalcHash(path);

            bool createdNew;
            mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                return false;
            }

            return true;
        }

        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            string path = "";

            roamingPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                AppName
            );

            if (!Directory.Exists(roamingPath))
            {
                Directory.CreateDirectory(roamingPath);
            }

            path = Path.Combine(roamingPath, "config.MtExpSolver");


            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    path = arg;
                }
            }

            if (!createMutex(path))
            {
                return;
            }

            formMtExpSolver = new FormMtExpSolver(path);
            
            Application.Run(formMtExpSolver);
        }
    }
}