namespace Experiments.Models
{
    public class Experiment
    {
        public int ExperimentId { get; set; }
        public string Key { get; set; }
        public string Option { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
