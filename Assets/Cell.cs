namespace Tetris{

	/// <summary>
	/// A 1x1 cell.
	/// </summary>
	public class Cell {
		public StagePosiotion Position;

		public Cell(StagePosiotion posiotion) {
			Position = posiotion;
		}

		public void Draw(Board board) {
			board.DrawAt(Position);
		}
	}
}