using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "Variables/BoolVariable", order = 0)]
public class BoolVariable : ScriptableObject
{
    public bool Value;

    public static implicit operator bool(BoolVariable obj)
    {
        return obj.Value;
    }
}