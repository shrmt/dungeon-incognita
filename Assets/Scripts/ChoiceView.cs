using System;
using TMPro;
using UnityEngine;

public class ChoiceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI choiceText;
    [SerializeField] private ButtonView[] optionViews;

    public void Show(string text, OptionViewModel[] options, Action<int> onChoosed)
    {
        foreach (var optionView in optionViews)
        {
            optionView.gameObject.SetActive(false);
        }

        for (int i = 0; i < options.Length; i++)
        {
            var optionView = optionViews[i];
            optionView.gameObject.SetActive(true);
            var optionIndex = i;
            optionView.Setup(options[i].Text, options[i].IsAvailable, () =>
            {
                onChoosed(optionIndex);
                gameObject.SetActive(false);
            });
        }

        choiceText.text = text;

        gameObject.SetActive(true);
    }
}

public class OptionViewModel
{
    public readonly string Text;
    public readonly bool IsAvailable;

    public OptionViewModel(string text, bool isAvailable)
    {
        Text = text;
        IsAvailable = isAvailable;
    }
}
