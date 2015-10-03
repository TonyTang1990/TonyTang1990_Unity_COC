using UnityEngine;
using System.Collections;

public class ZomBunnyButton : MonoBehaviour {
	public void onClick()
	{
		Debug.Log ("ZomBunnyButton::onClick");
		MapManager.mMapInstance.setCurrentSelectedSoldier (SoldierType.E_ZOMBUNNY);
	}
}
