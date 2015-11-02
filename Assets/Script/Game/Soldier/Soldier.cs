using UnityEngine;
using System.Collections;
using System;
using Pathfinding;
using System.Collections.Generic;

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
public class Soldier : MonoBehaviour, GameObjectType {

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

	private List<Building> mCurrentCalculatingPaths = null;

	public Building ShortestPathTarget
	{
		get
		{
			return mShortestPathObject;
		}
		set
		{
			mShortestPathObject = value;
		}
	}
	private Building mShortestPathObject;

	public Vector3[] AttackablePostions
	{
		get
		{
			return mAttackablePositions;
		}
	}
	private Vector3[] mAttackablePositions; 

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

	public float ShortestTargetPathLength {
		get {
			return mShortestTargetPathLength;
		}
		set
		{
			mShortestTargetPathLength = value;
		}
	}
	private float mShortestTargetPathLength;

	public float NearestTargetPathLength {
		get {
			return mNearestTargetPathLength;
		}
		set
		{
			mNearestTargetPathLength = value;
		}
	}
	private float mNearestTargetPathLength = Mathf.Infinity;

	public Path ShortestPath {
		get {
			return mShortestPath;
		}
	}
	protected Path mShortestPath;

	private Path[] mLastPaths;

	/* Number of paths completed so far */
	private int mNumCompleted = 0;

	public int CurrentWayPoint {
		get {
			return mCurrentWayPoint;
		}
	}
	protected int mCurrentWayPoint = 0;

	public float mNextWaypointDistance = 0.2f;

	//private CharacterController mController;

	public float NearestTargetDistance {
		get {
			return mNearestTargetDistance;
		}
		set {
			mNearestTargetDistance = value;
		}
	}
	private float mNearestTargetDistance = 0.0f;

	public float NearestReachableTargetDIstance
	{
		get
		{
			return mNearestReachableTargetDistance;
		}
		set
		{
			mNearestReachableTargetDistance = value;
		}
	}
	private float mNearestReachableTargetDistance = 0.0f;

	public ObjectType GameType
	{
		get
		{
			return mGameType;
		}
		set
		{
			mGameType = value;
		}
	}
	private ObjectType mGameType;

	private GameObject mDetectionRangeCollider;
	
	private SoldierDetectRange mDetectionRange;

	public float mDetectionDistance;

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

		//mController = GetComponent<CharacterController> ();

		mSAttackState = new SoldierAttackState (this);

		mSMoveState = new SoldierMoveState (this);

		mSDeadState = new SoldierDeadState (this);

		mAttackablePositions = new Vector3[8];
	
		mGameType = ObjectType.EOT_SOLDIER;
		Debug.Log ("Soldier::Awake() mGameType = " + mGameType);

		mDetectionRangeCollider = gameObject.transform.Find ("AttackRangeCollider").gameObject;
		mDetectionRangeCollider.GetComponent<SphereCollider> ().radius = mDetectionDistance;
		Debug.Log ("DetectionDistance = " + mDetectionDistance);
		mDetectionRange = mDetectionRangeCollider.GetComponent<SoldierDetectRange> ();
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

	private bool ShouldChangeAttackTarget()
	{
		if (mAttackingObject != null)
		{
			if (!mAttackingObject.mBI.IsDestroyed) {
				return false;
			}
			else
			{
				return true;
			}
		} else {
			return true;
		}
	}

	private Building ObtainAttackObjectInDetectionRange()
	{
		mCurrentCalculatingPaths = mDetectionRange.RangeTargetList;
		CalculateAllPathsInfo(mDetectionRange.RangeTargetList);

		return mShortestPathObject;
	}

	public void MakeDecision()
	{
		if (gameObject != null && !mIsDead) {
			mDetectionRange.RangeTargetList.RemoveAll(item => item.mBI.IsDestroyed);
			mOldAttackingObject = mAttackingObject;
			if(ShouldChangeAttackTarget())
			{
				//mAttackingObject = null;
				Debug.Log ("mDetectionRange.RangeTargetList.Count = " + mDetectionRange.RangeTargetList.Count);
				mAttackingObject = ObtainAttackObjectInDetectionRange();
				//CalculateAllPathsInfo(MapManager.mMapInstance.NullWallBuildingsInfoInGame);
				//Otherwise chose one as attack target in whole map
				if(mAttackingObject == null)
				{
					Debug.Log("Chose Target From Whole Map");
				    mAttackingObject= GameManager.mGameInstance.ObtainAttackObject (this);
				}

				if(mAttackingObject != mOldAttackingObject && mAttackingObject!=null)
				{
					CalculatePath();
				}
			}
		}
	}

