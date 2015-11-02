using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public enum BuildingType
{
	E_WALL = 0,
	E_HOUSE = 1,
	E_DRAWER
}

[Serializable]
public struct OccupiedSize
{
	public int mRow;
	public int mColumn;
}

[Serializable]
public struct BuildingPosition
{
	public float mX;
	public float mY;
	public float mZ;
}

[Serializable]
public class BuildingInfo
{
	public BuildingType mBT;

	public OccupiedSize mSize;

	public float mBHP;

	public Vector3 Position
	{
		get
		{
			return new Vector3(mPosition.mX, mPosition.mY, mPosition.mZ);
		}
		set
		{
			mPosition.mX = value.x;
			mPosition.mY = value.y;
			mPosition.mZ = value.z;
		}
	}
	private BuildingPosition mPosition;

	public bool IsDestroyed
	{
		get
		{
			return mIsDestroyed;
		}
		set
		{
			mIsDestroyed = value;
		}
	}
	private bool mIsDestroyed = false;

	public BuildingType getBuildingType()
	{
		return mBT;
	}
	
	public OccupiedSize getSize()
	{
		return mSize;
	}

}

[Serializable]
public class Building : MonoBehaviour, GameObjectType {

	public BuildingInfo mBI;
	
	public bool mAttackable = true;

	private TextMesh mHPText;
	
	protected Vector3 mSpawnPoint;

	[HideInInspector] public BuildingState mBCurrentState;
	
	[HideInInspector] public BuildingAttackState mBAttackState;
	
	[HideInInspector] public BuildingIdleState mBIdleState;

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

	public virtual void Awake()
	{
		Debug.Log ("Building::Awake()");
		mHPText = gameObject.transform.Find ("HealthText").gameObject.GetComponent<TextMesh> ();
		if (mHPText == null) {
			Debug.Log("mHPText == null");
		}
		mHPText.text = "HP: " + mBI.mBHP;
		
		mGameType = ObjectType.EOT_BUILDING;
		Debug.Log ("Building::Awake() mGameType = " + mGameType);
	}

	public virtual void Start()
	{

	}

	public virtual void Update()
	{
		if (gameObject && mBCurrentState != null) {
			mBCurrentState.UpdateState();
		}
	}

	public virtual void FixedUpdate()
	{
		if (gameObject) {
			mHPText.text = "HP: " + mBI.mBHP;
		}
	}

	public virtual void TakeDamage(float damage)
	{
		if (mBI.mBHP > damage) {
			mBI.mBHP -= damage;
		} else {
			Debug.Log("IsDestroyed == true");
			mBI.mBHP = 0;
			mBI.IsDestroyed = true;
		}
	}

	public virtual void UpdateChildPosition()
	{

	}

	public virtual bool CanAttack()
	{
		return false;
	}

	public virtual void Attack()
	{

	}

	public virtual bool IsTargetAvalibleToAttack()
	{
		return false;
	}
	/*
	void OnTriggerEnter(Collider other) {
		Debug.Log ("other.name = " + other.name);
		if (other.tag == "Bullet") {
			float damage = other.gameObject.GetComponent<Bullet> ().mDamage;
			Debug.Log ("damage = " + damage);
			if (mBI.mBHP > damage) {
				mBI.mBHP -= damage;
				Destroy (other.gameObject);
			} else {
				mBI.mBHP = 0;
				mBI.IsDestroyed = true;
				//MapManager.mMapInstance.RemoveObjectFromMap(mBI);
				Destroy (other.gameObject);
				//Destroy (gameObject);
			}
		}
	}
	*/
}
