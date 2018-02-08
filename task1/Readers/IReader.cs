namespace task1.Readers {
    public interface IReader<T> {
        bool Read(string message, out T res);
    }
}