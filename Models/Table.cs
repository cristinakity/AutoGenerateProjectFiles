namespace AutoGenerateProjectFiles.Console.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string Schema { get; set; } = string.Empty;
        public string OriginalName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Folders { get; set; } = string.Empty;
        public List<Column>? Columns { get; set; }
    }
}
