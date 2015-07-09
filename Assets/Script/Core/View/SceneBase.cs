using UnityEngine;
using System.Collections;
using System;

public class SceneBase : LayerBase {

	protected bool _cache = false;
	/// <summary>
	/// cache
	/// false -> destroy when close
	/// </summary>
	
	public bool cache{
		get{
			return _cache;
		}
	}

	protected SceneType _type;
	/// <summary>
	/// Scene ID
	/// </summary>
	public SceneType type{
		get{
			return _type;
		}
	}

	protected object[] _sceneArgs;
	/// <summary>
	/// Record init args for scene
	/// </summary>

	public object[] sceneArgs{
		get{
			return _sceneArgs;
		}
	}

	///<summary>
	/// Init Scene 
	///</summary>
	/// <param name="sceneArgs"></param>
	public virtual void OnInit(params object[] sceneArgs){
		_sceneArgs = sceneArgs;
		Init ();
	}

	/// <summary>
	/// Reset Data
	/// </summary>
	/// <param name="sceneArgs"></param>
	public virtual void OnResetArgs(params object[] sceneArgs){
		_sceneArgs = sceneArgs;
	}

	/// <summary>
	/// On Showing
	/// </summary>
	public virtual void OnShowing() { }
	/// <summary>
	/// Showed
	/// </summary>
	public virtual void OnShowed() { }
	/// <summary>
	/// Hiding
	/// </summary>
	public virtual void OnHiding() { }
	/// <summary>
	/// Hided
	/// </summary>
	public virtual void OnHided() { }

}
	
	/// <summary>
	/// define Scene Type
	/// 有一個特殊要求，SceneType.tostring等於該場景的classname。原因是：在切換場景時，參數為scenetype,此時需要根據string反射得到該className對應的對象。
	/// </summary>
	public enum SceneType
	{
		/// <summary> TestScene </summary>
		SceneTest,
		SceneTest2,
		SceneLogin,
		SceneLoading,
		SceneCreateRote,
		SceneMain,
		SceneFriends,
		SceneMail
		//SceneLogins,
	}
