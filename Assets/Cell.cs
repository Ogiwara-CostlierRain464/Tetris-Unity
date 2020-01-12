namespace Tetris{

	public enum Color {
		Red, Yellow, Green
	}

	/// <summary>
	/// A 1x1 cell.
	/// </summary>
	public class Cell {

		public StagePosiotion Position;
		public Color Color;

		public Cell(StagePosiotion posiotion, Color color) {
			Position = posiotion;
			Color = color;
		}

		public void Draw(Board board) {
			board.DrawAt(Position);
		}
	}
}