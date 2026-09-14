using System;
using UnityEngine;

namespace IndianDeliverySimulator.Core
{
    public class DeliveryManager : MonoBehaviour
    {
        public static DeliveryManager Instance { get; private set; }

        public DeliveryOrder CurrentOrder { get; private set; }
        public float Wallet { get; private set; }

        public event Action<DeliveryOrder> OrderChanged;
        public event Action<float> WalletChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void GenerateTestOrder()
        {
            int otp = UnityEngine.Random.Range(1000, 10000);
            var payment = UnityEngine.Random.value > 0.5f
                ? PaymentMethod.CashOnDelivery
                : PaymentMethod.Online;

            CurrentOrder = new DeliveryOrder(
                $"IDS-{UnityEngine.Random.Range(10000, 99999)}",
                "QuickKart Supermarket",
                "Arjun",
                otp,
                payment == PaymentMethod.CashOnDelivery ? 620f : 0f,
                payment,
                47f);

            OrderChanged?.Invoke(CurrentOrder);
        }

        public bool AcceptOrder()
        {
            if (CurrentOrder == null || CurrentOrder.State != DeliveryState.Waiting)
                return false;

            CurrentOrder.State = DeliveryState.GoingToStore;
            OrderChanged?.Invoke(CurrentOrder);
            return true;
        }

        public bool ConfirmPickup()
        {
            if (CurrentOrder == null || CurrentOrder.State != DeliveryState.AtStore)
                return false;

            CurrentOrder.State = DeliveryState.PickedUp;
            OrderChanged?.Invoke(CurrentOrder);
            return true;
        }

        public bool ArriveAtCustomer()
        {
            if (CurrentOrder == null || CurrentOrder.State != DeliveryState.GoingToCustomer)
                return false;

            CurrentOrder.State = DeliveryState.AwaitingOtp;
            OrderChanged?.Invoke(CurrentOrder);
            return true;
        }

        public bool VerifyOtp(int enteredOtp)
        {
            if (CurrentOrder == null || CurrentOrder.State != DeliveryState.AwaitingOtp)
                return false;

            if (enteredOtp != CurrentOrder.Otp)
                return false;

            CurrentOrder.State = CurrentOrder.PaymentMethod == PaymentMethod.CashOnDelivery
                ? DeliveryState.AwaitingCash
                : DeliveryState.ReadyForHandover;

            OrderChanged?.Invoke(CurrentOrder);
            return true;
        }

        public bool CollectCash(float amount)
        {
            if (CurrentOrder == null || CurrentOrder.State != DeliveryState.AwaitingCash)
                return false;

            if (amount < CurrentOrder.CashAmount)
                return false;

            CurrentOrder.State = DeliveryState.ReadyForHandover;
            OrderChanged?.Invoke(CurrentOrder);
            return true;
        }

        public bool CompleteDelivery()
        {
            if (CurrentOrder == null || CurrentOrder.State != DeliveryState.ReadyForHandover)
                return false;

            CurrentOrder.State = DeliveryState.Completed;
            Wallet += CurrentOrder.Earnings;
            WalletChanged?.Invoke(Wallet);
            OrderChanged?.Invoke(CurrentOrder);
            return true;
        }
    }
}
