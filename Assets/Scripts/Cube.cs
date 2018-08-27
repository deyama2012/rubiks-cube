using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum RotationType { Face, Cube }

public class Cube : MonoBehaviour
{
	public GameObject cubiePrefab;

	[Range(0.3f, 2f)]
	public float duration = 0.5f;

	private Transform[][][] cubeMatrix = new Transform[3][][];
	private Transform[] selected;
	private bool coroutineRunning;

	//public bool relativeToCamera;

	private MoveHistory moveHistory;

	/// ///////////////////////////////////////////////

	void Awake()
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
				cubeMatrix[2][i][j].GetComponent<Cubie>().SetColor(3, new Color(1, 0.33f, 0));
				cubeMatrix[i][2][j].GetComponent<Cubie>().SetColor(4, Color.green);
				cubeMatrix[i][0][j].GetComponent<Cubie>().SetColor(5, Color.blue);
			}

		moveHistory = GetComponent<MoveHistory>();
	}


	Transform[] GetFaceCubies(Vector3 faceNormal)
	{
		Transform[] cubies = new Transform[9];

		int x = (int) faceNormal.x;
		int y = (int) faceNormal.y;
		int z = (int) faceNormal.z;

		int k = 0;

		if (x != 0)
		{
			x += 1;

			for (int i = y; i < 3; i++)
				for (int j = z; j < 3; j++, k++)
					cubies[k] = cubeMatrix[x][i][j];
		}

		else if (y != 0)
		{
			y += 1;

			for (int i = x; i < 3; i++)
				for (int j = z; j < 3; j++, k++)
					cubies[k] = cubeMatrix[i][y][j];
		}

		else if (z != 0)
		{
			z += 1;

			for (int i = x; i < 3; i++)
				for (int j = y; j < 3; j++, k++)
					cubies[k] = cubeMatrix[i][j][z];
		}

		return cubies;
	}


	Transform[] GetAllCubies()
	{
		Transform[] cubies = new Transform[27];

		int m = 0;
		for (int x = 0; x < 3; x++)
			for (int y = 0; y < 3; y++)
				for (int z = 0; z < 3; z++, m++)
					cubies[m] = cubeMatrix[x][y][z];

		return cubies;
	}


	//	void Update()
	//	{
	//		Vector3 x = GetCameraAlignedVector(transform.right);
	//		Vector3 y = GetCameraAlignedVector(transform.up);
	//		Vector3 z = GetCameraAlignedVector(-transform.forward);
	//
	//		Debug.DrawRay(transform.position, z * 3, Color.blue);
	//		Debug.DrawRay(transform.position, x * 3, Color.red);
	//		Debug.DrawRay(transform.position, y * 3, Color.green);
	//	}


	Vector3 GetCameraAlignedVector(Vector3 vector)
	{
		Vector3[] normals = { transform.forward, -transform.forward, transform.right, -transform.right, transform.up, -transform.up };

		// Forward
		float maxDotProductZ = 0;
		Vector3 forward = Vector3.zero;
		foreach (var normal in normals)
		{
			float dotProductZ = Vector3.Dot(Camera.main.transform.forward, normal);

			if (dotProductZ > maxDotProductZ)
			{
				maxDotProductZ = dotProductZ;
				forward = normal;
			}
		}

		// Up
		float maxDotProductY = 0;
		Vector3 up = Vector3.zero;
		foreach (var normal in normals)
		{
			if (normal == forward || normal == -forward)
				continue;

			float dotProductY = Vector3.Dot(Camera.main.transform.up, normal);

			if (dotProductY > maxDotProductY)
			{
				maxDotProductY = dotProductY;
				up = normal;
			}
		}

		Quaternion rotation = Quaternion.LookRotation(forward, up);
		return rotation * vector;
	}


	Quaternion GetCameraRotation()
	{
		Vector3[] normals = { transform.forward, -transform.forward, transform.right, -transform.right, transform.up, -transform.up };

		// Forward
		float maxDotProductZ = 0;
		Vector3 forward = Vector3.zero;
		foreach (var normal in normals)
		{
			float dotProductZ = Vector3.Dot(Camera.main.transform.forward, normal);

			if (dotProductZ > maxDotProductZ)
			{
				maxDotProductZ = dotProductZ;
				forward = normal;
			}
		}

		// Up
		float maxDotProductY = 0;
		Vector3 up = Vector3.zero;
		foreach (var normal in normals)
		{
			if (normal == forward || normal == -forward)
				continue;

			float dotProductY = Vector3.Dot(Camera.main.transform.up, normal);

			if (dotProductY > maxDotProductY)
			{
				maxDotProductY = dotProductY;
				up = normal;
			}
		}

		return Quaternion.LookRotation(forward, up);
	}


	IEnumerator RotateCoroutine(RotationType rotationType, Vector3 axis, float angle)
	{
		coroutineRunning = true;

		Vector3 point = rotationType == RotationType.Face ? axis : Vector3.zero;

		Vector3[] positions = new Vector3[selected.Length];
		Quaternion[] rotations = new Quaternion[selected.Length];

		for (int i = 0; i < rotations.Length; i++)
			rotations[i] = selected[i].rotation * Quaternion.Euler(axis * angle);

		for (int i = 0; i < positions.Length; i++)
			positions[i] = Quaternion.Euler(axis * angle) * selected[i].position;

		float t = 0;

		while (t < duration)
		{
			foreach (var cubie in selected)
				cubie.RotateAround(point, axis, angle / duration * Time.deltaTime);

			t += Time.deltaTime;
			yield return null;
		}

		// Snap cubies' rotations
		for (int i = 0; i < rotations.Length; i++)
			selected[i].eulerAngles = RoundToInt(selected[i].eulerAngles);

		// Snap cubies' positions
		for (int i = 0; i < positions.Length; i++)
			selected[i].position = positions[i];

		// Update cube matrix based on cubies' current world positions
		for (int i = 0; i < positions.Length; i++)
		{
			int x = Mathf.RoundToInt(positions[i].x + 1);
			int y = Mathf.RoundToInt(positions[i].y + 1);
			int z = Mathf.RoundToInt(positions[i].z + 1);

			cubeMatrix[x][y][z] = selected[i];
		}

		coroutineRunning = false;
	}


	public void Undo()
	{
		if (coroutineRunning)
			return;

		MoveInfo info;
		if (moveHistory.TryUndo(out info))
		{
			if (info.type == RotationType.Face)
				selected = GetFaceCubies(info.axis);
			else
				selected = GetAllCubies();

			StartCoroutine(RotateCoroutine(info.type, info.axis, info.angle));
		}
	}


	public void UndoAll()
	{
		if (coroutineRunning)
			return;

		StartCoroutine(UndoAllCoroutine());
	}


	IEnumerator UndoAllCoroutine()
	{
		MoveInfo info;
		while (moveHistory.TryUndo(out info))
		{
			if (info.type == RotationType.Face)
				selected = GetFaceCubies(info.axis);
			else
				selected = GetAllCubies();

			yield return StartCoroutine(RotateCoroutine(info.type, info.axis, info.angle));
		}
	}


	public void ClearHistory()
	{
		if (!coroutineRunning)
			moveHistory.ClearHistory();
	}


	public void DoSequence(Moves[] moveSequence, bool relativeToCamera)
	{
		if (!coroutineRunning)
			StartCoroutine(DoSequenceCoroutine(moveSequence, relativeToCamera));
	}


	// TODO Cache camera position and use it throughout the sequence
	IEnumerator DoSequenceCoroutine(Moves[] moveSequence, bool relativeToCamera)
	{
		Quaternion camRot = GetCameraRotation();

		foreach (var move in moveSequence)
		{
			Vector3 axis = MoveSequence.AxisFromMoveName[move];

			if (relativeToCamera)
				axis = camRot * axis;

			int angle = MoveSequence.AngleFromMoveName(move);
			RotationType type = (int) move < 12 ? RotationType.Face : RotationType.Cube;

			if (type == RotationType.Face)
			{
				selected = GetFaceCubies(axis);
				moveHistory.RememberFaceMove(axis, angle);
			}
			else
			{
				selected = GetAllCubies();
				moveHistory.RememberCubeMove(axis, angle);
			}

			yield return StartCoroutine(RotateCoroutine(type, axis, angle));
		}
	}


	public void RotateCube(Vector3 axis, int angle)
	{
		if (coroutineRunning)
			return;

		selected = GetAllCubies();

		moveHistory.RememberCubeMove(axis, angle);
		StartCoroutine(RotateCoroutine(RotationType.Cube, axis, angle));
	}


	public void RotateFace(Vector3 axis, int angle, bool relativeToCamera)
	{
		if (coroutineRunning)
			return;

		if (relativeToCamera)
			axis = GetCameraAlignedVector(axis);

		selected = GetFaceCubies(axis);

		moveHistory.RememberFaceMove(axis, angle);
		StartCoroutine(RotateCoroutine(RotationType.Face, axis, angle));
	}


	static Vector3 RoundToInt(Vector3 eulerAngles)
	{
		eulerAngles.x = Mathf.Round(eulerAngles.x / 90) * 90;
		eulerAngles.y = Mathf.Round(eulerAngles.y / 90) * 90;
		eulerAngles.z = Mathf.Round(eulerAngles.z / 90) * 90;

		return eulerAngles;
	}
}