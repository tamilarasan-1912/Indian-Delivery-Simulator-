using IndianDeliverySimulator.Core;
using IndianDeliverySimulator.Interaction;
using IndianDeliverySimulator.Player;
using IndianDeliverySimulator.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IndianDeliverySimulator.Editor
{
    public static class PrototypeSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/DeliveryPrototype.unity";

        [MenuItem("Indian Delivery Simulator/Build V0.1 Prototype Scene")]
        public static void Build()
        {
            EnsureFolder("Assets/Scenes");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateLighting();
            CreateGround();
            CreatePlayer();
            CreateDeliveryManager();
            CreateStore();
            CreateCustomer();
            CreateStreetProps();

            EditorSceneManager.SaveScene(scene, ScenePath);
            Debug.Log($"Indian Delivery Simulator prototype created: {ScenePath}");
        }

        private static void CreateLighting()
        {
            var lightObject = new GameObject("Sun");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.55f, 0.55f, 0.55f);
        }

        private static void CreateGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "IndianStreetGround";
            ground.transform.position = new Vector3(0f, -0.5f, 10f);
            ground.transform.localScale = new Vector3(32f, 1f, 40f);
        }

        private static void CreatePlayer()
        {
            var player = new GameObject("Player");
            player.transform.position = new Vector3(0f, 1f, 0f);
            player.AddComponent<CharacterController>();
            player.AddComponent<PlayerController>();
            player.AddComponent<DeliveryInteraction>();

            var cameraObject = new GameObject("PlayerCamera");
            cameraObject.transform.SetParent(player.transform);
            cameraObject.transform.localPosition = new Vector3(0f, 0.75f, 0f);
            cameraObject.transform.localRotation = Quaternion.identity;
            var camera = cameraObject.AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.fieldOfView = 70f;
        }

        private static void CreateDeliveryManager()
        {
            var managerObject = new GameObject("DeliveryManager");
            managerObject.AddComponent<DeliveryManager>();
            managerObject.AddComponent<PhoneOrderUI>();
        }

        private static void CreateStore()
        {
            var store = GameObject.CreatePrimitive(PrimitiveType.Cube);
            store.name = "QuickKartStore";
            store.transform.position = new Vector3(0f, 2f, 8f);
            store.transform.localScale = new Vector3(8f, 4f, 5f);

            var pickup = new GameObject("PackagePickupPoint");
            pickup.transform.SetParent(store.transform);
            pickup.transform.localPosition = new Vector3(0f, -0.25f, -0.6f);
            pickup.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            var collider = pickup.AddComponent<BoxCollider>();
            collider.isTrigger = false;
            pickup.AddComponent<PickupPoint>();
        }

        private static void CreateCustomer()
        {
            var building = GameObject.CreatePrimitive(PrimitiveType.Cube);
            building.name = "CustomerApartment";
            building.transform.position = new Vector3(0f, 3f, 25f);
            building.transform.localScale = new Vector3(8f, 6f, 5f);

            var door = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door.name = "CustomerDeliveryPoint";
            door.transform.position = new Vector3(0f, 1.25f, 22.35f);
            door.transform.localScale = new Vector3(1.5f, 2.5f, 0.3f);
            door.AddComponent<CustomerDeliveryPoint>();
        }

        private static void CreateStreetProps()
        {
            CreateBlock(new Vector3(-8f, 1.5f, 10f), new Vector3(4f, 3f, 8f), "StreetBuildingA");
            CreateBlock(new Vector3(8f, 2f, 17f), new Vector3(4f, 4f, 6f), "StreetBuildingB");
            CreateBlock(new Vector3(-8f, 2f, 25f), new Vector3(4f, 4f, 6f), "StreetBuildingC");
        }

        private static void CreateBlock(Vector3 position, Vector3 scale, string name)
        {
            var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = name;
            block.transform.position = position;
            block.transform.localScale = scale;
        }

        private static void EnsureFolder(string path)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
                AssetDatabase.CreateFolder("Assets", "Scenes");
        }
    }
}
