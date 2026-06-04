import { useEffect, useState, useRef } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchSongs } from "../store/songsSlice";
import { RootState, AppDispatch } from "../store/store";
import { songsApi } from "../api/songsApi";
import { useAuth } from "../context/AuthContext";
import { Search } from "lucide-react";

export default function Songs() {
  const dispatch = useDispatch<AppDispatch>();
  const { songs, loading } = useSelector((state: RootState) => state.songs);
  const [filtered, setFiltered] = useState(songs);
  const [query, setQuery] = useState("");
  const { user } = useAuth();
  const listenedRef = useRef(new Set<number>());

  useEffect(() => {
    dispatch(fetchSongs());
  }, [dispatch]);

  useEffect(() => {
    setFiltered(songs);
  }, [songs]);

  useEffect(() => {
    if (user?.userId) {
      songsApi.getHistory(parseInt(user.userId)).then(history => {

        history.forEach((h: any) => listenedRef.current.add(h.songId));
      });
    }
  }, [user]);

  const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    const q = e.target.value;
    setQuery(q);
    if (!q.trim()) {
      setFiltered(songs);
      return;
    }
    const lower = q.toLowerCase();
    setFiltered(songs.filter(s =>
      s.title?.toLowerCase().includes(lower) ||
      s.artistName?.toLowerCase().includes(lower) ||
      s.genre?.toLowerCase().includes(lower) ||
      s.lyricsSummary?.toLowerCase().includes(lower)
    ));
  };

  const handlePlay = async (songId: number) => {
  if (!listenedRef.current.has(songId) && user?.userId) {
    listenedRef.current.add(songId);
    try {      await songsApi.addHistory(songId, parseInt(user.userId), 0);    } catch (e) {
      console.error("שגיאה ברישום האזנה", e);
    }
  }
};


  if (loading) return <p style={{ color: "white", textAlign: "center" }}>טוען שירים...</p>;

  return (
    <div>
      <h2 className="gradient-text" style={{ marginBottom: "24px" }}>🎵 כל השירים</h2>

      <div style={{ position: "relative", marginBottom: "24px" }}>
        <Search size={16} style={{ position: "absolute", right: "12px", top: "50%", transform: "translateY(-50%)", color: "#A99DC7" }} />
        <input
          type="text"
          value={query}
          onChange={handleSearch}
          placeholder="חפש לפי שם שיר, זמר, ז'אנר או מילות שיר..."
          style={{
            width: "100%",
            padding: "12px 40px 12px 16px",
            borderRadius: "12px",
            border: "1px solid rgba(124,58,237,0.4)",
            background: "rgba(15,10,30,0.8)",
            color: "white",
            fontSize: "14px",
            outline: "none",
            direction: "rtl",
            boxSizing: "border-box"
          }}
        />
      </div>

      {filtered.length === 0 && (
        <p style={{ color: "#6B5A8E", textAlign: "center" }}>לא נמצאו שירים תואמים</p>
      )}

      <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(280px, 1fr))", gap: "16px" }}>
        {filtered.map(song => (
          <div key={song.songId} className="card-glass" style={{ padding: "20px" }}>
            <h3 style={{ margin: "0 0 8px", color: "white" }}>{song.title}</h3>
            <p style={{ margin: "0 0 4px", color: "#A99DC7" }}>🎤 {song.artistName}</p>
            <p style={{ margin: "0 0 4px", color: "#A99DC7" }}>🎸 {song.genre}</p>
            {song.lyricsSummary && <p style={{ margin: "8px 0 0", color: "#6B5A8E", fontSize: "13px" }}>{song.lyricsSummary}</p>}
            {song.audioUrl && (
              <audio
                controls
                style={{ width: "100%", marginTop: "12px", borderRadius: "8px", filter: "invert(1) hue-rotate(180deg)" }}
                onPlay={() => {
                    handlePlay(song.songId);
                   }}
              >
                <source src={song.audioUrl} type="audio/mpeg" />
              </audio>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}