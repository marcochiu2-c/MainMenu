using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

///<summary>
/// model management 
/// </summary>
public class ModelMgr : MonoBehaviour {

	protected static ModelMgr mInstance;
	public static bool hasInstance{
		get{
			return mInstance != null;
		}
	}

	public static ModelMgr GetInstances(){
		if(!hasInstance){
			mInstance = new ModelMgr();
		}
		return mInstance;
	}

	///<summary>
	/// save all the model
	/// </summary>
	private Dictionary<string, BaseModel> dictionary;

	private ModelMgr(){
		dictionary = new Dictionary<string, BaseModel>();
	}

	///<summary>
	/// get model
	/// </summary>
	public T GetModel<T>() where T : BaseModel{
		Type type = typeof(T);
		if(dictionary.ContainsKey(type.Name)){
			return dictionary[type.Name] as T;
		}

		T model = System.Activator.CreateInstance(type) as T;
		dictionary.Add (type.Name, model);
		return model;
	}

	///<summary>
	/// Clear
	/// </summary>
	public void Destroy(){
		Clear();
		dictionary = null;
	}

	public void Clear(){
		foreach (BaseModel m in dictionary.Values){
			m.Destroy();
		}
		dictionary.Clear ();
	}
}
