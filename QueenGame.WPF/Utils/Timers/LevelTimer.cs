using System.Timers;

namespace QueenGame.WPF.Utils.Timers
{
    public class LevelTimer
    {
        private bool isRunning;
        private int timeDelay;
        private System.Timers.Timer timer;

        public LevelTimer(int timeDelay)
        {
            this.timeDelay = timeDelay;
            timer = new System.Timers.Timer(timeDelay);
            isRunning = false;
        }

        public void Start()
        {
            if (!isRunning)
            {
                timer.Start();
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                timer.Stop();
            }
        }

        public event ElapsedEventHandler Elapsed
        {
            add { timer.Elapsed += value; }
            remove { timer.Elapsed -= value; }
        }
    }
}
