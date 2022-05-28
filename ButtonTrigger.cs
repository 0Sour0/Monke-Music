using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
    public class ButtonTrigger : MonoBehaviour 
    {
    public bool Pressed = false;

    void OnTriggerEnter(Collider collider)
    {
        Pressed = true;
        GetComponent<MeshRenderer>().enabled = false;
        Debug.Log("Button On!");
    }

    void OnTriggerExit(Collider collider)
    {
        Pressed = false;
        GetComponent<MeshRenderer>().enabled = true;
        Debug.Log("Button On!");
    }
}

