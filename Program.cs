namespace Fantasma
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (Core core = new Core(720, 480, "Fantasma"))
            {
                core.Run();
            }
        }
    }
}
