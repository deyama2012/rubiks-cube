using UnityEngine;
using UnityEngine.UI;

public class GuiToggle : MonoBehaviour
{
    private Toggle toggle;
    [SerializeField] private BoolVariable counterClockwise;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    void Update()
    {
        toggle.isOn = counterClockwise.Value;
    }

    public void OnToggleValueChanged(Toggle toggle)
    {
        counterClockwise.Value = toggle.isOn;
    }
}
