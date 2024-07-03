using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace np_project_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\Hamza Khan\Desktop\New folder (2)\Hamza\Dictionary.txt";
            string[] lines = File.ReadAllLines(filePath);
            Console.WriteLine("Welcome To Dictionary Server");
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ip = host.AddressList[0];
            IPEndPoint ipe = new IPEndPoint(ip, 12000);
            try
            {
                Socket listner = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listner.Bind(ipe);
                listner.Listen(10000);
                bool isStart = true, loop = true;

                string data = "";


                Socket handler = null;
                while (loop)
                {
                    try
                    {
                        handler = listner.Accept();
                    }
                    catch (Exception e)
                    {

                    }
                    byte[] buffer = new byte[1024];
                    byte[] temp = new byte[1024];
                    string meaning = "Meaning Not Found try Again";
                    handler.Receive(buffer);
                    temp = decode(buffer);
                    //Console.WriteLine(buffer + "found");
                    data = Encoding.Default.GetString(temp);
                    //Encoding.ASCII.GetString(buffer);

                    Console.WriteLine(data + " receive from client");
                    foreach (string line in lines)
                    {
                        string[] strSplit = line.Split('=');
                        if (data == strSplit[0])
                        {
                            Console.WriteLine(strSplit[0] + "found");
                            meaning = strSplit[1];
                            break;
                        }
                    }
                    buffer = Encoding.ASCII.GetBytes(meaning);
                    handler.Send(buffer);

                    //loop=false;
                }


                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
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
            return temp;
        }

    }
}