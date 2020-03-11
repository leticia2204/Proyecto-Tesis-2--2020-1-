using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamara : MonoBehaviour
{
    Transform target;
    float tLX, tLY, bRX, bRY;
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, tLX, bRX),
            Mathf.Clamp(target.position.y, bRY, tLY),
            transform.position.z
        );
    }

    public void SetBound(GameObject map)
    {
        Tiled2Unity.TiledMap config = map.GetComponent<Tiled2Unity.TiledMap>();
        float camerasize = Camera.main.orthographicSize;

        tLX = map.transform.position.x + camerasize;
        tLY = map.transform.position.y - camerasize;
        bRX = map.transform.position.x + config.NumTilesWide - camerasize;
        bRY = map.transform.position.y - config.NumTilesHigh + camerasize;

    }
}
