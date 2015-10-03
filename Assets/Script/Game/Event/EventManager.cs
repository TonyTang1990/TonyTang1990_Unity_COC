using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

	private Dictionary<string, UnityEvent> mEventDictionary;

	public static EventManager mEMInstance = null;

	void Awake()
	{
		if (mEMInstance == null) {
			mEMInstance = this;
			mEMInstance.Init();
		} else if (mEMInstance != this) {
			Destroy(gameObject);
		}
	}

	void Init()
	{
		if (mEventDictionary == null) {
			mEventDictionary = new Dictionary<string, UnityEvent>();
		}
	}

	public static void StartListening(string eventname, UnityAction listener)
	{
		UnityEvent evt = null;
		if (mEMInstance.mEventDictionary.TryGetValue (eventname, out evt)) {
			evt.AddListener (listener);
		} else {
			evt = new UnityEvent();
			evt.AddListener(listener);
			mEMInstance.mEventDictionary.Add(eventname, evt);
		}
	}


}
