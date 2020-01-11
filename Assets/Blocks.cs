using System.Collections.Generic;

namespace Tetris.Blocks{
	public class T : Block {

		public T() : base(HomePosition) {}

		public override List<StagePosiotion> WhenRotate(Operarion operarion, List<StagePosiotion> posiotions) {
			throw new System.NotImplementedException();
		}

		// ⊥
		private static List<StagePosiotion> HomePosition = new List<StagePosiotion> {
			new StagePosiotion(1,0),
			new StagePosiotion(1,1),
			new StagePosiotion(2,1),
			new StagePosiotion(2,2),
		};

		private static List<StagePosiotion> RotateClock = new List<StagePosiotion> {
			new StagePosiotion(0,0),
			new StagePosiotion(0,1),
			new StagePosiotion(1,1),
			new StagePosiotion(0,2),
		};

		private static List<StagePosiotion> RotateAntiClock = new List<StagePosiotion> {
			new StagePosiotion(1,0),
			new StagePosiotion(0,1),
			new StagePosiotion(1,1),
			new StagePosiotion(1,2),
		};

		private List<StagePosiotion> Flip = new List<StagePosiotion> {
			new StagePosiotion(0,0),
			new StagePosiotion(1,0),
			new StagePosiotion(2,0),
			new StagePosiotion(1,1),
		};
	}
}