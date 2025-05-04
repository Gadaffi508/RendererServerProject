using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 10000);
        server.Start();
        Console.WriteLine("Server başlatıldı...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Bir istemci bağlandı.");

            Thread thread = new Thread(() => HandleClient(client));
            thread.Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int byteCount;

        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string msg = Encoding.UTF8.GetString(buffer, 0, byteCount);
            Console.WriteLine("Gelen: " + msg);

            string response = "Sunucudan: " + msg.ToUpper();
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);
        }

        client.Close();
    }
}