namespace PaymentAPI.Model
{
    public class Result<T>
    {
        public T Entity { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
