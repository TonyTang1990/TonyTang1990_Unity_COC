using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Pathfinding;

public class GameManager : MonoBehaviour {

	public static GameManager mGameInstance = null;
	
	public bool isBuildingSelected
	{
		get
		{
			return mIsBuildingSelected;
		}
		set
		{
			mIsBuildingSelected = value;
		}
	}
	private bool mIsBuildingSelected = false;

	public bool isSoldierSelected
	{
		get
		{
			return mIsSoldierSelected;
		}
		set
		{
			mIsSoldierSelected = value;
		}
	}
	private bool mIsSoldierSelected = false;

	public GameObject CurrentSelectedBuilding {
		get {
			return mCurrentSelectedBuilding;
		}
		set {
			mCurrentSelectedBuilding = value;
		}
	}
	private GameObject mCurrentSelectedBuilding;
	
	private SoldierType mCurrentSelectedSoldierType;

	public Vector2 CurrentOccupiedIndex {
		get {
			return mCurrentOccupiedIndex;
		}
		set {
			mCurrentOccupiedIndex = value;
		}
	}
	private Vector2 mCurrentOccupiedIndex;

	private GameObject mNavigationObject;

	public AstarPath Navigation {
		get {
			return mNavigation;
		}
	}
	private AstarPath mNavigation;

	void Awake()
	{
		if (mGameInstance == null) {
			mGameInstance = this;
		} else if (mGameInstance != this) {
			Destroy(gameObject);
		}

		mNavigationObject = GameObject.FindGameObjectWithTag ("Navigation");

		mNavigation = mNavigationObject.GetComponent<AstarPath> ();
	}

	// Use this for initialization
	void Start () {
		MapManager.mMapInstance.LoadMap ();
		
		MapManager.mMapInstance.SaveMap ();

		if (mNavigation != null) {
			mNavigation.Scan ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (mIsBuildingSelected) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, LayerMask.GetMask ("TerrainTile"))) {
				if(hit.collider)
				{
					//hit.collider.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0);
					Vector3 tempselectposition = hit.collider.transform.position;
					tempselectposition.y += 0.5f;
					mCurrentSelectedBuilding.transform.position = tempselectposition;
					mCurrentSelectedBuilding.GetComponent<Building>().mBI.Position = tempselectposition;
					mCurrentOccupiedIndex = hit.collider.GetComponent<TerrianTile>().getIndex();
				}
			}
		}
	}

	public void setCurrenctSelectedBuilding(int index)
	{
		if (mCurrentSelectedBuilding) {
			Destroy(mCurrentSelectedBuilding);
		}
		mCurrentSelectedBuilding = Instantiate(MapManager.mMapInstance.mBuildings [index],new Vector3(0.0f,100.0f,0.0f),Quaternion.identity) as GameObject;
		mIsBuildingSelected = true;
		mIsSoldierSelected = false;
	}
	
	public void BuildBuilding()
	{
		Debug.Log ("mCurrentOccupiedIndex.x = " + mCurrentOccupiedIndex.x);
		Debug.Log ("mCurrentOccupiedIndex.y = " + mCurrentOccupiedIndex.y);
		
		if (mCurrentSelectedBuilding) {
			Building buildinginfo = mCurrentSelectedBuilding.GetComponent<Building>() as Building;
			
			for( int i = 0 ; i < buildinginfo.mBI.getSize().mRow; i++ )
			{
				for( int j = 0; j < buildinginfo.mBI.getSize().mColumn; j++ )
				{
					MapManager.mMapInstance.MapOccupied [(int)mCurrentOccupiedIndex.x + i, (int)mCurrentOccupiedIndex.y + j] = true;
					MapManager.mMapInstance.Map.setMapOccupiedInfo((int)mCurrentOccupiedIndex.x + i, (int)mCurrentOccupiedIndex.y + j, true);
					//mTerrainTilesScript [(int)mCurrentOccupiedIndex.x + i, (int)mCurrentOccupiedIndex.y + j].setOccupiedBuilding (mCurrentSelectedBuilding);
				}
			}
			
			mCurrentSelectedBuilding.GetComponent<Building>().UpdateChildPosition();
			
			MapManager.mMapInstance.Map.addBuilding(mCurrentSelectedBuilding.GetComponent<Building>().mBI);
			
			MapManager.mMapInstance.BuildingsInGame.Add(mCurrentSelectedBuilding);
			
			MapManager.mMapInstance.BuildingsInfoInGame.Add(mCurrentSelectedBuilding.GetComponent<Building>());
			
			mCurrentSelectedBuilding = null;
			
			mIsBuildingSelected = false;
			
			MapManager.mMapInstance.PrintAllOccupiedInfo ();

			MapManager.mMapInstance.SaveMap ();
		}
	}
	
	public void setCurrentSelectedSoldier(SoldierType stp)
	{
		if (mCurrentSelectedBuilding) {
			Destroy(mCurrentSelectedBuilding);
		}
		mCurrentSelectedSoldierType = stp;
		mIsSoldierSelected = true;
		mIsBuildingSelected = false;
	}
	
	public void DeploySoldier(Vector3 hitpoint)
	{
		GameObject go = SoldierFactory.SpawnSoldier(mCurrentSelectedSoldierType,hitpoint);
		MapManager.mMapInstance.SoldiersInGame.Add(go);
		MapManager.mMapInstance.SoldiersScriptInGame.Add(go.GetComponent<Soldier>());
	}
	
	public Building ObtainAttackObject(Soldier sod)
	{
		Building targetbuilding = null;
		float shortestdistance = Mathf.Infinity;
		float currentdistance = 0.0f;
		foreach (Building bd in MapManager.mMapInstance.BuildingsInfoInGame) {
			if( bd.mBI.IsDestroyed || bd.mBI.mBT == BuildingType.E_WALL)
			{
				//Debug.Log("IsDestroyed = " + bd.mBI.IsDestroyed);
				continue;
			}
			else
			{
				currentdistance = Vector3.Distance(bd.mBI.Position, sod.gameObject.transform.position);
				if( currentdistance < shortestdistance )
				{
					shortestdistance = currentdistance;
					targetbuilding = bd;
				}
			}
		}
		return targetbuilding;
	}
	
	public Soldier ObtainAttackSoldier(Building bd)
	{
		Soldier targetsoldier = null;
		float shortestdistance = Mathf.Infinity;
		float currentdistance = 0.0f;
		foreach (Soldier so in MapManager.mMapInstance.SoldiersScriptInGame) {
			if( so.IsDead )
			{
				//Debug.Log("IsDestroyed = " + bdi.IsDestroyed);
				continue;
			}
			else
			{
				currentdistance = Vector3.Distance(so.transform.position, bd.mBI.Position);
				if( currentdistance < shortestdistance )
				{
					shortestdistance = currentdistance;
					targetsoldier = so;
				}
			}
		}
		return targetsoldier;
	}
}
