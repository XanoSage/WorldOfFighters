﻿using Assets.Scripts.GameLogic.Weapons;
using UnityEngine;

namespace Assets.Scripts.GameLogic.Plane
{
	public enum PlaneType
	{
		PlayerPlane, // green-orange
		EnemyPlaneGreen,
		EnemyPlaneWhite, 
		EnemyPlaneWhiteGray
	}

	public class PlaneModel
	{
		#region Constants

		private const int DefaultLives = 1;
		private const int DefaultHealthPoint = 1;
		private const float DefaultSpeed = 10f;


		#endregion

		#region Variables

		private PlaneType _planeType;

		public PlaneType Type
		{
			get { return _planeType; }
		}

		private int _lives;
		private int _healthPoint;

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

		#endregion

		#region Constructor

		public PlaneModel()
		{
			_lives = DefaultLives;
			_healthPoint = DefaultHealthPoint;
			_speed = DefaultSpeed;
			_id = 0;
			_planeType = PlaneType.PlayerPlane;
			_acceleration = 0f;
			_accelerationDown = 0f;
			_planeOwner = OwnerInfo.Player;
		}

		public PlaneModel(PlaneType type, int id, int lives, int healtPoint, float speed, float acceleration,
						  float accelereationDown, OwnerInfo owner)
		{
			_planeType = type;
			_id = id;
			_lives = lives;
			_healthPoint = healtPoint;
			_speed = speed;
			_acceleration = acceleration;
			_accelerationDown = accelereationDown;
			_planeOwner = owner;
		}

		public static PlaneModel Create()
		{
			return new PlaneModel();
		}

		public static PlaneModel Create(PlaneType type, int id, int lives, int healtPoint, float speed, float acceleration,
										float accelereationDown, OwnerInfo owner)
		{
			return new PlaneModel(type, id, lives, healtPoint, speed, acceleration, accelereationDown, owner);
		}

		#endregion
	}
}
