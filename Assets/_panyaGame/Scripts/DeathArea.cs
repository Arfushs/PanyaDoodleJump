using System;
using _panyaGame.Scripts.Player_Related;
using UnityEngine;

namespace _panyaGame.Scripts
{
    public class DeathArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.FireOnPlayerLost();
        }
    }
}
