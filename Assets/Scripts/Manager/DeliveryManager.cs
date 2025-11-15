using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliveryManager : Singleton<DeliveryManager>
{
    #region Parameter
    [SerializeField] private CargoObjectListSO cargoObjectListSO;
    [SerializeField] private List<Transform> lostSpawnPoints;
    [SerializeField] private DeliveryTable[] deliveryTables; // Where will player delivery (it just only 3 place in total)

    private int waitingCargoMax = 4;
    private List<CargoObjectSO> _waitingCargoObjectSOList = new List<CargoObjectSO>();

    public int totalLostItemCount { get; private set; } = 5;
    public DeliveryState currentDeliveryState { get; private set; }
    public CargoObjectSO currentDeliveryObject { get; private set; }
    #endregion

    #region Execute
    void Start()
    {
        currentDeliveryState = DeliveryState.IDLE;

    }
    #endregion

    #region Add Order
    public CargoObjectSO AddRandomOrder()
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

    public void AddLostItemToWaitingList(CargoObjectSO cargo)
    {
        if (!_waitingCargoObjectSOList.Contains(cargo))
        {
            _waitingCargoObjectSOList.Add(cargo);
            Debug.Log($"[DeliveryManager] Added lost item {cargo.objectName} to waiting list.");
        }

    }

    /// <summary>
    /// This is where to delivery
    /// </summary>
    /// <param name="order">the order player get</param>
    /// <returns>The place to deliver</returns>
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

    public List<CargoObjectSO> GetWaitingList()
    {
        return _waitingCargoObjectSOList;
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

    public bool HasPendingOrder()
    {
        return _waitingCargoObjectSOList != null && _waitingCargoObjectSOList.Count > 0;
    }

    public void RemoveOrder(CargoObjectSO cargoObjectSO)
    {
        if (_waitingCargoObjectSOList.Contains(cargoObjectSO))
        {
            _waitingCargoObjectSOList.Remove(cargoObjectSO);
            Debug.Log($"Order {cargoObjectSO.name} has been removed from waiting list (thrown away).");
        }
        else
        {
            Debug.LogWarning($"Tried to remove {cargoObjectSO.name} but it was not in waiting list.");
        }
    }
    #endregion

    #region LostCargoObject
    public void SpawnLostItems()
    {
        if (cargoObjectListSO == null || cargoObjectListSO.cargoObjectSOList.Count == 0)
        {
            Debug.LogWarning("[DeliveryManager] CargoObjectListSO is empty or missing!");
            return;
        }

        if (lostSpawnPoints == null || lostSpawnPoints.Count == 0)
        {
            Debug.LogWarning("[DeliveryManager] No spawn points assigned for lost items!");
            return;
        }

        // Mix the list
        List<CargoObjectSO> randomCargoList = cargoObjectListSO.cargoObjectSOList.OrderBy(x => UnityEngine.Random.value).ToList();

        // Make sure not to exceed the number of items available in the mixed list.
        int spawnCount = Mathf.Min(totalLostItemCount, randomCargoList.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            // Choose Random CargoObjectSO
            CargoObjectSO selectedCargoSO = randomCargoList[i];

            // Choose Random spawn Point
            Transform randomPoint = lostSpawnPoints[UnityEngine.Random.Range(0, lostSpawnPoints.Count)];
            lostSpawnPoints.Remove(randomPoint); // not spawn at the same place

            // Spawn Lost object prefab
            Transform cargoGO = Instantiate(selectedCargoSO.cargoOrderPrefab, randomPoint.position, Quaternion.identity);
            CargoObject cargo = cargoGO.GetComponent<CargoObject>();
        }

        Debug.Log($"[DeliveryManager] Successfully spawned {spawnCount} lost items for Lv2!");
    }
    #endregion

    #region Deliver Event
    public event EventHandler<OnDeliveryEventArgs> OnStartDelivery;
    public event EventHandler<OnDeliveryEventArgs> OnDeliverySuccess;
    public event EventHandler<OnDeliveryEventArgs> OnDeliveryFail;
    public event EventHandler OnStopDelivery;

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
        if (_waitingCargoObjectSOList != null)
        {
            OnStartDelivery?.Invoke(this, new OnDeliveryEventArgs(cargoObjectSO, table));
            currentDeliveryState = DeliveryState.DELIVER;
        }
    }

    public void TriggerStopDelivery()
    {
        Debug.Log($"[DeliveryManager] Stop delivery");
        OnStopDelivery?.Invoke(this, EventArgs.Empty);
        currentDeliveryState = DeliveryState.IDLE;
    }

    public void TriggerDeliverySuccess(CargoObjectSO cargoObjectSO, DeliveryTable table)
    {
        OnDeliverySuccess?.Invoke(this, new OnDeliveryEventArgs(cargoObjectSO, table));
        if (cargoObjectSO.isLostItem)
        {
            WorldManager.Instance.IncreaseLostItemFound();
        }

        UIManager.Instance.ShowDeliverySuccessFeedback(cargoObjectSO.cargoPrice);
                
        currentDeliveryState = DeliveryState.IDLE;
    }

    public void TriggerDeliveryFail(CargoObjectSO cargoObjectSO, DeliveryTable table)
    {
        OnDeliveryFail?.Invoke(this, new OnDeliveryEventArgs(cargoObjectSO, table));

        UIManager.Instance.ShowWrongDeliveryFeedback(cargoObjectSO.penaltyPrice);
     
        currentDeliveryState = DeliveryState.DELIVER;
    }
    #endregion
}

public enum DeliveryState
{
    DELIVER,
    IDLE
}