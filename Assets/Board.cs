using System.Collections.Generic;
using System.Linq;
using GameCanvas;
using UnityEngine;

namespace Tetris{
    public enum Operation {
		Down, Right, Left, RotateClock, RotateAntiClock
	}

	public class Board {

		List<Cell> Blocks = new List<Cell>();
		Block AcitveBlock = null;

		private Proxy gc;

		public Board(Proxy gc) {
			this.gc = gc;

			AcitveBlock = ChooseNextBlock();
		}

		void Step() {
			// down active blocks.
			// when active block can go donw, go down
			// when cannot, delete some lines.
			if (AcitveBlock.CanDo(Operation.Down, Blocks)) {
				//Debug.Log("Going down. Pos:" + AcitveBlock.BasePosition);
				AcitveBlock.Do(Operation.Down);
			} else {
				Debug.Log("Can't go down anymore.");
				Debug.Log(Blocks);
				// add to cells

				AddActiveBlockToCellsAndNewActiveBlock();
				RemoveDeletableLines();
			}

		}

		/// <summary>
		/// Add ActiveBlock to `Blocks`, and add new ActiveBlock.
		/// </summary>
		private void AddActiveBlockToCellsAndNewActiveBlock() {
			var absPoses = AcitveBlock.AbsolutePosiotions();

			absPoses
				.Select(absPos => new Cell(absPos, AcitveBlock.Color))
				.ToList()
				.ForEach(cell => Blocks.Add(cell));

			AcitveBlock = ChooseNextBlock();
		}

		private Block ChooseNextBlock() {
			var list = new List<Block> {
				new Block(new Shapes.WaveLeft(), Color.red),
				new Block(new Shapes.L(), new Color(1, 0.647f, 0,1)),
				new Block(new Shapes.Box(), Color.yellow),
				new Block(new Shapes.WaveRight(), Color.green),
				new Block(new Shapes.I(), Color.cyan),
				new Block(new Shapes.OppositeL(), Color.blue),
				new Block(new Shapes.T(), new Color(0.5f, 0, 0.5f,1)),
			};

			var code = gc.Random(0, 6);

			return list[code];
		}

		/// <summary>
		/// Called for each frames.
		/// </summary>
		public void Update() {
			if (IsMoveTime()) {
				Step();
			}

			if (gc.GetIsKeyBegan(EKeyCode.Down)) {
				AcitveBlock.DoIfItCan(Operation.Down ,Blocks);
			}
			if (gc.GetIsKeyBegan(EKeyCode.Left)) {
				AcitveBlock.DoIfItCan(Operation.Left, Blocks);
			}
			if (gc.GetIsKeyBegan(EKeyCode.Right)) {
				AcitveBlock.DoIfItCan(Operation.Right, Blocks);
			}
			if (gc.GetIsKeyBegan(EKeyCode.J)) {
				AcitveBlock.DoIfItCan(Operation.RotateAntiClock, Blocks);
			}
			if (gc.GetIsKeyBegan(EKeyCode.K)) {
				AcitveBlock.DoIfItCan(Operation.RotateClock, Blocks);
			}
		}

		private int lastTime = 0;
        
        private bool IsMoveTime() {
			var ceil = (int)gc.TimeSinceStartup;

			if (lastTime < ceil) {
				lastTime = ceil;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Called on `DrawGame`
		/// </summary>
		public void Draw() {
			AcitveBlock.Draw(this);
			Blocks.ForEach(e => e.Draw(this));
		}

		public void DrawAt(StagePosiotion pos, Color color) {
			var pixelPos = pos.ToAbsolutePosition();

			gc.SetColor(color);

			gc.FillRect(
				pixelPos.x,
				pixelPos.y,
				Companion.BLOCK_SIZE,
				Companion.BLOCK_SIZE
			);
		}

		void RemoveDeletableLines() {
			// check for each lines.
			// if deleted, move down all blocks for count of deleted lines.
			// it is clear that maimum delete lines are 4 because of shape.

			// first, count for each lines.
			
			var counts = Enumerable
				.Range(0, Companion.STAGE_HEIGHT)
				.Select(_ => 0)
				.ToList();

			Blocks.ForEach(cell => {
				var y = cell.Position.Y;

				counts[y] += 1;
			});


			var deleteLines = counts
				.Select((item, index) => new { item, index })
				.Where(e => e.item == Companion.STAGE_WIDTH)
				.Select(e => e.index)
				.ToList();

			Debug.Log("Delete for line:" + counts);

			if (deleteLines.Count == 0) return;

			// delete lines.
			Blocks = Blocks
				.Where(cell => !deleteLines.Contains(cell.Position.Y))
				.ToList();

			var deleteLineCount = deleteLines.Count;
			var moveUpOn = deleteLines.Max();

			Blocks = Blocks
				.Select(cell => {
					if (cell.Position.Y < moveUpOn) {
						cell.Position = new StagePosiotion(
							cell.Position.X,
							cell.Position.Y + deleteLineCount
						);
					}

					return cell;
				})
				.ToList();

		}


	}
}