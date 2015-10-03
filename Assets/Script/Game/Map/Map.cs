using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class Map {

	public List<BuildingInfo> mBuildings = new List<BuildingInfo>();

	public bool[,] mMapOccupied = new bool[MapManager.mMapInstance.getRows, MapManager.mMapInstance.getColumns];

	public void addBuilding(BuildingInfo building)
	{
		Debug.Log(string.Format("building.getPosition().mX = {0} mY = {1} mZ = {2}",building.Position.x,building.Position.y,building.Position.z));
		mBuildings.Add (building);
	}

	public List<BuildingInfo> getBuildings()
	{
		return mBuildings;
	}

	public void setMapOccupiedInfo(int row, int column, bool isoccupied)
	{
		mMapOccupied [row, column] = isoccupied;
	}

	public bool getMapOccupiedInfo(int row, int column)
	{
		if (mMapOccupied.Length == 0) {
			Debug.Log("mMapOccupied.Length == 0");
		}
		return mMapOccupied[row,column];
	}
}
