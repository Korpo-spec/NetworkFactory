using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class PlayerBuild : NetworkBehaviour
{
    [SerializeField] private Map mapObj;

    [SerializeField] private GameObject buildingToPlace;

    private Building buildingToPlaceIbuilding;

    [SerializeField] private GameObject buildingGhost;

    private SpriteRenderer buildingGhostRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        mapObj = FindObjectOfType<Map>();
        buildingToPlaceIbuilding = buildingToPlace.GetComponent<Building>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
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
            vec.x += buildingToPlaceIbuilding.size.x / 2;
            vec.y += buildingToPlaceIbuilding.size.y / 2;
            buildingGhostRenderer.transform.position = vec;
            bool placeable = mapObj.CheckIfPossiblePlacement(buildingToPlaceIbuilding, vec);
            buildingGhostRenderer.color = placeable ? Color.white : Color.red;
            if (Input.GetMouseButtonDown(0) && placeable)
            {
                mapObj.Place(buildingToPlace.GetComponent<Building>(), mapObj.CameraRaycastVec2(Camera.main), buildingGhostRenderer.transform.rotation);
            }
        }
        
        
    }
}
