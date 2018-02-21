using System.IO;
using System.Xml.Serialization;

using task1;

namespace task2 {
    public class XmlRailsSerializer : IRailsSerializer {
        private XmlSerializer _serializer;

        public XmlRailsSerializer() {
            _serializer = new XmlSerializer(typeof(RailsInfo));
        }

        public RailsInfo DeserializeRails(string path) {
            using (StreamReader streamReader = new StreamReader(path)) {
                return _serializer.Deserialize(streamReader) as RailsInfo;
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

        public void SerializeRails(string path, RailsInfo rails) {
            using (StreamWriter streamWriter = new StreamWriter(path)) {
                _serializer.Serialize(streamWriter, rails);
            }
        }
    }
}
