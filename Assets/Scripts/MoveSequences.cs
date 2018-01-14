using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveSequences
{
	public static MoveInfo[] sequenceA =
	{
		new MoveInfo(RotationType.Face, Vector3.down, -90),
		new MoveInfo(RotationType.Face, Vector3.right, -90),
		new MoveInfo(RotationType.Face, Vector3.down, 90),
		new MoveInfo(RotationType.Face, Vector3.right, 90)
	};
	
	public static Moves[] sequenceB =
	{
		Moves.Di,
		Moves.R,
		Moves.D,
		Moves.Ri
	};
	
	public static Dictionary<Moves, Vector3> AxisFromMoveName = new Dictionary<Moves, Vector3>()
	{
		{Moves.F, Vector3.back},
		{Moves.Fi, Vector3.back},
		{Moves.B, Vector3.forward},
		{Moves.Bi, Vector3.forward},
		{Moves.L, Vector3.left},
		{Moves.Li, Vector3.left},
		{Moves.R, Vector3.right},
		{Moves.Ri, Vector3.right},
		{Moves.U, Vector3.up},
		{Moves.Ui, Vector3.up},
		{Moves.D, Vector3.down},
		{Moves.Di, Vector3.down}
	};
	
	public static int AngleFromMoveName(Moves move)
	{
		return ((int)move % 2) == 0 ? 90 : -90;
	}
}
