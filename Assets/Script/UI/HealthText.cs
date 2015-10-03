using UnityEngine;
using System.Collections;

public class HealthText : MonoBehaviour {
	Quaternion mFixedRotation;

	void Awake()
	{
		mFixedRotation = transform.rotation;
	}
	void LateUpdate()
	{
		transform.rotation = mFixedRotation;
	}
}
