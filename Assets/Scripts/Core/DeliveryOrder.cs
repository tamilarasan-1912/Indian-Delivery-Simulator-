using System;
using UnityEngine;

namespace IndianDeliverySimulator.Core
{
    public enum PaymentMethod
    {
        Online,
        CashOnDelivery
    }

    public enum DeliveryState
    {
        Waiting,
        Accepted,
        GoingToStore,
        AtStore,
        PickedUp,
        GoingToCustomer,
        AtCustomer,
        AwaitingOtp,
        AwaitingCash,
        ReadyForHandover,
        Completed,
        Failed
    }

    [Serializable]
    public class DeliveryOrder
    {
        public string OrderId;
        public string StoreName;
        public string CustomerName;
        public int Otp;
        public float CashAmount;
        public PaymentMethod PaymentMethod;
        public DeliveryState State;
        public float Earnings;

        public DeliveryOrder(string orderId, string storeName, string customerName,
            int otp, float cashAmount, PaymentMethod paymentMethod, float earnings)
        {
            OrderId = orderId;
            StoreName = storeName;
            CustomerName = customerName;
            Otp = otp;
            CashAmount = cashAmount;
            PaymentMethod = paymentMethod;
            Earnings = earnings;
            State = DeliveryState.Waiting;
        }
    }
}
