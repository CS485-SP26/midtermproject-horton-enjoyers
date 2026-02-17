using System.Xml;
using UnityEngine;

public class StaticRotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.Rotate(0f, 45f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
