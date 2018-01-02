using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationMatrix : MonoBehaviour
{
	public Transform target;

	[SerializeField]
	bool coroutineRunning;

	[ContextMenu("Rotate")]
	public void Rotate()
	{
		if (!coroutineRunning)
			StartCoroutine(RotateCoroutine());
	}


	IEnumerator RotateCoroutine()
	{
		coroutineRunning = true;

		float duration = 3;
		float t = 0;
		Vector3 startPos = target.position;

		Quaternion rotation = Quaternion.Euler(0, 90, 0);
		Matrix4x4 m = Matrix4x4.Rotate(rotation);
		Vector3 endPos = m.MultiplyPoint(startPos);

		Vector3 lookStartPos = new Vector3(0, 0, startPos.z);
		Vector3 looktEndPos = m.MultiplyPoint(lookStartPos);

		while (t < duration)
		{
			target.position = Vector3.Slerp(startPos, endPos, t / duration);

			Vector3 lookAt = Vector3.Slerp(lookStartPos, looktEndPos, t / duration);
			target.LookAt(lookAt);

			t += Time.deltaTime;

			yield return null;
		}

		target.position = endPos;

		coroutineRunning = false;
	}


	// IEnumerator RotateCoroutine()
	// {
	// 	float duration = 2;
	// 	float t = 0;
	// 	Vector3 startPos = target.position;

	// 	while (t < duration)
	// 	{
	// 		Quaternion rotation = Quaternion.Euler(0, Mathf.Lerp(0, 90, t / duration), 0);
	// 		Matrix4x4 m = Matrix4x4.Rotate(rotation);

	// 		target.position = m.MultiplyPoint3x4(startPos);

	// 		t += Time.deltaTime;

	// 		yield return null;
	// 	}
	// }
}