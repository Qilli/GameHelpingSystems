using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    public interface IOnHitEventsCallbacks
    {
        void onHitByMissle(GameObject hitObject, Vector3 hitPoint, Base.Game.DamageSourceInfo damageSource = null);
    }
}
