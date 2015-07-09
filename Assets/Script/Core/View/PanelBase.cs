using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelBase : LayerBase {

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

	protected PanelName _type;
	/// <summary>
	/// Panel ID
	/// </summary>
	public PanelName type
	{
		get
		{
			return _type;
		}
	}

	/// <summary> Click bg close Panel </summary>
	protected bool _isClickMaskColse = true;

	public bool isClickMaskColse
	{
		get
		{
			return _isClickMaskColse;
		}
		set
		{
			_isClickMaskColse = value;
		}
	}

	/// <summary> Panel Show Style </summary>
	protected PanelMgr.PanelShowStyle _showStyle = PanelMgr.PanelShowStyle.CenterScaleBigNomal;

	public PanelMgr.PanelShowStyle PanelShowStyle
	{
		get
		{
			return _showStyle;
		}
	}

	/// <summary> Panel Mask Style </summary>
	protected PanelMgr.PanelMaskStyle _maskStyle = PanelMgr.PanelMaskStyle.BlackAlpha;
	public PanelMgr.PanelMaskStyle PanelMaskStyle
	{
		get
		{
			return _maskStyle;
		}
	}

	/// <summary> Open Duration </summary>
	protected float _openDuration = 0.2f;
	public float OpenDuration
	{
		get
		{
			return _openDuration;
		}
	}

	protected object[] _panelArgs;
	/// <summary>
	/// Record init Panel args 
	/// </summary>
	public object[] panelArgs
	{
		get
		{
			return _panelArgs;
		}
	}

	/// <summary>
	/// Init Panel
	/// </summary>
	/// <param name="panelArgs">面板参数</param>
	public virtual void OnInit(params object[] panelArgs)
	{
		_panelArgs = panelArgs;
		Init();
	}

	/// <summary>
	/// On Showing
	/// </summary>
	public virtual void OnShowing()
	{
		
	}

	/// <summary>
	/// ResetArgs
	/// </summary>
	/// <param name="panelArgs"></param>
	public virtual void OnResetArgs(params object[] panelArgs)
	{
		_panelArgs = panelArgs;
	}

	/// <summary>
	/// after showing
	/// </summary>
	public virtual void OnShowed()
	{
		
	}

	/// <summary>
	/// Close
	/// </summary>
	protected virtual void Close()
	{
		PanelMgr.GetInstance().HidePanel(type);
	}
	/// <summary>
	/// Close Immediately
	/// </summary>
	protected virtual void CloseImmediate()
	{
		PanelMgr.GetInstance().DestroyPanel(type);
	}
	
	public virtual void OnHideFront()
	{
		_cache = false;
	}
	
	public virtual void OnHideDone()
	{
		
	}

}
	/// <summary>
	/// List of Panel Name
	/// </summary>
	public enum PanelName
	{
		none = 0,
		PanelTest,
		PanelLogin,
		PanelUserInfo,
	}
