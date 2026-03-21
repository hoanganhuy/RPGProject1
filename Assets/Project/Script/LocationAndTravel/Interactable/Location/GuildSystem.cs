using UnityEngine;

public class GuildSystem : MonoBehaviour
{
    public GameObject inGuildPanel;
    public GameObject jobBoardPanel;
    public LocationSystem locationSystem;
    public GameManager gm;

    private void Awake()
    {
        inGuildPanel.SetActive(false);
        jobBoardPanel.SetActive(false);
    }

    public void EnterGuild()
    {
        gm.ChangeMode(GameMode.InGuild);

        inGuildPanel.SetActive(true);
        jobBoardPanel.SetActive(true);

        locationSystem.OpenAndCLoseMapLocation(false);
    }

    public void ExitGuild()
    {
        gm.ChangeMode(GameMode.Exploration);

        inGuildPanel.SetActive(false);
        jobBoardPanel.SetActive(false);

        locationSystem.OpenAndCLoseMapLocation(true);
    }
}