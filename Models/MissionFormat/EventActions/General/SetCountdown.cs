namespace Imperial_Commander_Editor
{
	public class SetCountdown : EventAction
	{
		int _countdownTimer;
		Guid _eventGUID, _triggerGUID;
		bool _showPlayerCountdown;//whether to show a number in IC2 so players know how many rounds remain

		public int countdownTimer { get => _countdownTimer; set { _countdownTimer = value; PC(); } }
		public Guid eventGUID { get => _eventGUID; set { _eventGUID = value; PC(); } }
		public Guid triggerGUID { get => _triggerGUID; set { _triggerGUID = value; PC(); } }
		public bool showPlayerCountdown { get => _showPlayerCountdown; set { _showPlayerCountdown = value; PC(); } }

		public SetCountdown()
		{

		}

		public SetCountdown( string dname
	, EventActionType et ) : base( et, dname )
		{
			countdownTimer = 0;
			eventGUID = Guid.Empty;
			triggerGUID = Guid.Empty;
			showPlayerCountdown = false;
		}
	}
}