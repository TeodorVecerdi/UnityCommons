namespace UnityCommons {
    public static partial class Utils {
        /// <summary>
        ///     Returns <value>true</value>
        ///     if <paramref name="value" /> is between <paramref name="minValue" /> (inclusive) and <paramref name="maxValue" /> (exclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheck(int value, int minValue, int maxValue) {
            return value >= minValue && value < maxValue;
        }

        /// <summary>
        ///     Returns <value>true</value>
        ///     if <paramref name="value" /> is between <paramref name="minValue" /> (inclusive) and <paramref name="maxValue" /> (inclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheckInclusive(int value, int minValue, int maxValue) {
            return value >= minValue && value <= maxValue;
        }

        /// <summary>
        ///     Returns <value>true</value>
        ///     if <paramref name="value" /> is between <value>0</value> (inclusive) and <paramref name="maxValue" /> (exclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheck(int value, int maxValue) {
            return RangeCheck(value, 0, maxValue);
        }

        /// <summary>
        ///     Returns <value>true</value>
        ///     if <paramref name="value" /> is between <value>0</value> (inclusive) and <paramref name="maxValue" /> (inclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheckInclusive(int value, int maxValue) {
            return RangeCheckInclusive(value, 0, maxValue);
        }
        
        /// <summary>
        ///     Returns <value>true</value>
        ///     if <paramref name="value" /> is between <paramref name="minValue" /> (inclusive) and <paramref name="maxValue" /> (exclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheck(float value, float minValue, float maxValue) {
            return value >= minValue && value < maxValue;
        }
        /// <summary>
        ///     Returns <value>true</value>
        ///     if <paramref name="value" /> is between <value>0</value> (inclusive) and <paramref name="maxValue" /> (exclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheck(float value, float maxValue) {
            return RangeCheck(value, 0, maxValue);
        }
    }
}