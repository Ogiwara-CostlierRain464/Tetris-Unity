using System.Collections.Generic;
using System.Linq;
using Tetris;

namespace Tetris{

	/// <summary>
	/// A Block like T shape or L shape.
	/// </summary>
	public class Block {
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

		public bool CanGoDown(List<List<Cell>> stageCells) {
			var absPoses = AbsolutePosiotions();

			var bottom = absPoses.Exists(e => e.IsBottom);
			if (bottom) return false;

			var whenGoDown = absPoses
				.Select(pos => new StagePosiotion(pos.X, pos.Y + 1))
				.ToList();

			var collision = whenGoDown.Exists(absPos =>
					stageCells.Exists(line =>
						line.Exists(stageCell =>
							stageCell.Position == absPos
						)
					)
				);

			if (collision) return false;


			return true;
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