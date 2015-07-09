using UnityEngine;
using System.Collections;
using System;

public class LayerBase : UIBase {

	///<summary>
	///Default z space between child 
	///</summary>
	public const float DEFAULT_Z_SPACE_BETWEEN_CHILD = 20f;

	/// <summary>
	/// Z space
	/// </summary>
	/// <returns></returns>
	public virtual float zSpace
	{
		get
		{
			return DEFAULT_Z_SPACE_BETWEEN_CHILD;
		}
	}
}
