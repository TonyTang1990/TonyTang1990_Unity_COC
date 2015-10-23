using UnityEngine;
using System.Collections;
using System;
using Pathfinding;

[Serializable]
public enum SoldierType
{
	E_HELLEPHANT = 0,
	E_ZOMBUNNY = 1,
	E_DEFAULT
}

/*
[Serializable]
public enum SoldierState
{
	E_IDLE = 0,
	E_MOVING = 1,
	E_ATTACKING = 2,
	E_DEAD
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

	public Building OldAttackObject {
		get {
			return mOldAttackingObject;
		}
	}
	protected Building mOldAttackingObject;

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

	public Seeker Seeker {
		get {
			return mSeeker;
		}
	}
	protected Seeker mSeeker;

	public Path AStarPath {
		get {
			return mAStarPath;
		}
	}
	protected Path mAStarPath;

	public int CurrentWayPoint {
		get {
			return mCurrentWayPoint;
		}
	}
	protected int mCurrentWayPoint = 0;

	public float mNextWaypointDistance = 0.2f;

	private CharacterController mController;

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

		mSeeker = GetComponent<Seeker> ();

		mController = GetComponent<CharacterController> ();

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
			mOldAttackingObject = mAttackingObject;
			mAttackingObject= GameManager.mGameInstance.ObtainAttackObject (this);
			if(mAttackingObject != mOldAttackingObject)
			{
				CalculatePath();
			}
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

	public void Move()
	{
		if (mAttackingObject != null) {
			if (mAStarPath == null) {
				//We have no path to move after yet
				return;
			}
			if (mCurrentWayPoint >= mAStarPath.vectorPath.Count) {
				Debug.Log ("End Of Path Reached");
				return;
			}
			//Direction to the next waypoint
			Vector3 dir = (mAStarPath.vectorPath [mCurrentWayPoint] - transform.position).normalized;

			transform.LookAt (mAStarPath.vectorPath [mCurrentWayPoint]);

			/*
		if (mController != null) {
			mController.SimpleMove (dir);
		}
		*/
			Vector3 newposition = transform.position + dir * mSpeed * Time.deltaTime;
			transform.position = newposition;

			//Check if we are close enough to the next waypoint
			//If we are, proceed to follow the next waypoint
			if (Vector3.Distance (transform.position, mAStarPath.vectorPath [mCurrentWayPoint]) < mNextWaypointDistance) {
				mCurrentWayPoint++;
				return;
			}
		}
	}
	
	public void CalculatePath()
	{
		if(mAttackingObject != null)
		{
			Debug.Log ("CalculatePath() called");
			StartCoroutine(WaitForPathCalculation());
		}
	}

	private IEnumerator WaitForPathCalculation()
	{
		mAStarPath = mSeeker.StartPath(transform.position, mAttackingObject.transform.position, OnPathComplete);
		yield return StartCoroutine (mAStarPath.WaitForPath ());
	}
	
	private void OnPathComplete(Path path)
	{
		if (!path.error) {
			//mAStarPath = path;
			if(mAStarPath==null)
			{
				Debug.Log ("mAStarPath == null");
			}
			mCurrentWayPoint = 0;
		} else {
			Debug.Log ("Oh noes, the target was not reachable: "+path.errorLog);
		}
	}
}

