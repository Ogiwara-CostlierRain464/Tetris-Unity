using System;

namespace Tetris{
	public static class Companion {
		private static readonly int ASPECT = 3;

		public static readonly int SCREEN_WIDTH = 240;
		public static readonly int SCREEN_HEIGHT = (int)(SCREEN_WIDTH * ASPECT);

		public static readonly int STAGE_WIDTH = 10;
		public static readonly int STAGE_HEIGHT = STAGE_WIDTH * ASPECT;

		public static readonly int BLOCK_SIZE = SCREEN_WIDTH / STAGE_WIDTH;
	}
}