using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer;

public class Server
{
    private readonly Dictionary<string, Socket> _users = new Dictionary<string, Socket>();
    private readonly Socket _socket;

    public Server()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000));
        _socket.Listen();
        Task.Run(AcceptConnections);
    }

    private async Task AcceptConnections()
    {
        try
        {
            while (true)
            {
                var client = await _socket.AcceptAsync();
                GetNameAndStartListen(client);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
        
    }

    private async void GetNameAndStartListen(Socket client)
    {
        var msg = "Name";
        var msgBuffer = Encoding.UTF8.GetBytes(msg);
        await client.SendAsync(msgBuffer);
        var buffer = new byte[1024];
        var bytes = await client.ReceiveAsync(buffer);
        var name = Encoding.UTF8.GetString(buffer, 0, bytes);
        _users.Add(name, client);
        Console.WriteLine("added user: " + name);
        OnReceivedData(client);
        Console.WriteLine("IM LEAVING");
    }

    private async void OnReceivedData(Socket client)
    {
        var buffer = new byte[1024];
        while (true)
        {
            var bytes =  await client.ReceiveAsync(buffer);
            var msgReceived = Encoding.UTF8.GetString(buffer, 0, bytes);
            Console.WriteLine(msgReceived);
            await Task.Delay(10000);
        }
    }
}