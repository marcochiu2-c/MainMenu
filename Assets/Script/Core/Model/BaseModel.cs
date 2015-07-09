using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Base model
/// dispatcher
/// </summary>
public class BaseModel : MonoBehaviour {


	#region ValueUpdateEvent
	///<summary>
	/// define Value pdate Event
	/// </summary>
	public event EventHandler<ValueUpdateEventArgs> ValueUpdateEvent;


	/// <summary>
	/// Dispatch Value Update Event
	/// </summary>
	/// <typeparam name="key">Event key</typeparam>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	protected void DispatchValueUpdateEvent(string key, object oldValue, object newValue){
		EventHandler<ValueUpdateEventArgs> handler = ValueUpdateEvent;
		if(handler != null){
			handler(this, new ValueUpdateEventArgs(key, oldValue, newValue));
		}
	}

	/// <summary>
	/// Dispatch Value Update Event
	/// </summary>
	protected void DispatchValueUpdateEvent(ValueUpdateEventArgs args){
		EventHandler<ValueUpdateEventArgs> handler = ValueUpdateEvent;
		if(handler != null){
			handler(this, args);
		}
	}
	#endregion

	#region ModelEvent

	///<summary>
	/// define Model Event
	/// </summary>
	public event EventHandler<ModelEventArgs> ModelEvent;

	/// <summary>
	/// Dispatch Model Event
	/// </summary>
	protected void DispatchModeleEvent(string type, params object[] args){
		EventHandler<ModelEventArgs> handler = ModelEvent;
		if(handler != null){
			handler(this, new ModelEventArgs(type, args));
		}
	}
	
	/// <summary>
	/// Dispatch Value Update Event
	/// </summary>
	protected void DispatchModelEvent(ModelEventArgs args){
		EventHandler<ModelEventArgs> handler = ModelEvent;
		if(handler != null){
			handler(this, args);
		}
	}
	#endregion


	///<summary>
	///Clear
	///</summary>
	virtual public void Destroy(){
		ModelEvent = null;
		ValueUpdateEvent = null;
	}

	///<summary>
	///Model Event
	///</summary>
	public class ModelEventArgs : EventArgs{
		public string type {get; set;}
		public object[] args;

		public ModelEventArgs(String type, params object[] args){
			this.type = type;
			this.args = args;
		}

		public ModelEventArgs(){

		}
	}

	///<summary>
	///Value Update Event
	///</summary>
	public class ValueUpdateEventArgs : EventArgs{
		public string key {get; set;}

		public object oldValue {get; set;}

		public object newValue {get; set;}

		public ValueUpdateEventArgs(String key, object oldValue, object newVlaue){
			this.key = key;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		public ValueUpdateEventArgs(){

		}
	}
}
