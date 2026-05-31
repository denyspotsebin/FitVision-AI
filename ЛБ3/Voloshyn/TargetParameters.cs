using System;

namespace FitVisionAI.Services
{
    public class TargetParameters
    {
        private const float MinWeight = 30f;
        private const float MaxWeight = 250f;
        private const float MinFat = 3f;
        private const float MaxFat = 50f;

        public float DesiredWeight { get; set; }
        public float BodyFatPercentage { get; set; }

        public bool ValidateData()
        {
            if (DesiredWeight < MinWeight || DesiredWeight > MaxWeight)
            {
                return false;
            }

            if (BodyFatPercentage < MinFat || BodyFatPercentage > MaxFat)
            {
                return false;
            }

            return true;
        }
    }
}