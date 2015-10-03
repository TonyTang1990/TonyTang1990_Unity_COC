using UnityEngine;
using System.Collections;

public class HellephantButton : MonoBehaviour {
	public void onClick()
	{
		Debug.Log ("ZomBearButton::onClick");
		MapManager.mMapInstance.setCurrentSelectedSoldier (SoldierType.E_HELLEPHANT);
	}
}
