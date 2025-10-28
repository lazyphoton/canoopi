using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTriggerStart : MonoBehaviour
{
    [SerializeField] BirdManager birdMiniGameManger;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);

        if (other.CompareTag("Player"))

        {
            birdMiniGameManger.MiniGameStarts();
        }
    }
}
