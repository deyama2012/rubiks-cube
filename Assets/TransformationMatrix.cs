using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationMatrix : MonoBehaviour
{
	public Transform[] targets;
	public bool coroutineRunning;
	public float duration;


	[ContextMenu("Rotate")]
	public void Rotate()
	{
		if (!coroutineRunning)
			StartCoroutine(RotateCoroutine());
	}




	IEnumerator RotateCoroutine()
	{
		coroutineRunning = true;

		GameObject face = new GameObject("Temporary_container");
		face.transform.position = Vector3.up;

		Transform root = targets[0].parent;

		// Parent
		foreach (var c in targets)
			c.SetParent(face.transform);

		// Quaternion from = face.transform.rotation;
		// Quaternion to = face.transform.rotation * Quaternion.Euler(Vector3.up * 90);
		// float t = 0;
		// while (t < duration)
		// {
		// 	face.transform.rotation = Quaternion.Slerp(from, to, t / duration);
		// 	t += Time.deltaTime;
		// 	yield return null;
		// }
		// face.transform.rotation = to;

		Quaternion rotation = face.transform.rotation * Quaternion.Euler(Vector3.up * 90);
		float t = 0;
		while (t < duration)
		{
			face.transform.Rotate(Vector3.up, 90 / duration * Time.deltaTime);
			t += Time.deltaTime;
			yield return null;
		}
		face.transform.rotation = rotation;

		// Unparent
		foreach (var c in targets)
			c.SetParent(root);

		Destroy(face);

		coroutineRunning = false;
	}
}