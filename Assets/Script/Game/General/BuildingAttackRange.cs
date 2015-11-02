using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingAttackRange: MonoBehaviour{

	public List<Soldier> RangeTargetList {
		get {
			return mRangeTargetList;
		}
		set {
			mRangeTargetList = value;
		}
	}
	private List<Soldier> mRangeTargetList;

	//public ObjectType mInterestedObjectType;

	void Awake()
	{
		mRangeTargetList = new List<Soldier> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "TerrainTile" || other.tag == "AttackRange" || other.tag == "Bullet") {
			return;
		} else {
			ObjectType objtype = other.gameObject.GetComponent<GameObjectType>().GameType;//other.transform.parent.gameObject.GetComponent<GameObjectType> ().GameType;
				if(objtype == ObjectType.EOT_SOLDIER)
				{
					Soldier so = other.gameObject.GetComponent<Soldier>();
					if (so.IsDead != true && mRangeTargetList.Contains (so) != true) {
						mRangeTargetList.Add (so);
					Debug.Log ("OnTriggerEnter mRanTargetList.Add(so) so.name = " + so.name);
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
				if(objtype == ObjectType.EOT_SOLDIER)
				{
					Soldier so = other.gameObject.GetComponent<Soldier>();
					if (mRangeTargetList.Contains (so) == true) {
						mRangeTargetList.Remove (so);
					Debug.Log ("OnTriggerExit mRanTargetList.Remove(so) so.name = " + so.name);
					}
				}
		}
	}

}
