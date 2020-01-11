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
		// 0 ~ STAGE_WIDTH-1
		public readonly int X;
		// 0 ~ STAGE_HEIGHT-1
		public readonly int Y;

		public StagePosiotion(int x, int y) {
			Debug.Assert(x <= Companion.STAGE_WIDTH - 1);
			Debug.Assert(y <= Companion.STAGE_HEIGHT - 1);

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
		/// Gets if it's in bottom(on floor).
		/// </summary>
		public bool IsBottom {
			get {
				return Y == Companion.STAGE_HEIGHT - 1;
			}
		}
	}
}