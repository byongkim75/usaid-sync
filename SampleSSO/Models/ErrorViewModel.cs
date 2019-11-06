using System;

namespace SampleSSO.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class ValueModel
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }
}