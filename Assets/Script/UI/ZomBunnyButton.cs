using UnityEngine;
using System.Collections;

public class ZomBunnyButton : MonoBehaviour {
	public void onClick()
	{
		Debug.Log ("ZomBunnyButton::onClick");
		GameManager.mGameInstance.setCurrentSelectedSoldier (SoldierType.E_ZOMBUNNY);
	}
}
