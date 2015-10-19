using UnityEngine;
using System.Collections;

public class BuildingAttackState : BuildingState {
	private Building mBuilding; 

	public BuildingAttackState(Building building)
	{
		mBuilding = building;
	}

	public void UpdateState()
	{
		if (mBuilding.Attackable) {
			Attack ();
		}
	}
	
	public void ToAttackState()
	{
		
	}
	
	public void ToIdleState()
	{
		
	}

	private void Attack()
	{
		if (mBuilding.GetType () is House) {
			var building = mBuilding as House;
			building.Attack();
		}
	}
}