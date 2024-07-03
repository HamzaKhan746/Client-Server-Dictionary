using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace DictionaryClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is Dictionary client");
            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ip = host.AddressList[0];
                IPEndPoint remote = new IPEndPoint(ip, 12000);

                Socket sender;
                // = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {



                    string data = "";

                    while (true)
                    {
                        byte[] buffer = new byte[2048];
                        byte[] temp = new byte[2048];
                        sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        sender.Connect(remote);
                        Console.WriteLine("Enter word to find Meaning");
                        data = Console.ReadLine();

                        buffer = Encoding.ASCII.GetBytes(data);
                        sender.Send(buffer);

                        sender.Receive(temp);
                        temp = decode(temp);
                        data = Encoding.ASCII.GetString(temp);
                        Console.WriteLine(data);




                        sender.Close();
                    }
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private static byte[] decode(byte[] buffer)
        {
            var i = buffer.Length - 1;
            while (buffer[i] == 0)
            {
                --i;
            }
            var temp = new byte[i + 1];
            Array.Copy(buffer, temp, i + 1);
            //MessageBox.Show(temp.Length.ToString());
            return temp;
        }
    }
}
