using System;
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
	
	
	#region Sequences
	public void DoSequence0()
	{
		rubiksCube.DoSequence(MoveSequence.middleLayer_Clockwise);
	}
	
	public void DoSequence1()
	{
		rubiksCube.DoSequence(MoveSequence.middleLayer_CounterClockwise);
	}
	
	public void DoSequence2()
	{
		rubiksCube.DoSequence(MoveSequence.yellowCrossA);
	}
	
	public void DoSequence3()
	{
		rubiksCube.DoSequence(MoveSequence.yellowCrossB);
	}
	
	public void DoSequence4()
	{
		rubiksCube.DoSequence(MoveSequence.yellowFace);
	}
	
	public void DoSequence5()
	{
		rubiksCube.DoSequence(MoveSequence.yellowCorners);
	}
	
	public void DoSequence6()
	{
		rubiksCube.DoSequence(MoveSequence.yellowEdges_Clockwise);
	}
	
	public void DoSequence7()
	{
		rubiksCube.DoSequence(MoveSequence.yellowEdges_CounterClockwise);
	}
	
	public void DoSequence8()
	{
		rubiksCube.DoSequence(MoveSequence.multiColoredCross);
	}
	
	public void DoSequence9()
	{
		rubiksCube.DoSequence(MoveSequence.squareInTheMiddle);
	}
	#endregion
	
	
	public void Scramble()
	{
		int steps = 10;
		Moves[] sequence = new Moves[steps];
		
		sequence[0] = (Moves)UnityEngine.Random.Range(0, 12);
		
		for(int i = 1; i < steps; i++)
		{
			int move = -1;
			do
			{
				move = UnityEngine.Random.Range(0, 12);
			}
			while((int)sequence[i-1] == move + 1 || (int)sequence[i-1] == move - 1);
			
			sequence[i] = (Moves)move;
		}
		
		rubiksCube.DoSequence(sequence);
	}


	public void OnToggleValueChanged(Toggle toggle)
	{
		counterClockwise = toggle.isOn;
	}
}

[Serializable]
public class Controls
{
	public KeyCode front = KeyCode.F;
	public KeyCode back = KeyCode.B;
	public KeyCode left = KeyCode.L;
	public KeyCode right = KeyCode.R;
	public KeyCode up = KeyCode.U;
	public KeyCode down = KeyCode.D;
	public KeyCode x = KeyCode.X;
	public KeyCode y = KeyCode.Y;
	public KeyCode z = KeyCode.Z;
}