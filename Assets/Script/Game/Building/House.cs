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
		Debug.Log ("House::Awake()");
		mSpawnPoint = gameObject.transform.Find ("BulletSpawnPoint").gameObject.transform.position;

		mHouseBulletScript = GetComponent<Bullet> ();

		mBAttackState = new BuildingAttackState (this);
		mBIdleState = new BuildingIdleState (this);
	}

	public override void Start()
	{
		base.Start ();
		mBCurrentState = mBIdleState;
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

	public override bool CanAttack()
	{
		if (gameObject != null && !mBI.IsDestroyed && mAttackable) {
			mAttackingObject = GameManager.mGameInstance.ObtainAttackSoldier (this);
			if(mAttackingObject!=null)
			{
				mDistanceToTarget = Vector3.Distance (transform.position, mAttackingObject.transform.position);
				if (mDistanceToTarget > mAttackDistance) {
					mIsAttacking = false;
					return false;
				} else {
					mAttackTimer = mAttackInterval;
					mIsAttacking = true;
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

	public override void Attack()
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

	public override bool IsTargetAvalibleToAttack()
	{
		if (mAttackingObject != null && !mAttackingObject.IsDead) {
			return IsTargetInAttackRange();
		} else {
			return false;
		}
	}

	private bool IsTargetInAttackRange()
	{
		float distancetotarget = Vector3.Distance (transform.position, mAttackingObject.transform.position);
		if (distancetotarget > mAttackDistance) {
			return false;
		}
		else
		{
			return true;
		}
	}

	public override void Update()
	{
		base.Update ();
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
