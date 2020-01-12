using Tetris;

public sealed class Game : GameBase
{
	Board Board;

    public override void InitGame()
    {
		Board = new Board(gc);

		gc.SetResolution(
			Companion.SCREEN_WIDTH,
			Companion.SCREEN_HEIGHT);
    }
    
    public override void UpdateGame()
    {
		Board.Update();
    }
    
    public override void DrawGame()
    { 
        // 画面を白で塗りつぶします
        gc.ClearScreen();

        // 0番の画像を描画します
        gc.DrawImage(0, 0, 0);

		Board.Draw();
    }
}
