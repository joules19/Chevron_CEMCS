using System;
using Throw;
using static System.Net.Mime.MediaTypeNames;
using static Chevron_CEMCS.DataAccess.PlayerModel;
using static Chevron_CEMCS.Models.ApiModels;

namespace Chevron_CEMCS.Helpers
{
    public class DataValidation
    {
        static List<string> listOfPositions = new List<string> { "defender", "forward", "midfielder" };
        static List<string> listOfSkills = new List<string> { "defense", "attack", "speed", "strength", "stamina" };

        public static string ValidateDataForNewPlayer(Player data)
        {
            List<string> listOfWordsToRemoveFromValidationMsgs = new List<string> { "data.", "skill.", "\n" };

            string message = string.Empty;

            try
            {
                data.name.ThrowIfNull().IfEmpty().IfWhiteSpace();
                data.position.ThrowIfNull().IfEmpty().IfWhiteSpace();
                data.playerSkills.Throw().IfCountLessThan(1);

                bool isPositionValid = listOfPositions.Contains(data.position);
                data.position.Throw("Position is not valid").IfFalse(isPositionValid);


                foreach (var skill in data.playerSkills)
                {
                    skill.value.Throw("Value should be between 0 and 100").IfLessThan(1).IfGreaterThan(100);
                    skill.skill.Throw().IfEmpty().IfWhiteSpace();

                    bool isSkillValid = listOfSkills.Contains(skill.skill);
                    skill.skill.Throw("Skill is not valid").IfFalse(isSkillValid);

                }
            }

            catch (Exception ex)
            {
                message = ex.Message;
            }

            // This line of code returns the error message without the data or skill variable keyword attacted
            // to every property upon error i.e. data.Email => Email skill.value => value
            return Methods.RemoveWord(message, listOfWordsToRemoveFromValidationMsgs);
        }


        public static string ValidateDataForTeamSelection(List<ProcessTeamVM> data)
        {
            List<string> listOfWordsToRemoveFromValidationMsgs = new List<string> { "data.", "searchParams.", "\n" };
            string message = string.Empty;

            try
            {
                foreach (var searchParams in data)
                {
                    bool isPositionValid = listOfPositions.Contains(searchParams.position);
                    searchParams.position.Throw("Position is not valid").IfFalse(isPositionValid);
                    searchParams.position.ThrowIfNull().IfEmpty().IfWhiteSpace();

                    bool isSkillValid = listOfSkills.Contains(searchParams.mainSkill);
                    searchParams.mainSkill.Throw("Skill is not valid").IfFalse(isSkillValid);
                    searchParams.mainSkill.ThrowIfNull().IfEmpty().IfWhiteSpace();

                    searchParams.numberOfPlayers.Throw("Value should be greater than 0").IfLessThan(1);
                }
            }

            catch (Exception ex)
            {
                message = ex.Message;
            }

            // This line of code returns the error message without the data or skill variable keyword attacted
            // to every property upon error i.e. data.Email => Email skill.value => value
            return Methods.RemoveWord(message, listOfWordsToRemoveFromValidationMsgs);
        }
    }
}

