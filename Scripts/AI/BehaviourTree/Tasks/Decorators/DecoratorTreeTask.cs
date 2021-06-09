using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "DecoratorTreeTask", menuName = "Decorator", order = 51)]
    public class DecoratorTreeTask : BehaviourTreeTask
    {
        public bool OnlySingleChildAllowed { get; } = false;
        public DecoratorTreeTask(bool onlySingleChild = false)
        {
            OnlySingleChildAllowed = onlySingleChild;
        }
    }

}
