namespace AutoGenerateProjectFiles.Console.Models
{
    public class Column
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsPrimaryKey { get; set; }
        public bool IsNull { get; set; }
        public int MaxLength { get; set; }
    }
}
