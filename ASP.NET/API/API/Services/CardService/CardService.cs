using API.Models;
using API.Models.Dtos.Card;
using AutoMapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace API.Services.CardService
{
    public class CardService : ICardService
    {
        private readonly IMapper _mapper;
        public CardService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ServiceResponse<string>> AddCard(AddCardDto newCard)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            // using var connection = new MySqlConnection("server=localhost;port=3306;database=cred_check;user=root;password=password");
            //using var connection = new MySqlConnection("server=cred-check.csoyvlm0mivw.us-east-2.rds.amazonaws.com;port=3306;database=credcheck;user=insertuser;");    // was server=host.docker.internal
           // var pass = "";
            using var connection = new MySqlConnection("server=cred-check.csoyvlm0mivw.us-east-2.rds.amazonaws.com;port=3306;database=credcheck;user=insertuser;");
            //using var connection = new MySqlConnection("server=cred-check.csoyvlm0mivw.us-east-2.rds.amazonaws.com;port=3306;database=credcheck;user=admin;password=" + pass);
            await connection.OpenAsync();
            var newcmd = "INSERT INTO `credcheck`.`cards` (`cardNumber`, `expirationDate`, `cvv`, `cardId`) VALUES ('" + newCard.cardNumber + "', '" + newCard.expirationDate + "', '" + newCard.cvv + "', '" + HASHALG(_mapper.Map<card>(newCard)) + "');";
           // var cmd = "INSERT INTO `cred_check`.`cards` (`cardNumber`, `expirationDate`, `cvv`) VALUES('" + newCard.cardNumber + "', '" + newCard.expirationDate + "', '" + newCard.cvv + "');";
            using var command = new MySqlCommand(newcmd, connection);
            int AffectedCnt = await command.ExecuteNonQueryAsync();
            serviceResponse.Data = HASHALG(_mapper.Map<card>(newCard));
            serviceResponse.Message = AffectedCnt.ToString() + " Rows Added.";
            if (AffectedCnt.ToString() == "0")
                serviceResponse.Success = false;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCardDto>> DeleteCard(string id)
        {
            ServiceResponse<GetCardDto> serviceResponse = new ServiceResponse<GetCardDto>();
            using var connection = new MySqlConnection("server=cred-check.csoyvlm0mivw.us-east-2.rds.amazonaws.com;port=3306;database=credcheck;user=removeuser;");
            await connection.OpenAsync();
            var cmd = "DELETE FROM `credcheck`.`cards` WHERE(`cardId` = '"+id+"');";
            using var command = new MySqlCommand(cmd, connection);
            int AffectedCnt = await command.ExecuteNonQueryAsync();
            serviceResponse.Message = AffectedCnt.ToString() + " Rows Affected.";
            if (AffectedCnt.ToString() == "0")
                serviceResponse.Success = false;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCardDto>> EditCard(EditCardDto EditedCard)
        {
            
            ServiceResponse<GetCardDto> serviceResponse = new ServiceResponse<GetCardDto>();
            using MySqlConnection connection = new MySqlConnection("server=cred-check.csoyvlm0mivw.us-east-2.rds.amazonaws.com;port=3306;database=credcheck;user=updateuser");
            await connection.OpenAsync();
            var cmd = "UPDATE `credcheck`.`cards` SET `expirationDate` = '"+ EditedCard.expirationDate+"',";
            cmd = cmd +" `cvv` = '"+EditedCard.cvv+ "', `cardNumber` = '" + EditedCard.cardNumber + "' WHERE(`cardId` = '" + EditedCard.cardId+"');";
            using var command = new MySqlCommand(cmd, connection);
            int AffectedCnt = await command.ExecuteNonQueryAsync();
            GetCardDto GottenCard = new GetCardDto();
            serviceResponse.Message = AffectedCnt.ToString() + " Rows Affected.";
            if (AffectedCnt.ToString() == "0")
                serviceResponse.Success = false;
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<List<GetCardDto>>> GetAllCards()
        {
            
            // using var connection = new MySqlConnection("server=localhost;port=3306;database=cred_check;user=root;password=password");
            using var connection = new MySqlConnection("server=cred-check.csoyvlm0mivw.us-east-2.rds.amazonaws.com;port=3306;database=credcheck;user=viewuser;");
            //using var connection = new MySqlConnection("Data Source=cred-check.csoyvlm0mivw.us-east-2.rds.amazonaws.com;Initial Catalog=credcheck;User ID=admin;Password=" + pass + ";");
            //using var connection =  new MySqlConnection("Data Source=cred-check.csoyvlm0mivw.us-east-2.rds.amazonaws.com,3306;User ID=admin;Password=" +pass+";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            await connection.OpenAsync();
            using var command = new MySqlCommand("SELECT * FROM cards;", connection);
            using var reader = await command.ExecuteReaderAsync();
            List<GetCardDto> GottenCards = new List<GetCardDto>();
            while (reader.Read())
            {
                GetCardDto cardrow = new GetCardDto();
                cardrow.cardNumber = reader.GetString(0);
                cardrow.expirationDate = reader.GetString(1);
                cardrow.cvv = reader.GetString(2);
                cardrow.cardId = reader.GetString(3);
                GottenCards.Add(cardrow);
                //System.Diagnostics.Debug.WriteLine(reader.GetString(0));
            }
            ServiceResponse<List<GetCardDto>> serviceResponse = new ServiceResponse<List<GetCardDto>>();
            serviceResponse.Data = GottenCards; //(cards.Select(c => _mapper.Map<GetCardDto>(c))).ToList();
            Random r = new Random();
            if (r.Next() % 100 < 50)
                serviceResponse.Message = hashof("test");
            else
                serviceResponse.Message = hashof("other");


            return serviceResponse;
        }
        public static string HASHALG(card rawcard) => hashof(rawcard.cardNumber + rawcard.expirationDate + rawcard.cvv);
        public static string hashof(string data)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<ServiceResponse<GetCardDto>> GetCardById(string id)
        {
            using var connection = new MySqlConnection("server=cred-check.csoyvlm0mivw.us-east-2.rds.amazonaws.com;port=3306;database=credcheck;user=viewuser;");

            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM cards WHERE(`cardId` = '" + id + "');", connection);
            using var reader = await command.ExecuteReaderAsync();
            GetCardDto GottenCard = new GetCardDto();
            while (reader.Read())
            {
                GottenCard.cardNumber = reader.GetString(0);
                GottenCard.expirationDate = reader.GetString(1);
                GottenCard.cvv = reader.GetString(2);
                GottenCard.cardId = null;//reader.GetString(3);
            }
            ServiceResponse<GetCardDto> serviceResponse = new ServiceResponse<GetCardDto>();
            serviceResponse.Data = GottenCard; //_mapper.Map<GetCardDto>(cards.FirstOrDefault(c => c.cardId == id));
            if (GottenCard.cardNumber == 0.ToString())
            {
                serviceResponse.Success = false;
                serviceResponse.Data = null;
                serviceResponse.Message = "SQL did not return a card, check that your card ID is correct!";
            }
            return serviceResponse;
        }
    }
}
