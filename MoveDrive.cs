// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using System.Runtime.InteropServices;
using AdvancedAppOne;

namespace AdvancedAppOne
{
	sealed class ModuleCard
	{
		///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''''''''''''''''DMC1000 函数列表''''''''''''''''''''''''''''''''''''''
		///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

		///'''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''初始化函数''''''''''''''''''''''''''
		///'''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_board_init();
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_board_close();

		///''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''脉冲输出设置函数'''''''''''''''''''
		///''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_set_pls_outmode(short axis, short pls_outmode);

		///''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''速度模式运动函数'''''''''''''''''''
		///''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_start_tv_move(short axis, int StrVel, int MaxVel, double Tacc);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern int d1000_get_speed(short axis);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_change_speed(short axis, int NewVel);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_decel_stop(short axis);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_immediate_stop(short axis);


		///''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''单轴位置模式函数'''''''''''''''''''
		///''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_start_t_move(short axis, int Dist, int StrVel, int MaxVel, double Tacc);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_start_ta_move(short axis, int Pos, int StrVel, int MaxVel, double Tacc);

		///''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''线性插补函数'''''''''''''''''''''''
		///''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_start_t_line(short TotalAxis, short AxisArray, int DistArray, int StrVel, int MaxVel, double Tacc);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_start_ta_line(short TotalAxis, short AxisArray, int PosArray, int StrVel, int MaxVel, double Tacc);


		///''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''回原点函数'''''''''''''''''''''''''
		///''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_home_move(short axis, int StrVel, int MaxVel, double Tacc);

		///''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''运动状态检测'''''''''''''''''''''''
		///''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_check_done(short axis);

		///''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''位置设定和读取'''''''''''''''''''''
		///''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern int d1000_get_command_pos(short axis);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern int d1000_set_command_pos(short axis, int Pos);

		///''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''通用I/O函数''''''''''''''''''''''''
		///''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_out_bit(short BitNo, short BitData);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_in_bit(short BitNo);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_get_outbit(short BitNo);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern void d1000_in_enable(int CardNo, int InputEn);
		///''''''''''''''''''''''''''''''''''''''''''''
		///'''''''''专用I/O接口函数''''''''''''''''''''
		///''''''''''''''''''''''''''''''''''''''''''''
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_set_sd(short axis, short SdMode);
		[DllImport("dmc1380.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern short d1000_get_axis_status(short axis);

		public static short m_UseAxis;

		public static int Y_STRSPD;
		public static int Y_RUNSPD;
		public static double Y_Tacc;
		public static int Y_Dist;
		public static int Y_Dist_;

		public static int X_STRSPD;
		public static int X_RUNSPD;
		public static double X_Tacc;
		public static int X_Dist;
		public static int X_Dist_;

		public static int XPaiZhaoValue;
		public static int YPaiZhaoValue;
		public static int XPaiZhao2Value;

		public static void Y_MoveToOrigin()
		{

			//d1000_start_tv_move(0, 3000, 4000, 1)  '往下移动

			d1000_start_t_move((short)0, 57000, 3000, 4000, 0.1); //往下走直到遇到下限位置

			while (d1000_check_done((short)0) != 3) //遇到限位
			{
				(new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).DoEvents();
			}

			d1000_start_t_move((short)0, -800, 200, 5000, 0.1);

			while (d1000_check_done((short)0) == 0) //正在运行
			{
				(new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).DoEvents();
			}

			d1000_start_tv_move((short)0, 200, 500, 0.1);

			while (d1000_check_done((short)0) != 3) //遇到限位
			{
				(new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).DoEvents();
			}

			d1000_set_command_pos((short)0, 0);


		}

		public static void X_MoveToOrigin()
		{

			d1000_start_tv_move((short)1, 4000, 6000, 1); //往右移动

			while (d1000_check_done((short)1) != 3) //遇到限位
			{
				(new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).DoEvents();
			}

			d1000_start_t_move((short)1, -800, 200, 5000, 0.1);

			while (d1000_check_done((short)1) == 0) //正在运行
			{
				(new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).DoEvents();
			}

			d1000_start_tv_move((short)1, 200, 500, 0.1);

			while (d1000_check_done((short)1) != 3) //遇到限位
			{
				(new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).DoEvents();
			}

			d1000_set_command_pos((short)1, 0);

			//d1000_start_t_move(1, -3000, 200, 5000, 0.1)    '回到拍照点

			//Do While d1000_check_done(1) = 0     '正在运行
			//    My.Application.DoEvents()
			//Loop

			// '''''''''''''''''''''''''''''''''''''''''''
			//d1000_start_t_move(0, dist, m_nStart, m_nSpeed, fAcc)
			///'''''''''''''''''''''''''''''''''''''''''

			//d1000_home_move(0, 500, -2500, 0.1)            '回原点运动初始速度[500]，回原点运动速度[1000]，加速时间[0.1]

			//While d1000_check_done(0) = 0
			//    My.Application.DoEvents()
			//End While

			//d1000_set_command_pos(0, 0)

			//d1000_start_t_move(0, -115000, 500, 7000, 0.1)  '从原点到指定第一个拍照位置

			//While d1000_check_done(0) = 0
			//    My.Application.DoEvents()
			//End While

			//d1000_set_command_pos(0, 0)

		}

		public static void Y_Up()
		{


			d1000_start_t_move((short)0, Y_Dist_, Y_STRSPD, Y_RUNSPD, Y_Tacc); //从原点到指定第一个拍照位置
																			   //d1000_start_t_move(0, -60000, 500, 5000, 0.1)  '从原点到指定第一个拍照位

			while (d1000_check_done((short)0) == 0)
			{
				(new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).DoEvents();
			}


		}

		public static void Y_Down()
		{


			d1000_start_t_move((short)0, Y_Dist, Y_STRSPD, Y_RUNSPD, Y_Tacc); //从原点到指定第一个拍照位置
																			  //d1000_start_t_move(0, 60000, 500, 5000, 0.1)  '从原点到指定第一个拍照位置

			while (d1000_check_done((short)0) == 0)
			{
				(new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).DoEvents();
			}


		}
	}

}
