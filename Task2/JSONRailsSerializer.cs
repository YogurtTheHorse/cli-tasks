using Newtonsoft.Json;
using System.IO;
using Task1;

namespace Task2 {
    public class JSONRailsSerializer : IRailsSerializer {
        private JsonSerializer _jsonSerializer;

        public JSONRailsSerializer() {
            _jsonSerializer = new JsonSerializer();
        }

        public RailsInfo DeserializeRails(string path) {
            using (StreamReader sw = new StreamReader(path)) {
                using (JsonReader reader = new JsonTextReader(sw)) {
                    return _jsonSerializer.Deserialize<RailsInfo>(reader);
                }
            }
        }

        public void SerializeRails(string path, RailsInfo rails) {
            using (StreamWriter sw = new StreamWriter(path)) {
                using (JsonWriter writer = new JsonTextWriter(sw)) {
                    _jsonSerializer.Serialize(writer, rails);
                }
            }
        }

        public bool TryDeserializeRails(string path, out RailsInfo rails) {
            try {
                rails = DeserializeRails(path);
                return true;
            } catch {
                rails = null;
                return false;
            }
        }
    }
}
