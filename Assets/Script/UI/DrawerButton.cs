using UnityEngine;
using System.Collections;

public class DrawerButton : MonoBehaviour {
	public void onClick()
	{
		Debug.Log ("DrawerButton::onClick");
		MapManager.mMapInstance.setCurrenctSelectedBuilding ((int)BuildingType.E_DRAWER);
	}
}
