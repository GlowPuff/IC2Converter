using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Imperial_Commander_Editor
{
	public class SpaceHighlight : INotifyPropertyChanged, IMapEntity
	{
		string _name;
		int _width, _height, _duration;
		Guid _mapSectionOwner;

		//common props
		public Guid GUID { get; set; }
		public string name
		{
			get { return _name; }
			set
			{
				_name = string.IsNullOrEmpty( value ) ? "New Highlight" : value;
				PC();
			}
		}
		public EntityType entityType { get; set; }
		public Vector entityPosition { get; set; }
		public double entityRotation { get; set; }
		public EntityProperties entityProperties { get; set; }
		public Guid mapSectionOwner { get { return _mapSectionOwner; } set { _mapSectionOwner = value; PC(); } }
		public bool hasProperties { get { return true; } }
		public bool hasColor { get { return true; } }

		//highlight props
		public string deploymentColor
		{
			get { return entityProperties.entityColor; }
			set
			{
				entityProperties.entityColor = value;
				PC();
			}
		}
		public int Width { get { return _width; } set { _width = value; PC(); } }
		public int Height { get { return _height; } set { _height = value; PC(); } }
		public int Duration { get { return _duration; } set { _duration = value; PC(); } }

		public void PC( [CallerMemberName] string n = "" )
		{
			if ( !string.IsNullOrEmpty( n ) )
				PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( n ) );
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public SpaceHighlight()
		{
		}

		public SpaceHighlight( Guid ownderGUID )
		{
			GUID = Guid.NewGuid();
			name = "New Highlight";
			entityType = EntityType.Highlight;
			//defaults NOT ACTIVE, unlike other entities
			entityProperties = new() { isActive = false, name = name };
			mapSectionOwner = ownderGUID;

			Width = 1;
			Height = 1;
			Duration = 0;

			deploymentColor = "Green";
		}

		public IMapEntity Duplicate()
		{
			var dupe = new SpaceHighlight();
			dupe.GUID = Guid.NewGuid();
			dupe.name = name + " (Duplicate)";
			dupe.entityType = entityType;
			dupe.entityProperties = new();
			dupe.entityProperties.CopyFrom( this );
			dupe.entityProperties.name = dupe.name;
			dupe.entityPosition = entityPosition;
			dupe.entityRotation = entityRotation;
			dupe.mapSectionOwner = mapSectionOwner;
			dupe.deploymentColor = deploymentColor;
			dupe.Width = Width;
			dupe.Height = Height;
			dupe.Duration = Duration;
			return dupe;
		}

		public bool Validate()
		{
			if ( !Utils.mainWindow.mission.EntityExists( GUID ) )
			{
				if ( GUID != Guid.Empty )
					name += " (NO LONGER VALID)";
				GUID = Guid.Empty;
				return false;
			}
			return true;
		}
	}
}
