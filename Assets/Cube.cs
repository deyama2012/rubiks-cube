using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cube : MonoBehaviour
{
	Transform[][][] cubeMatrix = new Transform[3][][];
	public GameObject cubiePrefab;
	public Transform[] selected;
	public int angle = 90;
	public float duration = 0.5f;
	private bool coroutineRunning;

	/// ///////////////////////////////////////////////

	void Start()
	{
		for (int x = 0; x < 3; x++)
		{
			cubeMatrix[x] = new Transform[3][];

			for (int y = 0; y < 3; y++)
			{
				cubeMatrix[x][y] = new Transform[3];

				for (int z = 0; z < 3; z++)
				{
					cubeMatrix[x][y][z] = Instantiate(cubiePrefab, new Vector3(x - 1, y - 1, z - 1), Quaternion.identity).transform;
					cubeMatrix[x][y][z].gameObject.name += x.ToString() + y.ToString() + z.ToString();
					cubeMatrix[x][y][z].GetComponent<Cubie>().SetAllSides(Color.black);
					cubeMatrix[x][y][z].transform.SetParent(transform);
				}
			}
		}

		for (int i = 0; i < 3; i++)
			for (int j = 0; j < 3; j++)
			{
				cubeMatrix[i][j][0].GetComponent<Cubie>().SetColor(0, Color.white);
				cubeMatrix[i][j][2].GetComponent<Cubie>().SetColor(1, Color.yellow);
				cubeMatrix[0][i][j].GetComponent<Cubie>().SetColor(2, Color.red);
				cubeMatrix[2][i][j].GetComponent<Cubie>().SetColor(3, new Color(1, 0.5f, 0));
				cubeMatrix[i][2][j].GetComponent<Cubie>().SetColor(4, Color.green);
				cubeMatrix[i][0][j].GetComponent<Cubie>().SetColor(5, Color.blue);
			}
	}


	public void Left()
	{
		if (coroutineRunning)
			return;

		selected = new Transform[9];

		int k = 0;
		for (int i = 0; i < 3; i++)
			for (int j = 0; j < 3; j++, k++)
				selected[k] = cubeMatrix[0][i][j];

		StartCoroutine(RotateAround(Vector3.left, Vector3.left, angle));
	}

	[ContextMenu("Print left face")]
	public void Print()
	{
		string s = string.Empty;

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				s += cubeMatrix[0][i][j].name + "\t\t\t";
			}
			s += "\r\n";
		}

		print(s);
	}


	IEnumerator RotateAround(Vector3 point, Vector3 axis, float angle)
	{
		coroutineRunning = true;
		float t = 0;

		Vector3[] positions = new Vector3[selected.Length];
		Quaternion[] rotations = new Quaternion[selected.Length];

		//Vector3[] oldPositions = new Vector3[selected.Length];
		// for (int i = 0; i < positions.Length; i++)
		// 	oldPositions[i] = selected[i].position;

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

		// Snap rotation
		for (int i = 0; i < rotations.Length; i++)
		{
			var vec = selected[i].eulerAngles;
			vec.x = Mathf.Round(vec.x / 90) * 90;
			vec.y = Mathf.Round(vec.y / 90) * 90;
			vec.z = Mathf.Round(vec.z / 90) * 90;
			selected[i].eulerAngles = vec;
		}

		// Snap position
		for (int i = 0; i < positions.Length; i++)
			selected[i].position = positions[i];

		// Update cube matrix
		for (int i = 0; i < positions.Length; i++)
		{
			int x = Mathf.RoundToInt(positions[i].x + 1);
			int y = Mathf.RoundToInt(positions[i].y + 1);
			int z = Mathf.RoundToInt(positions[i].z + 1);

			cubeMatrix[x][y][z] = selected[i];
		}

		// for (int i = 0; i < selected.Length; i++)
		// {
		// 	selected[i].rotation = Quaternion.identity;
		// 	selected[i].position = oldPositions[i];
		// 	selected[i].GetComponent<Cubie>().Rotate(axis);
		// }

		coroutineRunning = false;
	}
}