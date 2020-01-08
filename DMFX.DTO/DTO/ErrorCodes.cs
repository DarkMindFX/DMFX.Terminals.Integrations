using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Interfaces
{
    public enum EErrorType
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
    public enum EErrorCodes
    {
        Success = 0,
        InvalidParserParams = 1,
        FileNotFound = 2,
        ParserError = 3,
        ImporterError = 4,
        InvalidSourceParams = 5,
        SubmissionNotFound = 6,
        UserAccountNotFound = 7,
        UserAccountNotValidated = 8,
        UserAccountExists = 9,
        InvalidSession = 10,
        EmptyCollection = 11,
        SchedulerBusy = 12,
        ImporterBusy = 13,
        FilingNotFound = 14,
        SessionClosed = 15,
        MailSendFailed = 16,

        QuotesSourceFail = 101,
        QuotesNotFound = 102,
        TickerNotFound = 103,
        TickerProcessingFail = 104,

        SchedulerJobsNotFound = 201,

        AlertsSourceFail = 301,

        MQDbError = 401,
        MQCommunicationError = 402,


        GeneralError = 99999
    }
}
