using UnityEngine;
using System.Collections;
using System;

[Serializable]
public enum SoldierType
{
	E_HELLEPHANT = 0,
	E_ZOMBUNNY = 1
}

/*
[Serializable]
public enum SoldierState
{
	E_IDLE = 0,
	E_MOVING = 1,
	E_ATTACKING
}
*/
[Serializable]
public enum PreferAttackType
{
	E_NULLATTACABLE = 0,
	E_ATTACKABLE = 1,
	E_ALL
}

[Serializable]
public class Soldier : MonoBehaviour {

	//public SoldierType mST;
	
	//public SoldierState mSS;

	public float mSpeed;

	public PreferAttackType mPreferAttackType;

	public float mAttackInterval;
	
	public float mAttackDistance;

	public float mSHP;
	
	public GameObject mBullet;

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
	protected float mAttackTimer = 0.0f;

	public Building AttackTarget {
		get {
			return mAttackingObject;
		}
		set {
			mAttackingObject = value;
		}
	}
	protected Building mAttackingObject;

	public Animator Anim {
		get {
			return mAnim;
		}
	}
	protected Animator mAnim;

	[HideInInspector] public SoldierState mSCurrentState;

	[HideInInspector] public SoldierAttackState mSAttackState;

	[HideInInspector] public SoldierDeadState mSDeadState;

	[HideInInspector] public SoldierMoveState mSMoveState;

	public bool IsDead{
		get
		{
			return mIsDead;
		}
		set
		{
			mIsDead = value;
		}
	}
	protected bool mIsDead = false;

	private TextMesh mHPText;

	public virtual void Awake()
	{
		Debug.Log("Soldier's Position = " + gameObject.transform.position);
		/*
		if (mST == SoldierType.E_ZOMBUNNY) {
			mAnim = GetComponent<Animator>();
		}
		*/

		mHPText = gameObject.transform.Find ("HealthText").gameObject.GetComponent<TextMesh> ();
		if (mHPText == null) {
			Debug.Log("mHPText == null");
		}
		mHPText.text = "HP: " + mSHP;

		mAnim = GetComponent<Animator>();

		mSAttackState = new SoldierAttackState (this);

		mSMoveState = new SoldierMoveState (this);

		mSDeadState = new SoldierDeadState (this);
	}

	public void Start()
	{
		mSCurrentState = mSMoveState;
	}

	public virtual void Update ()
	{
		if (gameObject) {
			mHPText.text = "HP: " + mSHP;
			mSCurrentState.UpdateState();
		}
	}

	public void TakeDamage (float damage)
	{
		if (mSHP > damage) {
			mSHP -= damage;
		} else {
			mSHP = 0;
			mIsDead = true;
		}
	}

	//AniamtionEvent call
	public void StartSinking ()
	{
		Invoke ("DestroyItself",3.0f);
	}

	public virtual void DestroyItself()
	{
		Destroy (gameObject);
	}
	
	public void MakeDecision()
	{
		if (gameObject != null && !mIsDead) {
			mAttackingObject= GameManager.mGameInstance.ObtainAttackObject (this);
		}
	}
	
	public bool IsTargetInAttackRange()
	{
		if (mAttackingObject != null) {
			mDistanceToTarget = Vector3.Distance (transform.position, mAttackingObject.mBI.Position);
			if (mDistanceToTarget > mAttackDistance) {
				return false;
			} else {
				return true;
			}
		} else {
			return false;
		}
	}

	public void Attack()
	{
		mAttackTimer += Time.deltaTime;
		if (mAttackTimer >= mAttackInterval) {
			mAttackTimer = 0.0f;
			GameObject bl = MonoBehaviour.Instantiate (mBullet, transform.position, Quaternion.identity) as GameObject;
			bl.GetComponent<Bullet> ().AttackTarget = mAttackingObject;
		}
	}
}

