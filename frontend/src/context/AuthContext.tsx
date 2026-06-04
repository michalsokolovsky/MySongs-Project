import { createContext, useContext, useState, useEffect, ReactNode } from "react";

interface User {
  token: string;
  username: string;
  userId: string;
  role: string;
}

interface AuthContextType {
  user: User | null;
  login: (data: any) => void;
  logout: () => void;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = sessionStorage.getItem("token");
    const username = sessionStorage.getItem("username");
    const userId = sessionStorage.getItem("userId");
    const role = sessionStorage.getItem("role");
    if (token && username && userId) {
      setUser({ token, username, userId, role: role || "User" });
    }
    setLoading(false);
  }, []);

  const login = (data: any) => {
    sessionStorage.setItem("token", data.token);
    sessionStorage.setItem("username", data.userName || data.username || "");
    sessionStorage.setItem("userId", String(data.userId));
    sessionStorage.setItem("role", data.role || "User");
    setUser({
      token: data.token,
      username: data.userName || data.username || "",
      userId: String(data.userId),
      role: data.role || "User"
    });
  };

  const logout = () => {
    sessionStorage.clear();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = (): AuthContextType => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
};

