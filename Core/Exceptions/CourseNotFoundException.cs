namespace Core.Exceptions;

public class CourseNotFoundException : Exception
{
    public CourseNotFoundException() : base() {}
    public CourseNotFoundException(string message) : base(message) {}
    public CourseNotFoundException(string message, Exception exception) : base(message, exception) {}
}