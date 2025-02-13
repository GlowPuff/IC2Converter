﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Imperial_Commander_Editor
{
	public class MissionEvent : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		string _eventName, _eventText, _allyDefeated, _heroWounded, _heroWithdraws, _activationOf;
		int _startOfRound, _endOfRound;
		bool _isGlobal, _isRepeatable, _isEndOfCurrentRound;

		//triggered by
		bool _useStartOfRound, _useEndOfRound, _useStartOfEachRound, _useEndOfEachRound, _useAllGroupsDefeated, _useAllHeroesWounded, _useAllyDefeated, _useHeroWounded, _useHeroWithdraws, _useActivation, _behaviorAll, _useAnyHeroWounded, _useAnyHeroDefeated;

		public Guid GUID { get; set; }
		public bool isGlobal
		{
			get { return _isGlobal; }
			set { _isGlobal = value; PC(); }
		}
		public string name
		{
			get { return _eventName; }
			set { _eventName = value; PC(); }
		}
		public string eventText
		{
			get { return _eventText; }
			set { _eventText = value; PC(); }
		}
		public string allyDefeated
		{
			get { return _allyDefeated; }
			set { _allyDefeated = value; PC(); }
		}
		public string heroWounded
		{
			get { return _heroWounded; }
			set { _heroWounded = value; PC(); }
		}
		public string heroWithdraws
		{
			get { return _heroWithdraws; }
			set { _heroWithdraws = value; PC(); }
		}
		public string activationOf
		{
			get { return _activationOf; }
			set { _activationOf = value; PC(); }
		}
		public int startOfRound
		{
			get { return _startOfRound; }
			set { _startOfRound = value; PC(); }
		}
		public int endOfRound
		{
			get { return _endOfRound; }
			set { _endOfRound = value; PC(); }
		}
		public bool useStartOfRound
		{
			get { return _useStartOfRound; }
			set { _useStartOfRound = value; PC(); }
		}
		public bool useEndOfRound
		{
			get { return _useEndOfRound; }
			set { _useEndOfRound = value; PC(); }
		}
		public bool useStartOfEachRound
		{
			get { return _useStartOfEachRound; }
			set { _useStartOfEachRound = value; PC(); }
		}
		public bool useEndOfEachRound
		{
			get { return _useEndOfEachRound; }
			set { _useEndOfEachRound = value; PC(); }
		}
		public bool useAllGroupsDefeated
		{
			get { return _useAllGroupsDefeated; }
			set { _useAllGroupsDefeated = value; PC(); }
		}
		public bool useAllHeroesWounded
		{
			get { return _useAllHeroesWounded; }
			set { _useAllHeroesWounded = value; PC(); }
		}
		public bool useAllyDefeated
		{
			get { return _useAllyDefeated; }
			set { _useAllyDefeated = value; PC(); }
		}
		public bool useHeroWounded
		{
			get { return _useHeroWounded; }
			set { _useHeroWounded = value; PC(); }
		}
		public bool useHeroWithdraws
		{
			get { return _useHeroWithdraws; }
			set { _useHeroWithdraws = value; PC(); }
		}
		public bool useAnyHeroWounded
		{
			get { return _useAnyHeroWounded; }
			set { _useAnyHeroWounded = value; PC(); }
		}
		public bool useAnyHeroDefeated
		{
			get { return _useAnyHeroDefeated; }
			set { _useAnyHeroDefeated = value; PC(); }
		}
		public bool useActivation
		{
			get { return _useActivation; }
			set { _useActivation = value; PC(); }
		}
		public bool isRepeatable
		{
			get { return _isRepeatable; }
			set { _isRepeatable = value; PC(); }
		}
		public bool isEndOfCurrentRound
		{
			get { return _isEndOfCurrentRound; }
			set { _isEndOfCurrentRound = value; PC(); }
		}
		public bool behaviorAll { get { return _behaviorAll; } set { _behaviorAll = value; PC(); } }
		public ObservableCollection<TriggeredBy> additionalTriggers { get; set; }

		[JsonConverter( typeof( EventActionConverter ) )]
		public ObservableCollection<IEventAction> eventActions { get; set; }

		public void PC( [CallerMemberName] string n = "" )
		{
			if ( !string.IsNullOrEmpty( n ) )
				PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( n ) );
		}

		public MissionEvent()
		{
			GUID = Guid.NewGuid();
			name = "Event Name";
			isGlobal = true;
			eventText = "";
			startOfRound = endOfRound = 1;
			useStartOfRound = useEndOfRound = useStartOfEachRound = useEndOfEachRound = useAllGroupsDefeated = useAllHeroesWounded = useAllyDefeated = useHeroWounded = useHeroWithdraws = useActivation = useAnyHeroWounded = useAnyHeroDefeated = false;
			behaviorAll = true;
			isRepeatable = false;
			isEndOfCurrentRound = false;
			additionalTriggers = new();
			eventActions = new();

			heroWounded = "H1";
			heroWithdraws = "H1";
			allyDefeated = "A001";
			activationOf = "DG001";
		}

		//public bool Validate()
		//{
		//	if ( !Utils.ValidateEvent( GUID ) )
		//	{
		//		name = "None (Global)";
		//		GUID = Guid.Empty;
		//		return false;
		//	}
		//	return true;
		//}

		//public object Clone()
		//{
		//	//serialize the object to JSON, then deserialize into a brand new INDEPENDENT object
		//	string output = JsonConvert.SerializeObject( this );
		//	var clone = JsonConvert.DeserializeObject<MissionEvent>( output );
		//	//give it a new, unique GUID and name
		//	clone.GUID = Guid.NewGuid();
		//	clone.name = name + " (Copy)";
		//	//make sure each event action has its own, new GUID
		//	foreach ( var item in clone.eventActions )
		//		item.GUID = Guid.NewGuid();

		//	return clone;
		//}
	}
}
