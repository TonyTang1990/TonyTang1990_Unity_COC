using UnityEngine;
using System.Collections;

public class Wall : Building {

	public override void Awake()
	{
		base.Awake ();
		Debug.Log ("Wall::Awake()");
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

	public override bool IsTargetAvalibleToAttack()
	{
		return false;
	}

	public override void Update()
	{
		base.Update ();
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
}
