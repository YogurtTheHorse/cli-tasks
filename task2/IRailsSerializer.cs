using task1;

namespace task2 {
    public interface IRailsSerializer {
        RailsInfo DeserializeRails(string path);
        bool TryDeserializeRails(string path, out RailsInfo rails);

        void SerializeRails(string path, RailsInfo rails);
    }
}
