namespace backend.Controllers.Response;

public class InternalResponse<T>
{
    public T Data { get; set; }

    public InternalResponse<T> Success(T data)
    {
        Data = data;
        return this;
    }
}