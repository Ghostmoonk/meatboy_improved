using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scie : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject gouttePrefab;
    [SerializeField] Texture tache;

    [Header("Spawn de gouttes")]
    [Range(0, 20)]
    [SerializeField] int minGouttesInstanted;
    [Range(20, 40)]
    [SerializeField] int maxGouttesInstanted;

    void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int amountGouttesToSpawn = Random.Range(minGouttesInstanted, maxGouttesInstanted);
            for (int i = 0; i < amountGouttesToSpawn; i++)
            {
                Goutte goutte = Instantiate(gouttePrefab, other.transform.position, Quaternion.identity).GetComponent<Goutte>();
                goutte.SetVelocity(new Vector3(Random.Range(-5f, 5f), Random.Range(0, 15f)));
            }
            GetComponent<Renderer>().material.mainTexture = tache;
            other.GetComponent<MeatBoy>().Die();
        }
    }
}
