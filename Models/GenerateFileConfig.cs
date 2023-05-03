namespace AutoGenerateProjectFiles.Console.Models
{
    public class GenerateFileConfig
    {
        public string Template { get; set; }
        public string Section { get; set; }
        public string SolutionName { get; set; }
        public string RootPath { get; set; }
        public List<Table> Tables { get; set; }
    }
}
