using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    public interface ILifePointsControl
    {
        public void addSomeLifePoints(float lifePoints);
        public void addSomeLifePointsPerc(float lifePointsPerc);
        public void removeLifePoints(float lifePoints);
        public void removeLifePointsPerc(float lifePointsPerc);
    }
}