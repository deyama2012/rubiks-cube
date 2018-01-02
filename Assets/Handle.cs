using System;
using UnityEngine;

[Serializable]
public struct Handle
{
	public Vector3 center;
	public Vector3 halfExtents;
	
	public Handle(Vector3 center, Vector3 halfExtents)
	{
		this.center = center;
		this.halfExtents = halfExtents;
	}
}

