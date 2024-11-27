namespace MtExpSolver
{
    internal static class Program
    {
        public static string AppName = "MtExpSolver";
        public static string roamingPath = "";

        public static FormMtExpSolver? formMtExpSolver = null;

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

            formMtExpSolver = new FormMtExpSolver(path);
            
            Application.Run(formMtExpSolver);
        }
    }
}