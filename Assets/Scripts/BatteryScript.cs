using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        //Destroy(gameObject);
        Vector3 newPosition;
        do {
            newPosition = new Vector3(
                Random.Range(-4.5f, 4.5f),
                this.transform.position.y,
                Random.Range(-4.5f, 4.5f)
            );
        } while (Vector3.Distance(this.transform.position, newPosition) < 3.0f);
        this.transform.position = newPosition;
    }
}
