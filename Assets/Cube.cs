using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
	public GameObject cubiePrefab;

	public float duration = 0.5f;
	public bool counterClockwise;

	private Transform[][][] cubeMatrix = new Transform[3][][];
	private Transform[] selected;
	private bool coroutineRunning;

	[Header("- Controls -")]
	public KeyCode front;
	public KeyCode back, left, right, up, down;

	public bool relativeToCamera;

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
				{
					cubies[m] = cubeMatrix[x][y][z];
				}

		return cubies;
	}


	void Update()
	{
		//PlayerInput();

		Vector3[] xyz = GetCameraRelativeForwardRightUpVectors();

		Vector3 x = xyz[0];
		Vector3 y = xyz[1];
		Vector3 z = xyz[2];

		Debug.DrawRay(transform.position, z * 3, Color.blue);
		Debug.DrawRay(transform.position, x * 3, Color.red);
		Debug.DrawRay(transform.position, y * 3, Color.green);
	}


	Vector3[] GetCameraRelativeForwardRightUpVectors()
	{
		Vector3[] normals = { transform.forward, -transform.forward, transform.right, -transform.right, transform.up, -transform.up };

		float maxDotProductZ = 0;
		Vector3 z = Vector3.zero;
		foreach (var normal in normals)
		{
			float dotProductZ = Vector3.Dot(-Camera.main.transform.forward, normal);

			if (dotProductZ > maxDotProductZ)
			{
				maxDotProductZ = dotProductZ;
				z = normal;
			}
		}

		float maxDotProductY = 0;
		Vector3 y = Vector3.zero;
		foreach (var normal in normals)
		{
			if (normal == z || normal == -z)
				continue;

			float dotProductY = Vector3.Dot(Camera.main.transform.up, normal);

			if (dotProductY > maxDotProductY)
			{
				maxDotProductY = dotProductY;
				y = normal;
			}
		}

		Vector3 x = Vector3.Cross(z, y);

		return new [] { x, y, z };
	}


	void PlayerInput()
	{
		counterClockwise = Input.GetKey(KeyCode.LeftControl);

		if (Input.GetKeyDown(front))
		{
			RotateFront();
			return;
		}

		if (Input.GetKeyDown(back))
		{
			RotateBack();
			return;
		}

		if (Input.GetKeyDown(left))
		{
			RotateLeft();
			return;
		}

		if (Input.GetKeyDown(right))
		{
			RotateRight();
			return;
		}

		if (Input.GetKeyDown(up))
		{
			RotateUp();
			return;
		}

		if (Input.GetKeyDown(down))
		{
			RotateDown();
			return;
		}
	}


	public void RotateCubeX()
	{
		if (coroutineRunning)
			return;

		selected = GetAllCubies();

		int angle = counterClockwise ? -90 : 90;
		StartCoroutine(RotateCoroutine(Vector3.zero, Vector3.right, angle));
	}


	public void RotateCubeY()
	{
		if (coroutineRunning)
			return;

		selected = GetAllCubies();

		int angle = counterClockwise ? -90 : 90;
		StartCoroutine(RotateCoroutine(Vector3.zero, Vector3.up, angle));
	}


	public void RotateCubeZ()
	{
		if (coroutineRunning)
			return;

		selected = GetAllCubies();

		int angle = counterClockwise ? -90 : 90;
		StartCoroutine(RotateCoroutine(Vector3.zero, Vector3.forward, angle));
	}


	IEnumerator RotateCoroutine(Vector3 point, Vector3 axis, float angle)
	{
		coroutineRunning = true;

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


	public void RotateLeft()
	{
		if (coroutineRunning)
			return;

		Vector3 axis = Vector3.left;

		if (relativeToCamera)
			axis = -GetCameraRelativeForwardRightUpVectors()[0];

		selected = GetFaceCubies(axis);

		int angle = counterClockwise ? -90 : 90;
		StartCoroutine(RotateCoroutine(axis, axis, angle));
	}


	public void RotateRight()
	{
		if (coroutineRunning)
			return;

		Vector3 axis = Vector3.right;

		if (relativeToCamera)
			axis = GetCameraRelativeForwardRightUpVectors()[0];

		selected = GetFaceCubies(axis);

		int angle = counterClockwise ? -90 : 90;
		StartCoroutine(RotateCoroutine(axis, axis, angle));
	}


	public void RotateFront()
	{
		if (coroutineRunning)
			return;

		Vector3 axis = Vector3.back;

		if (relativeToCamera)
			axis = GetCameraRelativeForwardRightUpVectors()[2];

		selected = GetFaceCubies(axis);

		int angle = counterClockwise ? -90 : 90;
		StartCoroutine(RotateCoroutine(axis, axis, angle));
	}


	public void RotateBack()
	{
		if (coroutineRunning)
			return;

		Vector3 axis = Vector3.forward;

		if (relativeToCamera)
			axis = -GetCameraRelativeForwardRightUpVectors()[2];

		selected = GetFaceCubies(axis);

		int angle = counterClockwise ? -90 : 90;
		StartCoroutine(RotateCoroutine(axis, axis, angle));

	}


	public void RotateUp()
	{
		if (coroutineRunning)
			return;

		Vector3 axis = Vector3.up;

		if (relativeToCamera)
			axis = GetCameraRelativeForwardRightUpVectors()[1];

		selected = GetFaceCubies(axis);

		int angle = counterClockwise ? -90 : 90;
		StartCoroutine(RotateCoroutine(axis, axis, angle));
	}


	public void RotateDown()
	{
		if (coroutineRunning)
			return;

		Vector3 axis = Vector3.up;

		if (relativeToCamera)
			axis = -GetCameraRelativeForwardRightUpVectors()[1];

		selected = GetFaceCubies(axis);

		int angle = counterClockwise ? -90 : 90;
		StartCoroutine(RotateCoroutine(axis, axis, angle));
	}


	public void OnToggleValueChanged(Toggle toggle)
	{
		counterClockwise = toggle.isOn;
	}


	static Vector3 RoundToInt(Vector3 eulerAngles)
	{
		eulerAngles.x = Mathf.Round(eulerAngles.x / 90) * 90;
		eulerAngles.y = Mathf.Round(eulerAngles.y / 90) * 90;
		eulerAngles.z = Mathf.Round(eulerAngles.z / 90) * 90;

		return eulerAngles;
	}
}