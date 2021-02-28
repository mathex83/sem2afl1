using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MartinSem2Aflevering1.UseClassLibrary;

namespace MartinSem2Aflevering1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
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
			dg1.Items.Clear();
			dg2.Items.Clear();
			var callers = HandleCall.GenerateCalls();
			foreach (CallerItem caller in callers[0])
				dg1.Items.Add(new { Number = caller.number, Priority = caller.priority });

			foreach (CallerItem vip in callers[1])
				dg2.Items.Add(new { Number = vip.number });

			System.Threading.Thread.Sleep(1000);
		}

		private void RefreshDataGrids(Button btn)
		{
			switch (btn.Name)
			{
				default:
					dg1.Items.Clear();
					dg2.Items.Clear();
					if (HandleCall.normalCallsList.Count > 0)
					{						
						foreach (CallerItem caller in HandleCall.normalCallsList)
							dg1.Items.Add(new { Number = caller.number, Priority = caller.priority });

						foreach (CallerItem vip in HandleCall.vipQ)
							dg2.Items.Add(new { Number = vip.number, Priority = vip.priority });
					}					
					break;
				case "endBtn":
					dg3.Items.Clear();
					if(HandleCall.endedList.Count > 0)
						foreach (EndItem endedCall in HandleCall.endedList)
						{
							dg3.Items.Add(new 
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

		private void TakeCallClick(object sender, RoutedEventArgs e)
		{
			curCall.Text = HandleCall.TakeACall();
			Button btn = sender as Button;			
			RefreshDataGrids(btn);
		}

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
