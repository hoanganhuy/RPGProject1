using UnityEngine;

public class GameDataSystem : MonoBehaviour
{
    public static GameDataSystem Instance;

    public int day = 1;
    public float hour = 8f;

    public int coin = 50;
    public int reputation = 10;

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UIDataSystem.Instance.Refresh();
    }
    public void AddHour(float h)
    {
        hour += h;

        while (hour >= 24f)
        {
            hour -= 24f;
            day++;
        }

        UpdateTimeFlags();
        UIDataSystem.Instance.Refresh();
    }

    public int GetCurrentHourInt()
    {
        return Mathf.FloorToInt(hour);
    }

    void UpdateTimeFlags()
    {
        // clear old
        FlagSystem.Instance.ClearFlag("morning");
        FlagSystem.Instance.ClearFlag("afternoon");
        FlagSystem.Instance.ClearFlag("night");

        if (hour >= 6 && hour < 12)
            FlagSystem.Instance.SetFlag("morning");
        else if (hour >= 12 && hour < 18)
            FlagSystem.Instance.SetFlag("afternoon");
        else
            FlagSystem.Instance.SetFlag("night");
    }

    public void AddCoin(int c)
    {
        coin += c;
        UIDataSystem.Instance.Refresh();
    }

    public void AddRep(int r)
    {
        reputation += r;
        UIDataSystem.Instance.Refresh();
    }
}