using System.Net;
using System.Text.Json.Serialization;

namespace EngConnect.BuildingBlock.Contracts.Shared;

public class Result
{
    // protected Result(bool isSuccess, Error? error)
    // {
    //     if (isSuccess && error != null || !isSuccess && error == null)
    //     {
    //         throw new InvalidOperationException();
    //     }
    //
    //     IsSuccess = isSuccess;
    //     Error = error;
    // }

    protected Result(bool isSuccess, HttpStatusCode httpStatusCode, Error? error)
    {
        if (isSuccess && error != null || !isSuccess && error == null)
        {
            throw new InvalidOperationException();
        }
        
        IsSuccess = isSuccess;
        Error = error;
        HttpStatusCode = httpStatusCode;
    }

    public Result()
    {
        IsSuccess = true;
        Error = null;
        HttpStatusCode = HttpStatusCode.OK;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }
    
    [JsonIgnore]
    public HttpStatusCode HttpStatusCode { get; }

    public static Result Success(HttpStatusCode httpStatusCode = HttpStatusCode.OK) => new(true, httpStatusCode, null);

    public static Result<TValue> Success<TValue>(TValue value, HttpStatusCode httpStatusCode = HttpStatusCode.OK) => new(value, true, httpStatusCode, null);

    public static Result Failure(HttpStatusCode httpStatusCode, Error error) => new(false, httpStatusCode, error);

    public static Result<TValue> Failure<TValue>(HttpStatusCode httpStatusCode, Error? error) => new(default, false, httpStatusCode, error);
    
    // public static Result<TValue> Failure<TValue>(Error? error) => new(default, false, HttpStatusCode.InternalServerError, error);

    public static Result<TValue> Failure<TValue>(TValue value, HttpStatusCode httpStatusCode, Error error) => new(value, false, httpStatusCode, error);

    // Default status code = InternalServerError if failure
    protected static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(HttpStatusCode.InternalServerError, null);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, HttpStatusCode httpStatusCode, Error? error) : base(isSuccess, httpStatusCode, error) =>
        _value = value;

    [JsonIgnore]
    public TValue? Value => IsSuccess
        ? _value!
        : default;

    // For API response compatibility
    public TValue? Data => IsSuccess
        ? _value!
        : default;

    [JsonIgnore]
    public TValue? FailureValue => IsFailure
        ? _value!
        : default;

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
    // public static implicit operator Result<TValue>(Error error) => Failure<TValue>(HttpStatusCode.InternalServerError, error);

    /// <summary>
    ///     Converts a Result to a Result&lt;TValue&gt; with the same error.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static Result<TValue> FromResult(Result result)
    {
        return Failure<TValue>(result.HttpStatusCode, result.Error);
    }
}