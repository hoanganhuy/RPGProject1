public class QuestRuntime
{
    public QuestSO quest;

    int currentStep;

    public QuestRuntime(QuestSO q)
    {
        quest = q;
        currentStep = 0;
    }

    public QuestStep GetCurrentStep()
    {
        if (currentStep >= quest.steps.Length)
            return null;

        return quest.steps[currentStep];
    }

    public void AdvanceStep()
    {
        currentStep++;
    }

    public bool IsCompleted()
    {
        return currentStep >= quest.steps.Length;
    }
}