using System.Collections.Generic;
using System.Linq;
using Tetris.Shapes;
using UnityEngine;

namespace Tetris {

	public enum Rotation {
		Home, Clock, AntiClock, Flip
	}

	/// <summary>
	/// A Block like T shape or L shape.
	/// </summary>
	public class Block {

		public IShape Shape;
		public Color Color;
		public Rotation Rotation = Rotation.Home;
		public StagePosiotion BasePosition;

		public Block(IShape shape, Color color) {
			BasePosition = StartPosition();
			Shape = shape;
			Color = color;
		}

		public List<StagePosiotion> AbsolutePosiotions() {
			var currentRelPoses = GetCurrentRelativePositions();

			return currentRelPoses
				.Select(relPos => relPos + BasePosition)
				.ToList();
		}

		private List<StagePosiotion> GetCurrentRelativePositions() =>
			Rotation == Rotation.Home ?
				Shape.HomePosition :
			Rotation == Rotation.Clock ?
				Shape.RotateClock :
			Rotation == Rotation.AntiClock ?
				Shape.RotateAntiClock :
			//default
				Shape.Flip;	


		public void DoIfItCan(Operation operation, List<Cell> stageCells) {
			if(CanDo(operation, stageCells)) {
				Do(operation);
			}
		}

		public void Do(Operation operation) {
			// update base position and rotation.
			if (operation == Operation.RotateAntiClock || operation == Operation.RotateClock) {
				Rotation = NextRotation(operation);
			} else {
				BasePosition = BasePosition + NextMove(operation);
			}
		}

		public bool CanDo(Operation operarion, List<Cell> stageCells) {
			// if it's out of range or collision when operation,
			//then it cannot move with the operation.

			var whenOperation = WhenOperation(operarion);

			var outOfStage = whenOperation.Exists(e => e.IsOutOfStage);
			if (outOfStage) return false;

			var collision = whenOperation.Exists(absPos =>
					stageCells.Exists(stageCell =>
						stageCell.Position == absPos
					)
				);

			if (collision) return false;

			return true;
		}

		private List<StagePosiotion> WhenOperation(Operation operation) =>	
			operation == Operation.RotateClock || operation == Operation.RotateAntiClock ?
				WhenRotate(operation) :
			// default
				WhenMove(operation);

		private List<StagePosiotion> WhenMove(Operation operation) {
			var adder = NextMove(operation);

			return AbsolutePosiotions()
				.Select(pos => pos + adder)
				.ToList();
		}

		private StagePosiotion NextMove(Operation operarion) =>
			operarion == Operation.Down ?
				new StagePosiotion(0, 1) :
			operarion == Operation.Left ?
				new StagePosiotion(-1, 0) :
				//default
				new StagePosiotion(1, 0);

		private List<StagePosiotion> WhenRotate(Operation operarion) {
			var nextRotation = NextRotation(operarion);

			var rotationRelativePoses =
				nextRotation == Rotation.Home ?
					Shape.HomePosition :
				nextRotation == Rotation.Clock ?
					Shape.RotateClock :
				nextRotation == Rotation.AntiClock ?
					Shape.RotateAntiClock :
					//default
					Shape.Flip;

			return rotationRelativePoses
				.Select(relPos => relPos + BasePosition)
				.ToList();
		}

		private Rotation NextRotation(Operation operation) {
			if (operation == Operation.RotateClock) {
				switch (Rotation) {
					case Rotation.Home:
						return Rotation.Clock;
					case Rotation.Clock:
						return Rotation.Flip;
					case Rotation.AntiClock:
						return Rotation.Home;
					default:
						return Rotation.AntiClock;
				}
			} else { // RotateAntiClock
				switch (Rotation) {
					case Rotation.Home:
						return Rotation.AntiClock;
					case Rotation.Clock:
						return Rotation.Home;
					case Rotation.AntiClock:
						return Rotation.Flip;
					default:
						return Rotation.Clock;
				}
			}
		}

		public bool IsBottom =>
			AbsolutePosiotions()
				.Exists(absPos => absPos.IsBottom);

		public void Draw(Board board) {
			AbsolutePosiotions()
				.ForEach(absPos => board.DrawAt(absPos, Color));

		}

		override public string ToString() {
			return $"(base: {BasePosition})";
		}

		static StagePosiotion StartPosition() =>
			new StagePosiotion(Companion.STAGE_WIDTH / 2, 0);
	}
}