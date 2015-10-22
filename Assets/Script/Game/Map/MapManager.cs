using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class MapManager : MonoBehaviour {
	
	public static MapManager mMapInstance = null;

	public Map Map {
		get {
			return mMap;
		}
	}
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

	public List<GameObject> BuildingsInGame {
		get {
			return mBuildingsInGame;
		}
	}
	private List<GameObject> mBuildingsInGame;

	public List<Building> BuildingsInfoInGame {
		get {
			return mBuildingsInfoInGame;
		}
	}
	private List<Building> mBuildingsInfoInGame;

	public List<GameObject> SoldiersInGame {
		get {
			return mSoldiersInGame;
		}
	}
	private List<GameObject> mSoldiersInGame;

	public List<Soldier> SoldiersScriptInGame {
		get {
			return mSoldiersScriptInGame;
		}
	}
	private List<Soldier> mSoldiersScriptInGame;
	
	private TerrianTile[,] mTerrainTilesScript;

	public bool[,] MapOccupied
	{
		get
		{
			return mMapOccupied;
		}
	}
	private bool[,] mMapOccupied;

	private Transform mMapHolder;

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

		mBuildingsInGame = new List<GameObject>();
		mBuildingsInfoInGame = new List<Building> ();
		mSoldiersInGame = new List<GameObject> ();
		mSoldiersScriptInGame = new List<Soldier>();
	}
	
	public void LoadMap()
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
		MapSetup();	
	}
	
	public void SaveMap()
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
			mBuildingsInGame.Add(bd);
			mBuildingsInfoInGame.Add(bd.GetComponent<Building>());
			Debug.Log("bdi.Position" + bdi.Position);
	    }
		/*
		Vector3 startposition = mTerrainTilesScript [0, 0].gameObject.transform.position;
		startposition.y += 0.5f;
		Vector3 endposition = mTerrainTilesScript [mRows-1, mColumns-1].gameObject.transform.position;
		endposition.y += 0.5f;
		VisualUlities.VUInstance.Draw3DLine (startposition);
		VisualUlities.VUInstance.Draw3DLine (endposition);
		endposition = mTerrainTilesScript [0, mColumns - 1].gameObject.transform.position;
		endposition.y += 0.5f;
		VisualUlities.VUInstance.Draw3DLine (endposition);
		VisualUlities.VUInstance.Draw3DLine (startposition);
		*/
		//VisualUlities.VUInstance.DestroyAllLines ();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

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
		if (GameManager.mGameInstance.CurrentSelectedBuilding) {
			Building buildinginfo = GameManager.mGameInstance.CurrentSelectedBuilding.GetComponent<Building>();
			Vector2 currentoccupiedindex = GameManager.mGameInstance.CurrentOccupiedIndex;
			for(int i = 0; i < buildinginfo.mBI.getSize().mRow; i++)
			{
				for(int j = 0; j < buildinginfo.mBI.getSize().mColumn; j++)
				{
					if( isValideTerrainIndex((int)currentoccupiedindex.x + i, (int)currentoccupiedindex.y + j) )
					{
						if(mMapOccupied[(int)currentoccupiedindex.x + i, (int)currentoccupiedindex.y + j])
						{
							Debug.Log(string.Format("mMapOccupied[{0}][{1}] = {2}",(int)currentoccupiedindex.x + i,(int)currentoccupiedindex.y + j,mMapOccupied[(int)currentoccupiedindex.x + i, (int)currentoccupiedindex.y + j]));
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

	public void PrintAllOccupiedInfo()
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
