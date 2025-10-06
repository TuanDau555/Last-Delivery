using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : SingletonPersistent<DeliveryManager>
{
    #region Parameter
    [SerializeField] private CargoObjectListSO cargoObjectListSO;
    [SerializeField] private DeliveryTable[] deliveryTables; // Where will player delivery (it just only 3 place in total)

    private int waitingCargoMax = 4;
    private List<CargoObjectSO> _waitingCargoObjectSOList = new List<CargoObjectSO>();
    public CargoObjectSO currentDeliveryObject { get; private set; }
    #endregion

    #region Add Order
    public CargoObjectSO AddOrder()
    {
        CargoObjectSO waitingCargoObjectSO = null;
        if (_waitingCargoObjectSOList.Count < waitingCargoMax)
        {
            waitingCargoObjectSO = cargoObjectListSO.cargoObjectSOList[
                UnityEngine.Random.Range(0, cargoObjectListSO.cargoObjectSOList.Count)];

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
        int randomTable = UnityEngine.Random.Range(0, deliveryTables.Length); // Lv 1 only 1 table to delivery so we will add constrain later
        DeliveryTable table = deliveryTables[randomTable];
        table.SetExpectedTable(order);
        return table;
    }

    public void SetCurrentDeliveryObject(CargoObjectSO cargoObjectSO)
    {
        currentDeliveryObject = cargoObjectSO;
    }
    #endregion

    #region Order Delivered
    public bool CheckDeliveryOrder(CargoObjectSO cargoObjectSO, DeliveryTable deliveryTable)
    {
        if (_waitingCargoObjectSOList.Any(c => c.id == cargoObjectSO.id)
            && deliveryTable._expectedCargo != null
            && deliveryTable._expectedCargo.id == cargoObjectSO.id)
        {
            RemoveOrder(cargoObjectSO);
            return true;
        }

        return false;
    }

    public void ClearDeliveryObject()
    {
        currentDeliveryObject = null;
    }

    public void RemoveOrder(CargoObjectSO cargoObjectSO)
    {
        if (_waitingCargoObjectSOList.Contains(cargoObjectSO))
        {
            _waitingCargoObjectSOList.RemoveAll(c => c.id == cargoObjectSO.id);
            Debug.Log($"Order {cargoObjectSO.name} has been removed from waiting list (thrown away).");
        }
        else
        {
            Debug.LogWarning($"Tried to remove {cargoObjectSO.name} but it was not in waiting list.");
        }
    }
    #endregion

    #region Deliver Event
    public event EventHandler<OnDeliveryEventArgs> OnStartDelivery;
    public event EventHandler<OnDeliveryEventArgs> OnDeliverySuccess;

    public class OnDeliveryEventArgs : EventArgs
    {

        public CargoObjectSO CargoObjectSO { get; private set; }
        public DeliveryTable DeliveryTable { get; private set; }
        public OnDeliveryEventArgs(CargoObjectSO cargoObject, DeliveryTable deliveryTable)
        {
            CargoObjectSO = cargoObject;
            DeliveryTable = deliveryTable;
        }
    }

    public void TriggerStartDelivery(CargoObjectSO cargoObjectSO, DeliveryTable table)
    {
        Debug.Log($"[DeliveryManager] Start delivery: {cargoObjectSO.objectName} → {table.name}");
        OnStartDelivery?.Invoke(this, new OnDeliveryEventArgs(cargoObjectSO, table));
    }

    public void TriggerDeliverySuccess(CargoObjectSO cargoObjectSO, DeliveryTable table)
    {
        Debug.Log($"[DeliveryManager] Delivery success: {cargoObjectSO.objectName} → {table.name}");
        OnDeliverySuccess?.Invoke(this, new OnDeliveryEventArgs(cargoObjectSO, table));
    }
    #endregion
}
