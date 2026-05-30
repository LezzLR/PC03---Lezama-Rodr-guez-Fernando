using Microsoft.ML.Data;

namespace GestionTareasApi.Models
{
    public class SentimentData
    {
        [ColumnName("SentimentText"), LoadColumn(0)]
        public string SentimentText { get; set; } = string.Empty;

        [ColumnName("Label"), LoadColumn(1)]
        public bool Sentiment { get; set; }
    }
}
