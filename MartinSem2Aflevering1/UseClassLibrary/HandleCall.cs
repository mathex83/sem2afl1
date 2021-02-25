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
		private static Random rand = new Random();
		private SingleList<string> vipList = new SingleList<string>();
		
		private PQueue<string> mainQ = new PQueue<string>(100);
		private PQueue<string> vipQ = new PQueue<string>(100);
		private int maxNumber = 44445;
		private int lowestBloodPriority = 10;

		private void StartUp()
		{
			mainQ.Clear();
			vipQ.Clear();

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
		public void GenerateCalls()
		{
			string a = rand.Next(1, maxNumber).ToString("D5");
			
			if (!mainQ.Contains(a))
			{
				mainQ.Enqueue(a, rand.Next(2, lowestBloodPriority));
			}

			if (vipList.Contains(a))
			{
				MakeVIP(a);
			}
		}

		private void MakeVIP(string s)
		{
			if (!vipQ.Contains(s))
			{
				vipQ.Enqueue(s, 1);
				
			}
		}
	}
}
