using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
	//public Controls controls;
	public Cube rubik;
	public bool counterClockwise;
	public bool relativeToCamera;

	/// /////////////////////////////////////////////////////////////

	void Update()
	{
#if !UNITY_EDITOR
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
			rubik.Undo();
#endif
	}


	#region Cube rotation
	public void RotateCubeX()
	{
		rubik.RotateCube(Vector3.right, counterClockwise ? -90 : 90);
	}


	public void RotateCubeY()
	{
		rubik.RotateCube(Vector3.up, counterClockwise ? -90 : 90);
	}


	public void RotateCubeZ()
	{
		rubik.RotateCube(Vector3.forward, counterClockwise ? -90 : 90);
	}
	#endregion


	#region Face rotation
	public void RotateLeft()
	{
		rubik.RotateFace(Vector3.left, counterClockwise ? -90 : 90, relativeToCamera);
	}

	public void RotateRight()
	{
		rubik.RotateFace(Vector3.right, counterClockwise ? -90 : 90, relativeToCamera);
	}

	public void RotateFront()
	{
		rubik.RotateFace(Vector3.back, counterClockwise ? -90 : 90, relativeToCamera);
	}

	public void RotateBack()
	{
		rubik.RotateFace(Vector3.forward, counterClockwise ? -90 : 90, relativeToCamera);
	}

	public void RotateUp()
	{
		rubik.RotateFace(Vector3.up, counterClockwise ? -90 : 90, relativeToCamera);
	}

	public void RotateDown()
	{
		rubik.RotateFace(Vector3.down, counterClockwise ? -90 : 90, relativeToCamera);
	}
	#endregion


	#region History
	public void Undo()
	{
		rubik.Undo();
	}

	public void UndoAll()
	{
		rubik.UndoAll();
	}

	public void ClearHistory()
	{
		rubik.ClearHistory();
	}
	#endregion


	#region Sequences
	public void DoSequence0()
	{
		rubik.DoSequence(MoveSequence.middleLayer_Clockwise, relativeToCamera);
	}

	public void DoSequence1()
	{
		rubik.DoSequence(MoveSequence.middleLayer_CounterClockwise, relativeToCamera);
	}

	public void DoSequence2()
	{
		rubik.DoSequence(MoveSequence.yellowCrossA, relativeToCamera);
	}

	public void DoSequence3()
	{
		rubik.DoSequence(MoveSequence.yellowCrossB, relativeToCamera);
	}

	public void DoSequence4()
	{
		rubik.DoSequence(MoveSequence.yellowFace, relativeToCamera);
	}

	public void DoSequence5()
	{
		rubik.DoSequence(MoveSequence.yellowCorners, relativeToCamera);
	}

	public void DoSequence6()
	{
		rubik.DoSequence(MoveSequence.yellowEdges_Clockwise, relativeToCamera);
	}

	public void DoSequence7()
	{
		rubik.DoSequence(MoveSequence.yellowEdges_CounterClockwise, relativeToCamera);
	}

	public void DoSequence8()
	{
		rubik.DoSequence(MoveSequence.multiColoredCross, relativeToCamera);
	}

	public void DoSequence9()
	{
		rubik.DoSequence(MoveSequence.squareInTheMiddle, relativeToCamera);
	}
	#endregion


	public void Scramble()
	{
		int steps = 10;
		Moves[] sequence = new Moves[steps];

		sequence[0] = (Moves) UnityEngine.Random.Range(0, 12);

		for (int i = 1; i < steps; i++)
		{
			int move = -1;
			do
			{
				move = UnityEngine.Random.Range(0, 12);
			}
			while ((int) sequence[i - 1] == move + 1 || (int) sequence[i - 1] == move - 1);

			sequence[i] = (Moves) move;
		}

		rubik.DoSequence(sequence, relativeToCamera);
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