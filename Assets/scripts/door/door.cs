using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public interface Interact
{
    void open();
}

public class door : MonoBehaviour, Interact
{
   bool isOpen = false;
   Renderer renderer;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    public void open()
    {
        if (isOpen)
        {
            Debug.Log("door closed");
            renderer.transform.position = new Vector3(renderer.transform.position.x, renderer.transform.position.y - 10, renderer.transform.position.z);



        }
        else
        {
            Debug.Log("door open");
            renderer.transform.position = new Vector3(renderer.transform.position.x, renderer.transform.position.y + 10, renderer.transform.position.z);

        }
        isOpen = !isOpen;

    }

}
