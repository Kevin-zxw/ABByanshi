using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.ToolBlock;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Cognex.VisionPro3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using AdvancedAppOne;
using System.Runtime.InteropServices;

namespace ABByanshi
{
	public delegate void myShowInfo(string Info); //定义委托
	public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
		}

		//定义区
		private CogAcqFifoTool AcqFifoTool4; //'ABB Camera
		private int myImgComplete4_numAcqs = 0;
		public CogJob myJob;
		//CogToolBlock myToolBlock;
        //CogToolBlock myRunToolblock3; //= myToolBlock.Tools("MyToolBlock3")

		Socket ABBClientSocket;
		byte[] ABBClientReceBufer = new byte[2001];

		string ABBClientReceStr;
		string ABBClientReceStrOut;
		string ABBClientConnectInforStr = "NetDisConnected";

		private const string NetConnected = "NetConnected";
		private const string NetDisConnected = "NetDisConnected";
		private const string NetError = "NetError";
		private const string NetListen = "NetListen";
		private object ABBClientSyncObject = new object();

		string ABBRemoteIPAddress = "169.254.108.179";
		string ABBRemotePort = "1025";

		bool ConnectABBflag = false;
		bool ChangeABBflag = true;

		bool ShowABBflagOK = false;
		bool ShowABBflagNO = false;
		bool PlateCheck = false;

		public bool startMoveFlag;
		public bool finishMoveFlag;

		int TriggerCamNum = 0;



		//ABB
		private void myImgComplete4(object sender, CogCompleteEventArgs e)	//定义ABB的相机
		{
			CogToolBlock myToolBlock;
			CogToolBlock myRunToolblock3; //= myToolBlock.Tools("MyToolBlock3")

			int numReadyVal = 0;
			int numPendingVal = 0;
			bool busyVal = false;

			CogToolGroup myTG = (CogToolGroup)myJob.VisionTool;
			myToolBlock = (CogToolBlock)myTG.Tools["AcquireImageAndProcessLeft"];
			myRunToolblock3 = (CogToolBlock)myToolBlock.Tools["MyToolBlock3"];

			CogAcqInfo info = new CogAcqInfo();
			ICogImage CurrentImage = default(ICogImage);
			// static int numAcqs = 0; 
			try
			{
				AcqFifoTool4.Operator.GetFifoState(out numPendingVal, out numReadyVal, out busyVal);
				if (numReadyVal > 0)
				{
					CurrentImage = (ICogImage)(AcqFifoTool4.Operator.CompleteAcquireEx(info));
				}
				else
				{
					//Throw New CogAcqAbnormalException("Ready count is not greater than 0.")
				}
				myImgComplete4_numAcqs++;

				// We need to run the garbage collector on occasion to cleanup
				// images that are no longer being used.
				if (myImgComplete4_numAcqs > 4)
				{
					GC.Collect();
					myImgComplete4_numAcqs = 0;
				}
				ModuleCard.d1000_out_bit((short)10, (short)1);
				// Issue another acquisition request if we are in manual trigger mode.
				if (!ReferenceEquals(CurrentImage, null))
				{
					myRunToolblock3.Inputs["Input2dImage"].Value = CurrentImage;
					myRunToolblock3.Run();
				}
				// CogRecordDisplay1.Image = CurrentImage
			}
			catch (Exception)
			{

			}
		}


        private void button1_Click(object sender, EventArgs e)				//连接ABB机器人socket
        {
			if (ConnectABBflag == true)
			{
				ShowABBflagOK = true;
				ShowABBflagNO = true;
				if (ABBClientConnectInforStr != NetConnected)
				{
					CommunicationMsg.AppendText(System.Convert.ToString("[" + Strings.Format(DateTime.Now, "hh:mm:ss") + "] Connect to robot ABB ..." + "\r\n"));
					IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(ABBRemoteIPAddress), int.Parse(ABBRemotePort));
					ABBClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					ABBClientSocket.ReceiveBufferSize = 2000;
					ABBClientSocket.SendBufferSize = 2000;
					ABBClientSocket.SendTimeout = 4;
					ABBClientSocket.BeginConnect(remoteEP, new AsyncCallback(ABBClientConnectCallback), ABBClientSocket);
					CommunicationMsg.AppendText("aaaaa1\n");					//字符戳，是有执行到这一步的
					if (ABBClientConnectInforStr != NetConnected)
                    {
						IPEndPoint clientipe = (IPEndPoint)ABBClientSocket.LocalEndPoint;
						CommunicationMsg.AppendText("aaaaa");
                    }
                }
			}
			else
			{
				if (ABBClientSocket != null)
				{
					ABBClientSocket.Close();
					ABBClientSocket.Dispose();
					ConnectABB.ForeColor = Color.Red;
					CommunicationMsg.AppendText(System.Convert.ToString("Disconnect to robot ABB" + "\r\n"));
				}
				ABBClientConnectInforStr = NetDisConnected;
			}
		}

		private void ABBClientConnectCallback(IAsyncResult ar)
		{
			// Retrieve the socket from the state object.
			Socket client = (Socket)ar.AsyncState;
			// Complete the connection.
			// client.EndConnect(ar)
			//Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString())
			// Signal that the connection has been made.
			// connectDone.Set()
			lock (ABBClientSyncObject)
			{
				if (client.Connected == true)
				{
					ABBClientConnectInforStr = NetConnected;
					ABBClientSocket.BeginReceive(ABBClientReceBufer, 0, 1, SocketFlags.None, new AsyncCallback(ABBClientReadCallback), ABBClientSocket);
				}
				else
				{
					ABBClientConnectInforStr = NetDisConnected;
				}
			}
		} //ClientConnectCallback

		private void ABBClientReadCallback(IAsyncResult ar)
		{
			try
			{
				int bytesRead = ABBClientSocket.EndReceive(ar);
				if (bytesRead > 0)
				{
					ABBClientReceStr = ABBClientReceStr + (Encoding.ASCII.GetString(ABBClientReceBufer, 0, bytesRead)); //ABBClientReceStr是接收的字符串
					int StrPos = ABBClientReceStr.IndexOf("\r\n");//读取完才会进入下一个if语句
					//返回第一个回车换行符的位置，ABB程序里都是这样的："WorkpieceArrived" + "\0D\0A"，返回的就是\0D的位置
					if (StrPos > -1)
					{
						ABBClientReceStrOut = ABBClientReceStr.Substring(0, StrPos);
						//MessageBox.Show(ABBClientReceStrOut + '\n');
						//然后再读取从0开始到换行的字符串，这里就是WorkpieceArrived
						myShowInfo mydelete = new myShowInfo(ABBShowInfoSub); //定义委托						
						//MessageBox.Show("hahahahahha");
						this.Invoke(mydelete, ABBClientReceStrOut); //ABBClientReceStrOut应该是作为字符串传入info变量,在这里断掉，是ABBShowInfoSub里面代码的问题
						//MessageBox.Show("卡死了");						

						ABBClientReceStr = ABBClientReceStr.Substring(StrPos + 2, ABBClientReceStr.Length - StrPos - 2); //看不懂，好像没啥用
					}
				}
                else
                {
					MessageBox.Show("byteread<0");
                }
				ABBClientSocket.BeginReceive(ABBClientReceBufer, 0, 1, SocketFlags.None, new AsyncCallback(ABBClientReadCallback), ABBClientSocket);
				//这里又异步调用了ABBClientReadCallback，代码在3993行
			}
			catch (Exception)
			{
				ABBClientConnectInforStr = NetError;
			}
		} //ClientReadCallback

		private void ABBClientSendCallback(IAsyncResult ar)
		{
			//Retrieve the socket from the state object.从状态目标读取socket
			try
			{
				Socket client = (Socket)ar.AsyncState; //把用户定义的对象转换为socket
				if (client.Connected == false)
				{
					ABBClientConnectInforStr = NetError;
				}
				// Complete sending the data to the remote device.
				int bytesSent = client.EndSend(ar);
			}
			catch (Exception)
			{
				ABBClientConnectInforStr = NetError;
			}
			//Console.WriteLine("Sent {0} bytes to server.", bytesSent)

			// Signal that all bytes have been sent.

		} //ClientSendCallback

		string ABB_Result;
		// VBConversions Note: Former VB static variables moved to class level because they aren't supported in C#. //传入一个string，读取到的
		private string ABBShowInfoSub_SaveInfo = "";

		private void ABBShowInfoSub(string Info)
		{
			CogToolBlock myToolBlock;
			CogToolBlock myRunToolblock3; //= myToolBlock.Tools("MyToolBlock3")

			// static string SaveInfo = ""; //VBConversions Note: Static variable moved to class level and renamed ABBShowInfoSub_SaveInfo. Local static variables are not supported in C#.
			if (ABBShowInfoSub_SaveInfo != Info)
			{
				TextBoxInfo.Text = Info + "\r\n" + TextBoxInfo.Text;
			}
			ABBShowInfoSub_SaveInfo = Info;
			if (Info == "WorkpieceArrived")
			{
				CommunicationMsg.AppendText(System.Convert.ToString("[" + Strings.Format(DateTime.Now, "hh:mm:ss") + "] From robot ABB : " + "\r\n"));
				CommunicationMsg.AppendText(System.Convert.ToString("Workpiece arrived target position" + "\r\n"));
			}
			else if (Info == "StartPlateMove")
			{
				finishMoveFlag = false;
				startMoveFlag = true;
				CommunicationMsg.AppendText(System.Convert.ToString("[" + Strings.Format(DateTime.Now, "hh:mm:ss") + "] From robot ABB : " + "\r\n"));
				CommunicationMsg.AppendText(System.Convert.ToString("Start move platform ..." + "\r\n"));
			}//下面开始的代码就会闪退
			else if (Info == "TriggerCam")
			{
				TriggerCamNum++; //本身定义为0，+1代表可以执行拍照了
				CommunicationMsg.AppendText(System.Convert.ToString("[" + Strings.Format(DateTime.Now, "hh:mm:ss") + "] From robot ABB : " + "\r\n"));
				CommunicationMsg.AppendText(System.Convert.ToString("Robot ABB camera trigger " + Conversion.Str(TriggerCamNum) + "\r\n"));
				CogToolGroup myTG = (CogToolGroup)myJob.VisionTool;
				myToolBlock = (CogToolBlock)myTG.Tools["AcquireImageAndProcessLeft"];
				myRunToolblock3 = (CogToolBlock)myToolBlock.Tools["MyToolBlock3"];
				if (TriggerCamNum == 1)
				{
					myRunToolblock3.Inputs["TriggerCamNum"].Value = 0;
				}
				else
				{
					myRunToolblock3.Inputs["TriggerCamNum"].Value = 1;
					TriggerCamNum = 0;
				}
			}

                //	try
                //	{
                //		ModuleCard.d1000_out_bit((short)10, (short)0);
                //		System.Threading.Thread.Sleep(50);
                //		int num = 0;
                //		myRunToolblock3.Inputs["Input2dImage"].Value = AcqFifoTool4.Operator.Acquire(out num); //应该是获取图像
                //		myRunToolblock3.Run(); //这个myruntoolblock3是一个模块，和外部的拍照组件链接（确信）
                //	}
                //	catch (Exception)
                //	{
                //		MessageBox.Show("error");
                //	}
                //}
                //判断定位是否正确
                //ABB_Result = System.Convert.ToString(myRunToolblock3.Outputs["ABB_Result"].Value); //这里是怎么判断的呢？？？？？
                //if (ABB_Result == "OK")
                //{
                //	if (ABBClientConnectInforStr == NetConnected)
                //	{
                //		byte[] byteData = Encoding.ASCII.GetBytes("WorkpieceLocationOK" + "\r\n");
                //		try
                //		{
                //			//Begin sending the data to the remote device.StockIsOK
                //			ABBClientSocket.BeginSend(byteData, 0, byteData.Length, (System.Net.Sockets.SocketFlags)0, new AsyncCallback(ABBClientSendCallback), ABBClientSocket);
                //		}
                //		catch (Exception)
                //		{
                //			ABBClientConnectInforStr = NetError;
                //		}
                //	}
                //}
                //else
                //{
                //	if (ABBClientConnectInforStr == NetConnected)
                //	{
                //		byte[] byteData = Encoding.ASCII.GetBytes("WorkpieceLocationNO" + "\r\n");
                //		try
                //		{
                //			//Begin sending the data to the remote device.StockIsOK
                //			ABBClientSocket.BeginSend(byteData, 0, byteData.Length, (System.Net.Sockets.SocketFlags)0, new AsyncCallback(ABBClientSendCallback), ABBClientSocket);
                //		}
                //		catch (Exception)
                //		{
                //			ABBClientConnectInforStr = NetError;
                //		}
                //	}
                //}
                //}
                //else if (Info == "IsPlateOK")
                //{
                //	PlateCheck = true;
                //}
                //else
                //{
                //	//CommunicationMsg.AppendText(Info + vbCrLf)
                //}
            }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
			if (ChangeABBflag == true)
			{
				ConnectABBflag = true;
				ChangeABBflag = false;
			}
			else
			{
				ConnectABBflag = false;
				ChangeABBflag = true;
			}
		}

		char textLast1;
		char textLast2;
		char textLast3;


		private void Test_Click(object sender, EventArgs e)
        {
			if (ABBClientConnectInforStr == NetConnected)
			{
				byte[] byteData = Encoding.ASCII.GetBytes("Helo World" + "\r\n");
				try
				{
					//Begin sending the data to the remote device.StockIsOK
					ABBClientSocket.BeginSend(byteData, 0, byteData.Length, (System.Net.Sockets.SocketFlags)0, new AsyncCallback(ABBClientSendCallback), ABBClientSocket);
				}
				catch (Exception)
				{
					ABBClientConnectInforStr = NetError;
				}
			}
		}

        private void TimerNetMonitor_Tick(object sender, EventArgs e)
        {
			TextBox1.Text = System.Convert.ToString(PlateCheck) + System.Convert.ToString(finishMoveFlag);
			if (CommunicationMsg.Lines.Length > 50)
			{
				textLast1 = CommunicationMsg.Text[49];
				textLast2 = CommunicationMsg.Text[50];
				textLast3 = CommunicationMsg.Text[51];
				CommunicationMsg.Text = "";
				CommunicationMsg.AppendText(textLast1 + "\r\n");
				CommunicationMsg.AppendText(textLast2 + "\r\n");
				CommunicationMsg.AppendText(textLast3 + "\r\n");
			}
			if (ConnectABBflag == true)
			{
				lock (ABBClientSyncObject)
				{
					if (ABBClientConnectInforStr != NetConnected)
					{
						if (ABBClientSocket != null)
						{
							ABBClientSocket.Close();
							ABBClientSocket.Dispose();
						}
						IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(ABBRemoteIPAddress), int.Parse(ABBRemotePort));
						ABBClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
						ABBClientSocket.ReceiveBufferSize = 2000;
						ABBClientSocket.SendBufferSize = 2000;
						ABBClientSocket.SendTimeout = 4;
						ABBClientSocket.BeginConnect(remoteEP, new AsyncCallback(ABBClientConnectCallback), ABBClientSocket);
					}
					else
					{
						byte[] byteData = Encoding.ASCII.GetBytes("OK" + "\r\n");
						try
						{
							//Begin sending the data to the remote device.StockIsOK
							ABBClientSocket.BeginSend(byteData, 0, byteData.Length, (System.Net.Sockets.SocketFlags)0, new AsyncCallback(ABBClientSendCallback), ABBClientSocket);
						}
						catch (Exception)
						{
							ABBClientConnectInforStr = NetError;
						}
					}
				}
				if (PlateCheck == true && finishMoveFlag == true)
				{
					byte[] byteData = Encoding.ASCII.GetBytes("PlateIsOK" + "\r\n");
					try
					{
						//Begin sending the data to the remote device.StockIsOK
						TextBoxInfo.Text = TextBoxInfo.Text + "SendToABBOnce" + "\r\n";
						ABBClientSocket.BeginSend(byteData, 0, byteData.Length, (System.Net.Sockets.SocketFlags)0, new AsyncCallback(ABBClientSendCallback), ABBClientSocket);
					}
					catch (Exception)
					{
						ABBClientConnectInforStr = NetError;
					}
					PlateCheck = false;
					// finishMoveFlag = False
				}
				if (ABBClientConnectInforStr == NetConnected)
				{
					ConnectABB.ForeColor = Color.Green;
					if (ShowABBflagOK == true)
					{
						CommunicationMsg.AppendText(System.Convert.ToString("[" + Strings.Format(DateTime.Now, "hh:mm:ss") + "] Connect to robot ABB successly" + "\r\n"));
						ShowABBflagOK = false;
						ShowABBflagNO = true;
					}
				}
				else
				{
					ConnectABB.ForeColor = Color.Red;
					if (ShowABBflagNO == true)
					{
						CommunicationMsg.AppendText(System.Convert.ToString("[" + Strings.Format(DateTime.Now, "hh:mm:ss") + "] Wait for robot ABB connection" + "\r\n"));
						ShowABBflagNO = false;
						ShowABBflagOK = true;
					}
				}
			}
		}

    }
}