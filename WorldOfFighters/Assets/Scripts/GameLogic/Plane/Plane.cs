using System;
using Assets.Scripts.GameLogic.Game;
using Assets.Scripts.GameLogic.Weapons;
using UnityEngine;

namespace Assets.Scripts.GameLogic.Plane
{
	public enum PlaneType
	{
		G4M1Betty, // green-orange, playerPlane
		Ki30Nagoya, // white, enemy
		JoyKikka, // black
		Ki57 // gray
	}

	public enum PlaneState
	{
		Invulnerability,
		Normal,
		Death,
	}

	[Serializable]
	public class PlaneSimple
	{
		[SerializeField] public PlaneType Type;
		[SerializeField] public int Lives;
		[SerializeField] public int HealthPoint;
		[SerializeField] public int Id;
		[SerializeField] public float Speed;
		[SerializeField] public float AccelerationPlane;
		[SerializeField] public float AccelerationDown;
		[SerializeField] public OwnerInfo Owner;
		[SerializeField] public int BonusPoint;
	}

	public class PlaneModel
	{
		#region Constants

		private const int MaxPlayerLife = 5;
		public const float InvulnerabilityTime = 3f;
		public const float DeathTime = 2f;

		#endregion

		#region Variables

		private PlaneType _planeType;

		public PlaneType Type
		{
			get { return _planeType; }
		}

		private int _lives;
		private int _livesDefault;

		public int Lives
		{
			get { return _lives; }
		}

		private int _healthPoint;

		private int _healthPointDefault;

		public int HealthPoint
		{
			get { return _healthPoint; }
		}

		private float _speed;

		public float Speed
		{
			get { return _speed; }
		}

		private float _acceleration;

		public float Acceleration
		{
			get { return _acceleration; }
		}

		private float _accelerationDown;

		public float AccelerationDown
		{
			get { return _accelerationDown; }
		}

		private int _id;

		public int Id
		{
			get { return _id; }
		}

		private OwnerInfo _planeOwner;

		public OwnerInfo Owner
		{
			get { return _planeOwner; }
		}

		private int _bonusPoint;

		public int BonusPoint
		{
			get { return _bonusPoint; }
		}

		private PlaneState _planeState;

		public PlaneState State
		{
			get { return _planeState; }
		}

		public event Action<PlaneModel> OnPlaneDeath;
		public event Action<PlaneModel> OnPlayerGameOver;

		private IPlaneDeathListener _planeDeathListener;

		#endregion

		#region Constructor

		public PlaneModel()
		{
			_lives = 1;
			_livesDefault = _lives;
			_healthPoint = 1;
			_healthPointDefault = _healthPoint;
			_speed = 300;
			_id = 0;
			_planeType = PlaneType.G4M1Betty;
			_acceleration = 0f;
			_accelerationDown = 0f;
			_planeOwner = OwnerInfo.Player;

			_bonusPoint = 0;
			_planeState = PlaneState.Normal;
		}

		public PlaneModel(PlaneType type, int id, int lives, int healtPoint, float speed, float acceleration,
		                  float accelereationDown, OwnerInfo owner, int bonusPoint)
		{
			_planeType = type;
			_id = id;
			_lives = lives;
			_livesDefault = _lives;
			_healthPoint = healtPoint;
			_healthPointDefault = _healthPoint;
			_speed = speed;
			_acceleration = acceleration;
			_accelerationDown = accelereationDown;
			_planeOwner = owner;

			_bonusPoint = bonusPoint;

			if (Owner == OwnerInfo.Player)
			{
				_planeState = PlaneState.Invulnerability;
			}
			else
			{
				_planeState = PlaneState.Normal;
			}
		}

		public static PlaneModel Create()
		{
			return new PlaneModel();
		}

		public static PlaneModel Create(PlaneType type, int id, int lives, int healtPoint, float speed, float acceleration,
		                                float accelereationDown, OwnerInfo owner, int bonusPoint)
		{
			return new PlaneModel(type, id, lives, healtPoint, speed, acceleration, accelereationDown, owner, bonusPoint);
		}

		public static PlaneModel Create(PlaneSimple planeSimple)
		{
			return new PlaneModel(planeSimple.Type, planeSimple.Id, planeSimple.Lives, planeSimple.HealthPoint, planeSimple.Speed,
			                      planeSimple.AccelerationPlane, planeSimple.AccelerationDown, planeSimple.Owner,
			                      planeSimple.BonusPoint);
		}

		#endregion

		public void AddLives(int count)
		{
			_lives += count;

			if (_lives > MaxPlayerLife)
			{
				_lives = MaxPlayerLife;
			}
		}

		public void AddLive()
		{
			_lives++;
			if (_lives > MaxPlayerLife)
			{
				_lives = MaxPlayerLife;
			}
		}

		public void SubstractionLives()
		{
			_lives--;

			if (_lives < 0)
			{
				_lives = 0;
				if (OnPlayerGameOver != null)
				{
					OnPlayerGameOver(this);
				}
			}
		}

		public void SetPlaneState(PlaneState state)
		{
			_planeState = state;
		}

		public void TakeDamage(int damage)
		{
			_healthPoint -= damage;

			if (_healthPoint <= 0)
			{
				_healthPoint = 0;
				SubstractionLives();
				_planeState = PlaneState.Death;
				if (OnPlaneDeath != null)
				{
					OnPlaneDeath(this);
				}
				if (_planeDeathListener != null)
				{
					_planeDeathListener.OnPlaneDeath(this);
				}
			}
		}

		public void Reset(bool resetAll = false)
		{
			if (_planeOwner == OwnerInfo.Player)
			{
				_planeState = PlaneState.Invulnerability;
			}
			else
			{
				_planeState = PlaneState.Normal;
			}
			_healthPoint = _healthPointDefault;

			if (resetAll)
			{
				_lives = _livesDefault;
			}
		}

		public void SetPlaneDeathListener(IPlaneDeathListener planeDeathListener)
		{
			_planeDeathListener = planeDeathListener;
		}

	}
}
