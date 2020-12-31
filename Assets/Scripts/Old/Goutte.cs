using UnityEngine;

public class Goutte : MonoBehaviour
{
    Vector3 velocity;
    [SerializeField] float force;
    [Range(2, 4f)]
    [SerializeField] float minLifetime;
    [Range(2, 4f)]
    [SerializeField] float maxLifetime;

    void Start()
    {
        Debug.Log(velocity);
        GetComponent<Rigidbody>().AddForce(velocity * force);
        Invoke("DestroyObject", Random.Range(minLifetime, maxLifetime));
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(GetComponent<Rigidbody>());
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    void DestroyObject() => Destroy(gameObject);



}
