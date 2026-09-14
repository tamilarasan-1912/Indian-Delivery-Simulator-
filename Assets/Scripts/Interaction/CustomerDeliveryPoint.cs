using IndianDeliverySimulator.Core;
using UnityEngine;

namespace IndianDeliverySimulator.Interaction
{
    public class CustomerDeliveryPoint : MonoBehaviour
    {
        [SerializeField] private int testOtp = 4837;

        public void Interact()
        {
            var manager = DeliveryManager.Instance;
            if (manager == null || manager.CurrentOrder == null)
                return;

            switch (manager.CurrentOrder.State)
            {
                case DeliveryState.GoingToCustomer:
                    manager.ArriveAtCustomer();
                    Debug.Log("Customer reached. Ask for OTP.");
                    break;

                case DeliveryState.AwaitingOtp:
                    if (manager.VerifyOtp(testOtp))
                        Debug.Log("OTP verified. Collect COD if required, then hand over the package.");
                    else
                        Debug.LogWarning("Incorrect OTP.");
                    break;

                case DeliveryState.AwaitingCash:
                    if (manager.CollectCash(manager.CurrentOrder.CashAmount))
                        Debug.Log("Cash collected. Package ready for handover.");
                    break;

                case DeliveryState.ReadyForHandover:
                    if (manager.CompleteDelivery())
                        Debug.Log("Delivery completed! Earnings added to wallet.");
                    break;
            }
        }
    }
}
