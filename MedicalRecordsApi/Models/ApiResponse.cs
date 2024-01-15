namespace MedicalRecordsApi.Models
{
    public class ApiResponse<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public int Status { get; set; }
        public string? Reason { get; set; } = string.Empty;

        //T can be a class or List of class List<class> or null
        public T Data { get; set; } = null;
    }
}
