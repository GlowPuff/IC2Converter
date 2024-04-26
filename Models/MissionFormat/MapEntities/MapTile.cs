using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Imperial_Commander_Editor
{
	public class MapTile : INotifyPropertyChanged, IMapEntity
	{
		string _tileID, _tileSide, _name;
		Expansion _expansion;
		Guid _mapSectionOwner;

		public Guid GUID { get; set; }
		public string name
		{
			get { return _name; }
			set
			{
				if ( !string.IsNullOrEmpty( value ) )
					_name = value;
				else
					_name = $"{_expansion}{_tileID}{_tileSide}";
				PC();
			}
		}
		public EntityType entityType { get; set; }
		public Vector entityPosition { get; set; }
		public double entityRotation { get; set; }
		public bool hasProperties { get { return false; } }
		public bool hasColor { get { return false; } }

		//tile props
		public string textureName { get; set; }
		public string tileID { get { return _tileID; } set { _tileID = value; PC(); } }
		public string tileSide { get { return _tileSide; } set { _tileSide = value.ToUpper(); SetSide(); PC(); } }
		public Expansion expansion { get { return _expansion; } set { _expansion = value; PC(); } }
		public EntityProperties entityProperties { get; set; }
		public Guid mapSectionOwner { get { return _mapSectionOwner; } set { _mapSectionOwner = value; PC(); } }

		public event PropertyChangedEventHandler PropertyChanged;
		public void PC( [CallerMemberName] string n = "" )
		{
			if ( !string.IsNullOrEmpty( n ) )
				PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( n ) );
		}

		public MapTile() { }

		public MapTile( string id, string exp = "Core", string side = "A" )
		{
			GUID = Guid.NewGuid();
			textureName = $"{exp[0].ToString().ToUpper() + exp.Substring( 1 ).ToLower()}_{id}{side}";
			_tileSide = side;
			_tileID = id;
			_expansion = (Expansion)Enum.Parse( typeof( Expansion ), exp, true );
			_name = $"{_expansion}{_tileID}{_tileSide}";
			entityType = EntityType.Tile;
			entityProperties = new() { name = name };
			//mapSectionOwner = Utils.mainWindow.activeSection.GUID;
		}

		public IMapEntity Duplicate()
		{
			return null;
		}

		public void SetSide()
		{
		}

		public bool Validate()
		{
			return true;
		}
	}
}
