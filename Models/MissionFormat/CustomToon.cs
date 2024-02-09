using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Imperial_Commander_Editor
{
	public class CustomToon : ObservableObject
	{
		//these properties don't change when copying from another Deployment Group
		string _cardName, _cardSubName, _cardID;
		Factions _faction;
		//and Deployment Card outline color, character type

		Guid _customCharacterGUID;
		Thumbnail _thumbnail;
		DeploymentCard _deploymentCard;
		string _groupAttack, _groupDefense;
		CardInstruction _cardInstruction;
		BonusEffect _bonusEffect;
		bool _canRedeploy, _canReinforce, _canBeDefeated, _useThreatMultiplier;

		public Guid customCharacterGUID { get => _customCharacterGUID; set { SetProperty( ref _customCharacterGUID, value ); } }
		//update the embedded DG's name/subname, faction, instructions ID, bonus effect ID, and id when it changes
		public string cardName
		{
			get => _cardName;
			set
			{
				SetProperty( ref _cardName, value );
				deploymentCard.name = value;
				cardInstruction.instName = value;
			}
		}
		public string cardSubName { get => _cardSubName; set { SetProperty( ref _cardSubName, value ); deploymentCard.subname = value; } }
		public string cardID
		{
			get => _cardID;
			set
			{
				SetProperty( ref _cardID, value );
				deploymentCard.id = value;
				if ( cardInstruction != null )
					cardInstruction.instID = value;
				if ( bonusEffect != null )
					bonusEffect.bonusID = value;
			}
		}
		public Factions faction
		{
			get => _faction;
			set
			{
				SetProperty( ref _faction, value );
				deploymentCard.faction = value.ToString();
			}
		}

		public ObservableCollection<CampaignSkill> heroSkills { get; set; }
		public string groupAttack
		{
			get => _groupAttack;
			set
			{
				SetProperty( ref _groupAttack, value );
				deploymentCard.attacks = Utils.ParseCustomDice( value.Split( ' ' ) );
			}
		}
		public string groupDefense
		{
			get => _groupDefense;
			set
			{
				SetProperty( ref _groupDefense, value );
				deploymentCard.defense = Utils.ParseCustomDice( value.Split( ' ' ) );
			}
		}

		public DeploymentCard deploymentCard { get => _deploymentCard; set => SetProperty( ref _deploymentCard, value ); }

		public Thumbnail thumbnail
		{
			get => _thumbnail;
			set => SetProperty( ref _thumbnail, value );
		}
		public BonusEffect bonusEffect
		{
			get => _bonusEffect;
			set => SetProperty( ref _bonusEffect, value );
		}
		public CardInstruction cardInstruction
		{
			get => _cardInstruction;
			set => SetProperty( ref _cardInstruction, value );
		}
		public bool canRedeploy
		{
			get => _canRedeploy;
			set => SetProperty( ref _canRedeploy, value );
		}
		public bool canReinforce
		{
			get => _canReinforce;
			set => SetProperty( ref _canReinforce, value );
		}
		public bool canBeDefeated
		{
			get => _canBeDefeated;
			set => SetProperty( ref _canBeDefeated, value );
		}
		public bool useThreatMultiplier
		{
			get => _useThreatMultiplier;
			set => SetProperty( ref _useThreatMultiplier, value );
		}

		public CustomToon()
		{
			deploymentCard = new();
			heroSkills = new();
			cardInstruction = new();
			thumbnail = new() { Name = "Select a Thumbnail", ID = "None" };
			//Utils.thumbnailData.NoneThumb;
		}
	}
}
