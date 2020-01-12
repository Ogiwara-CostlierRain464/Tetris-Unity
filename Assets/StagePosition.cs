using System;
using System.Collections.Generic;
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

	public class StagePosiotion : IEquatable<StagePosiotion> {
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

		public static bool operator ==(StagePosiotion left, StagePosiotion right) {
			return EqualityComparer<StagePosiotion>.Default.Equals(left, right);
		}

		public static bool operator !=(StagePosiotion left, StagePosiotion right) {
			return !(left == right);
		}

		public override bool Equals(object obj) {
			if ((obj == null) || !this.GetType().Equals(obj.GetType())) {
				return false;
			} else {
				StagePosiotion pos = (StagePosiotion) obj;
				return (pos.X == X) && (pos.Y == Y);
			}
		}

		public bool Equals(StagePosiotion other) {
			return other != null &&
				   X == other.X &&
				   Y == other.Y &&
				   IsOutOfStage == other.IsOutOfStage &&
				   IsBottom == other.IsBottom;
		}

		public override int GetHashCode() {
			var hashCode = 1221827801;
			hashCode = hashCode * -1521134295 + X.GetHashCode();
			hashCode = hashCode * -1521134295 + Y.GetHashCode();
			hashCode = hashCode * -1521134295 + IsOutOfStage.GetHashCode();
			hashCode = hashCode * -1521134295 + IsBottom.GetHashCode();
			return hashCode;
		}

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