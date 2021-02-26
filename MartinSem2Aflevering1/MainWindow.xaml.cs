using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
			//dg1.ItemsSource = HandleCall.oc;
		}

		private void ExitClick(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void StartClick(object sender, RoutedEventArgs e)
		{
			foreach (var item in HandleCall.GenerateCalls())
			{
				dg1.Items.Add(new { Number = item.number, Priority = item.priority });					
			}
		}
	}
}
