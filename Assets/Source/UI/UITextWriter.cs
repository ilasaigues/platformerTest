using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UITextWriter : MonoBehaviour
{
    [SerializeField]
    [TextArea(3, 6)]
    private string _unformattedText;

    [SerializeField]
    private List<ScriptableInt> _orderedNumbers;

    private TMPro.TextMeshProUGUI _textMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        _textMeshProUGUI = GetComponent<TMPro.TextMeshProUGUI>();
        UpdateText(0);
    }

    void OnEnable()
    {
        foreach (var variable in _orderedNumbers)
        {
            variable.SubscribeToValueChanged(UpdateText);
        }
    }

    void OnDisable()
    {
        foreach (var variable in _orderedNumbers)
        {
            variable.UnSubscribeToValueChanged(UpdateText);
        }
    }

    private void UpdateText(int value)
    {
        _textMeshProUGUI.text = string.Format(_unformattedText, _orderedNumbers.Select(sv => sv.Value.ToString()).ToArray());
    }
}
