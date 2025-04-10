using UnityEngine;

namespace PropertyTycoon{

public class Ship_start : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        transform.position = new Vector3((float)-5.3, (float)-5, (float)-0.12);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}