using UnityEngine;
using System.Collections;

public class ResourceMgr : MonoBehaviour {

	#region initialization
	private static ResourceMgr mInstance;
	
	/// <summary>
	/// Get Instance
	/// </summary>
	/// <returns></returns>
	public static ResourceMgr GetInstance()
	{
		if (mInstance == null)
		{
			mInstance = new GameObject("_ResourceMgr").AddComponent<ResourceMgr>();
		}
		return mInstance;
	}
	private ResourceMgr()
	{
		hashtable = new Hashtable();
	}
	#endregion

	/// <summary> Asset Cache Container </summary>
	private Hashtable hashtable;
	/// <summary>
	/// Load Asset
	/// </summary>
	/// <typeparam name="T">Asset Type</typeparam>
	/// <param name="path">Asset Path</param>
	/// <param name="cacheAsset">Caache Asset or not</param>
	/// <returns></returns>
	public T Load<T>(string path, bool cache) where T : UnityEngine.Object{
		if (hashtable.Contains (path)){
			return hashtable[path] as T;
		}

		Debug.Log(string.Format("Load asset from resource folder, path:{0}, cache{1}", path, cache));
		T assetObj = Resources.Load<T>(path);
		if (assetObj == null){
			Debug.LogWarning ("Cannot find the asset" + path);
		}
		if (cache){
			hashtable.Add (path, assetObj);
			Debug.Log ("Asset has been cached, Resources' path = " + path);
		}

		return assetObj;
	}

	/// <summary>
	/// Create Resource for GameObject
	/// </summary>
	/// <param name="path">Asset path</param>
	/// <param name="cacheAsset">aache Asset or not</param>
	/// <returns></returns>
	public GameObject CreateGameObject(string path, bool cache){
		UnityEngine.GameObject assetObj = Load<GameObject> (path, cache);
		GameObject go = UnityEngine.Object.Instantiate(assetObj) as GameObject;
		if(go == null){
			Debug.LogWarning("Create Resource fail" + path);
		}

		return go;
	}

}
