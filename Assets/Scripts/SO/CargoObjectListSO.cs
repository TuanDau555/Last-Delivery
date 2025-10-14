using System.Collections.Generic;
using UnityEngine;

public class CargoObjectListSO : ScriptableObject
{
    [Tooltip("This is holding all of the cargo objects")]
    public List<CargoObjectSO> cargoObjectSOList;
}
