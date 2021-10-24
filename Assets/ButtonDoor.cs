using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    public Transform player;
    public GameObject Door;
    public float dstToDetect;
    bool pressed = false;

    // Update is called once per frame
    void Update()
    {
        if (pressed) return;
        if(Vector3.Distance(player.position, transform.position) < dstToDetect)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                pressed = true;
                Door.SetActive(false);
            }
        }
    }
}
