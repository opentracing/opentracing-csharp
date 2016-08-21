namespace OpenTracing
{
    public interface ISpanContext
    {
        string GetBaggageItem(string key);

        void SetBaggageItem(string key, string value);
    }
}