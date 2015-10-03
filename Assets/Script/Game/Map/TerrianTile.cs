using UnityEngine;
using System.Collections;

public class TerrianTile : MonoBehaviour {

	private SpriteRenderer mSR;

	private Color mOriginalColor;

	//private bool mOccuppied;

	//private GameObject mOccupiedBuilding;

	private Vector2 mIndex;

	void Awake()
	{
		mSR = GetComponent<SpriteRenderer> ();
		mOriginalColor = mSR.color;
		//mOccuppied = false;
		mIndex = new Vector2(0,0);
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
