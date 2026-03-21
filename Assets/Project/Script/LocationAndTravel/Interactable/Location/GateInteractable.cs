public class GateInteractable : LocationInteractableBase
{
    public override void Interact(GameManager gm)
    {
        gm.OpenWorldMap();
    }
}