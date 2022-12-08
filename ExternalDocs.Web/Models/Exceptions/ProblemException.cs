namespace ExternalDocs.Web.Models.Exceptions
{
    [Serializable]
    internal class ProblemException : Exception
    {
        public ProblemDetails Problem { get; }

        public ProblemException(ProblemDetails problem)
        {
            Problem = problem;
        }
    }
}
