using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualUlities : MonoBehaviour {

	public static VisualUlities VUInstance = null;

	private GameObject mLineObject;

	private LineRenderer mLineRender;

	private int mIndexNumber = 0;

	void Awake()
	{
		Debug.Log ("VisualUtilies::Awake() called");
		if (VUInstance == null) 
		{
			VUInstance = this;
		} else if (VUInstance != this) 
		{
			Destroy (gameObject);
		}
		
		DontDestroyOnLoad(gameObject);

		Init();
	}

	public void Init()
	{
		if (mLineRender == null) {
			mLineRender = gameObject.AddComponent<LineRenderer> ();
			mLineRender.enabled = true;
			mLineRender.SetColors (new Color (255, 255, 0), new Color (255, 255, 0));
			mLineRender.SetWidth (0.5f, 0.5f);
		}else {
			Debug.Log("mLineRender has already been Inited");
		}
	}
	
	public void Draw3DLine(Vector3 position)
	{
		mLineRender.SetVertexCount (mIndexNumber+1);
		mLineRender.SetPosition (mIndexNumber, position);
		mIndexNumber++;
	}

	public void DestroyAllLines()
	{
		mIndexNumber = 0;
		mLineRender.SetVertexCount(mIndexNumber);
	}

	public void DisableLineRender()
	{
		mLineRender.enabled = false;
	}
}
