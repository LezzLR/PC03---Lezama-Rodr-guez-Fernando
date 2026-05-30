using System;
using System.Collections.Generic;
using Microsoft.ML;
using GestionTareasApi.Models;

namespace GestionTareasApi.Services
{
    public class SentimentAnalysisService
    {
        private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

        public SentimentAnalysisService()
        {
            // 1. Inicializar MLContext
            var mlContext = new MLContext(seed: 1);

            // 2. Cargar datos de entrenamiento balanceados en español
            var trainingData = GetTrainingData();
            var trainDataView = mlContext.Data.LoadFromEnumerable(trainingData);

            // 3. Definir pipeline de entrenamiento:
            // - Featurizar el texto convirtiendo los caracteres en vectores numéricos
            // - Agregar el entrenador de clasificación binaria (Regresión Logística SDCA)
            var pipeline = mlContext.Transforms.Text.FeaturizeText(
                outputColumnName: "Features", 
                inputColumnName: nameof(SentimentData.SentimentText))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "Label", 
                    featureColumnName: "Features"));

            // 4. Entrenar el modelo
            var model = pipeline.Fit(trainDataView);

            // 5. Crear el motor de predicción reutilizable
            _predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
        }

        /// <summary>
        /// Clasifica el comentario provisto como "Positivo" o "Negativo".
        /// </summary>
        public string PredictSentiment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return "Negativo"; // Valor por defecto o manejo de bordes
            }

            var input = new SentimentData { SentimentText = comment };
            var result = _predictionEngine.Predict(input);

            // True = Positivo, False = Negativo
            return result.Prediction ? "Positivo" : "Negativo";
        }

        private List<SentimentData> GetTrainingData()
        {
            return new List<SentimentData>
            {
                // Frases Positivas
                new SentimentData { SentimentText = "La tarea fue completada correctamente y el sistema funciona bien", Sentiment = true },
                new SentimentData { SentimentText = "Excelente trabajo de todo el equipo", Sentiment = true },
                new SentimentData { SentimentText = "Me encanta cómo funciona, es súper rápido y fácil", Sentiment = true },
                new SentimentData { SentimentText = "El proyecto se entregó a tiempo y el resultado es excelente", Sentiment = true },
                new SentimentData { SentimentText = "Todo marcha de maravilla, muy buen desempeño", Sentiment = true },
                new SentimentData { SentimentText = "Muy contento con esta nueva herramienta", Sentiment = true },
                new SentimentData { SentimentText = "La interfaz es hermosa y muy intuitiva", Sentiment = true },
                new SentimentData { SentimentText = "Espectacular software, ahorra muchísimo tiempo", Sentiment = true },
                new SentimentData { SentimentText = "Gran soporte técnico y todo funcionando perfecto", Sentiment = true },
                new SentimentData { SentimentText = "La solución cumple perfectamente con las expectativas", Sentiment = true },
                new SentimentData { SentimentText = "Me gusta mucho el diseño, es ágil y moderno", Sentiment = true },
                new SentimentData { SentimentText = "Funciona de manera excelente y fluida", Sentiment = true },

                // Frases Negativas
                new SentimentData { SentimentText = "Pésimo sistema, tiene demasiados fallos", Sentiment = false },
                new SentimentData { SentimentText = "No funciona la aplicación, se cierra a cada rato", Sentiment = false },
                new SentimentData { SentimentText = "Muy insatisfecho con el rendimiento, va lentísimo", Sentiment = false },
                new SentimentData { SentimentText = "Es una pérdida de tiempo, no sirve para nada", Sentiment = false },
                new SentimentData { SentimentText = "Tiene errores graves y no me deja guardar nada", Sentiment = false },
                new SentimentData { SentimentText = "Horrible experiencia, la interfaz es confusa y fea", Sentiment = false },
                new SentimentData { SentimentText = "No funciona correctamente y el soporte es malo", Sentiment = false },
                new SentimentData { SentimentText = "Muchos bugs en producción, inservible", Sentiment = false },
                new SentimentData { SentimentText = "El programa falla constantemente al crear tareas", Sentiment = false },
                new SentimentData { SentimentText = "No lo recomiendo para nada, muy mala calidad", Sentiment = false },
                new SentimentData { SentimentText = "El sistema da error en cada endpoint, es un desastre", Sentiment = false },
                new SentimentData { SentimentText = "Falta de optimización y demasiados tiempos de espera", Sentiment = false }
            };
        }
    }
}
