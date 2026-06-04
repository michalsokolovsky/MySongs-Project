import axios from "axios";

const BASE_URL = "https://localhost:7147/api";
const getToken = () => sessionStorage.getItem("token");

const authHeaders = () => ({
  "Content-Type": "application/json",
  Authorization: `Bearer ${getToken()}`,
});

export interface SongDto {
  songId: number;
  title: string;
  artistName: string;
  genre: string;
  lyricsSummary: string;
  audioUrl: string;
  fileHash: string;
}

export interface UserDto {
  userId: number;
  username: string;
  email: string;
  role: string;
  token: string;
}

export interface HistoryItem {
  songId: number;
  listenDate: string;
}

export interface Recommendation {
  songId: number;
  recommendedAt: string;
}

export const songsApi = {
  login: async (email: string, password: string): Promise<UserDto> => {
    try {
      const res = await axios.post(`${BASE_URL}/Auth/login`, {
        userId: 0,
        username: "",
        email,
        password,
        createdAt: new Date().toISOString()
      });
      return res.data;
    } catch (error: any) {
      const msg = error.response?.data || "אימייל או סיסמה שגויים";
      throw new Error(msg);
    }
  },

 register: async (username: string, email: string, password: string): Promise<string> => {
    try {
      const res = await axios.post(`${BASE_URL}/Auth/register`, { username, email, password });
      return res.data;
    } catch (error: any) {
      const msg = error.response?.data || "שגיאה בהרשמה";
      throw new Error(msg);
    }
  },

  getAllSongs: async (): Promise<SongDto[]> => {
    const res = await axios.get(`${BASE_URL}/Songs`, { headers: authHeaders() });
    return res.data;
  },

  uploadSong: async (audioFile: File, artistName: string): Promise<SongDto> => {
    const formData = new FormData();
    formData.append("audioFile", audioFile);
    formData.append("artistName", artistName);
    try {
      const res = await axios.post(`${BASE_URL}/Songs/upload`, formData, {
        headers: { Authorization: `Bearer ${getToken()}` },
      });
      return res.data;
    } catch (error: any) {
      const msg = error.response?.data || "שגיאה בהעלאה";
      throw new Error(msg);
    }
  },

  deleteSong: async (id: number): Promise<void> => {
    await axios.delete(`${BASE_URL}/Songs/${id}`, { headers: authHeaders() });
  },

  updateSong: async (id: number, fields: Partial<SongDto>): Promise<boolean> => {
    await axios.put(`${BASE_URL}/Songs/${id}`, fields, { headers: authHeaders() });
    return true;
  },

  addHistory: async (songId: number, userId: number, duration: number): Promise<void> => {
    await axios.post(`${BASE_URL}/ListeningHistory`, 
      { songId, userId: parseInt(userId.toString()), duration },
      { headers: authHeaders() }
    );
  },

  getHistory: async (userId: number): Promise<HistoryItem[]> => {
    const res = await axios.get(`${BASE_URL}/ListeningHistory/user/${userId}`, { headers: authHeaders() });
    return res.data;
  },

  getRecommendations: async (userId: number): Promise<Recommendation[]> => {
    await axios.post(`${BASE_URL}/Recommendations/generate/${userId}`, {}, { headers: authHeaders() });
    const res = await axios.get(`${BASE_URL}/Recommendations/user/${userId}`, { headers: authHeaders() });
    return res.data;
  },
};