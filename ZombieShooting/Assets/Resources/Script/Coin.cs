using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    [SerializeField] private int score = 200;

    public void Use(GameObject target)
    {
        GameManager.instance.AddScore(score);
        Destroy(gameObject);
    }
}
