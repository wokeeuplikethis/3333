using ThirdKR;

namespace ThirdKR_Fin
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            AppForm appForm = new AppForm();
            Calculator calculator = new Calculator();
            MessagesService messagesService = new MessagesService();
            Validator validator = new Validator();
            FileManager fileManager = new FileManager();
            Presenter presenter = new Presenter(appForm, calculator, messagesService, validator, fileManager);
            presenter.Run();
        }
    }
}