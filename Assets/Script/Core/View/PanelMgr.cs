using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Panel Management
/// </summary>
public class PanelMgr {

	#region initialization
	protected static PanelMgr mInstance;
	public static bool hasInstance{
		get{
			return mInstance != null;
		}
	}
	
	public static PanelMgr GetInstance(){
		if (!hasInstance){
			mInstance = new PanelMgr();
		}
		return mInstance;
	}
	private PanelMgr(){
		panels = new Dictionary<PanelName, PanelBase>();
	}
	#endregion

	#region define data
	/// <summary>
	/// Show Style
	/// </summary>
	public enum PanelShowStyle{
		Nomal,
		CenterScaleBigNomal,
		UpToSlide,
		DownToSlide,
		LeftToSlide,
		RightToSlide,
	}

	/// <summary>
	/// Mask Style
	/// </summary>
	public enum PanelMaskStyle
	{
		/// <summary>
		/// No Background
		/// </summary>
		None,
		/// <summary>
		/// 半透明背景
		/// </summary>
		BlackAlpha,
		/// <summary>
		/// No Background, but has BOX close 
		/// </summary>
		Alpha,
	}

	/// <summary>
	/// Save Current Panel
	/// </summary>
	public Dictionary<PanelName, PanelBase> panels;

	#endregion

	/// <summary>
	/// Current Panel
	/// </summary>
	public PanelBase current;
	
	public void Destroy() { }

	/// <summary>
	/// Show Panel
	/// </summary>
	/// <param name="sceneType"></param>
	/// <param name="sceneArgs"></param>
	public void ShowPanel(PanelName panelName, params object[] sceneArgs){
		
		if (panels.ContainsKey(panelName)){
			Debug.LogError("Panel has been shown");
			current = panels[panelName];
			current.gameObject.SetActive(false);
			current.OnInit(sceneArgs);
			current.OnShowing();
			LayerMgr.GetInstance().SetLayer(current.gameObject, LayerType.Panel);
		}
		else{
			GameObject go = new GameObject(panelName.ToString());
			//sceneType.tostring = classname in the current scene
			current = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(go, "Assets/Script/Core/View/PanelMgr.cs (94,14)", panelName.ToString()) as PanelBase; 
			current.gameObject.SetActive(false);
			current.OnInit(sceneArgs);
			panels.Add(current.type, current);
			current.OnShowing();
			LayerMgr.GetInstance().SetLayer(current.gameObject, LayerType.Panel);
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			go.transform.localRotation = Quaternion.identity;
			MaskStyle(current);
		}
		StartShowPanel(current, current.PanelShowStyle,true);
	}

	/// <summary> </summary>
	private void StartShowPanel(PanelBase go, PanelShowStyle showStyle, bool isOpen){
		switch(showStyle)
		{
		case PanelShowStyle.Nomal:
			ShowNomal(go, isOpen);
			break;
		case PanelShowStyle.CenterScaleBigNomal:
			CenterScaleBigNomal(go, isOpen);
			break;
		case PanelShowStyle.LeftToSlide:
			LeftAndRightToSlide(go, false, isOpen);
			break;
		case PanelShowStyle.RightToSlide:
			LeftAndRightToSlide(go, true, isOpen);
			break;
		case PanelShowStyle.UpToSlide:
			TopAndDownToSlide(go, true, isOpen);
			break;
		case PanelShowStyle.DownToSlide:
			TopAndDownToSlide(go, false, isOpen);
			break;
		}
	}
		
	#region 显示方式
	/// <summary> Default </summary>
	void ShowNomal(PanelBase go, bool isOpen){
		if (isOpen)
{
			current.gameObject.SetActive(true);
			current.OnShowed();
		}
		else DestroyPanel(go.type);
	}
	/// <summary> CenterScaleBig </summary>
	void CenterScaleBigNomal(PanelBase go, bool isOpen){
		TweenScale ts = go.gameObject.GetComponent<TweenScale>();
		if (ts == null)ts = go.gameObject.AddComponent<TweenScale>();
		//
		ts.from = Vector3.zero;
		ts.to = Vector3.one;
		ts.duration = go.OpenDuration;
		ts.method = UITweener.Method.EaseInOut;
		ts.SetOnFinished(() =>
		                 {
			if(isOpen) go.OnShowed();
			else DestroyPanel(go.type);
		});
		go.gameObject.SetActive(true);
		if (!isOpen) ts.Play(isOpen);
	}
	/// <summary> LeftAndRightToSlide </summary>
	void LeftAndRightToSlide(PanelBase go, bool isRight,bool isOpen){
		TweenPosition tp = go.gameObject.GetComponent<TweenPosition>();
		if (tp == null) tp = go.gameObject.AddComponent<TweenPosition>();
		tp.from = isRight == true ? new Vector3(640, 0, 0) : new Vector3(-640, 0, 0);
		tp.to = Vector3.zero;
		tp.duration = go.OpenDuration;
		tp.method = UITweener.Method.EaseInOut;
		tp.SetOnFinished(() =>
		                 {
			if (isOpen) go.OnShowed();
			else DestroyPanel(go.type);
		});
		go.gameObject.SetActive(true);
		if (!isOpen) tp.Play(isOpen);
	}
	/// <summary> opAndDownToSlide </summary>
	void TopAndDownToSlide(PanelBase go, bool isTop,bool isOpen){
		TweenPosition tp = go.gameObject.GetComponent<TweenPosition>();
		if (tp == null) tp = go.gameObject.AddComponent<TweenPosition>();
		//
		tp.from = isTop == true ? new Vector3(0, 640, 0) : new Vector3(0, -640, 0);
		tp.to = Vector3.zero;
		tp.duration = go.OpenDuration;
		tp.method = UITweener.Method.EaseInOut;
		tp.SetOnFinished(() =>
		                 {
			if (isOpen) go.OnShowed();
			else DestroyPanel(go.type);
		});
		go.gameObject.SetActive(true);
		if (!isOpen) tp.Play(isOpen);
	}
	
	#endregion

	#region Mask Style
	
	void MaskStyle(PanelBase go){
		float alpha = 1;
		switch(go.PanelMaskStyle){
		case PanelMaskStyle.Alpha:
			alpha = 0.001f;
			break;
		case PanelMaskStyle.BlackAlpha:
			alpha = 0.5f;
			break;
		}
		GameObject mask = ResourceMgr.GetInstance().CreateGameObject("Public/prefab/PanelMask", true);
		mask.transform.parent = go.gameObject.transform;
		mask.transform.localPosition = Vector3.zero;
		mask.transform.localEulerAngles = Vector3.zero;
		mask.transform.localScale = Vector3.one;
		
		UIPanel panel = mask.GetComponent<UIPanel>();
		panel.alpha = alpha;
		LayerMgr.GetInstance().SetLayer(go.gameObject, LayerType.Panel);
	}
	
	#endregion

	/// <summary>
	/// Hide Panel
	/// </summary>
	public void HidePanel(PanelName type){
		if (panels.ContainsKey(type)){
			PanelBase pb = null;
			pb = panels[type];
			StartShowPanel(pb, pb.PanelShowStyle, false);
		}
		else Debug.LogError("Panel doesn't exist");
	}
	
	
	/// <summary>
	/// Force Destroy Panel
	/// </summary>
	/// <param name="panel"></param>
	public void DestroyPanel(PanelName type){
		if (panels.ContainsKey(type)){
			PanelBase pb = panels[type];
			if (!pb.cache){
				GameObject.Destroy(pb.gameObject);
				panels.Remove(type);
			}
		}
	}

}
