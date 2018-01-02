using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Manipulator : MonoBehaviour
{
	public Handle[] handles;
	
	public Transform[] selected;
	
	public int angle = 90;
	
	public bool coroutineRunning;
	
	
	[ContextMenu("Initialize Handles")]
	public void InitializeHandles()
	{
		handles = new Handle[6];
		
		handles[0] = new Handle(Vector3.left, new Vector3(0.1f, 2, 2));
		handles[1] = new Handle(Vector3.right, new Vector3(0.1f, 2, 2));
		
		handles[2] = new Handle(Vector3.down, new Vector3(2, 0.1f, 2));
		handles[3] = new Handle(Vector3.up, new Vector3(2, 0.1f, 2));
		
		handles[4] = new Handle(Vector3.back, new Vector3(2, 2, 0.1f));
		handles[5] = new Handle(Vector3.forward, new Vector3(2, 2, 0.1f));
	}
	
	

	void Rotate(Transform cubie)
	{
		Matrix4x4 m = Matrix4x4.identity;
		
		m.m00 = Mathf.Cos(90);
		m.m20 = -Mathf.Sin(90);
		m.m20 = Mathf.Sin(90);
		m.m22 = Mathf.Cos(90);
		
		Vector3 pos = m.MultiplyPoint(cubie.position);
		//Vector3 dir = m.MultiplyVector(cubie.position - Vector3.left);
		
		cubie.position = pos;
		cubie.LookAt(Vector3.left);
	}
	

	
	
	public void Left()
	{
		if(coroutineRunning)
			return;
		
		selected = Physics.OverlapBox(handles[0].center, handles[0].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		
		foreach (var cubie in selected)
			//cubie.RotateAround(Vector3.left, Vector3.left, angle);
			Rotate(cubie);
	}
	
	public void Right()
	{
		selected = Physics.OverlapBox(handles[1].center, handles[1].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		foreach (var cubie in selected)
			cubie.RotateAround(Vector3.right, Vector3.right, angle);
	}
	
	public void Down()
	{
		selected = Physics.OverlapBox(handles[2].center, handles[2].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		foreach (var cubie in selected)
			cubie.RotateAround(Vector3.down, Vector3.down, angle);
	}
	
	public void Up()
	{
		selected = Physics.OverlapBox(handles[3].center, handles[3].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		foreach (var cubie in selected)
			cubie.RotateAround(Vector3.up, Vector3.up, angle);
	}
	
	public void Front()
	{
		selected = Physics.OverlapBox(handles[4].center, handles[4].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		foreach (var cubie in selected)
			cubie.RotateAround(Vector3.back, Vector3.back, angle);
	}
	
	public void Back()
	{
		selected = Physics.OverlapBox(handles[5].center, handles[5].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		foreach (var cubie in selected)
			cubie.RotateAround(Vector3.forward, Vector3.forward, angle);
	}
	
	
//	void OnDrawGizmos()
//	{
//		if(handles.Length < 6)
//			return;
//
//		Vector3 center = handles[i].center;
//		int size = 2;
//
//		Vector3 p1, p2, p3, p4;
//		p1 = center + Vector3.left * size + Vector3.back * size;
//		p2 = center + Vector3.right * size + Vector3.back * size;
//		p3 = center + Vector3.right * size + Vector3.forward * size;
//		p4 = center + Vector3.left * size + Vector3.forward * size;
//
//		Gizmos.DrawLine(p1, p2);
//		Gizmos.DrawLine(p2, p3);
//		Gizmos.DrawLine(p3, p4);
//		Gizmos.DrawLine(p4, p1);
//	}
}
