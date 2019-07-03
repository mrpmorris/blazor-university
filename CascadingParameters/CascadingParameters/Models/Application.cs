using System;

namespace CascadingParameters.Models
{
    public class Application
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public Candidate Candidate { get; set; }
    }
}
