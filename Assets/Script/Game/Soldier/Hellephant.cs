using UnityEngine;
using System.Collections;

public class Hellephant : Soldier {
	public override void Awake()
	{
		base.Awake ();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		
		if (gameObject != null && !IsDead) {
			mAttackingObject = MapManager.mMapInstance.ObtainAttackObject (this);
			
			if (mAttackingObject != null) {
				Move ();
			}
		}
	}
	
	public override void Move()
	{
		base.Move ();
		
		if (!mAnim.GetBool ("SoldierMoving")) {
			mAttackTimer += Time.deltaTime;
			if(mAttackTimer >= mAttackInterval)
			{
				mAttackTimer = 0.0f;
				Attack();
			}
		}
		Vector3 movedirection = mAttackingObject.mBI.Position - transform.position;
		movedirection.Normalize();
		transform.LookAt (mAttackingObject.mBI.Position);
		Vector3 newposition = transform.position + movedirection * mSpeed * Time.deltaTime;
		mDistanceToTarget = Vector3.Distance(transform.position,mAttackingObject.mBI.Position);
		if (mDistanceToTarget > mAttackDistance) {
			transform.position = newposition;
			if (mAnim != null) {
				mAnim.SetBool ("SoldierMoving", true);
				mAttackTimer = mAttackInterval;
			}
		} else {
			if( mAnim != null )
			{
				mAnim.SetBool("SoldierMoving",false);
			}
		}
	}
	
	public override void Attack(/*float distance*/)
	{
		GameObject bl = Instantiate (mBullet, transform.position, Quaternion.identity) as GameObject;
		bl.GetComponent<Bullet>().AttackTarget = mAttackingObject;
	}
	
	public override void TakeDamage (float damage)
	{
		base.TakeDamage (damage);
		if (IsDead ) {
			mAnim.SetTrigger("SoldierDead");
		}
	}
	
	//AniamtionEvent call
	public void StartSinking ()
	{
		Invoke ("DestroyItself",3.0f);
	}
	
	public override void DestroyItself()
	{
		base.DestroyItself ();
	}
}
