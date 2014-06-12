using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using EveAI.Live;
using EveAI.Live.Account;
using EveAI.Live.Character;
using EveAI.Live.Corporation;
using System.Windows.Forms;
using System.Threading;
namespace NGA_Admin
{
    class playerMgm
    {
        
        public static AccountEntry charIDs = new AccountEntry();
        public static AuthenticationData auth = new AuthenticationData();
        public static EveApi api = new EveApi();

        public static string myDataBase = "";
        public static MySqlConnection conn = new MySqlConnection(myDataBase);

        public static void addRecruit(int id, string vcode)
        {
            List<AccountEntry> chars = new List<AccountEntry>();
            List<long> CharNum = new List<long>();
            conn.Open();
            MySqlCommand addApi = new MySqlCommand("INSERT INTO `workflow`.`recruits` (`id`, `vID`, `vCode`) VALUES (NULL, '"+ id +"', '"+ vcode +"');", conn);
            try
            {
                addApi.ExecuteNonQuery();
                conn.Dispose();
                for(int i = 0; i < 3; i++)
                {
                    auth.KeyID = id;
                    auth.VCode = vcode;
                    api.Authentication = auth;
                    APIKeyInfo datInfo = api.getApiKeyInfo();
                    chars.AddRange(datInfo.Characters);
                    charIDs.CharacterID = chars[i].CharacterID;
                    CharNum.Add(charIDs.CharacterID);
                }
                conn.Open();
                MySqlCommand addChars = new MySqlCommand("UPDATE `workflow`.`recruits` SET `char_id1` = '" + CharNum[0] + "', `char_id2` = '" + CharNum[1] + "', `char_id3` = '" + CharNum[2] + "' WHERE `recruits`.`vID` = " + id + ";", conn);
                addChars.ExecuteNonQuery();
                conn.Dispose();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static List<AccountEntry> getRecruits()
        {
            List<AccountEntry> recruits = new List<AccountEntry>();
            MySqlCommand getRecruit = new MySqlCommand("SELECT * FROM recruits order by id DESC", conn);
            conn.Open();
            MySqlDataReader rdr = getRecruit.ExecuteReader();
            while (rdr.Read())
            {
                auth.KeyID = int.Parse(rdr.GetString("vID"));
                auth.VCode = rdr.GetString("vCode");
                api.Authentication = auth;
                APIKeyInfo datInfo = api.getApiKeyInfo();
                recruits.AddRange(datInfo.Characters);
            }
            conn.Dispose();
            return recruits;
        }
        public static string getBasicInfo(AccountEntry character)
        {
            string dick;
            MySqlCommand getRecruit = new MySqlCommand("SELECT * FROM `recruits` WHERE `char_id1` LIKE '" + character.CharacterID + "' OR `char_id2` LIKE '" + character.CharacterID + "' OR `char_id3` LIKE '" + character.CharacterID + "'", conn);
            conn.Open();
            MySqlDataReader rdr = getRecruit.ExecuteReader();
            while (rdr.Read())
            {
                auth.KeyID = int.Parse(rdr.GetString("vID"));
                auth.VCode = rdr.GetString("vCode");
                auth.CharacterID = character.CharacterID;
                api.Authentication = auth;
                // MessageBox.Show(auth.KeyID.ToString() + " : " + auth.VCode + " : " + auth.CharacterID.ToString());
            }
            conn.Dispose();
            CharacterSheet charSheet = api.GetCharacterSheet();

            dick = "Character Name: " + charSheet.Name + "\nWallet Balance: " + String.Format("{0:N0}", charSheet.Balance) + "\n"
                + "Date of Birth: " + charSheet.DateOfBirth + "\nCorporation: " + charSheet.CorporationName + "\n"
                + "Skill Points: " + String.Format("{0:N0}", charSheet.SkillpointTotal) + "\n"
                + "Max Clone SP: " + String.Format("{0:N0}", charSheet.CloneSkillpoints) + "\n";

            //api.Authentication = auth;
            return dick;
        }
        public static List<Asset> retAssets()
        {
            List<Asset> SHIT = api.GetCharacterAssets();

            return SHIT;   
        }

    }
}
