import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit";
import { songsApi } from "../api/songsApi";

export interface Song {
  songId: number;
  title: string;
  artistName: string;
  genre: string;
  lyricsSummary: string;
  audioUrl: string;
  fileHash: string;
}

interface SongsState {
  songs: Song[];
  loading: boolean;
  error: string | null;
}

const initialState: SongsState = {
  songs: [],
  loading: false,
  error: null,
};

export const fetchSongs = createAsyncThunk("songs/fetchAll", async () => {
  return await songsApi.getAllSongs();
});

export const deleteSongAsync = createAsyncThunk("songs/delete", async (id: number) => {
  await songsApi.deleteSong(id);
  return id;
});

const songsSlice = createSlice({
  name: "songs",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchSongs.pending, (state) => { state.loading = true; })
      .addCase(fetchSongs.fulfilled, (state, action: PayloadAction<Song[]>) => {
        state.loading = false;
        state.songs = action.payload;
      })
      .addCase(fetchSongs.rejected, (state) => {
        state.loading = false;
        state.error = "שגיאה בטעינת שירים";
      })
      .addCase(deleteSongAsync.fulfilled, (state, action: PayloadAction<number>) => {
        state.songs = state.songs.filter(s => s.songId !== action.payload);
      });
  },
});

export default songsSlice.reducer;
