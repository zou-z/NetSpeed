namespace NetSpeed.DataType
{
    internal struct RefreshIntervals
    {
        public const int Interval_1000 = 1000;
        public const int Interval_500 = 500;
        public const int Interval_200 = 200;
        public const int Interval_100 = 100;

        public const int Default = Interval_1000;

        public static int[] GetValues()
        {
            return new int[]
            {
                Interval_1000,
                Interval_500,
                Interval_200,
                Interval_100
            };
        }
    }
}
