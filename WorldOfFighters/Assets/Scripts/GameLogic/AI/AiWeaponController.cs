using UnityEngine;
using System.Collections;

public class AiWeaponController : MonoBehaviour
{

	#region Variables

	[SerializeField] private float _cooldownTime; //frequency

	private float _cooldownTimeCounter;

	private WeaponsController _weaponsController;

	#endregion

	#region MonoBehaviour actions

	// Use this for initialization
	void Start ()
	{

		_weaponsController = GetComponent<WeaponsController>();
		if (_weaponsController == null)
		{
			throw new MissingComponentException("AiWeaponController.Start - can't find componemt");
		}

		_cooldownTimeCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.Instance != null && GameController.Instance.State != GameModel.GameState.Playing)
			return;

		FireUpdate();
	}

	#endregion

	#region Actions

	private void FireUpdate()
	{
		if (_cooldownTimeCounter < _cooldownTime)
		{
			_cooldownTimeCounter += Time.deltaTime;
		}

		if (_cooldownTimeCounter >= _cooldownTime)
		{
			_cooldownTimeCounter = 0;

			_weaponsController.Fire();
		}
	}

	#endregion
}
