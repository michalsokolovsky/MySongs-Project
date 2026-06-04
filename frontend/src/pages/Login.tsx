import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { songsApi } from "../api/songsApi";

export default function Login() {
  const [isRegister, setIsRegister] = useState(false);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [username, setUsername] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async () => {
    setError("");
    if (!email || !password) {
      setError("נא למלא אימייל וסיסמה");
      return;
    }
    if (isRegister && !username) {
      setError("נא למלא שם משתמש");
      return;
    }
    try {
      if (isRegister) {
        await songsApi.register(username, email, password);
        setIsRegister(false);
          setSuccess("נרשמת בהצלחה! כעת התחבר");
          setError("");
      } else {
        const data = await songsApi.login(email, password);
        login(data);
        navigate("/songs");
      }
    } catch (e: any) {
      setError(e.message);
    }
  };

  return (
    <div style={{ minHeight: "100vh", background: "#0F0A1E", display: "flex", alignItems: "center", justifyContent: "center", direction: "rtl" }}>
      <div className="card-glass" style={{ padding: "40px", width: "360px" }}>
        <h2 className="gradient-text" style={{ textAlign: "center", marginBottom: "24px" }}>
          🎵 {isRegister ? "הרשמה" : "התחברות"}
        </h2>
        {isRegister && (
          <input
            placeholder="שם משתמש"
            value={username}
            onChange={e => setUsername(e.target.value)}
            style={{ width: "100%", padding: "10px", marginBottom: "12px", borderRadius: "10px", border: "1px solid rgba(124,58,237,0.3)", background: "rgba(255,255,255,0.05)", color: "white", boxSizing: "border-box" }}
          />
        )}
        <input
          placeholder="אימייל"
          value={email}
          onChange={e => setEmail(e.target.value)}
          style={{ width: "100%", padding: "10px", marginBottom: "12px", borderRadius: "10px", border: "1px solid rgba(124,58,237,0.3)", background: "rgba(255,255,255,0.05)", color: "white", boxSizing: "border-box" }}
        />
        <input
          type="password"
          placeholder="סיסמה"
          value={password}
          onChange={e => setPassword(e.target.value)}
          style={{ width: "100%", padding: "10px", marginBottom: "16px", borderRadius: "10px", border: "1px solid rgba(124,58,237,0.3)", background: "rgba(255,255,255,0.05)", color: "white", boxSizing: "border-box" }}
        />
        {error && <p style={{ color: "#F87171", textAlign: "center", marginBottom: "12px" }}>{error}</p>}
        {success && <p style={{ color: "#34D399", textAlign: "center", marginBottom: "12px" }}>{success}</p>}

        <button className="btn-primary" style={{ width: "100%" }} onClick={handleSubmit}>
          {isRegister ? "הרשמה" : "התחברות"}
        </button>
        <p style={{ textAlign: "center", marginTop: "16px", color: "#A99DC7", cursor: "pointer" }} onClick={() => setIsRegister(!isRegister)}>
          {isRegister ? "כבר יש לך חשבון? התחבר" : "אין לך חשבון? הירשם"}
        </p>
      </div>
    </div>
  );
}