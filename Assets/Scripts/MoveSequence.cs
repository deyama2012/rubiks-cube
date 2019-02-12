using System.Collections.Generic;
using UnityEngine;

public static class MoveSequence
{
    public static Moves[] middleLayer_Clockwise =
    {
        Moves.U,
        Moves.R,
        Moves.Ui,
        Moves.Ri,
        Moves.Ui,
        Moves.Fi,
        Moves.U,
        Moves.F
    };

    public static Moves[] middleLayer_CounterClockwise =
    {
        Moves.Ui,
        Moves.Li,
        Moves.U,
        Moves.L,
        Moves.U,
        Moves.F,
        Moves.Ui,
        Moves.Fi
    };

    public static Moves[] yellowCrossA =
    {
        Moves.F,
        Moves.U,
        Moves.R,
        Moves.Ui,
        Moves.Ri,
        Moves.Fi
    };

    public static Moves[] yellowCrossB =
    {
        Moves.F,
        Moves.R,
        Moves.U,
        Moves.Ri,
        Moves.Ui,
        Moves.Fi
    };

    public static Moves[] yellowFace =
    {
        Moves.R,
        Moves.U,
        Moves.Ri,
        Moves.U,
        Moves.R,
        Moves.U,
        Moves.U,
        Moves.Ri
    };

    public static Moves[] yellowCorners =
    {
        Moves.Ri,
        Moves.F,
        Moves.Ri,
        Moves.B,
        Moves.B,
        Moves.R,
        Moves.Fi,
        Moves.Ri,
        Moves.B,
        Moves.B,
        Moves.R,
        Moves.R,
        Moves.Ui
    };

    public static Moves[] yellowEdges_Clockwise =
    {
        Moves.F,
        Moves.F,
        Moves.U,
        Moves.L,
        Moves.Ri,
        Moves.F,
        Moves.F,
        Moves.Li,
        Moves.R,
        Moves.U,
        Moves.F,
        Moves.F
    };

    public static Moves[] yellowEdges_CounterClockwise =
    {
        Moves.F,
        Moves.F,
        Moves.Ui,
        Moves.L,
        Moves.Ri,
        Moves.F,
        Moves.F,
        Moves.Li,
        Moves.R,
        Moves.Ui,
        Moves.F,
        Moves.F
    };

    public static Moves[] multiColoredCross =
    {
        Moves.R,
        Moves.R,
        Moves.L,
        Moves.L,
        Moves.U,
        Moves.U,
        Moves.D,
        Moves.D,
        Moves.F,
        Moves.F,
        Moves.B,
        Moves.B
    };

    public static Moves[] squareInTheMiddle =
    {
        Moves.R,
        Moves.Li,
        Moves.U,
        Moves.Di,
        Moves.Fi,
        Moves.B,
        Moves.R,
        Moves.Li
    };

    public static Dictionary<Moves, Vector3> AxisFromMoveName = new Dictionary<Moves, Vector3>
    {
        { Moves.F, Vector3.back },
        { Moves.Fi, Vector3.back },
        { Moves.B, Vector3.forward },
        { Moves.Bi, Vector3.forward },
        { Moves.L, Vector3.left },
        { Moves.Li, Vector3.left },
        { Moves.R, Vector3.right },
        { Moves.Ri, Vector3.right },
        { Moves.U, Vector3.up },
        { Moves.Ui, Vector3.up },
        { Moves.D, Vector3.down },
        { Moves.Di, Vector3.down }
    };

    public static int AngleFromMoveName(Moves move)
    {
        return ((int)move % 2) == 0 ? 90 : -90;
    }
}