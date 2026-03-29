using UnityEngine;

public class EventRuntimeSystem : MonoBehaviour
{
    public static EventRuntimeSystem Instance;

    public EventSO[] worldEvents;

    void Awake()
    {
        Instance = this;
    }
    public EventSO TryGetInteractEvent(string targetId)
    {
        var quest = QuestRuntimeSystem.Instance.GetActiveQuest();

        // ===== 1. STORY EVENT =====
        if (quest != null)
        {
            Debug.Log("khong get duoc activequest");
            foreach (var e in quest.data.events)
            {
                if (e.layer != EventLayer.Story)
                    continue;

                if (e.triggerType != EventTriggerType.OnInteract)
                    continue;

                if (e.targetId != targetId)
                    continue;

                if (e.requiredStep != -1 &&
                    e.requiredStep != quest.currentStep)
                    continue;

                if (!CheckCondition(e))
                    continue;

                return e;
            }
        }

        // ===== 2. CONTEXTUAL =====
        foreach (var e in worldEvents)
        {
            if (e.layer != EventLayer.Contextual)
                continue;

            if (e.triggerType != EventTriggerType.OnInteract)
                continue;

            if (e.targetId != targetId)
                continue;

            if (!CheckCondition(e))
                continue;

            if (Random.value <= e.triggerChance)
                return e;
        }

        // ===== 3. GENERIC =====
        foreach (var e in worldEvents)
        {
            if (e.layer != EventLayer.Generic)
                continue;

            if (e.triggerType != EventTriggerType.OnInteract)
                continue;

            if (!CheckCondition(e))
                continue;

            if (Random.value <= e.triggerChance)
                return e;
        }

        return null;
    }
    public EventSO TryGetTravelEvent()
    {
        var quest = QuestRuntimeSystem.Instance.GetActiveQuest();

        // ===== 1. STORY EVENT =====
        if (quest != null)
        {
            foreach (var e in quest.data.events)
            {
                if (e.layer != EventLayer.Story)
                    continue;

                if (e.triggerType != EventTriggerType.OnTravelStep)
                    continue;

                if (e.requiredStep != -1 &&
                    e.requiredStep != quest.currentStep)
                    continue;

                if (!CheckCondition(e))
                    continue;

                return e;
            }
        }

        // ===== 2. CONTEXTUAL =====
        foreach (var e in worldEvents)
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
        foreach (var e in worldEvents)
        {
            if (e.layer != EventLayer.Generic)
                continue;

            if (e.triggerType != EventTriggerType.OnTravelStep)
                continue;

            if (!CheckCondition(e))
                continue;

            if (Random.value <= e.triggerChance)
                return e;
        }

        return null;
    }
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