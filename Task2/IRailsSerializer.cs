using Task1;

namespace Task2 {
    public interface IRailsSerializer {
        RailsInfo DeserializeRails(string path);
        bool TryDeserializeRails(string path, out RailsInfo rails);

        void SerializeRails(string path, RailsInfo rails);
    }
}
