using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ControlerTopDown player;

    private void Update()
    {
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x+8, player.transform.position.y + 12f, player.transform.position.z-8);

    }

}
