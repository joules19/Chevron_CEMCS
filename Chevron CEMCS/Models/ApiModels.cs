
using System;

namespace Chevron_CEMCS.Models
{
    public class ApiModels
    {
        public class PlayerSkill
        {
            public string skill { get; set; }
            public int value { get; set; }
        }
        public class PlayerVM
        {
            public string name { get; set; }
            public string position { get; set; }
            public List<PlayerSkill> playerSkills { get; set; }
        }


        public class ProcessTeamVM
        {
            public string position { get; set; }
            public string mainSkill { get; set; }
            public int numberOfPlayers { get; set; }
        }

    }
}

