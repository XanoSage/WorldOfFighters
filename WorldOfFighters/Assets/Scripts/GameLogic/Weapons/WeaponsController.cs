using System.Collections.Generic;
using Assets.Scripts.GameLogic.Helper;
using UnityEngine;
using System.Collections;

public class WeaponsController : MonoBehaviour
{
	#region Variables

	private WeaponsInformer _weaponsInformer;

	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	void Start ()
	{

		_weaponsInformer = GetComponent<WeaponsInformer>();

		if (_weaponsInformer == null)
		{
			throw new MissingComponentException("WeaponsController");
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
		{
			Fire();
		}
	}

	#endregion

	#region Actions

	private void Fire()
	{
		_weaponsInformer.Fire();
	}

	#endregion
}
