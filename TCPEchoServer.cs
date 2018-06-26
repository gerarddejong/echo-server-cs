using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace EchoServer
{
    class TCPEchoServer
    {
        private const int port = 5005;
        
        static void Main(string[] args) 
        {
            try 
            {
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                while (true) 
                { 
                    NetworkStream networkStream = null;
                    TcpClient client = null;

                    try 
                    {
                        Console.WriteLine("Listening for client on port: " + port);

                        client = listener.AcceptTcpClient(); // Wait for a client to connect
                        networkStream = client.GetStream();
                        
                        Console.WriteLine("Accepted connection from client port: " + ((IPEndPoint)client.Client.RemoteEndPoint).Port);

                        byte[] welcome = Encoding.ASCII.GetBytes("Welcome to Echo Server (type 'quit' to close connection)...\n");
                        networkStream.Write(welcome, 0, welcome.Length);

                        byte[] receiveBuffer = new byte[1024];
                        int receivedBytesCount;
                        while ((receivedBytesCount = networkStream.Read(receiveBuffer, 0, receiveBuffer.Length)) > 0) 
                        {
                            String message = Encoding.ASCII.GetString(receiveBuffer);
                            Console.Write(message);
                            
                            if(message.Length > 4 && "quit".Equals(message.Substring(0, 4)))
                            {
                                Console.WriteLine("Client has quit!\n");
                                break;
                            }
                            else
                            {
                                networkStream.Write(receiveBuffer, 0, receivedBytesCount); // Echo
                            }
                        }
                    } 
                    catch (Exception e) 
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        networkStream.Close();
                        client.Close();
                    }
                }
            } 
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }
    }
}
