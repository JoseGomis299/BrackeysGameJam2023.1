using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectUtils.Attacking
{
    public class Damage
    {
        public Vector3 origin;
        public float damageAmount;
        public float pushForce;

        public Damage(Vector3 origin, float damageAmount, float pushForce)
        {
            this.origin = origin;
            this.damageAmount = damageAmount;
            this.pushForce = pushForce;
        }

    }
}
