using UnityEngine;
using System.Collections;

public class WallButton : MonoBehaviour{
	public void onClick()
	{
		Debug.Log ("WallButton::onClick");
		MapManager.mMapInstance.setCurrenctSelectedBuilding ((int)BuildingType.E_WALL);
	}
}
