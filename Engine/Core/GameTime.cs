namespace Engine.Core
{
    public class GameTime
    {
        public TimeSpan TotalGametime { get; set; }
        public TimeSpan ElapsedGametime { get; set; }

        public float Delta { get { return (float)ElapsedGametime.TotalSeconds; } }

        public GameTime()
        {
            TotalGametime = TimeSpan.Zero;
            ElapsedGametime = TimeSpan.Zero;
        }

        public GameTime(TimeSpan totalGametime, TimeSpan elapsedGametime)
        {
            TotalGametime = totalGametime;
            ElapsedGametime = elapsedGametime;
        }
    }
}
