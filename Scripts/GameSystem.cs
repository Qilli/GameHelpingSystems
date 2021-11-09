//k.Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.ObjectsControl;

namespace Base
{

    public class GameSystem : BaseObject,Events.IOnEvent
    {
        public enum GameState
        {
            PAUSE,
            PLAY,
            END_GAME,
            START,
            GAME_OVER
        }
        [Header("Params")]
        public GameState gameState;
        public ObjectsManagerController objectsMgr;
        [Header("Handled events")]
        public Events.GameEventID killedPlayerEventID;
        [Header("Events to sent")]
        public Events.GameEventID gameOverEventID;
        #region PUBLIC FUNCTIONS
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
        public virtual void onEventResponse(Events.BaseEvent event_)
        {
            if(event_.GetEventID==killedPlayerEventID)
            {
                //kill event, check if it is a player
                Events.KilledEvent killedEvent = event_ as Events.KilledEvent;
                if (killedEvent?.isPlayer == true) changeState(GameState.GAME_OVER);
            }
        }
        public bool hasEventID(Base.Events.GameEventID eventID)
        {
            return eventID==killedPlayerEventID;
        }

        public virtual void changeState(GameState newState)
        {
            if (newState == gameState) return;

            objectsMgr.onChangeState(newState);

            switch (newState)
            {
                case GameState.PAUSE:
                    {
                        Physics2D.simulationMode = SimulationMode2D.Script;
                    }
                    break;
                case GameState.END_GAME:
                    {
                        Physics2D.simulationMode = SimulationMode2D.Script;
                    }
                    break;
                case GameState.PLAY:
                    {
                        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
                    }
                    break;
                case GameState.START:
                    {
                        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

                    }
                    break;
                case GameState.GAME_OVER:
                    {
                        Physics2D.simulationMode = SimulationMode2D.Script;
                        sendGameOverEvent();
                        //set flag to global blackboard
                        Base.AI.Behaviours.GlobalBlackboard.It.setBooleanParam("IsGameOver", true);
                    }
                    break;
            }

            gameState = newState;

        }
        #endregion
        #region PRIVATE FUNCTIONS
        void sendGameOverEvent()
        {
            //send game over event
            if (gameOverEventID != null)
            {
                Events.GameOverEvent gameOver = new Events.GameOverEvent(gameOverEventID);
                gameOver.Sender = this;
                Base.GlobalDataContainer.It.eventsManager.dispatchEvent(gameOver);
            }
        }
        #endregion
    }

}
