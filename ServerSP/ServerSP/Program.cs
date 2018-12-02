using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerSP
{
    class Program
    {
        static Socket socket;
        static IPAddress address;
        static int Port;

        static void Main(string[] args)
        {
            try
            {
                // Socket
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //IP Address
                address = GetIPAddress();
                //Port
                Port = 1997;
                // Binding
                socket.Bind(new IPEndPoint(address, Port));
                //Listening
                socket.Listen(10);
                //Accept
                socket = socket.Accept();
                //Recieve
                var recieveMesage = new Thread(Recieve);
                recieveMesage.Start();

                //Send
                while (true)
                {
                    byte[] SendData = Encoding.Default.GetBytes(Console.ReadLine());
                    socket.Send(SendData);
                }
                
            }
            catch(Exception ex) {
                //Close The Connection
                Console.WriteLine(ex);
                socket.Close();
            }

        }

        static IPAddress GetIPAddress()
        {
            string HostName = Dns.GetHostName();
            IPHostEntry address = Dns.GetHostEntry(HostName);
            IPAddress[] iPAddresses = address.AddressList;
            return iPAddresses[iPAddresses.Length - 1];
        }
        public static void Recieve()
        {
            while (true)
            {
                Thread.Sleep(500);
                byte[] Buffer = new byte[255];
                int bytesRecieved = socket.Receive(Buffer, 0, Buffer.Length, 0);
                Array.Resize(ref Buffer, bytesRecieved);
                string Data = Encoding.Default.GetString(Buffer);
                Console.WriteLine(Data);
            }
        }
    }
}
