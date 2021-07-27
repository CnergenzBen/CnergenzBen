using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Tcp
{
   

    public partial class Form1:Form
    {
        Tcp.TcpOption t1 = new TcpOption();
        Tcp.TcpOption.AllResult Result;
        //声明
        public Form1( )
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender , EventArgs e)
        {
           for(int i = 0; i < 10; i++)
            {
                 t1.Action = int.Parse(textBox2.Text);
                 byte [ ] bytes = Encoding.ASCII.GetBytes(textBox1.Text.ToString());
                Result = t1.TCPSend(bytes);
                Console.WriteLine("Error = "+Result.ErrorMessage+"  send = " + Result.Result + " data Send = "+ Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive) );
                Thread.Sleep(100);
            }
          
        }

        private void button2_Click(object sender , EventArgs e)
        {
            t1.TcpClose();
            Console.WriteLine(t1.TCPResult.Result);
                        
        }

        private void button3_Click(object sender , EventArgs e)
        {
          t1.ConnectTcp("192.168.1.2" , "2");
           Console.WriteLine(t1.TCPResult.ErrorMessage +" == "+ t1.TCPResult.Result);
        }

        private void button4_Click(object sender , EventArgs e)
        {
            
        }
       
        private void button5_Click(object sender , EventArgs e)
        {
           
            t1.TCPSend(Encoding.ASCII.GetBytes(textBox1.Text.ToString()));
            
        }

        private void button6_Click(object sender , EventArgs e)
        {
            byte [ ] test = Encoding.ASCII.GetBytes("PULL234");
            Console.WriteLine(BitConverter.ToString(test));
            Console.WriteLine(BitConverter.ToString(t1.ToModbus(test)));
        }

        private void Form1_Load(object sender , EventArgs e)
        {

        }

        private void button7_Click(object sender , EventArgs e)
        {
            t1.Action = int.Parse(textBox2.Text);
            byte [ ] bytes = Encoding.ASCII.GetBytes(textBox1.Text.ToString());
            Result = t1.TCPSendOriginal(bytes);
            Console.WriteLine("Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend +" DataResult = " + Result.DataReceive+"  Data byte Receive " + BitConverter.ToString(Result.ByteDataReceive));
            Thread.Sleep(100);
        }
    }



    public class TcpOption
    {

        public class AllResult
        {
            public String DataReceive;
            public byte[] ByteDataReceive = new byte[2048];
            public String DataSend;
            public byte[] ByteDataSend = new byte[2048];
            public string ErrorMessage;
            public bool Result;
            public Byte[] CRC = new byte[10];
        }

        public int Action;
        public byte [ ] buffer = new byte [2048];
        public byte [] bufferRecive;

        private byte [ ] PULL = Encoding.ASCII.GetBytes("PULL");
        private byte [ ] SENR = Encoding.ASCII.GetBytes("SENR");
        private byte [] Crc;
        private byte [ ] buffer3;
        private Socket socketSend;

        public AllResult TCPResult = new AllResult();

        private bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000 , SelectMode.SelectRead);
            bool part2 = ( s.Available == 0 );
            if(part1 && part2)
                return false;
            else
                return true;
        }

        private bool IsSocketConnected(Socket client)
        {
            bool blockingState = client.Blocking;
            try
            {
                byte [ ] tmp = new byte [1];
                client.Blocking = false;
                client.Send(tmp , 0 , 0);
                return false;
            }
            catch(SocketException e)
            {
                // 产生 10035 == WSAEWOULDBLOCK 错误，说明被阻止了，但是还是连接的
                if(e.NativeErrorCode.Equals(10035))
                    return false;
                else
                    return true;
            }
            finally
            {
                client.Blocking = blockingState;    // 恢复状态
            }
        }

        public AllResult ConnectTcp(String IP , string Port)
        {
                 TcpClose();
            try
            {
                TcpClient Tcpclient1 = new TcpClient();

                var result = Tcpclient1.BeginConnect(IP, Convert.ToInt32(Port.Trim()),null,null);

                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                if(!success)
                {
                    throw new Exception("Failed to connect.");
                }

                // we have connected



                socketSend = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(IP.Trim());
                socketSend.Connect(ip , Convert.ToInt32(Port.Trim()));
                TCPResult.Result = true;
                return TCPResult;

            }
            catch
            {
                TCPResult.Result = false;
                TCPResult.ErrorMessage = "Connection TCP Failed";
                return TCPResult;
            }
        }

        private void DataProcess( byte[]dataSend)
        {
            if(Action == 0)
            {
                PULL.CopyTo(buffer , 0);
                dataSend.CopyTo(buffer , PULL.Length);
                Crc = ToModbus(buffer);
                TCPResult.CRC = Crc;
                buffer.CopyTo(buffer3 , 0);
                Crc.CopyTo(buffer3 , dataSend.Length + PULL.Length);
            }
            else if(Action == 1)
            {
                SENR.CopyTo(buffer , 0);
                dataSend.CopyTo(buffer , PULL.Length);
                Crc = ToModbus(buffer);
                TCPResult.CRC = Crc;
                buffer.CopyTo(buffer3 , 0);
                Crc.CopyTo(buffer3 , dataSend.Length + PULL.Length);

            }
        }

        public AllResult TCPSend(byte[] dataSend)
        {
            buffer = new byte [dataSend.Length + 4];
            buffer3 = new byte [dataSend.Length + 6];
            Crc = new byte [2];


            DataProcess(dataSend);

            try
            {
                try
                {
                    TCPResult.ByteDataSend = buffer3;
                    TCPResult.DataSend     = Encoding.ASCII.GetString(buffer3 , 0 , buffer3.Length);
                    int receive            = socketSend.Send(buffer3);

                }
                catch
                {
                    TCPResult.Result        = false;
                    TCPResult.ErrorMessage  = "Send Step Failed";
                    return TCPResult;
                }

                try
                {
                    TCPResult.ByteDataReceive = new byte [1024];
                    TCPResult.DataReceive     = "";
                    int r                     = socketSend.Receive(TCPResult.ByteDataReceive);
                    TCPResult.DataReceive     = Encoding.ASCII.GetString(TCPResult.ByteDataReceive , 0 , r);
                }
                catch
                {
                    TCPResult.Result            = false;
                    TCPResult.ErrorMessage      = "Receive Step Failed";
                    return TCPResult;
                }
                TCPResult.Result = true;
                TCPResult.ErrorMessage = "Receive Success";
                return TCPResult;             

            }
            catch(Exception ex)
            {
                return TCPResult;
                // MessageBox.Show("发送消息出错:" + ex.Message);
            }
        }

        public AllResult TCPSendOriginal(byte [ ] dataSend)
        {


            try
            {
                try
                {
                    TCPResult.ByteDataSend  = dataSend;
                    TCPResult.DataSend      = Encoding.ASCII.GetString(dataSend , 0 , dataSend.Length);
                    int receive             = socketSend.Send(dataSend);

                }
                catch
                {
                    TCPResult.Result        = false;
                    TCPResult.ErrorMessage  = "Send Step Failed";
                    return TCPResult;
                }

                try
                {
                    TCPResult.ByteDataReceive = new byte [1024];
                    TCPResult.DataReceive      = "";
                    int r = socketSend.Receive(TCPResult.ByteDataReceive);
                    TCPResult.DataReceive = Encoding.ASCII.GetString(TCPResult.ByteDataReceive , 0 , r);
                }
                catch
                {
                    TCPResult.Result = false;
                    TCPResult.ErrorMessage = "Receive Step Failed";
                    return TCPResult;
                }
                TCPResult.Result = true;
                TCPResult.ErrorMessage = "Receive Success";
                return TCPResult;

            }
            catch(Exception ex)
            {
                return TCPResult;
                // MessageBox.Show("发送消息出错:" + ex.Message);
            }
        }

        public AllResult TcpClose( )
        {
            try
            {
                if(socketSend != null)
                {
                    if(!IsSocketConnected(socketSend))
                    {
                        if(SocketConnected(socketSend))
                        {

                            socketSend.Shutdown(SocketShutdown.Both);
                            socketSend.Close();

                        }
                    }

                }
                TCPResult.Result = true;
                TCPResult.ErrorMessage = "Sucess Close TCP";
                return TCPResult;
            }
            catch(Exception ex)
            {

                // MessageBox.Show("Close:" + ex.Message);
                 TCPResult.Result = false;
                 TCPResult.ErrorMessage = "Close TCP Failed:" ;
                 return TCPResult;
            }
        }

        public  byte [ ] ToModbus(byte [ ] byteData)
        {
            byte [ ] CRC = new byte [2];

            UInt16 wCrc = 0xFFFF;
            for(int i = 0; i < byteData.Length; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData [i]);
                for(int j = 0; j < 8; j++)
                {
                    if(( wCrc & 0x0001 ) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;//异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }

            CRC [0] = (byte)( ( wCrc & 0xFF00 ) >> 8 );//高位在后
            CRC [1] = (byte)( wCrc & 0x00FF );       //低位在前
            return CRC;

        }

    }
}
