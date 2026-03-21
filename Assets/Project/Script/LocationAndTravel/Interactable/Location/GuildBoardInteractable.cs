public class GuildInteractable : LocationInteractableBase
{
    public override void Interact(GameManager gm)
    {
        gm.guildSystem.EnterGuild();
    }
}