using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class MapManager : MonoBehaviour {
	
	public static MapManager mMapInstance = null;

	private Map mMap;

	public int getColumns
	{
		get
		{
			return mColumns;
		}
		private set
		{
			mColumns = value;
		}
	}
	public int mColumns = 64;

	public int getRows
	{
		get
		{
			return mRows;
		}
		private set
		{
			mRows = value;
		}
	}
	public int mRows = 64;

	public GameObject[] mTerrainTiles;

	public List<GameObject> mBuildings;
	
	public List<GameObject> mSoldiers;

	private List<GameObject> mBuldingsInGame;

	private List<Building> mBuldingsInfoInGame;

	private List<GameObject> mSoldiersInGame;

	private List<Soldier> mSoldiersScriptInGame;
	
	private TerrianTile[,] mTerrainTilesScript;

	private bool[,] mMapOccupied;

	private Transform mMapHolder;

	private GameObject mCurrentSelectedBuilding;

	private SoldierType mCurrentSelectedSoldierType;

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

	private Vector2 mCurrentOccupiedIndex;

	private string mMapSavePath;

	void Awake()
	{
		if (mMapInstance == null) {
			mMapInstance = this;
		} else if (mMapInstance != this) {
			Destroy(gameObject);
		}

		mMapSavePath = Application.persistentDataPath + "/mapInfo.dat";
		Debug.Log ("mMapSavePath = " + mMapSavePath);

		mBuldingsInGame = new List<GameObject>();
		mBuldingsInfoInGame = new List<Building> ();
		mSoldiersInGame = new List<GameObject> ();
		mSoldiersScriptInGame = new List<Soldier>();
	}
	
	void LoadMap()
	{
		Debug.Log("LoadMap()");
		if (File.Exists (mMapSavePath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fs = File.Open (mMapSavePath, FileMode.Open);
			mMap = (Map)bf.Deserialize (fs);
			fs.Close ();
		} else {
			mMap = new Map ();
			SaveMap();
		}
	}
	
	void SaveMap()
	{
		Debug.Log("SaveMap()");
		if (!File.Exists (mMapSavePath)) {
			FileStream fsc = File.Create(mMapSavePath);
			fsc.Close();
		}
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = File.Open (mMapSavePath, FileMode.Open);
		
		bf.Serialize (fs, mMap);
		fs.Close ();
	}

	void MapSetup()
	{
		mMapHolder = new GameObject ("Map").transform;
		Debug.Log("mMapHolder.position.x = " + mMapHolder.position.x);
		Debug.Log("mMapHolder.position.y = " + mMapHolder.position.y);
		Debug.Log("mMapHolder.position.z = " + mMapHolder.position.z);

		mMapOccupied = new bool[mRows,mColumns];

		mTerrainTilesScript = new TerrianTile[mRows, mColumns];

		for (int i = -(mRows/2); i < mRows/2; i++) 
		{
			for(int j = -(mColumns/2); j < mColumns/2; j++)
			{
				GameObject toInstantiate = mTerrainTiles[ (i + mRows/2 + j + mColumns/2)%5];//mTerrainTiles[Random.Range (0,mTerrainTiles.Length)];

				GameObject instance = Instantiate(toInstantiate,new Vector3(i,5f,j),Quaternion.LookRotation(Vector3.up, mMapHolder.forward)) as GameObject;

				instance.transform.SetParent(mMapHolder);

				mTerrainTilesScript[i + mRows/2,j + mColumns/2] = instance.GetComponent<TerrianTile>();

				//mTerrainTilesScript[i + mRows/2,j + mColumns/2].setOccupied(false);

				mTerrainTilesScript[i + mRows/2,j + mColumns/2].setIndex(i + mRows/2,j + mColumns/2);

				mMapOccupied[i + mRows/2,j + mColumns/2] = false;

				if(mMap.getMapOccupiedInfo(i + mRows/2,j + mColumns/2) == true)
				{
					Debug.Log(string.Format("mMap.getMapOccupiedInfo[{0}][{1}] = {2}]",i + mRows/2,j + mColumns/2,mMap.getMapOccupiedInfo(i + mRows/2,j + mColumns/2)));
				}
				mMapOccupied[i + mRows/2,j + mColumns/2] = mMap.getMapOccupiedInfo(i + mRows/2,j + mColumns/2);
			}
		}

        Debug.Log("mMap.getBuildings().Capacity = " + mMap.getBuildings().Capacity);
		GameObject bd;
		Vector3 position;
		foreach( BuildingInfo bdi in mMap.getBuildings())
        {
			Debug.Log("bdi.getBuildingType() = " + bdi.getBuildingType());
			Debug.Log(string.Format("bd.getPosition().x = {0} .y = {1} .z = {2}", bdi.Position.x, bdi.Position.y, bdi.Position.z));
			position = new Vector3(bdi.Position.x,bdi.Position.y,bdi.Position.z);
			bd = Instantiate(mBuildings[(int)bdi.getBuildingType()],position,Quaternion.identity) as GameObject;
			bd.GetComponent<Building>().mBI.Position = position;
			mBuldingsInGame.Add(bd);
			mBuldingsInfoInGame.Add(bd.GetComponent<Building>());
			Debug.Log("bdi.Position" + bdi.Position);
	    }
	}

	public void setCurrenctSelectedBuilding(int index)
	{
		if (mCurrentSelectedBuilding) {
			Destroy(mCurrentSelectedBuilding);
		}
		mCurrentSelectedBuilding = Instantiate(mBuildings [index],new Vector3(0.0f,100.0f,0.0f),Quaternion.identity) as GameObject;
		mIsBuildingSelected = true;
		mIsSoldierSelected = false;
	}

	// Use this for initialization
	void Start () {
		LoadMap ();
		
		SaveMap ();

		MapSetup ();
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

	private bool isValideTerrainIndex(int row, int column)
	{
		bool isrowindexvalide = !( row < 0 || row >= mRows);
		bool iscolumnindexvalide = !(column < 0 || column >= mColumns);
		bool isvalideindex = (isrowindexvalide && iscolumnindexvalide);
		return isvalideindex;
	}

	public bool IsTerrainAvaibleToBuild()
	{
		bool isavaliable = true;
		if (mCurrentSelectedBuilding) {
			Building buildinginfo = mCurrentSelectedBuilding.GetComponent<Building>();
			for(int i = 0; i < buildinginfo.mBI.getSize().mRow; i++)
			{
				for(int j = 0; j < buildinginfo.mBI.getSize().mColumn; j++)
				{
					if( isValideTerrainIndex((int)mCurrentOccupiedIndex.x + i, (int)mCurrentOccupiedIndex.y + j) )
					{
						if(mMapOccupied[(int)mCurrentOccupiedIndex.x + i, (int)mCurrentOccupiedIndex.y + j])
						{
							Debug.Log(string.Format("mMapOccupied[{0}][{1}] = {2}",(int)mCurrentOccupiedIndex.x + i,(int)mCurrentOccupiedIndex.y + j,mMapOccupied[(int)mCurrentOccupiedIndex.x + i, (int)mCurrentOccupiedIndex.y + j]));
							isavaliable = false;
						}
					}
					else{
						Debug.Log("Index invalide");
						isavaliable = false;
						break;
					}
				}
			}
		}
		Debug.Log ("isavaliable = " + isavaliable);
		return isavaliable;
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
					mMapOccupied [(int)mCurrentOccupiedIndex.x + i, (int)mCurrentOccupiedIndex.y + j] = true;
					mMap.setMapOccupiedInfo((int)mCurrentOccupiedIndex.x + i, (int)mCurrentOccupiedIndex.y + j, true);
					//mTerrainTilesScript [(int)mCurrentOccupiedIndex.x + i, (int)mCurrentOccupiedIndex.y + j].setOccupiedBuilding (mCurrentSelectedBuilding);
				}
			}

			mCurrentSelectedBuilding.GetComponent<Building>().UpdateChildPosition();

			mMap.addBuilding(mCurrentSelectedBuilding.GetComponent<Building>().mBI);

			mBuldingsInGame.Add(mCurrentSelectedBuilding);

			mBuldingsInfoInGame.Add(mCurrentSelectedBuilding.GetComponent<Building>());

			mCurrentSelectedBuilding = null;

			mIsBuildingSelected = false;
			
			PrintAllOccupiedInfo ();

			SaveMap ();
		}
	}
	
	public void setCurrentSelectedSoldier(SoldierType stp)
	{
		mCurrentSelectedSoldierType = stp;
		mIsSoldierSelected = true;
		mIsBuildingSelected = false;
	}

	public void DeploySoldier(Vector3 hitpoint)
	{
		GameObject go = SoldierFactory.SpawnSoldier(mCurrentSelectedSoldierType,hitpoint);
        mSoldiersInGame.Add(go);
		mSoldiersScriptInGame.Add(go.GetComponent<Soldier>());
	}

	public Building ObtainAttackObject(Soldier sod)
	{
		Building targetbuilding = null;
		float shortestdistance = Mathf.Infinity;
		float currentdistance = 0.0f;
		foreach (Building bd in mBuldingsInfoInGame) {
			if( bd.mBI.IsDestroyed )
			{
				//Debug.Log("IsDestroyed = " + bdi.IsDestroyed);
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
		foreach (Soldier so in mSoldiersScriptInGame) {
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

	void PrintAllOccupiedInfo()
	{
		for(int i = 0; i < mRows; i++ )
		{
			for(int j = 0; j < mColumns; j++)
			{
				if(mMapOccupied[i,j])
				{
					Debug.Log(string.Format("mMapOccupied[{0}][{1}] = {2}",i,j,mMapOccupied[i,j]));
				}
			}
		}
	}
}
