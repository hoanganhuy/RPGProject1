using System.Collections.Generic;
using UnityEngine;

public class EventRuntimeSystem : MonoBehaviour
{
    public static EventRuntimeSystem Instance;

    public EventSO[] eventGeneric;
    public List<EventSO> runTimeEventGeneric;
    public EventSO[] eventContxtual;
    public List<EventSO> runTimeEventContxtual;
    public EventSO newDayEvent;

    void Awake()
    {
        Instance = this;

        runTimeEventGeneric = new List<EventSO>(eventGeneric);
        runTimeEventContxtual = new List<EventSO>(eventContxtual);
    }
    public EventSO TryGetInteractEvent(string targetId)
    {
        var quest = QuestRuntimeSystem.Instance.GetActiveQuest();

        // ===== 1. STORY EVENT =====
        if (quest != null)
        {
            List<EventSO> candidates = new List<EventSO>();

            foreach (var e in quest.data.events)
            {
                if (e.layer != EventLayer.Story)
                    continue;

                if (e.targetId != targetId)
                    continue;

                if (e.triggerType != EventTriggerType.OnInteract)
                    continue;

                if (e.requiredStep != -1 &&
                    e.requiredStep != quest.currentStep)
                    continue;

                if (!CheckCondition(e))
                    continue;

                candidates.Add(e);
            }

            //  chọn random
            if (candidates.Count > 0)
            {
                return candidates[Random.Range(0, candidates.Count)];
            }
        }

        //// ===== 2. CONTEXTUAL =====
        //foreach (var e in eventContxtual)
        //{
        //    if (e.layer != EventLayer.Contextual)
        //        continue;

        //    if (e.triggerType != EventTriggerType.OnInteract)
        //        continue;

        //    if (e.targetId != targetId)
        //        continue;

        //    if (!CheckCondition(e))
        //        continue;

        //    if (Random.value <= e.triggerChance)
        //        return e;
        //}

        //// ===== 3. GENERIC =====
        //foreach (var e in eventGeneric)
        //{
        //    if (e.layer != EventLayer.Generic)
        //        continue;

        //    if (e.triggerType != EventTriggerType.OnInteract)
        //        continue;

        //    if (!CheckCondition(e))
        //        continue;

        //    if (Random.value <= e.triggerChance)
        //        return e;
        //}

        return null;
    }
    public EventSO TryGetEventLocation()
    {
        //===== 1. EventLocation ======
        foreach (var e in runTimeEventContxtual)
        {
            if (e.layer != EventLayer.Contextual)
                continue;

            if (e.triggerType != EventTriggerType.OnLocationEnter)
                continue;

            if (!CheckCondition(e))
                continue;

            if (Random.value <= e.triggerChance)
                return e;
        }
        // ===== 2. GENERIC =====
        foreach (var e in runTimeEventGeneric)
        {
            if (e.layer != EventLayer.Generic)
                continue;

            if (e.triggerType != EventTriggerType.OnLocationEnter)
                continue;

            if (!CheckCondition(e))
                continue;

            if (Random.value <= e.triggerChance)
            {
                if (!e.repeatable)
                {
                    runTimeEventGeneric.Remove(e);
                }
                return e;
            }
        }
        return null;
    }
    public EventSO TryGetTravelEvent(float multiplier)
    {
        
        // ===== 2. CONTEXTUAL =====
        foreach (var e in runTimeEventContxtual)
        {
            if (e.layer != EventLayer.Contextual)
                continue;

            if (e.triggerType != EventTriggerType.OnTravelStep)
                continue;

            if (!CheckCondition(e))
                continue;

            if (Random.value <= e.triggerChance)
                return e;
        }

        // ===== 3. GENERIC =====
        foreach (var e in runTimeEventGeneric)
        {
            if (e.layer != EventLayer.Generic)
                continue;

            if (e.triggerType != EventTriggerType.OnTravelStep)
                continue;

            if (!CheckCondition(e))
                continue;

            float finalChance = e.triggerChance * multiplier;

            if (Random.value <= finalChance)
            {
                if (!e.repeatable)
                    runTimeEventGeneric.Remove(e);

                return e;
            }
        }

        return null;
    }
    public void TriggerTimePass(float delta)
    {
        foreach (var e in eventGeneric)
        {
            if (e.triggerType != EventTriggerType.OnTimePass)
                continue;

            if (!CheckCondition(e))
                continue;

            if (Random.value <= e.triggerChance)
            {
                GameManager.Instance.ChangeMode(GameMode.InEvent);
                GameManager.Instance.dialogueSystem.StartEventDialogue(e);
                return;
            }
        }
    }
    public void TriggerNewDay()
    {
        var e = newDayEvent; // EventSO bạn assign sẵn

        GameManager.Instance.ChangeMode(GameMode.InEvent);
        GameManager.Instance.dialogueSystem.StartEventDialogue(e);
    }
    //Condition
    bool CheckCondition(EventSO e)
    {
        // REQUIRED
        if (e.requiredFlags != null)
        {
            foreach (var f in e.requiredFlags)
            {
                if (!FlagSystem.Instance.HasFlag(f))
                    return false;
            }
        }

        // FORBIDDEN
        if (e.forbiddenFlags != null)
        {
            foreach (var f in e.forbiddenFlags)
            {
                if (FlagSystem.Instance.HasFlag(f))
                    return false;
            }
        }

        return true;
    }
}