using System.ServiceModel;
using System;
using ClassLibrary;

namespace ConsoleApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceHost host = new ServiceHost(typeof(ServiceChat));
                host.Open();
                Console.WriteLine("listening on http://localhost:8733...");
                Console.ReadKey();
            }
            catch(AddressAlreadyInUseException)
            {
                Console.WriteLine("Address aready in use!");
            }
        }
    }
}
