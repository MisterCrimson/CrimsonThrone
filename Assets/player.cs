using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vitals
{
    public class player : MonoBehaviour
    {
        public Health Health { get; private set; }
            public Stamina Stamina { get; private set; }
            
            private void Awake()
            {
                Health = GetComponent<Health>();
                Stamina = GetComponent<Stamina>();
            }
    }
}
