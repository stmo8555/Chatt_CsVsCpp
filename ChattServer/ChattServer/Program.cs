// See https://aka.ms/new-console-template for more information

using System.Collections.Specialized;
using System.Threading.Channels;
using ChatServer;

var server = new Server();


while (true)
{
    Console.WriteLine("hello");
    Thread.Sleep(5000);
}
