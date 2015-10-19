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
		if (mBuilding.gameObject != null && !mBuilding.mBI.IsDestroyed && mBuilding.mAttackable) {
			if (mBuilding.CanAttack())
				ToAttackState();
		}
	}
	
	public void ToAttackState()
	{
		mBuilding.mBCurrentState = mBuilding.mBAttackState;
	}
	
	public void ToIdleState()
	{

	}
}
