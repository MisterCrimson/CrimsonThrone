using System;
using UnityEngine;


namespace Vitals
{
    public class Health : VitalsBase
    {
        public GameObject character;
        public Health health;
        health.Value = 0;
        health.MaxValue = 100;

         // Method to find and return the Player component
        private void findPlayerTag()
        {
            // Find the GameObject with the tag "Player"
            character = GameObject.FindWithTag("Player");
        }
        #region Health Methods

        public void Heal(float amount)
        {
            // Ensure that character is initialized and has a Player component
            if (health.Value = 0)
            {
                character.health.Increase(amount);
            }
            else
            {
                Debug.LogWarning("Player component not found.");
            }
        }

        public void Damage(float amount)
        {
            if (health.maxValue >=100)
            {
                character.health.Decrease(amount);
            }
            else
            {
                Debug.LogWarning("Player component not found.");
            }
        }
        #endregion

    }
}