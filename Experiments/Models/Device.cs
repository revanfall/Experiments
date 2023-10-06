namespace Experiments.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public Experiment Experiment { get; set; }
    }
}
