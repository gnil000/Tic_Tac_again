using System.Text.Json.Serialization;

namespace Tic_Tac_again.Models
{
    public class Client
    {
        public string Name { get; set; }

        [JsonIgnore]
        public Client? Opponent { get; set; }
        //[JsonIgnore]
        //public bool isPlaying { get; set; }
        [JsonIgnore]
        public bool WaitMove { get; set; }
        //[JsonIgnore]
        public int Marker { get; private set; }// 0 - X | 1 - O

        public int ConnId { get; }

        public int isWin { get; private set; }//-1 - isDraw | 0 - lose | 1 - win | -2 - nothing
        public bool isPlaying { get; private set; }
        public bool isSearchGame { get; private set; } 

        public Client(string name)
        {
            Name = name;
            isPlaying = false;
            if (DataSource.GetInstance()._clients.Count==0)
                ConnId = DataSource.GetInstance()._clients.Count+1;
            else
                ConnId = DataSource.GetInstance()._clients.Max(x=> x.ConnId)+1;
            isWin = -2;
            isSearchGame = false;
        } 

        public void SetIsWin(int x)
        {
            isWin = x;
        }

        public void SetIsPlaying(bool x) { 
            isPlaying = x;
        }

        public void SetMarker(int x) {
            Marker = x;
        }

        public void SetSearchGame(bool x)
        {
            isSearchGame = x;
        }

    }
}
