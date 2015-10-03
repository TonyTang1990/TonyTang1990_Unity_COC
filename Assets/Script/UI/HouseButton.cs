using UnityEngine;
using System.Collections;

public class HouseButton : MonoBehaviour{
	public void onClick()
	{
		Debug.Log ("HouseButton::onClick");
		MapManager.mMapInstance.setCurrenctSelectedBuilding ((int)BuildingType.E_HOUSE);
	}
}
