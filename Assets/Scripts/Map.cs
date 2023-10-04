using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Map : NetworkBehaviour
{
    [SerializeField] private ItemDictionary itemDictionary;
    public Grid<Building> map;

    public static Map mapObject;
    // Start is called before the first frame update
    void Start()
    {
        itemDictionary.AddListToDictionary();
        map = new Grid<Building>(50, 50, 1f, new Vector3(-25, -25, 0), Vector3.forward, null);
        mapObject = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject CameraRaycast(Camera camera)
    {
        GameObject result = null;
        Vector2 vec;
        if (map.CameraRaycast(camera, out vec))
        {
            result = map.GetValue(vec)?.gameObject;
        }
        return result;
    }

    public Vector2 CameraRaycastVec2(Camera camera)
    {
        Vector2 vec;
        map.CameraRaycast(camera, out vec);
        
        return vec;
    }

    public bool CheckIfPossiblePlacement(Building building, Vector2 pos)
    {
        for (int i = 0; i < building.size.x; i++)
        {
            for (int j = 0; j < building.size.y; j++)
            {
                Vector2 pos2 = pos + new Vector2(25, 25);
                pos2.x += i;
                pos2.y += j;
                Building temp = map.GetValue(pos2);
                if (temp != null)
                {
                    return false;
                }
            }
            
        }
        return true;
    }

    public void Place(Building building, Vector2 pos, Quaternion rotation)
    {
        SpawnBuildingServerRpc(building.buildingID, pos, rotation);
    }

    public void ReplaceBuilding(Building building, Vector2 pos, Quaternion rotation)
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBuildingServerRpc(FixedString32Bytes spawn, Vector2 pos, Quaternion rotation)
    {
        Building building = itemDictionary.Items[spawn].building;
        pos = pos.FloorToInt();
        pos.x += building.size.x / 2;
        pos.y += building.size.y / 2;
        GameObject g = Instantiate(building.gameObject, pos, rotation);
        building = g.GetComponent<Building>();
        building = building.OnPlaceServerSide(this, pos);
        building.NetworkObject.Spawn();
        SpawnBuildingClientRpc(building.NetworkObject.NetworkObjectId, pos);
    }

    [ClientRpc]
    private void SpawnBuildingClientRpc(ulong objectID, Vector2 pos)
    {
        Vector2 pos2 =  pos + new Vector2(25, 25);
        Building building = GetNetworkObject(objectID).GetComponent<Building>();
        for (int i = 0; i < building.size.x; i++)
        {
            for (int j = 0; j < building.size.y; j++)
            {
                pos2 = pos + new Vector2(25, 25);
                pos2.x += i;
                pos2.y += j;
                map.SetValue(pos2, building);
            }
            
        }
        
        
        
        building.OnPlace(this, pos2);
    }
    
    
    
    
}
