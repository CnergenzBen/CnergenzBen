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
                TcpOption.Action = int.Parse(textBox2.Text);
                byte [ ] bytes = TcpOption.StringToByteArray(textBox1.Text.ToString());
                //byte [ ] bytes = Encoding.ASCII.GetBytes(textBox1.Text.ToString());
                Result = t1.TCPSend(bytes);
                Console.WriteLine("Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));
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
            t1.ConnectTcp("192.168.1.1" , "1");
            Console.WriteLine(t1.TCPResult.ErrorMessage + " == " + t1.TCPResult.Result);
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
            TcpOption.Action = int.Parse(textBox2.Text);
            byte [ ] bytes = Encoding.ASCII.GetBytes(textBox1.Text.ToString());
            Result = t1.TCPSendOriginal(bytes);
            Console.WriteLine("Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive " + BitConverter.ToString(Result.ByteDataReceive));
            Thread.Sleep(100);
        }

        private void TEST_Click(object sender , EventArgs e)
        {
            t1.checLED(t1);
            /*
            byte [ ] data = new byte [200];
            data [0] = 64;
            TcpOption.Action = 1;

            for(int i = 1; i < 200; i++)
            {
                data [i] = 0x00;
            }
            for(int i = 0; i < 6; i++)
            {

                data [0] /= 2;
                Result = t1.TCPSend(data);
                Console.WriteLine("Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));

                Thread.Sleep(20);
            }
            data [0] = 1;

            for(int i = 0; i < 256; i++)
            {
                RotateRight(data);
                // byte [ ] bytes = TcpOption.StringToByteArray(data);
                Result = t1.TCPSend(data);
                Console.WriteLine("Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));

                Thread.Sleep(20);
            }*/
        }



        public static void RotateRight(byte [ ] bytes)
        {
            bool carryFlag = ShiftRight(bytes);

            if(carryFlag == true)
            {
                bytes [0] = (byte)( bytes [0] | 0x80 );
            }
        }

        public static bool ShiftRight(byte [ ] bytes)
        {
            bool rightMostCarryFlag = false;
            int rightEnd = bytes.Length - 1;

            // Iterate through the elements of the array right to left.
            for(int index = rightEnd; index >= 0; index--)
            {
                // If the rightmost bit of the current byte is 1 then we have a carry.
                bool carryFlag = ( bytes [index] & 0x01 ) > 0;

                if(index < rightEnd)
                {
                    if(carryFlag == true)
                    {
                        // Apply the carry to the leftmost bit of the current bytes neighbor to the right.
                        bytes [index + 1] = (byte)( bytes [index + 1] | 0x80 );
                    }
                }
                else
                {
                    rightMostCarryFlag = carryFlag;
                }

                bytes [index] = (byte)( bytes [index] >> 1 );
            }

            return rightMostCarryFlag;
        }


        public static bool ShiftLeft(byte [ ] bytes)
        {
            bool leftMostCarryFlag = false;

            // Iterate through the elements of the array from left to right.
            for(int index = 0; index < bytes.Length; index++)
            {
                // If the leftmost bit of the current byte is 1 then we have a carry.
                bool carryFlag = ( bytes [index] & 0x80 ) > 0;

                if(index > 0)
                {
                    if(carryFlag == true)
                    {
                        // Apply the carry to the rightmost bit of the current bytes neighbor to the left.
                        bytes [index - 1] = (byte)( bytes [index - 1] | 0x01 );
                    }
                }
                else
                {
                    leftMostCarryFlag = carryFlag;
                }

                bytes [index] = (byte)( bytes [index] << 1 );
            }

            return leftMostCarryFlag;
        }

        public static void RotateLeft(byte [ ] bytes)
        {
            bool carryFlag = ShiftLeft(bytes);

            if(carryFlag == true)
            {
                bytes [bytes.Length - 1] = (byte)( bytes [bytes.Length - 1] | 0x01 );
            }
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

        public static byte [ ] StringToByteArray(string hex)
        {
            return Enumerable.Range(0 , hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x , 2) , 16))
                             .ToArray();
        }
        public static int Action;
        public byte [ ] buffer = new byte [2048];
        public byte [] bufferRecive;

        private byte [ ] SLE1 = Encoding.ASCII.GetBytes("SLE1");
        private byte [ ] SLE0 = Encoding.ASCII.GetBytes("SLE0");
        private byte [ ] SENR = Encoding.ASCII.GetBytes("SENR");
        private byte [ ] BLN1 = Encoding.ASCII.GetBytes("BLN1");
        private byte [ ] BLN0 = Encoding.ASCII.GetBytes("BLN0");
        private byte [ ] BUZ0 = Encoding.ASCII.GetBytes("BUZ0");
        private byte [ ] BUZ1 = Encoding.ASCII.GetBytes("BUZ1");
        private byte [] Crc;
        private byte [ ] buffer3;
        private Socket socketSend;

        public AllResult TCPResult = new AllResult();

        public void checLED(TcpOption t2 )
        {
           // TcpOption t2 = new TcpOption();
            Tcp.TcpOption.AllResult Result;
            byte [ ] data = new byte [200];
            data [0] = 64;
            TcpOption.Action = 1;

            for(int i = 1; i < 200; i++)
            {
                data [i] = 0x00;
            }
            for(int i = 0; i < 6; i++)
            {
                data [0] /= 2 ;
                Result = t2.TCPSend(data);
                Console.WriteLine("Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));

                Thread.Sleep(20);
            }
            data [0] = 1;

            for(int i = 0; i < 250; i++)
            {
                RotateRight(data);
                // byte [ ] bytes = TcpOption.StringToByteArray(data);
                Result = t2.TCPSend(data);
                Console.WriteLine("Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));

                Thread.Sleep(20);
            }
        }

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

                var result = Tcpclient1.BeginConnect(IP , Convert.ToInt32(Port.Trim()) , null , null);

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

        private void DataProcess(byte [ ] dataSend)
        {
            if(Action == 0)
            {
                SLE0.CopyTo(buffer , 0);
                dataSend.CopyTo(buffer , SLE0.Length);
                Crc = ToModbus(buffer);
                TCPResult.CRC = Crc;
                buffer.CopyTo(buffer3 , 0);
                Crc.CopyTo(buffer3 , dataSend.Length + SLE0.Length);
            }
            else if(Action == 1)
            {
                SLE1.CopyTo(buffer , 0);
                dataSend.CopyTo(buffer , SLE1.Length);
                Crc = ToModbus(buffer);
                TCPResult.CRC = Crc;
                buffer.CopyTo(buffer3 , 0);
                Crc.CopyTo(buffer3 , dataSend.Length + SLE1.Length);

            }
            else if(Action == 2)
            {
                BLN0.CopyTo(buffer , 0);
                dataSend.CopyTo(buffer , BLN0.Length);
                Crc = ToModbus(buffer);
                TCPResult.CRC = Crc;
                buffer.CopyTo(buffer3 , 0);
                Crc.CopyTo(buffer3 , dataSend.Length + BLN0.Length);

            }
            else if(Action == 3)
            {
                BLN1.CopyTo(buffer , 0);
                dataSend.CopyTo(buffer , BLN1.Length);
                Crc = ToModbus(buffer);
                TCPResult.CRC = Crc;
                buffer.CopyTo(buffer3 , 0);
                Crc.CopyTo(buffer3 , dataSend.Length + BLN1.Length);

            }
            else if(Action == 4)
            {
                BUZ0.CopyTo(buffer , 0);
                dataSend.CopyTo(buffer , BUZ0.Length);
                Crc = ToModbus(buffer);
                TCPResult.CRC = Crc;
                buffer.CopyTo(buffer3 , 0);
                Crc.CopyTo(buffer3 , dataSend.Length + BUZ0.Length);

            }
            else if(Action == 5)
            {
                BUZ0.CopyTo(buffer , 0);
                dataSend.CopyTo(buffer , BUZ1.Length);
                Crc = ToModbus(buffer);
                TCPResult.CRC = Crc;
                buffer.CopyTo(buffer3 , 0);
                Crc.CopyTo(buffer3 , dataSend.Length + BUZ1.Length);

            }
        }

        public AllResult TCPSend(byte [ ] dataSend)
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
                    TCPResult.DataSend = Encoding.ASCII.GetString(buffer3 , 0 , buffer3.Length);
                    int receive = socketSend.Send(buffer3);

                }
                catch
                {
                    TCPResult.Result = false;
                    TCPResult.ErrorMessage = "Send Step Failed";
                    return TCPResult;
                }

                try
                {
                    TCPResult.ByteDataReceive = new byte [1024];
                    TCPResult.DataReceive = "";
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

        public AllResult TCPSendOriginal(byte [ ] dataSend)
        {


            try
            {
                try
                {
                    TCPResult.ByteDataSend = dataSend;
                    TCPResult.DataSend = Encoding.ASCII.GetString(dataSend , 0 , dataSend.Length);
                    int receive = socketSend.Send(dataSend);

                }
                catch
                {
                    TCPResult.Result = false;
                    TCPResult.ErrorMessage = "Send Step Failed";
                    return TCPResult;
                }

                try
                {
                    TCPResult.ByteDataReceive = new byte [1024];
                    TCPResult.DataReceive = "";
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
                TCPResult.ErrorMessage = "Close TCP Failed:";
                return TCPResult;
            }
        }

        public byte [ ] ToModbus(byte [ ] byteData)
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

        private static void RotateRight(byte [ ] bytes)
        {
            bool carryFlag = ShiftRight(bytes);

            if(carryFlag == true)
            {
                bytes [0] = (byte)( bytes [0] | 0x80 );
            }
        }

        private static bool ShiftRight(byte [ ] bytes)
        {
            bool rightMostCarryFlag = false;
            int rightEnd = bytes.Length - 1;

            // Iterate through the elements of the array right to left.
            for(int index = rightEnd; index >= 0; index--)
            {
                // If the rightmost bit of the current byte is 1 then we have a carry.
                bool carryFlag = ( bytes [index] & 0x01 ) > 0;

                if(index < rightEnd)
                {
                    if(carryFlag == true)
                    {
                        // Apply the carry to the leftmost bit of the current bytes neighbor to the right.
                        bytes [index + 1] = (byte)( bytes [index + 1] | 0x80 );
                    }
                }
                else
                {
                    rightMostCarryFlag = carryFlag;
                }

                bytes [index] = (byte)( bytes [index] >> 1 );
            }

            return rightMostCarryFlag;
        }


        private static bool ShiftLeft(byte [ ] bytes)
        {
            bool leftMostCarryFlag = false;

            // Iterate through the elements of the array from left to right.
            for(int index = 0; index < bytes.Length; index++)
            {
                // If the leftmost bit of the current byte is 1 then we have a carry.
                bool carryFlag = ( bytes [index] & 0x80 ) > 0;

                if(index > 0)
                {
                    if(carryFlag == true)
                    {
                        // Apply the carry to the rightmost bit of the current bytes neighbor to the left.
                        bytes [index - 1] = (byte)( bytes [index - 1] | 0x01 );
                    }
                }
                else
                {
                    leftMostCarryFlag = carryFlag;
                }

                bytes [index] = (byte)( bytes [index] << 1 );
            }

            return leftMostCarryFlag;
        }

        private static void RotateLeft(byte [ ] bytes)
        {
            bool carryFlag = ShiftLeft(bytes);

            if(carryFlag == true)
            {
                bytes [bytes.Length - 1] = (byte)( bytes [bytes.Length - 1] | 0x01 );
            }
        }

    }
}
