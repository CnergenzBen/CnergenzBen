using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpServer
{
    
    public partial class Form1:Form
    {
        
        public Form1( )
        {
            InitializeComponent();
            textBoxPort.Text = "50000";
            textBoxServer.Text = "192.168.1.13";
        }
        public static void AddcomboBox1(string item )
        {
            Form1 obj = new Form1();
            obj.comboBox1.Items.Add(item);
        }
        private void button1_Click(object sender , EventArgs e)
        {
            Tcpserver.createServer(textBoxServer.Text, textBoxPort.Text);
        }
    }

    public class Tcpserver
    {
        private static Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();
        public static Socket socketSend;
        public static void createServer( string IP , String Port )
        {
            try
            {
                Socket socketWatch = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(IP);
                //Create port object
                IPEndPoint point = new IPEndPoint(ip , Convert.ToInt32(Port));
                //monitor
                socketWatch.Bind(point);

                Console.WriteLine("Monitor success");
                //The maximum number that can be monitored at a certain point in time, more than the set number will be queued
                socketWatch.Listen(10);

                //Start the thread and keep monitoring the client connection
                Thread th = new Thread(Listen);
                th.IsBackground = true;
                th.Start(socketWatch);
            }
            catch(SocketException ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

       public static void  Listen(object o)
        {
            //The parameter in the thread can only be object
            Socket socketWatch = o as Socket;
            while(true)
            {
                try
                {
                    //Waiting for the connection, the connection is successful and returns a socket for data transmission and reception, while loop can allow different clients to connect
                    socketSend = socketWatch.Accept();
                    //Store the ip port of the remote client and the corresponding socket in the key-value pair
                    dicSocket.Add(socketSend.RemoteEndPoint.ToString() , socketSend);
                    //Store the ip port and corresponding socket of the remote client in the drop-down menu for selection

                    //comboBox1.Items.Add(socketSend.RemoteEndPoint.ToString());
                    Form1.AddcomboBox1(socketSend.RemoteEndPoint.ToString());
                    //After the connection is successful, return the IP and port of each client
                    MessageBox.Show(socketSend.RemoteEndPoint.ToString() + ":" + " connection succeeded" + "\r\n");
                    //Start a new thread to continuously receive messages from the client
                    Thread th = new Thread(DataReceive);
                    th.IsBackground = true;
                    th.Start(socketSend);
                }
                catch(SocketException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

       public static void DataReceive(object o)
        {
            Socket socketSend = o as Socket;
            // keep receiving messages
            while(true)
            {
                try
                {
                    //Define receiving buffer
                    byte [ ] bufferReceive = new byte [1024 * 1024 * 2];
                    //The number of bytes received, when the client goes offline, receiveNumber=0
                    int receiveNumber = socketSend.Receive(bufferReceive);

                    //Judging whether the client is offline, to prevent the server from receiving empty messages when the client is offline
                    if(receiveNumber == 0)
                    {
                        break;
                    }
                    //Bytes are converted to strings
                    string str = Encoding.UTF8.GetString(bufferReceive , 0 , receiveNumber);
                    send(str);
                    //Print to the form
                    Console.WriteLine(socketSend.RemoteEndPoint + ": " + str);
                }
                catch(SocketException e)
                {
                    Console.WriteLine(e.ToString());
                }

            }

        }

        public static void send(string data)
        {
            string str = data.Trim();
            byte [ ] sendMessage = System.Text.Encoding.UTF8.GetBytes(str);

            //Get the user's IP address in the drop-down box
            // string ip = comboBoxUser.SelectedItem.ToString();
            //Send a message to the client
            //socketSend.Send(sendMessage);
            foreach(var OneItem in dicSocket)
            {
                dicSocket [OneItem.Key.ToString()].Send(sendMessage);
            }
            
            //Clear the screen
            //textBoxMessage.Text = "";
        }

    }
}
