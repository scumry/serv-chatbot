using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    public static async Task SaveMsg(EndPoint adres, string msg)
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 12345);
        string newIp = ModIP(adres, ipPoint);
        string[] data = msg.Split(":");
        string fileName = GenerateFilenAME(adres.ToString(), data[0]);
        using var sw = new StreamWriter(String.Format($"C:/Users/is22-11/Desktop/msg/{fileName}.txt"), true);
        await sw.WriteAsync(adres.ToString() + '_' + msg + "\n");
        sw.Flush();
    }
    public static string GenerateFilenAME(string firstIp, string secondIp)
    {
        for (int i = 0; i < firstIp.Length; i++)
        {
            int.TryParse(firstIp.Substring(i, 1), out int val);
            int.TryParse(secondIp.Substring(i, 1), out int val2);

            if (val > val2)
            {
                return ModIP(secondIp) + "_" + ModIP(firstIp);
            }
            else if (val2 > val)
            {
                return ModIP(firstIp) + "_" + ModIP(secondIp);
            }
        }
        return null;
    }
    public static string ModIP(EndPoint adres, IPEndPoint ipPoint)
    {
        return adres.ToString().Replace('.', '_').Substring(0, 15);

    }
    public static string ModIP(string adres)
    {
        return adres.ToString().Replace('.', '_').Substring(0, 15);

    }
    public static async Task Parse(string msg)
    {
    }
    public static async Task Main()
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 12345);
        socket.Bind(ipPoint);
        socket.Listen();
        while (true)
        {
            using var tcpClient = await socket.AcceptAsync();
            System.Console.WriteLine("Есть подключение");

            byte[] bytesRead = new byte[255];
            try
            {
                while (true)
                {
                    int count = await tcpClient.ReceiveAsync(bytesRead, SocketFlags.None);
                    if (count == 0)
                    {
                        System.Console.WriteLine("Соединение разорвано");
                        break;
                    }

                    string msg = Encoding.UTF8.GetString(bytesRead, 0, count);
                    await Parse(msg);
                    await Console.Out.WriteLineAsync("Принято сообщение");
                    await SaveMsg(tcpClient.RemoteEndPoint, msg);

                    Array.Clear(bytesRead, 0, bytesRead.Length); // Очистка буфера после обработки данных
                }
            }
            catch (SocketException ex)
            {
                System.Console.WriteLine($"Ошибка при передаче данных: {ex.Message}");
            }
        }


    }
}