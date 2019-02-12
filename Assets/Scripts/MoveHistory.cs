using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;



public class MoveHistory : MonoBehaviour
{
	[SerializeField]
	private List<Moves> listMoveNames = new List<Moves>();
	private List<MoveInfo> listMoveInfo = new List<MoveInfo>();

    [SerializeField]
    private Text textLog;

    [SerializeField]
    private bool stopLogging;

	/// ////////////////////////////////////////////////////////////

	void Awake()
	{
		textLog.text = string.Empty;

		listMoveNames.Clear();
	}
	

	public void RememberFaceMove(Vector3 axis, int angle)
	{
		if (stopLogging)
			return;

		Moves move;

		if (axis == Vector3.forward)
			move = angle > 0 ? Moves.B : Moves.Bi;

		else if (axis == Vector3.back)
			move = angle > 0 ? Moves.F : Moves.Fi;

		else if (axis == Vector3.left)
			move = angle > 0 ? Moves.L : Moves.Li;

		else if (axis == Vector3.right)
			move = angle > 0 ? Moves.R : Moves.Ri;

		else if (axis == Vector3.up)
			move = angle > 0 ? Moves.U : Moves.Ui;

		else
			move = angle > 0 ? Moves.D : Moves.Di;

		listMoveNames.Add(move);
		listMoveInfo.Add(new MoveInfo(RotationType.Face, axis, angle));

		RefreshLog();
	}


	public void RememberCubeMove(Vector3 axis, int angle)
	{
		if (stopLogging)
			return;

		Moves move;

		if (axis == Vector3.right)
			move = angle > 0 ? Moves.X : Moves.Xi;

		else if (axis == Vector3.forward)
			move = angle > 0 ? Moves.Z : Moves.Yi;

		else
			move = angle > 0 ? Moves.Y : Moves.Zi;

		listMoveNames.Add(move);
		listMoveInfo.Add(new MoveInfo(RotationType.Cube, axis, angle));

		RefreshLog();
	}


	public bool TryUndo(out MoveInfo moveInfo)
	{
		if (listMoveNames.Count > 0 && listMoveInfo.Count > 0)
		{
			listMoveNames.RemoveAt(listMoveNames.Count - 1);
			moveInfo = listMoveInfo[listMoveInfo.Count - 1];
			moveInfo.angle = -moveInfo.angle;
			listMoveInfo.RemoveAt(listMoveInfo.Count - 1);
			RefreshLog();
			return true;
		}
		moveInfo = default(MoveInfo);
		return false;
	}


	void RefreshLog()
	{
		StringBuilder builder = new StringBuilder();

		foreach (var move in listMoveNames)
			builder.Append(move + " ");

		textLog.text = builder.ToString();
	}
	
	
	public void ClearHistory()
	{
		listMoveNames.Clear();
		listMoveInfo.Clear();
		RefreshLog();
	}
}

[Serializable]
public struct MoveInfo
{
	public RotationType type;
	public Vector3 axis;
	public int angle;

	public MoveInfo(RotationType type, Vector3 axis, int angle)
	{
		this.type = type;
		this.axis = axis;
		this.angle = angle;
	}
}