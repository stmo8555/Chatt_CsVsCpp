using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace ChattApi;

public class Api
{
    private readonly Func<string, Task> _onNewMsg;
    private readonly TcpClient _tcpClient;

    public Api(Func<string> authenticationMethod, Func<string, Task> onNewMsg)
    {
        _onNewMsg = onNewMsg;
        do
        {
            try
            {
                _tcpClient = new TcpClient("127.0.0.1", 5000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        } while (_tcpClient != null && !_tcpClient.Connected);

        Authenticate(authenticationMethod);
        Task.Run(ListeningChannel);
    }

    private void Authenticate(Func<string> authenticationMethod)
    {
        var buf = new byte[1024];
        var msg = "";
        do
        {
            var a = _tcpClient.GetStream().Read(buf);
            msg = Encoding.UTF8.GetString(buf, 0, a);
        } while (msg != "Name");

        var name = authenticationMethod();
        var bytesToSend = Encoding.UTF8.GetBytes(name);
        _tcpClient.GetStream().Write(bytesToSend);
    }

    private async void ListeningChannel()
    {
        while (true)
        {
            var buf = new byte[1024];
            var a = await _tcpClient.GetStream().ReadAsync(buf);
            var msg = Encoding.UTF8.GetString(buf, 0, a);
            await _onNewMsg(msg);
        }
    }

    public async void SendMessage(string? msg)
    {
        var bytes = Encoding.UTF8.GetBytes(msg);
        await _tcpClient.GetStream().WriteAsync(bytes);
    }
}