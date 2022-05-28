using UnityEngine;

    public class ButtonToggle : MonoBehaviour 
    {
    public bool Toggled = false;
    public bool NotToggled = true;

    void OnTriggerEnter(Collider collider)
    {
        if (NotToggled)
        {
            Toggled = true;
            NotToggled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            Toggled = false;
            NotToggled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}

