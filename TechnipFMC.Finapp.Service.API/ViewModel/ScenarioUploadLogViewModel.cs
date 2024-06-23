using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ScenarioUploadLogViewModel
    {
		public int ScenarioID { get; set; }
		public Guid UploadSessionID { get; set; }
		public string ProjectCode { get; set; }
		public int RowNumber { get; set; }		
		public bool UploadStatus { get; set; }
		public string UploadDescription { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public string ErrorCode { get; set; }
		public int ColumnNumber { get; set; }
	}
}