using MartinSem2Aflevering1.UseClassLibrary;
using System.Windows;
using System.Windows.Controls;

namespace MartinSem2Aflevering1
{
	public partial class MainWindow : Window
	{		
		public MainWindow()
		{
			InitializeComponent();
		}

		/*Adds 10 calls to a priorityqueue generated in HandleCall.GenerateCalls()
		and adds them to the incoming calls datagrid.*/
		private void GenerateClick(object sender, RoutedEventArgs e)
		{
			incomingGrid.Items.Clear();
			prioGrid.Items.Clear();
			var callers = HandleCall.GenerateCalls();
			foreach (CallerItem caller in callers[0])
				incomingGrid.Items.Add(new { Number = caller.number, Priority = caller.priority });

			foreach (CallerItem vip in callers[1])
				prioGrid.Items.Add(new { Number = vip.number });

			System.Threading.Thread.Sleep(1000);
		}

		//Refreshes items in the DataGrids by clearing them and then reload items from the Lists.
		private void RefreshDataGrids(Button btn)
		{
			switch (btn.Name)
			{
				default:
					incomingGrid.Items.Clear();
					prioGrid.Items.Clear();
					if (HandleCall.normalCallsList.Count > 0)
					{						
						foreach (CallerItem caller in HandleCall.normalCallsList)
							incomingGrid.Items.Add(new { Number = caller.number, Priority = caller.priority });

						foreach (CallerItem vip in HandleCall.vipQ)
							prioGrid.Items.Add(new { Number = vip.number, Priority = vip.priority });
					}					
					break;
				case "endBtn":
					endGrid.Items.Clear();
					if(HandleCall.endedList.Count > 0)
						foreach (EndItem endedCall in HandleCall.endedList)
						{
							endGrid.Items.Add(new 
							{
								Number = endedCall.number,
								Priority = endedCall.priority,
								CallTaken = endedCall.callTaken,
								CallEnded = endedCall.callEnded
							});
						}
					break;
			}
		}

		//ButtonClick that basically calls HandleCall.TakeACall to pull an item from incomingGrid or prioGrid and set it as the current call.
		private void TakeCallClick(object sender, RoutedEventArgs e)
		{
			curCall.Text = HandleCall.TakeACall();
			Button btn = sender as Button;			
			RefreshDataGrids(btn);
		}

		//ButtonClick that basically calls HandleCall.EndACall to add items from curCall to the endGrid.
		private void EndCallClick(object sender, RoutedEventArgs e)
		{
			curCall.Text = "";
			HandleCall.EndACall();
			Button btn = sender as Button;			
			RefreshDataGrids(btn);
		}

		//Method to close app.
		private void ExitClick(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}		
	}
}
