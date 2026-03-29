using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventChoiceUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button button;

    EventChoiceSO data;
    DialogueSystem dialogue;

    public void Setup(EventChoiceSO c, DialogueSystem d)
    {
        data = c;
        dialogue = d;

        text.text = c.description;

        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        dialogue.OnChoiceSelected(data);
    }
}