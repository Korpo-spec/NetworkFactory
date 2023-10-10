using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class PlayerBuild : NetworkBehaviour
{
    [SerializeField] private Map mapObj;

    [SerializeField] private Building buildingToPlace;

    

    [SerializeField] private GameObject buildingGhost;

    [SerializeField]
    private InventoryHotbarUI hotbarUI;

    private SpriteRenderer buildingGhostRenderer;


    private KeyCode[] keyCodes = new KeyCode[]
    {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6,
        KeyCode.Alpha7, KeyCode.Alpha8
    };
    
    // Start is called before the first frame update
    void Start()
    {
        hotbarUI = FindObjectOfType<InventoryHotbarUI>();
        mapObj = FindObjectOfType<Map>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown((keyCodes[i])))
            {
                Item item = hotbarUI.GetHotbarItem(i);
                if (item && item.building)
                {
                    buildingToPlace = item.building;
                    if (buildingGhostRenderer)
                    {
                        buildingGhostRenderer.sprite = buildingToPlace.GetComponent<SpriteRenderer>().sprite;
                    }
                    
                }
                else
                {
                    buildingToPlace = null;
                }
                    
            }
        }


        if (Input.GetKeyDown(KeyCode.A) && buildingToPlace)
        {
            if (buildingGhostRenderer)
            {
                Destroy(buildingGhostRenderer);
                return;
            }
            Debug.Log("spawn");
            
            buildingGhostRenderer = Instantiate(buildingGhost).GetComponent<SpriteRenderer>();
            buildingGhostRenderer.sprite =
                buildingToPlace.GetComponent<SpriteRenderer>().sprite;
        }

        if (buildingGhostRenderer)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                buildingGhostRenderer.transform.Rotate(new Vector3(0,0,90));
            }
            Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec = vec.FloorToInt();
            vec.x += buildingToPlace.size.x / 2;
            vec.y += buildingToPlace.size.y / 2;
            buildingGhostRenderer.transform.position = vec;
            bool placeable = mapObj.CheckIfPossiblePlacement(buildingToPlace, vec);
            buildingGhostRenderer.color = placeable ? Color.white : Color.red;
            if (Input.GetMouseButtonDown(0) && placeable)
            {
                mapObj.Place(buildingToPlace.GetComponent<Building>(), mapObj.CameraRaycastVec2(Camera.main), buildingGhostRenderer.transform.rotation);
            }
        }
        
        
    }
}
