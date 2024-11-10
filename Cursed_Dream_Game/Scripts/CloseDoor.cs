using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    private bool isOpen = false;

    public void OpenDoor(){
        if (!isOpen){
            isOpen = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
