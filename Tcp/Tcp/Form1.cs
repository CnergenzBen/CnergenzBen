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
using System.Collections;

namespace Tcp
{


    public partial class Form1:Form
    {
        Tcp.TcpOption t1 = new TcpOption();
        Tcp.TcpOption t2 = new TcpOption();
        Tcp.TcpOption.AllResult Result;
        private static object MainObjlock1 = new object();


        Task task;
        Task task1;
        Task task2;
        int check = 0;
        bool onoff;

        //声明
        public Form1( )
        {
            InitializeComponent();
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns [0].Name = "NO";
            dataGridView1.Columns [1].Name = "Signal";
            dataGridView1.Columns [2].Name = "Now Signal";
            for(int i = 0; i < 64; i++)
            {
                dataGridView1.Rows.Add(i + 1 , "false" , "false");
            }
            t1.ConnectTcp("192.168.3.155" , "1");

            if(t1.TCPResult.Result == true)
            {
                timer1.Interval = 500;
                timer1.Enabled = true;
                timer2.Interval = 50;
                timer2.Enabled = true;

            }


        }



        private void button1_Click(object sender , EventArgs e)
        {

            t1.Action = int.Parse(textBox2.Text);
            t1.ByteLedQty100.InsetOfftime(Convert.ToInt32(timetext.Text.ToString()));

            Result = t1.TCPSend(t1.ByteLedQty100.ByteDataSendArray100);
            Console.WriteLine("StealCheck=" + Result.StealChack + "//Error Bool=" + Result.Result + " //Error = " + Result.ErrorMessage);
            Console.WriteLine(" data Send = " + Result.DataSend + "Bytesend = " + BitConverter.ToString(Result.ByteDataSend));
            Console.WriteLine(" DataRecive= " + Result.DataReceive + "Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));
            Thread.Sleep(100);
        }

        private void button2_Click(object sender , EventArgs e)
        {
            t1.TcpClose();
            Console.WriteLine(t1.TCPResult.Result);

        }

        private void button3_Click(object sender , EventArgs e)
        {
            t1.ConnectTcp("192.168.1.155" , "1");
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
            Task test1 = t1.checLED(t1);
            test1.ConfigureAwait(false);

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
            richTextBox1.Clear();

            richTextBox1.Text = "StealCheck=" + Result.StealChack + "//Error Bool=" + Result.Result + " //Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive);
            Console.WriteLine("CRCREsult"+Result.CRCResult);
            Console.WriteLine("StealCheck=" + Result.StealChack + "//Error Bool=" + Result.Result + " //Error = " + Result.ErrorMessage);
            Console.WriteLine(" data Send = " + Result.DataSend + "Bytesend = " + BitConverter.ToString(Result.ByteDataSend));
            Console.WriteLine(" DataRecive= " + Result.DataReceive + "Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));
            for(int i = 0; i < 64; i++)
            {
                Console.Write(t1.StealHappenRecordArray [i]);
                richTextBox1.Text += t1.StealHappenRecordArray [i];


            }
            Console.Write("@@");
            Thread.Sleep(100);
        }

        private void timer1_Tick(object sender , EventArgs e)
        {
            byte [ ] empty = new byte [100];

            label3.Text = t1.TCPResult.StealChack.ToString();

            if(check == 0)
            {
                check = 1;
                task = t1.checkTopButtonLed();


                task1 = Task.Run(( ) =>
                {

                    t1.Action = 2;
                    Result = t1.TCPSend(empty);
                } , t1.source.Token);
                Thread.Sleep(1000);
                return;
            }


            if(task1.Status != TaskStatus.Running)
            {
                task1 = Task.Run(( ) =>
                {
                    t1.Action = 2;
                    Result = t1.TCPSend(empty);
                } , t1.source.Token);
                Console.WriteLine("progress");
            }


        }

        private void tt_Click(object sender , EventArgs e)
        {
            Task checkUpDown = t1.checkTopButtonLed();
            checkUpDown.ConfigureAwait(false);
        }

        private void button7_Click_1(object sender , EventArgs e)
        {
            for(int i = Convert.ToInt32(on1.Text); i < Convert.ToInt32(on2.Text); i++)
            {
                t1.ByteLedQty100.InsertLocationLedBit(i);
            }

        }

        private void timer2_Tick(object sender , EventArgs e)
        {


            for(int i = 0; i < 64; i++)
            {
                DataGridViewRow newGrid = dataGridView1.Rows [i];
                newGrid.Cells [0].Value = i + 1;
                if(t1.SensorLocation [i] == 1)
                {
                    newGrid.Cells [1].Value = true;
                    newGrid.Cells [1].Style.BackColor = Color.Green;
                }

                if(t1.RefreshSensorLocation [i] == 1)
                {
                    newGrid.Cells [2].Value = true;
                    newGrid.Cells [2].Style.BackColor = Color.Green;
                }
                else
                {
                    newGrid.Cells [2].Value = false;
                    newGrid.Cells [2].Style.BackColor = Color.Empty;
                }
            }



        }

        private void button9_Click(object sender , EventArgs e)
        {
            if(onoff == true)
            {
                onoff = false;
            }
            else
            {
                onoff = true;
            }

            if(onoff == true)
            {
                t1.source = new CancellationTokenSource();
                timer1.Interval = 100;
                timer1.Enabled = true;
                timer2.Interval = 50;
                timer2.Enabled = true;
                button9.BackColor = Color.Green;

            }
            else
            {
                //task2.Dispose();
                t1.source.Cancel();
                timer1.Interval = 100;
                timer1.Enabled = false;
                timer2.Interval = 50;
                timer2.Enabled = false;
                button9.BackColor = Color.Red;
            }

        }
    }







    public class TcpOption
    {
        #region class declare
        private static object objlock1 = new object();
        private static object objlock2 = new object();
        public CancellationTokenSource source = new CancellationTokenSource();
        public class data
        {
            private byte[] timeByte = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };
            public byte[] ByteDataSendArray100 = new byte[100];
            public byte dataAdd;

            public void InsetOfftime(int time)
            {
                ByteDataSendArray100 = new byte [100];
                ByteDataSendArray100 [2] = timeByte [time % 10];
                ByteDataSendArray100 [1] = timeByte [( time / 10 ) % 10];
                ByteDataSendArray100 [0] = timeByte [( time / 100 ) % 10];

            }
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
            public int StealChack;
            public String DataReceive;
            public byte[] ByteDataReceive = new byte[2048];
            public String DataSend;
            public byte[] ByteDataSend = new byte[2048];
            public string ErrorMessage = "null";
            public string SendErrorMessage = "success send";
            public string ReviceErrorMessage = "success recive";
            public bool Result = true;//true = success false = failed
            public Byte[] CRC = new byte[10];//checksum
            public bool  CRCResult;
        }
        #endregion

        #region parameter

        /// <summary>
        /// <param name="Action"> choose what action for PCB board</param>
        /// </summary>
        public int Action;
        public byte[] buffer = new byte[256];
        public BitArray Sensor = new BitArray(new byte[256]);
        public BitArray StealHappenRecord = new BitArray(new byte[256]);
        public int[] SensorLocation = new int[500];
        public int[] StealHappenRecordArray = new int[500];
        public int[] RefreshSensorLocation = new int[500];
        private byte[] Crc;
        private byte[] buffer3;
        private Socket socketSend;
        public AllResult TCPResult = new AllResult();
        public data ByteLedQty100 = new data();
        public int lockAction;
        /// <summary>
        /// <param name="ListDataBytes"> before use need add how many byte u need 1st</para>
        /// <param name="ListDataBytes.insertdata">this func is </param>
        /// </summary>
        //public static List<data> ListDataBytes = new List<data>();

        #endregion

        #region Action parameter

        private string[] ActionStr = new string[]{  "SLE1",
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
                                                      "B111",
                                                      "OFTO",
                                                      "PARA",
                                                      "ACH0",
                                                      "ACH1",
                                                      "RST1",
                                                      "STEN",
                                                      "RERR",
                                                      "TERR",
                                                      "FIL0",
                                                      "FIL1",
                                                      "REM0",
                                                      "REM1"


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

        public async Task<bool> checLED(TcpOption t2)
        {
            await Task.Run(( ) =>
            {
                for(int i = 0; i < 256; i++)
                {
                    if(t2.source.IsCancellationRequested == true)
                    {
                        break;
                    }
                    t2.ByteLedQty100.InsertLocationLedBit(i);
                    t2.Action = 0;
                    var Result = t2.TCPSend(t2.ByteLedQty100.ByteDataSendArray100);
                    Console.WriteLine("//Error Bool=" + Result.Result + " //Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));
                    //await Task.Delay(50);
                    t2.ByteLedQty100.ByteDataSendArray100Clear();
                    t2.Action = 0;
                    Result = t2.TCPSend(t2.ByteLedQty100.ByteDataSendArray100);

                }
            });
            Console.WriteLine("done");
            return true;
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
            byte [ ] action = Encoding.ASCII.GetBytes(ActionStr [lockAction]);
            action.CopyTo(buffer , 0);
            dataSend.CopyTo(buffer , action.Length);
            Crc = ToModbus(buffer);
            TCPResult.CRC = Crc;
            buffer.CopyTo(buffer3 , 0);
            Crc.CopyTo(buffer3 , dataSend.Length + action.Length);

        }

        private byte [ ] CalculateDataReturnCRC(byte [ ] dataRev)
        {
            return Crc = ToModbus(dataRev);
        }

        public async Task checkTopButtonLed( )
        {
            await Task.Run(( ) =>
            {
                for(int i = 0; i < 7; i++)
                {
                    if(source.IsCancellationRequested == true)
                    {
                        break;
                    }
                    for(int k = 0; k < 3; k++)
                    {
                        Action = i + 10;
                        //byte [ ] bytes = t1.StringToByteArray(textBox1.Text.ToString());
                        TCPSend(ByteLedQty100.ByteDataSendArray100);

                        Action = i + 18;
                        TCPSend(ByteLedQty100.ByteDataSendArray100);

                    }
                }
            } , source.Token);
        }
        /// <summary>
        /// this func is send data whit Action select and already add checksum bytes[]
        /// </summary>
        /// <param name="dataSend"></param>
        /// <returns></returns>
        public AllResult TCPSend(byte [ ] dataSend)
        {
            
            byte [ ] TempCRCDataReciveFormCalture = new byte [2];

            lock(objlock1)
            {
                byte [ ] lockSendData = dataSend;
                TCPResult = new AllResult();
                buffer = new byte [dataSend.Length + 4];
                buffer3 = new byte [dataSend.Length + 6];
                Crc = new byte [2];
                lockAction = this.Action;

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

                        //buffer3=Encoding.ASCII.GetBytes("SENR");
                        var r = Task.Run(( ) => socketSend.Send(buffer3));
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
                            return TCPResult;
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
                    /////////start REcive Data
                    try
                    {

                        TCPResult.ByteDataReceive = new byte [1024];
                        TCPResult.DataReceive = "";
                        var r = Task.Run(( ) => socketSend.Receive(TCPResult.ByteDataReceive));
                        if(r.Wait(2000))
                        {
                            TCPResult.DataReceive = Encoding.ASCII.GetString(TCPResult.ByteDataReceive , 0 , r.Result);
                            if(TCPResult.DataReceive.IndexOf("ERRO") != -1)//have found ERRO character
                            {
                                TCPResult.Result = false;
                                TCPResult.SendErrorMessage = "Failed recive because command wrong get ERRO message return";
                                TCPResult.ErrorMessage = "Failed recive because command wrong";
                                return TCPResult;
                            }
                            else if(TCPResult.DataReceive.IndexOf("GOOD") != -1)//have found GOOD character 
                            {
                                TCPResult.Result = true;
                                TCPResult.SendErrorMessage = "Recive success";
                                TCPResult.ErrorMessage = "Recive GOOD";
                            }
                        }
                        else
                        {

                            TCPResult.Result = false;
                            TCPResult.SendErrorMessage = "Failed recive Because wait over 2 sec Please check connection";
                            TCPResult.ErrorMessage = "time over then 2 sec send failed please check connettion";
                            return TCPResult;
                            // timeout logic
                        }


                        // Console.WriteLine("123");

                        if(lockAction == 2)    //SENR read sensor
                        {
                            Sensor = new BitArray(TCPResult.ByteDataReceive);
                            TCPResult.StealChack = TCPResult.ByteDataReceive [17];
                            RefreshSensorLocation = new int [1000];
                            for(int i = 0; i < Sensor.Length; i++)
                            {
                                // Console.WriteLine("SensorResult"+Sensor[i]);
                                if(Sensor [i] == true)
                                {
                                    SensorLocation [i] = 1;
                                    RefreshSensorLocation [i] = 1;
                                }
                            }
                        }
                        if(lockAction == 31)//rerr read error location
                        {
                            StealHappenRecord = new BitArray(TCPResult.ByteDataReceive);
                            StealHappenRecordArray = new int [500];
                            try
                            {
                                for(int i = 0; i < StealHappenRecordArray.Length; i++)
                                {

                                    if(StealHappenRecord [i] == true)
                                    {
                                        StealHappenRecordArray [i] = 1;

                                    }
                                    else
                                    {
                                        StealHappenRecordArray [i] = 0;
                                    }
                                    Console.WriteLine(i + "=" + StealHappenRecord [i].ToString());
                                }
                            }
                            catch
                            {
                                Console.WriteLine("erroe");
                            }
                           

                        }

                       

                        if(lockAction == 36 || lockAction == 34)
                        {
                            do
                            {
                                TCPResult.ByteDataReceive = new byte [1024];
                                TCPResult.DataReceive = "";
                                r = Task.Run(( ) => socketSend.Receive(TCPResult.ByteDataReceive));
                                //r.Wait();
                                TCPResult.DataReceive = Encoding.ASCII.GetString(TCPResult.ByteDataReceive , 0 , r.Result);
                            } while(TCPResult.DataReceive == null);
                        }

                        // task completed within timeout
                        byte [ ] DataWhitOutCRC = new byte [r.Result-2];
                        Array.Copy(TCPResult.ByteDataReceive , DataWhitOutCRC , r.Result - 2);
                        TempCRCDataReciveFormCalture = ToModbusCRCRecive(DataWhitOutCRC);
                        if(TempCRCDataReciveFormCalture[0] == TCPResult.ByteDataReceive[r.Result - 2] && TempCRCDataReciveFormCalture [1] == TCPResult.ByteDataReceive [r.Result - 1])
                        {
                            
                            TCPResult.CRCResult = true;
                        }
                        else
                        {
                            TCPResult.CRCResult = false;
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

        }
        /// <summary>
        /// this func is just send what data come from, mean u need to add the action and checksum by your self
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
            string bitString = Encoding.Default.GetString(byteData);
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

        public byte [ ] ToModbusCRCRecive(byte [ ] byteData)
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
