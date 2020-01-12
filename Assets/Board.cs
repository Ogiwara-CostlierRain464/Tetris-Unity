using System.Collections.Generic;
using System.Linq;
using GameCanvas;
using UnityEngine;

namespace Tetris{
    public enum Operation {
		Down, Right, Left, RotateClock, RotateAntiClock
	}

	public class Board {

		// [[Cell;WIDTH]; HEIGHT]
		List<List<Cell>> Blocks = null;
		Block AcitveBlock = null;

		private Proxy gc;

		public Board(Proxy gc) {
			this.gc = gc;

			Blocks = Enumerable
			.Range(0, Companion.STAGE_HEIGHT)
			.Select(index => new List<Cell>(Companion.STAGE_WIDTH))
			.ToList();

			AcitveBlock = new Block(
				new Shapes.T(),
				Color.green
			);
		}

		void Step() {
			// down active blocks.
			// when active block can go donw, go down
			// when cannot, delete some lines.
			if (AcitveBlock.CanDo(Operation.Down, Blocks)) {
				Debug.Log("Going down. Pos:" + AcitveBlock.BasePosition);
				AcitveBlock.Do(Operation.Down);
			} else {
				Debug.Log("Can't go down anymore.");
				// add to cells
				RemoveDeletableLines();
				AddActiveBlockToCellsAndNewActiveBlock();
			}

		}

		/// <summary>
		/// Add ActiveBlock to `Blocks`, and add new ActiveBlock.
		/// </summary>
		private void AddActiveBlockToCellsAndNewActiveBlock() {
			var absPoses = AcitveBlock.AbsolutePosiotions();

			
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
			Blocks.ForEach(line => line.ForEach(e => e.Draw(this)));
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
			Enumerable.Range(0, 4).ToList().ForEach(index => {
				var line = Blocks[index];
				if (LineCanRemove(line)) {
					Blocks.Remove(line);
				}
			});
		}

		bool LineCanRemove(List<Cell> line) =>
			line.Count == Companion.STAGE_WIDTH;

	}
}