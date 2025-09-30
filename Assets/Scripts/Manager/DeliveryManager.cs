using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : SingletonPersistent<DeliveryManager>
{
    [SerializeField] private CargoObjectListSO cargoObjectListSO;
    [SerializeField] private DeliveryTable[] deliveryTables; // Where will player delivery (it just only 3 place in total)

    private int waitingCargoMax = 4;
    private List<CargoObjectSO> _waitingCargoObjectSOList = new List<CargoObjectSO>();
    public CargoObjectSO currentDeliveryObject { get; private set; }

    public CargoObjectSO AddOrder()
    {
        CargoObjectSO waitingCargoObjectSO = null;
        if (_waitingCargoObjectSOList.Count < waitingCargoMax)
        {
            waitingCargoObjectSO = cargoObjectListSO.cargoObjectSOList[
                Random.Range(0, cargoObjectListSO.cargoObjectSOList.Count)];

            _waitingCargoObjectSOList.Add(waitingCargoObjectSO);
            Debug.Log("Order: " + waitingCargoObjectSO);
        }
        else
        {
            Debug.LogWarning("You have reach this day order limit");
        }
        return waitingCargoObjectSO;
    }

    public DeliveryTable TableToDelivery(CargoObjectSO order)
    {
        int randomTable = Random.Range(0, deliveryTables.Length); // Lv 1 only 1 table to delivery so we will add constrain later
        DeliveryTable table = deliveryTables[randomTable];
        table.SetExpectedTable(order);
        return table;
    }
    public bool CheckDeliveryOrder(CargoObjectSO cargoObjectSO, DeliveryTable deliveryTable)
    {
        if (_waitingCargoObjectSOList.Any(c => c.id == cargoObjectSO.id)
            && deliveryTable._expectedCargo != null
            && deliveryTable._expectedCargo.id == cargoObjectSO.id)
        {
            _waitingCargoObjectSOList.RemoveAll(c => c.id == cargoObjectSO.id);
            return true;
        }

        return false;
    }

    public void SetCurrentDeliveryObject(CargoObjectSO cargoObjectSO)
    {
        currentDeliveryObject = cargoObjectSO;
    }

    public void ClearDeliveryObject()
    {
        currentDeliveryObject = null;
    }
}
