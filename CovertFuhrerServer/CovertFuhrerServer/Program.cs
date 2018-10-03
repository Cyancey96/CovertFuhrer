using System;

namespace CovertFuhrerServer
{
    internal class Program
    {
        private static void Main()
        {
            Console.Title = "Covert Fuhrer Server";

            using (var server = new Server())
                server.Start();
        }
    }
}
