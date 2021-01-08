using UI;
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
        [SerializeField] public int BuildingHealth = 1;

        [Header("Special Variables")]
        [SerializeField] public Button goldButton;
        [SerializeField] public Button manaButton;

        [SerializeField] public ResourceUIManager resourceManager;

        [HideInInspector] public bool CanCollectResources = false;

        [Header("Audio")]
        [SerializeField] private AudioClip collectSFX;

        /// <summary>Init functions, This function gets called when the building gets created (like a start)</summary>
        public void Init()
        {

        }

        /// <summary>Add the CollectResource function to the OnButtonClick Event to collect the resource</summary>
        public void AddButtonListener()
        {
            goldButton.onClick.AddListener(delegate { CollectResources(); });
            manaButton.onClick.AddListener(delegate { CollectResources(); });

        }

        /// <summary>Collect the resource of the current building</summary>
        public void CollectResources()
        {
            Debug.Log(Resource);

            DataManager.ResourcesGained(ResourcesInStorage);

            if (Resource == ResourceType.GoldMine)
            {
                GameController.Gold += ResourcesInStorage;
            }
            if(Resource == ResourceType.ManaWell)
            {
                GameController.Mana += ResourcesInStorage;
            }


            resourceManager.UpdateResourceUI();

            ResourcesInStorage = 0;

            if (collectSFX != null)
                FindObjectOfType<AudioManagement>().PlayAudioClip(collectSFX, AudioMixerGroups.SFX);
        }

        private void Update()
        {
            if(BuildingHealth <= 0)
            {
                Destroy(goldButton.gameObject);
                Destroy(manaButton.gameObject);
                Destroy(gameObject);

                Debug.LogFormat("{0} Died!!!", gameObject.name);
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

            if(ResourcesInStorage >= MinimumCollectionCount && Resource == ResourceType.GoldMine)
            {
                CanCollectResources = true;
                goldButton.gameObject.SetActive(true);
            }
            else if(ResourcesInStorage < MinimumCollectionCount)
            {
                CanCollectResources = false;
                goldButton.gameObject.SetActive(false);
            }

            if (ResourcesInStorage >= MinimumCollectionCount && Resource == ResourceType.ManaWell)
            {
                CanCollectResources = true;
                manaButton.gameObject.SetActive(true);
            }
            else if (ResourcesInStorage < MinimumCollectionCount)
            {
                CanCollectResources = false;
                manaButton.gameObject.SetActive(false);
            }
        }

        // Return an Int with the amount of resource in storage
        public int GetResource { get { return ResourcesInStorage; } }

        // Return a bool to check if this building is allowed to collect
        public bool CanCollectResource { get { return CanCollectResource; } }
    }
}