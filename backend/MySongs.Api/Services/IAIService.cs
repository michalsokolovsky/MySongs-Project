using MySongs.Api.Services;

namespace MySongs.Api.Services
{
    public interface IAIService
    {
        Task<SongAnalysisResult> AnalyzeSongMetadata(string filePath);
    }
}
