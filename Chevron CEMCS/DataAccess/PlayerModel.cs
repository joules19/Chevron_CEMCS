using System;

namespace Chevron_CEMCS.DataAccess
{
    public class PlayerModel
    {
        public class PlayerSkill
        {
            public int id { get; set; }
            public string skill { get; set; }
            public int value { get; set; }
            public int playerId { get; set; }
        }

        public class Player
        {
            public int id { get; set; }
            public string name { get; set; }
            public string position { get; set; }
            public List<PlayerSkill> playerSkills { get; set; }
        }
    }
}

