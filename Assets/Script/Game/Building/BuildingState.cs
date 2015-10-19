using UnityEngine;
using System.Collections;


public interface BuildingState {
	
	void UpdateState();
	
	void ToAttackState();
	
	void ToIdleState();
}