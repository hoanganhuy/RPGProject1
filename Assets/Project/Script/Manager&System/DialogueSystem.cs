using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogPanel;

    public Image npcArt;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI description;

    public Button continueButton;

    public NPCDatabase npcDB;

    public Transform choiceRoot;
    public GameObject choicePrefab;

    EventSO currentEvent;
    int dialogueIndex;

    void Start()
    {
        dialogPanel.SetActive(false);
        continueButton.onClick.AddListener(Continue);
    }

    // ===== START EVENT =====
    public void StartEventDialogue(EventSO e)
    {
        currentEvent = e;
        dialogueIndex = 0;

        GameManager.Instance.locationSystem.OpenAndCLoseMapLocation(false);
        GameManager.Instance.ChangeMode(GameMode.InDialogue);
        dialogPanel.SetActive(true);

        ShowCurrentLine();
        ClearChoices();
    }

    void ShowCurrentLine()
    {
        var line = currentEvent.dialogues[dialogueIndex];

        description.text = line.text;

        if (!string.IsNullOrEmpty(line.npcId))
        {
            var npc = npcDB.Get(line.npcId);
            npcArt.sprite = npc.portrait;
            npcName.text = npc.displayName;
        }
        else
        {
            npcArt.sprite = null;
            npcName.text = "";
        }
    }

    void Continue()
    {
        dialogueIndex++;

        if (dialogueIndex < currentEvent.dialogues.Length)
        {
            ShowCurrentLine();
            return;
        }

        SpawnChoices();
    }

    void SpawnChoices()
    {
        ClearChoices();

        foreach (var choice in currentEvent.choices)
        {
            var obj = Instantiate(choicePrefab, choiceRoot);
            obj.GetComponent<EventChoiceUI>()
                .Setup(choice, this);
        }
    }

    void ClearChoices()
    {
        foreach (Transform c in choiceRoot)
            Destroy(c.gameObject);
    }

    // ===== CHOICE =====
    public void OnChoiceSelected(EventChoiceSO c)
    {
        ExecuteChoice(c);

        currentEvent = null;
        dialogPanel.SetActive(false);
        ClearChoices();
        var gm = GameManager.Instance;

        // 🔥 nếu đang travel → resume
        if (gm.travelManager != null && gm.travelManager.IsTravelling())
        {
            gm.travelManager.ResumeTravel();
            gm.ChangeMode(GameMode.Busy); // tiếp tục travel
        }
        else
        {
            gm.ChangeMode(GameMode.Exploration);
        }
    }

    void ExecuteChoice(EventChoiceSO c)
    {
        GameDataSystem.Instance.AddCoin(c.coinDelta);
        GameDataSystem.Instance.AddRep(c.reputationDelta);
        GameDataSystem.Instance.AddHour(c.timeDelta);

        if (c.flagsSet != null)
        {
            foreach (var f in c.flagsSet)
                FlagSystem.Instance.SetFlag(f);
        }

        if (c.flagsCleared != null)
        {
            foreach (var f in c.flagsCleared)
                FlagSystem.Instance.ClearFlag(f);
        }

        // 🔥 STEP BASED
        if (c.advanceStep)
        {
            QuestRuntimeSystem.Instance.AdvanceStep();
        }
    }
}