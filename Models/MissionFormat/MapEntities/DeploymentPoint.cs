using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Imperial_Commander_Editor
{
	public class DeploymentPoint : INotifyPropertyChanged, IMapEntity
	{
		string _name;
		Guid _mapSectionOwner;

		//common props
		public Guid GUID { get; set; }
		public string name
		{
			get { return _name; }
			set
			{
				_name = string.IsNullOrEmpty( value ) ? "New Deployment Point" : value;
				PC();
			}
		}
		public EntityType entityType { get; set; }
		public Vector entityPosition { get; set; }
		public double entityRotation { get; set; }
		public EntityProperties entityProperties { get; set; }
		public bool hasProperties { get { return false; } }
		public bool hasColor { get { return true; } }

		//dp props
		public string deploymentColor
		{
			get { return entityProperties.entityColor; }
			set
			{
				entityProperties.entityColor = value;
				PC();
			}
		}
		public Guid mapSectionOwner { get { return _mapSectionOwner; } set { _mapSectionOwner = value; PC(); } }
		public DeploymentPointProps deploymentPointProps { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		public void PC( [CallerMemberName] string n = "" )
		{
			if ( !string.IsNullOrEmpty( n ) )
				PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( n ) );
		}

		public DeploymentPoint()
		{
		}

		public DeploymentPoint( Guid ownerGUID )
		{
			GUID = Guid.NewGuid();
			name = "New Deployment Point";
			entityType = EntityType.DeploymentPoint;
			entityProperties = new() { name = name };
			mapSectionOwner = ownerGUID;
			deploymentPointProps = new();

			entityProperties.isActive = false;

			deploymentColor = "Gray";
		}

		public IMapEntity Duplicate()
		{
			var dupe = new DeploymentPoint();
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
			dupe.deploymentPointProps = new();
			dupe.deploymentPointProps.CopyFrom( this.deploymentPointProps );

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
