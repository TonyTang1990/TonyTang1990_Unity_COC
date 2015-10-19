using UnityEngine;
using System.Collections;

public class HellephantButton : MonoBehaviour {
	public void onClick()
	{
		Debug.Log ("ZomBearButton::onClick");
		GameManager.mGameInstance.setCurrentSelectedSoldier (SoldierType.E_HELLEPHANT);
	}
}
