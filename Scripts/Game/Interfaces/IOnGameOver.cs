using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Game
{
    public interface IOnGameOver
    {
        void onGameOver(bool isGameFailed);
    }
}
