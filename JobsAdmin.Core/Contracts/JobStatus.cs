﻿using System.ComponentModel;

namespace JobsAdmin.Core.Contracts
{
    public enum JobStatus: int
    {
        [Description("In queued")]
        InQueued,

        [Description("In progress")]
        InProgress,

        [Description("Finished")]
        Finished,

        [Description("Ready to remove")]
        ReadyToRemove,

        [Description("Scheduled")]
        Scheduled
    }
}
