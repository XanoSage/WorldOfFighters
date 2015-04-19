namespace Assets.Scripts.GameLogic.Weapons
{
	public enum WeaponsType
	{
		Bullet,
		Missile
	}

	public enum OwnerInfo
	{
		Player,
		AI
	}

	public class Weapons
	{
		#region Variables

		private WeaponsType _weaponsType;

		public WeaponsType Type
		{
			get { return _weaponsType; }
		}

		private float _speed;

		public float Speed
		{
			get { return _speed; }
		}

		private float _cooldownTime;

		public float CooldownTime
		{
			get { return _cooldownTime; }
		}

		private float _timeToBlowUp;

		public float TimeToBlowUp
		{
			get { return _timeToBlowUp; }
		}

		private int _damage;

		public int Damage
		{
			get { return _damage; }
		}

		private OwnerInfo _ownerInfo;
		
		public OwnerInfo Owner
		{
			get { return _ownerInfo; }
		}

		#endregion

		#region Constructor

		public Weapons()
		{
			_weaponsType = WeaponsType.Bullet;
			_speed = 0;
			_cooldownTime = 0;
			_timeToBlowUp = 0;
			_damage = 0;
			_ownerInfo = OwnerInfo.Player;
		}

		public Weapons(WeaponsType type, float speed, float cooldownTimer, float timeToBlowUp, int damage)
		{
			_weaponsType = type;
			_speed = speed;
			_cooldownTime = cooldownTimer;
			_timeToBlowUp = timeToBlowUp;
			_damage = damage;
		}

		public static Weapons Create()
		{
			return new Weapons();
		}

		public static Weapons Create(WeaponsType type, float speed, float cooldownTimer, float timeToBlowUp, int damage)
		{
			return new Weapons(type, speed, cooldownTimer, timeToBlowUp, damage);
		}

		#endregion

		#region Actions

		public void SetOwner(OwnerInfo owner)
		{
			_ownerInfo = owner;
		}

		#endregion
	}
}
