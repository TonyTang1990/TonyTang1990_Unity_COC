using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	public static InputManager mInputInstance = null;

	public float mValidInputDeltaTime = 0.5f;

	private float mInputTimer = 0.0f;

	private float mTriggerCount = 0.0f;

	void Awake()
	{
		if (mInputInstance == null) {
			mInputInstance = this;
		} else if (mInputInstance != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		mInputTimer += Time.deltaTime;

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Left Mouse Clicked");
			/*
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, LayerMask.GetMask ("Clickable"))) {
			if(hit.collider)
			{

			}
		}
		else
		{
		*/
			if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ()) {
				mInputTimer = 0.0f;
				if (GameManager.mGameInstance.isSoldierSelected) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, Mathf.Infinity, LayerMask.GetMask ("TerrainTile"))) {
						if (hit.collider) {
							Debug.Log ("hit.point = " + hit.point);
							GameManager.mGameInstance.DeploySoldier (hit.point);
						}
					}
				}
			}
			//}
		}


		if (mInputTimer > mValidInputDeltaTime) {
			if (Input.GetKey (KeyCode.O)) {
				Debug.Log ("KeyCode.O Pressed");
				mInputTimer = 0.0f;
				if (MapManager.mMapInstance.IsTerrainAvaibleToBuild ()) {
					GameManager.mGameInstance.BuildBuilding ();
				}
			}

			if (Input.GetKey (KeyCode.F1)) {
				Debug.Log ("KeyCode.F1 Pressed");
				mInputTimer = 0.0f;
				GameManager.mGameInstance.DeselectChoosingStaff();
				//Test eventmanager
				//mTriggerCount++;
				//EventManager.TriggerEvent("CalculatePath");
			}

			if (Input.GetKey (KeyCode.R)) {
				Debug.Log ("KeyCode.R Pressed");
				mInputTimer = 0.0f;
				//Test eventmanager
				if (EventManager.mEMInstance != null) {
					Debug.Log ("F1Pressed message registerd");
					EventManager.StartListening("F1Pressed",F1Pressed);
			   }
			}
		}
		/*
		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			Debug.Log("Touched");
			Vector2 touchposition = Input.GetTouch (0).position;
			Debug.Log ("touchposition.x = " + touchposition.x);
			Debug.Log ("touchposition.y = " + touchposition.y);
			//Camera.main.ScreenToWorldPoint(
		}
		*/
	}

	void F1Pressed()
    {
		Debug.Log ("F1Pressed callback triggered times: " + mTriggerCount);
	}
}
