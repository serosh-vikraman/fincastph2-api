using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ScenarioUploadLog
	{
		public int ScenarioID { get; set; }
		public Guid UploadSessionID { get; set; }
		public string ProjectCode { get; set; }
		public int RowNumber { get; set; }
		public bool UploadStatus { get; set; }
		public string UploadDescription { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public string ErrorCode { get; set; }
		public int ColumnNumber { get; set; }
	}

	public class ProjectUploadLog
	{
	 
		public string SessionID { get; set; }		 
		public int RowNumber { get; set; }
		public int ColumnNumber { get; set; }
		public bool Status { get; set; }
		public string Message { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public string ErrorCode { get; set; }
		
	}
}
