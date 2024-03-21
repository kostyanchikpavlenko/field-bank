namespace FieldBank.API.Common.Results
{
    /// <summary>
    /// Represents a generic result of an operation, including a value of type T along with success/failure status and error information.
    /// </summary>
    /// <typeparam name="T">The type of value associated with the result.</typeparam>
    public class Result<T> : Result
    {
        private readonly T _value;

        /// <summary>
        /// Gets the value associated with the successful result. Throws an exception for failure results.
        /// </summary>
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

        /// <summary>
        /// Initializes a new instance of the Result class with the specified value, success status, error information, and optional status code.
        /// </summary>
        /// <param name="value">The value associated with the result.</param>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="error">The error information associated with the result.</param>
        /// <param name="state">Optional status code/state of the result.</param>
        protected internal Result(T value, bool isSuccess, Error error, ResultState? state = null)
            : base(isSuccess, error, state)
        {
            _value = value;
        }

        /// <summary>
        /// Creates a successful Result instance with the specified value and success status code (ResultState.Ok).
        /// </summary>
        /// <param name="value">The value associated with the success.</param>
        /// <returns>A successful Result instance with the specified value.</returns>
        public static Result<T> Success(T value) => new(value, true, null, ResultState.Ok);

        /// <summary>
        /// Creates a failed Result instance with the specified error and status code (ResultState.NotFound).
        /// </summary>
        /// <param name="error">The error information associated with the failure.</param>
        /// <returns>A failed Result instance with the specified error.</returns>
        public static Result<T> NotFound(Error error) => new(default, false, error, ResultState.NotFound);

        /// <summary>
        /// Creates a failed Result instance with the specified error and status code (ResultState.UnprocessableEntity).
        /// </summary>
        /// <param name="error">The error information associated with the failure.</param>
        /// <returns>A failed Result instance with the specified error.</returns>
        public static new Result<T> UnprocessableEntity(Error error) => new(default, false, error, ResultState.UnprocessableEntity);

        /// <summary>
        /// Creates a failed Result instance with the specified error and status code (ResultState.Unauthorized).
        /// </summary>
        /// <param name="error">The error information associated with the failure.</param>
        /// <returns>A failed Result instance with the specified error.</returns>
        public static Result<T> Unauthorized(Error error) => new(default, false, error, ResultState.Unauthorized);

        /// <summary>
        /// Creates a failed Result instance with the specified error and status code (ResultState.BadRequest).
        /// </summary>
        /// <param name="error">The error information associated with the failure.</param>
        /// <returns>A failed Result instance with the specified error.</returns>
        public new static Result<T> Failure(Error error) => new(default, false, error, ResultState.BadRequest);

        /// <summary>
        /// Implicitly converts a value of type T to a Result instance with success status and the provided value.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <returns>A successful Result instance with the converted value.</returns>
        public static implicit operator Result<T>(T value) => Create(value);
    }
}
