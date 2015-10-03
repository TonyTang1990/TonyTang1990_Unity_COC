using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour {

    public float mMoveSpeed = 24.0f;
	public float mZoomSpeed = 3.0f;

	public float mOrthographicMinSize = 10.0f;
	public float mOrthographicMaxSize = 30.0f;

	public float mCameraMinX = -60.0f;
	public float mCameraMaxX = -20.0f;
	public float mCameraMinZ = -60.0f;
	public float mCameraMaxZ = -20.0f;

	public GameObject mTargetObject;

	private Vector3 mLookDirection;

    void Start()
    {
		Debug.Log ("Camera transform.position.x = " + transform.position.x);
		Debug.Log ("Camera transform.position.y = " + transform.position.y);
		Debug.Log ("Camera transform.position.z = " + transform.position.z);
    }

	void Update()
	{
		mLookDirection = mTargetObject.transform.position - transform.position;
		mLookDirection.Normalize ();
		VisualUlities.VUInstance.Draw3DLine (transform.position, mTargetObject.transform.position);

		if (Input.GetKey (KeyCode.U)) {
			Camera.main.orthographicSize += mZoomSpeed * Time.deltaTime;
	   }

		if (Input.GetKey (KeyCode.P)) {
			Camera.main.orthographicSize -= mZoomSpeed * Time.deltaTime;
		}
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, mOrthographicMinSize, mOrthographicMaxSize);
	}

    void LateUpdate()
    {
        //Vector3 normalizedforward = transform.forward.normalized;

        float moveHorizontal = Input.GetAxis("Horizontal") * mMoveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * mMoveSpeed * Time.deltaTime;

		Vector3 movement = transform.rotation * new Vector3(moveHorizontal, 0.0f, moveVertical);
		movement.y = 0.0f;

		transform.position += movement;
		float clampx;
		float clampz;
		clampx = Mathf.Clamp (transform.position.x, mCameraMinX, mCameraMaxX);
		clampz = Mathf.Clamp (transform.position.z, mCameraMinZ, mCameraMaxZ);
		transform.position = new Vector3 (clampx, transform.position.y, clampz);
    }
}
