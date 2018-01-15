using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
	public Controls controls;
	private Cube rubiksCube;
	public bool counterClockwise;

	/// /////////////////////////////////////////////////////////////

	void Start()
	{
		rubiksCube = GetComponent<Cube>();
	}

	void Update()
	{
#if !UNITY_EDITOR
		PlayerInput();
#endif
	}

	void PlayerInput()
	{
		counterClockwise = Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);

		if (Input.GetKeyDown(controls.front))
			RotateFront();

		else if (Input.GetKeyDown(controls.back))
			RotateBack();

		else if (Input.GetKeyDown(controls.left))
			RotateLeft();

		else if (Input.GetKeyDown(controls.right))
			RotateRight();

		else if (Input.GetKeyDown(controls.up))
			RotateUp();

		else if (Input.GetKeyDown(controls.down))
			RotateDown();

		else if (Input.GetKeyDown(controls.x))
			RotateCubeX();

		else if (Input.GetKeyDown(controls.y))
			RotateCubeY();

		else if (Input.GetKeyDown(controls.z))
			RotateCubeZ();
	}


	#region Cube rotation
	public void RotateCubeX()
	{
		rubiksCube.RotateCube(Vector3.right, counterClockwise ? -90 : 90);
	}


	public void RotateCubeY()
	{
		rubiksCube.RotateCube(Vector3.up, counterClockwise ? -90 : 90);
	}


	public void RotateCubeZ()
	{
		rubiksCube.RotateCube(Vector3.forward, counterClockwise ? -90 : 90);
	}
	#endregion


	#region Face rotation
	public void RotateLeft()
	{
		rubiksCube.RotateFace(Vector3.left, counterClockwise ? -90 : 90);
	}


	public void RotateRight()
	{
		rubiksCube.RotateFace(Vector3.right, counterClockwise ? -90 : 90);
	}


	public void RotateFront()
	{
		rubiksCube.RotateFace(Vector3.back, counterClockwise ? -90 : 90);
	}


	public void RotateBack()
	{
		rubiksCube.RotateFace(Vector3.forward, counterClockwise ? -90 : 90);
	}


	public void RotateUp()
	{
		rubiksCube.RotateFace(Vector3.up, counterClockwise ? -90 : 90);
	}


	public void RotateDown()
	{
		rubiksCube.RotateFace(Vector3.down, counterClockwise ? -90 : 90);
	}
	#endregion


	#region History
	public void Undo()
	{
		rubiksCube.Undo();
	}

	public void UndoAll()
	{
		rubiksCube.UndoAll();
	}

	public void ClearHistory()
	{
		rubiksCube.ClearHistory();
	}
	#endregion


	public void OnToggleValueChanged(Toggle toggle)
	{
		counterClockwise = toggle.isOn;
	}
}

[Serializable]
public class Controls
{
	public KeyCode front, back, left, right, up, down, x, y, z;
}