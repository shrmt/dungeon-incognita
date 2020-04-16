using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button button;

    public void Setup(string text, bool isAvailable, Action onClick)
    {
        buttonText.text = text;
        button.interactable = isAvailable;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());
    }
}
