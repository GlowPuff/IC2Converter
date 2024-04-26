using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Imperial_Commander_Editor
{
	public class Console : INotifyPropertyChanged, IMapEntity
	{
		string _name;
		Guid _mapSectionOwner;//, _GUID;

		//common props
		public Guid GUID { get; set; }
		//{ get { return _GUID; } set { _GUID = value; entityProperties.ownerGUID = value; } }
		public string name
		{
			get { return _name; }
			set
			{
				_name = string.IsNullOrEmpty( value ) ? "New Console" : value;
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

		//console props
		public string deploymentColor
		{
			get { return entityProperties.entityColor; }
			set
			{
				entityProperties.entityColor = value;
				PC();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void PC( [CallerMemberName] string n = "" )
		{
			if ( !string.IsNullOrEmpty( n ) )
				PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( n ) );
		}

		public Console()
		{
		}

		public Console( Guid ownderGUID )
		{
			GUID = Guid.NewGuid();
			name = "New Terminal";
			entityType = EntityType.Console;
			entityProperties = new() { name = name };
			mapSectionOwner = ownderGUID;

			deploymentColor = "Gray";
		}

		public IMapEntity Duplicate()
		{
			var dupe = new Console();
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
