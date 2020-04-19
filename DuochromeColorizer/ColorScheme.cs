namespace ChromaSchemeColorizer
{
    internal class ColorScheme
    {
        internal float time { get; }
        internal float[] colorLeft { get; }
        internal float[] colorRight { get; }
        internal float[] envColorLeft { get => _envColorLeft ?? colorLeft; }
        private float[] _envColorLeft;
        internal float[] envColorRight { get => _envColorRight ?? colorRight; }
        private float[] _envColorRight;
        internal float[] obstacleColor { get; }
        internal float[] bombColor { get; }

        internal ColorScheme(float time, float[] colorLeft, float[] colorRight, float[] envColorLeft, float[] envColorRight, float[] obstacleColor, float[] bombColor)
        {
            this.time = time;
            this.colorLeft = colorLeft;
            this.colorRight = colorRight;
            _envColorLeft = envColorLeft;
            _envColorRight = envColorRight;
            this.obstacleColor = obstacleColor;
            this.bombColor = bombColor;
        }
    }
}