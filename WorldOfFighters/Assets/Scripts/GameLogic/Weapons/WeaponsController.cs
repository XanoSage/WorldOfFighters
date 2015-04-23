using System.Collections.Generic;
using Assets.Scripts.GameLogic.Helper;
using Assets.Scripts.GameLogic.Plane;
using Assets.Scripts.GameLogic.Weapons;
using UnityEngine;
using System.Collections;
[RequireComponent(typeof (WeaponsInformer))]
public class WeaponsController : MonoBehaviour
{
	#region Variables

	private WeaponsInformer _weaponsInformer;
	private PlaneControlling _plane;

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

		_plane = GetComponent<PlaneControlling>();

	}
	
	// Update is called once per frame
	void Update () {

		if (GameController.Instance != null && GameController.Instance.State != GameModel.GameState.Playing)
			return;


		if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
		{
			if (_plane != null && _plane.Owner == OwnerInfo.Player && _plane.Plane.State != PlaneState.Death)
				Fire();
		}
	}

	#endregion

	#region Actions

	public void Fire()
	{
		if (_weaponsInformer ==null)
			return;

		_weaponsInformer.Fire();
	}

	#endregion
}
