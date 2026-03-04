using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class EventUIController : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI descriptionText;
    private bool waitingForClose;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    private EventData currentEvent;
    private Action<EventChoiceData> onChoiceSelected;
    private void Update()
    {
        if (waitingForClose && Input.GetMouseButtonDown(0))
        {
            waitingForClose = false;
            HideEvent();
        }
    }
    public void ShowEvent(EventData eventData, Action<EventChoiceData> callback)
    {
        dialogPanel.SetActive(true);

        currentEvent = eventData;
        onChoiceSelected = callback;

        descriptionText.text = eventData.description;

        GenerateChoices(eventData.choices);
    }

    private void GenerateChoices(List<EventChoiceData> choices)
    {
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var choice in choices)
        {
            var btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
            var text = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = choice.buttonText;

            var btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => SelectChoice(choice));
        }
    }

    private void SelectChoice(EventChoiceData choice)
    {
        descriptionText.text = choice.resultText;
        waitingForClose = true;
        foreach (Transform child in choiceContainer)
        {
            child.gameObject.SetActive(false);
        }

        onChoiceSelected?.Invoke(choice);
    }

    public void HideEvent()
    {
        dialogPanel.SetActive(false);
    }
}