using UnityEngine;

namespace Assets.Scripts.GameLogic.Game
{
	public class ResourceController
	{

		#region Constants

		public const string PlayerPlanePrefabsPath = "Prefabs/Fighters/PlayerFighters";
		private const int DefaultItemCount = 5;

		#endregion

		#region Variables

		#endregion

		#region Load recources

		public static void LoadResources<T>(string loadPathPrefab, int count = DefaultItemCount, Transform parent = null) where T : Component
		{
			Object[] items = Resources.LoadAll(loadPathPrefab);

			Debug.Log("ResourceController.LoadResources - Loading item, was loaded " + items.Length + "");

			foreach (Object item in items)
			{
				GameObject itemNew = GameObject.Instantiate(item) as GameObject;

				if (itemNew == null)
				{
					Debug.LogError("ResourceController.LoadResources - Can't instatiate item object");
					continue;
				}

				T itemT = itemNew.GetComponent<T>();

				if (itemT == null)
				{
					Debug.LogError("ResourceController.LoadResources - Can't get component item from game object");
					continue;
				}

				if (parent != null)
				{
					itemT.transform.parent = parent;
				}

				//Pool.Push(itemT as PoolItem);

				Pool.AddMassive(itemT as PoolItem, count);
			}

		}

		private static T CreatePoolItem<T>(string itemPrefabPath, Transform parent = null) where T : Component
		{

			GameObject item = Resources.Load(itemPrefabPath) as GameObject;
			GameObject itemInScene = GameObject.Instantiate(item) as GameObject;

			if (itemInScene != null)
			{
				T itemInSceneObject = itemInScene.GetComponent<T>();

				if (parent != null)
				{
					itemInSceneObject.transform.parent = parent;
				}

				Pool.Push(itemInSceneObject as PoolItem);

				return itemInSceneObject;
			}

			return null;
		}


		public static T GetItemFromPool<T>(string itemPrefabPath) where T : Component
		{
			GameObject itemGo = null;
			T itemFromPool = default(T);

			itemGo = Resources.Load(itemPrefabPath) as GameObject;



			if (itemGo != null)
			{
				itemFromPool = Pool.Pop(itemGo.GetComponent<T>() as PoolItem) as T ??
				               CreatePoolItem<T>(itemPrefabPath).GetComponent<T>();
			}
			 
			return itemFromPool;
		}

		#region WeaponsBehaviour

		public static WeaponsBehaviour GetBulletFromPool(string itemPrefabPath, Transform parent = null)
		{
			GameObject itemGo = null;
			WeaponsBehaviour itemFromPool = default(WeaponsBehaviour);

			itemGo = Resources.Load(itemPrefabPath) as GameObject;

			if (itemGo != null)
			{
				itemFromPool = Pool.Pop(itemGo.GetComponent<WeaponsBehaviour>() as PoolItem) as WeaponsBehaviour ??
							   CreatePoolItem<WeaponsBehaviour>(itemPrefabPath).GetComponent<WeaponsBehaviour>();
			}

			return itemFromPool;
		}

		#endregion

		public static PlaneControlling GetPlaneFromPool(string itemPrefabPath, Transform parent = null)
		{
			GameObject itemGo = null;
			PlaneControlling itemFromPool = default(PlaneControlling);

			itemGo = Resources.Load(itemPrefabPath) as GameObject;

			if (itemGo != null)
			{
				itemFromPool = Pool.Pop(itemGo.GetComponent<PlaneControlling>() as PoolItem) as PlaneControlling ??
							   CreatePoolItem<PlaneControlling>(itemPrefabPath, parent).GetComponent<PlaneControlling>();
				
				if (parent != null && itemFromPool.transform.parent == null)
				{
					itemFromPool.transform.parent = parent;
				}
			}

			return itemFromPool;
		}

		#endregion
	}
}