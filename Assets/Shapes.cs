using System.Collections.Generic;

namespace Tetris.Shapes{
	public interface IShape {
		List<StagePosiotion> HomePosition { get; }
		List<StagePosiotion> RotateClock { get; }
		List<StagePosiotion> RotateAntiClock { get; }
		List<StagePosiotion> Flip { get; }
	}

	public class T : IShape {

		// ⊥
		public List<StagePosiotion> HomePosition {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(1,0),
					new StagePosiotion(0,1),
					new StagePosiotion(1,1),
					new StagePosiotion(2,1),
				};
			}
		}

		// |-
		public List<StagePosiotion> RotateClock {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(0,0),
					new StagePosiotion(0,1),
					new StagePosiotion(1,1),
					new StagePosiotion(0,2),
				};
			}
		}

		// -|
		public List<StagePosiotion> RotateAntiClock {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(1,0),
					new StagePosiotion(0,1),
					new StagePosiotion(1,1),
					new StagePosiotion(1,2),
				};
			}
		}

		// T
		public List<StagePosiotion> Flip {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(0,0),
					new StagePosiotion(1,0),
					new StagePosiotion(2,0),
					new StagePosiotion(1,1),
				};
			}
		}
	}

	public class I : IShape {

		// I
		public List<StagePosiotion> HomePosition {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(0,-1),
					new StagePosiotion(0,0),
					new StagePosiotion(0,1),
					new StagePosiotion(0,2),
				};
			}
		}

		// -
		public List<StagePosiotion> RotateClock {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(-1,0),
					new StagePosiotion(0,0),
					new StagePosiotion(1,0),
					new StagePosiotion(2,0),
				};
			}
		}

		// I
		public List<StagePosiotion> RotateAntiClock {
			get {
				return RotateClock;
			}
		}

		// -
		public List<StagePosiotion> Flip {
			get {
				return HomePosition;
			}
		}
	}

	public class L : IShape {

		// L
		public List<StagePosiotion> HomePosition {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(0,-1),
					new StagePosiotion(0,0),
					new StagePosiotion(0,1),
					new StagePosiotion(1,1),
				};
			}
		}

		// 『
		public List<StagePosiotion> RotateClock {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(0,0),
					new StagePosiotion(1,0),
					new StagePosiotion(-1,0),
					new StagePosiotion(-1,1),
				};
			}
		}

		// 』
		public List<StagePosiotion> RotateAntiClock {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(-1,0),
					new StagePosiotion(0,0),
					new StagePosiotion(1,0),
					new StagePosiotion(1,-1),
				};
			}
		}

		// --
		//  |
		//  |
		public List<StagePosiotion> Flip {
			get {
				return new List<StagePosiotion> {
					new StagePosiotion(0,0),
					new StagePosiotion(-1,-1),
					new StagePosiotion(0,-1),
					new StagePosiotion(0,1),
				};
			}
		}
	}
}