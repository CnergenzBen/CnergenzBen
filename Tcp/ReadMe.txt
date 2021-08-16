class TcpOption
	{
		class data // data for send
		{
			ByteDataSendArray100 = new byte[100];              // Led location in decimal
            private void InsertLocationLedBit(int LedLocation) // put location Led decimal in doc show
			ByteDataSendArray100Clear( )					  //clear all data ByteDataSendArray100
			public StringBuilder DataShow( )				 //show data now
		}
		
		calss AllResult // data result will be return
		{
			public String DataReceive; 						//show in string recive
            public byte[] ByteDataReceive = new byte[2048]; //show in byte recive 
            public String DataSend;							//show in string send 
            public byte[] ByteDataSend = new byte[2048];    //show in byte send
            public string ErrorMessage= "null" ;			//null is the initial status
            public string SendErrorMessage = "success send";//
            public string ReviceErrorMessage = "success recive";
            public bool Result = true;						//true = success false = failed
            public Byte[] CRC = new byte[10];				//checksum
		}
		
	int	action 1  SLE1
			   2  SLE0
			   3  SENR
			   4  BLN1
			   5  BLN0
			   6  BUZ0
			   7  BUZ1
			   8  BYP0
			   9  BYP1
			   10 T000
			   11 T001
			   12 T010
			   13 T011
			   14 T100
			   15 T101
			   16 T110
			   17 T111
			   18 B000
			   19 B001
			   20 B010
			   21 B011
			   22 B100
			   23 B101
			   24 B110
			   25 B111


	}


for example u declare 

Tcp.TcpOption t1 = new TcpOption();
Tcp.TcpOption.AllResult Result; // this parameter for recive data return

t1.ConnectTcp("192.168.1.1" , "1"); this function for connection
Console.WriteLine(t1.TCPResult.ErrorMessage + " == " + t1.TCPResult.Result); // this is check success or not will be return

t1.Action = int.Parse(textBox2.Text); 										//choose the action before send
t1.ByteLedQty100.InsertLocationLedBit(Convert.ToInt32(LedLocation.Text));   // inssert the Led location u wan follow doc 
Result = t1.TCPSend(t1.ByteLedQty100.ByteDataSendArray100);					// tcp send byte the checksum will be create automatic
Console.WriteLine("//Error Bool="+Result.Result +" //Error = " + Result.ErrorMessage + "  send = " + Result.Result + " data Send = " + Result.DataSend + " DataResult = " + Result.DataReceive + "  Data byte Receive= " + BitConverter.ToString(Result.ByteDataReceive));
Thread.Sleep(100);