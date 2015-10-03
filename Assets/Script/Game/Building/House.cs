using UnityEngine;
using System.Collections;

public class House : Building {

	public GameObject mHouseBullet;

	public float mAttackInterval;

	public float mAttackDistance;

	private Soldier mAttackingObject;

	private bool mIsAttacking = false;

	protected float mDistanceToTarget = 0.0f;
	
	protected float mAttackTimer = 0.0f;
	
	private Bullet mHouseBulletScript;

	public override void Awake()
	{
		base.Awake ();

		mSpawnPoint = gameObject.transform.Find ("BulletSpawnPoint").gameObject.transform.position;

		mHouseBulletScript = GetComponent<Bullet> ();
	}
	
	public override void FixedUpdate()
	{
		base.FixedUpdate ();
	}
	
	public override void TakeDamage(float damage)
	{
		base.TakeDamage (damage);
	}

	public override void UpdateChildPosition()
	{
		mSpawnPoint = gameObject.transform.Find ("BulletSpawnPoint").gameObject.transform.position;
	}
	
	public void Attack()
	{
		GameObject bl = Instantiate (mHouseBullet, mSpawnPoint, Quaternion.identity) as GameObject;
		bl.GetComponent<Bullet>().AttackSoldier = mAttackingObject;
	}

	public void Update()
	{
		if (gameObject != null && !mBI.IsDestroyed) {
			mAttackingObject = MapManager.mMapInstance.ObtainAttackSoldier (this);

			if(mAttackingObject != null && mAttackable)
			{
				if (mIsAttacking) {
					mAttackTimer += Time.deltaTime;
					if (mAttackTimer >= mAttackInterval) {
						mAttackTimer = 0.0f;
						Attack ();
					}
				}
				
				mDistanceToTarget = Vector3.Distance (transform.position, mAttackingObject.transform.position);
				if (mDistanceToTarget > mAttackDistance) {
					mIsAttacking = false;
					mAttackTimer = mAttackInterval;
				} else {
					mIsAttacking = true;
				}
			}
		}
	}
}
