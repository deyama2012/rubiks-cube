using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Manipulator : MonoBehaviour
{
	public Handle[] handles;
	public Transform[] selected;
	public int angle = 90;
	public float duration = 1;
	[SerializeField]
	private bool coroutineRunning;


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


	IEnumerator RotateAround(Vector3 point, Vector3 axis, float angle)
	{
		coroutineRunning = true;
		float t = 0;

		Quaternion[] rotations = new Quaternion[selected.Length];
		Vector3[] positions = new Vector3[selected.Length];

		for (int i = 0; i < rotations.Length; i++)
			rotations[i] = selected[i].rotation * Quaternion.Euler(axis * angle);

		for (int i = 0; i < positions.Length; i++)
			positions[i] = Quaternion.Euler(axis * angle) * selected[i].position;

		while (t < duration)
		{
			foreach (var cubie in selected)
				cubie.RotateAround(point, axis, angle / duration * Time.deltaTime);

			t += Time.deltaTime;
			yield return null;
		}

		for (int i = 0; i < rotations.Length; i++)
		{
			var vec = selected[i].eulerAngles;
			vec.x = Mathf.Round(vec.x / 90) * 90;
			vec.y = Mathf.Round(vec.y / 90) * 90;
			vec.z = Mathf.Round(vec.z / 90) * 90;
			selected[i].eulerAngles = vec;
		}

		for (int i = 0; i < positions.Length; i++)
			selected[i].position = positions[i];

		coroutineRunning = false;
	}


	public void Left()
	{
		if (coroutineRunning)
			return;

		selected = Physics.OverlapBox(handles[0].center, handles[0].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		StartCoroutine(RotateAround(Vector3.left, Vector3.left, angle));
	}

	public void Right()
	{
		if (coroutineRunning)
			return;

		selected = Physics.OverlapBox(handles[1].center, handles[1].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		StartCoroutine(RotateAround(Vector3.right, Vector3.right, angle));
	}

	public void Down()
	{
		if (coroutineRunning)
			return;

		selected = Physics.OverlapBox(handles[2].center, handles[2].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		StartCoroutine(RotateAround(Vector3.down, Vector3.down, angle));
	}

	public void Up()
	{
		if (coroutineRunning)
			return;

		selected = Physics.OverlapBox(handles[3].center, handles[3].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		StartCoroutine(RotateAround(Vector3.down, Vector3.down, angle));
	}

	public void Front()
	{
		if (coroutineRunning)
			return;

		selected = Physics.OverlapBox(handles[4].center, handles[4].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		StartCoroutine(RotateAround(Vector3.back, Vector3.back, angle));
	}

	public void Back()
	{
		if (coroutineRunning)
			return;

		selected = Physics.OverlapBox(handles[5].center, handles[5].halfExtents, Quaternion.identity).Select(c => c.transform).ToArray();
		StartCoroutine(RotateAround(Vector3.forward, Vector3.forward, angle));
	}
}