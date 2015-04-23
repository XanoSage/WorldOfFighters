using System;
using System.Collections.Generic;
using Assets.Scripts.GameLogic.Game;
using Assets.Scripts.GameLogic.Weapons;
using UnityEngine;
using System.Collections;

public class WeaponsInformer : MonoBehaviour
{

	#region Constants

	private const string BulletG4M1PrefabPath = "Prefabs/Weapons/BulletG4M1Betty";
	private const string BulletJoyKikkaPrefabPath = "Prefabs/Weapons/BulletJoyKikka";
	private const string BulletKi30NagoyaPrefabPath = "Prefabs/Weapons/BulletKi30Nagoya";
	private const string BulletKi57PrefabPath = "Prefabs/Weapons/BulletKi57";

	#endregion

	#region Variables
	[SerializeField]
	private BulletsHelper _shellParent;

	public enum WeaponsState
	{
		ReadyToShoot,
		Reload
	}

	[Serializable]
	public class WeaponsInfoPair
	{
		public WeaponsBehaviour Weapons;
		public Transform StartPosition;
		private WeaponsInformer.WeaponsState _state;
		public WeaponsState State
		{
			get { return _state; }
		}

		private float _cooldownTimer;

		public void Update()
		{
			if (_state != WeaponsState.Reload) return;

			if (_cooldownTimer < Weapons.CooldownTime)
			{
				_cooldownTimer += Time.deltaTime;
			}

			if (_cooldownTimer >= Weapons.CooldownTime)
			{
				_state = WeaponsState.ReadyToShoot;
				_cooldownTimer = 0;
			}
		}

		public void Fire()
		{
			if (_state == WeaponsState.ReadyToShoot)
			{
				_state = WeaponsState.Reload;
			}
		}

	}

	[SerializeField] private List<WeaponsInfoPair> _weaponsInfoPairs;

	public List<WeaponsInfoPair> WeaponsInfoPairs { get { return _weaponsInfoPairs; } }

	#endregion

	#region MonoBehaviour Action

	private void Awake()
	{
		_shellParent = FindObjectOfType<BulletsHelper>();

		if (_shellParent == null)
		{
			throw new MissingComponentException("WeaponsInformer.Start - can't find BulletsHelper component");
		}	
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () {

		if (WeaponsInfoPairs == null)
			return;

		foreach (WeaponsInfoPair weaponsInfoPair in WeaponsInfoPairs)
		{
			weaponsInfoPair.Update();
		}
	}

	#endregion

	#region Actions
	
	public void Fire()
	{
		foreach (WeaponsInfoPair weaponsInfoPair in _weaponsInfoPairs)
		{
			if (weaponsInfoPair.State == WeaponsState.Reload)
				continue;

			WeaponsBehaviour weaponsBehaviour = ResourceController.GetBulletFromPool(GetWeaponPrefabPathByType(weaponsInfoPair.Weapons.Type), _shellParent.transform);

			if (weaponsBehaviour != null)
			{
				Vector3 direction = weaponsInfoPair.Weapons.Owner == OwnerInfo.Player ? weaponsInfoPair.StartPosition.up : Vector3.up;

				weaponsBehaviour.Init(_shellParent.transform, weaponsInfoPair.StartPosition.position, weaponsInfoPair.Weapons.transform.rotation,
									  direction);
			}

			weaponsInfoPair.Fire();
			
		}
	}

	private string GetWeaponPrefabPathByType(WeaponsType type)
	{
		string path = BulletG4M1PrefabPath;

		switch (type)
		{
			case WeaponsType.BulletG4M1Betty:
				path = BulletG4M1PrefabPath;
				break;

			case WeaponsType.BulletJoyKikka:
				path = BulletJoyKikkaPrefabPath;
				break;

			case WeaponsType.BulletKi30Nagoya:
				path = BulletKi30NagoyaPrefabPath;
				break;

			case WeaponsType.BulletKi57:
				path = BulletKi57PrefabPath;
				break;
		}

		return path;
	}

	#endregion
}
