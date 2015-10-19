using UnityEngine;
using System.Collections;

public class SoldierAttackState : SoldierState {

	private Soldier mSoldier; 

	public SoldierAttackState(Soldier soldier)
	{
		mSoldier = soldier;
	}

	public void UpdateState()
	{
		if (!mSoldier.IsDead) {
			mSoldier.MakeDecision ();
			if(mSoldier.IsTargetInAttackRange())
			{	
				Attack ();
			}
			else
			{
				ToMoveState();
			}
			/*
			mSoldier.AttackTimer += Time.deltaTime;
			if (mSoldier.AttackTimer >= mSoldier.mAttackInterval) {
				mSoldier.AttackTimer = 0.0f;
			*/
			//}
		} else {
			ToDeadState();
		}
	}

	public void ToAttackState()
	{

	}

	public void ToMoveState()
	{
		mSoldier.Anim.SetBool("SoldierMoving",true);
		mSoldier.mSCurrentState = mSoldier.mSMoveState;
	}

	public void ToDeadState()
	{
		mSoldier.Anim.SetBool("SoldierDead",true);
		mSoldier.mSCurrentState = mSoldier.mSDeadState;
	}

	private void Attack()
	{
		if (mSoldier.AttackTarget != null && !mSoldier.AttackTarget.mBI.IsDestroyed) {
			mSoldier.Attack();
			/*
			GameObject bl = MonoBehaviour.Instantiate (mSoldier.mBullet, mSoldier.transform.position, Quaternion.identity) as GameObject;
			bl.GetComponent<Bullet> ().AttackTarget = mSoldier.AttackTarget;
			*/
		} else {
			ToMoveState();
		}
	}
}
