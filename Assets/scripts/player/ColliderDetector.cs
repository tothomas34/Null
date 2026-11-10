using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ColliderDetector : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Interact interact;
        if(other.TryGetComponent<Interact>(out interact))
        interact.open();
    }

}
