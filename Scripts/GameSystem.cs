//k.Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.ObjectsControl;

namespace Base
{

    public class GameSystem : BaseObject
    {
        public enum GameState
        {
            PAUSE,
            PLAY,
            END_GAME,
            START,
            GAME_OVER
        }

        public GameState gameState;
        public ObjectsManagerController objectsMgr;

        public override void init()
        {
            base.init();
            changeState(GameState.PLAY);
        }

        public virtual void pauseGame(bool pause_)
        {
            if (pause_) changeState(GameState.PAUSE);
            else changeState(GameState.PLAY);
        }

        public virtual void quitGame()
        {
            Application.Quit();
        }

        public virtual void changeState(GameState newState)
        {
            if (newState == gameState) return;

            objectsMgr.onChangeState(newState);

            switch (newState)
            {
                case GameState.PAUSE:
                    {
                        Physics2D.autoSimulation = false;


                    }
                    break;
                case GameState.END_GAME:
                    {
                        Physics2D.autoSimulation = false;

                    }
                    break;
                case GameState.PLAY:
                    {
                        Physics2D.autoSimulation = true;


                    }
                    break;
                case GameState.START:
                    {
                        Physics2D.autoSimulation = true;

                    }
                    break;
                case GameState.GAME_OVER:
                    {
                        Physics2D.autoSimulation = false;

                    }
                    break;
            }

            gameState = newState;

        }
    }

}
