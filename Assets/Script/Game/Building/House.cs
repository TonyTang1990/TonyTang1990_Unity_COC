using UnityEngine;
using System.Collections;

public class House : Building {

	public GameObject mHouseBullet;

	public float mAttackInterval;

	public float mAttackDistance;

	public Soldier AttackingObject {
		get {
			return mAttackingObject;
		}
		set
		{
			mAttackingObject = value;
		}
	}
	private Soldier mAttackingObject;

	public bool IsAttacking
	{
		get
		{
			return mIsAttacking;
		}
		set
		{
			mIsAttacking = value;
		}
	}
	private bool mIsAttacking = false;

	public float DistanceToTarget {
		get {
			return mDistanceToTarget;
		}
		set {
			mDistanceToTarget = value;
		}
	}
	private float mDistanceToTarget = 0.0f;

	public float AttackTimer {
		get {
			return mAttackTimer;
		}
		set {
			mAttackTimer = value;
		}
	}
	private float mAttackTimer = 0.0f;
	
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
		if (mIsAttacking) {
			mAttackTimer += Time.deltaTime;
			if (mAttackTimer >= mAttackInterval) {
				mAttackTimer = 0.0f;
				GameObject bl = Instantiate (mHouseBullet, mSpawnPoint, Quaternion.identity) as GameObject;
				bl.GetComponent<Bullet>().AttackSoldier = mAttackingObject;
			}
		}
	}

	public void Update()
	{
		/*
		if (gameObject != null && !mBI.IsDestroyed) {
			mAttackingObject = GameManager.mGameInstance.ObtainAttackSoldier (this);

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
		*/
	}
}
