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
		bool _isBusy;
		ConvertType _convertType;

		public ObservableCollection<string> sourceListItems { get; set; }
		public string destinationFolder { get => _destinationFolder; set => SetProperty( ref _destinationFolder, value ); }
		public string progressText { get => _progressText; set => SetProperty( ref _progressText, value ); }
		public bool isBusy { get => _isBusy; set => SetProperty( ref _isBusy, value ); }
		public ConvertType convertType { get => _convertType; set => SetProperty( ref _convertType, value ); }

		public MissionConverter()
		{
			destinationFolder = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "ImperialCommander", "TranslatedMissions" );

			if ( !Directory.Exists( destinationFolder ) )
				Directory.CreateDirectory( destinationFolder );

			isBusy = false;
			sourceListItems = new();
			progressText = "Waiting for work";
			convertType = ConvertType.Both;
		}

		public void RemoveMission( string m )
		{
			sourceListItems.Remove( m );
			if ( sourceListItems.Count > 0 )
				progressText = $"Waiting for work ({sourceListItems.Count} items added)";
			else
				progressText = "Waiting for work";
		}

		public void AddMission( string m )
		{
			sourceListItems.Add( m );
			progressText = $"Waiting for work ({sourceListItems.Count} items added)";
		}

		public void ClearList()
		{
			sourceListItems.Clear();
			progressText = "Waiting for work";
		}

		public async void DoTranslationWork( bool assignRandomGUID, Action callBack )
		{
			isBusy = true;
			cancellationToken = new();
			bool errorThrown = false;

			try
			{
				await Task.Run( () =>
				{
					int count = 1;
					int max = sourceListItems.Count;

					foreach ( var missionFileName in sourceListItems )
					{
						cancellationToken.Token.ThrowIfCancellationRequested();
						//Utils.Log( "working..." );
						if ( errorThrown )
							break;

						progressText = $"Processing {count++} of {max}...";
						//load the mission to add any missing properties in the newer format
						var m = FileManager.LoadMission( missionFileName );
						if ( m != null
							&& assignRandomGUID
							&& (convertType == ConvertType.Convert || convertType == ConvertType.Both) )
						{
							m.missionGUID = Guid.NewGuid();
						}

						//save the Mission and translation
						if ( m == null || !FileManager.Save( m, missionFileName, destinationFolder, convertType ) )
						{
							progressText = "Error!";
							errorThrown = true;
							cancellationToken.Cancel();
							Utils.Log( $"Error saving Translation!" );
						}
					}
				}, cancellationToken.Token );

				//work is done
				isBusy = false;
				Utils.Log( "work done!" );
				progressText = "Finished!";
				if ( assignRandomGUID && (convertType == ConvertType.Convert || convertType == ConvertType.Both) )
					progressText = "Finished!  A new 'missionGUID' property was also assigned.";
				callBack.Invoke();
			}
			catch ( Exception )
			{
				progressText = "Error!";
				isBusy = false;
				errorThrown = true;
				cancellationToken.Cancel();
				Utils.Log( "CANCELED" );
				callBack.Invoke();
			}
		}

		public void CancelTranslation()
		{
			isBusy = false;
			cancellationToken.Cancel();
		}
	}
}
