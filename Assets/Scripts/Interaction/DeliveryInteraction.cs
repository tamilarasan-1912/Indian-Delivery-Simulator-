using IndianDeliverySimulator.Core;
using UnityEngine;

namespace IndianDeliverySimulator.Interaction
{
    public class DeliveryInteraction : MonoBehaviour
    {
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private Camera interactionCamera;

        private void Awake()
        {
            if (interactionCamera == null)
                interactionCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                TryInteract();
        }

        private void TryInteract()
        {
            if (interactionCamera == null || DeliveryManager.Instance == null)
                return;

            if (!Physics.Raycast(interactionCamera.transform.position,
                    interactionCamera.transform.forward, out RaycastHit hit, interactionDistance))
                return;

            var pickup = hit.collider.GetComponentInParent<PickupPoint>();
            if (pickup != null)
                pickup.Interact();
        }
    }

    public class PickupPoint : MonoBehaviour
    {
        public void Interact()
        {
            var manager = DeliveryManager.Instance;
            if (manager == null || manager.CurrentOrder == null)
                return;

            switch (manager.CurrentOrder.State)
            {
                case DeliveryState.GoingToStore:
                    manager.ArriveAtStore();
                    Debug.Log("Reached store. Press E again to pick up the package.");
                    break;

                case DeliveryState.AtStore:
                    if (manager.ConfirmPickup())
                    {
                        manager.StartCustomerTrip();
                        Debug.Log("Package picked up. Deliver it to the customer.");
                    }
                    break;
            }
        }
    }
}
