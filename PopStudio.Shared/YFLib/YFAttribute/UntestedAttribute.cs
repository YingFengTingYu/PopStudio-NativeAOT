namespace YFLib.YFAttribute
{
    /// <summary>
    /// Show the function is untested
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    internal class UntestedAttribute : Attribute
    {
        public UntestedAttribute(string name)
        {
            Console.WriteLine($"The method {name} is untested");
        }
    }
}