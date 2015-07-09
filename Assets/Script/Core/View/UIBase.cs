using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 功能：
/// 1.根據資源地址，創建實例，主資源和child資源.如果主資源不存在則生成一個空的gameobject,child資源會放主資源實例下，child資源可以有0到多個。
/// 注：
/// 1）OnInitSkinFront中需要設置主資源（mainSkinPath）和child資源（AddChildSkinPath）
/// 2）資源默認是resource下的資源。如是assetBundle.需在OnInitSkinFront中修改skinSrcType
///
/// 2.主資源及主資源下所有的boxCollider,如果boxCollider.gameobject.name已"Btn"開頭。則會觸發OnClick(target)
/// 3.在2的基礎上，實現點擊時間間隔約束功能，默認打開，如需開啟設置clickIntervalRestrict
/// 4.SetColliderEnabled 設置該對像下所有boxcollider是否可以交互
/// 5.OnSecondUpdate，每秒刷新。默認為不開啟，如需開啟，設置secondUpdateEnabled = true
///
/// 6.生命週期OnInitFront->OnInitSkinFront->OnInitSkin->OnInitSkinDone->OnInitDone->OnSecondUpdate-OnUpdate-OnFixedUpdate-OnLateUpdate->（OnDestroyFront->OnOnDestroy->OnDestroyDone）
/// 注：如果沒有調用Init,這些邏輯不會執行.除（）內刪除相關
/// </summary>
public class UIBase : MonoBehaviour {

	///<summary>
	///Second Update Enabled = false; 
	///</summary>
	protected bool secondUpdateEnabled = false;

	///<summary>
	///Second Update Time 
	///</summary>
	private float secondUpdateTime = 0.0f;

	private GameObject _skin;
	///<summary>
	///Skin
	///</summary>
	public GameObject skin{
		get{
			return _skin;
		}
	}

	///<summary>
	/// Main Skin
	///</summary>
	private string mainSkinPath;
	///<summary>
	///Skin's transform
	///</summary>
	public Transform skinTransform{
		get {
			if (skin != null){
				return skin.transform;
			}

			return null;
		}
	}

	///<summary>
	///all boxCollider 
	///</summary>
	private List<Collider> colliderList = new List<Collider>();

	private bool _initDoneFlag = false;
	///<summary>
	///Init Done Flag 
	///</summary>
	public bool initDoneFlag{
		get{
			return _initDoneFlag;
		}
	}

	public void OnDestroy(){
		OnDestroyFront();

		OnOnDestroy();
		colliderList.Clear();
		colliderList = null;
		StopAllCoroutines();

		OnDestroyDone();
	}

	void Update(){
		if(! initDoneFlag){
			return;
		}

		OnUpdate ();

		if (secondUpdateEnabled){
			float delta = Time.deltaTime;
			secondUpdateTime += delta;
			if (secondUpdateTime > 1.0f){
				secondUpdateTime -= 1.0f;
				OnSecondUpdate ();
			}
		}
	}

	void FixedUpdate(){
		if(!initDoneFlag){
			return;
		}
		OnFixedUpdate ();
	}

	void LateUpdate(){
		if(!initDoneFlag){
			return;
		}
		OnLateUpdate ();
	}

	protected void Init(){
		if(initDoneFlag){
			return;
		}
		OnInitFront();
		OnInitSkinFront();
		OnInitSkin();
		OnInitSkinDone();

		Profiler.BeginSample("SpriteBase:UIEventListener BoxCollider");
		Collider[] triggers = this.GetComponentsInChildren<Collider>(true);

		for(int i = 0, max = triggers.Length; i < max; i++){
			Collider trigger = triggers[i];
			//Button that using Btn for naming can trigger OnClick only 
			if (trigger.gameObject.name.StartsWith("Btn") == true){
				UIEventListener listener = UIEventListener.Get (trigger.gameObject);
				listener.onClick = Click;
			}

			colliderList.Add (trigger);
			//set over scale
			UIButtonScale btnScale = trigger.gameObject.GetComponent<UIButtonScale>();
			if (btnScale != null){
				btnScale.hover = Vector3.one;
			}
		}

		Profiler.EndSample();
		Profiler.BeginSample("SpriteBase: OnInitDone");
		OnInitDone();
		Profiler.EndSample ();
		_initDoneFlag = true;
	}

	/// <summary>
	/// Before Init 
	/// </summary>
	protected virtual void OnInitFront() { }
	/// <summary>
	/// Before Init skin
	/// </summary>
	protected virtual void OnInitSkinFront(){ }
	/// <summary>
	/// Init Skin
	/// </summary>
	protected virtual void OnInitSkin(){
		if(mainSkinPath != null){
			_skin = LoadSrc(mainSkinPath);
		}
		else{
			_skin = new GameObject("Skin");
		}

		skin.transform.parent = this.transform;
		//skin.transform.localPosition = Vector3.zero;
		skin.transform.localEulerAngles = Vector3.zero;
		skin.transform.localScale = Vector3.one;
	}


	/// <summary>
	/// Init Skin Done
	/// </summary>
	protected virtual void OnInitSkinDone() { }
	/// <summary>
	/// Init Done
	/// </summary>
	protected virtual void OnInitDone(){ }

	/// <summary>
	/// Base on SecondUpdate
	/// </summary>
	protected virtual void OnSecondUpdate(){ }
	protected virtual void OnUpdate() { }
	protected virtual void OnFixedUpdate() { }
	protected virtual void OnLateUpdate() { }
	protected virtual void OnDestroyFront() { }
	protected virtual void OnOnDestroy() { }
	protected virtual void OnDestroyDone() { }

	/// <summary>
	/// OnClcik
	/// </summary>
	/// <param name="target"></param>
	protected virtual void OnClick(GameObject target) { }

	protected void Click(GameObject target){
		if (initDoneFlag){
			OnClick (target);
		}
	}

	/// <summary>
	/// Set Main Skin
	/// </summary>
	/// <param name="path"></param>
	protected void SetMainSkinPath(string path){
		if (initDoneFlag){
			Debug.LogWarning ("Init Finished, Please InitSkinFornt to set main skin" + path);
		}
		mainSkinPath = path;
	}

	/// <summary>
	/// Load Asset
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	protected GameObject LoadSrc(string path)
	{
		return ResourceMgr.GetInstance().CreateGameObject(path,false);
	}
	
	
	
}
