namespace Aiv.Fast2D.Component {
    public class RandomTimer {

        private float timeMin;
        private float timeMax;
        private float remainingSeconds;

        public RandomTimer(float timeMin, float timeMax) {
            this.timeMin = timeMin;
            this.timeMax = timeMax;

            Reset();
        }

        public void Tick() {
            remainingSeconds -= Game.DeltaTime;
        }

        public bool IsOver() {
            return remainingSeconds <= 0;
        }

        public void Reset() {
            remainingSeconds = RandomGenerator.GetRandomFloat(timeMin, timeMax);
        }
    }
}