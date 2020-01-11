using System.Collections.Generic;
using System.Linq;
using GameCanvas;
using UnityEngine;
using Sequence = System.Collections.IEnumerator;

public sealed class Game : GameBase
{
	const int WIDTH = 720;
	Tetris.Board Board = new Tetris.Board();

    
    public override void InitGame()
    {
        // キャンバスの大きさを設定します
        gc.SetResolution(720, 1280);
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

        // 黒の文字を描画します
        gc.SetColor(0, 0, 0);
        gc.SetFontSize(48);
        gc.DrawString("この文字と青空の画像が", 40, 160);
        gc.DrawString("見えていれば成功です", 40, 270);
    }
}

namespace Tetris {
	static class Companion {
		public static readonly int STAGE_WIDTH = 10;
		public static readonly int STAGE_HEIGHT = 30;
	}

	/// <summary>
	/// Stage posiotion.
	/// ----> x
	/// |
	/// |
	/// |
	/// y
	/// </summary>
	class StagePosiotion {
		private int _x;
		private int _y;

		public int X {
			set {
				if (Companion.STAGE_HEIGHT >= value) {
					_x = value;
				} else {
					_x = Companion.STAGE_HEIGHT;
				}
			}
			get {
				return _x;
			}
		}

		public int Y {
			set {
				if (Companion.STAGE_HEIGHT >= value) {
					_y = value;
				} else {
					_y = Companion.STAGE_HEIGHT;
				}
			}
			get {
				return _y;
			}
		}

		public StagePosiotion(int x, int y) {
			Debug.Assert(x <= Companion.STAGE_WIDTH);
			Debug.Assert(y <= Companion.STAGE_HEIGHT);

			X = x; Y = y;
		}

		override public string ToString() {
			return $"(x:{X} y:{Y})";
		}

		public static StagePosiotion operator +(StagePosiotion pos1, StagePosiotion pos2) =>
			new StagePosiotion(pos1.X + pos2.X, pos1.Y + pos2.Y);



	}

	/// <summary>
	/// A 1x1 cell.
	/// </summary>
	class Cell {
		public StagePosiotion Position;

		public Cell(StagePosiotion posiotion) {
			Position = posiotion;
		}
	}

	/// <summary>
	/// A block.
	/// </summary>
	class Block {
		// e.g. ---- 
		//  □
		// □□□
		//
		// □□□□

		// Block does not own it's type

		/// <summary>
		/// The base position.
		/// </summary>
		public StagePosiotion BasePosition;
		public List<StagePosiotion> AbsolutePositions;


		public Block(List<StagePosiotion> absolutePositions) {
			BasePosition = Start();
			AbsolutePositions = absolutePositions;
		}

		public bool CanGoDown(List<List<Cell>> stageCells) =>
			!AbsolutePositions.Exists(absolutePosition => {
				return stageCells.Exists(line => line.Exists(stageCell => {
					return stageCell.Position == absolutePosition;
				}));
			});

		public void GoDown() {
			BasePosition.Y += 1;
		}

		public bool CanRotate() {
			return false;
		}

		public List<StagePosiotion> Positions() =>
			AbsolutePositions.Select(position => position + BasePosition).ToList();
		

		override public string ToString() {
			return $"(base: {BasePosition})";
		}

		static StagePosiotion Start() => new StagePosiotion(Companion.STAGE_WIDTH / 2, 0);
	}



	enum Operarion {
		Down, Right, Left, RotateClock, RotateAntiClock
	}

	class Board {
		const int WIDTH = 10;
		const int HEIGHT = 30;

		// [[Cell;WIDTH]; HEIGHT]
		List<List<Cell>> Blocks = null;
		Block AcitveBlock = null;

		public Board() {
			Blocks = Enumerable
			.Range(0, HEIGHT)
			.Select(index => new List<Cell>(WIDTH))
			.ToList();

			AcitveBlock = new Block(new List<StagePosiotion> { new StagePosiotion(0,0), new StagePosiotion(0,1)  });
		}

		void Step() {
			// down active blocks.
			// when active block can go donw, go down
			// when cannot, delete some lines.
			if (AcitveBlock.CanGoDown(Blocks)) {
				AcitveBlock.GoDown();
			} else {
				// add to cells
				RemoveDeletableLines();
			}

		}

		/// <summary>
		/// Move ActiveBlock cell to stage.
		/// </summary>
		public void MoveToStage() {

		}

		public void Update(Proxy gc) {
			if (gc.GetIsKeyPress(EKeyCode.Down)) {
				Step();

				Debug.Log(AcitveBlock);
			}
		}

		void RemoveDeletableLines() {
			Enumerable.Range(0, 4).ToList().ForEach(index => {
				var line = Blocks[index];
				if (LineCanRemove(line)) {
					Blocks.Remove(line);
				}
			});
		}

		bool LineCanRemove(List<Cell> line) =>
			line.Count == WIDTH;

	}
}
