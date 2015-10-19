using UnityEngine;
using System.Collections;

public class BuildingIdleState : BuildingState {
	private Building mBuilding; 

	public BuildingIdleState(Building building)
	{
		mBuilding = building;
	}

	public void UpdateState()
	{
		if (IsAttackable ()) {
			ToAttackState();
		}
	}
	
	public void ToAttackState()
	{

	}
	
	public void ToIdleState()
	{

	}

	private bool IsAttackable()
	{
		if (mBuilding.Attackable && mBuilding.gameObject != null && !mBuilding.mBI.IsDestroyed) {
			if (mBuilding.GetType () is House) {
				var building = mBuilding as House;
				building.AttackingObject = GameManager.mGameInstance.ObtainAttackSoldier (mBuilding);
				building.DistanceToTarget = Vector3.Distance (building.transform.position, building.AttackingObject.transform.position);
				if (building.DistanceToTarget > building.mAttackDistance) {
					building.IsAttacking = false;
					return false;
				} else {
					building.AttackTimer = building.mAttackInterval;
					building.IsAttacking = true;
					return true;
				}
			}
			else
			{
				return false;
			}
		} else {
			return false;
		}
	}
}
