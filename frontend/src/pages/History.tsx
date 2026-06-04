import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { RootState } from "../store/store";
import { Song } from "../store/songsSlice";
import { songsApi } from "../api/songsApi";
import { useAuth } from "../context/AuthContext";


interface HistoryItem {
  songId: number;
  listenDate: string;
}

interface Recommendation {
  songId: number;
  recommendedAt: string;
}

export default function History() {
  const songs = useSelector((state: RootState) => state.songs.songs);
  const [recommendations, setRecommendations] = useState<Recommendation[]>([]);
  const [history, setHistory] = useState<HistoryItem[]>([]);
  const [activeTab, setActiveTab] = useState("history");
  const [loading, setLoading] = useState(false);
  const { user } = useAuth();

  const getSongById = (id: number): Song | undefined => songs.find(s => s.songId === id);

  useEffect(() => {
    if (user?.userId) {
        songsApi.getHistory(parseInt(user.userId)).then(setHistory);
    }
  }, [user]);

  const handleGetRecommendations = async () => {
    setLoading(true);
const recs = await songsApi.getRecommendations(parseInt(user.userId));
    setRecommendations(recs);
    setLoading(false);
  };

  return (
    <div>
      <h2 className="gradient-text" style={{ marginBottom: "24px" }}>🎵 היסטוריה והמלצות</h2>

      <div style={{ display: "flex", gap: "12px", marginBottom: "24px" }}>
        <button onClick={() => setActiveTab("history")}
          style={{ padding: "10px 24px", borderRadius: "10px", border: "none", cursor: "pointer", fontWeight: "600",
            background: activeTab === "history" ? "linear-gradient(135deg, #7C3AED, #EC4899)" : "rgba(255,255,255,0.05)", color: "white" }}>
          🎧 היסטוריית האזנה
        </button>
        <button onClick={() => { setActiveTab("recommendations"); handleGetRecommendations(); }}
          style={{ padding: "10px 24px", borderRadius: "10px", border: "none", cursor: "pointer", fontWeight: "600",
            background: activeTab === "recommendations" ? "linear-gradient(135deg, #7C3AED, #EC4899)" : "rgba(255,255,255,0.05)", color: "white" }}>
          ⭐ המלצות עבורך
        </button>
      </div>

      {activeTab === "history" && (
        <div>
          {history.length === 0 ? (
            <p style={{ color: "#A99DC7" }}>אין היסטוריית האזנה עדיין 🎵</p>
          ) : (
            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(280px, 1fr))", gap: "16px" }}>
              {history.map((h, i) => {
                const song = getSongById(h.songId);
                return (
                  <div key={i} className="card-glass" style={{ padding: "20px" }}>
                    <h3 style={{ margin: "0 0 8px", color: "white" }}>{song?.title || "שיר לא ידוע"}</h3>
                    <p style={{ margin: "0 0 4px", color: "#A99DC7" }}>🎤 {song?.artistName}</p>
                    <p style={{ margin: "0 0 4px", color: "#A99DC7" }}>🎸 {song?.genre}</p>
                    {song?.audioUrl && (
                      <audio controls style={{ width: "100%", marginTop: "12px" }}>
                        <source src={song.audioUrl} type="audio/mpeg" />
                      </audio>
                    )}
                  </div>
                );
              })}
            </div>
          )}
        </div>
      )}

      {activeTab === "recommendations" && (
        <div>
          {loading ? (
            <p style={{ color: "#A99DC7" }}>מנתח העדפות וממליץ... ⏳</p>
          ) : recommendations.length === 0 ? (
            <p style={{ color: "#A99DC7" }}>
              {history.length === 0
                ? "האזן לכמה שירים קודם כדי לקבל המלצות! 🎵"
                : "כבר שמעת את כל השירים במאגר! המנהל יוסיף עוד בקרוב 🎶"}
            </p>
          ) : (
            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(280px, 1fr))", gap: "16px" }}>
              {recommendations.map((rec, i) => {
                const song = getSongById(rec.songId);
                return (
                  <div key={i} className="card-glass" style={{ padding: "20px" }}>
                    <h3 style={{ margin: "0 0 8px", color: "white" }}>{song?.title || "שיר לא ידוע"}</h3>
                    <p style={{ margin: "0 0 4px", color: "#A99DC7" }}>🎤 {song?.artistName}</p>
                    <p style={{ color: "#6B5A8E", fontSize: "13px" }}>
                      ⭐ הומלץ: {new Date(rec.recommendedAt).toLocaleDateString("he-IL")}
                    </p>
                    {song?.audioUrl && (
                      <audio controls style={{ width: "100%", marginTop: "12px" }}>
                        <source src={song.audioUrl} type="audio/mpeg" />
                      </audio>
                    )}
                  </div>
                );
              })}
            </div>
          )}
        </div>
      )}
    </div>
  );
}