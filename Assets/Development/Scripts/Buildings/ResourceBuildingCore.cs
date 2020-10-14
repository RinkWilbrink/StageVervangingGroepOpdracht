using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        // Variables
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

        // Private variables
        [HideInInspector] public bool CanCollectResources = false;
        [SerializeField] public GameObject UIPopUp;

        [SerializeField] private Button button;

        private void Start()
        {
            //
            button.onClick.AddListener(delegate { CollectResources(); } );
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