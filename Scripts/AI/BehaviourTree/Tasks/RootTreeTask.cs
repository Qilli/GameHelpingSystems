using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [CreateAssetMenu(fileName = "RootTreeTask", menuName = "Root", order = 51)]
    public class RootTreeTask : BehaviourTreeTask
    {
        public override TaskResult run(BehaviourTreeController.TreeStatus controller)
        {
            return children[0].run(controller);
        }
    }


}
