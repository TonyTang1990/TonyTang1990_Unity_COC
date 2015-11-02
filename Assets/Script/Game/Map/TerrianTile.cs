using UnityEngine;
using System.Collections;

public class TerrianTile : MonoBehaviour, GameObjectType {

	private SpriteRenderer mSR;

	private Color mOriginalColor;

	//private bool mOccuppied;

	//private GameObject mOccupiedBuilding;

	private Vector2 mIndex;

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

	void Awake()
	{
		mSR = GetComponent<SpriteRenderer> ();
		mOriginalColor = mSR.color;
		//mOccuppied = false;
		mIndex = new Vector2(0,0);

		mGameType = ObjectType.EOT_TERRAIN;
		Debug.Log ("TerrainTile::Awake() mGameType = " + mGameType);
	}
	/*
	public void setOccupied(bool occupied)
	{
		mOccuppied = occupied;
		Debug.Log ("setOccupied(" + occupied + ")");
	}
	*/
	public void setIndex(int row, int column)
	{
		Debug.Log ("row = " + row);
		Debug.Log ("column = " + column);

		mIndex = new Vector2(row,column);
	}

	public Vector2 getIndex()
	{
		return mIndex;
	}

	/*
	public void setOccupiedBuilding(GameObject building)
	{
		mOccupiedBuilding = building;
	}
	*/

	void OnMouseDown()
	{
		Debug.Log ("OnMouseDown()");
	}
	
	void OnMouseUp()
	{
		Debug.Log ("OnMouseUp()");
	}

	void OnMouseExit()
	{
		//if (!mOccuppied) {
			//Debug.Log ("OnMouseExit()");
			mSR.color = mOriginalColor;
		//}
	}
}
