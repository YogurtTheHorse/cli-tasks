using Task1.TasksIO;

namespace Task1 {
    public class RailsReader {
        private TaskIO _taskIO;

        public int BlockCount { get; protected set; }

        public RailsReader(TaskIO taskIO) {
			_taskIO = taskIO;
        }

        public RailsInfoParseResult ReadRailsInfo() {
            RailsInfo railsInfo = new RailsInfo();

            if (!_taskIO.ReadInteger("Blocks count: ", out int blocksCount)) {
                return new RailsInfoParseResult() {
                    Error = "Couldn't read blocks count"
                };
            }

            if (blocksCount <= 0) {
                return new RailsInfoParseResult() {
                    Error = "Blocks count must be graeter than zero"
                };
            }

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