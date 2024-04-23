namespace Docs_Editor.Models
{
    public class MST_DocsModel
    {
        public int? DocID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
