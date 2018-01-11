using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Moves { F = 0, Fi, B, Bi, L, Li, R, Ri, U, Ui, D, Di, X, Xi, Y, Yi, Z, Zi }

public class MoveHistory : MonoBehaviour
{
	private static MoveHistory _instance;
	public static MoveHistory Instance { get { return _instance; } }

	[SerializeField]
	private List<Moves> moves = new List<Moves>();

	public Text movesLog;

	public bool stopLogging;

	/// ////////////////////////////////////////////////////////////

	void Awake()
	{
		if (_instance != null && _instance != this)
			Destroy(_instance);
		else
			_instance = this;

		movesLog.text = string.Empty;
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

		moves.Add(move);

		movesLog.text += move.ToString() + " ";
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

		moves.Add(move);

		movesLog.text += move.ToString() + " ";
	}
}