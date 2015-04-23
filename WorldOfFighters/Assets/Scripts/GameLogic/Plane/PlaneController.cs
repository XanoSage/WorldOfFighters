using Assets.Scripts.GameLogic.Plane;
using Assets.Scripts.GameLogic.Weapons;
using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlaneControlling))]
public class PlaneController : MonoBehaviour
{

	#region Variables

	private PlaneModel _planeModel;
	private PlaneControlling _planeControlling;

	private SpriteRenderer _spriteRenderer;


	private float _invulnerabilityTimer = 0;
	private float _deathStateTimer = 0;

	private float _visibleTime = 0.5f;

	private float _visibleTimer;

	private bool _visible = true;

	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	void Start ()
	{
		_planeControlling = GetComponent<PlaneControlling>();

		if (_planeControlling == null)
		{
			throw new MissingComponentException("PlayerController.Start - can't find PlaneControlling component");
		}

		_planeModel = _planeControlling.Plane;
		_planeModel.OnPlaneDeath += OnPlaneDeath;


		_spriteRenderer = GetComponent<SpriteRenderer>();

		if (_spriteRenderer == null)
		{
			throw new MissingComponentException("PlayerController.Start - can't find SpriteRenderer component");
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log("WeaponsBehaviour.OnCollisionEnter2D - collision name: " + coll.transform.name);
		WeaponsBehaviour weaponsBehaviour = coll.transform.GetComponent<WeaponsBehaviour>();

		if (weaponsBehaviour != null)
		{
			if (_planeModel.Owner != weaponsBehaviour.Owner)
			{
				_planeModel.TakeDamage(weaponsBehaviour.Weapon.Damage);

			}
		}
	}
	#endregion

	#region Actions

	private void UpdatePlaneState()
	{
		switch (_planeModel.State)
		{
			case PlaneState.Normal:
				break;
			case PlaneState.Invulnerability:
				UpdateInvulnerability();
				if (_invulnerabilityTimer < PlaneModel.InvulnerabilityTime)
				{
					_invulnerabilityTimer += Time.deltaTime;
				}

				if (_invulnerabilityTimer >= PlaneModel.InvulnerabilityTime)
				{
					_invulnerabilityTimer = 0;
					SpriteVisible(true);
					_planeModel.SetPlaneState(PlaneState.Normal);
				}
				break;
			case PlaneState.Death:
				if (_deathStateTimer < PlaneModel.DeathTime)
				{
					_deathStateTimer += Time.deltaTime;
				}

				if (_deathStateTimer >= PlaneModel.DeathTime)
				{
					_deathStateTimer = 0f;
					if (_planeModel.Owner == OwnerInfo.Player)
						_planeModel.SetPlaneState(PlaneState.Invulnerability);
					else
					{
						_planeModel.SetPlaneState(PlaneState.Normal);
					}
					OnAfterDeath();
				}
				break;
		}
	}

	private void UpdateInvulnerability()
	{
		if (_visibleTimer < _visibleTime)
		{
			_visibleTimer += Time.deltaTime;
		}

		if (_visibleTimer >= _visibleTime)
		{
			_visibleTimer = 0;
			SpriteVisible(!_visible);
		}
	}

	private void SpriteVisible(bool isVisible)
	{
		_visible = isVisible;
		_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, isVisible ? 1 : 0);
	}

	private void OnPlaneDeath(PlaneModel plane)
	{
		//_planeControlling.Deactivate();
		//transform.position = GameController.Instance.PlayerStartPosition.position;
	}

	private void OnAfterDeath()
	{
		transform.position = GameController.Instance.PlayerStartPosition.position;
	}
	#endregion
}
