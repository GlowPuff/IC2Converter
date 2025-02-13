﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Imperial_Commander_Editor
{
	public class ProjectItem : ObservableObject, IComparable<ProjectItem>
	{
		string _title;

		public string Title { get => _title; set { SetProperty( ref _title, value ); } }
		public string Date { get; set; }
		public string Description { get; set; }
		public string fileName { get; set; }
		//public string relativePath { get; set; }
		public string fileVersion { get; set; }
		public long timeTicks { get; set; }
		public string missionGUID { get; set; }
		public string fullPathWithFilename { get; set; }

		public int CompareTo( ProjectItem other ) => timeTicks > other.timeTicks ? -1 : timeTicks < other.timeTicks ? 1 : 0;
	}

	public class DeploymentColor
	{
		public string name { get; set; }
		public Color color { get; set; }

		public DeploymentColor( string n, Color c )
		{
			name = n;
			color = c;
		}
	}

	//public class Question
	//{
	//	public string buttonText { get; set; }
	//	public Guid triggerGUID { get; set; }
	//	public Guid eventGUID { get; set; }
	//}

	public class EntityModifier
	{
		public Guid GUID { get; set; } = Guid.NewGuid();
		public Guid sourceGUID { get; set; }
		public bool hasColor { get; set; }
		public bool hasProperties { get; set; }
		public EntityProperties entityProperties { get; set; }
	}

	public class ButtonAction
	{
		public Guid GUID { get; set; } = Guid.NewGuid();
		public string buttonText { get; set; }
		public Guid triggerGUID { get; set; }
		public Guid eventGUID { get; set; }
	}

	public class DPData
	{
		public Guid GUID { get; set; }
	}

	public class EnemyGroupData : INotifyPropertyChanged
	{
		CustomInstructionType _customInstructionType;
		string _customText, _cardName, _cardID;
		Guid _defeatedTrigger, _defeatedEvent;
		bool _useGenericMugshot, _useInitialGroupCustomName;

		public Guid GUID { get; set; }
		public string cardName { get { return _cardName; } set { _cardName = value; PC(); } }
		public string cardID { get { return _cardID; } set { _cardID = value; PC(); } }
		public CustomInstructionType customInstructionType { get { return _customInstructionType; } set { _customInstructionType = value; PC(); } }
		public string customText { get { return _customText; } set { _customText = value; PC(); } }
		public ObservableCollection<DPData> pointList { get; set; } = new();
		public GroupPriorityTraits groupPriorityTraits { get; set; }
		public Guid defeatedTrigger { get { return _defeatedTrigger; } set { _defeatedTrigger = value; PC(); } }
		public Guid defeatedEvent { get { return _defeatedEvent; } set { _defeatedEvent = value; PC(); } }
		public bool useGenericMugshot { get { return _useGenericMugshot; } set { _useGenericMugshot = value; PC(); } }
		public bool useInitialGroupCustomName { get { return _useInitialGroupCustomName; } set { _useInitialGroupCustomName = value; PC(); } }

		public void PC( [CallerMemberName] string n = "" )
		{
			if ( !string.IsNullOrEmpty( n ) )
				PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( n ) );
		}
		public event PropertyChangedEventHandler PropertyChanged;

		public EnemyGroupData()
		{

		}

		public EnemyGroupData( DeploymentCard dc, DeploymentPoint dp )
		{
			GUID = Guid.NewGuid();
			cardName = dc.name;
			cardID = dc.id;
			customText = "";
			customInstructionType = CustomInstructionType.Replace;
			groupPriorityTraits = new();
			defeatedTrigger = Guid.Empty;
			defeatedEvent = Guid.Empty;
			useGenericMugshot = false;
			useInitialGroupCustomName = false;
			for ( int i = 0; i < dc.size; i++ )
			{
				pointList.Add( new() { GUID = dp.GUID } );
			}
		}

		public void SetDP( Guid guid )
		{
			int c = pointList.Count;
			pointList.Clear();
			for ( int i = 0; i < c; i++ )
			{
				pointList.Add( new() { GUID = guid } );
			}
		}

		public void UpdateCard( DeploymentCard newcard )
		{
			cardName = newcard.name;
			cardID = newcard.id;

			var oldPoints = pointList.ToArray();
			pointList.Clear();
			for ( int i = 0; i < newcard.size; i++ )
			{
				if ( i < oldPoints.Length )
					pointList.Add( oldPoints[i] );
				else
					pointList.Add( new() { GUID = Guid.Empty } );
			}
		}
	}

	public class GitHubResponse
	{
		public string tag_name;
		public string body;
	}

	public class InputRange : INotifyPropertyChanged
	{
		string _theText;
		int _fromValue, _toValue;
		Guid _triggerGUID, _eventGUID;

		public void PC( [CallerMemberName] string n = "" )
		{
			if ( !string.IsNullOrEmpty( n ) )
				PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( n ) );
		}
		public event PropertyChangedEventHandler PropertyChanged;

		public Guid GUID { get; set; } = Guid.NewGuid();
		public string theText { get { return _theText; } set { _theText = value; PC(); } }
		public int fromValue { get { return _fromValue; } set { _fromValue = value; PC(); } }
		public int toValue { get { return _toValue; } set { _toValue = value; PC(); } }
		public Guid triggerGUID { get { return _triggerGUID; } set { _triggerGUID = value; PC(); } }
		public Guid eventGUID { get { return _eventGUID; } set { _eventGUID = value; PC(); } }

		public InputRange()
		{
		}
	}

	public class ThumbnailData
	{
		public List<Thumbnail> Other, Rebel, Imperial, Mercenary, StockImperial, StockAlly, StockHero, StockVillain;
		public Thumbnail NoneThumb => new() { Name = "Select a Thumbnail", ID = "None" };//None[0];

		public ThumbnailData()
		{
			Other = new List<Thumbnail>();
			Rebel = new List<Thumbnail>();
			Imperial = new List<Thumbnail>();
			Mercenary = new List<Thumbnail>();
			StockImperial = new List<Thumbnail>();
			StockAlly = new List<Thumbnail>();
			StockHero = new List<Thumbnail>();
			StockVillain = new List<Thumbnail>();
		}

		public List<Thumbnail> Filter( ThumbType ttype )
		{
			List<Thumbnail> None = new( new Thumbnail[] { new() { Name = "Select a Thumbnail", ID = "None" } } );

			switch ( ttype )
			{
				case ThumbType.All:
					return None.Concat( Other ).Concat( Rebel ).Concat( Imperial ).Concat( Mercenary ).Concat( StockImperial ).Concat( StockAlly ).Concat( StockHero ).Concat( StockVillain ).OrderBy( x => x.Name ).ToList();
				case ThumbType.Other:
					return None.Concat( Other ).OrderBy( x => x.Name ).ToList();
				case ThumbType.Rebel:
					return None.Concat( Rebel ).OrderBy( x => x.Name ).ToList();
				case ThumbType.Imperial:
					return None.Concat( Imperial ).OrderBy( x => x.Name ).ToList();
				case ThumbType.Mercenary:
					return None.Concat( Mercenary ).OrderBy( x => x.Name ).ToList();
				case ThumbType.StockImperial:
					return None.Concat( StockImperial ).OrderBy( x => x.Name ).ToList();
				case ThumbType.StockAlly:
					return None.Concat( StockAlly ).OrderBy( x => x.Name ).ToList();
				case ThumbType.StockHero:
					return None.Concat( StockHero ).OrderBy( x => x.Name ).ToList();
				case ThumbType.StockVillain:
					return None.Concat( StockVillain ).OrderBy( x => x.Name ).ToList();
				default:
					return Other;
			}
		}
	}

	public class Thumbnail
	{
		public string Name { get; set; }//full name of icon's character
		public string ID { get; set; }//basically the filename
	}

	public class CardInstruction
	{
		public string instName, instID;
		public List<InstructionOption> content = new();
	}

	public class InstructionOption
	{
		public List<string> instruction = new();//line by line instructions
	}

	public class BonusEffect
	{
		public string bonusID;
		public List<string> effects = new();
	}

	public class CampaignSkill
	{
		public string owner;//use custom toon's unique ID
		public string id;//use custom toon's unique ID
		public string name { get; set; }
		public int cost { get; set; }
	}

	public class MissionNameData
	{
		public string id { get; set; }
		public string name { get; set; }
	}
}
