using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class LaserDetection : MonoBehaviour
{
    public GameObject Light;
    public Color Color;
    // Start is called before the first frame update
    void Start()
    {
        Color= Light.GetComponent<Light>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Light.GetComponent<Light>().color = Color.red;
            Invoke("Light_Normal", 10f);
        }
    }

    public void Light_Normal()
    {
        Light.GetComponent<Light>().color = Color;
    }

}
