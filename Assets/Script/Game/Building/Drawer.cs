using UnityEngine;
using System.Collections;

public class Drawer : Building {
	
	public override void Awake()
	{
		base.Awake ();
		Debug.Log ("Drawer::Awake()");
		mBAttackState = new BuildingAttackState (this);
		mBIdleState = new BuildingIdleState (this);
	}
	
	public override void Start()
	{
		base.Start ();
		mBCurrentState = mBIdleState;
	}

	public override void Update()
	{
		base.Update ();
	}


	public override void FixedUpdate()
	{
		base.FixedUpdate ();
	}
	
	public override void TakeDamage(float damage)
	{
		base.TakeDamage (damage);
	}

	public override bool CanAttack()
	{
		return false;
	}

	public override void Attack()
	{

	}

	public override bool IsTargetAvalibleToAttack()
	{
		return false;
	}
}
