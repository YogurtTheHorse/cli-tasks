using System;

namespace Task1 {
    public struct RailsInfoParseResult {
        private RailsInfo _info;

        public RailsInfo Info {
            get {
                if (IsSuccess) {
                    return _info;
                } else {
                    throw new InvalidOperationException("Rails didn't parse correct. Tried to get not parsed info.");
                }
            }
            set {
                if (IsSuccess) {
                    _info = value;
                } else {
                    throw new InvalidOperationException("Rails didn't parse correct. Tried to set not parsed info.");
                }
            }
        }

        public bool IsSuccess {
            get {
                return String.IsNullOrEmpty(Error);
            }
        }

        public string Error { get; set; }
    }
}
