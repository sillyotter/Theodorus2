using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    public class StatusReport
    {
        public const string Channel = "Progress";

        #region Constructor

        public StatusReport(string message, int? progress, ExecutionStatus? currentStatus)
        {
            Message = message;
            Progress = progress;
            CurrentStatus = currentStatus;
        }

        #endregion

        #region Properties

        public string Message { get; private set; }
        public int? Progress { get; private set; }
        public ExecutionStatus? CurrentStatus { get; private set; }

        #endregion

    }
}