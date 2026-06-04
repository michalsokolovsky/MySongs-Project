import { Outlet, NavLink, useNavigate } from "react-router-dom";
import { Music, History, LogOut } from "lucide-react";
import { useAuth } from "../context/AuthContext";

export default function Layout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  return (
    <div style={{ minHeight: "100vh", background: "#0F0A1E", color: "white", direction: "rtl" }}>
      <style>{`
        body { margin: 0; font-family: 'Segoe UI', sans-serif; }
        .gradient-text {
          background: linear-gradient(90deg, #A78BFA, #EC4899);
          -webkit-background-clip: text;
          -webkit-text-fill-color: transparent;
        }
        .card-glass {
          background: rgba(26,16,53,0.85);
          border: 1px solid rgba(124,58,237,0.25);
          border-radius: 16px;
          backdrop-filter: blur(12px);
        }
        .btn-primary {
          background: linear-gradient(135deg, #7C3AED, #EC4899);
          color: white;
          border: none;
          border-radius: 12px;
          padding: 10px 24px;
          font-weight: 600;
          cursor: pointer;
          transition: opacity 0.2s, transform 0.1s;
        }
        .btn-primary:hover { opacity: 0.9; transform: translateY(-1px); }
        .nav-link { color: #A99DC7; text-decoration: none; padding: 8px 16px; border-radius: 10px; display: flex; align-items: center; gap: 8px; transition: all 0.2s; }
        .nav-link:hover { background: rgba(124,58,237,0.2); color: white; }
        .nav-link.active { background: rgba(124,58,237,0.3); color: white; }
      `}</style>
      <header style={{ position: "fixed", top: 0, left: 0, right: 0, zIndex: 100, background: "rgba(15,10,30,0.95)", borderBottom: "1px solid rgba(124,58,237,0.2)", padding: "0 24px", height: "64px", display: "flex", alignItems: "center", justifyContent: "space-between" }}>
        <h1 className="gradient-text" style={{ margin: 0, fontSize: "20px", fontWeight: "bold" }}>🎵 מאגר שירים</h1>
        <nav style={{ display: "flex", gap: "8px", alignItems: "center" }}>
  <NavLink to="/songs" className={({ isActive }) => "nav-link" + (isActive ? " active" : "")}>
    <Music size={16} /> שירים
  </NavLink>
  {user?.role !== "Admin" && (
  <NavLink to="/history" className={({ isActive }) => "nav-link" + (isActive ? " active" : "")}>
    <History size={16} /> היסטוריה
  </NavLink>
)}
  {user?.role === "Admin" && (
    <NavLink to="/admin" className={({ isActive }) => "nav-link" + (isActive ? " active" : "")}>
      🎛️ ניהול
    </NavLink>
  )}
  <span style={{ color: "#6B5A8E", fontSize: "13px", marginRight: "8px" }}>שלום, {user?.username}</span>
  <button onClick={handleLogout} style={{ background: "none", border: "none", cursor: "pointer", color: "#F87171", display: "flex", alignItems: "center", gap: "4px", fontSize: "13px" }}>
    <LogOut size={16} /> יציאה
  </button>
</nav>
      </header>
      <main style={{ padding: "80px 24px 24px", maxWidth: "1200px", margin: "0 auto" }}>
        <Outlet />
      </main>
    </div>
  );
}