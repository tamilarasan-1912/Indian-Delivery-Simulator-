using IndianDeliverySimulator.Core;
using UnityEngine;

namespace IndianDeliverySimulator.UI
{
    /// <summary>
    /// Temporary uGUI phone/order screen for the vertical slice.
    /// Press N to open/close the phone. The final art can replace this runtime-generated UI.
    /// </summary>
    public class PhoneOrderUI : MonoBehaviour
    {
        private GameObject phonePanel;
        private UnityEngine.UI.Text orderText;
        private UnityEngine.UI.Text walletText;
        private UnityEngine.UI.Button acceptButton;

        private void Start()
        {
            BuildUI();

            if (DeliveryManager.Instance != null)
            {
                DeliveryManager.Instance.OrderChanged += Refresh;
                DeliveryManager.Instance.WalletChanged += RefreshWallet;
                DeliveryManager.Instance.GenerateTestOrder();
                Refresh(DeliveryManager.Instance.CurrentOrder);
                RefreshWallet(DeliveryManager.Instance.Wallet);
            }
        }

        private void OnDestroy()
        {
            if (DeliveryManager.Instance != null)
            {
                DeliveryManager.Instance.OrderChanged -= Refresh;
                DeliveryManager.Instance.WalletChanged -= RefreshWallet;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
                phonePanel.SetActive(!phonePanel.activeSelf);
        }

        private void BuildUI()
        {
            var canvasObject = new GameObject("PhoneCanvas");
            canvasObject.transform.SetParent(transform, false);

            var canvas = canvasObject.AddComponent<UnityEngine.Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            phonePanel = CreatePanel(canvasObject.transform, "PhonePanel");
            var panelRect = phonePanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.65f, 0.1f);
            panelRect.anchorMax = new Vector2(0.95f, 0.9f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            orderText = CreateText(phonePanel.transform, "OrderText", new Vector2(0.08f, 0.55f), new Vector2(0.92f, 0.9f));
            orderText.alignment = TextAnchor.UpperLeft;
            orderText.fontSize = 24;

            walletText = CreateText(phonePanel.transform, "WalletText", new Vector2(0.08f, 0.42f), new Vector2(0.92f, 0.52f));
            walletText.alignment = TextAnchor.MiddleLeft;
            walletText.fontSize = 22;

            acceptButton = CreateButton(phonePanel.transform, "AcceptOrder", "ACCEPT ORDER", new Vector2(0.08f, 0.12f), new Vector2(0.92f, 0.32f));
            acceptButton.onClick.AddListener(AcceptOrder);
        }

        private void AcceptOrder()
        {
            var manager = DeliveryManager.Instance;
            if (manager == null || manager.CurrentOrder == null)
                return;

            if (manager.AcceptOrder())
            {
                manager.StartStoreTrip();
                acceptButton.interactable = false;
                Refresh(manager.CurrentOrder);
            }
        }

        private void Refresh(DeliveryOrder order)
        {
            if (order == null || orderText == null)
                return;

            orderText.text =
                $"QUICKKART\n\n" +
                $"Order: {order.OrderId}\n" +
                $"Store: {order.StoreName}\n" +
                $"Customer: {order.CustomerName}\n\n" +
                $"Payment: {order.PaymentMethod}\n" +
                (order.PaymentMethod == PaymentMethod.CashOnDelivery ? $"Amount: ₹{order.CashAmount:0}\n" : "") +
                $"Earnings: ₹{order.Earnings:0}\n\n" +
                $"Status: {order.State}";
        }

        private void RefreshWallet(float wallet)
        {
            if (walletText != null)
                walletText.text = $"Wallet: ₹{wallet:0}";
        }

        private static GameObject CreatePanel(Transform parent, string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var image = go.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0.04f, 0.04f, 0.04f, 0.96f);
            return go;
        }

        private static UnityEngine.UI.Text CreateText(Transform parent, string name, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var text = go.AddComponent<UnityEngine.UI.Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        private static UnityEngine.UI.Button CreateButton(Transform parent, string name, string label, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var image = go.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0.1f, 0.55f, 0.2f, 1f);
            var button = go.AddComponent<UnityEngine.UI.Button>();

            var text = CreateText(go.transform, "Label", Vector2.zero, Vector2.one);
            text.text = label;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 24;

            return button;
        }
    }
}
