using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {      
        this.gameObject.SetActive(false);
    }
}
