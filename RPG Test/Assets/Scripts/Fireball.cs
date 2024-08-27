using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject explosion;
    private int life = 3;

    private void Awake() {
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag != "MainCharacter") {
            GameObject newExplotion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(newExplotion, 2f);
        }
    }

    public float GetSpeed() {
        return speed;
    }
}
