namespace Nexas.Inventory.Application.Base.Entity
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }
        public string ErrorCode { get; private set; }
        public Dictionary<string, string[]> ValidationErrors { get; private set; }
        public int StatusCode { get; private set; }

        private Result(bool success, string message, T data, string errorCode, Dictionary<string, string[]> validationErrors, int statusCode)
        {
            Success = success;
            Message = message;
            Data = data;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
            StatusCode = statusCode;
        }

        // ✅ Success with data
        public static Result<T> Ok(T data, string message = "Operation completed successfully.")
            => new(true, message, data, null, null, 200);

        // ✅ Success without data
        public static Result<T> Ok(string message = "Operation completed successfully.")
            => new(true, message, default, null, null, 200);

        // ❌ Generic failure
        public static Result<T> Fail(string message, string errorCode = null, int statusCode = 400)
            => new(false, message, default, errorCode, null, statusCode);

        // ❌ Validation error (422)
        public static Result<T> ValidationError(Dictionary<string, string[]> errors, string message = "Validation error.")
            => new(false, message, default, "VALIDATION_ERROR", errors, 422);

        // ❌ Not found (404)
        public static Result<T> NotFound(string message = "Resource not found.")
            => new(false, message, default, "NOT_FOUND", null, 404);

        // ❌ Unauthorized (401)
        public static Result<T> Unauthorized(string message = "Unauthorized.")
            => new(false, message, default, "UNAUTHORIZED", null, 401);

        // ❌ Forbidden (403)
        public static Result<T> Forbidden(string message = "Access denied.")
            => new(false, message, default, "FORBIDDEN", null, 403);

        // ❌ Internal server error (500)
        public static Result<T> ServerError(string message = "Internal server error.")
            => new(false, message, default, "SERVER_ERROR", null, 500);
    }
}
