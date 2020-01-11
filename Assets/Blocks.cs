using System.Collections.Generic;

namespace Tetris.Blocks{
	public class T : Block {

		public T() : base(
			new List<StagePosiotion> {
				new StagePosiotion(1,0),
				new StagePosiotion(1,1),
				new StagePosiotion(2,1),
				new StagePosiotion(2,2),
			}
		) {}

		public override List<StagePosiotion> WhenRotate(Operarion operarion, List<StagePosiotion> posiotions) {
			throw new System.NotImplementedException();
		}
	}
}