using Assets.Scripts.GameLogic.Plane;
using UnityEngine;
using System.Collections;

public class CheckScreenBordrer : MonoBehaviour
{

	#region Variables
	private Vector3 _minBorder;
	private Vector3 _maxBorder;

	private Rigidbody2D _rigidbody;

	#endregion

	#region MonoBehaviours Actions
	
	// Use this for initialization
	void Start ()
	{

		_rigidbody = GetComponent<Rigidbody2D>();

		if (_rigidbody == null)
		{
			throw new MissingComponentException("CheckScreenBorder.Start - can't find RigidBody2d component");
		}

		InitBoundsOfTheScreen();
	}
	
	// Update is called once per frame
	void Update () {
		CheckBorders();
	}
	#endregion

	#region Actions

	private void InitBoundsOfTheScreen()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		Vector3 scrVectorMin = new Vector3(spriteRenderer.sprite.rect.width / 4.5f, spriteRenderer.sprite.rect.height / 4.5f, Camera.main.transform.position.z);
		Vector3 scrVectorMax = new Vector3(Screen.width - spriteRenderer.sprite.rect.width / 4.5f,
										   Screen.height - spriteRenderer.sprite.rect.height / 4.5f, Camera.main.transform.position.z);

		_minBorder =
			Camera.main.ScreenToWorldPoint(scrVectorMin);

		_maxBorder =
			Camera.main.ScreenToWorldPoint(scrVectorMax);

		Debug.Log(string.Format("_minBorder: {0}, _maxBorder: {1}, screen: {2}x{3}, scrMin:{4}, scrMax:{5}", _minBorder,
								_maxBorder, Screen.width, Screen.height, scrVectorMin, scrVectorMax));
	}

	private void CheckBorders()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		float x = _rigidbody.transform.position.x;
		float y = _rigidbody.transform.position.y;

		bool isNeedCorrectPos = false;

		if (spriteRenderer == null)
			return;

		if (x > _minBorder.x)
		{
			x = _minBorder.x;
			isNeedCorrectPos = true;
		}

		else if (x < _maxBorder.x)
		{
			x = _maxBorder.x;
			isNeedCorrectPos = true;
		}

		if (y > _minBorder.y)
		{
			y = _minBorder.y;
			isNeedCorrectPos = true;
		}
		else if (y < _maxBorder.y)
		{
			y = _maxBorder.y;
			isNeedCorrectPos = true;
		}

		if (isNeedCorrectPos)
		{
			_rigidbody.transform.position = new Vector3(x, y, _rigidbody.transform.position.z);
		}

	}

	#endregion
}
