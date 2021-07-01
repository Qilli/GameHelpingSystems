using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    public interface IOnLifeChangeActions
    {
        void onPreDestroy();
        void onBorn();
    }
}
