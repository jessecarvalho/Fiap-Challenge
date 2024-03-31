namespace Core.Exceptions;

public class CourseNotUniqueException : Exception
{
    public CourseNotUniqueException() : base() {}
    public CourseNotUniqueException(string message) : base(message) {}
    public CourseNotUniqueException(string message, Exception exception) : base(message, exception) {}
}