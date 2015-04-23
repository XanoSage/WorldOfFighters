using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class AiPathHelper : MonoBehaviour
{

	#region Variables

	[Serializable]
	public class PathPair
	{
		[SerializeField] public int Index;
		[SerializeField] public List<Transform> AiPath;
	}


	[SerializeField] private List<PathPair> _pathPairs;

	#endregion

	#region MonoBehaviour actions


	// Use this for initialization
	private void Start()
	{

	}

	// Update is called once per frame
	private void Update()
	{

	}

	#endregion

	public List<Transform> GetAiPath()
	{
		int index = Random.Range(0, _pathPairs.Count);
		return _pathPairs[index].AiPath;
	}
}
