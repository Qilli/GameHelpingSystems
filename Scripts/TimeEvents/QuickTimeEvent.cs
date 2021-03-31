//K.Homa 02.03.2021
using System;
namespace Base.QTE
{
    public class QuickTimeEvent
    {
        public enum QTEState
        {
            STOP,
            RUN,
            PAUSED,
            END
        }

        protected float interval = 0;
        protected int counter = 0;
        protected int repeatCount = 0;
        protected float timer;
        protected QTEState state;
        protected System.Action listener;
        protected System.Action<System.Object> listenerObj;
        protected System.Object obj;
        public QTEState getState { get { return state; } private set { state = value; } }

        public QuickTimeEvent(int repeatCount_, float interval_, System.Action listener_)
        {
            repeatCount = repeatCount_;
            interval = interval_;
            listener = listener_;
            listenerObj = null;
        }

        public QuickTimeEvent(int repeatCount_, float interval_, System.Action<System.Object> listener_, System.Object obj_)
        {
            repeatCount = repeatCount_;
            interval = interval_;
            listenerObj = listener_;
            obj = obj_;
            listener = null;
        }

        private QuickTimeEvent() { }

        public void start()
        {
            timer = 0;
            counter = 0;
            state = QTEState.RUN;
        }

        public void pause()
        {
            if (state == QTEState.RUN)
                state = QTEState.PAUSED;
        }

        public void resume()
        {
            if (state == QTEState.PAUSED) state = QTEState.RUN;
        }

        public bool isOver() { return state == QTEState.END; }

        public void updateEvent(float delta)
        {
            if (state == QTEState.RUN)
            {
                timer += delta;
                if (timer >= interval)
                {
                    timer = timer - interval;
                    counter++;
                    listener?.Invoke();
                    listenerObj?.Invoke(obj);
                    if (counter >= repeatCount)
                    {
                        state = QTEState.END;
                    }
                }
            }
        }

    }
}
