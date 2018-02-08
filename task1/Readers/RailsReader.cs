namespace task1.Readers {
    public class RailsReader {
        private IReader<int> _intReader;
        private IReader<BlockState> _blocksReader;

        public int BlockCount { get; protected set; }

        public RailsReader(IReader<int> intReader, IReader<BlockState> blocksReader) {
            _intReader = intReader;
            _blocksReader = blocksReader;
        }

        public RailsInfoParseResult ReadRailsInfo() {
            RailsInfo railsInfo = new RailsInfo(_intReader, _blocksReader);

            if (!_intReader.Read("Blocks count: ", out int blocksCount)) {
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
                if (!_blocksReader.Read($"Block {i + 1} state (R/Y/G): ", out BlockState state)) {
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