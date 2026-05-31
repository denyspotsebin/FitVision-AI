namespace FitVisionAI.Services
{
    public class BasePhoto
    {
        public bool IsQualityGood { get; set; }

        public bool AnalyzeLighting()
        {
            return IsQualityGood;
        }
    }
}