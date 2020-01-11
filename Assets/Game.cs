using Tetris;

public sealed class Game : GameBase
{
	Board Board;

    public override void InitGame()
    {
		Board = new Tetris.Board(gc);

		gc.SetResolution(
			Companion.SCREEN_WIDTH,
			Tetris.Companion.SCREEN_HEIGHT);
    }
    
    public override void UpdateGame()
    {
		Board.Update(gc);
    }
    
    public override void DrawGame()
    { 
        // 画面を白で塗りつぶします
        gc.ClearScreen();

        // 0番の画像を描画します
        gc.DrawImage(0, 0, 0);

		Board.Draw();

        // 黒の文字を描画します
        gc.SetColor(0, 0, 0);
        gc.SetFontSize(48);
        gc.DrawString("この文字と青空の画像が", 40, 160);
        gc.DrawString("見えていれば成功です", 40, 270);
    }
}
