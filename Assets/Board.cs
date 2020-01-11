using System.Collections.Generic;
using System.Linq;
using GameCanvas;
using UnityEngine;

namespace Tetris{
    public enum Operarion {
		Down, Right, Left, RotateClock, RotateAntiClock
	}

	public class Board {
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

			AcitveBlock = new Blocks.T();
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
		/// Called for each frames.
		/// </summary>
		public void Update() {
			if (IsMoveTime()) {
				Step();
			}

			if (gc.GetIsKeyBegan(EKeyCode.Down)) {
				AcitveBlock.GoDownIfItCan(Blocks);
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