using UnityEngine;
using System.Collections;

public class SoldierFactory : MonoBehaviour {
	void Awake()
	{

	}

	public static GameObject SpawnSoldier(SoldierType st, Vector3 spawnpoint)
	{
		return Instantiate (MapManager.mMapInstance.mSoldiers [(int)st], spawnpoint, Quaternion.identity) as GameObject;
	}
}
