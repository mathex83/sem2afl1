using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MartinSem2Aflevering1.UseClassLibrary
{
	class HandleCall
	{
		public static MainWindow mw = new MainWindow();
		private static Random rand = new Random();
		private static SingleList<string> vipList = new SingleList<string>();		//represents a list of VIP-callers.
		private SingleList<string> endedList = new SingleList<string>();	//represents the items that has been handled.

		private static PQueue<string> mainQ = new PQueue<string>(10);				//The priority-queue with all the incoming calls.
		private static PQueue<string> vipQ = new PQueue<string>(10);              //If a caller is VIP it's added to this queue aswell.
		
		private const int maxNumber = 44445;
		private const int lowestBloodPriority = 6;
		public static List<DgRow> dgRows;

		public void StartUp()
		{
			mainQ.Clear();
			vipQ.Clear();
			endedList.Clear();
			//mainDT.Columns.Add("Number", typeof(string));
			//mainDT.Columns.Add("Priority", typeof(int));
			//System.Data.DataRow myRow;

			vipList.AddLast("11111");
			vipList.AddLast("22222");
			vipList.AddLast("33333");
			vipList.AddLast("44444");
			vipList.AddLast("12345");
			vipList.AddLast("01234");
			vipList.AddLast("23456");
			vipList.AddLast("34567");
			vipList.AddLast("10000");
			vipList.AddLast("20000");
			vipList.AddLast("30000");
			vipList.AddLast("40000");
						
		}
		public static List<DgRow> GenerateCalls()
		{			
			for (int i = 0; i < 50; i++)
			{
				string a = rand.Next(1, maxNumber).ToString("D5");
				if (!mainQ.Contains(a))
				{
					if (vipList.Contains(a))
						AddVIPCall(a);
					else
						mainQ.Enqueue(a, rand.Next(2, lowestBloodPriority));
				}
			}
			string k = "";
			dgRows = new List<DgRow>();
			for (int i = 0;i < mainQ.Priorities; i++)
			{				
				foreach (string str in mainQ.GetElementsWithPriority(i))
				{
					dgRows.Add( new DgRow { number = str, priority = i } );				
				}				
			}
			return dgRows;
		}

		private static void AddVIPCall(string s)
		{
			if (!vipQ.Contains(s))
			{
				mainQ.Enqueue(s, 1);
				vipQ.Enqueue(s, 1);
			}
		}
	}

	class DgRow
	{
		public string number;
		public int priority;
	}
}
