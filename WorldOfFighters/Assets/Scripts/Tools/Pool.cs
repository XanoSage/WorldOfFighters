using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour
{

	public static Pool instance;
	private static List<PoolItem> items;
	public static Transform pooledItemsParent;
	public static bool IsMultiplayer;

	#region Initialization

	[System.Serializable]
	public class ItemCountPair
	{
		public PoolItem itemPrefab;
		public int count;
	}

	public List<ItemCountPair> startItemsDescription;

	private void Awake()
	{
		instance = this;
		InitFirst();
	}

	public void InitFirst()
	{
		pooledItemsParent = transform;

		items = new List<PoolItem>();

		foreach (ItemCountPair pair in startItemsDescription)
		{
			for (int i = 0; i < pair.count; i++)
				InstantiateItem(pair.itemPrefab);
		}
	}


	public static void AddMassive(PoolItem item, int count)
	{
		for (int i = 0; i < count; i++)
		{
			InstantiateItem(item);
		}
	}

	#endregion


	private static int GetObjectIndex(PoolItem itemPrefab)
	{


		for (int i = 0; i < items.Count; i++)
		{

			if (items[i].EqualsTo(itemPrefab))
			{

				return i;
			}

		}

		InstantiateItem(itemPrefab);
		return items.Count - 1;
	}



	private static void InstantiateItem(PoolItem itemPrefab)
	{
		PoolItem newItem = null;

		newItem = Instantiate(itemPrefab) as PoolItem;

		if (newItem != null)
			Pool.Push(newItem);
	}

	public static PoolItem Pop(PoolItem itemPrefab)
	{


		int index = GetObjectIndex(itemPrefab);

		if (index == -1)
		{
			Debug.LogError(string.Format("POP. No such object in pool: {0}", itemPrefab));
			return null;
		}

		PoolItem item = items[index];
		items.RemoveAt(index);
		item.Activate();

		return item;
	}

	public static void Push(PoolItem item)
	{
		item.Deactivate();
		items.Add(item);
	}

	public static void UnloadItem()
	{
		for (int i = 0; i != items.Count; i++)
		{
			Destroy(items[i].gameObject);
			items.RemoveAt(i);
			i--;
		}
	}
}
