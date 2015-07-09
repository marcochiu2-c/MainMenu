using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SceneMgr {

	#region initialization
	protected static SceneMgr mInstance;
	public static bool hasInstance{
		get{
			return mInstance  != null;
		}
	}

	public static SceneMgr GetInstance(){
		if (!hasInstance){
			mInstance = new SceneMgr();
		}
		return mInstance;
	}
	#endregion

	public delegate void OnSwitchingScene (SceneType type);

	/// <summary>
	/// Handler for Switching Scene 
	/// </summary>
	public OnSwitchingScene OnSwitchingSceneHandler;
	/// <summary>
	/// Save current Scene
	/// </summary>
	public Dictionary<SceneType, SceneBase> scenes;
	/// <summary>
	/// Scurrent Scene
	/// </summary>
	public SceneBase current;
	/// <summary>
	/// Switch Recoder
	/// </summary>
	private List<SwitchRecorder> switchRecorders;
	/// <summary>
	/// Mian Scene
	/// </summary>
	private const SceneType mainSceneType = SceneType.SceneTest;

	private SceneMgr(){
		scenes = new Dictionary<SceneType, SceneBase>();
		switchRecorders = new List<SwitchRecorder>();
	}

	public void Destory(){
		OnSwitchingSceneHandler = null;

		switchRecorders.Clear ();
		switchRecorders = null;

		scenes.Clear ();
		scenes = null;
	}

	/// <summary>
	/// Siwtch Scene
	/// </summary>
	/// <param name="sceneType"></param>
	/// <param name="sceneArgs"></param>
	public void SwitchingScene(SceneType sceneType, params object[] sceneArgs){
		if(current != null){
			if (sceneType == current.type){
				Debug.LogError ("try to switch scene tos current scene" + sceneType.ToString());
				return;
			}
		}

		//enter mainScene, clear switchRecorders
		if (sceneType == mainSceneType){
			switchRecorders.Clear ();
		}

		//switch Record 
		switchRecorders.Add (new SwitchRecorder(sceneType, sceneArgs));

		HideCurrentScene();
		ShowScene(sceneType, sceneArgs);
		if (OnSwitchingSceneHandler != null){
			OnSwitchingSceneHandler(sceneType);
		}
	}

	/// <summary>
	/// SwitchingToPrevScene
	/// </summary>
	public void SwitchingToPrevScene()
	{
		if(switchRecorders.Count < 2)
		{
			Debug.LogWarning("Switching to Prev. Scene, No record for the Prev. Scene");
			return;
		}
		SwitchRecorder sr = switchRecorders[switchRecorders.Count - 2];
		//Switch to prev scene and remove the 'current' scene and prev. scene
		switchRecorders.RemoveRange(switchRecorders.Count - 2, 2);
		SwitchingScene(sr.sceneType, sr.sceneArgs);
	}

	/// <summary>
	/// Show assigned scene
	/// </summary>
	private void ShowScene(SceneType sceneType, params object[] sceneArgs){
		if(scenes.ContainsKey(sceneType)){
			current = scenes[sceneType];
			current.OnShowing();
			current.OnResetArgs(sceneArgs);
			NGUITools.SetActive(current.gameObject, true);
			current.OnShowed();
		}
		else{
			GameObject go = new GameObject(sceneType.ToString());
			//sceneType.tostring = classname for current scene
			current = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(go, "Assets/Script/Core/View/SceneMgr.cs (120,14)", sceneType.ToString()) as SceneBase; 
			current.OnInit(sceneArgs);
			scenes.Add(current.type, current);
			current.OnShowing();
			LayerMgr.GetInstance().SetLayer(current.gameObject, LayerType.Scene);
			go.transform.localScale = Vector3.one;
			go.transform.localRotation = Quaternion.identity;
			current.OnShowed();
		}
	}

	/// <summary>
	/// Hide Current Scene
	/// </summary>
	private void HideCurrentScene(){
		if(current != null){
			current.OnHiding();
			NGUITools.SetActive(current.gameObject, false);
			current.OnHided();
			
			if (! current.cache){
				scenes.Remove(current.type);
				GameObject.Destroy(current.gameObject);
			}
		}
	}


	/// <summary>
	/// Record
	/// </summary>
	internal struct SwitchRecorder
	{
		internal SceneType sceneType;
		internal object[] sceneArgs;
		
		internal SwitchRecorder(SceneType sceneType, params object[] sceneArgs)
		{
			this.sceneType = sceneType;
			this.sceneArgs = sceneArgs;
		}
	}

}
