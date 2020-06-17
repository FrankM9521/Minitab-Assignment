using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Minitab.Assignment.Api.Models
{
    /// <summary>
    /// Errors returned from server
    /// </summary>
    [Serializable]
    public class ErrorMessages
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }
    }
}
