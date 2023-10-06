using Experiments.Models;

namespace Experiments.Service
{
    public static class ExperimentCalculator
    {
        /// <summary>
        /// This method determines the result of an experiment for button color selection.
        /// Generates a random index to choose an option from the list of options.
        /// </summary>
        /// <param name="options">The list of options to choose from.</param>
        /// <returns>The selected button color option.</returns>
        public static Option CalculateButtonColorExperimentResult(List<Option> options)
        {
            Random random = new Random();
            int r = random.Next(0, options.Count);

            return options[r];
        }

        /// <summary>
        /// This method determines the result of an experiment related to pricing.
        /// Generates a random number and determines the selected price option accordingly depending of probability.
        /// </summary>
        /// <param name="options">The list of options to choose from.</param>
        /// <returns>The selected price option based on the experiment result.</returns>
        public static Option CalculatePriceExperimentResult(List<Option> options)
        {
            Random rand = new Random();
            int randomNumber = rand.Next(1, 101);
            if (randomNumber <= 75)
            {
                return options[0];
            }
            else if (randomNumber <= 85)
            {
                return options[1];
            }
            else if (randomNumber <= 90)
            {
                return options[2];
            }
            else
            {
                return options[3];
            }
        }

    }
}
