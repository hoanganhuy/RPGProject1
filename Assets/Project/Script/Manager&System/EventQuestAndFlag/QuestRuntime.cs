public class QuestRuntime
{
    public QuestSO data;

    public int currentStep;
    public int startHour;

    public bool completed;
    public bool expired;

    public QuestRuntime(QuestSO quest, int currentHour)
    {
        data = quest;
        startHour = currentHour;
        currentStep = quest.startStep;
    }
}