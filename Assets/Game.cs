using System.Collections.Generic;
using System.Linq;
using GameCanvas;
using UnityEngine;
using Sequence = System.Collections.IEnumerator;

public sealed class Game : GameBase
{
	Tetris.Board Board;

    
    public override void InitGame()
    {
		Board = new Tetris.Board(gc);

		gc.SetResolution(
			Tetris.Companion.SCREEN_WIDTH,
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

namespace Tetris {
	static class Companion {
		private static readonly int ASPECT = 3;

		public static readonly int SCREEN_WIDTH = 240;
		public static readonly int SCREEN_HEIGHT = (int)(SCREEN_WIDTH * ASPECT);

		public static readonly int STAGE_WIDTH = 10;
		public static readonly int STAGE_HEIGHT = STAGE_WIDTH * ASPECT;

		public static readonly int BLOCK_SIZE = SCREEN_WIDTH / STAGE_WIDTH;
	}

	/// <summary>
	/// Coordinate in stage, no relationship with Pixel coordinate.
	/// StagePosition can be absolute position or relative position.
	///  ----> x
	/// |
	/// |
	/// y
	/// </summary>

	class StagePosiotion {
		// 0 ~ STAGE_WIDTH-1
		public readonly int X;
		// 0 ~ STAGE_HEIGHT-1
		public readonly int Y;

		public StagePosiotion(int x, int y) {
			Debug.Assert(x <= Companion.STAGE_WIDTH - 1);
			Debug.Assert(y <= Companion.STAGE_HEIGHT - 1);

			X = x; Y = y;
		}

		override public string ToString() {
			return $"(x:{X} y:{Y})";
		}

		public static StagePosiotion operator +(StagePosiotion pos1, StagePosiotion pos2) =>
			new StagePosiotion(pos1.X + pos2.X, pos1.Y + pos2.Y);

		public Vector2Int ToAbsolutePosition() =>
			new Vector2Int(
				X * Companion.BLOCK_SIZE,
				Y * Companion.BLOCK_SIZE
			);

		/// <summary>
		/// Gets if it's in bottom(on floor).
		/// </summary>
		public bool IsBottom {
			get {
				return Y == Companion.STAGE_HEIGHT - 1;
			}
		}
	}

	/// <summary>
	/// A 1x1 cell.
	/// </summary>
	class Cell {
		public StagePosiotion Position;

		public Cell(StagePosiotion posiotion) {
			Position = posiotion;
		}

		public void Draw(Board board) {
			board.DrawAt(Position);
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
		public List<StagePosiotion> RelativePositions;

		public Block(List<StagePosiotion> relativePositions) {
			BasePosition = StartPosition();
			RelativePositions = relativePositions;
		}

		public List<StagePosiotion> AbsolutePosiotions() {
			var absPos = RelativePositions
				.Select(relPos => relPos + BasePosition)
				.ToList();

			absPos.Add(BasePosition);

			return absPos;
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



	enum Operarion {
		Down, Right, Left, RotateClock, RotateAntiClock
	}

	class Board {
		const int WIDTH = 10;
		const int HEIGHT = 30;

		// [[Cell;WIDTH]; HEIGHT]
		List<List<Cell>> Blocks = null;
		Block AcitveBlock = null;

		private Proxy gc;

		public Board(Proxy gc) {
			this.gc = gc;

			Blocks = Enumerable
			.Range(0, HEIGHT)
			.Select(index => new List<Cell>(WIDTH))
			.ToList();

			AcitveBlock = new Block(
				new List<StagePosiotion> {
					new StagePosiotion(0,0),
					new StagePosiotion(0,1)
				}
			);
		}

		void Step() {
			// down active blocks.
			// when active block can go donw, go down
			// when cannot, delete some lines.
			if (AcitveBlock.CanGoDown(Blocks)) {
				Debug.Log("Going down.");
				AcitveBlock.GoDown();
			} else {
				Debug.Log("Can't go down anymore.");
				// add to cells
				RemoveDeletableLines();

			}

		}


		private void AddActiveBlockToCellsAndNewActiveBlock() {

		}

		/// <summary>
		/// Called each frames.
		/// </summary>
		/// <param name="gc"></param>
		public void Update(Proxy gc) {
			if (gc.GetIsKeyBegan(EKeyCode.Down)) {
				Step();
				Debug.Log(AcitveBlock);
			}
		}

		/// <summary>
		/// Called on `DrawGame`
		/// </summary>
		public void Draw() {
			AcitveBlock.Draw(this);
			Blocks.ForEach(line => line.ForEach(e => e.Draw(this)));
		}

		public void DrawAt(StagePosiotion pos) {
			var pixelPos = pos.ToAbsolutePosition();

			gc.FillRect(
				pixelPos.x,
				pixelPos.y,
				Companion.BLOCK_SIZE,
				Companion.BLOCK_SIZE
			);
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
