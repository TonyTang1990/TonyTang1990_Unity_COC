using UnityEngine;
using System.Collections;
using System;

[Serializable]
public enum SoldierType
{
	E_HELLEPHANT = 0,
	E_ZOMBUNNY = 1
}

[Serializable]
public enum SoldierState
{
	E_IDLE = 0,
	E_MOVING = 1,
	E_ATTACKING
}

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
	
	public SoldierState mSS;

	public float mSpeed;

	public PreferAttackType mPreferAttackType;

	public float mAttackInterval;

	public float mAttackDistance;

	public float mSHP;

	protected float mDistanceToTarget = 0.0f;

	protected float mAttackTimer = 0.0f;
	
	protected Building mAttackingObject;
	
	public GameObject mBullet;
	
	protected Animator mAnim;

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
	private bool mIsDead = false;

	//private Animator mAnim;

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
	}

	public virtual void Update ()
	{
		if (gameObject) {
			mHPText.text = "HP: " + mSHP;
		}
	}

	public virtual void Move ()
	{

	}

	public virtual void TakeDamage (float damage)
	{
		if (mSHP > damage) {
			mSHP -= damage;
		} else {
			mSHP = 0;
			mIsDead = true;
		}
	}

	public virtual void DestroyItself()
	{
		Destroy (gameObject);
	}
	
   public virtual void Attack()
   {
	    
   }
}

