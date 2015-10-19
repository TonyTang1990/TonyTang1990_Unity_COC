using UnityEngine;
using System.Collections;

public class SoldierDeadState : SoldierState {

	private Soldier mSoldier;

	public SoldierDeadState(Soldier solider)
	{
		mSoldier = solider;
	}

	public void UpdateState()
	{
		
	}
	
	public void ToAttackState()
	{
		
	}
	
	public void ToMoveState()
	{
		
	}
	
	public void ToDeadState()
	{
		
	}
}
