using System.Text.Json.Serialization;

namespace Tic_Tac_again.Models
{
    public class Client
    {
        public string Name { get; set; }

        [JsonIgnore]
        public Client? Opponent { get; set; }
        [JsonIgnore]
        public bool isPlaying { get; set; }
        [JsonIgnore]
        public bool WaitMove { get; set; }
        [JsonIgnore]
        public int Marker { get; set; }// 0 - X | 1 - O

        public int ConnId { get; }

        public int isWin { get; private set; }//-1 - isDraw | 0 - lose | 1 - win | -2 - nothing

        public Client(string name)
        {
            Name = name;
            isPlaying = false;
            ConnId = DataSource.GetInstance()._clients.Count+1;
            isWin = -2;
        } 

        public void SetIsWin(int x)
        {
            isWin = x;
        }

    }
}
