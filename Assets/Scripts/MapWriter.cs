using UnityEngine;

public class MapWriter : MonoBehaviour
{

    // Define dictionary to reference input to corresponding prefabs


    private class MapObject
    {
        Vector2Int loc; //grid pos
        GameObject prefab;

        public GameObject GetPrefab()
        {
            return prefab;
        }

        public Vector2Int GetLocation()
        {
            return loc;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // (location and object format)
    // each object has prefab listed (data structure?)
    // account for multiple things on tile (receiver + base tile or Conveyro + box)
    // 
    MapObject[] ReadLayout()
    {
        return new MapObject[2];
    }

    // multiply location by scale of tile
    // List of objects - each corresponding to location
    void BuildMap(MapObject[] mapObjects)
    {
        float scale_of_tile = 2.5f;
        foreach (MapObject object_ in mapObjects)
        {
            Instantiate(object_.GetPrefab(), new Vector3(scale_of_tile * object_.GetLocation().y, 0, scale_of_tile * object_.GetLocation().x), Quaternion.identity);
        }


    }

}
