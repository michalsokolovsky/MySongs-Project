using MySongs.Common.DTOs;

namespace MySongs.Services.Interfaces
{
    public interface IAIAnalysisService
    {
        Task<AIAnalysisResult> AnalyzeSongAsync(SongDto song);
        Task<AIAnalysisResult> AnalyzeSongWithLyricsAsync(SongDto song, string lyrics);
    }
}