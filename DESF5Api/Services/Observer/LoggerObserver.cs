namespace DESF5Api.Services.Observer
{
    public class LoggerObserver : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"[LOG] {DateTime.UtcNow}: {message}");
        }
    }
}