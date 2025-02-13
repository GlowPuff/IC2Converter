﻿namespace Imperial_Commander_Editor
{
	public class EnemyDeployment : EventAction
	{
		string _enemyName, _deploymentGroup, _modification, _repositionInstructions;
		int _threatCost;
		bool _canReinforce, _canRedeploy, _canBeDefeated, _useThreat, _showMod, _useCustomInstructions, _useGenericMugshot, _useResetOnRedeployment;
		Guid _specificDeploymentPoint;
		DeploymentSpot _deploymentPoint;
		Thumbnail _thumbnail;

		public string enemyName { get { return _enemyName; } set { _enemyName = value; PC(); } }
		public string deploymentGroup { get { return _deploymentGroup; } set { _deploymentGroup = value; PC(); } }
		public string repositionInstructions { get { return _repositionInstructions; } set { _repositionInstructions = value; PC(); } }
		public int threatCost { get { return _threatCost; } set { _threatCost = value; PC(); } }
		public string modification { get { return _modification; } set { _modification = value; PC(); } }
		public bool canReinforce { get { return _canReinforce; } set { _canReinforce = value; PC(); } }
		public bool canRedeploy { get { return _canRedeploy; } set { _canRedeploy = value; PC(); } }
		public bool canBeDefeated { get { return _canBeDefeated; } set { _canBeDefeated = value; PC(); } }
		public bool useThreat { get { return _useThreat; } set { _useThreat = value; PC(); } }
		public bool useCustomInstructions { get { return _useCustomInstructions; } set { _useCustomInstructions = value; PC(); } }
		public bool useGenericMugshot { get { return _useGenericMugshot; } set { _useGenericMugshot = value; PC(); } }
		public bool showMod { get { return _showMod; } set { _showMod = value; PC(); } }
		public bool useResetOnRedeployment { get { return _useResetOnRedeployment; } set { _useResetOnRedeployment = value; PC(); } }
		public Guid specificDeploymentPoint { get { return _specificDeploymentPoint; } set { _specificDeploymentPoint = value; PC(); } }
		public DeploymentSpot deploymentPoint { get { return _deploymentPoint; } set { _deploymentPoint = value; PC(); } }
		public EnemyGroupData enemyGroupData { get; set; }
		public Thumbnail thumbnail { get => _thumbnail; set { _thumbnail = value; PC(); } }

		public EnemyDeployment()
		{

		}

		public EnemyDeployment( string dname
			, EventActionType et ) : base( et, dname )
		{
			_deploymentGroup = "DG001";
			_threatCost = 0;
			_canRedeploy = _canReinforce = true;
			_deploymentPoint = DeploymentSpot.Active;
			_specificDeploymentPoint = Guid.Empty;
			_useThreat = _showMod = false;
			_useCustomInstructions = false;
			_canBeDefeated = true;
			_repositionInstructions = "";
			_useGenericMugshot = false;
			_useResetOnRedeployment = false;
			_thumbnail = Utils.thumbnailData.NoneThumb;

			DeploymentCard card = Utils.enemyData.First( x => x.id == _deploymentGroup );
			enemyGroupData = new( card, new() { name = "Active Deployment Point", GUID = Guid.Empty } );
		}

		public void UpdateCard( DeploymentCard newcard )
		{
			enemyGroupData.UpdateCard( newcard );
		}
	}
}