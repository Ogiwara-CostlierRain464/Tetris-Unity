using UnityEngine;

namespace Tetris{
		/// <summary>
	/// Coordinate in stage, no relationship with Pixel coordinate.
	/// StagePosition can be absolute position or relative position.
	///  ----> x
	/// |
	/// |
	/// y
	/// </summary>

	public class StagePosiotion {
		public readonly int X;
		public readonly int Y;

		public StagePosiotion(int x, int y) {
			X = x; Y = y;
		}

		override public string ToString() {
			return $"(x:{X} y:{Y})";
		}

		public static StagePosiotion operator +(StagePosiotion pos1, StagePosiotion pos2) =>
			new StagePosiotion(pos1.X + pos2.X, pos1.Y + pos2.Y);

		public Vector2Int ToAbsolutePosition() =>
			new Vector2Int(
				X * Companion.BLOCK_SIZE,
				Y * Companion.BLOCK_SIZE
			);

		/// <summary>
		/// Gets if this position is out of stage
		/// when position is absolute position.
		/// </summary>
		public bool IsOutOfStage {
			get {
				return
					X < 0 ||
					X > Companion.STAGE_WIDTH - 1 ||
					Y < 0 ||
					Y > Companion.STAGE_HEIGHT - 1;

			}
		}

		public bool IsBottom {
			get {
				return Y == Companion.SCREEN_HEIGHT - 1;
			}
		}
	}
}