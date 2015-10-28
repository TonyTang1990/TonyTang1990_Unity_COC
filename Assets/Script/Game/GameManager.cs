using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Pathfinding;
using System.Collections.Generic;

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

	public GameObject mAttackableObject;
	/*
	public List<Vector3> BuildingPositionInGame
	{
		get
		{
			return mBuildingPositionInGame;
		}
	}
	private List<Vector3> mBuildingPositionInGame;

	public Dictionary<Vector3,Building> BuildingDictionaryInGame {
		get {
			return mBuildingDictionaryInGame;
		}
	}
	private Dictionary<Vector3,Building> mBuildingDictionaryInGame;
	*/
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
					//tempselectposition.y += 0.5f;
					int occupiedrow = mCurrentSelectedBuilding.GetComponent<Building>().mBI.getSize().mRow;
					int occupiedcolumn = mCurrentSelectedBuilding.GetComponent<Building>().mBI.getSize().mColumn;
					float terrainoffset = 0.5f;
					tempselectposition.x += terrainoffset * (occupiedrow - 1);
					tempselectposition.z += terrainoffset * (occupiedcolumn - 1);
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

			if(mCurrentSelectedBuilding.GetComponent<Building>().mBI.getBuildingType() != BuildingType.E_WALL)
			{
				MapManager.mMapInstance.NullWallBuildingsInfoInGame.Add(mCurrentSelectedBuilding.GetComponent<Building>());
				MapManager.mMapInstance.NullWallBuildingNumber++;
			}

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

	public void DeselectChoosingStaff()
	{
		if (mCurrentSelectedBuilding != null) {
			Destroy(mCurrentSelectedBuilding);
		}

		mCurrentSelectedSoldierType = SoldierType.E_DEFAULT;
		mIsSoldierSelected = false;
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
				//sod.AttackTarget = bd;
				//sod.CalculatePath();
				currentdistance = Vector3.Distance(bd.mBI.Position, sod.gameObject.transform.position);
				if( currentdistance < shortestdistance )
				{
					shortestdistance = currentdistance;
					targetbuilding = bd;
				}
			}
		}
		if(targetbuilding != null)
		{
			Debug.Log("targetbuilding.position = " + targetbuilding.transform.position);

			sod.NearestTargetDistance = shortestdistance;

			for(int i = 0; i < sod.AttackablePostions.Length; i++)
			{
				sod.AttackablePostions[i] = targetbuilding.mBI.Position;
			}

			sod.AttackablePostions[0].x += sod.mAttackDistance;
			sod.AttackablePostions[0].z -= sod.mAttackDistance;

			sod.AttackablePostions[1].x += sod.mAttackDistance;

			sod.AttackablePostions[2].x += sod.mAttackDistance;
			sod.AttackablePostions[2].z += sod.mAttackDistance;

			sod.AttackablePostions[3].z -= sod.mAttackDistance;

			sod.AttackablePostions[4].z += sod.mAttackDistance;

			sod.AttackablePostions[5].x -= sod.mAttackDistance;
			sod.AttackablePostions[5].z -= sod.mAttackDistance;

			sod.AttackablePostions[6].x -= sod.mAttackDistance;

			sod.AttackablePostions[7].x -= sod.mAttackDistance;
			sod.AttackablePostions[7].z += sod.mAttackDistance;
		}
		/*
		for(int i = 0; i < sod.AttackablePostions.Length; i++)
		{
			Debug.Log (string.Format("sod.AttackablePositions[{0}] = {1}",i,sod.AttackablePostions[i]));
			Instantiate (mAttackableObject, sod.AttackablePostions[i], Quaternion.identity);
		}
		*/
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
