namespace MagicVilla_VillaAPI.Logger

{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine("Error : " + message);
            }
            else if (type == "warning")
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Error : " + message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {

                Console.WriteLine(message);
            }
            
        }
    }
}
