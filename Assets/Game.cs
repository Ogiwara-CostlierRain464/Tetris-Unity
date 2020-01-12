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
        gc.ClearScreen();
        gc.DrawImage(0, 0, 0);

        gc.SetColor(0,0,0);
        gc.SetFontSize(24);
        gc.DrawString("矢印キーで移動、\nJとKで回転", 20, 20);

        Board.Draw();
    }
}
