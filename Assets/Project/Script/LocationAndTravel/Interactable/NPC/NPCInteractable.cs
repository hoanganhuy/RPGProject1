using UnityEngine;

public class NPCInteractable : LocationInteractableBase
{
    public string npcID;

    public override void Interact(GameManager gm)
    {
        gm.dialogueSystem.StartDialogue(npcID);
    }
}