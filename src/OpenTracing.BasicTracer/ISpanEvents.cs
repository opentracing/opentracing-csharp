namespace OpenTracing.BasicTracer
{
    public interface ISpanEvents
    {
        void onStart();

        void onFinished();

        void onBaggage(string key, string value);

        void onLog(string message);

        void onTag(string key, string value);
    }
}
