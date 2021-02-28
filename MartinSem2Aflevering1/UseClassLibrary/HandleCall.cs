using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace MartinSem2Aflevering1.UseClassLibrary
{
	class HandleCall
	{
		private const int maxNumber = 44445;	//just to have 44444 included :-)
		private const int lowestBloodPriority = 6;

		public static MainWindow mw = new MainWindow();
		private static Random rand = new Random();
		private static List<CallerItem> incomingList = new List<CallerItem>();
		private static List<string> listOfVips = new List<string>()
			{
				"11111","22222","33333","44444","12345","01234",
				"23456","34567","10000","20000","30000","40000"
			};		
		private static PQueue<string> q = new PQueue<string>(lowestBloodPriority);    //The priority-queue contains incoming calls by priority.
		public static List<CallerItem> vipQ = new List<CallerItem>();  //If a caller is VIP it's added to this instead.
		private static List<EndItem> ongoingList = new List<EndItem>();	//This call is in progress.
		public static List<EndItem> endedList = new List<EndItem>(); //Represents the items that has been handled.
		public static List<CallerItem> normalCallsList;		

		public static List<List<CallerItem>> GenerateCalls()
		{
			string caller="";
			List<List<CallerItem>> output;
			
			//Adds 9 callers to normalQ and 1 to vipQ.
			for (int i = 0; i < 10; i++)
			{
				if (i == 7)
				{
					/*Every 7th call is always a VIP call. Picks one from start of vipList
					and adds it to the back of list and returns it to caller*/
					CallerItem callerItem = new CallerItem { number = listOfVips[0], priority = 0 };

					//foreach(var x in vipQ) 
						//if(x.number == callerItem.number)
							//return true;
					if (!vipQ.Exists(x => x.number == callerItem.number))
					{
						vipQ.Add(callerItem);
						listOfVips.Add(callerItem.number);
						listOfVips.RemoveAt(0);
					}										
				}
				else
				{
					//The other calls are random.
					caller = rand.Next(1, maxNumber).ToString("D5");

					//A call can't exist twice in q and randomly added calls as vip is not possible here.					
					if (q.Contains(caller))
						i--;
					else if (listOfVips.Contains(caller))
						continue;
					else
						q.Enqueue(caller, rand.Next(1, lowestBloodPriority));
				}
			}			
			
			normalCallsList = new List<CallerItem>();

			//Sends all non-vip-calls to normalCallsList.
			for (int pNormal = 1; pNormal < q.Priorities; pNormal++)
				foreach (string normalElement in q.GetElementsWithPriority(pNormal))
					normalCallsList.Add(new CallerItem { number = normalElement, priority = pNormal });

			output = new List<List<CallerItem>>() { normalCallsList, vipQ };

			return output;
		}

		public static string TakeACall()
		{
			string startedCall;
			switch (ongoingList.Count)
			{
				case 0:
					EndItem call = new EndItem();
					if (vipQ.Count > 0)
					{
						call.number = vipQ[0].number;
						call.priority = vipQ[0].priority;
						vipQ.RemoveAt(0);
					}
					else
					{
						call.number = normalCallsList[0].number;
						call.priority = normalCallsList[0].priority;
						normalCallsList.RemoveAt(0);
					}
					
					call.callTaken = $"{DateTime.Now:dd-MM-yy HH:mm:ss}";
					call.callEnded = "";
					ongoingList.Add(call);					
					break;
				default:
					MessageBox.Show("You can only have 1 call ongoing!");
					break;
			}

			startedCall = ongoingList[0].number;
			return startedCall;
		}

		public static void PrioritizeACall(CallerItem element)
		{
			
		}

		public static void EndACall()
		{
			if (ongoingList.Count > 0)
			{
				EndItem call = new EndItem
				{
					number = ongoingList[0].number,
					priority = ongoingList[0].priority,
					callTaken = ongoingList[0].callTaken,
					callEnded = $"{DateTime.Now:dd-MM-yy HH:mm:ss}"
				};
				endedList.Add(call);
				ongoingList.RemoveAt(0);
			}
		}
	}
	

	class CallerItem
	{
		public string number;
		public int priority;
	}

	class EndItem
	{
		public string number;
		public int priority;
		public string callTaken;
		public string callEnded;
	}	
}
