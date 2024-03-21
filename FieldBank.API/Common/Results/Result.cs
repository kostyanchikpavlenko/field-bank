namespace FieldBank.API.Common.Results
{
    /// <summary>
    /// Represents the result of an operation, including success/failure status and error information.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the Result class with the specified success status, error information, and optional status code.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="error">The error information associated with the result.</param>
        /// <param name="statusCode">Optional status code/state of the result.</param>
        protected Result(bool isSuccess, Error error, ResultState? statusCode = null)
        {
            IsSuccess = isSuccess;
            Error = error;
            State = statusCode;
        }

        /// <summary>
        /// Gets the optional state/status code of the result.
        /// </summary>
        public ResultState? State { get; }

        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the operation failed.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the error information associated with the result.
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Creates a successful Result instance with no error and a success status code (ResultState.Ok).
        /// </summary>
        /// <returns>A successful Result instance.</returns>
        public static Result Success() => new(true, Error.None, ResultState.Ok);

        /// <summary>
        /// Creates a successful Result instance with the specified value and no error.
        /// </summary>
        /// <typeparam name="TValue">The type of value associated with the success.</typeparam>
        /// <param name="value">The value associated with the success.</param>
        /// <returns>A successful Result instance with the specified value.</returns>
        private static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        /// <summary>
        /// Creates a failed Result instance with the specified error and status code (ResultState.UnprocessableEntity).
        /// </summary>
        /// <param name="error">The error information associated with the failure.</param>
        /// <returns>A failed Result instance with the specified error.</returns>
        public static Result UnprocessableEntity(Error error) => new(false, error, ResultState.UnprocessableEntity);

        /// <summary>
        /// Creates a failed Result instance with the specified error and status code (ResultState.BadRequest).
        /// </summary>
        /// <param name="error">The error information associated with the failure.</param>
        /// <returns>A failed Result instance with the specified error.</returns>
        public static Result Failure(Error error) => new(false, error, ResultState.BadRequest);

        /// <summary>
        /// Creates a failed Result instance of a specified generic type with the specified error.
        /// </summary>
        /// <typeparam name="TValue">The type of value associated with the failure.</typeparam>
        /// <param name="error">The error information associated with the failure.</param>
        /// <returns>A failed Result instance with the specified error.</returns>
        private static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        /// <summary>
        /// Creates a Result instance of a specified generic type based on whether the provided value is not null.
        /// </summary>
        /// <typeparam name="TValue">The type of value to create a Result for.</typeparam>
        /// <param name="value">The value to create a Result for.</param>
        /// <returns>A Result instance representing success or failure based on the value provided.</returns>
        protected static Result<TValue> Create<TValue>(TValue value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }
}
