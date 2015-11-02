using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierDetectRange : MonoBehaviour {

	public List<Building> RangeTargetList {
		get {
			return mRangeTargetList;
		}
		set {
			mRangeTargetList = value;
		}
	}
	private List<Building> mRangeTargetList;
	
	//public BuildingType mInterestedBuildingType;
	
	void Awake()
	{
		mRangeTargetList = new List<Building> ();
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "TerrainTile" || other.tag == "AttackRange" || other.tag == "Bullet") {
			return;
		} else {
			//mRangeTargetList.RemoveAll(item => item == null);
			ObjectType objtype = other.gameObject.GetComponent<GameObjectType>().GameType;//other.transform.parent.gameObject.GetComponent<GameObjectType> ().GameType;
			if(objtype == ObjectType.EOT_BUILDING)
			{
				Building bd = other.gameObject.GetComponent<Building>();
				if(bd.mBI.getBuildingType() == BuildingType.E_WALL)
				{
					return;
				}
				else
				{
					if (bd.mBI.IsDestroyed != true && mRangeTargetList.Contains (bd) != true) {
						mRangeTargetList.Add (bd);
						Debug.Log ("Soldier mRanTargetList.Add(bd) bd.name = " + bd.name);
					}
				}
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "TerrainTile" || other.tag == "AttackRange" || other.tag == "Bullet") {
			return;
		} else {
			ObjectType objtype = other.gameObject.GetComponent<GameObjectType> ().GameType;
			if(objtype == ObjectType.EOT_BUILDING)
			{
				Building bd = other.gameObject.GetComponent<Building>();
				if(bd.mBI.getBuildingType()  == BuildingType.E_WALL)
				{
					return;
				}
				else
				{
					if (mRangeTargetList.Contains (bd) != true) {
						mRangeTargetList.Remove (bd);
						Debug.Log ("Soldier mRanTargetList.Remove(bd) bd.name = " + bd.name);
					}
				}
			}
		}
	}
}