	private void CalculateAllPathsInfo(List<Building> bds)
	{
		//If any paths are currently being calculated, cancel them to avoid wasting processing power
		if (mLastPaths != null)
			//mLastPaths = null;
			for (int i=0; i<mLastPaths.Length; i++) {
				mLastPaths [i].Error ();
			}

		//Create a new lastPaths array if necessary (can reuse the old one?)
		int validbuildingnumbers = 0; //= MapManager.mMapInstance.NullWallBuildingNumber;
		int nullwallbuildingnumbers = bds.Count;
		foreach (Building nwbd in bds) {
			if(nwbd.mBI.IsDestroyed != true)
			{
				validbuildingnumbers++;
			}
		}
		if (mLastPaths == null || mLastPaths.Length != validbuildingnumbers) {
			mLastPaths = new Path[validbuildingnumbers];
		}

		Building bd = null;
		int pathindex = 0;
		for(int i = 0; i < nullwallbuildingnumbers; i++)
		{
		//foreach (Building bd in MapManager.mMapInstance.BuildingsInfoInGame) {
			//bd = MapManager.mMapInstance.BuildingsInfoInGame[i];
			/*
			if( bd.mBI.IsDestroyed || bd.mBI.mBT == BuildingType.E_WALL)
			{
				//Debug.Log("IsDestroyed = " + bd.mBI.IsDestroyed);
				continue;
			}
			else
			{
			*/
				Debug.Log ("CalculateAllPathsInfo() called");
				bd = bds[i];
				if(bd.mBI.IsDestroyed!=true)
				{
					Debug.Log ("transform.position = " + transform.position);
					Debug.Log ("bd.transform.position = " + bd.transform.position);
					ABPath p = ABPath.Construct (transform.position, bd.transform.position, OnPathInfoComplete);
					mLastPaths[pathindex] = p;
					AstarPath.StartPath (p);
					pathindex++;
				}
			//}
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
		Debug.Log ("mAttackingObject.transform.position = " + mAttackingObject.transform.position);
		mAStarPath = mSeeker.StartPath(transform.position, mAttackingObject.transform.position, OnPathComplete);
		yield return StartCoroutine (mAStarPath.WaitForPath ());
		//float temppathlength = 0.0f;
		mNearestTargetPathLength = mAStarPath.GetTotalLength ();
		Debug.Log ("mNearestTargetPathLength = " + mNearestTargetPathLength);
		//mAStarPath.Claim (this);
	}
	
	private void OnPathComplete(Path path)
	{
		if (!path.error) {
			//mAStarPath = path;
			//if(mAStarPath==null)
			//{
				//Debug.Log ("mAStarPath == null");
			//}
			mCurrentWayPoint = 0;
			float targetpositiondistance = Vector3.Distance (mAttackingObject.transform.position, path.vectorPath [path.vectorPath.Count - 1]);
			Debug.Log ("targetpositiondistance = " + targetpositiondistance);
		} else {
			Debug.Log ("Oh noes, the target was not reachable: "+path.errorLog);
			mAStarPath = null;
		}
	}

	private void OnPathInfoComplete(Path p)
	{
		if (!p.error) {
			/*float temppathlength = p.GetTotalLength ();
			if (temppathlength < mShortestTargetPathLength) {
				mShortestTargetPathLength = temppathlength;
				//mShortestPathObject = bd;
				Debug.Log ("mShortestTargetPathLength = " + mShortestTargetPathLength);
			}
			mCurrentWayPoint = 0;
			*/
			//Make sure this path is not an old one
			for (int i=0;i<mLastPaths.Length;i++) {
				if (mLastPaths[i] == p) {
					mNumCompleted++;
					
					if (mNumCompleted >= mLastPaths.Length) {
						CompleteSearchClosest ();
					}
					return;
				}
			}
		} else {
			Debug.Log ("Oh noes, the target was not reachable: "+p.errorLog);
			mShortestPath = null;
		}
	}

	/* Called when all paths have completed calculation */
	public void CompleteSearchClosest () {
		
		//Find the shortest path
		Path shortest = null;
		float shortestLength = float.PositiveInfinity;
		
		//Loop through the paths
		for (int i=0;i<mLastPaths.Length;i++) {
			//Get the total length of the path, will return infinity if the path had an error
			float length = mLastPaths[i].GetTotalLength();
			
			Debug.Log ("length = "+ length);

			if (shortest == null || length < mShortestTargetPathLength) {
				shortest = mLastPaths[i];
				mShortestTargetPathLength = length;
				mShortestPathObject = mCurrentCalculatingPaths[i];				
			}
		}
		Debug.Log ("mShortestTargetPathLength = "+ mShortestTargetPathLength);
		mShortestPath = shortest;
	}
}

