import { useEffect, useState, useRef } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchSongs, deleteSongAsync, Song } from "../store/songsSlice";
import { RootState, AppDispatch } from "../store/store";
import { songsApi } from "../api/songsApi";
import { Plus, Trash2, Music, Upload, X, Pencil, Check } from "lucide-react";

export default function Admin() {
  const dispatch = useDispatch<AppDispatch>();
  const { songs, loading } = useSelector((state: RootState) => state.songs);
  const [saving, setSaving] = useState(false);
  const [showForm, setShowForm] = useState(false);
  const [audioFiles, setAudioFiles] = useState<File[]>([]);
  const [uploadProgress, setUploadProgress] = useState("");
  const [editingSong, setEditingSong] = useState<number | null>(null);
  const [editForm, setEditForm] = useState<{
    title: string;
    artistName: string;
    genre: string;
    lyricsSummary: string;
  }>({
    title: "",
    artistName: "",
    genre: "",
    lyricsSummary: ""
  });
  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    dispatch(fetchSongs());
  }, [dispatch]);

  const handleUpload = async (e: React.FormEvent) => {
    e.preventDefault();
    if (audioFiles.length === 0) return;
    setSaving(true);
    const results: string[] = [];
    for (let i = 0; i < audioFiles.length; i++) {
      const file = audioFiles[i];
      const fileName = file.name.replace(".mp3", "").replace(".wav", "").replace(".ogg", "");
      const parts = fileName.split("-");
      const artist = parts.length > 1 ? parts[0].trim() : fileName.trim();
      try {
        setUploadProgress(`מעלה שיר ${i + 1} מתוך ${audioFiles.length}: ${file.name}...`);
        await songsApi.uploadSong(file, artist);
        results.push(`✅ ${file.name} - נוסף בהצלחה`);
      } catch (e: any) {
        results.push(`⚠️ ${file.name} - ${e.message}`);
      }
    }
    setSaving(false);
    setUploadProgress(results.join("\n"));
    setAudioFiles([]);
    if (fileInputRef.current) fileInputRef.current.value = "";
    dispatch(fetchSongs());
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("למחוק את השיר?")) return;
    dispatch(deleteSongAsync(id));
  };

  const startEdit = (song: Song) => {
    setEditingSong(song.songId);
    setEditForm({
      title: song.title,
      artistName: song.artistName,
      genre: song.genre,
      lyricsSummary: song.lyricsSummary,
    });
  };

  const handleSaveEdit = async (id: number) => {
    await songsApi.updateSong(id, editForm);
    setEditingSong(null);
    dispatch(fetchSongs());
  };

  const inputStyle = {
    padding: "4px 8px", borderRadius: "8px",
    border: "1px solid rgba(124,58,237,0.4)",
    background: "rgba(15,10,30,0.8)", color: "white",
    outline: "none", fontSize: "13px", width: "100%"
  };

  return (
    <div style={{ direction: "rtl" }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
        <h1 className="gradient-text" style={{ fontSize: "24px", fontWeight: "bold", margin: 0 }}>
          ניהול מאגר שירים
        </h1>
        <button onClick={() => setShowForm(!showForm)} className="btn-primary" style={{ display: "flex", alignItems: "center", gap: "8px" }}>
          <Plus size={16} /> הוסף שירים
        </button>
      </div>

      {showForm && (
        <div className="card-glass" style={{ padding: "24px", marginBottom: "24px" }}>
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "16px" }}>
            <h2 style={{ margin: 0, color: "white" }}>הוספת שירים באמצעות AI</h2>
            <button onClick={() => setShowForm(false)} style={{ background: "none", border: "none", cursor: "pointer", color: "#A78BFA" }}>
              <X size={20} />
            </button>
          </div>
          <form onSubmit={handleUpload} style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
            <div>
              <label style={{ color: "#A99DC7", fontSize: "12px", display: "block", marginBottom: "6px" }}>
                קבצי שמע (אפשר לבחור כמה - שם הזמר יילקח משם הקובץ)
              </label>
              <input
                ref={fileInputRef}
                type="file"
                accept="audio/*"
                multiple
                onChange={e => setAudioFiles(Array.from(e.target.files || []))}
                required
                style={{ color: "white" }}
              />
              {audioFiles.length > 0 && (
                <p style={{ color: "#A99DC7", fontSize: "12px", marginTop: "8px" }}>
                  {audioFiles.length} קבצים נבחרו
                </p>
              )}
              {uploadProgress && (
                <p style={{ color: "#A78BFA", fontSize: "12px", marginTop: "8px", whiteSpace: "pre-line" }}>{uploadProgress}</p>
              )}
            </div>
            <div style={{ display: "flex", gap: "12px" }}>
              <button type="submit" disabled={saving} className="btn-primary" style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                <Upload size={16} /> {saving ? uploadProgress || "מעלה..." : "העלה ונתח"}
              </button>
              <button type="button" onClick={() => setShowForm(false)}
                style={{ padding: "10px 24px", borderRadius: "12px", border: "none", background: "rgba(124,58,237,0.15)", color: "#A99DC7", cursor: "pointer" }}>
                ביטול
              </button>
            </div>
          </form>
        </div>
      )}

      <div className="card-glass" style={{ overflow: "hidden" }}>
        <div style={{ padding: "16px", borderBottom: "1px solid rgba(124,58,237,0.2)" }}>
          <h2 style={{ margin: 0, color: "white" }}>שירים במאגר ({songs.length})</h2>
        </div>
        {loading ? (
          <div style={{ padding: "32px", textAlign: "center", color: "#6B5A8E" }}>טוען...</div>
        ) : songs.length === 0 ? (
          <div style={{ padding: "48px", textAlign: "center", color: "#6B5A8E" }}>
            <Music size={48} style={{ margin: "0 auto 16px", opacity: 0.3 }} />
            <p>אין שירים במאגר עדיין</p>
          </div>
        ) : (
          <table style={{ width: "100%", borderCollapse: "collapse" }}>
            <thead>
              <tr style={{ borderBottom: "1px solid rgba(124,58,237,0.2)" }}>
                {["שם השיר", "זמר", "ז'אנר", "תיאור", ""].map(h => (
                  <th key={h} style={{ padding: "12px 16px", textAlign: "right", fontSize: "12px", color: "#A99DC7", fontWeight: "500" }}>{h}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {songs.map((song, i) => (
                <tr key={song.songId} style={{ borderBottom: "1px solid rgba(124,58,237,0.1)", background: i % 2 === 0 ? "transparent" : "rgba(124,58,237,0.05)" }}>
                  {editingSong === song.songId ? (
                    <>
                      <td style={{ padding: "8px 16px" }}>
                        <input value={editForm.title} onChange={e => setEditForm(f => ({ ...f, title: e.target.value }))} style={inputStyle} />
                      </td>
                      <td style={{ padding: "8px 16px" }}>
                        <input value={editForm.artistName} onChange={e => setEditForm(f => ({ ...f, artistName: e.target.value }))} style={inputStyle} />
                      </td>
                      <td style={{ padding: "8px 16px" }}>
                        <input value={editForm.genre} onChange={e => setEditForm(f => ({ ...f, genre: e.target.value }))} style={inputStyle} />
                      </td>
                      <td style={{ padding: "8px 16px" }}>
                        <input value={editForm.lyricsSummary} onChange={e => setEditForm(f => ({ ...f, lyricsSummary: e.target.value }))} style={inputStyle} />
                      </td>
                      <td style={{ padding: "8px 16px", display: "flex", gap: "8px" }}>
                        <button onClick={() => handleSaveEdit(song.songId)} style={{ background: "none", border: "none", cursor: "pointer", color: "#34D399" }}>
                          <Check size={16} />
                        </button>
                        <button onClick={() => setEditingSong(null)} style={{ background: "none", border: "none", cursor: "pointer", color: "#6B5A8E" }}>
                          <X size={16} />
                        </button>
                      </td>
                    </>
                  ) : (
                    <>
                      <td style={{ padding: "12px 16px", color: "white", fontSize: "14px", fontWeight: "600" }}>{song.title}</td>
                      <td style={{ padding: "12px 16px", color: "#A99DC7", fontSize: "14px" }}>{song.artistName}</td>
                      <td style={{ padding: "12px 16px" }}>
                        <span style={{ fontSize: "11px", padding: "2px 8px", borderRadius: "20px", background: "rgba(124,58,237,0.25)", color: "#C4B5FD" }}>{song.genre}</span>
                      </td>
                      <td style={{ padding: "12px 16px", color: "#A99DC7", fontSize: "12px", maxWidth: "200px", overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}>{song.lyricsSummary}</td>
                      <td style={{ padding: "12px 16px" }}>
                        <div style={{ display: "flex", gap: "8px" }}>
                          <button onClick={() => startEdit(song)} style={{ background: "none", border: "none", cursor: "pointer", color: "#A78BFA", padding: "6px" }}>
                            <Pencil size={16} />
                          </button>
                          <button onClick={() => handleDelete(song.songId)} style={{ background: "none", border: "none", cursor: "pointer", color: "#F87171", padding: "6px" }}>
                            <Trash2 size={16} />
                          </button>
                        </div>
                      </td>
                    </>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}