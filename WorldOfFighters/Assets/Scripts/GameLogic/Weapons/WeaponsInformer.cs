using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class WeaponsInformer : MonoBehaviour
{

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

	// Use this for initialization
	void Start ()
	{
		_shellParent = FindObjectOfType<BulletsHelper>();

		if (_shellParent == null)
		{
			throw new MissingComponentException("WeaponsInformer.Start - can't find BulletsHelper component");
		}
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

			GameObject  shellOpject = (GameObject)Instantiate(weaponsInfoPair.Weapons.transform.gameObject, weaponsInfoPair.StartPosition.position,
			                                                weaponsInfoPair.StartPosition.rotation);

			if (shellOpject == null)
			{
				continue;
			}

			WeaponsBehaviour weaponsBehaviour = shellOpject.GetComponent<WeaponsBehaviour>();

			if (weaponsBehaviour != null)
			{
				weaponsBehaviour.Init(_shellParent.transform, weaponsInfoPair.StartPosition.position, weaponsInfoPair.StartPosition.rotation,
				                      weaponsInfoPair.StartPosition.up);
			}

			weaponsInfoPair.Fire();
			
		}
	}

	#endregion
}
