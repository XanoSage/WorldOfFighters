using System;
using UnityEngine;

namespace Assets.Scripts.GameLogic.Weapons
{
	public enum WeaponsType
	{
		BulletG4M1Betty,
		BulletKi30Nagoya,
		BulletJoyKikka,
		BulletKi57,
	}

	public enum OwnerInfo
	{
		Player,
		AI
	}

	[Serializable]
	public class WeaponSimple
	{
		[SerializeField]
		public WeaponsType Type;

		[SerializeField]
		public float Speed;
		[SerializeField]
		public float BlowUpTime;
		[SerializeField]
		public int Damage;
		[SerializeField]
		public float CooldownTimer;
		[SerializeField]
		public OwnerInfo Owner;
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
			_weaponsType = WeaponsType.BulletG4M1Betty;
			_speed = 0;
			_cooldownTime = 0;
			_timeToBlowUp = 0;
			_damage = 0;
			_ownerInfo = OwnerInfo.Player;
		}

		public Weapons(WeaponsType type, float speed, float cooldownTimer, float timeToBlowUp, int damage, OwnerInfo owner)
		{
			_weaponsType = type;
			_speed = speed;
			_cooldownTime = cooldownTimer;
			_timeToBlowUp = timeToBlowUp;
			_damage = damage;

			_ownerInfo = owner;
		}

		public static Weapons Create()
		{
			return new Weapons();
		}

		public static Weapons Create(WeaponsType type, float speed, float cooldownTimer, float timeToBlowUp, int damage, OwnerInfo owner)
		{
			return new Weapons(type, speed, cooldownTimer, timeToBlowUp, damage, owner);
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
