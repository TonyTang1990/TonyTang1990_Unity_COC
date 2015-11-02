using UnityEngine;
using System.Collections;

public enum ObjectType
{
	EOT_BUILDING = 0,
	EOT_SOLDIER = 1,
	EOT_TERRAIN = 2
}

public interface GameObjectType {
	ObjectType GameType
	{
		get;
		set;
	}
}
