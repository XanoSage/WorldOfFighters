using Assets.Scripts.GameLogic.Weapons;
using Assets.Scripts.GameLogic.Helper;
using UnityEngine;
using System.Collections;

public class WeaponsBehaviour : PoolItem, ICooldownTime
{

	#region Variables

	[SerializeField] private WeaponSimple _simple;

	private Weapons _weaponModel;

	private Rigidbody2D _rigidbody;

	private float _blowUpCounter = 0;

	private Vector3 _direction = Vector3.zero;

	private bool _isInit;

	public WeaponsType Type
	{
		get { return _simple.Type; }
	}

	public OwnerInfo Owner
	{
		get { return _simple.Owner; }
	}

	public Weapons Weapon
	{
		get { return _weaponModel; }
	}

	#endregion

	#region MonoBehaviour Action

	private void Awake()
	{
		_weaponModel = Weapons.Create(_simple.Type, _simple.Speed, _simple.CooldownTimer, _simple.BlowUpTime, _simple.Damage,
		                              _simple.Owner);
	}

	// Use this for initialization
	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();

		if (_rigidbody == null)
		{
			throw new MissingComponentException("WeaponsBehaviour.Start - can't find RigidBody component");
		}

		_isInit = false;

	}

	// Update is called once per frame
	private void Update()
	{

	}

	private void FixedUpdate()
	{
		if (GameController.Instance != null && GameController.Instance.State != GameModel.GameState.Playing)
			return;

		UpdatePosition();
		UpdateCounter();
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log("WeaponsBehaviour.OnCollisionEnter2D - collision name: " + coll.transform.name);
	}

	#endregion

	#region Actions

	private void TimetoBlowUp()
	{
		GameController.Instance.RemoveBullet(this);
		BulletDestroy();
	}

	private void UpdatePosition()
	{
		if (_rigidbody == null)
			return;

		if (_weaponModel == null)
			return;

		_rigidbody.transform.Translate(_direction*_weaponModel.Speed);
	}

	private void UpdateCounter()
	{
		if (_weaponModel == null)
			return;

		if (_blowUpCounter < _weaponModel.TimeToBlowUp)
		{
			_blowUpCounter += Time.deltaTime;
		}

		if (_blowUpCounter >= _weaponModel.TimeToBlowUp)
		{
			_blowUpCounter = 0;
			TimetoBlowUp();
		}
	}

	public void Init(Transform parent, Vector3 startPosition, Quaternion startRotation, Vector3 direction)
	{
		transform.parent = parent;
		transform.position = startPosition;
		transform.rotation = startRotation;
		_direction = direction;

		_blowUpCounter = 0;

		_isInit = true;

		GameController.Instance.AddBullet(this);
	}

	public void BulletDestroy()
	{
		_isInit = false;
		_blowUpCounter = 0;
		Deactivate();
		Pool.Push(this);
	}

	#endregion

	#region PoolItem Implementation

	public override bool EqualsTo(PoolItem item)
	{
		if (!(item is WeaponsBehaviour))
			return false;

		WeaponsBehaviour weapons = item as WeaponsBehaviour;

		if (weapons.Type != Type)
			return false;

		return true;
	}

	public override void Activate()
	{
		base.Activate();

		gameObject.SetActive(true);
	}

	public override void Deactivate()
	{
		base.Deactivate();

		gameObject.SetActive(false);
	}

	#endregion

	#region ICooldown implementation

	public float CooldownTime
	{
		get { return _weaponModel == null ? _simple.CooldownTimer : _weaponModel.CooldownTime; }
		set { }
	}

	#endregion
}
