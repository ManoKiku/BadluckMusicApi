﻿namespace BadluckMusicApi.Models.DB
{
    public class MusicHobby
    {
        public int Id { get; set; }
        public int HobbyId { get; set; }
        public int MusicId { get; set; }
         
        public Hobby Hobby {  get; set; } 
        public Music Music { get; set; }
    }
}
