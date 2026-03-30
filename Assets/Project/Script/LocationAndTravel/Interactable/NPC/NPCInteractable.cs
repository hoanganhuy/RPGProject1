using UnityEngine;

public class NPCInteractable : LocationInteractableBase
{
    public string npcID;

    public override void Interact(GameManager gm)
    {
        if (gm.currentMode != GameMode.Exploration)
            return;

        var e = EventRuntimeSystem.Instance
                    .TryGetInteractEvent(npcID);

        if (e != null)
        {
            gm.dialogueSystem.StartEventDialogue(e);
        }
        else
        {
            // fallback đơn giản (narration)
            
            Debug.Log("No event triggered for NPC: " + npcID);
        }
    }
}