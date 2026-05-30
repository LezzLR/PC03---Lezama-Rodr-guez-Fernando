using Microsoft.ML.Data;

namespace GestionTareasApi.Models
{
    public class SentimentPrediction : SentimentData
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
