using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float mBSpeed;

	public float mDamage;

	private Vector3 mMoveDirection;

	public float LifeTime
	{
		set
		{
			mLifeTime = value;
			Invoke ("DestroyItself",mLifeTime);
			//Debug.Log("Bullet LifeTime = " + mLifeTime);
		}
	}
	private float mLifeTime;

	public Building AttackTarget
	{
		set
		{
			mAttackTarget = value;
			CaculateInfoRequired();
		}
	}
	private Building mAttackTarget;

	public Soldier AttackSoldier
	{
		set
		{
			mAttackSoldier = value;
			CaculateInfoRequired();
		}
	}
	private Soldier mAttackSoldier;

	private void CaculateInfoRequired()
	{
		if(mAttackSoldier != null)
		{
			//Debug.Log("mAttackSoldier.transform.position = " + mAttackSoldier.transform.position);
			//Debug.Log("transform.position = " + transform.position);

			float distance = Vector3.Distance(mAttackSoldier.transform.position, transform.position);
			float arrivetime = distance / mBSpeed;
			LifeTime = arrivetime;
			mMoveDirection = mAttackSoldier.transform.position - transform.position;
			mMoveDirection.Normalize();
			//Debug.Log("mMoveDirection = " + mMoveDirection);
		}
		if(mAttackTarget != null)
		{
			float distance = Vector3.Distance(mAttackTarget.mBI.Position, transform.position);
			float arrivedtime = distance / mBSpeed;
			LifeTime = arrivedtime;
			mMoveDirection = mAttackTarget.mBI.Position - transform.position;
			mMoveDirection.Normalize();
			//Debug.Log("mMoveDirection = " + mMoveDirection);
		}
	}

	// Update is called once per frame
	void Update () {
		if (mAttackTarget != null || mAttackSoldier != null) {
			transform.position += mMoveDirection * mBSpeed * Time.deltaTime;
		}
	}

	void DestroyItself()
	{
		Destroy (gameObject);
	}

	void OnDestroy()
	{
		if (mAttackTarget != null && !mAttackTarget.mBI.IsDestroyed) {
			mAttackTarget.TakeDamage (mDamage);
		}
		if (mAttackSoldier != null && !mAttackSoldier.IsDead) {
			mAttackSoldier.TakeDamage(mDamage);
		}
		//Debug.Log ("Bullet::OnDestroy()");
	}
}
