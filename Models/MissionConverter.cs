using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Imperial_Commander_Editor;

namespace IC2_Mass_Mission_Converter
{
	public class MissionConverter : ObservableObject
	{
		CancellationTokenSource cancellationToken;

		string _destinationFolder, _progressText;
		bool _isBusy, _onlyExtract;

		public ObservableCollection<string> sourceListItems { get; set; }
		public string destinationFolder { get => _destinationFolder; set => SetProperty( ref _destinationFolder, value ); }
		public string progressText { get => _progressText; set => SetProperty( ref _progressText, value ); }
		public bool isBusy { get => _isBusy; set => SetProperty( ref _isBusy, value ); }
		public bool onlyExtract { get => _onlyExtract; set => SetProperty( ref _onlyExtract, value ); }

		public MissionConverter()
		{
			destinationFolder = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "ImperialCommander", "TranslatedMissions" );

			if ( !Directory.Exists( destinationFolder ) )
				Directory.CreateDirectory( destinationFolder );

			isBusy = false;
			onlyExtract = false;
			sourceListItems = new();
			progressText = "Waiting for work.";
		}

		public void RemoveMission( string m )
		{
			sourceListItems.Remove( m );
		}

		public void AddMission( string m )
		{
			sourceListItems.Add( m );
		}

		public void ClearList()
		{
			sourceListItems.Clear();
		}

		public async void DoTranslationWork( Action callBack )
		{
			isBusy = true;
			cancellationToken = new();

			try
			{
				await Task.Run( async () =>
				{
					int count = 1;
					int max = sourceListItems.Count;

					foreach ( var missionFileName in sourceListItems )
					{
						cancellationToken.Token.ThrowIfCancellationRequested();
						//Utils.Log( "working..." );

						progressText = $"Processing {count++} of {max}...";
						//load the mission to add any missing properties in the newer format
						var m = FileManager.LoadMission( missionFileName );

						//save the Mission and translation
						if ( !FileManager.Save( m, missionFileName, destinationFolder, onlyExtract ) )
							Utils.Log( $"Error saving Translation!" );
					}
				}, cancellationToken.Token );

				//work is done
				isBusy = false;
				Utils.Log( "work done!" );
				progressText = "Finished!";
				callBack.Invoke();
			}
			catch ( Exception )
			{
				Utils.Log( "CANCELED" );
			}
		}

		public void CancelTranslation()
		{
			isBusy = false;
			cancellationToken.Cancel();
		}
	}
}
