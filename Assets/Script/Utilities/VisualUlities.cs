using UnityEngine;
using System.Collections;

public class VisualUlities : MonoBehaviour {

	public static VisualUlities VUInstance = null;

	private GameObject mLineObject;
	private LineRenderer mLineRender;

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
			mLineRender = gameObject.AddComponent<LineRenderer>();
			mLineRender.enabled = false;
			mLineRender.SetColors(new Color(255,0,0), new Color(255,0,0));
			mLineRender.SetWidth(1.0f,1.0f);
		} else {
			Debug.Log("mLineRender has already been Inited");
		}
	}
	
	public void Draw3DLine(Vector3 startposition, Vector3 endPosition)
	{
		mLineRender.enabled = true;
		mLineRender.SetPosition (0, startposition);
		mLineRender.SetPosition (1, endPosition);
	}
	
	public void SetLineWidth(float startwidth, float endwidth)
	{
		if (startwidth > 0.0f && endwidth > 0.0f) {
			mLineRender.SetWidth (startwidth, endwidth);
		}
	}
	
	public void DisableLineRender()
	{
		mLineRender.enabled = false;
	}
}
