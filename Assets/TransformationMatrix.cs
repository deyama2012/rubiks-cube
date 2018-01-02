using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationMatrix : MonoBehaviour
{
	public Transform[] targets;

	bool coroutineRunning;

	public float duration;


	[ContextMenu("Rotate")]
	public void Rotate()
	{
		if (!coroutineRunning)
			StartCoroutine(RotateCoroutine(duration));
	}


	IEnumerator RotateCoroutine(float duration)
	{
		coroutineRunning = true;

		int count = targets.Length; // always 9 for Rubic's Cube layer

		float t = 0;

		Quaternion rotation = Quaternion.Euler(0, 90, 0);
		Matrix4x4 m = Matrix4x4.Rotate(rotation);

		Vector3[] startPos = new Vector3[count];
		Vector3[] endPos = new Vector3[count];
		Vector3[] lookStartPos = new Vector3[count];
		Vector3[] looktEndPos = new Vector3[count];

		for (int i = 0; i < count; i++)
		{
			startPos[i] = targets[i].position;
			endPos[i] = m.MultiplyPoint(startPos[i]);
			lookStartPos[i] = targets[i].position + targets[i].forward;
			looktEndPos[i] = m.MultiplyPoint(lookStartPos[i]);
		}

		while (t < duration)
		{
			for (int i = 0; i < count; i++)
			{
				targets[i].position = Vector3.Slerp(startPos[i], endPos[i], t / duration);

				Vector3 lookAt = Vector3.Slerp(lookStartPos[i], looktEndPos[i], t / duration);
				targets[i].LookAt(lookAt);
			}

			t += Time.deltaTime;

			yield return null;
		}

		for (int i = 0; i < count; i++)
		{
			targets[i].position = endPos[i];
			targets[i].LookAt(looktEndPos[i]);
		}

		coroutineRunning = false;
	}
}