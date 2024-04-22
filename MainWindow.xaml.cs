using System.IO;
using System.Windows;
using System.Windows.Controls;
using Imperial_Commander_Editor;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace IC2_Mass_Mission_Converter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public Mission mission { get; set; }

		public MissionConverter missionConverter { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			missionConverter = new();
			DataContext = missionConverter;

			Utils.InitColors();
		}

		private void exitButton_Click( object sender, RoutedEventArgs e )
		{
			Close();
		}

		private void removeButton_Click( object sender, RoutedEventArgs e )
		{
			missionConverter.RemoveMission( ((Button)sender).DataContext as string );
			if ( missionConverter.sourceListItems.Count == 0 )
				doConvert.IsEnabled = false;
		}

		private void changeDestBtn_Click( object sender, RoutedEventArgs e )
		{
			CommonOpenFileDialog dlg = new() { IsFolderPicker = true, InitialDirectory = missionConverter.destinationFolder };
			if ( dlg.ShowDialog() == CommonFileDialogResult.Ok )
				missionConverter.destinationFolder = dlg.FileName;
		}

		private void doConvert_Click( object sender, RoutedEventArgs e )
		{
			doConvert.Visibility = Visibility.Collapsed;
			cancelConvert.Visibility = Visibility.Visible;

			missionConverter.DoTranslationWork( assignRandomGUIDCB.IsChecked.Value, () =>
			{
				doConvert.Visibility = Visibility.Visible;
				cancelConvert.Visibility = Visibility.Collapsed;
			} );
		}

		private void loadMissionBtn_Click( object sender, RoutedEventArgs e )
		{
			missionConverter.sourceListItems.Clear();
			CommonOpenFileDialog dialog = new() { InitialDirectory = missionConverter.destinationFolder, Multiselect = true };
			if ( dialog.ShowDialog() == CommonFileDialogResult.Ok )
			{
				var files = dialog.FileNames;
				foreach ( var item in files )
				{
					missionConverter.AddMission( item );
				}
				if ( missionConverter.sourceListItems.Count > 0 )
					doConvert.IsEnabled = true;
			}
		}

		private void cancelConvert_Click( object sender, RoutedEventArgs e )
		{
			doConvert.Visibility = Visibility.Visible;
			cancelConvert.Visibility = Visibility.Collapsed;

			missionConverter.CancelTranslation();
		}

		private void clearListBtn_Click( object sender, RoutedEventArgs e )
		{
			missionConverter.ClearList();
		}

		private void dropContainer_Drop( object sender, DragEventArgs e )
		{
			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
			{
				//grab the filenames
				string[] filename = e.Data.GetData( DataFormats.FileDrop ) as string[];
				foreach ( var file in filename )
				{
					if ( Path.GetExtension( file ) == ".json" )
					{
						missionConverter.AddMission( file );
					}
				}

				if ( missionConverter.sourceListItems.Count > 0 )
					doConvert.IsEnabled = true;
			}
		}

		private void dropContainer_DragEnter( object sender, DragEventArgs e )
		{
			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
			{
				string[] filename = e.Data.GetData( DataFormats.FileDrop ) as string[];
				if ( Path.GetExtension( filename[0] ) == ".json" )
					e.Effects = DragDropEffects.All;
			}
		}
	}
}