using System;

namespace XpTestBuilder.Common
{
    public class JobInfo
    {
        public Guid JobID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedFrom { get; set; }
        public CommandData Request { get; set; }

        public JobInfo()
        {
            JobID = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }
    }
}