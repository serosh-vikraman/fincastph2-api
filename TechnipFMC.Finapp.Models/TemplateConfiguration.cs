
namespace TechnipFMC.Finapp.Models
{
    public class TemplateConfiguration
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string ExcelCellPosition { get; set; }
        public int ScenarioDataTypeId { get; set; }
        public string Quarter { get; set; }
        public string Year { get; set; }
    }
}
