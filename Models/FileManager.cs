using System.IO;
using System.Reflection;
using System.Windows;
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
		public static bool Save( Mission mission, string sourcePath, string baseOutputFolder, bool onlyExtract )
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
			if ( !onlyExtract )
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
			try
			{
				//strip default language out of the Mission and save it
				TranslatedMission translation = TranslatedMission.CreateTranslation( mission );
				var langID = mission.languageID.Split( '(', ')' )[1];
				string tfname = $"{mission.fileName.Substring( 0, mission.fileName.Length - 5 )}_{langID}.json";
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

				return m;
			}
			catch ( Exception e )
			{
				MessageBox.Show( $"Could not load the Mission.\r\n{filename}\r\nException:\r\n" + e.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
				return null;
			}
		}

		public static Mission LoadMissionFromString( string json )
		{
			//make sure it's a mission, simple check for a property in the text
			if ( !json.Contains( "missionGUID" ) )
				return null;

			try
			{
				var m = JsonConvert.DeserializeObject<Mission>( json );
				//Utils.Log( "LoadMissionFromString: " + m.missionProperties.missionID );
				return m;
			}
			catch ( Exception e )
			{
				MessageBox.Show( "LoadMissionFromString()::Could not load the Mission.\n\nException:\n" + e.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
				return null;
			}
		}

		/// <summary>
		/// "filename" is a full path, returns null on failure
		/// </summary>
		public static ProjectItem CreateProjectItem( string filename )
		{
			ProjectItem projectItem = new ProjectItem();

			if ( !File.Exists( filename ) )
				return null;

			var mission = LoadMissionFromString( File.ReadAllText( filename ) );
			if ( mission != null )
			{
				projectItem.fullPathWithFilename = filename;
				projectItem.fileName = new FileInfo( filename ).Name;
				projectItem.Title = mission.missionProperties.missionName;
				projectItem.Date = mission.saveDate;
				projectItem.fileVersion = mission.fileVersion;
				projectItem.timeTicks = mission.timeTicks;
				projectItem.Description = mission.missionProperties.missionDescription;
			}
			else
				return null;

			return projectItem;

			//ProjectItem projectItem = new();
			//FileInfo fi = new FileInfo( filename );
			//string basePath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "ImperialCommander" );

			//string[] text = File.ReadAllLines( filename );
			//foreach ( var line in text )
			//{
			//	//manually parse each line
			//	string[] split = line.Split( ':' );
			//	if ( split.Length == 2 )
			//	{
			//		projectItem.fileName = fi.Name;
			//		projectItem.relativePath = Path.GetRelativePath( basePath, new DirectoryInfo( filename ).FullName );

			//		split[0] = split[0].Replace( "\"", "" ).Replace( ",", "" ).Trim();
			//		split[1] = split[1].Replace( "\"", "" ).Replace( ",", "" ).Trim();
			//		if ( split[0] == "missionName" )
			//			projectItem.Title = split[1];
			//		if ( split[0] == "saveDate" )
			//			projectItem.Date = split[1];
			//		if ( split[0] == "fileVersion" )
			//			projectItem.fileVersion = split[1];
			//		if ( split[0] == "timeTicks" )
			//			projectItem.timeTicks = long.Parse( split[1] );
			//	}
			//	else if ( split.Length > 2 )//mission name with a colon
			//	{
			//		for ( int i = 0; i < split.Length; i++ )
			//			split[i] = split[i].Replace( "\"", "" ).Replace( ",", "" ).Trim();
			//		if ( split[0] == "missionName" )
			//		{
			//			int idx = line.IndexOf( ':' );
			//			int c = line.LastIndexOf( ',' );
			//			string mname = line.Substring( idx + 1, c - idx - 1 ).Replace( "\"", "" ).Trim();
			//			projectItem.Title = mname;
			//		}
			//	}
			//}
			//return projectItem;
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
