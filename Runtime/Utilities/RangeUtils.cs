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
        ///     if <paramref name="value" />{x,y} is between <paramref name="minValue" />{x,y} (inclusive) and <paramref name="maxValue" />{x,y} (exclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheck((int a, int b) value, (int a, int b) minValue, (int a, int b) maxValue) {
            return RangeCheck(value.a, minValue.a, maxValue.a) && RangeCheck(value.b, minValue.b, maxValue.b);
        }

        /// <summary>
        ///     Returns <value>true</value>
        ///     if <paramref name="value" />{x,y} is between <paramref name="minValue" />{x,y} (inclusive) and <paramref name="maxValue" />{x,y} (inclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheckInclusive((int a, int b) value, (int a, int b) minValue, (int a, int b) maxValue) {
            return RangeCheckInclusive(value.a, minValue.a, maxValue.a) && RangeCheckInclusive(value.b, minValue.b, maxValue.b);
        }

        /// <summary>
        ///     Returns <value>true</value>
        ///     if <paramref name="value" />{x,y} is between <value>0</value>{x,y} (inclusive) and <paramref name="maxValue" />{x,y} (exclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheck((int a, int b) value, (int a, int b) maxValue) {
            return RangeCheck(value.a, 0, maxValue.a) && RangeCheck(value.b, 0, maxValue.b);
        }

        /// <summary>
        ///     Returns <value>true</value>
        ///     if <paramref name="value" />{x,y} is between <value>0</value>{x,y} (inclusive) and <paramref name="maxValue" />{x,y} (inclusive),
        ///     and <value>false</value> otherwise.
        /// </summary>
        public static bool RangeCheckInclusive((int a, int b) value, (int a, int b) maxValue) {
            return RangeCheckInclusive(value.a, 0, maxValue.a) && RangeCheckInclusive(value.b, 0, maxValue.b);
        }
    }
}