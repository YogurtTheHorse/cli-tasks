using System;
using System.Collections.Generic;

namespace Task1 {
    public class RailsInfo {
        public List<BlockState> Blocks;
        public int BlocksCount => Blocks.Count;

        public RailsInfo() {
            Blocks = new List<BlockState>();
        }

        public void PushBlockState(BlockState blockstate) {
            if (blockstate == BlockState.Red && BlocksCount > 0 && Blocks[Blocks.Count - 1] == BlockState.Green) {
                Blocks[Blocks.Count - 1] = BlockState.Yellow;
            }

            if (blockstate == BlockState.Incorrect) {
                throw new InvalidOperationException();
            }

            Blocks.Add(blockstate);
        }
    }
}
