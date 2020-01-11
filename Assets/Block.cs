using System;
using System.Collections.Generic;
using System.Linq;
using Tetris;

namespace Tetris{

	/// <summary>
	/// A Block like T shape or L shape.
	/// </summary>
	public abstract class Block {
		// e.g. ---- 
		//  □
		// □□□
		//
		// □□□□

		// 

		/// <summary>
		/// Base position of block.
		/// </summary>
		public StagePosiotion BasePosition;
		public List<StagePosiotion> RelativePositions;

		public Block(List<StagePosiotion> relativePositions) {
			BasePosition = StartPosition();
			RelativePositions = relativePositions;
		}

		public List<StagePosiotion> AbsolutePosiotions() {
			return RelativePositions
				.Select(relPos => relPos + BasePosition)
				.ToList();
		}

		public void GoDownIfItCan(List<List<Cell>> stageCells) {
			if (CanGoDown(stageCells)) {
				GoDown();
			}
		}

		public bool CanDo(Operarion operarion, List<List<Cell>> stageCells) {
			var absPoses = AbsolutePosiotions();

			var bottom = absPoses.Exists(e => e.IsBottom);
			if (bottom) return false;

			var whenOperation = WhenOperation(operarion, absPoses);

			var collision = whenOperation.Exists(absPos =>
					stageCells.Exists(line =>
						line.Exists(stageCell =>
							stageCell.Position == absPos
						)
					)
				);

			if (collision) return false;

			return true;
		}

		private List<StagePosiotion> WhenOperation(Operarion operarion, List<StagePosiotion> posiotions) {
			if(
				operarion == Operarion.RotateClock
				|| operarion == Operarion.RotateAntiClock) return WhenRotate(operarion, posiotions);

			var adder =
				operarion == Operarion.Down?
					new StagePosiotion(0, 1):
				operarion == Operarion.Left?
					new StagePosiotion(-1, 0):
				//default
					new StagePosiotion(1, 0);

			return posiotions
				.Select(pos => pos + adder)
				.ToList();
		}

		public abstract List<StagePosiotion> WhenRotate(Operarion operarion, List<StagePosiotion> posiotions);


		public bool CanGoDown(List<List<Cell>> stageCells) {
			return CanDo(Operarion.Down, stageCells);
		}

		public void GoDown() {
			BasePosition = new StagePosiotion(
					BasePosition.X,
					BasePosition.Y + 1
				);
		}

		public bool CanRotate() {
			return false;
		}

		public void Draw(Board board) {
			// TODO: Call Cells' draw method.
			AbsolutePosiotions()
				.ForEach(absPos => board.DrawAt(absPos));

		}

		override public string ToString() {
			return $"(base: {BasePosition})";
		}

		static StagePosiotion StartPosition() =>
			new StagePosiotion(Companion.STAGE_WIDTH / 2, 0);
	}
}