using UnityEngine;
using System.Collections;

public interface SoldierState {

	void UpdateState();

	void ToMoveState();

	void ToAttackState();

	void ToDeadState();
}
