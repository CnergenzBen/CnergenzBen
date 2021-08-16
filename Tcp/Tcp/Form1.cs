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

            t1.Action = int.Parse(textBox2.Text);
            byte [ ] bytes = t1.StringToByteArray(textBox1.Text.ToString());
            Result = t1.TCPSend(bytes);
            Console.WriteLine("Error bool="+ Result.Result + "//Error = " + Result.ErrorMessage + "  //send = " + Result.SendErrorMessage + " //data Send = " + Result.DataSend +"  //Data Recive Error Message = "+ Result.ReviceErrorMessage + " //Data Recive Result = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));
            Thread.Sleep(100);
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
            t1.Action = int.Parse(textBox2.Text);
            byte [ ] bytes = Encoding.ASCII.GetBytes(textBox1.Text.ToString());
            Result = t1.TCPSendOriginal(bytes);
            Console.WriteLine("Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive " + BitConverter.ToString(Result.ByteDataReceive));
            Thread.Sleep(100);
        }

        private void TEST_Click(object sender , EventArgs e)
        {
            t1.checLED(t1);
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

        private void button8_Click(object sender , EventArgs e)
        {
            t1.ByteLedQty100.InsertLocationLedBit(Convert.ToInt32(LedLocation.Text));
        }

        private void button4_Click_1(object sender , EventArgs e)
        {
            t1.ByteLedQty100.DataShow();
        }

        private void button5_Click_1(object sender , EventArgs e)
        {
            t1.ByteLedQty100.ByteDataSendArray100Clear();
        }

        private void button6_Click_1(object sender , EventArgs e)
        {
            t1.Action = int.Parse(textBox2.Text);
            //byte [ ] bytes = t1.StringToByteArray(textBox1.Text.ToString());
            Result = t1.TCPSend(t1.ByteLedQty100.ByteDataSendArray100);
            Console.WriteLine("//Error Bool="+Result.Result +" //Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));
            Thread.Sleep(100);
        }
    }







    public class TcpOption
    {
        #region class declare
        public class data
        {
            public byte[] ByteDataSendArray100 = new byte[100];
            public byte dataAdd;
            private void InsertData(int LedLocation , int data)
            {

                switch(data)
                {
                    case 0:
                        dataAdd = 0x01;
                        break;
                    case 1:
                        dataAdd = 0x02;
                        break;
                    case 2:
                        dataAdd = 0x04;
                        break;
                    case 3:
                        dataAdd = 0x08;
                        break;
                    case 4:
                        dataAdd = 0x10;
                        break;
                    case 5:
                        dataAdd = 0x20;
                        break;
                    case 6:
                        dataAdd = 0x40;
                        break;
                    case 7:
                        dataAdd = 0x80;
                        break;

                }

                ByteDataSendArray100 [LedLocation] = (byte)( ByteDataSendArray100 [LedLocation] | dataAdd );
            }
            public void InsertLocationLedBit(int LedLocation)
            {
                LedLocation -= 1;
                this.InsertData(LedLocation / 8 , LedLocation % 8);
            }
            public void ByteDataSendArray100Clear( )
            {
                ByteDataSendArray100 = new byte [100];
            }
            public StringBuilder DataShow( )
            {
                StringBuilder sb = new StringBuilder(ByteDataSendArray100.Length * 3);
                for(int i = 0; i < this.ByteDataSendArray100.Length; i++)
                {
                    Console.WriteLine(ByteDataSendArray100 [i]);
                    sb.Append(Convert.ToString(ByteDataSendArray100 [i] , 16).PadLeft(2 , '0').PadRight(3 , ' '));
                }
                Console.WriteLine(sb.ToString().ToUpper());
                return sb;
            }

        }
        public class AllResult
        {
            public String DataReceive;
            public byte[] ByteDataReceive = new byte[2048];
            public String DataSend;
            public byte[] ByteDataSend = new byte[2048];
            public string ErrorMessage= "null" ;
            public string SendErrorMessage = "success send";
            public string ReviceErrorMessage = "success recive";
            public bool Result = true;//true = success false = failed
            public Byte[] CRC = new byte[10];//checksum
        }
        #endregion

        #region parameter

        /// <summary>
        /// <param name="Action"> choose what action for PCB board</param>
        /// </summary>
        public int Action;
        public byte [ ] buffer = new byte [2048];
        private byte [] Crc;
        private byte [ ] buffer3;
        private Socket socketSend;
        public AllResult TCPResult = new AllResult();
        public data ByteLedQty100 = new data();
        /// <summary>
        /// <param name="ListDataBytes"> before use need add how many byte u need 1st</para>
        /// <param name="ListDataBytes.insertdata">this func is </param>
        /// </summary>
        //public static List<data> ListDataBytes = new List<data>();

        #endregion

        #region Action parameter
       /* private byte [ ] SLE1 = Encoding.ASCII.GetBytes("SLE1");
        private byte [ ] SLE0 = Encoding.ASCII.GetBytes("SLE0");
        private byte [ ] SENR = Encoding.ASCII.GetBytes("SENR");
        private byte [ ] BLN1 = Encoding.ASCII.GetBytes("BLN1");
        private byte [ ] BLN0 = Encoding.ASCII.GetBytes("BLN0");
        private byte [ ] BUZ0 = Encoding.ASCII.GetBytes("BUZ0");
        private byte [ ] BUZ1 = Encoding.ASCII.GetBytes("BUZ1");
        private byte [ ] BYP0 = Encoding.ASCII.GetBytes("BYP0");
        private byte [ ] BYP1 = Encoding.ASCII.GetBytes("BYP1");


        private byte [ ] T000 = Encoding.ASCII.GetBytes("T000");
        private byte [ ] T001 = Encoding.ASCII.GetBytes("T001");
        private byte [ ] T010 = Encoding.ASCII.GetBytes("T010");
        private byte [ ] T011 = Encoding.ASCII.GetBytes("T011");
        private byte [ ] T100 = Encoding.ASCII.GetBytes("T100");
        private byte [ ] T101 = Encoding.ASCII.GetBytes("T101");
        private byte [ ] T110 = Encoding.ASCII.GetBytes("T110");
        private byte [ ] T111 = Encoding.ASCII.GetBytes("T111");

        private byte [ ] B000 = Encoding.ASCII.GetBytes("B000");
        private byte [ ] B001 = Encoding.ASCII.GetBytes("B001");
        private byte [ ] B010 = Encoding.ASCII.GetBytes("B010");
        private byte [ ] B011 = Encoding.ASCII.GetBytes("B011");
        private byte [ ] B100 = Encoding.ASCII.GetBytes("B100");
        private byte [ ] B101 = Encoding.ASCII.GetBytes("B101");
        private byte [ ] B110 = Encoding.ASCII.GetBytes("B110");
        private byte [ ] B111 = Encoding.ASCII.GetBytes("B111");*/

        private  string [] ActionStr = new string[]{  "SLE1",
                                                      "SLE0",
                                                      "SENR",
                                                      "BLN1",
                                                      "BLN0",
                                                      "BUZ0",
                                                      "BUZ1",
                                                      "BYP0",
                                                      "BYP1",   
                                                      "T000",
                                                      "T001",
                                                      "T010",
                                                      "T011",
                                                      "T100",
                                                      "T101",
                                                      "T110",
                                                      "T111",
                                                      "B000",
                                                      "B001",
                                                      "B010",
                                                      "B011",
                                                      "B100",
                                                      "B101",
                                                      "B110",
                                                      "B111"
        };

        #endregion

        #region func
        public byte [ ] StringToByteArray(string hex)
        {
            return Enumerable.Range(0 , hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x , 2) , 16))
                             .ToArray();
        }

        public void checLED(TcpOption t2)
        {
            // TcpOption t2 = new TcpOption();
            Tcp.TcpOption.AllResult Result;
            byte [ ] data = new byte [200];
            data [0] = 64;
            this.Action = 0;

            for(int i = 1; i < 200; i++)
            {
                data [i] = 0x00;
            }
            for(int i = 0; i < 6; i++)
            {
                data [0] /= 2;
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

        /// <summary>
        /// this function is checking tcp connnection status
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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

                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                if(!success)
                {
                    TCPResult.ErrorMessage = "connection failed";
                    throw new Exception("Failed to connect.");
                }

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

        /// <summary>
        /// process Action instrution string
        /// </summary>
        /// <param name="dataSend">data for send to board</param>
        private void AddActionAndCheckSum(byte [ ] dataSend)
        {
             byte[] action = Encoding.ASCII.GetBytes(ActionStr[Action]);
             action.CopyTo(buffer , 0);
             dataSend.CopyTo(buffer , action.Length);
             Crc = ToModbus(buffer);
             TCPResult.CRC = Crc;
             buffer.CopyTo(buffer3 , 0);
             Crc.CopyTo(buffer3 , dataSend.Length + action.Length);
           
        }

        /// <summary>
        /// this func is send data whit Action select and already add checksum bytes[]
        /// </summary>
        /// <param name="dataSend"></param>
        /// <returns></returns>
        public AllResult TCPSend(byte [ ] dataSend)
        {
            TCPResult = new AllResult();
            buffer = new byte [dataSend.Length + 4];
            buffer3 = new byte [dataSend.Length + 6];
            Crc = new byte [2];
            AddActionAndCheckSum(dataSend);
            TCPResult.Result = true;
            if(socketSend == null || !SocketConnected(socketSend))
            {
                TCPResult.ErrorMessage = "connection failed please reconnect";
                TCPResult.Result = false;
                return TCPResult;
            }

            try
            {
                try
                {
                    TCPResult.ByteDataSend = buffer3;
                    TCPResult.DataSend = Encoding.ASCII.GetString(buffer3 , 0 , buffer3.Length);
                    var r = Task.Run(()=> socketSend.Send(buffer3));
                    if(r.Wait(5000))
                    {
                        TCPResult.DataReceive = Encoding.ASCII.GetString(TCPResult.ByteDataReceive , 0 , r.Result);
                        // task completed within timeout
                    }
                    else
                    {
                        
                        TCPResult.Result = false;
                        TCPResult.SendErrorMessage = "Failed send Because wait over 2 sec Please check connection";
                        TCPResult.ErrorMessage = "time over then 2 sec send failed please check connettion";
                        // timeout logic
                    }
                    //int receive = socketSend.Send(buffer3);

                }
                catch(Exception ex)
                {
                    TCPResult.Result = false;
                    TCPResult.ErrorMessage = "Send Step Failed";
                    return TCPResult;
                }

                try
                {
                    TCPResult.ByteDataReceive = new byte [1024];
                    TCPResult.DataReceive = "";
                    var r =  Task.Run(( ) =>  socketSend.Receive(TCPResult.ByteDataReceive));
                    if(r.Wait(5000))
                    {
                        TCPResult.DataReceive = Encoding.ASCII.GetString(TCPResult.ByteDataReceive , 0 , r.Result);
                        // task completed within timeout
                    }
                    else
                    {
                        TCPResult.ReviceErrorMessage = "time over 2 sec recive failed Please check connection";
                        TCPResult.ErrorMessage = "time over 2 sec Recive failed";
                        TCPResult.Result = false;
                        return TCPResult;
                        // timeout logic
                    }
                }
                catch
                {
                    TCPResult.Result = false;
                    TCPResult.ErrorMessage = "Receive Step Failed";
                    return TCPResult;
                }
                TCPResult.ErrorMessage = "Receive Success";
                return TCPResult;
            }
            catch
            {
                TCPResult.Result = false;
                return TCPResult;
                // MessageBox.Show("发送消息出错:" + ex.Message);
            }
        }
        /// <summary>
        /// this func is just send what data com from, mean u need to add the action and checksum by your self
        /// </summary>
        /// <param name="dataSend"></param>
        /// <returns></returns>
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
            socketSend = null;
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
        /// <summary>
        /// generate checksum by byte data, must include the action byte ,is not will error at board
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
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

        #region rotate funtion
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
        #endregion
        #endregion

    }
}
