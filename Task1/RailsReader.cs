using Task1.TasksIO;

namespace Task1 {
    public class RailsReader {
        private ITaskIO _taskIO;

        public int BlockCount { get; protected set; }

        public RailsReader(ITaskIO taskIO) {
			_taskIO = taskIO;
        }

        public RailsInfoParseResult ReadRailsInfo() {
            RailsInfo railsInfo = new RailsInfo();

			int blocksCount = 1;

			do {
				if (blocksCount <= 0) {
					_taskIO.WriteLine("Blocks count must be greater than 0.");
				}

				if (!_taskIO.ReadInteger("Blocks count: ", out blocksCount)) {
					return new RailsInfoParseResult() {
						Error = "Couldn't read blocks count"
					};
				}
			} while (blocksCount <= 0);


			BlockCount = blocksCount;

            for (int i = 0; i < blocksCount; ++i) {
                if (!_taskIO.ReadBlock($"Block {i + 1} state (R/Y/G): ", out BlockState state)) {
                    return new RailsInfoParseResult() {
                        Error = $"Couldn't read {i + 1} block."
                    };
                }

                railsInfo.PushBlockState(state);
            }

            return new RailsInfoParseResult() {
                Info = railsInfo
            };
        }
    }
}