using UnityEngine;
using UnityEngine.UI;

namespace ResourceBuilding
{
    public enum ResourceType
    {
        GoldMine = 0, ManaWell = 1
    }
    public class ResourceBuildingCore : MonoBehaviour
    {
        // Resource Variables
        [Header("Resource Type")]
        [SerializeField] public ResourceType Resource;

        [Header("Resource Stats")]
        [SerializeField] private float ResourceCollectionTime;
        private float ResourceCollectTimer;

        [Header("Resource Stats")]
        [SerializeField] private int MinimumCollectionCount;
        [SerializeField] private int MaximumStorageCount;
        [SerializeField] private int AmountAddedPerInterval;

        [Header("Storage")]
        [SerializeField] private int ResourcesInStorage;

        //Buiding Variables
        [Header("Building Variables")]
        [SerializeField] public int BuildingHealth;

        [Header("Special Variables")]
        [SerializeField] public GameObject UIPopUp;
        [SerializeField] public Button button;

        [HideInInspector] public bool CanCollectResources = false;

        public void AddButtonListener()
        {
            button.onClick.AddListener(delegate { CollectResources(); });
        }

        public void CollectResources()
        {
            Debug.Log(Resource);

            if(Resource == ResourceType.GoldMine)
            {
                GameController.Gold += ResourcesInStorage;
            }
            if(Resource == ResourceType.ManaWell)
            {
                GameController.Mana += ResourcesInStorage;
            }
        }

        private void Update()
        {
            if(BuildingHealth <= 0)
            {
                Destroy(button.gameObject);
                Destroy(gameObject);
            }

            if(ResourcesInStorage < MaximumStorageCount)
            {
                if(ResourceCollectTimer >= ResourceCollectionTime)
                {
                    ResourcesInStorage += AmountAddedPerInterval;
                    ResourceCollectTimer = 0f;
                }
                else
                {
                    ResourceCollectTimer += Time.deltaTime;
                }
            }

            if(ResourcesInStorage >= MinimumCollectionCount)
            {
                CanCollectResources = true;
                UIPopUp.SetActive(true);
            }
            else if(ResourcesInStorage < MinimumCollectionCount)
            {
                CanCollectResources = false;
                UIPopUp.SetActive(false);
            }
        }

        // Return an Int with the amount of resource in storage
        public int GetResource { get { return ResourcesInStorage; } }

        // Return a bool to check if this building is allowed to collect
        public bool CanCollectResource { get { return CanCollectResource; } }
    }
}