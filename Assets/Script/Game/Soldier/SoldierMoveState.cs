﻿using UnityEngine;
using System.Collections;

public class SoldierMoveState : SoldierState {

	private Soldier mSoldier;

	public SoldierMoveState(Soldier soldier)
	{
		mSoldier = soldier;
	}

	public void UpdateState()
	{
		if (!mSoldier.IsDead) {
			mSoldier.MakeDecision ();
			Move ();
		} else {
			ToDeadState();
		}
	}

	public void ToAttackState()
	{
		mSoldier.AttackTimer = mSoldier.mAttackInterval;
		mSoldier.Anim.SetBool("SoldierMoving",false);
		mSoldier.mSCurrentState = mSoldier.mSAttackState as SoldierState;
	}
	
	public void ToMoveState()
	{
		
	}

	public void ToDeadState()
	{
		mSoldier.Anim.SetBool("SoldierMoving",false);
		mSoldier.Anim.SetBool("SoldierDead",true);
		mSoldier.mSCurrentState = mSoldier.mSDeadState;
	}

	private void Move()
	{
		if (mSoldier.AttackTarget != null && !mSoldier.IsTargetInAttackRange()) {
			Vector3 movedirection = mSoldier.AttackTarget.mBI.Position - mSoldier.transform.position;
			movedirection.Normalize();
			mSoldier.transform.LookAt (mSoldier.AttackTarget.mBI.Position);
			Vector3 newposition = mSoldier.transform.position + movedirection * mSoldier.mSpeed * Time.deltaTime;
			//mSoldier.DistanceToTarget = Vector3.Distance(mSoldier.transform.position,mSoldier.AttackTarget.mBI.Position);
			//if (mSoldier.DistanceToTarget > mSoldier.mAttackDistance) {
				mSoldier.transform.position = newposition;
			//} else {
				//if( mSoldier.Anim != null )
				//{
					//mSoldier.AttackTimer = mSoldier.mAttackInterval;				
				//}
			//}
		}
		else
		{
			ToAttackState();
		}
	}
}
