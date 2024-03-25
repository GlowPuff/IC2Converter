using System.IO;
using System.Reflection;
using System.Windows;
using IC2_Mass_Mission_Converter;
using Newtonsoft.Json;

namespace Imperial_Commander_Editor
{
	public class FileManager
	{
		public FileManager() { }

		/// <summary>
		/// Checks if the base save folder exists (Documents/ImperialCommander) and creates it if not
		/// </summary>
		private static bool CreateBaseDirectory()
		{
			string basePath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "ImperialCommander" );

			if ( !Directory.Exists( basePath ) )
			{
				var di = Directory.CreateDirectory( basePath );
				if ( di == null )
				{
					MessageBox.Show( "Could not create the base project folder.\r\nTried to create: " + basePath, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// saves a mission and its default translation to base project folder
		/// </summary>
		/// <param name="mission">The Mission</param>
		/// <param name="sourcePath">Full path to Mission (path+filename+ext)</param>
		/// <param name="basePath">Full destination folder path</param>
		/// <returns></returns>
		public static bool Save( Mission mission, string sourcePath, string baseOutputFolder, ConvertType convertType )
		{
			FileInfo fi = new( sourcePath );
			mission.fileName = fi.Name;
			mission.fullPathToFile = sourcePath;
			mission.saveDate = DateTime.Now.ToString( "M/d/yyyy" );
			mission.timeTicks = DateTime.Now.Ticks;
			mission.fileVersion = Utils.formatVersion;

			//save the Mission in the same folder as the translation
			string destinationMissionPath = Path.Combine( baseOutputFolder, fi.Name );

			string output = JsonConvert.SerializeObject( mission, Formatting.Indented );
			//save the Mission
			if ( convertType == ConvertType.Both || convertType == ConvertType.Convert )
			{
				try
				{
					using ( var stream = File.CreateText( destinationMissionPath ) )
					{
						stream.Write( output );
					}
				}
				catch ( Exception e )
				{
					MessageBox.Show( "Could not save the Mission.\r\n\r\nException:\r\n" + e.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
					return false;
				}
			}

			//now save the translation
			if ( convertType == ConvertType.Both || convertType == ConvertType.Extract )
			{
				try
				{
					//strip default language out of the Mission and save it
					TranslatedMission translation = TranslatedMission.CreateTranslation( mission );
					string[] langID = ["", "XX"];
					if ( mission.languageID.Contains( "(" ) && mission.languageID.Contains( ")" ) )
						langID = mission.languageID.Split( '(', ')' );
					string tfname = $"{mission.fileName.Substring( 0, mission.fileName.Length - 5 )}_{langID[1]}.json";
					//$"{mission.fullPathToFile.Substring( 0, mission.fullPathToFile.Length - 5 )}_{lang}.json";
					//serialize to json
					output = JsonConvert.SerializeObject( translation, Formatting.Indented );
					//save it
					using ( var stream = File.CreateText( Path.Combine( baseOutputFolder, tfname ) ) )
					{
						stream.Write( output );
					}
				}
				catch ( Exception e )
				{
					MessageBox.Show( "Could not save the Mission's translation.\r\n\r\nException:\r\n" + e.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Loads a mission from its .json.
		/// Supply the FULL PATH with the filename
		/// </summary>
		public static Mission LoadMission( string filename )
		{
			string json = "";
			string basePath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "ImperialCommander" );

			try
			{
				using ( StreamReader sr = new( filename ) )
				{
					json = sr.ReadToEnd();
				}

				//sanity check
				if ( !json.Contains( "missionGUID" ) )
					throw new( "File doesn't appear to be a Mission." );

				var m = JsonConvert.DeserializeObject<Mission>( json );
				if ( m is null )
					throw new Exception( "LoadMission()::JsonConvert.DeserializeObject<Mission>() returned null." );
				//overwrite fileName, relativePath and fileVersion properties so they are up-to-date
				FileInfo fi = new FileInfo( filename );
				m.fileName = fi.Name;
				m.fullPathToFile = fi.FullName;
				m.fileVersion = Utils.formatVersion;

				//convert all local triggers/events to global versions
				int eCount = 0;
				int tCount = 0;
				foreach ( MapSection item in m.mapSections )
				{
					foreach ( var evnt in item.missionEvents )
					{
						if ( evnt.GUID != Guid.Empty )
						{
							m.globalEvents.Add( evnt );
							eCount++;
						}
					}
					foreach ( var trig in item.triggers )
					{
						if ( trig.GUID != Guid.Empty )
						{
							m.globalTriggers.Add( trig );
							tCount++;
						}
					}
					//clear them so they aren't used
					item.missionEvents.Clear();
					item.triggers.Clear();
				}

				Utils.Log( $"LoadMission()::Converted [{eCount}] LOCAL Events and [{tCount}] LOCAL Triggers to GLOBAL in {fi.Name}" );

				return m;
			}
			catch ( Exception e )
			{
				MessageBox.Show( $"Could not load the Mission.\r\n{filename}\r\nException:\r\n" + e.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
				return null;
			}
		}

		/// <summary>
		/// Load a Resource asset embedded in the app
		/// </summary>
		public static T LoadAsset<T>( string assetName )
		{
			try
			{
				string json = "";
				var assembly = Assembly.GetExecutingAssembly();
				var resourceName = assembly.GetManifestResourceNames().Single( str => str.EndsWith( assetName ) );
				using ( Stream stream = assembly.GetManifestResourceStream( resourceName ) )
				using ( StreamReader reader = new StreamReader( stream ) )
				{
					json = reader.ReadToEnd();
				}

				return JsonConvert.DeserializeObject<T>( json );
			}
			catch ( JsonReaderException e )
			{
				Utils.Log( $"FileManager::LoadData() ERROR:\r\nError parsing {assetName}" );
				Utils.Log( e.Message );
				throw new Exception( $"FileManager::LoadData() ERROR:\r\nError parsing {assetName}" );
			}
		}
	}
}
