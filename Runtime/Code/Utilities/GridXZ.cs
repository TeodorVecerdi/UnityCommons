using TMPro;
using UnityEngine;

namespace UnityCommons {
    public class GridXZ<T> {
        public event GridValueChangedEvent OnGridValueChanged;

        private readonly int width;
        private readonly int height;
        private readonly float cellSize;
        private readonly Vector3 gridOrigin;
        private readonly T[,] grid;
        private readonly TextMeshPro[,] debugText;

        public GridXZ(int width, int height, float cellSize, Vector3 gridOrigin = default, T startingValue = default, bool debug = false, DebugOptions? debugOptions = null) {
            debugOptions ??= new DebugOptions();
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.gridOrigin = gridOrigin;

            grid = new T[width, height];
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    grid[x, y] = startingValue;
                }
            }

            if (!debug) return;
            
            var rotation = Quaternion.Euler(90, 0, 0);
            debugText = new TextMeshPro[width, height];
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    Debug.DrawLine(GetWorldCoordinates(x, y), GetWorldCoordinates(x, y + 1), Color.white, debugOptions.Value.LineDuration, false);
                    Debug.DrawLine(GetWorldCoordinates(x, y), GetWorldCoordinates(x + 1, y), Color.white, debugOptions.Value.LineDuration, false);

                    debugText[x, y] = Utils.CreateWorldText(grid[x, y].ToString(), position: GetWorldCoordinates(x, y) + new Vector3(cellSize, cellSize) * 0.5f,
                                                            fontSize: debugOptions.Value.FontSize, rotation: rotation, horizontalAlignment: HorizontalAlignmentOptions.Center,
                                                            verticalAlignment: VerticalAlignmentOptions.Middle);
                }
            }

            Debug.DrawLine(GetWorldCoordinates(0, height), GetWorldCoordinates(width, height), Color.white, debugOptions.Value.LineDuration, false);
            Debug.DrawLine(GetWorldCoordinates(width, 0), GetWorldCoordinates(width, height), Color.white, debugOptions.Value.LineDuration, false);
            OnGridValueChanged += args => { debugText[args.X, args.Y].text = args.NewValue.ToString(); };
        }

        public Vector3 GetWorldCoordinates(int x, int y) {
            return new Vector3(x, 0, y) * cellSize + gridOrigin;
        }

        public (int x, int y) GetGridCoordinates(Vector3 worldCoordinates) {
            worldCoordinates -= gridOrigin;
            return (Mathf.FloorToInt(worldCoordinates.x / cellSize), Mathf.FloorToInt(worldCoordinates.z / cellSize));
        }

        public T this[int x, int y] {
            get {
                if (Utils.RangeCheck(x, width) && Utils.RangeCheck(y, height)) {
                    return grid[x, y];
                }

                return default;
            }
            set {
                if (Utils.RangeCheck(x, width) && Utils.RangeCheck(y, height)) {
                    grid[x, y] = value;
                    OnGridValueChanged?.Invoke((x, y, value));
                }
            }
        }

        public T this[Vector3 worldPosition] {
            get {
                var (x, y) = GetGridCoordinates(worldPosition);
                return this[x, y];
            }
            set {
                var (x, y) = GetGridCoordinates(worldPosition);
                this[x, y] = value;
            }
        }

        public delegate void GridValueChangedEvent(GridValueChangedEventArgs eventArgs);

        public readonly struct GridValueChangedEventArgs {
            public readonly int X;
            public readonly int Y;
            public readonly T NewValue;

            public GridValueChangedEventArgs(int x, int y, T newValue) {
                X = x;
                Y = y;
                NewValue = newValue;
            }

            public static implicit operator GridValueChangedEventArgs((int x, int y, T value) tuple) {
                var (x, y, value) = tuple;
                return new GridValueChangedEventArgs(x, y, value);
            }
        }
    }
}