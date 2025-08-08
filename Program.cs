namespace ClipboardMasking.Win
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Ensure single instance
            using var instanceMutex = new System.Threading.Mutex(initiallyOwned: true, name: "Global\\ClipboardMaskingWin", out bool createdNew);
            if (!createdNew)
            {
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Forms.MainForm());
        }
    }
}