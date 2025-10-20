using UnityEngine;

public class Table : BaseInteract
{
    [SerializeField] private CargoObjectSO cargoObjectSO;

    public override void Interact(PlayerController player)
    {
        // There nothing on the table...
        if (!HasCargoObject())
        {
            // ...And Player is carrying object
            if (player.HasCargoObject())
            {
                // Place that object on the table
                player.GetCargoObject().SetObjectParent(this);
            }
            // Player has nothing
            else
            {
                Debug.LogWarning("Player have nothing to do with it");
            }
        }
        // There something on the table...
        else
        {
            // ...And Player carrying something
            if (player.HasCargoObject())
            {
                // Nothing happen
                Debug.LogWarning("Already have an object");
            }
            // ... Player not carrying anything
            else
            {
                // Pick it up
                GetCargoObject().SetObjectParent(player);
            }
        }
    }
}
