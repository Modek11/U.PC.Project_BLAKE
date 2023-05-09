using UnityEngine;

public class WallsFader : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            other.transform.GetChild(0).gameObject.SetActive(true);
            other.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
